using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Blizzard_Controller.Configuration;
using Blizzard_Controller.UI.Overlay;
using Blizzard_Controller.Platform;
using Blizzard_Controller.Input;

namespace Blizzard_Controller.Input
{
    public static class ControllerInputs
    {
        // Backwards-compatible public static fields
        public static double deadzone = 0.1;
        public static int mouseDistance = 20;
        public static int mouseDistanceDefault = 20;
        public static bool shuttingDown = false;

        public static async Task CheckGameProc()
        {
            while (!shuttingDown)
            {
                try
                {
                    // Use GameSettings to get platform-appropriate process names
                    var sc2 = OverlayWindow.GetProcess(GameSettings.ProcessNames.SC2ProcName);
                    var sc1 = OverlayWindow.GetProcess(GameSettings.ProcessNames.SC1ProcName);
                    var wc3 = OverlayWindow.GetProcess(GameSettings.ProcessNames.WC3ProcName);
                    var wc1 = OverlayWindow.GetProcess(GameSettings.ProcessNames.WC1ProcName);
                    var wc2 = OverlayWindow.GetProcess(GameSettings.ProcessNames.WC2ProcName);

                    // Update OverlayWindow static fields (shared with overlay rendering code)
                    OverlayWindow.SC2Proc = sc2;
                    OverlayWindow.SC1Proc = sc1;
                    OverlayWindow.WC3Proc = wc3;
                    OverlayWindow.WC1Proc = wc1;
                    OverlayWindow.WC2Proc = wc2;

                    // Update the UI-facing game status label if AppSettings is available
                    string status = "Not Running";
                    if (sc2 != null) status = "StarCraft II";
                    else if (sc1 != null) status = "StarCraft";
                    else if (wc3 != null) status = "Warcraft III";
                    else if (wc1 != null) status = "Warcraft I";
                    else if (wc2 != null) status = "Warcraft II";

                    try
                    {
                        AppSettings.Instance.UpdateGameStatus(status);
                    }
                    catch { /* best-effort update; ignore if AppSettings unavailable */ }
                }
                catch { }

                await Task.Delay(1000);
            }
        }

        // Track button states to detect press and release
        private static readonly System.Collections.Generic.Dictionary<string, bool> _lastButtonState = new();

        public static void processButtons()
        {
            // Verify controller is connected before processing
            if (!ControllerState.IsGamepadConnected())
            {
                _lastButtonState.Clear(); // Reset state if controller disconnected
                return;
            }

            // Only process player 0 by default (primary controller)
            int player = 0;
            try
            {
                var holdingLJoy = _lastButtonState.GetValueOrDefault("LJoy");
                var holdingRJoy = _lastButtonState.GetValueOrDefault("RJoy");
                var holdingRT = _lastButtonState.GetValueOrDefault("RT");
                var holdingA = _lastButtonState.GetValueOrDefault("A");

                // Control + Number combinations
                if (ControllerState.IsGamepadButtonDown(player, GamepadButton.RightTrigger2))
                {
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.LeftFaceUp))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcLeftControl);
                        InputSimulator.SimulateKeyPress(KeyCode.Vc1);
                        InputSimulator.SimulateKeyRelease(KeyCode.Vc1);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcLeftControl);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.LeftFaceRight))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcLeftControl);
                        InputSimulator.SimulateKeyPress(KeyCode.Vc2);
                        InputSimulator.SimulateKeyRelease(KeyCode.Vc2);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcLeftControl);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.LeftFaceDown))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcLeftControl);
                        InputSimulator.SimulateKeyPress(KeyCode.Vc3);
                        InputSimulator.SimulateKeyRelease(KeyCode.Vc3);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcLeftControl);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.LeftFaceLeft))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcLeftControl);
                        InputSimulator.SimulateKeyPress(KeyCode.Vc4);
                        InputSimulator.SimulateKeyRelease(KeyCode.Vc4);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcLeftControl);
                    }
                }

                // Number keys
                if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.LeftFaceUp))
                {
                    InputSimulator.SimulateKeyPress(KeyCode.Vc1);
                    InputSimulator.SimulateKeyRelease(KeyCode.Vc1);
                }
                if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.LeftFaceRight))
                {
                    InputSimulator.SimulateKeyPress(KeyCode.Vc2);
                    InputSimulator.SimulateKeyRelease(KeyCode.Vc2);
                }
                if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.LeftFaceDown))
                {
                    InputSimulator.SimulateKeyPress(KeyCode.Vc3);
                    InputSimulator.SimulateKeyRelease(KeyCode.Vc3);
                }
                if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.LeftFaceLeft))
                {
                    InputSimulator.SimulateKeyPress(KeyCode.Vc4);
                    InputSimulator.SimulateKeyRelease(KeyCode.Vc4);
                }

                // Thumb stick buttons
                if (ControllerState.IsGamepadButtonDown(player, GamepadButton.LeftThumb) && !holdingLJoy)
                {
                    InputSimulator.SimulateKeyPress(KeyCode.VcLeftShift);
                    _lastButtonState["LJoy"] = true;
                }
                else if (ControllerState.IsGamepadButtonUp(player, GamepadButton.LeftThumb) && holdingLJoy)
                {
                    InputSimulator.SimulateKeyRelease(KeyCode.VcLeftShift);
                    _lastButtonState["LJoy"] = false;
                }

                if (ControllerState.IsGamepadButtonDown(player, GamepadButton.RightThumb) && !holdingRJoy)
                {
                    _lastButtonState["RJoy"] = true;
                }
                else if (ControllerState.IsGamepadButtonUp(player, GamepadButton.RightThumb) && holdingRJoy)
                {
                    _lastButtonState["RJoy"] = false;
                }

                // Right Trigger (middle mouse)
                if (ControllerState.IsGamepadButtonDown(player, GamepadButton.RightTrigger2) && !holdingRT)
                {
                    InputSimulator.SimulateMouseClick(PlatformInvoke.MouseClicks.middle_down);
                    _lastButtonState["RT"] = true;
                }
                else if (ControllerState.IsGamepadButtonUp(player, GamepadButton.RightTrigger2) && holdingRT)
                {
                    InputSimulator.SimulateMouseClick(PlatformInvoke.MouseClicks.middle_up);
                    _lastButtonState["RT"] = false;
                }

                // Start button (F10)
                if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.MiddleRight))
                {
                    InputSimulator.SimulateKeyPress(KeyCode.VcF10);
                    InputSimulator.SimulateKeyRelease(KeyCode.VcF10);
                }

                // No trigger buttons held
                if (ControllerState.IsGamepadButtonUp(player, GamepadButton.LeftTrigger1) && 
                    ControllerState.IsGamepadButtonUp(player, GamepadButton.LeftTrigger2) && 
                    ControllerState.IsGamepadButtonUp(player, GamepadButton.RightTrigger1) && 
                    ControllerState.IsGamepadButtonUp(player, GamepadButton.RightTrigger2))
                {
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceUp))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcF1);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcF1);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceRight))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcF2);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcF2);
                    }
                    if (ControllerState.IsGamepadButtonDown(player, GamepadButton.RightFaceDown) && !holdingA)
                    {
                        InputSimulator.SimulateMouseClick(PlatformInvoke.MouseClicks.left_down);
                        _lastButtonState["A"] = true;
                    }
                    else if (ControllerState.IsGamepadButtonUp(player, GamepadButton.RightFaceDown) && holdingA)
                    {
                        InputSimulator.SimulateMouseClick(PlatformInvoke.MouseClicks.left_up);
                        _lastButtonState["A"] = false;
                    }
                }
                // RB held down
                else if (ControllerState.IsGamepadButtonDown(player, GamepadButton.RightTrigger1))
                {
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceDown))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcQ);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcQ);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceLeft))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcW);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcW);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceUp))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcE);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcE);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceRight))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcR);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcR);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.MiddleLeft))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcT);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcT);
                    }
                }
                // LB held down
                else if (ControllerState.IsGamepadButtonDown(player, GamepadButton.LeftTrigger1))
                {
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceDown))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcA);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcA);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceLeft))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcS);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcD);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceUp))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcD);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcD);
                        InputSimulator.SimulateKeyPress(KeyCode.VcK);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcK);
                        InputSimulator.SimulateKeyPress(KeyCode.VcL);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcL);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceRight))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcF);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcF);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.MiddleLeft))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcG);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcG);
                    }
                }
                // LT held down
                else if (ControllerState.IsGamepadButtonDown(player, GamepadButton.LeftTrigger2))
                {
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceDown))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcZ);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcZ);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceLeft))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcX);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcX);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceUp))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcC);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcC);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.RightFaceRight))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcV);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcV);
                    }
                    if (ControllerState.IsGamepadButtonPressed(player, GamepadButton.MiddleLeft))
                    {
                        InputSimulator.SimulateKeyPress(KeyCode.VcB);
                        InputSimulator.SimulateKeyRelease(KeyCode.VcB);
                    }
                }

                Thread.Sleep(10);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error processing buttons: {ex.Message}");
            }
        }

        public static void processJoysticks()
        {
            int player = 0;
            try
            {
                // To-Do: program this
            }
            catch { }
        }
    }
}
