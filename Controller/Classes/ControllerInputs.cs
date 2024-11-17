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

    public static InputSimulator sim = new InputSimulator();

    const uint WM_KEYDOWN = 0x0100;
    const uint WM_KEYUP = 0x0101;

    private const int SW_MAXIMIZE = 3;

    public static bool IsWindowedMode(IntPtr hWnd)
    {
        int style = Invoke.GetWindowLong(hWnd, Invoke.GWL_STYLE);
        // Check if the window has the overlapped window style, which indicates windowed mode
        return (style & Invoke.WS_OVERLAPPEDWINDOW) == Invoke.WS_OVERLAPPEDWINDOW;
    }

    /// <summary>
    /// Check if game is running. If not, change public variable (gameProcStatus) to false to stop BGWorkers till a new game is launched.
    /// </summary>
    public static void CheckGameProc()
    {
        while (true)
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
                gameProcStatus = "WarCraft I: Remastered";
                if (IsWindowedMode(pname.First().MainWindowHandle)) // maximize windowed mode
                    Invoke.ShowWindow(pname.First().MainWindowHandle, SW_MAXIMIZE);

            }
            else if (Process.GetProcessesByName(GameSettings.ProcessNames.WC2ProcName).Length > 0)
            {
                pname = Process.GetProcessesByName(GameSettings.ProcessNames.WC2ProcName);
                gameProcStatus = "WarCraft II: Remastered";
                if (IsWindowedMode(pname.First().MainWindowHandle)) // maximize windowed mode
                    Invoke.ShowWindow(pname.First().MainWindowHandle, SW_MAXIMIZE);
            }
            else
            {
                pname = null;
                gameProcStatus = "Not Running";
            }

            if (shuttingDown)
                break;

            Thread.Sleep(500);
        }
    } 

    public static void globalKeyPress(int key, bool shift = false, bool ctrl = false, bool alt = false)
    {
        if (pname.Length > 0)
            Invoke.PostMessage(pname[0].MainWindowHandle, WM_KEYDOWN, (IntPtr)key, (IntPtr)0);

        if (key != Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.LEFT) && 
            key != Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.RIGHT) && 
            key != Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.UP) && 
            key != Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.DOWN))
                Thread.Sleep(150); // prevent double press
    }

    public static void globalKeyRelease(int key)
    {
        if (pname.Length > 0)
            Invoke.PostMessage(pname[0].MainWindowHandle, WM_KEYUP, (IntPtr)key, (IntPtr)0);
    }

    /// <summary>
    /// Mouse click. x, y coords with button.
    /// </summary>
    /// <param name="x">cursor X position</param>
    /// <param name="y">cursor Y position</param>
    /// <param name="btn">Button to press. See MouseClicks enum.</param>
    public static void globalMouseClick(Invoke.MouseClicks btn, int x = 0, int y = 0)
    {
        var sim = new InputSimulator();
        if (btn == Invoke.MouseClicks.WM_LBUTTONDOWN)
            sim.Mouse.LeftButtonDown();
        else if (btn == Invoke.MouseClicks.WM_LBUTTONUP)
            sim.Mouse.LeftButtonUp();

        if (btn == Invoke.MouseClicks.WM_RBUTTONDBLCLK)
            sim.Mouse.RightButtonClick();

        if (btn == Invoke.MouseClicks.WM_MBUTTONDOWN)
            sim.Mouse.MiddleButtonDown();
        else if (btn == Invoke.MouseClicks.WM_MBUTTONUP)
            sim.Mouse.MiddleButtonUp();
        Thread.Sleep(150); // prevent double press
    }

    /// <summary>
    /// Process all controller button presses. This function loops endlessly.
    /// </summary>
    public static void processButtons()
    {
            if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceUp))
            {
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.CONTROL));
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_1));

                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.CONTROL));
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_1));
            }
            if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceRight))
            {
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.CONTROL));
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_2));

                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.CONTROL));
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_2));
            }
            if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceDown))
            {
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.CONTROL));
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_3));

                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.CONTROL));
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_3));
            }
            if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceLeft))
            {
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.CONTROL));
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_4));

                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.CONTROL));
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_4));
            }

            if (IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceUp))
            {
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_1));
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_1));
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceRight))
            {
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_2));
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_2));
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceDown))
            {
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_3));
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_3));
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceLeft))
            {
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_4));
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.VK_4));
            }

            if (IsGamepadButtonDown(gamepad, GamepadButton.LeftThumb) && holdingLJoy == false)
            {
                globalKeyPress(0xA0);
                globalKeyPress(0x10);
                Debug.WriteLine("holding shift");
                holdingLJoy = true;
            }
            else if (IsGamepadButtonUp(gamepad, GamepadButton.LeftThumb) && holdingLJoy == true)
            {
                globalKeyRelease(0xA0);
                globalKeyRelease(0x10);
                holdingLJoy = false;
            }

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
                globalMouseClick(Invoke.MouseClicks.WM_MBUTTONDOWN); // middle mouse btn
                mouseDistance = mouseDistanceDefault;
                holdingRT = true;
            }
            else if (IsGamepadButtonUp(gamepad, GamepadButton.RightTrigger2) && holdingRT == true) // release
            {
                globalMouseClick(Invoke.MouseClicks.WM_MBUTTONUP); // middle mouse btn
                mouseDistance = mouseDistanceDefault;
                holdingRT = false;
            }

            if (IsGamepadButtonPressed(gamepad, GamepadButton.MiddleRight))
            {
                globalKeyPress(0x79); // F10
            }

            // Not holding RB, RT, LB, LT and pressing buttons
            if (IsGamepadButtonUp(gamepad, GamepadButton.LeftTrigger1) && IsGamepadButtonUp(gamepad, GamepadButton.LeftTrigger2) && IsGamepadButtonUp(gamepad, GamepadButton.RightTrigger1) && IsGamepadButtonUp(gamepad, GamepadButton.RightTrigger2))
            {
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
                {
                    globalKeyPress(0x70); //F1
                }
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceRight))
                {
                    globalKeyPress(0x71); //F2
                }
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
                {
                    globalMouseClick(Invoke.MouseClicks.WM_RBUTTONDBLCLK); // right mouse btn
                }
                if (IsGamepadButtonDown(gamepad, GamepadButton.RightFaceDown) && holdingA == false)
                {
                    globalMouseClick(Invoke.MouseClicks.WM_LBUTTONDOWN);
                    holdingA = true;
                }
                else if (IsGamepadButtonUp(gamepad, GamepadButton.RightFaceDown) && holdingA == true)
                {
                    globalMouseClick(Invoke.MouseClicks.WM_LBUTTONUP); // l mouse click up
                    holdingA = false;
                }
            }
            // Holding RB down.
            else if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger1))
            {
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
                    globalKeyPress(0x51); //Q
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
                    globalKeyPress(0x57); //W
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
                    globalKeyPress(0x45); //E
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceRight))
                    globalKeyPress(0x52); //R
                if (IsGamepadButtonPressed(gamepad, GamepadButton.MiddleLeft))
                    globalKeyPress(0x54); //T
            }
            // Holding LB down.
            else if (IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger1))
            {
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
                    globalKeyPress(0x41); //A
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
                    globalKeyPress(0x53); //S
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
            {
                globalKeyPress(0x44); //D
                globalKeyPress(0x4B); //K // for sc1 (works for Terran, not Zerg. Need to find class offset and read value in memory)
                globalKeyPress(0x4C); //L // for sc1
            }
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceRight))
                    globalKeyPress(0x46); //F
                if (IsGamepadButtonPressed(gamepad, GamepadButton.MiddleLeft))
                    globalKeyPress(0x47); //G
            }
            // Holding LT down.
            else if (IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger2))
            {
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
                    globalKeyPress(0x5A); //Z
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
                    globalKeyPress(0x58); //X
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
                    globalKeyPress(0x43); //C
                if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceRight))
                    globalKeyPress(0x56); //V
                if (IsGamepadButtonPressed(gamepad, GamepadButton.MiddleLeft))
                    globalKeyPress(0x42); //B
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
            Invoke.GetCursorPos(ref cursorPos);

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

            Invoke.SetCursorPos(cursorPos.X, cursorPos.Y);
            //Debug.WriteLine("mX:" + mX + " mY:" + mY + " myTime:" + myTime); // DEBUG
        }

        // move camera
        if (GetGamepadAxisMovement(gamepad, GamepadAxis.RightY) < deadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.RightY) != 0.0 && !holdingRT)
        {
            holdingRJoyDirUp = true;
            if (holdingRT)
                globalKeyPress(0x21); //PGUP
            else
            {
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.UP));
                if (holdingRJoyDirDown)
                {
                    globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.DOWN));
                    holdingRJoyDirDown = false;
                }
            }
        }
        else if (GetGamepadAxisMovement(gamepad, GamepadAxis.RightY) > -deadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.RightY) != 0.0 && !holdingRT)
        {
            holdingRJoyDirDown = true;
            if (holdingRT)
                globalKeyPress(0x22); //PGDN
            else
            {
                if (holdingRJoyDirUp)
                {
                    globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.UP));
                    holdingRJoyDirUp = false;
                }
                globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.DOWN));
            }
        }
        else
        {
            if (holdingRJoyDirUp || holdingRJoyDirDown)
            {
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.UP));
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.DOWN));
                holdingRJoyDirUp = false;
                holdingRJoyDirDown = false;
            }
        }

        if (GetGamepadAxisMovement(gamepad, GamepadAxis.RightX) > deadzone && !holdingRT)
        {
            if (holdingRJoyDirLeft)
            {
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.LEFT));
                holdingRJoyDirLeft = false;
            }
            globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.RIGHT));
            holdingRJoyDirRight = true;
        }
        else if (GetGamepadAxisMovement(gamepad, GamepadAxis.RightX) < -deadzone && !holdingRT && !holdingRT)
        {
            globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.LEFT));
            if (holdingRJoyDirRight)
            {
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.RIGHT));
                holdingRJoyDirRight = false;
            }
            holdingRJoyDirLeft = true;
        }
        else
        {
            if (holdingRJoyDirRight || holdingRJoyDirLeft)
            {
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.LEFT));
                globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.RIGHT));
                holdingRJoyDirRight = false;
                holdingRJoyDirLeft = false;
            }
        }

        Thread.Sleep(10);
    }
}
