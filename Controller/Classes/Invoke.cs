namespace Blizzard_Controller;

public class Invoke
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    // https://docs.microsoft.com/en-us/windows/win32/inputdev/mouse-input-notifications
    public enum MouseClicks
    {
        left_down = 0x0201,
        left_up = 0x0202,
        left_BLCLK = 0x0203,
        right_down = 0x0204,
        right_up = 0x0205,
        right_BLCLK = 0x0206,
        middle_down = 0x0207,
        middle_up = 0x0208,
        middle_BLCLK = 0x0209,
    }

#if WINDOWS
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
    public static extern bool GetWindowRect(nint hwnd, out RECT lpRect);
#elif LINUX
    // XWindowAttributes struct
    [StructLayout(LayoutKind.Sequential)]
    public struct XWindowAttributes
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public int border_width;
        public int depth;
        public IntPtr visual;
        public IntPtr root;
        public int class_;
        public int bit_gravity;
        public int win_gravity;
        public int backing_store;
        public ulong backing_planes;
        public ulong backing_pixel;
        public bool save_under;
        public IntPtr colormap;
        public bool map_installed;
        public int map_state;
        public long all_event_masks;
        public long your_event_mask;
        public long do_not_propagate_mask;
        public bool override_redirect;
        public IntPtr screen;
    }

    // =====================
    // Core X11 Functions
    // =====================

    [DllImport("libX11")]
    public static extern IntPtr XOpenDisplay(IntPtr display);

    [DllImport("libX11")]
    public static extern int XCloseDisplay(IntPtr display);

    [DllImport("libX11")]
    public static extern IntPtr XDefaultRootWindow(IntPtr display);

    [DllImport("libX11")]
    public static extern int XQueryTree(
        IntPtr display,
        IntPtr window,
        out IntPtr root_return,
        out IntPtr parent_return,
        out IntPtr children_return,
        out uint nchildren_return);

    [DllImport("libX11")]
    public static extern int XFree(IntPtr data);

    [DllImport("libX11")]
    public static extern int XGetWindowAttributes(
        IntPtr display,
        IntPtr window,
        out XWindowAttributes attributes);

    [DllImport("libX11")]
    public static extern int XGetWindowProperty(
        IntPtr display,
        IntPtr window,
        IntPtr property,
        IntPtr long_offset,
        IntPtr long_length,
        bool delete,
        ulong req_type,
        out IntPtr actual_type_return,
        out int actual_format_return,
        out ulong nitems_return,
        out ulong bytes_after_return,
        out IntPtr prop_return);

    [DllImport("libX11")]
    public static extern IntPtr XInternAtom(
        IntPtr display,
        string atom_name,
        bool only_if_exists);
#elif MACOS
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
#endif

}