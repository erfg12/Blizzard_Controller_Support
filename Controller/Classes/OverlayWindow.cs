namespace Blizzard_Controller;
public class OverlayWindow
{
    #region init_vars
    public static Process[] pname = null;
    public static Process SC2Proc, SC1Proc, WC3Proc, WC1Proc, WC2Proc;
    public static string overlayBtns = "xbox";

    int _overlayWidth { get; set; } = 0;
    int _overlayHeight { get; set; } = 0;
    int _cellColumns { get; set; } = 0;
    int _sideOffset { get; set; } = 0;
    int _bottomOffset { get; set; } = 0;

    public static double GetAspectRatio(int width, int height)
    {
        var roundThis = (double)width / height;
        return Math.Round(roundThis, 1);
    }
    #endregion

    #region GetGameWindowSize
#if WINDOWS
    public Invoke.RECT GetWindowSize(Process gameProc)
    {
        Invoke.RECT gameWindowSize = new Invoke.RECT();

        Invoke.GetWindowRect(gameProc.MainWindowHandle, out gameWindowSize);

        return gameWindowSize;
    }
#elif MACOS
    public Invoke.RECT GetWindowSize(Process gameProc)
    {
        int processId = gameProc.Id;
        IntPtr array = Invoke.CGWindowListCopyWindowInfo(1, 0);
        if (array == IntPtr.Zero) return default;

        long count = Invoke.CFArrayGetCount(array);
        IntPtr pidKey = Invoke.CFStringCreateWithCString(IntPtr.Zero, "kCGWindowOwnerPID", 0x0600);
        IntPtr boundsKey = Invoke.CFStringCreateWithCString(IntPtr.Zero, "kCGWindowBounds", 0x0600);
        IntPtr layerKey = Invoke.CFStringCreateWithCString(IntPtr.Zero, "kCGWindowLayer", 0x0600);

        Invoke.RECT rect = default;

        for (long i = 0; i < count; i++)
        {
            IntPtr dict = Invoke.CFArrayGetValueAtIndex(array, i);

            // Check PID
            if (Invoke.CFDictionaryGetValueIfPresent(dict, pidKey, out IntPtr pidValue) != 0)
            {
                Invoke.CFNumberGetValue(pidValue, 9, out int pid);
                if (pid == processId)
                {
                    // Ensure it's main window (layer 0)
                    Invoke.CFDictionaryGetValueIfPresent(dict, layerKey, out IntPtr layerVal);
                    Invoke.CFNumberGetValue(layerVal, 9, out int layer);
                    if (layer != 0)
                    {
                        Console.WriteLine("layer not found?");
                        continue;
                    }

                    // Get bounds
                    Invoke.CFDictionaryGetValueIfPresent(dict, boundsKey, out IntPtr boundsDict);
                    IntPtr xKey = Invoke.CFStringCreateWithCString(IntPtr.Zero, "X", 0x0600);
                    IntPtr yKey = Invoke.CFStringCreateWithCString(IntPtr.Zero, "Y", 0x0600);
                    IntPtr wKey = Invoke.CFStringCreateWithCString(IntPtr.Zero, "Width", 0x0600);
                    IntPtr hKey = Invoke.CFStringCreateWithCString(IntPtr.Zero, "Height", 0x0600);

                    Invoke.CFDictionaryGetValueIfPresent(boundsDict, xKey, out IntPtr xVal);
                    Invoke.CFDictionaryGetValueIfPresent(boundsDict, yKey, out IntPtr yVal);
                    Invoke.CFDictionaryGetValueIfPresent(boundsDict, wKey, out IntPtr wVal);
                    Invoke.CFDictionaryGetValueIfPresent(boundsDict, hKey, out IntPtr hVal);

                    Invoke.CFNumberGetValue(xVal, 9, out int x);
                    Invoke.CFNumberGetValue(yVal, 9, out int y);
                    Invoke.CFNumberGetValue(wVal, 9, out int w);
                    Invoke.CFNumberGetValue(hVal, 9, out int h);

                    rect.Left = x;
                    rect.Top = y;
                    rect.Right = x + w;
                    rect.Bottom = y + h;
                    break;
                }
            }
        }

        Invoke.CFRelease(array);
        return rect;
    }
#elif LINUX
public Invoke.RECT GetWindowSize(Process gameProc)
{
    IntPtr display = Invoke.XOpenDisplay(IntPtr.Zero);
    if (display == IntPtr.Zero)
        return default;

    IntPtr root = Invoke.XDefaultRootWindow(display);

    // Find all windows for this PID
    var window = FindWindowByPID(display, root, gameProc.Id);
    if (window == IntPtr.Zero)
    {
        Invoke.XCloseDisplay(display);
        return default;
    }

    Invoke.XWindowAttributes attr;
    if (Invoke.XGetWindowAttributes(display, window, out attr) == 0)
    {
        Invoke.XCloseDisplay(display);
        return default;
    }

    Invoke.RECT rect = new Invoke.RECT
    {
        Left = attr.x,
        Top = attr.y,
        Right = attr.x + attr.width,
        Bottom = attr.y + attr.height
    };

    Invoke.XCloseDisplay(display);
    return rect;
}

private IntPtr FindWindowByPID(IntPtr display, IntPtr root, int pid)
{
    IntPtr rootReturn, parentReturn;
    IntPtr childrenPtr;
    uint nChildren;

    // Query the window tree
    if (Invoke.XQueryTree(display, root, out rootReturn, out parentReturn, out childrenPtr, out nChildren) == 0)
        return IntPtr.Zero;

    IntPtr result = IntPtr.Zero;

    if (nChildren > 0)
    {
        // Convert unmanaged array of window handles to managed IntPtr[]
        IntPtr[] children = new IntPtr[nChildren];
        for (int i = 0; i < nChildren; i++)
        {
            children[i] = Marshal.ReadIntPtr(childrenPtr, i * IntPtr.Size);
        }

        // Iterate through children
        for (int i = 0; i < children.Length; i++)
        {
            if (WindowMatchesPID(display, children[i], pid))
            {
                result = children[i];
                break;
            }

            // Recursively search in child windows
            result = FindWindowByPID(display, children[i], pid);
            if (result != IntPtr.Zero)
                break;
        }

        Invoke.XFree(childrenPtr);
    }

    return result;
}


private bool WindowMatchesPID(IntPtr display, IntPtr window, int pid)
{
    // Get _NET_WM_PID property
    IntPtr actualType;
    int actualFormat;
    ulong nItems, bytesAfter;
    IntPtr propPID;

    if (Invoke.XGetWindowProperty(display, window,
        Invoke.XInternAtom(display, "_NET_WM_PID", false),
        IntPtr.Zero, new IntPtr(1), false,
        0, out actualType, out actualFormat, out nItems, out bytesAfter, out propPID) != 0)
        return false;

    if (propPID == IntPtr.Zero)
        return false;

    int windowPID = System.Runtime.InteropServices.Marshal.ReadInt32(propPID);
    Invoke.XFree(propPID);

    return windowPID == pid;
}
#endif
    #endregion

    public void Initialize()
    {
        int gamepad = 0;

        SetConfigFlags(ConfigFlags.TransparentWindow | ConfigFlags.MousePassthroughWindow);
        SetWindowState(ConfigFlags.UndecoratedWindow);
        SetWindowState(ConfigFlags.TopmostWindow);
        SetTargetFPS(60);
        InitWindow(1, 1, "overlay");

#if MACOS
        string mapping = "";
        mapping += "030000005e040000130b000020050000,Xbox Wireless Controller,platform:Mac OS X,a:b0,b:b1,x:b3,y:b4,back:b10,guide:b12,start:b11,leftstick:b13,rightstick:b14,leftshoulder:b6,rightshoulder:b7,dpup:h0.1,dpdown:h0.4,dpleft:h0.8,dpright:h0.2,leftx:a0,lefty:a1,rightx:a2,righty:a3,lefttrigger:a5,righttrigger:a4,\n";
        mapping += "030000004c050000e60c000000010000,PS5 Controller,platform:Mac OS X,a:b1,b:b2,x:b0,y:b3,back:b8,guide:b13,start:b9,leftstick:b10,rightstick:b11,leftshoulder:b4,rightshoulder:b5,dpup:h0.1,dpdown:h0.4,dpleft:h0.8,dpright:h0.2,leftx:a0,lefty:a1,rightx:a2,righty:a5,lefttrigger:a3,righttrigger:a4,\n";
        mapping += "030000004c050000cc09000000010000,PS4 Controller,platform:Mac OS X,a:b1,b:b2,x:b0,y:b3,back:b8,guide:b13,start:b9,leftstick:b10,rightstick:b11,leftshoulder:b4,rightshoulder:b5,dpup:h0.1,dpdown:h0.4,dpleft:h0.8,dpright:h0.2,leftx:a0,lefty:a1,rightx:a2,righty:a5,lefttrigger:a3,righttrigger:a4,\n";
        mapping += "030000004c0500006802000000010000,PS3 Controller,platform:Mac OS X,a:b14,b:b13,x:b15,y:b12,back:b0,guide:b16,start:b3,leftstick:b1,rightstick:b2,leftshoulder:b10,rightshoulder:b11,dpup:b4,dpdown:b6,dpleft:b7,dpright:b5,leftx:a0,lefty:a1,rightx:a2,righty:a3,lefttrigger:b8,righttrigger:b9,\n";
        SetGamepadMappings(mapping);
#endif

        Invoke.RECT gameWindowSize = new();

        // fits at w:1024 x h:768
        double baseHeight = 768.0;
        double baseWidth = 1024.0;

        Console.WriteLine($"Starting overlay...");

        // x, y
        var aBtnImg = LoadTextureFromImage(LoadImage("Resources/a_btn.png"));
        var xBtnImg = LoadTextureFromImage(LoadImage("Resources/x_btn.png"));
        var yBtnImg = LoadTextureFromImage(LoadImage("Resources/y_btn.png"));
        var bBtnImg = LoadTextureFromImage(LoadImage("Resources/b_btn.png"));
        var backBtnImg = LoadTextureFromImage(LoadImage("Resources/back_btn.png"));
        var lBtnImg = LoadTextureFromImage(LoadImage("Resources/lb_btn.png"));
        var ltBtnImg = LoadTextureFromImage(LoadImage("Resources/lt_btn.png"));
        var rBtnImg = LoadTextureFromImage(LoadImage("Resources/rb_btn.png"));

        var ps_xBtn = LoadTextureFromImage(LoadImage("Resources/ps_x_btn.png"));
        var ps_squareBtn = LoadTextureFromImage(LoadImage("Resources/ps_square_btn.png"));
        var ps_triangleBtn = LoadTextureFromImage(LoadImage("Resources/ps_triangle_btn.png"));
        var ps_circleBtn = LoadTextureFromImage(LoadImage("Resources/ps_circle_btn.png"));
        var ps_shareBtn = LoadTextureFromImage(LoadImage("Resources/ps_share_btn.png"));
        var ps_l1Btn = LoadTextureFromImage(LoadImage("Resources/ps_l1_btn.png"));
        var ps_l2Btn = LoadTextureFromImage(LoadImage("Resources/ps_l2_btn.png"));
        var ps_r1Btn = LoadTextureFromImage(LoadImage("Resources/ps_r1_btn.png"));

        int cellWidth = 0;
        int cellHeight = 0;

        int check = 0;

        while (!ControllerInputs.shuttingDown)
        {
            // check if game died. do these operations every 100ms or so
            if (check >= 100)
            {
                ControllerInputs.controller = IsGamepadAvailable(gamepad);
                SC2Proc = Process.GetProcessesByName(GameSettings.ProcessNames.SC2ProcName).FirstOrDefault();
                SC1Proc = Process.GetProcessesByName(GameSettings.ProcessNames.SC1ProcName).FirstOrDefault();
                WC3Proc = Process.GetProcessesByName(GameSettings.ProcessNames.WC3ProcName).FirstOrDefault();
                WC2Proc = Process.GetProcessesByName(GameSettings.ProcessNames.WC2ProcName).FirstOrDefault();
                WC1Proc = Process.GetProcessesByName(GameSettings.ProcessNames.WC1ProcName).FirstOrDefault();
                if (SC2Proc != null || SC1Proc != null || WC3Proc != null || WC1Proc != null || WC2Proc != null)
                {
                    if (SC1Proc != null)
                    {
                        gameWindowSize = GetWindowSize(SC1Proc);
                        _overlayWidth = GameSettings.StarCraft1.overlayWidth;
                        _overlayHeight = GameSettings.StarCraft1.overlayHeight;
                        _cellColumns = GameSettings.StarCraft1.cellColumns;
                        _bottomOffset = GameSettings.StarCraft1.bottomOffset;
                        _sideOffset = GameSettings.StarCraft1.sideOffset;
                    }
                    else if (SC2Proc != null)
                    {
                        gameWindowSize = GetWindowSize(SC2Proc);
                        _overlayWidth = GameSettings.StarCraft2.overlayWidth;
                        _overlayHeight = GameSettings.StarCraft2.overlayHeight;
                        _cellColumns = GameSettings.StarCraft2.cellColumns;
                        _bottomOffset = GameSettings.StarCraft2.bottomOffset;
                        _sideOffset = GameSettings.StarCraft2.sideOffset;
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
                    // these games have their command grid on the left side of the window
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

                    double diff = gameHeight / baseHeight;
                    double overlayHeight = _overlayHeight * diff;
                    double overlayWidth = _overlayWidth * diff;

                    SetWindowSize(Convert.ToInt32(overlayWidth), Convert.ToInt32(overlayHeight));

                    cellWidth = GetRenderWidth() / _cellColumns;
                    cellHeight = GetRenderHeight() / 4; // cell count + 1

                    aBtnImg.Width = cellWidth;
                    aBtnImg.Height = cellHeight;
                    xBtnImg.Width = cellWidth;
                    xBtnImg.Height = cellHeight;
                    yBtnImg.Width = cellWidth;
                    yBtnImg.Height = cellHeight;
                    bBtnImg.Width = cellWidth;
                    bBtnImg.Height = cellHeight;
                    lBtnImg.Width = cellWidth;
                    lBtnImg.Height = cellHeight;
                    ltBtnImg.Width = cellWidth;
                    ltBtnImg.Height = cellHeight;
                    rBtnImg.Width = cellWidth;
                    rBtnImg.Height = cellHeight;
                    backBtnImg.Width = cellWidth;
                    backBtnImg.Height = cellHeight;

                    ps_xBtn.Width = cellWidth;
                    ps_xBtn.Height = cellHeight;
                    ps_squareBtn.Width = cellWidth;
                    ps_squareBtn.Height = cellHeight;
                    ps_triangleBtn.Width = cellWidth;
                    ps_triangleBtn.Height = cellHeight;
                    ps_circleBtn.Width = cellWidth;
                    ps_circleBtn.Height = cellHeight;
                    ps_l1Btn.Width = cellWidth;
                    ps_l1Btn.Height = cellHeight;
                    ps_l2Btn.Width = cellWidth;
                    ps_l2Btn.Height = cellHeight;
                    ps_r1Btn.Width = cellWidth;
                    ps_r1Btn.Height = cellHeight;
                    ps_shareBtn.Width = cellWidth;
                    ps_shareBtn.Height = cellHeight;

                    // WC3 offsets their command grid differently per aspect ratio
                    if (WC3Proc != null)
                    {
                        var test = GetAspectRatio(gameWidth, gameHeight);
                        if (test == 1.8)
                        {
                            _sideOffset = Convert.ToInt32(0.135 * gameWidth);
                        }
                        else if (test == 1.6)
                        {
                            _sideOffset = Convert.ToInt32(0.095 * gameWidth);
                        }
                    }

                    if (WC1Proc != null || WC2Proc != null) // left side
                        SetWindowPosition(gameWindowSize.Left - _sideOffset, gameWindowSize.Bottom - GetRenderHeight() - _bottomOffset);
                    else                                    // right side
                        SetWindowPosition(gameWindowSize.Right - GetRenderWidth() - _sideOffset, gameWindowSize.Bottom - GetRenderHeight() - _bottomOffset);
                }
                check = 0;
            }
            else
                check++;

            BeginDrawing();
            ClearBackground(Color.Blank);

            // no games running, so don't do anything
            if (SC2Proc == null && SC1Proc == null && WC3Proc == null && WC1Proc == null && WC2Proc == null || gameWindowSize.Left - gameWindowSize.Right == 0)
            {
                EndDrawing();
                continue;
            }

            bool leftSide = WC1Proc != null || WC2Proc != null;

            if (IsGamepadAvailable(gamepad))
            {

                //draw overlay buttons only if we're holding trigger buttons
                if (
                    IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger1) ||
                    IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger1) ||
                    IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger2))
                {
                    //Console.WriteLine("holding a trigger down");
                    var customColor = new Raylib_cs.Color(255, 255, 255, 150); // make images slightly transparent
                    // top row
                    List<Texture2D> btnList = new() { aBtnImg, xBtnImg, yBtnImg, bBtnImg, backBtnImg };
                    List<Texture2D> ps_btnList = new() { ps_xBtn, ps_squareBtn, ps_triangleBtn, ps_circleBtn, ps_shareBtn };
                    int c = _cellColumns - (leftSide ? 0 : 1);
                    for (int i = 0; i < _cellColumns - 1; i++)
                    {
                        if (overlayBtns.Equals("playstation"))
                            DrawTexture(ps_btnList[i], GetRenderWidth() - cellWidth * c--, 0, customColor);
                        else
                            DrawTexture(btnList[i], GetRenderWidth() - cellWidth * c--, 0, customColor);
                    }

                    // side buttons
                    DrawTexture(overlayBtns.Equals("playstation") ? ps_r1Btn : rBtnImg, leftSide ? gameWindowSize.Left + cellWidth * (_cellColumns - 1) : 0, GetRenderHeight() - cellHeight * 3, customColor);
                    DrawTexture(overlayBtns.Equals("playstation") ? ps_l1Btn : lBtnImg, leftSide ? gameWindowSize.Left + cellWidth * (_cellColumns - 1) : 0, GetRenderHeight() - cellHeight * 2, customColor);
                    if (WC1Proc == null)
                        DrawTexture(overlayBtns.Equals("playstation") ? ps_l2Btn : ltBtnImg, leftSide ? gameWindowSize.Left + cellWidth * (_cellColumns - 1) : 0, GetRenderHeight() - cellHeight, customColor);

                    // row highlighting
                    if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger1))
                        DrawRectangleLines(
                                GetRenderWidth() - cellWidth * (_cellColumns - (leftSide ? 0 : 1)) - 4,
                                GetRenderHeight() - cellHeight * 3,
                                GetRenderWidth() - cellWidth,
                                cellHeight,
                                Raylib_cs.Color.Green
                            );
                    if (IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger1))
                        DrawRectangleLines(
                            GetRenderWidth() - cellWidth * (_cellColumns - (leftSide ? 0 : 1)) - 4,
                            GetRenderHeight() - cellHeight * 2,
                            GetRenderWidth() - cellWidth,
                            cellHeight,
                            Raylib_cs.Color.Green
                        );
                    if (IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger2) && WC1Proc == null)
                        DrawRectangleLines(
                            GetRenderWidth() - cellWidth * (_cellColumns - (leftSide ? 0 : 1)) - 4,
                            GetRenderHeight() - cellHeight,
                            GetRenderWidth() - cellWidth,
                            cellHeight,
                            Raylib_cs.Color.Green
                        );
                    ControllerInputs.processButtons();
                    ControllerInputs.processJoysticks();
                }
            }

            EndDrawing();
        }
    }
}
