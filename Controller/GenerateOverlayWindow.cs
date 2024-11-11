using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Globalization;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.Color;
using System.Windows;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blizzard_Controller
{
    public class GenerateOverlayWindow
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #region init_vars
        public static System.Diagnostics.Process[] pname = null;

        //private Rectangle resolution = Screen.PrimaryScreen.Bounds;

        public static string SC2ProcName = "SC2_x64";
        public static string WC3ProcName = "Warcraft III";
        public static string SC1ProcName = "StarCraft";
        public static System.Diagnostics.Process SC2Proc, SC1Proc, WC3Proc;

        int _overlayWidth { get; set; } = 0;
        int _overlayHeight { get; set; } = 0;
        int _cellColumns { get; set; } = 0;
        int _rightOffset { get; set; } = 0;
        int _bottomOffset { get; set; } = 0;

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out System.Drawing.Rectangle rect);

        public static double GetAspectRatio(int width, int height)
        {
            var roundThis = (double)width / height;
            return Math.Round(roundThis, 1);
        }
        #endregion

        public void Initialize()
        {
            //ControllerInputs ci = new();
            int gamepad = 0;
            Raylib.SetConfigFlags(ConfigFlags.TransparentWindow | ConfigFlags.MousePassthroughWindow);
            Raylib.SetWindowState(ConfigFlags.UndecoratedWindow);
            Raylib.SetWindowState(ConfigFlags.TopmostWindow);
            Raylib.SetTargetFPS(60);
            InitWindow(1, 1, "overlay");

            RECT gameWindowSize = new();

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


            while (true)
            {
                // check if game died. do these operations every 100ms or so
                if (check >= 100)
                {
                    ControllerInputs.controller = IsGamepadAvailable(gamepad);
                    SC2Proc = System.Diagnostics.Process.GetProcessesByName(SC2ProcName).FirstOrDefault();
                    SC1Proc = System.Diagnostics.Process.GetProcessesByName(SC1ProcName).FirstOrDefault();
                    WC3Proc = System.Diagnostics.Process.GetProcessesByName(WC3ProcName).FirstOrDefault();
                    if (SC2Proc != null || SC1Proc != null || WC3Proc != null)
                    {
                        if (SC1Proc != null)
                        {
                            GetWindowRect(SC1Proc.MainWindowHandle, out gameWindowSize);
                            _overlayWidth = GameSettings.StarCraft1.overlayWidth;
                            _overlayHeight = GameSettings.StarCraft1.overlayHeight;
                            _cellColumns = GameSettings.StarCraft1.cellColumns;
                            _bottomOffset = GameSettings.StarCraft1.bottomOffset;
                            _rightOffset = GameSettings.StarCraft1.rightOffset;
                        }
                        if (SC2Proc != null)
                        {
                            GetWindowRect(SC2Proc.MainWindowHandle, out gameWindowSize);
                            _overlayWidth = GameSettings.StarCraft2.overlayWidth;
                            _overlayHeight = GameSettings.StarCraft2.overlayHeight;
                            _cellColumns = GameSettings.StarCraft2.cellColumns;
                            _bottomOffset = GameSettings.StarCraft2.bottomOffset;
                            _rightOffset = GameSettings.StarCraft2.rightOffset;
                        }
                        if (WC3Proc != null)
                        {
                            GetWindowRect(WC3Proc.MainWindowHandle, out gameWindowSize);
                            _overlayWidth = GameSettings.WarCraft3.overlayWidth;
                            _overlayHeight = GameSettings.WarCraft3.overlayHeight;
                            _cellColumns = GameSettings.WarCraft3.cellColumns;
                            _bottomOffset = GameSettings.WarCraft3.bottomOffset;
                            _rightOffset = GameSettings.WarCraft3.rightOffset;
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
                                _rightOffset = Convert.ToInt32(0.135 * gameWidth);
                            }
                            else if (test == 1.6)
                            {
                                _rightOffset = Convert.ToInt32(0.095 * gameWidth);
                            }
                        }

                        SetWindowSize(Convert.ToInt32(overlayWidth), Convert.ToInt32(overlayHeight));
                        Raylib.SetWindowPosition(gameWindowSize.Right - GetRenderWidth() - _rightOffset, gameWindowSize.Bottom - GetRenderHeight() - _bottomOffset);
                    }
                    check = 0;
                }
                else
                    check++;

                BeginDrawing();
                ClearBackground(Blank);

                if ((SC2Proc == null && SC1Proc == null && WC3Proc == null) || (gameWindowSize.Left - gameWindowSize.Right == 0))
                {
                    EndDrawing();
                    continue;
                }

                // top row
                List<Texture2D> btnList = new() { aBtnImg, xBtnImg, yBtnImg, bBtnImg, backBtnImg };
                int c = _cellColumns - 1;
                for (int i = 0; i < _cellColumns - 1; i++)
                {
                    DrawTexture(btnList[i], Raylib.GetRenderWidth() - cellWidth * c--, 0, White);
                }

                // left side
                DrawTexture(rBtnImg, 0, GetRenderHeight() - cellHeight * 3, White);
                DrawTexture(lBtnImg, 0, GetRenderHeight() - cellHeight * 2, White);
                DrawTexture(ltBtnImg, 0, GetRenderHeight() - cellHeight, White);

                if (IsGamepadAvailable(gamepad))
                {
                    // row highlighting
                    if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger1))
                        DrawRectangleLines(
                            Raylib.GetRenderWidth() - cellWidth * (_cellColumns - 1) - 4,
                            GetRenderHeight() - cellHeight * 3,
                            Raylib.GetRenderWidth() - cellWidth,
                            cellHeight,
                            Green
                        );
                    if (IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger1))
                        DrawRectangleLines(
                            Raylib.GetRenderWidth() - cellWidth * (_cellColumns - 1) - 4,
                            GetRenderHeight() - cellHeight * 2,
                            Raylib.GetRenderWidth() - cellWidth,
                            cellHeight,
                            Green
                        );
                    if (IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger2))
                        DrawRectangleLines(
                            Raylib.GetRenderWidth() - cellWidth * (_cellColumns - 1) - 4,
                            GetRenderHeight() - cellHeight,
                            Raylib.GetRenderWidth() - cellWidth,
                            cellHeight,
                            Green
                        );
                    ControllerInputs.processButtons();
                    ControllerInputs.processJoysticks();
                }

                EndDrawing();
            }
        }
    }
}
