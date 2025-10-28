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
    static extern IntPtr objc_getClass(string name);

    [DllImport("/usr/lib/libobjc.dylib")]
    static extern IntPtr sel_registerName(string name);

    [DllImport("/usr/lib/libobjc.dylib")]
    static extern void objc_msgSend(IntPtr receiver, IntPtr selector, int value);

    [StructLayout(LayoutKind.Sequential)]
    struct CGRect
    {
        public CGPoint origin;
        public CGSize size;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct CGPoint { public double x, y; }

    [StructLayout(LayoutKind.Sequential)]
    struct CGSize { public double width, height; }

}