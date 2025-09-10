#if WINDOWS
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blizzard_Controller;
public class OverlayWindowMonoGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public OverlayWindowMonoGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        IsMouseVisible = false; // no cursor
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        var hwnd = Window.Handle;

        // make window layered + click-through
        SetWindowExStyle(hwnd, WS_EX_LAYERED | WS_EX_TRANSPARENT);

        MakeBorderless(hwnd);

        // set magenta as transparent color key
        SetLayeredWindowAttributes(hwnd, COLOR_KEY, 0, LWA_COLORKEY);

        var form = (Form)Control.FromHandle(Window.Handle);
        form.TopMost = true; // <-- keeps window always on top
    }

    private void MakeBorderless(IntPtr hwnd)
    {
        if (IntPtr.Size == 8)
        {
            var style = GetWindowLongPtr64(hwnd, GWL_STYLE).ToInt64();
            // clear existing style and set WS_POPUP
            SetWindowLongPtr64(hwnd, GWL_STYLE, new IntPtr(WS_POPUP));
        }
        else
        {
            var style = GetWindowLong32(hwnd, GWL_STYLE);
            SetWindowLong32(hwnd, GWL_STYLE, unchecked((int)WS_POPUP));
        }
    }


    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // clear with the colorkey color (magenta)
        GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(255, 0, 255));

        _spriteBatch.Begin();
        //_spriteBatch.Draw(GetDummyTexture(Color.White), new Microsoft.Xna.Framework.Rectangle(100, 100, 200, 200), Microsoft.Xna.Framework.Color.White * 0.7f);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    #region Win32 Interop

    const int GWL_EXSTYLE = -20;
    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;
    const uint LWA_COLORKEY = 0x00000001;
    const uint COLOR_KEY = 0x00FF00FF; // magenta in 0x00BBGGRR
    const int GWL_STYLE = -16;
    const uint WS_POPUP = 0x80000000;


    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    static extern int GetWindowLong32(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);
    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    private IntPtr SetWindowExStyle(IntPtr hwnd, uint addFlags)
    {
        if (IntPtr.Size == 8)
        {
            var cur = GetWindowLongPtr64(hwnd, GWL_EXSTYLE);
            var newv = new IntPtr(cur.ToInt64() | addFlags);
            return SetWindowLongPtr64(hwnd, GWL_EXSTYLE, newv);
        }
        else
        {
            int cur = GetWindowLong32(hwnd, GWL_EXSTYLE);
            return new IntPtr(SetWindowLong32(hwnd, GWL_EXSTYLE, cur | (int)addFlags));
        }
    }

    #endregion
}
#endif