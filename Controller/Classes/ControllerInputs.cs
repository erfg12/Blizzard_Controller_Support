using SharpHook;
using SharpHook.Data;
using SharpHook.Native;

namespace Blizzard_Controller;
public class ControllerInputs
{
    // Settings that can be updated by the UI
    public static double deadzone = 0.05;
    public static int mouseDistance = 10;
    public static int mouseDistanceDefault = 10;
    public static double faster = 0.8; // increase mouse speed once it goes past this
    public static double slower = 0.4;
    public static bool shuttingDown = false;

    public static Process[] pname = null;

    public static Point cursorPos = new Point(0,0);

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

    public static EventSimulator simulator = new EventSimulator();

    /// <summary>
    /// Check if controller is connected and update the shared settings
    /// </summary>
    public static void CheckControllerStatus()
    {
        while (!shuttingDown)
        {
            bool isConnected = IsGamepadAvailable(gamepad);
            
            if (controller != isConnected)
            {
                controller = isConnected;
                AppSettings.Instance.UpdateControllerStatus(isConnected);
            }
            
            Thread.Sleep(1000); // Check every second
        }
    }

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
    /// Updates the shared AppSettings instance to keep UI synchronized.
    /// </summary>
    public static void CheckGameProc()
    {
        while (!shuttingDown)
        {
            string newGameStatus;
            
            if (Process.GetProcessesByName(GameSettings.ProcessNames.SC2ProcName).Length > 0)
            {
                pname = Process.GetProcessesByName(GameSettings.ProcessNames.SC2ProcName);
                newGameStatus = "StarCraft 2 Running";
            }
            else if (Process.GetProcessesByName(GameSettings.ProcessNames.SC1ProcName).Length > 0)
            {
                pname = Process.GetProcessesByName(GameSettings.ProcessNames.SC1ProcName);
                newGameStatus = "StarCraft: Remastered";
            }
            else if (Process.GetProcessesByName(GameSettings.ProcessNames.WC3ProcName).Length > 0)
            {
                pname = Process.GetProcessesByName(GameSettings.ProcessNames.WC3ProcName);
                newGameStatus = "WarCraft III: Reforged";
            }
            else if (Process.GetProcessesByName(GameSettings.ProcessNames.WC1ProcName).Length > 0)
            {
                pname = Process.GetProcessesByName(GameSettings.ProcessNames.WC1ProcName);
                //if (pname == null) continue;
                newGameStatus = "WarCraft I: Remastered";
                if (IsWindowedMode(pname.First().MainWindowHandle)) // maximize windowed mode
                    MaximizeWindow(pname.First().MainWindowHandle);
            }
            else if (Process.GetProcessesByName(GameSettings.ProcessNames.WC2ProcName).Length > 0)
            {
                pname = Process.GetProcessesByName(GameSettings.ProcessNames.WC2ProcName);
                //if (pname == null) continue;
                newGameStatus = "WarCraft II: Remastered";
                if (IsWindowedMode(pname.First().MainWindowHandle)) // maximize windowed mode
                    MaximizeWindow(pname.First().MainWindowHandle);
            }
            else
            {
                pname = null;
                newGameStatus = "Not Running";
            }

            // Update both the static variable and the shared settings
            if (gameProcStatus != newGameStatus)
            {
                gameProcStatus = newGameStatus;
                AppSettings.Instance.UpdateGameStatus(newGameStatus);
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
        if (btn == Invoke.MouseClicks.left_down)
            simulator.SimulateMousePress(MouseButton.Button1);
        else if (btn == Invoke.MouseClicks.left_up)
            simulator.SimulateMouseRelease(MouseButton.Button1);

        if (btn == Invoke.MouseClicks.right_BLCLK)
        {
            simulator.SimulateMousePress(MouseButton.Button3);
            simulator.SimulateMouseRelease(MouseButton.Button3);
        }

        if (btn == Invoke.MouseClicks.middle_down)
            simulator.SimulateMousePress(MouseButton.Button2);
        else if (btn == Invoke.MouseClicks.middle_up)
            simulator.SimulateMouseRelease(MouseButton.Button2);
        Thread.Sleep(150); // prevent double press
    }

    /// <summary>
    /// Process all controller button presses. This function loops endlessly.
    /// </summary>
    public static void processButtons()
    {
        if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceUp))
        {
            simulator.SimulateKeyPress(KeyCode.VcLeftControl);
            simulator.SimulateKeyPress(KeyCode.Vc1);

            simulator.SimulateKeyRelease(KeyCode.Vc1);
            simulator.SimulateKeyRelease(KeyCode.VcLeftControl);
        }
        if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceRight))
        {
            simulator.SimulateKeyPress(KeyCode.VcLeftControl);
            simulator.SimulateKeyPress(KeyCode.Vc2);

            simulator.SimulateKeyRelease(KeyCode.Vc2);
            simulator.SimulateKeyRelease(KeyCode.VcLeftControl);
        }
        if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceDown))
        {
            simulator.SimulateKeyPress(KeyCode.VcLeftControl);
            simulator.SimulateKeyPress(KeyCode.Vc3);

            simulator.SimulateKeyRelease(KeyCode.Vc3);
            simulator.SimulateKeyRelease(KeyCode.VcLeftControl);
        }
        if (IsGamepadButtonDown(gamepad, GamepadButton.RightTrigger2) && IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceLeft))
        {
            simulator.SimulateKeyPress(KeyCode.VcLeftControl);
            simulator.SimulateKeyPress(KeyCode.Vc4);

            simulator.SimulateKeyRelease(KeyCode.Vc4);
            simulator.SimulateKeyRelease(KeyCode.VcLeftControl);
        }

        if (IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceUp))
        {
            simulator.SimulateKeyPress(KeyCode.Vc1);
            simulator.SimulateKeyRelease(KeyCode.Vc1);
        }
        if (IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceRight))
        {
            simulator.SimulateKeyPress(KeyCode.Vc2);
            simulator.SimulateKeyRelease(KeyCode.Vc2);
        }
        if (IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceDown))
        {
            simulator.SimulateKeyPress(KeyCode.Vc3);
            simulator.SimulateKeyRelease(KeyCode.Vc3);
        }
        if (IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceLeft))
        {
            simulator.SimulateKeyPress(KeyCode.Vc4);
            simulator.SimulateKeyRelease(KeyCode.Vc4);
        }

        if (IsGamepadButtonDown(gamepad, GamepadButton.LeftThumb) && holdingLJoy == false)
        {
            simulator.SimulateKeyPress(KeyCode.VcLeftShift);
            holdingLJoy = true;
        }
        else if (IsGamepadButtonUp(gamepad, GamepadButton.LeftThumb) && holdingLJoy == true)
        {
            simulator.SimulateKeyRelease(KeyCode.VcLeftShift);
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
            globalMouseClick(Invoke.MouseClicks.middle_down); // middle mouse btn
            //mouseDistance = mouseDistanceDefault;
            holdingRT = true;
        }
        else if (IsGamepadButtonUp(gamepad, GamepadButton.RightTrigger2) && holdingRT == true) // release
        {
            globalMouseClick(Invoke.MouseClicks.middle_up); // middle mouse btn
            //mouseDistance = mouseDistanceDefault;
            holdingRT = false;
        }

        if (IsGamepadButtonPressed(gamepad, GamepadButton.MiddleRight))
        {
            simulator.SimulateKeyPress(KeyCode.VcF10);
            simulator.SimulateKeyRelease(KeyCode.VcF10);
        }

        // Not holding RB, RT, LB, LT and pressing buttons
        if (IsGamepadButtonUp(gamepad, GamepadButton.LeftTrigger1) && IsGamepadButtonUp(gamepad, GamepadButton.LeftTrigger2) && IsGamepadButtonUp(gamepad, GamepadButton.RightTrigger1) && IsGamepadButtonUp(gamepad, GamepadButton.RightTrigger2))
        {
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
            {
                simulator.SimulateKeyPress(KeyCode.VcF1);
                simulator.SimulateKeyRelease(KeyCode.VcF1);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceRight))
            {
                simulator.SimulateKeyPress(KeyCode.VcF2);
                simulator.SimulateKeyRelease(KeyCode.VcF2);
            }
            //if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
            //{
            //    globalMouseClick(Invoke.MouseClicks.right_BLCLK); // right mouse btn
            //}
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
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
            {
                simulator.SimulateKeyPress(KeyCode.VcQ);
                simulator.SimulateKeyRelease(KeyCode.VcQ);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
            {
                simulator.SimulateKeyPress(KeyCode.VcW);
                simulator.SimulateKeyRelease(KeyCode.VcW);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
            {
                simulator.SimulateKeyPress(KeyCode.VcE);
                simulator.SimulateKeyRelease(KeyCode.VcE);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceRight))
            {
                simulator.SimulateKeyPress(KeyCode.VcR);
                simulator.SimulateKeyRelease(KeyCode.VcR);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.MiddleLeft))
            {
                simulator.SimulateKeyPress(KeyCode.VcT);
                simulator.SimulateKeyRelease(KeyCode.VcT);
            }
        }
        // Holding LB down.
        else if (IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger1))
        {
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
            {
                simulator.SimulateKeyPress(KeyCode.VcA);
                simulator.SimulateKeyRelease(KeyCode.VcA);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
            {
                //aix3c.Send("{s}"); //S
                simulator.SimulateKeyPress(KeyCode.VcS);
                simulator.SimulateKeyRelease(KeyCode.VcD);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
            {
                simulator.SimulateKeyPress(KeyCode.VcD);
                simulator.SimulateKeyRelease(KeyCode.VcD);
                simulator.SimulateKeyPress(KeyCode.VcK); // K for SC1 (works for Terran, not Zerg. Need to find class offset and read value in memory)
                simulator.SimulateKeyRelease(KeyCode.VcK);
                simulator.SimulateKeyPress(KeyCode.VcL); // L for SC1
                simulator.SimulateKeyRelease(KeyCode.VcL);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceRight))
            {
                simulator.SimulateKeyPress(KeyCode.VcF);
                simulator.SimulateKeyRelease(KeyCode.VcF);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.MiddleLeft))
            {
                simulator.SimulateKeyPress(KeyCode.VcG);
                simulator.SimulateKeyRelease(KeyCode.VcG);
            }
        }
        // Holding LT down.
        else if (IsGamepadButtonDown(gamepad, GamepadButton.LeftTrigger2))
        {
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
            {
                simulator.SimulateKeyPress(KeyCode.VcZ);
                simulator.SimulateKeyRelease(KeyCode.VcZ);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
            {
                simulator.SimulateKeyPress(KeyCode.VcX);
                simulator.SimulateKeyRelease(KeyCode.VcX);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
            {
                simulator.SimulateKeyPress(KeyCode.VcC);
                simulator.SimulateKeyRelease(KeyCode.VcC);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceRight))
            {
                simulator.SimulateKeyPress(KeyCode.VcV);
                simulator.SimulateKeyRelease(KeyCode.VcV);
            }
            if (IsGamepadButtonPressed(gamepad, GamepadButton.MiddleLeft))
            {
                simulator.SimulateKeyPress(KeyCode.VcB);
                simulator.SimulateKeyRelease(KeyCode.VcB);
            }
        }
        Thread.Sleep(10);
    }

    /// <summary>
    /// Process all controller joystick movements. This function loops endlessly.
    /// Uses the shared AppSettings for deadzone and cursor speed values.
    /// </summary>
    public static void processJoysticks()
    {
        // Use the shared settings for deadzone (with fallback to static variable for compatibility)
        double currentDeadzone = AppSettings.Instance.Deadzone;
        
        if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > -currentDeadzone || GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < currentDeadzone
            || GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > -currentDeadzone || GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < currentDeadzone)
        {
            if (AppSettings.Instance.VariableCursorSpeed)
            {
                // Use the shared settings for cursor speed
                int currentCursorSpeed = AppSettings.Instance.CursorSpeed;
                
                // left/right slower
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > currentDeadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > faster)
                    {
                        cursorPos.X += currentCursorSpeed / 2;
                    }
                }
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -currentDeadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > -faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > -slower)
                    {
                        cursorPos.X -= currentCursorSpeed / 2;
                    }
                }

                // up/down slower
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > currentDeadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < slower)
                    {
                        cursorPos.Y += currentCursorSpeed / 2;
                    }
                }
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -currentDeadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > -faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > -slower)
                    {
                        cursorPos.Y -= currentCursorSpeed / 2;
                    }
                }

                // left/right normal
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > currentDeadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > slower)
                        cursorPos.X += currentCursorSpeed;
                }
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -currentDeadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > -faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -slower)
                        cursorPos.X -= currentCursorSpeed;
                }

                // up/down normal
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > currentDeadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > slower)
                        cursorPos.Y += currentCursorSpeed;
                }
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -currentDeadzone)
                {
                    if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > -faster && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -slower)
                        cursorPos.Y -= currentCursorSpeed;
                }

                // left/right faster
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > currentDeadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > faster)
                    cursorPos.X += currentCursorSpeed * 2;
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -currentDeadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -faster)
                    cursorPos.X -= currentCursorSpeed * 2;

                // up/down faster
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > currentDeadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > faster)
                    cursorPos.Y += currentCursorSpeed * 2;
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -currentDeadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -faster)
                    cursorPos.Y -= currentCursorSpeed * 2;
            }
            else
            {
                // Use the shared settings for cursor speed
                int currentCursorSpeed = AppSettings.Instance.CursorSpeed;
                
                // left/right slower
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > currentDeadzone) cursorPos.X += currentCursorSpeed;
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < -currentDeadzone) cursorPos.X -= currentCursorSpeed;

                // up/down slower
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > currentDeadzone) cursorPos.Y += currentCursorSpeed;
                if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < -currentDeadzone) cursorPos.Y -= currentCursorSpeed;
            }

            // if joystick is moving, move the cursor
            if (GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) > currentDeadzone || GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX) < currentDeadzone ||
                GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) > currentDeadzone || GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY) < currentDeadzone)
            {
                simulator.SimulateMouseMovement((short)cursorPos.X, (short)cursorPos.Y);
                //Debug.WriteLine("mX:" + mX + " mY:" + mY + " myTime:" + myTime); // DEBUG
            }

        }

        // move camera
        // Right Joystick UP -1
        if (GetGamepadAxisMovement(gamepad, GamepadAxis.RightY) < -currentDeadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.RightY) != 0.0 && !holdingRT)
        {
            holdingRJoyDirUp = true;
            if (holdingRT)
            {
                simulator.SimulateKeyPress(KeyCode.VcPageUp); //PGUP
                simulator.SimulateKeyRelease(KeyCode.VcPageUp);
            }
            else
            {
                simulator.SimulateKeyPress(KeyCode.VcUp);
                if (holdingRJoyDirDown)
                {
                    simulator.SimulateKeyRelease(KeyCode.VcDown);
                    holdingRJoyDirDown = false;
                }
            }
        }
        // Right Joystick DOWN 1
        else if (GetGamepadAxisMovement(gamepad, GamepadAxis.RightY) > currentDeadzone && GetGamepadAxisMovement(gamepad, GamepadAxis.RightY) != 0.0 && !holdingRT)
        {
            holdingRJoyDirDown = true;
            if (holdingRT)
            {
                simulator.SimulateKeyPress(KeyCode.VcPageDown); //PGDN
                simulator.SimulateKeyRelease(KeyCode.VcPageDown);
            }
            else
            {
                if (holdingRJoyDirUp)
                {
                    simulator.SimulateKeyRelease(KeyCode.VcUp);
                    holdingRJoyDirUp = false;
                }
                simulator.SimulateKeyPress(KeyCode.VcDown);
            }
        }
        else
        {
            if (holdingRJoyDirUp || holdingRJoyDirDown)
            {
                simulator.SimulateKeyRelease(KeyCode.VcUp);
                simulator.SimulateKeyRelease(KeyCode.VcDown);
                holdingRJoyDirUp = false;
                holdingRJoyDirDown = false;
            }
        }

        if (GetGamepadAxisMovement(gamepad, GamepadAxis.RightX) > currentDeadzone && !holdingRT)
        {
            if (holdingRJoyDirLeft)
            {
                simulator.SimulateKeyRelease(KeyCode.VcLeft);
                holdingRJoyDirLeft = false;
            }
            simulator.SimulateKeyPress(KeyCode.VcRight);
            holdingRJoyDirRight = true;
        }
        else if (GetGamepadAxisMovement(gamepad, GamepadAxis.RightX) < -currentDeadzone && !holdingRT && !holdingRT)
        {
            simulator.SimulateKeyPress(KeyCode.VcLeft);
            if (holdingRJoyDirRight)
            {
                simulator.SimulateKeyRelease(KeyCode.VcRight);
                holdingRJoyDirRight = false;
            }
            holdingRJoyDirLeft = true;
        }
        else
        {
            if (holdingRJoyDirRight || holdingRJoyDirLeft)
            {
                simulator.SimulateKeyRelease(KeyCode.VcLeft);
                simulator.SimulateKeyRelease(KeyCode.VcRight);
                holdingRJoyDirRight = false;
                holdingRJoyDirLeft = false;
            }
        }

        Thread.Sleep(10);
    }
}
