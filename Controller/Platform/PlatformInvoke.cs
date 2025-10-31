namespace Blizzard_Controller.Platform;

/// <summary>
/// Platform abstraction layer for native system calls.
/// Delegates to platform-specific implementations.
/// </summary>
public static class PlatformInvoke
{
    // Common structures
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

#if !WINDOWS
    public static class SDL
    {
        [DllImport("libSDL2-2.0.0.dylib")]
    public static extern IntPtr SDL_GL_GetCurrentWindow();

    [DllImport("libSDL2-2.0.0.dylib")]
    public static extern int SDL_SetWindowBordered(IntPtr window, bool bordered);

    [DllImport("libSDL2-2.0.0.dylib")]
    public static extern int SDL_SetWindowAlwaysOnTop(IntPtr window, bool onTop);

    [DllImport("libSDL2-2.0.0.dylib")]
    public static extern int SDL_SetWindowOpacity(IntPtr window, float opacity);

    [DllImport("libSDL2-2.0.0.dylib")]
    public static extern IntPtr SDL_CreateRenderer(IntPtr window, int index, uint flags);

    [DllImport("libSDL2-2.0.0.dylib")]
    public static extern int SDL_SetRenderDrawBlendMode(IntPtr renderer, SDL_BlendMode blendMode);

    [DllImport("libSDL2-2.0.0.dylib")]
    public static extern int SDL_SetRenderDrawColor(IntPtr renderer, byte r, byte g, byte b, byte a);

    [DllImport("libSDL2-2.0.0.dylib")]
    public static extern int SDL_RenderClear(IntPtr renderer);

    [DllImport("libSDL2-2.0.0.dylib")]
    public static extern int SDL_RenderFillRect(IntPtr renderer, ref SDL_Rect rect);

    [DllImport("libSDL2-2.0.0.dylib")]
    public static extern void SDL_RenderPresent(IntPtr renderer);

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_Rect
        {
            public int x, y, w, h;
        }
    
    public const uint SDL_RENDERER_ACCELERATED = 0x00000002;
    public const uint SDL_RENDERER_PRESENTVSYNC = 0x00000004; // optional: vsync

    public enum SDL_BlendMode : uint
    {
        SDL_BLENDMODE_NONE = 0x00000000,
        SDL_BLENDMODE_BLEND = 0x00000001,
        SDL_BLENDMODE_ADD = 0x00000002,
        SDL_BLENDMODE_MOD = 0x00000004
    }
    }
#endif

    // Mouse input constants
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
        
        // Simplified click events for direct mapping
        left_click = left_BLCLK,
        right_click = right_BLCLK,
        middle_click = middle_BLCLK
    }
}