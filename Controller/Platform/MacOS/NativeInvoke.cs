using System;
using System.Runtime.InteropServices;

namespace Blizzard_Controller.Platform.MacOS;

/// <summary>
/// macOS-specific native API calls
/// </summary>
public static class NativeInvoke
{
    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    public static extern long CFArrayGetCount(IntPtr array);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    public static extern IntPtr CFArrayGetValueAtIndex(IntPtr array, long index);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    public static extern IntPtr CFStringCreateWithCString(IntPtr alloc, string str, uint encoding);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    public static extern int CFNumberGetValue(IntPtr number, int type, out int value);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    public static extern int CFDictionaryGetValueIfPresent(IntPtr dict, IntPtr key, out IntPtr value);

    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    public static extern IntPtr CGWindowListCopyWindowInfo(uint option, uint relativeToWindow);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    public static extern void CFRelease(IntPtr cf);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    public static extern IntPtr CFPropertyListCreateData(IntPtr allocator, IntPtr propertyList, int format, int options, IntPtr errorString);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    public static extern IntPtr CFDataGetBytePtr(IntPtr data);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    public static extern long CFDataGetLength(IntPtr data);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/ApplicationServices")]
    public static extern bool AXIsProcessTrusted();

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/ApplicationServices")]
    public static extern IntPtr AXUIElementCreateSystemWide();

    [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
    public static extern IntPtr objc_getClass(string name);

    [DllImport("/usr/lib/libobjc.dylib")]
    public static extern IntPtr sel_registerName(string name);

    [DllImport("/usr/lib/libobjc.dylib")]
    public static extern void objc_msgSend(IntPtr receiver, IntPtr selector, int value);

    [StructLayout(LayoutKind.Sequential)]
    public struct CGRect
    {
        public CGPoint origin;
        public CGSize size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CGPoint { public double x, y; }

    [StructLayout(LayoutKind.Sequential)]
    public struct CGSize { public double width, height; }

    [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
    public static extern void NSWindow_setFrame(IntPtr window, CGRect frame, bool display);

    [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
    public static extern void objc_msgSend(IntPtr receiver, IntPtr selector, CGRect frame, bool display);

    [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
    public static extern void objc_msgSend_CGRect_Bool(IntPtr receiver, IntPtr selector, CGRect frame, bool display);

    // For methods returning void with int parameter
    [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
    public static extern void objc_msgSend_Int(IntPtr receiver, IntPtr selector, int value);

    [StructLayout(LayoutKind.Explicit)]
    public struct SDL_WindowType
    {
        [FieldOffset(0)] public SDL_CocoaWindow cocoa;
        // other platforms (Windows, Linux) can be ignored if targeting macOS
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_SysWMinfo
    {
        public SDL_version version;
        public SDL_WindowType window; // Platform-dependent union
        public IntPtr subsystem;      // unused here

        public SDL_CocoaWindow cocoa; // Only valid on macOS
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_version
    {
        public byte major;
        public byte minor;
        public byte patch;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_CocoaWindow
    {
        public IntPtr window; // NSWindow*
    }


    [DllImport("libSDL2-2.0.0.dylib")]
    public static extern int SDL_GetWindowWMInfo(IntPtr window, ref SDL_SysWMinfo info);

    [DllImport("libSDL2-2.0.0.dylib", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_SetWindowPosition(IntPtr window, int x, int y);
    
    [DllImport("libSDL2-2.0.0.dylib", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_SetWindowSize(IntPtr window, int w, int h);

}