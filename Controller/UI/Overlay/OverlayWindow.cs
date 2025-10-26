using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Blizzard_Controller.UI.Overlay;

/// <summary>
/// Base overlay window functionality shared across all platforms
/// </summary>
public class OverlayWindow
{
    // keep the same public static fields so existing code compiles
    public static Process[] pname = null;
    public static Process SC2Proc, SC1Proc, WC3Proc, WC1Proc, WC2Proc;
    public static string overlayBtns = "xbox";

    // Helper to calculate aspect ratio (kept from previous implementation)
    public static double GetAspectRatio(int width, int height)
    {
        var roundThis = (double)width / height;
        return Math.Round(roundThis, 1);
    }

    // Process lookup (platform-aware)
    public static Process GetProcess(string procName)
    {
#if LINUX
        foreach (var process in Process.GetProcesses())
        {
            string cmdlinePath = $"/proc/{process.Id}/cmdline";
            try
            {
                if (File.Exists(cmdlinePath))
                {
                    string cmdline = File.ReadAllText(cmdlinePath).Replace('\0', ' ');
                    if (cmdline.Contains(procName, StringComparison.OrdinalIgnoreCase))
                    {
                        return process;
                    }
                }
            }
            catch { }
        }
        return null;
#else
        return Process.GetProcessesByName(procName).FirstOrDefault();
#endif
    }

    // Keep a GetWindowSize helper that uses Invoke.PInvoke functions. Overlay
    // implementations (MonoGame) can call this directly.
    public Platform.PlatformInvoke.RECT GetWindowSize(Process gameProc)
    {
#if WINDOWS
        Platform.PlatformInvoke.RECT gameWindowSize = new Platform.PlatformInvoke.RECT();
        Platform.Windows.NativeInvoke.GetWindowRect(gameProc.MainWindowHandle, out gameWindowSize);
        return gameWindowSize;
#elif MACOS
        int processId = gameProc.Id;
        IntPtr array = Platform.MacOS.NativeInvoke.CGWindowListCopyWindowInfo(1, 0);
        if (array == IntPtr.Zero) return default;
        long count = Platform.MacOS.NativeInvoke.CFArrayGetCount(array);
        IntPtr pidKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "kCGWindowOwnerPID", 0x0600);
        IntPtr boundsKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "kCGWindowBounds", 0x0600);
        IntPtr layerKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "kCGWindowLayer", 0x0600);

        Platform.PlatformInvoke.RECT rect = default;
        for (long i = 0; i < count; i++)
        {
            IntPtr dict = Platform.MacOS.NativeInvoke.CFArrayGetValueAtIndex(array, i);
            if (Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(dict, pidKey, out IntPtr pidValue) != 0)
            {
                Platform.MacOS.NativeInvoke.CFNumberGetValue(pidValue, 9, out int pid);
                if (pid == processId)
                {
                    Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(dict, layerKey, out IntPtr layerVal);
                    Platform.MacOS.NativeInvoke.CFNumberGetValue(layerVal, 9, out int layer);
                    if (layer != 0) continue;
                    Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(dict, boundsKey, out IntPtr boundsDict);
                    IntPtr xKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "X", 0x0600);
                    IntPtr yKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "Y", 0x0600);
                    IntPtr wKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "Width", 0x0600);
                    IntPtr hKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "Height", 0x0600);

                    Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(boundsDict, xKey, out IntPtr xVal);
                    Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(boundsDict, yKey, out IntPtr yVal);
                    Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(boundsDict, wKey, out IntPtr wVal);
                    Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(boundsDict, hKey, out IntPtr hVal);

                    Platform.MacOS.NativeInvoke.CFNumberGetValue(xVal, 9, out int x);
                    Platform.MacOS.NativeInvoke.CFNumberGetValue(yVal, 9, out int y);
                    Platform.MacOS.NativeInvoke.CFNumberGetValue(wVal, 9, out int w);
                    Platform.MacOS.NativeInvoke.CFNumberGetValue(hVal, 9, out int h);

                    rect.Left = x;
                    rect.Top = y;
                    rect.Right = x + w;
                    rect.Bottom = y + h;
                    break;
                }
            }
        }
        Platform.MacOS.NativeInvoke.CFRelease(array);
        return rect;
#else
        return default;
#endif
    }
}