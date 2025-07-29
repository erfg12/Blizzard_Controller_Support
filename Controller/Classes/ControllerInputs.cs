namespace Blizzard_Controller;
class ControllerInputs
{
    public static double deadzone = 0.0;
    public static int mouseDistance = 12;
    public static int mouseDistanceDefault = 12;
    public static double faster = 0.5; // increase mouse speed once it goes past this
    public static double slower = 0.4;
    public static bool shuttingDown = false;

    public static Process[] pname = null;

    public static Point cursorPos = new Point();

    public static bool holdingA = false;
    public static bool holdingRT = false;
    public static bool holdingRJoy = false;
    public static bool holdingLJoy = false;
    public static bool holdingRJoyDirLeft = false;
    public static bool holdingRJoyDirRight = false;
    public static bool holdingRJoyDirUp = false;
    public static bool holdingRJoyDirDown = false;

    public static string gameProcStatus = "Not Running";
    public static bool controller = false;
    public static int gamepad = 0;

    private const int SW_MAXIMIZE = 3;

    public static bool IsWindowedMode(IntPtr hWnd)
    {
        bool retVal = false;

#if WINDOWS
        int style = Invoke.GetWindowLong(hWnd, Invoke.GWL_STYLE);
        retVal = (style & Invoke.WS_OVERLAPPEDWINDOW) == Invoke.WS_OVERLAPPEDWINDOW;
#elif LINUX
        // to-do
#elif MACOS
        // to-do
#endif

        return retVal;
    }

    public static void MaximizeWindow(IntPtr handle)
    {
#if WINDOWS
        Invoke.ShowWindow(handle, SW_MAXIMIZE);
#elif LINUX
        // to-do
#elif MACOS
        // to-do
#endif
    }

    /// <summary>
    /// Check if game is running. If not, change public variable (gameProcStatus) to false to stop BGWorkers till a new game is launched.
    /// </summary>
    public static void CheckGameProc()
    {
        while (!shuttingDown)
        {
            if (Process.GetProcessesByName(GameSettings.ProcessNames.SC2ProcName).Length > 0)
            {
                pname = Process.GetProcessesByName(GameSettings.ProcessNames.SC2ProcName);
                gameProcStatus = "StarCraft 2 Running";
            }
            else if (Process.GetProcessesByName(GameSettings.ProcessNames.SC1ProcName).Length > 0)
            {
                pname = Process.GetProcessesByName(GameSettings.ProcessNames.SC1ProcName);
                gameProcStatus = "StarCraft: Remastered";
            }
            else if (Process.GetProcessesByName(GameSettings.ProcessNames.WC3ProcName).Length > 0)
            {
                pname = Process.GetProcessesByName(GameSettings.ProcessNames.WC3ProcName);
                gameProcStatus = "WarCraft III: Reforged";
            }
            else if (Process.GetProcessesByName(GameSettings.ProcessNames.WC1ProcName).Length > 0)
            {
                pname = Process.GetProcessesByName(GameSettings.ProcessNames.WC1ProcName);
                //if (pname == null) continue;
                gameProcStatus = "WarCraft I: Remastered";
                if (IsWindowedMode(pname.First().MainWindowHandle)) // maximize windowed mode
                    MaximizeWindow(pname.First().MainWindowHandle);
            }
            else if (Process.GetProcessesByName(GameSettings.ProcessNames.WC2ProcName).Length > 0)
            {
                pname = Process.GetProcessesByName(GameSettings.ProcessNames.WC2ProcName);
                //if (pname == null) continue;
                gameProcStatus = "WarCraft II: Remastered";
                if (IsWindowedMode(pname.First().MainWindowHandle)) // maximize windowed mode
                    MaximizeWindow(pname.First().MainWindowHandle);
            }
            else
            {
                pname = null;
                gameProcStatus = "Not Running";
            }

            Thread.Sleep(500);
        }
    }

    /// <summary>
    /// Mouse click. x, y coords with button.
    /// </summary>
    /// <param name="x">cursor X position</param>
    /// <param name="y">cursor Y position</param>
    /// <param name="btn">Button to press. See MouseClicks enum.</param>
    public static void globalMouseClick(Invoke.MouseClicks btn, int x = 0, int y = 0)
    {
        // if (btn == Invoke.MouseClicks.left_down)
        //     aix3c.MouseDown();
        // else if (btn == Invoke.MouseClicks.left_up)
        //     aix3c.MouseUp();

        // if (btn == Invoke.MouseClicks.right_BLCLK)
        //     aix3c.MouseClick("RIGHT");

        // if (btn == Invoke.MouseClicks.middle_down)
        //     aix3c.MouseDown("MIDDLE");
        // else if (btn == Invoke.MouseClicks.middle_up)
        //     aix3c.MouseUp("MIDDLE");
        Thread.Sleep(150); // prevent double press
    }

    /// <summary>
    /// Process all controller button presses. This function loops endlessly.
    /// </summary>
    public static void processButtons()
    {
        // if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceUp))
        // {
        //     aix3c.Send("^1"); // CTRL + 1
        // }
        // if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceRight))
        // {
        //     aix3c.Send("^2"); // CTRL + 2
        // }
        // if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceDown))
        // {
        //     aix3c.Send("^3"); // CTRL + 3
        // }
        // if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceLeft))
        // {
        //     aix3c.Send("^4"); // CTRL + 4
        // }

        // if (IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceUp))
        // {
        //     aix3c.Send("1"); // 1
        // }
        // if (IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceRight))
        // {
        //     aix3c.Send("2"); // 2
        // }
        // if (IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceDown))
        // {
        //     aix3c.Send("3"); // 3
        // }
        // if (IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceLeft))
        // {
        //     aix3c.Send("4"); // 4
        // }

        // if (IsGamepadButtonDown(gamepad, GamepadButton.LeftThumb) && holdingLJoy == false)
        // {
        //     aix3c.Send("{LSHIFT down}");
        //     holdingLJoy = true;
        // }
        // else if (IsGamepadButtonUp(gamepad, GamepadButton.LeftThumb) && holdingLJoy == true)
        // {
        //     aix3c.Send("{LSHIFT up}");
        //     holdingLJoy = false;
        // }

        if (IsGamepadButtonDown(gamepad, GamepadButton.RightThumb) && holdingRJoy == false)
        {
            holdingRJoy = true;
        }
        else if (IsGamepadButtonUp(gamepad, GamepadButton.RightThumb) && holdingRJoy == true)
        {
            holdingRJoy = false;
        }

        if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && holdingRT == false) // hold down if not already
        {
            globalMouseClick(Invoke.MouseClicks.middle_down); // middle mouse btn
            mouseDistance = mouseDistanceDefault;
            holdingRT = true;
        }
        else if (IsGamepadButtonUp(gamepad, GamepadButton.RightTrigger2) && holdingRT == true) // release
        {
            globalMouseClick(Invoke.MouseClicks.middle_up); // middle mouse btn
            mouseDistance = mouseDistanceDefault;
            holdingRT = false;
        }

        if (IsGamepadButtonPressed(gamepad, GamepadButton.MiddleRight))
        {
            // aix3c.Send("{f10}"); // F10
        }

        // Not holding RB, RT, LB, LT and pressing buttons
        if (IsGamepadButtonUp(gamepad, GamepadButton.LeftTrigger1) && IsGamepadButtonUp(gamepad, GamepadButton.LeftTrigger2) && IsGamepadButtonUp(gamepad, GamepadButton.RightTrigger1) && IsGamepadButtonUp(gamepad, GamepadButton.RightTrigger2))
        {
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
            // {
            //     aix3c.Send("{f1}"); //F1
            // }
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceRight))
            // {
            //     aix3c.Send("{f2}"); //F2
            // }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
            {
                globalMouseClick(Invoke.MouseClicks.right_BLCLK); // right mouse btn
            }
            if (IsGamepadButtonDown(gamepad, GamepadButton.RightFaceDown) && holdingA == false)
            {
                globalMouseClick(Invoke.MouseClicks.left_down);
                holdingA = true;
            }
            else if (IsGamepadButtonUp(gamepad, GamepadButton.RightFaceDown) && holdingA == true)
            {
                globalMouseClick(Invoke.MouseClicks.left_up); // l mouse click up
                holdingA = false;
            }
        }
        // Holding RB down.
        else if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger1))
        {
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
            //     aix3c.Send("{q}"); //Q
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
            //     aix3c.Send("{w}"); //W
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
            //     aix3c.Send("{e}"); //E
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceRight))
            //     aix3c.Send("{r}"); //R
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.MiddleLeft))
            //     aix3c.Send("{t}"); //T
        }
        // Holding LB down.
        else if (IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger1))
        {
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
            //     aix3c.Send("{a}"); //A
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
            //     aix3c.Send("{s}"); //S
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
            // {
            //     aix3c.Send("{d}"); //D
            //     aix3c.Send("{k}"); //K // for sc1 (works for Terran, not Zerg. Need to find class offset and read value in memory)
            //     aix3c.Send("{l}"); //L // for sc1
            // }
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceRight))
            //     aix3c.Send("{f}"); //F
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.MiddleLeft))
            //     aix3c.Send("{g}"); //G
        }
        // Holding LT down.
        else if (IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger2))
        {
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
            //     aix3c.Send("{z}"); //Z
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
            //     aix3c.Send("{x}"); //X
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
            //     aix3c.Send("{c}"); //C
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceRight))
            //     aix3c.Send("{v}"); //V
            // if (IsGamepadButtonPressed(gamepad, GamepadButton.MiddleLeft))
            //     aix3c.Send("{b}"); //B
        }
        Thread.Sleep(10);
    }

    /// <summary>
    /// Process all controller joystick movements. This function loops endlessly.
    /// </summary>
    public static void processJoysticks()
    {
        if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > deadzone || GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -deadzone
            || GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > deadzone || GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -deadzone)
        {
#if WINDOWS
            Invoke.GetCursorPos(ref cursorPos);
#elif LINUX
            // to-do
#elif MACOS
            // to-do
#endif

            if (Properties.Settings.Default.IncreaseCursorSpeed)
            {
                // left/right slower
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > deadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < slower)
                    {
                        cursorPos.X += mouseDistance / 2;
                    }
                }
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -deadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > -faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > -slower)
                    {
                        cursorPos.X -= mouseDistance / 2;
                    }
                }

                // up/down slower
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > deadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < slower)
                    {
                        cursorPos.Y += mouseDistance / 2;
                    }
                }
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -deadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > -faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > -slower)
                    {
                        cursorPos.Y -= mouseDistance / 2;
                    }
                }

                // left/right normal
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > deadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > slower)
                        cursorPos.X += mouseDistance;
                }
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -deadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > -faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -slower)
                        cursorPos.X -= mouseDistance;
                }

                // up/down normal
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > deadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > slower)
                        cursorPos.Y += mouseDistance;
                }
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -deadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > -faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -slower)
                        cursorPos.Y -= mouseDistance;
                }

                // left/right faster
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > deadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > faster)
                    cursorPos.X += mouseDistance * 2;
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -deadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -faster)
                    cursorPos.X -= mouseDistance * 2;

                // up/down faster
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > deadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > faster)
                    cursorPos.Y += mouseDistance * 2;
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -deadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -faster)
                    cursorPos.Y -= mouseDistance * 2;
            }
            else
            {
                // left/right slower
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > deadzone) cursorPos.X += mouseDistance;
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -deadzone) cursorPos.X -= mouseDistance;

                // up/down slower
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > deadzone) cursorPos.Y += mouseDistance;
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -deadzone) cursorPos.Y -= mouseDistance;
            }
#if WINDOWS
            Invoke.SetCursorPos(cursorPos.X, cursorPos.Y);
#elif LINUX
            // to-do
#elif MACOS
            // to-do
#endif
            //Debug.WriteLine("mX:" + mX + " mY:" + mY + " myTime:" + myTime); // DEBUG
        }

        // move camera
        if (GetGamepadAxisMovement(gamepad, GamepadAxis.RightY) < deadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.RightY) != 0.0 && !holdingRT)
        {
            holdingRJoyDirUp = true;
            // if (holdingRT)
            //     aix3c.Send("{PGUP}"); //PGUP
            // else
            // {
            //     aix3c.Send("{UP down}");
            //     if (holdingRJoyDirDown)
            //     {
            //         aix3c.Send("{DOWN up}");
            //         holdingRJoyDirDown = false;
            //     }
            // }
        }
        else if (GetGamepadAxisMovement(gamepad, GamepadAxis.RightY) > -deadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.RightY) != 0.0 && !holdingRT)
        {
            holdingRJoyDirDown = true;
            // if (holdingRT)
            //     aix3c.Send("{PGDN}"); //PGDN
            // else
            // {
            //     if (holdingRJoyDirUp)
            //     {
            //         aix3c.Send("{UP up}");
            //         holdingRJoyDirUp = false;
            //     }
            //     aix3c.Send("{DOWN down}");
            // }
        }
        else
        {
            if (holdingRJoyDirUp || holdingRJoyDirDown)
            {
                // aix3c.Send("{UP up}");
                // aix3c.Send("{DOWN up}");
                holdingRJoyDirUp = false;
                holdingRJoyDirDown = false;
            }
        }

        if (GetGamepadAxisMovement(gamepad, GamepadAxis.RightX) > deadzone && !holdingRT)
        {
            // if (holdingRJoyDirLeft)
            // {
            //     aix3c.Send("{LEFT up}");
            //     holdingRJoyDirLeft = false;
            // }
            // aix3c.Send("{RIGHT down}");
            holdingRJoyDirRight = true;
        }
        else if (GetGamepadAxisMovement(gamepad, GamepadAxis.RightX) < -deadzone && !holdingRT && !holdingRT)
        {
            // aix3c.Send("{LEFT down}");
            // if (holdingRJoyDirRight)
            // {
            //     aix3c.Send("{RIGHT up}");
            //     holdingRJoyDirRight = false;
            // }
            holdingRJoyDirLeft = true;
        }
        else
        {
            if (holdingRJoyDirRight || holdingRJoyDirLeft)
            {
                // aix3c.Send("{LEFT up}");
                // aix3c.Send("{RIGHT up}");
                holdingRJoyDirRight = false;
                holdingRJoyDirLeft = false;
            }
        }

        Thread.Sleep(10);
    }
}
