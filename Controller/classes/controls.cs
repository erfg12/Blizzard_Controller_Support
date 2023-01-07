using System.Diagnostics;
using System.Runtime.InteropServices;
using SharpDX.XInput;
using System.Threading;
using System;
using System.Drawing;
using WindowsInput;

namespace Blizzard_Controller.classes
{
    class controls
    {
        public static int deadzone = 3000;
        public static int mouseDistance = 12;
        public static int mouseDistanceDefault = 12;
        public static int faster = 30000; // increase mouse speed once it goes past this
        public static int slower = 15000;
        public static bool shuttingDown = false;

        public static string SC2ProcName = "SC2_x64";
        public static string WC3ProcName = "Warcraft III";
        public static string SC1ProcName = "StarCraft";

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
        public static Controller controller = null;

        public static InputSimulator sim = new InputSimulator();

        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;

        [DllImport("User32.Dll", EntryPoint = "PostMessageA", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        /// <summary>
        /// Check if game is running. If not, change public variable (gameProcStatus) to false to stop BGWorkers till a new game is launched.
        /// </summary>
        public static void CheckGameProc()
        {
            while (true)
            {
                if (Process.GetProcessesByName(SC2ProcName).Length > 0)
                {
                    pname = Process.GetProcessesByName(SC2ProcName);
                    gameProcStatus = "StarCraft 2 Running";
                }
                else if (Process.GetProcessesByName(SC1ProcName).Length > 0)
                {
                    pname = Process.GetProcessesByName(SC1ProcName);
                    gameProcStatus = "StarCraft: Remastered";
                }
                else if (Process.GetProcessesByName(WC3ProcName).Length > 0)
                {
                    pname = Process.GetProcessesByName(WC3ProcName);
                    gameProcStatus = "WarCraft III: Reforged";
                }
                else
                {
                    pname = null;
                    gameProcStatus = "Not Running";
                }

                if (shuttingDown)
                    break;

                Thread.Sleep(500);
                //Debug.WriteLine("checking for game (CheckGameProc)... " + gameProcStatus);
            }
        }

        public static void globalKeyPress(int key, bool shift = false, bool ctrl = false, bool alt = false)
        {
            //if (pname.Length > 0)
            //{
            //    if (shift) // gotta add ctrl, alt, etc.
            //        sim.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.SHIFT, (WindowsInput.Native.VirtualKeyCode)key);
            //    sim.Keyboard.KeyDown((WindowsInput.Native.VirtualKeyCode)key);
            //}
            if (pname.Length > 0)
                PostMessage(pname[0].MainWindowHandle, WM_KEYDOWN, (IntPtr)key, (IntPtr)0);

            if (key != Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.LEFT) && 
                key != Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.RIGHT) && 
                key != Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.UP) && 
                key != Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.DOWN))
                    Thread.Sleep(150); // prevent double press
        }

        public static void globalKeyRelease(int key)
        {
            if (pname.Length > 0)
                PostMessage(pname[0].MainWindowHandle, WM_KEYUP, (IntPtr)key, (IntPtr)0);
        }

        // referenced https://docs.microsoft.com/en-us/windows/win32/inputdev/mouse-input-notifications
        public enum MouseClicks
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_RBUTTONDBLCLK = 0x0206,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,
            WM_MBUTTONDBLCLK = 0x0209,
        }

        /// <summary>
        /// Mouse click. x, y coords with button.
        /// </summary>
        /// <param name="x">cursor X position</param>
        /// <param name="y">cursor Y position</param>
        /// <param name="btn">Button to press. See MouseClicks enum.</param>
        public static void globalMouseClick(MouseClicks btn, int x = 0, int y = 0)
        {
            var sim = new InputSimulator();
            if (btn == MouseClicks.WM_LBUTTONDOWN)
                sim.Mouse.LeftButtonDown();
            else if (btn == MouseClicks.WM_LBUTTONUP)
                sim.Mouse.LeftButtonUp();

            if (btn == MouseClicks.WM_RBUTTONDBLCLK)
                sim.Mouse.RightButtonClick();

            if (btn == MouseClicks.WM_MBUTTONDOWN)
                sim.Mouse.MiddleButtonDown();
            else if (btn == MouseClicks.WM_MBUTTONUP)
                sim.Mouse.MiddleButtonUp();
            Thread.Sleep(150); // prevent double press
        }

        /// <summary>
        /// Process all controller button presses. This function loops endlessly.
        /// </summary>
        public static void processButtons()
        {
            var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

            if (controllers == null)
                return;
            if (controllers.Length == 0)
                return;

            foreach (var selectControler in controllers)
            {
                if (selectControler == null)
                    return;

                if (selectControler.IsConnected)
                {
                    controller = selectControler;
                    break;
                }
            }

            if (controller != null)
            {
                State previousState;

                try
                {
                    previousState = controller.GetState();
                }
                catch
                {
                    controller = null;
                    return;
                }

                while (controller.IsConnected)
                {
                    if (shuttingDown)
                        break;
                    if (gameProcStatus.Equals("Not Running"))
                        continue;

                    State state;

                    try
                    {
                        state = controller.GetState();
                    }
                    catch
                    {
                        controller = null;
                        break;
                    }

                    //if (previousState.PacketNumber != state.PacketNumber)
                    //{
                    if ((state.Gamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0 && holdingLJoy == false)
                    {
                        globalKeyPress(0xA0);
                        globalKeyPress(0x10);
                        Debug.WriteLine("holding shift");
                        holdingLJoy = true;
                    }
                    else if ((state.Gamepad.Buttons & GamepadButtonFlags.LeftThumb) == 0 && holdingLJoy == true)
                    {
                        globalKeyRelease(0xA0);
                        globalKeyRelease(0x10);
                        holdingLJoy = false;
                    }

                    if ((state.Gamepad.Buttons & GamepadButtonFlags.RightThumb) != 0 && holdingRJoy == false)
                    {
                        holdingRJoy = true;
                    }
                    else if ((state.Gamepad.Buttons & GamepadButtonFlags.RightThumb) == 0 && holdingRJoy == true)
                    {
                        holdingRJoy = false;
                    }

                    if (state.Gamepad.RightTrigger == 255 && holdingRT == false) // hold down if not already
                    {
                        globalMouseClick(MouseClicks.WM_MBUTTONDOWN); //AutoItX.MouseDown("MIDDLE");
                        mouseDistance = mouseDistanceDefault * 3;
                        holdingRT = true;
                    }
                    else if (state.Gamepad.RightTrigger == 0 && holdingRT == true) // release
                    {
                        globalMouseClick(MouseClicks.WM_MBUTTONUP); //AutoItX.MouseUp("MIDDLE");
                        mouseDistance = mouseDistanceDefault;
                        holdingRT = false;
                    }

                    if ((state.Gamepad.Buttons & GamepadButtonFlags.Start) != 0)
                    {
                        globalKeyPress(0x79); //AutoItX.Send("{F10}");
                    }

                    // Not holding RB, RT, LB, LT and pressing buttons
                    if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
                    {
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.Y) != 0)
                        {
                            globalKeyPress(0x70); //F1
                        }
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.B) != 0)
                        {
                            globalKeyPress(0x71); //F2
                        }
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.X) != 0)
                        {
                            globalMouseClick(MouseClicks.WM_RBUTTONDBLCLK); //AutoItX.MouseClick("RIGHT");
                        }
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.A) != 0 && holdingA == false)
                        {
                            globalMouseClick(MouseClicks.WM_LBUTTONDOWN); //AutoItX.MouseDown();
                            holdingA = true;
                        }
                        else if ((state.Gamepad.Buttons & GamepadButtonFlags.A) == 0 && holdingA == true) // release
                        {
                            globalMouseClick(MouseClicks.WM_LBUTTONUP); //AutoItX.MouseUp();
                            holdingA = false;
                        }
                        //if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0)
                        //{
                        //    globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.DOWN));
                        //} else if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadDown) == 0)
                        //{
                        //    globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.DOWN));
                        //}
                        //if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0)
                        //{
                        //    globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.UP));
                        //} else if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadUp) == 0)
                        //{
                        //    globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.UP));
                        //}
                        //if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0)
                        //{
                        //    globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.LEFT));
                        //} else if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadLeft) == 0)
                        //{
                        //    globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.LEFT));
                        //}
                        //if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0)
                        //{
                        //    globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.RIGHT));
                        //} else if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadRight) == 0)
                        //{
                        //    globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.RIGHT));
                        //}
                    }
                    // Holding RB down.
                    else if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
                    {
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.A) != 0)
                            globalKeyPress(0x51); //AutoItX.Send("{Q}");
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.X) != 0)
                            globalKeyPress(0x57); //AutoItX.Send("{W}");
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.Y) != 0)
                            globalKeyPress(0x45); //AutoItX.Send("{E}");
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.B) != 0)
                            globalKeyPress(0x52); //AutoItX.Send("{R}");
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.Back) != 0)
                            globalKeyPress(0x54); //AutoItX.Send("{T}");
                    }
                    // Holding LB down.
                    else if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
                    {
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.A) != 0)
                            globalKeyPress(0x41); //AutoItX.Send("{A}");
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.X) != 0)
                            globalKeyPress(0x53); //AutoItX.Send("{S}");
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.Y) != 0)
                            globalKeyPress(0x44); //AutoItX.Send("{D}");
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.B) != 0)
                            globalKeyPress(0x46); //AutoItX.Send("{F}");
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.Back) != 0)
                            globalKeyPress(0x47); //AutoItX.Send("{G}");
                    }
                    // Holding LT down.
                    else if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 255 && state.Gamepad.RightTrigger == 0)
                    {
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.A) != 0)
                            globalKeyPress(0x5A); //AutoItX.Send("{Z}");
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.X) != 0)
                            globalKeyPress(0x58); //AutoItX.Send("{X}");
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.Y) != 0)
                            globalKeyPress(0x43); //AutoItX.Send("{C}");
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.B) != 0)
                            globalKeyPress(0x56); //AutoItX.Send("{V}");
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.Back) != 0)
                            globalKeyPress(0x42); //AutoItX.Send("{B}");
                    }
                    //}
                    Thread.Sleep(10);
                    previousState = state;
                }
            }
        }

        /// <summary>
        /// Process all controller joystick movements. This function loops endlessly.
        /// </summary>
        public static void processJoysticks()
        {
            var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

            if (controllers == null)
                return;
            if (controllers.Length == 0)
                return;

            foreach (var selectControler in controllers)
            {
                if (selectControler == null)
                    return;

                if (selectControler.IsConnected)
                {
                    controller = selectControler;
                    break;
                }
            }

            if (controller != null)
            {
                State previousState;

                try
                {
                    previousState = controller.GetState();
                }
                catch
                {
                    controller = null;
                    return;
                }

                while (controller.IsConnected)
                {
                    if (shuttingDown)
                        break;
                    if (gameProcStatus.Equals("Not Running"))
                        continue;
                    if (controller == null)
                        continue;

                    State state;

                    try
                    {
                        state = controller.GetState();
                    }
                    catch
                    {
                        controller = null;
                        break;
                    }

                    //if (previousState.PacketNumber != state.PacketNumber)
                    //{
                    //Debug.WriteLine(state.Gamepad); // DEBUG
                    if (state.Gamepad.LeftThumbX > deadzone || state.Gamepad.LeftThumbX < -deadzone || state.Gamepad.LeftThumbY > deadzone || state.Gamepad.LeftThumbY < -deadzone)
                    {
                        GetCursorPos(ref cursorPos);

                        if (Properties.Settings.Default.IncreaseCursorSpeed)
                        {

                            // left/right slower
                            if (state.Gamepad.LeftThumbX > deadzone)
                            {
                                if (state.Gamepad.LeftThumbX < faster && state.Gamepad.LeftThumbX < slower)
                                {
                                    cursorPos.X += mouseDistance / 2;
                                    //Debug.WriteLine("slower");
                                }
                            }
                            if (state.Gamepad.LeftThumbX < -deadzone)
                            {
                                if (state.Gamepad.LeftThumbX > -faster && state.Gamepad.LeftThumbX > -slower)
                                {
                                    cursorPos.X -= mouseDistance / 2;
                                    //Debug.WriteLine("slower");
                                }
                            }

                            // up/down slower
                            if (state.Gamepad.LeftThumbY > deadzone)
                            {
                                if (state.Gamepad.LeftThumbY < faster && state.Gamepad.LeftThumbY < slower)
                                {
                                    cursorPos.Y -= mouseDistance / 2;
                                    //Debug.WriteLine("slower");
                                }
                            }
                            if (state.Gamepad.LeftThumbY < -deadzone)
                            {
                                if (state.Gamepad.LeftThumbY > -faster && state.Gamepad.LeftThumbY > -slower)
                                {
                                    cursorPos.Y += mouseDistance / 2;
                                    //Debug.WriteLine("slower");
                                }
                            }

                            // left/right normal
                            if (state.Gamepad.LeftThumbX > deadzone)
                            {
                                if (state.Gamepad.LeftThumbX < faster && state.Gamepad.LeftThumbX > slower)
                                    cursorPos.X += mouseDistance;
                            }
                            if (state.Gamepad.LeftThumbX < -deadzone)
                            {
                                if (state.Gamepad.LeftThumbX > -faster && state.Gamepad.LeftThumbX < -slower)
                                    cursorPos.X -= mouseDistance;
                            }

                            // up/down normal
                            if (state.Gamepad.LeftThumbY > deadzone)
                            {
                                if (state.Gamepad.LeftThumbY < faster && state.Gamepad.LeftThumbY > slower)
                                    cursorPos.Y -= mouseDistance;
                            }
                            if (state.Gamepad.LeftThumbY < -deadzone)
                            {
                                if (state.Gamepad.LeftThumbY > -faster && state.Gamepad.LeftThumbY < -slower)
                                    cursorPos.Y += mouseDistance;
                            }

                            // left/right faster
                            if (state.Gamepad.LeftThumbX > deadzone && state.Gamepad.LeftThumbX > faster)
                                cursorPos.X += mouseDistance * 2;
                            if (state.Gamepad.LeftThumbX < -deadzone && state.Gamepad.LeftThumbX < -faster)
                                cursorPos.X -= mouseDistance * 2;

                            // up/down faster
                            if (state.Gamepad.LeftThumbY > deadzone && state.Gamepad.LeftThumbY > faster)
                                cursorPos.Y -= mouseDistance * 2;
                            if (state.Gamepad.LeftThumbY < -deadzone && state.Gamepad.LeftThumbY < -faster)
                                cursorPos.Y += mouseDistance * 2;
                        }
                        else
                        {
                            // left/right slower
                            if (state.Gamepad.LeftThumbX > deadzone) cursorPos.X += mouseDistance;
                            if (state.Gamepad.LeftThumbX < -deadzone) cursorPos.X -= mouseDistance;

                            // up/down slower
                            if (state.Gamepad.LeftThumbY > deadzone) cursorPos.Y -= mouseDistance;
                            if (state.Gamepad.LeftThumbY < -deadzone) cursorPos.Y += mouseDistance;
                        }

                        SetCursorPos(cursorPos.X, cursorPos.Y);
                        //Debug.WriteLine("mX:" + mX + " mY:" + mY + " myTime:" + myTime); // DEBUG
                    }


                    //if (state.Gamepad.RightThumbY > deadzone)
                    //    globalKeyPress(0x21); //AutoItX.Send("{PGUP}");
                    //else if (state.Gamepad.RightThumbY < -deadzone)
                    //    globalKeyPress(0x22); //AutoItX.Send("{PGDN}");

                    // move camera
                    if (state.Gamepad.RightThumbY > deadzone)
                    {
                        holdingRJoyDirUp = true;
                        if (holdingRT)
                            globalKeyPress(0x21); //AutoItX.Send("{PGUP}");
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
                    else if (state.Gamepad.RightThumbY < -deadzone)
                    {
                        holdingRJoyDirDown = true;
                        if (holdingRT)
                            globalKeyPress(0x22); //AutoItX.Send("{PGDN}");
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

                    if (state.Gamepad.RightThumbX > deadzone)
                    {
                        if (holdingRJoyDirLeft)
                        {
                            globalKeyRelease(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.LEFT));
                            holdingRJoyDirLeft = false;
                        }
                        globalKeyPress(Convert.ToInt32(WindowsInput.Native.VirtualKeyCode.RIGHT));
                        holdingRJoyDirRight = true;
                    }
                    else if (state.Gamepad.RightThumbX < -deadzone)
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

                    System.Threading.Thread.Sleep(10);
                    previousState = state;
                }
            }
        }
    }
}
