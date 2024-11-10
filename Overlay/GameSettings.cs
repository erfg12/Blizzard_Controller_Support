namespace Overlay;
public class GameSettings
{
    public class StarCraft1
    {
        public static readonly int overlayWidth = 280;
        public static readonly int overlayHeight = 255;
        public static readonly int cellColumns = 4;
        public static readonly int rightOffset = 5;
        public static readonly int bottomOffset = 10;
    }
    public class StarCraft2
    {
        public static readonly int overlayWidth = 320;
        public static readonly int overlayHeight = 220;
        public static readonly int cellColumns = 6;
        public static readonly int rightOffset = 20;
        public static readonly int bottomOffset = 10;
    }
    public class WarCraft3
    {
        public static readonly int overlayWidth = 280;
        public static readonly int overlayHeight = 225;
        public static readonly int cellColumns = 5;
        public static readonly int rightOffset = 260; // only works for 1080p resolution atm, this game offsets the grid on different resolutions
        public static readonly int bottomOffset = 10;
    }
}
