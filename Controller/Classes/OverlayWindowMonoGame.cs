
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

using Blizzard_Controller.Configuration;
using Color = Microsoft.Xna.Framework.Color;
using System.Reflection;
using static Blizzard_Controller.Platform.PlatformInvoke;

#if WINDOWS
using static Blizzard_Controller.Platform.Windows.NativeInvoke;
#elif MACOS
using static Blizzard_Controller.Platform.MacOS.NativeInvoke;
#elif LINUX
using static Blizzard_Controller.Platform.Linux.NativeInvoke;
#endif

namespace Blizzard_Controller;

public class OverlayWindowMonoGame : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch? spriteBatch;
    bool anyTrigger = false;
    int posX = 0;
    int posY = 0;
    int targetWidth = 0;
    int targetHeight = 0;

    // Processes
    Process? SC2Proc, SC1Proc, WC3Proc, WC1Proc, WC2Proc;

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
    Texture2D? aBtnImg, xBtnImg, yBtnImg, bBtnImg, backBtnImg;
    Texture2D? lBtnImg, ltBtnImg, rBtnImg;
    Texture2D? ps_xBtn, ps_squareBtn, ps_triangleBtn, ps_circleBtn, ps_shareBtn, ps_l1Btn, ps_l2Btn, ps_r1Btn;

    // helper 1x1 pixel for rectangle lines
    Texture2D? pixel;

    // overlay mode
    string overlayBtns = "xbox";

    IntPtr hWnd;

    public OverlayWindowMonoGame()
    {
        graphics = new GraphicsDeviceManager(this)
        {
            PreferMultiSampling = false,
            SynchronizeWithVerticalRetrace = true,
            PreferredBackBufferWidth = 1,
            PreferredBackBufferHeight = 1
        };
        graphics.ApplyChanges();
        IsMouseVisible = false;

        // create tiny window first; we'll resize/position later
        graphics.PreferredBackBufferWidth = 1;
        graphics.PreferredBackBufferHeight = 1;

        Window.AllowUserResizing = false;
        Window.IsBorderless = true;

        Content.RootDirectory = "Content";
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        if (spriteBatch == null)
        {
            throw new InvalidOperationException("Failed to create SpriteBatch");
        }

        var hwnd = Window.Handle;

#if WINDOWS
        SetWindowExStyle(hwnd, WS_EX_LAYERED | WS_EX_TRANSPARENT);
        MakeBorderless(hwnd);
        SetLayeredWindowAttributes(hwnd, COLOR_KEY, 0, LWA_COLORKEY);
#elif MACOS
        IntPtr window = SDL.SDL_GL_GetCurrentWindow();
        SDL.SDL_SetWindowBordered(window, false);
        SDL.SDL_SetWindowAlwaysOnTop(window, true);
        SDL.SDL_SetWindowOpacity(window, 0.5f); // fully opaque for rendering
#endif

        // Create 1x1 white pixel
        pixel = new Texture2D(GraphicsDevice, 1, 1);
        if (pixel != null)
            pixel.SetData(new[] { Color.White });

        // For MacOS app bundle support, check both paths
        string path = Path.Combine(AppContext.BaseDirectory, "../Resources/ui/resources/buttonimages/");
        if (!Directory.Exists(path))
        {
            path = Path.Combine(AppContext.BaseDirectory, "UI/Resources/ButtonImages/");
        }

        // Load textures from UI/Resources/ButtonImages folder (png)
        aBtnImg = LoadTexture(Path.Combine(path, "a_btn.png"));
        xBtnImg = LoadTexture(Path.Combine(path, "x_btn.png"));
        yBtnImg = LoadTexture(Path.Combine(path, "y_btn.png"));
        bBtnImg = LoadTexture(Path.Combine(path, "b_btn.png"));
        backBtnImg = LoadTexture(Path.Combine(path, "back_btn.png"));
        lBtnImg = LoadTexture(Path.Combine(path, "lb_btn.png"));
        ltBtnImg = LoadTexture(Path.Combine(path, "lt_btn.png"));
        rBtnImg = LoadTexture(Path.Combine(path, "rb_btn.png"));

        ps_xBtn = LoadTexture(Path.Combine(path, "ps_x_btn.png"));
        ps_squareBtn = LoadTexture(Path.Combine(path, "ps_square_btn.png"));
        ps_triangleBtn = LoadTexture(Path.Combine(path, "ps_triangle_btn.png"));
        ps_circleBtn = LoadTexture(Path.Combine(path, "ps_circle_btn.png"));
        ps_shareBtn = LoadTexture(Path.Combine(path, "ps_share_btn.png"));
        ps_l1Btn = LoadTexture(Path.Combine(path, "ps_l1_btn.png"));
        ps_l2Btn = LoadTexture(Path.Combine(path, "ps_l2_btn.png"));
        ps_r1Btn = LoadTexture(Path.Combine(path, "ps_r1_btn.png"));
    }

    private void MakeBorderless(IntPtr hwnd)
    {
#if WINDOWS
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
#endif
    }

    private IntPtr SetWindowExStyle(IntPtr hwnd, uint addFlags)
    {
#if WINDOWS
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
#else
        return IntPtr.Zero;
#endif
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
    Platform.PlatformInvoke.RECT gameWindowSize = new Platform.PlatformInvoke.RECT();

    protected override void Update(GameTime gameTime)
    {
        // Poll every ~60 frames ticks (approx matches original check >= 60)
        if (check++ >= 60)
        {
            // detect game processes (use your GameSettings.* names)
            SC2Proc = GetProcess(GameSettings.ProcessNames.SC2ProcName);
            SC1Proc = GetProcess(GameSettings.ProcessNames.SC1ProcName);
            WC3Proc = GetProcess(GameSettings.ProcessNames.WC3ProcName);
            WC2Proc = GetProcess(GameSettings.ProcessNames.WC2ProcName);
            WC1Proc = GetProcess(GameSettings.ProcessNames.WC1ProcName);

            if (SC2Proc != null || SC1Proc != null || WC3Proc != null || WC1Proc != null || WC2Proc != null)
            {
                if (SC2Proc != null)
                {
                    gameWindowSize = GetWindowSize(SC2Proc);
                    _overlayWidth = GameSettings.StarCraft2.overlayWidth;
                    _overlayHeight = GameSettings.StarCraft2.overlayHeight;
                    _cellColumns = GameSettings.StarCraft2.cellColumns;
                    _bottomOffset = GameSettings.StarCraft2.bottomOffset;
                    _sideOffset = GameSettings.StarCraft2.sideOffset;
                }
                else if (SC1Proc != null)
                {
                    gameWindowSize = GetWindowSize(SC1Proc);
                    _overlayWidth = GameSettings.StarCraft1.overlayWidth;
                    _overlayHeight = GameSettings.StarCraft1.overlayHeight;
                    _cellColumns = GameSettings.StarCraft1.cellColumns;
                    _bottomOffset = GameSettings.StarCraft1.bottomOffset;
                    _sideOffset = GameSettings.StarCraft1.sideOffset;
                }
                else if (WC3Proc != null)
                {
                    gameWindowSize = GetWindowSize(WC3Proc);
                    _overlayWidth = GameSettings.WarCraft3.overlayWidth;
                    _overlayHeight = GameSettings.WarCraft3.overlayHeight;
                    _cellColumns = GameSettings.WarCraft3.cellColumns;
                    _bottomOffset = GameSettings.WarCraft3.bottomOffset;
                    _sideOffset = GameSettings.WarCraft3.sideOffset;
                }
                else if (WC1Proc != null)
                {
                    gameWindowSize = GetWindowSize(WC1Proc);
                    _overlayWidth = GameSettings.WarCraft1.overlayWidth;
                    _overlayHeight = GameSettings.WarCraft1.overlayHeight;
                    _cellColumns = GameSettings.WarCraft1.cellColumns;
                    _bottomOffset = GameSettings.WarCraft1.bottomOffset;
                    _sideOffset = GameSettings.WarCraft1.sideOffset;
                }
                else if (WC2Proc != null)
                {
                    gameWindowSize = GetWindowSize(WC2Proc);
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

                //Console.WriteLine("Overlay w:" + _overlayWidth + " Game window size: " + gameWidth + "x" + gameHeight + ", scale: " + scaleX + "x" + scaleY);

                overlayWidth = (int)(_overlayWidth * scaleX);
                overlayHeight = (int)(_overlayHeight * scaleY);

                cellWidth = Math.Max(1, overlayWidth / Math.Max(1, _cellColumns));
                cellHeight = Math.Max(1, overlayHeight / 4);

                // WC3 special aspect offsets
                if (WC3Proc != null)
                {
                    var test = GetAspectRatio(gameWidth, gameHeight);
                    if (test == 1.8) _sideOffset = Convert.ToInt32(0.135 * gameWidth);
                    else if (test == 1.6) _sideOffset = Convert.ToInt32(0.095 * gameWidth);
                    else if (test == 1.5) _sideOffset = Convert.ToInt32(0.080 * gameWidth);
                }

                targetWidth = overlayWidth;
                targetHeight = overlayHeight;

                if (WC1Proc != null || WC2Proc != null) // left side
                {
                    posX = gameWindowSize.Left - _sideOffset;
                    posY = gameWindowSize.Bottom - targetHeight - _bottomOffset;
                }
                else // right side
                {
                    posX = gameWindowSize.Right - targetWidth - _sideOffset;
                    posY = gameWindowSize.Bottom - targetHeight - _bottomOffset;
                }
            }
#if WINDOWS
            SetWindowPositionAndSize(posX, posY, targetWidth, targetHeight);
#endif
            check = 0;
        }

#if !WINDOWS
        SetWindowPositionAndSize(posX, posY, anyTrigger ? targetWidth : 1, anyTrigger ? targetHeight : 1);
#endif

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(0, 0, 0));
        //GraphicsDevice.Clear(Color.Transparent);

        // If no game found, skip drawing
        if ((SC2Proc == null && SC1Proc == null && WC3Proc == null && WC1Proc == null && WC2Proc == null) ||
             Math.Abs(gameWindowSize.Left - gameWindowSize.Right) == 0)
        {
            base.Draw(gameTime);
            return;
        }

        bool leftSide = WC1Proc != null || WC2Proc != null;
        var pad = GamePad.GetState(PlayerIndex.One);
        anyTrigger = pad.Triggers.Left > 0.5f || pad.IsButtonDown(Buttons.RightShoulder) || pad.IsButtonDown(Buttons.LeftShoulder);

        if (anyTrigger)
        {
            // draw semi-transparent button images when trigger down
            float alpha = 0.6f;
            Microsoft.Xna.Framework.Color tint = Microsoft.Xna.Framework.Color.White * alpha;

            if (spriteBatch != null)
                spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);

            // Top row
            var btnList = new Texture2D?[] { aBtnImg, xBtnImg, yBtnImg, bBtnImg, backBtnImg };
            var psList = new Texture2D?[] { ps_xBtn, ps_squareBtn, ps_triangleBtn, ps_circleBtn, ps_shareBtn };

            int c = _cellColumns - (leftSide ? 0 : 1);
            for (int i = 0; i < _cellColumns - 1; i++)
            {
                //Console.WriteLine("Overlay button " + i + " overlayWidth:" + overlayWidth + ", cellWidth:" + cellWidth);
                int dstX = overlayWidth - cellWidth * c--;
                int dstY = 0;
                var srcTex = overlayBtns.Equals("Playstation") ? psList[i] : btnList[i];
                if (srcTex != null)
                {
                    //spriteBatch.Draw(srcTex, new Microsoft.Xna.Framework.Rectangle(dstX, dstY, cellWidth, cellHeight), null, tint);
                    spriteBatch.Draw(srcTex, new Microsoft.Xna.Framework.Rectangle(dstX, dstY, cellWidth, cellHeight), tint);
                    //Console.WriteLine("Drew button index " + i + " at " + dstX + "," + dstY);
                }
                else
                {
                    Console.WriteLine("Missing texture for button index " + i);
                }
            }

            // side buttons
            Texture2D? rTex = overlayBtns.Equals("Playstation") ? ps_r1Btn : rBtnImg;
            Texture2D? lTex = overlayBtns.Equals("Playstation") ? ps_l1Btn : lBtnImg;
            Texture2D? ltTex = overlayBtns.Equals("Playstation") ? ps_l2Btn : ltBtnImg;

            int sideX = leftSide ? gameWindowSize.Left + cellWidth * (_cellColumns - 1) : 0;

            if (rTex != null)
                spriteBatch.Draw(rTex, new Microsoft.Xna.Framework.Rectangle(sideX, overlayHeight - (cellHeight * 3), cellWidth, cellHeight), tint);
            if (lTex != null)
                spriteBatch.Draw(lTex, new Microsoft.Xna.Framework.Rectangle(sideX, overlayHeight - (cellHeight * 2), cellWidth, cellHeight), tint);
            if (WC1Proc == null && ltTex != null)
                spriteBatch.Draw(ltTex, new Microsoft.Xna.Framework.Rectangle(sideX, overlayHeight - cellHeight, cellWidth, cellHeight), tint);

            // row highlighting (green outlines) for triggers
            if (pad.IsButtonDown(Buttons.RightShoulder))
                DrawRectangleLines(spriteBatch, new Microsoft.Xna.Framework.Rectangle(cellWidth, overlayHeight - cellHeight * 3 - 1, overlayWidth - cellWidth, cellHeight), 2, Microsoft.Xna.Framework.Color.Green);
            if (pad.IsButtonDown(Buttons.LeftShoulder))
                DrawRectangleLines(spriteBatch, new Microsoft.Xna.Framework.Rectangle(cellWidth, overlayHeight - cellHeight * 2 - 1, overlayWidth - cellWidth, cellHeight), 2, Microsoft.Xna.Framework.Color.Green);
            if (pad.Triggers.Left > 0.5f && WC1Proc == null) // approximate LeftTrigger2
                DrawRectangleLines(spriteBatch, new Microsoft.Xna.Framework.Rectangle(cellWidth, overlayHeight - cellHeight - 1, overlayWidth - cellWidth, cellHeight), 2, Microsoft.Xna.Framework.Color.Green);

            spriteBatch.End();
        }

        base.Draw(gameTime);
    }

    // --- helpers ---

    Texture2D LoadTexture(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine("Texture file not found: " + path);
            return null;
        }
        else
        {
            Console.WriteLine("Loading texture: " + path);
        }
        using var fs = File.OpenRead(path);
        return Texture2D.FromStream(GraphicsDevice, fs);
    }

    void DrawRectangleLines(SpriteBatch? sb, Microsoft.Xna.Framework.Rectangle rect, int thickness, Microsoft.Xna.Framework.Color color)
    {
        if (sb == null || pixel == null) return;

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

    // Uses your existing Platform.Windows.NativeInvoke.GetWindowRect
    public Platform.PlatformInvoke.RECT GetWindowSize(Process gameProc)
    {
#if WINDOWS
        Platform.PlatformInvoke.RECT gameWindowSize = new Platform.PlatformInvoke.RECT();
        Platform.Windows.NativeInvoke.GetWindowRect(gameProc.MainWindowHandle, out gameWindowSize);
        return gameWindowSize;
#elif MACOS
        int processId = gameProc.Id;
        IntPtr array = Platform.MacOS.NativeInvoke.CGWindowListCopyWindowInfo(1, 0);
        if (array == IntPtr.Zero) return default;
        long count = Platform.MacOS.NativeInvoke.CFArrayGetCount(array);
        IntPtr pidKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "kCGWindowOwnerPID", 0x0600);
        IntPtr boundsKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "kCGWindowBounds", 0x0600);
        IntPtr layerKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "kCGWindowLayer", 0x0600);

        Platform.PlatformInvoke.RECT rect = default;
        for (long i = 0; i < count; i++)
        {
            IntPtr dict = Platform.MacOS.NativeInvoke.CFArrayGetValueAtIndex(array, i);
            if (Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(dict, pidKey, out IntPtr pidValue) != 0)
            {
                Platform.MacOS.NativeInvoke.CFNumberGetValue(pidValue, 9, out int pid);
                if (pid == processId)
                {
                    Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(dict, layerKey, out IntPtr layerVal);
                    Platform.MacOS.NativeInvoke.CFNumberGetValue(layerVal, 9, out int layer);
                    if (layer != 0) continue;
                    Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(dict, boundsKey, out IntPtr boundsDict);
                    IntPtr xKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "X", 0x0600);
                    IntPtr yKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "Y", 0x0600);
                    IntPtr wKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "Width", 0x0600);
                    IntPtr hKey = Platform.MacOS.NativeInvoke.CFStringCreateWithCString(IntPtr.Zero, "Height", 0x0600);

                    Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(boundsDict, xKey, out IntPtr xVal);
                    Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(boundsDict, yKey, out IntPtr yVal);
                    Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(boundsDict, wKey, out IntPtr wVal);
                    Platform.MacOS.NativeInvoke.CFDictionaryGetValueIfPresent(boundsDict, hKey, out IntPtr hVal);

                    Platform.MacOS.NativeInvoke.CFNumberGetValue(xVal, 9, out int x);
                    Platform.MacOS.NativeInvoke.CFNumberGetValue(yVal, 9, out int y);
                    Platform.MacOS.NativeInvoke.CFNumberGetValue(wVal, 9, out int w);
                    Platform.MacOS.NativeInvoke.CFNumberGetValue(hVal, 9, out int h);

                    rect.Left = x;
                    rect.Top = y;
                    rect.Right = x + w;
                    rect.Bottom = y + h;
                    break;
                }
            }
        }
        Platform.MacOS.NativeInvoke.CFRelease(array);
        return rect;
#else
        return default;
#endif
    }

    void SetWindowPositionAndSize(int x, int y, int width, int height)
    {
#if WINDOWS
        if (hWnd == IntPtr.Zero) hWnd = this.Window.Handle;
        if (hWnd == IntPtr.Zero) return;
        MoveWindow(hWnd, x, y, width, height, true);
        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TOPMOST | WS_EX_TRANSPARENT);
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_SHOWWINDOW);
#else
        IntPtr window = SDL.SDL_GL_GetCurrentWindow();
        SDL_SetWindowPosition(window, x, y);
        SDL_SetWindowSize(window, width, height);
#endif
        //Console.WriteLine("SetWindowPositionAndSize macOS: " + x + "," + y + " " + width + "x" + height);
    }

    const uint COLOR_KEY = 0x00000000; // magenta in 0x00BBGGRR

}