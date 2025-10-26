using System;
using System.Runtime.InteropServices;

namespace Blizzard_Controller.Platform.Linux;

/// <summary>
/// Linux-specific native API calls (X11)
/// </summary>
public static class NativeInvoke
{
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
    public static extern int XTranslateCoordinates(
        IntPtr display, 
        IntPtr src_w, 
        IntPtr dest_w,
        int src_x, 
        int src_y, 
        out int dest_x, 
        out int dest_y, 
        out IntPtr child);

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
}