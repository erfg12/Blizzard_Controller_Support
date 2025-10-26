using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Blizzard_Controller.Platform.Windows;

/// <summary>
/// Windows-specific native API calls
/// </summary>
public static class NativeInvoke
{
    public const int GWL_STYLE = -16;
    public const int WS_OVERLAPPEDWINDOW = 0x00CF0000;

    [DllImport("User32.Dll")]
    public static extern long SetCursorPos(int x, int y);
    
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(ref Point lpPoint);
    
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    
    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowRect(nint hwnd, out PlatformInvoke.RECT lpRect);
}