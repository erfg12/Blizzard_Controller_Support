namespace Blizzard_Controller;
public class OverlayWindow
{
    #region init_vars
    public static Process[] pname = null;
    public static Process SC2Proc, SC1Proc, WC3Proc, WC1Proc, WC2Proc;

    int _overlayWidth { get; set; } = 0;
    int _overlayHeight { get; set; } = 0;
    int _cellColumns { get; set; } = 0;
    int _sideOffset { get; set; } = 0;
    int _bottomOffset { get; set; } = 0;

    [DllImport("user32.dll")]
    private static extern int GetWindowRect(nint hwnd, out System.Drawing.Rectangle rect);

    public static double GetAspectRatio(int width, int height)
    {
        var roundThis = (double)width / height;
        return Math.Round(roundThis, 1);
    }
    #endregion

    public void Initialize()
    {
        int gamepad = 0;
        SetConfigFlags(ConfigFlags.TransparentWindow | ConfigFlags.MousePassthroughWindow);
        SetWindowState(ConfigFlags.UndecoratedWindow);
        SetWindowState(ConfigFlags.TopmostWindow);
        SetTargetFPS(60);
        InitWindow(1, 1, "overlay");

        Invoke.RECT gameWindowSize = new();

        // fits at w:1024 x h:768
        double baseHeight = 768.0;
        double baseWidth = 1024.0;

        Console.WriteLine($"Starting overlay...");

        // x, y
        var aBtnImg = LoadTextureFromImage(LoadImage("Resources/a_btn.png")); // 0,0
        var xBtnImg = LoadTextureFromImage(LoadImage("Resources/x_btn.png")); // 1,0
        var yBtnImg = LoadTextureFromImage(LoadImage("Resources/y_btn.png")); // 2,0
        var bBtnImg = LoadTextureFromImage(LoadImage("Resources/b_btn.png")); // 3,0
        var backBtnImg = LoadTextureFromImage(LoadImage("Resources/back_btn.png")); // 0,1
        var lBtnImg = LoadTextureFromImage(LoadImage("Resources/lb_btn.png")); // 0,2
        var ltBtnImg = LoadTextureFromImage(LoadImage("Resources/lt_btn.png")); // 0,3
        var rBtnImg = LoadTextureFromImage(LoadImage("Resources/rb_btn.png"));

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
                        Invoke.GetWindowRect(SC1Proc.MainWindowHandle, out gameWindowSize);
                        _overlayWidth = GameSettings.StarCraft1.overlayWidth;
                        _overlayHeight = GameSettings.StarCraft1.overlayHeight;
                        _cellColumns = GameSettings.StarCraft1.cellColumns;
                        _bottomOffset = GameSettings.StarCraft1.bottomOffset;
                        _sideOffset = GameSettings.StarCraft1.sideOffset;
                    }
                    else if (SC2Proc != null)
                    {
                        Invoke.GetWindowRect(SC2Proc.MainWindowHandle, out gameWindowSize);
                        _overlayWidth = GameSettings.StarCraft2.overlayWidth;
                        _overlayHeight = GameSettings.StarCraft2.overlayHeight;
                        _cellColumns = GameSettings.StarCraft2.cellColumns;
                        _bottomOffset = GameSettings.StarCraft2.bottomOffset;
                        _sideOffset = GameSettings.StarCraft2.sideOffset;
                    }
                    else if (WC3Proc != null)
                    {
                        Invoke.GetWindowRect(WC3Proc.MainWindowHandle, out gameWindowSize);
                        _overlayWidth = GameSettings.WarCraft3.overlayWidth;
                        _overlayHeight = GameSettings.WarCraft3.overlayHeight;
                        _cellColumns = GameSettings.WarCraft3.cellColumns;
                        _bottomOffset = GameSettings.WarCraft3.bottomOffset;
                        _sideOffset = GameSettings.WarCraft3.sideOffset;
                    }
                    // these games have their command grid on the left side of the window
                    else if (WC1Proc != null)
                    {
                        Invoke.GetWindowRect(WC1Proc.MainWindowHandle, out gameWindowSize);
                        _overlayWidth = GameSettings.WarCraft1.overlayWidth;
                        _overlayHeight = GameSettings.WarCraft1.overlayHeight;
                        _cellColumns = GameSettings.WarCraft1.cellColumns;
                        _bottomOffset = GameSettings.WarCraft1.bottomOffset;
                        _sideOffset = GameSettings.WarCraft1.sideOffset;
                    }
                    else if (WC2Proc != null)
                    {
                        Invoke.GetWindowRect(WC2Proc.MainWindowHandle, out gameWindowSize);
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

                    SetWindowSize(Convert.ToInt32(overlayWidth), Convert.ToInt32(overlayHeight));
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
            ClearBackground(Raylib_cs.Color.Blank);

            // no games running, so don't do anything
            if (SC2Proc == null && SC1Proc == null && WC3Proc == null && WC1Proc == null && WC2Proc == null || gameWindowSize.Left - gameWindowSize.Right == 0)
            {
                EndDrawing();
                continue;
            }

            bool leftSide = WC1Proc != null || WC2Proc != null;

            if (IsGamepadAvailable(gamepad))
            {
                // draw overlay buttons only if we're holding trigger buttons
                if (
                    IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger1) ||
                    IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger1) ||
                    IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger2))
                {
                    var customColor = new Raylib_cs.Color(255, 255, 255, 150); // make images slightly transparent
                    // top row
                    List<Texture2D> btnList = new() { aBtnImg, xBtnImg, yBtnImg, bBtnImg, backBtnImg };
                    int c = _cellColumns - (leftSide ? 0 : 1);
                    for (int i = 0; i < _cellColumns - 1; i++)
                    {
                        DrawTexture(btnList[i], GetRenderWidth() - cellWidth * c--, 0, customColor);
                    }

                    // side buttons
                    DrawTexture(rBtnImg, leftSide ? gameWindowSize.Left + cellWidth * (_cellColumns - 1) : 0, GetRenderHeight() - cellHeight * 3, customColor);
                    DrawTexture(lBtnImg, leftSide ? gameWindowSize.Left + cellWidth * (_cellColumns - 1) : 0, GetRenderHeight() - cellHeight * 2, customColor);
                    if (WC1Proc == null)
                        DrawTexture(ltBtnImg, leftSide ? gameWindowSize.Left + cellWidth * (_cellColumns - 1) : 0, GetRenderHeight() - cellHeight, customColor);
                }

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

            EndDrawing();
        }
    }
}
