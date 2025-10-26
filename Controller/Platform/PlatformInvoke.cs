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