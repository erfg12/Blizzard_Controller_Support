#if WINDOWS
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XInput;
using System.Windows.Forms;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace Blizzard_Controller;
public class OverlayWindowMonoGame : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    // Overlay settings (populated from your GameSettings in the original)
    int _overlayWidth = 0;
    int _overlayHeight = 0;
    int _cellColumns = 0;
    int _sideOffset = 0;
    int _bottomOffset = 0;

    // runtime sizes
    int overlayWidth = 0;
    int overlayHeight = 0;
    int cellWidth = 0;
    int cellHeight = 0;

    // textures
    Texture2D aBtnImg, xBtnImg, yBtnImg, bBtnImg, backBtnImg;
    Texture2D lBtnImg, ltBtnImg, rBtnImg;
    Texture2D ps_xBtn, ps_squareBtn, ps_triangleBtn, ps_circleBtn, ps_shareBtn, ps_l1Btn, ps_l2Btn, ps_r1Btn;

    // helper 1x1 pixel for rectangle lines
    Texture2D pixel;

    // overlay mode
    string overlayBtns = "xbox";

    // Win32 handle
    IntPtr hWnd;

    public OverlayWindowMonoGame()
    {
        graphics = new GraphicsDeviceManager(this)
        {
            PreferMultiSampling = false,
            SynchronizeWithVerticalRetrace = true
        };
        IsMouseVisible = false;

        // create tiny window first; we'll resize/position later
        graphics.PreferredBackBufferWidth = 1;
        graphics.PreferredBackBufferHeight = 1;
        Window.ClientSizeChanged += (s, e) => { /* no-op */ };

        Window.AllowUserResizing = false;
        Window.IsBorderless = true;

        Content.RootDirectory = "Content";
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

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        var hwnd = Window.Handle;

        SetWindowExStyle(hwnd, WS_EX_LAYERED | WS_EX_TRANSPARENT);

        MakeBorderless(hwnd);

        // set magenta as transparent color key
        SetLayeredWindowAttributes(hwnd, COLOR_KEY, 0, LWA_COLORKEY);

        // Create 1x1 white pixel
        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });

        // Load textures from Resources folder (png)
        aBtnImg = LoadTexture("Resources/a_btn.png");
        xBtnImg = LoadTexture("Resources/x_btn.png");
        yBtnImg = LoadTexture("Resources/y_btn.png");
        bBtnImg = LoadTexture("Resources/b_btn.png");
        backBtnImg = LoadTexture("Resources/back_btn.png");
        lBtnImg = LoadTexture("Resources/lb_btn.png");
        ltBtnImg = LoadTexture("Resources/lt_btn.png");
        rBtnImg = LoadTexture("Resources/rb_btn.png");

        ps_xBtn = LoadTexture("Resources/ps_x_btn.png");
        ps_squareBtn = LoadTexture("Resources/ps_square_btn.png");
        ps_triangleBtn = LoadTexture("Resources/ps_triangle_btn.png");
        ps_circleBtn = LoadTexture("Resources/ps_circle_btn.png");
        ps_shareBtn = LoadTexture("Resources/ps_share_btn.png");
        ps_l1Btn = LoadTexture("Resources/ps_l1_btn.png");
        ps_l2Btn = LoadTexture("Resources/ps_l2_btn.png");
        ps_r1Btn = LoadTexture("Resources/ps_r1_btn.png");
    }

    protected override void UnloadContent()
    {
        pixel?.Dispose();
        aBtnImg?.Dispose();
        xBtnImg?.Dispose();
        yBtnImg?.Dispose();
        bBtnImg?.Dispose();
        backBtnImg?.Dispose();
        lBtnImg?.Dispose();
        ltBtnImg?.Dispose();
        rBtnImg?.Dispose();
        ps_xBtn?.Dispose();
        ps_squareBtn?.Dispose();
        ps_triangleBtn?.Dispose();
        ps_circleBtn?.Dispose();
        ps_shareBtn?.Dispose();
        ps_l1Btn?.Dispose();
        ps_l2Btn?.Dispose();
        ps_r1Btn?.Dispose();
        base.UnloadContent();
    }

    int check = 0;
    Invoke.RECT gameWindowSize = new Invoke.RECT();

    protected override void Update(GameTime gameTime)
    {
        AppSettings.Instance.UpdateControllerStatus(GamePad.GetState(0).IsConnected);

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            Exit();

        // Poll every ~100 frames ticks (approx matches original check >= 100)
        if (check++ >= 100)
        {
            // detect game processes (use your GameSettings.* names)
            if (OverlayWindow.SC2Proc != null || OverlayWindow.SC1Proc != null || OverlayWindow.WC3Proc != null || OverlayWindow.WC1Proc != null || OverlayWindow.WC2Proc != null)
            {
                if (OverlayWindow.SC2Proc != null)
                {
                    gameWindowSize = GetWindowSize(OverlayWindow.SC2Proc);
                    _overlayWidth = GameSettings.StarCraft2.overlayWidth;
                    _overlayHeight = GameSettings.StarCraft2.overlayHeight;
                    _cellColumns = GameSettings.StarCraft2.cellColumns;
                    _bottomOffset = GameSettings.StarCraft2.bottomOffset;
                    _sideOffset = GameSettings.StarCraft2.sideOffset;
                }
                else if (OverlayWindow.SC1Proc != null)
                {
                    gameWindowSize = GetWindowSize(OverlayWindow.SC1Proc);
                    _overlayWidth = GameSettings.StarCraft1.overlayWidth;
                    _overlayHeight = GameSettings.StarCraft1.overlayHeight;
                    _cellColumns = GameSettings.StarCraft1.cellColumns;
                    _bottomOffset = GameSettings.StarCraft1.bottomOffset;
                    _sideOffset = GameSettings.StarCraft1.sideOffset;
                }
                else if (OverlayWindow.WC3Proc != null)
                {
                    gameWindowSize = GetWindowSize(OverlayWindow.WC3Proc);
                    _overlayWidth = GameSettings.WarCraft3.overlayWidth;
                    _overlayHeight = GameSettings.WarCraft3.overlayHeight;
                    _cellColumns = GameSettings.WarCraft3.cellColumns;
                    _bottomOffset = GameSettings.WarCraft3.bottomOffset;
                    _sideOffset = GameSettings.WarCraft3.sideOffset;
                }
                else if (OverlayWindow.WC1Proc != null)
                {
                    gameWindowSize = GetWindowSize(OverlayWindow.WC1Proc);
                    _overlayWidth = GameSettings.WarCraft1.overlayWidth;
                    _overlayHeight = GameSettings.WarCraft1.overlayHeight;
                    _cellColumns = GameSettings.WarCraft1.cellColumns;
                    _bottomOffset = GameSettings.WarCraft1.bottomOffset;
                    _sideOffset = GameSettings.WarCraft1.sideOffset;
                }
                else if (OverlayWindow.WC2Proc != null)
                {
                    gameWindowSize = GetWindowSize(OverlayWindow.WC2Proc);
                    _overlayWidth = GameSettings.WarCraft2.overlayWidth;
                    _overlayHeight = GameSettings.WarCraft2.overlayHeight;
                    _cellColumns = GameSettings.WarCraft2.cellColumns;
                    _bottomOffset = GameSettings.WarCraft2.bottomOffset;
                    _sideOffset = GameSettings.WarCraft2.sideOffset;
                }

                var gameWidth = Math.Abs(gameWindowSize.Left - gameWindowSize.Right);
                var gameHeight = Math.Abs(gameWindowSize.Top - gameWindowSize.Bottom);

                float scaleX = gameWidth / 1280f;
                float scaleY = gameHeight / 720f;

                overlayWidth = (int)(_overlayWidth * scaleX);
                overlayHeight = (int)(_overlayHeight * scaleY);

                cellWidth = Math.Max(1, overlayWidth / Math.Max(1, _cellColumns));
                cellHeight = Math.Max(1, overlayHeight / 4);

                // resize textures by recreating scaled versions: MonoGame can't set width/height on Texture2D directly.
                // Simpler: draw them scaled during Draw using dest rectangles computed from cellWidth/cellHeight.

                // WC3 special aspect offsets
                if (OverlayWindow.WC3Proc != null)
                {
                    var test = GetAspectRatio(gameWidth, gameHeight);
                    if (test == 1.8) _sideOffset = Convert.ToInt32(0.135 * gameWidth);
                    else if (test == 1.6) _sideOffset = Convert.ToInt32(0.095 * gameWidth);
                    else if (test == 1.5) _sideOffset = Convert.ToInt32(0.080 * gameWidth);
                }

                int targetWidth = overlayWidth;
                int targetHeight = overlayHeight;

                int posX;
                int posY;

                if (OverlayWindow.WC1Proc != null || OverlayWindow.WC2Proc != null) // left side
                {
                    posX = gameWindowSize.Left - _sideOffset;
                    posY = gameWindowSize.Bottom - targetHeight - _bottomOffset;
                }
                else // right side
                {
                    posX = gameWindowSize.Right - targetWidth - _sideOffset;
                    posY = gameWindowSize.Bottom - targetHeight - _bottomOffset;
                }

                // Apply window size & position
                SetWindowPositionAndSize(posX, posY, targetWidth, targetHeight);
            }

            check = 0;
        }

        // Handle controller input processes each update
        ControllerInputs.processButtons();
        ControllerInputs.processJoysticks();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(0,0,0));

        // If no game found, skip drawing
        if ((OverlayWindow.SC2Proc == null && OverlayWindow.SC1Proc == null && OverlayWindow.WC3Proc == null && OverlayWindow.WC1Proc == null && OverlayWindow.WC2Proc == null) ||
             Math.Abs(gameWindowSize.Left - gameWindowSize.Right) == 0)
        {
            base.Draw(gameTime);
            return;
        }

        bool leftSide = OverlayWindow.WC1Proc != null || OverlayWindow.WC2Proc != null;
        var pad = GamePad.GetState(PlayerIndex.One);
        bool anyTrigger = pad.Triggers.Left > 0.5f || pad.Triggers.Right > 0.5f || pad.IsButtonDown(Buttons.LeftTrigger);

        if (anyTrigger)
        {
            // draw semi-transparent button images when trigger down
            float alpha = 0.6f;
            Microsoft.Xna.Framework.Color tint = Microsoft.Xna.Framework.Color.White * alpha;

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);

            // Top row
            var btnList = new Texture2D[] { aBtnImg, xBtnImg, yBtnImg, bBtnImg, backBtnImg };
            var psList = new Texture2D[] { ps_xBtn, ps_squareBtn, ps_triangleBtn, ps_circleBtn, ps_shareBtn };

            int c = _cellColumns - (leftSide ? 0 : 1);
            for (int i = 0; i < _cellColumns - 1; i++)
            {
                int dstX = overlayWidth - cellWidth * c--;
                int dstY = 0;
                var srcTex = overlayBtns.Equals("Playstation") ? psList[i] : btnList[i];
                if (srcTex != null)
                    spriteBatch.Draw(srcTex, new Microsoft.Xna.Framework.Rectangle(dstX, dstY, cellWidth, cellHeight), tint);
            }

            // side buttons
            Texture2D rTex = overlayBtns.Equals("Playstation") ? ps_r1Btn : rBtnImg;
            Texture2D lTex = overlayBtns.Equals("Playstation") ? ps_l1Btn : lBtnImg;
            Texture2D ltTex = overlayBtns.Equals("Playstation") ? ps_l2Btn : ltBtnImg;

            int sideX = leftSide ? gameWindowSize.Left + cellWidth * (_cellColumns - 1) : 0;

            if (rTex != null)
                spriteBatch.Draw(rTex, new Microsoft.Xna.Framework.Rectangle(sideX, overlayHeight - (cellHeight * 3), cellWidth, cellHeight), tint);
            if (lTex != null)
                spriteBatch.Draw(lTex, new Microsoft.Xna.Framework.Rectangle(sideX, overlayHeight - (cellHeight * 2), cellWidth, cellHeight), tint);
            if (OverlayWindow.WC1Proc == null && ltTex != null)
                spriteBatch.Draw(ltTex, new Microsoft.Xna.Framework.Rectangle(sideX, overlayHeight - cellHeight, cellWidth, cellHeight), tint);

            // row highlighting (green outlines) for triggers
            if (pad.Triggers.Right > 0.5f)
                DrawRectangleLines(spriteBatch, new Microsoft.Xna.Framework.Rectangle(cellWidth, overlayHeight - cellHeight * 3 - 1, overlayWidth - cellWidth, cellHeight), 2, Microsoft.Xna.Framework.Color.Green);
            if (pad.Triggers.Left > 0.5f)
                DrawRectangleLines(spriteBatch, new Microsoft.Xna.Framework.Rectangle(cellWidth, overlayHeight - cellHeight * 2 - 1, overlayWidth - cellWidth, cellHeight), 2, Microsoft.Xna.Framework.Color.Green);
            if (pad.IsButtonDown(Buttons.LeftShoulder) && OverlayWindow.WC1Proc == null) // approximate LeftTrigger2
                DrawRectangleLines(spriteBatch, new Microsoft.Xna.Framework.Rectangle(cellWidth, overlayHeight - cellHeight - 1, overlayWidth - cellWidth, cellHeight), 2, Microsoft.Xna.Framework.Color.Green);

            spriteBatch.End();
        }

        base.Draw(gameTime);
    }

    // --- helpers ---

    Texture2D LoadTexture(string path)
    {
        if (!File.Exists(path)) return null;
        using var fs = File.OpenRead(path);
        return Texture2D.FromStream(GraphicsDevice, fs);
    }

    void DrawRectangleLines(SpriteBatch sb, Microsoft.Xna.Framework.Rectangle rect, int thickness, Microsoft.Xna.Framework.Color color)
    {
        // top
        sb.Draw(pixel, new Microsoft.Xna.Framework.Rectangle(rect.Left, rect.Top, rect.Width, thickness), color);
        // bottom
        sb.Draw(pixel, new Microsoft.Xna.Framework.Rectangle(rect.Left, rect.Bottom - thickness, rect.Width, thickness), color);
        // left
        sb.Draw(pixel, new Microsoft.Xna.Framework.Rectangle(rect.Left, rect.Top, thickness, rect.Height), color);
        // right
        sb.Draw(pixel, new Microsoft.Xna.Framework.Rectangle(rect.Right - thickness, rect.Top, thickness, rect.Height), color);
    }

    double GetAspectRatio(int width, int height)
    {
        var roundThis = (double)width / height;
        return Math.Round(roundThis, 1);
    }

    public static Process GetProcess(string procName)
    {
        return Process.GetProcessesByName(procName).FirstOrDefault();
    }

    // Uses your existing Invoke.GetWindowRect
    public Invoke.RECT GetWindowSize(Process gameProc)
    {
        Invoke.RECT gameWindowSize = new Invoke.RECT();
        Invoke.GetWindowRect(gameProc.MainWindowHandle, out gameWindowSize);
        return gameWindowSize;
    }

    // Window positioning/resizing helpers (Win32)
    void SetWindowPositionAndSize(int x, int y, int width, int height)
    {
        if (hWnd == IntPtr.Zero) hWnd = WindowHandle();
        if (hWnd == IntPtr.Zero) return;

        // set client size via MoveWindow
        MoveWindow(hWnd, x, y, width, height, true);

        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
    }

    // Try to obtain Window handle by calling GetActiveWindow as fallback
    IntPtr WindowHandle()
    {
        return GetActiveWindow();
    }

    // --- Win32 interop constants & functions ---
    const int GWL_EXSTYLE = -20;
    const int WS_EX_LAYERED = 0x00080000;
    const int WS_EX_TRANSPARENT = 0x00000020;
    const int WS_EX_TOPMOST = 0x00000008;
    const int LWA_ALPHA = 0x02;
    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    const uint SWP_NOMOVE = 0x0002;
    const uint SWP_NOSIZE = 0x0001;
    const uint SWP_SHOWWINDOW = 0x0040;
    public const int LWA_COLORKEY = 0x1;
    const int GWL_STYLE = -16;
    const uint WS_POPUP = 0x80000000;
    const uint COLOR_KEY = 0x00000000; // magenta in 0x00BBGGRR

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    static extern int GetWindowLong32(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);
    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    // end Win32
}
#endif