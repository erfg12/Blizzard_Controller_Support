using SharpDX.XInput;

namespace Blizzard_Controller;

static class Program
{
    static void Main()
    {
#if WINDOWS
        var game = new OverlayWindowMonoGame();
        game.Run();
#else
        var form = new OverlayWindow();
        form.Initialize();
#endif
    }
}
