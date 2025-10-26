using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Blizzard_Controller.Configuration;

// MonoGame exposes GamePad APIs on all supported runtimes (Windows, Linux, macOS).
// The platform compatibility analyzer (CA1416) can report false positives because
// it looks for OS-specific annotations; suppress CA1416 here for the MonoGame calls.
#pragma warning disable CA1416

namespace Blizzard_Controller.Input;

/// <summary>
/// Cross-platform controller state backed by MonoGame's GamePad API.
/// MonoGame exposes the same GamePad API across supported platforms, so
/// we can use it directly here instead of splitting per-OS.
/// </summary>
public class ControllerState
{
        public static bool IsGamepadConnected()
        {
                try
                {
                        // Check all four possible player indices - treat connected if any controller is connected
                        bool connected = false;
                        for (int i = 0; i < 4; i++)
                        {
                                var state = GamePad.GetState((PlayerIndex)i);
                                if (state.IsConnected)
                                {
                                        connected = true;
                                        break;
                                }
                        }

                        // Best-effort: update the UI label about controller connection status
                        try
                        {
                                AppSettings.Instance.UpdateControllerStatus(connected);
                        }
                        catch { }

                        return connected;
                }
                catch
                {
                        // If GamePad is unavailable or not initialized, treat as disconnected
                        try { AppSettings.Instance.UpdateControllerStatus(false); } catch { }
                        return false;
                }
    }

    public static float GetGamepadAxisMovement(int gamepadIndex, GamepadAxis axis)
        {
                try
                {
                        var player = (PlayerIndex)Math.Clamp(gamepadIndex, 0, 3);
                        var state = GamePad.GetState(player);

                        return axis switch
                        {
                                GamepadAxis.LeftX => state.ThumbSticks.Left.X,
                                GamepadAxis.LeftY => state.ThumbSticks.Left.Y,
                                GamepadAxis.RightX => state.ThumbSticks.Right.X,
                                GamepadAxis.RightY => state.ThumbSticks.Right.Y,
                                _ => 0f
                        };
                }
                catch
                {
                        // If GamePad is unavailable or not initialized, return 0
                        return 0f;
                }
        }

        public static bool IsGamepadButtonDown(int gamepadIndex, GamepadButton b)
        {
                try
                {
                        var player = (PlayerIndex)Math.Clamp(gamepadIndex, 0, 3);
                        var state = GamePad.GetState(player);
                        var pressed = ButtonState.Pressed;

                        return b switch
                        {
                                GamepadButton.LeftFaceUp => state.Buttons.Y == pressed,
                                GamepadButton.LeftFaceRight => state.Buttons.B == pressed,
                                GamepadButton.LeftFaceDown => state.Buttons.A == pressed,
                                GamepadButton.LeftFaceLeft => state.Buttons.X == pressed,
                                GamepadButton.RightFaceUp => state.Buttons.Y == pressed,
                                GamepadButton.RightFaceRight => state.Buttons.B == pressed,
                                GamepadButton.RightFaceDown => state.Buttons.A == pressed,
                                GamepadButton.RightFaceLeft => state.Buttons.X == pressed,
                                GamepadButton.MiddleLeft => state.Buttons.LeftStick == pressed,
                                GamepadButton.MiddleRight => state.Buttons.RightStick == pressed,
                                GamepadButton.LeftThumb => state.Buttons.LeftStick == pressed,
                                GamepadButton.RightThumb => state.Buttons.RightStick == pressed,
                                GamepadButton.LeftTrigger1 => state.Buttons.LeftShoulder == pressed,
                                GamepadButton.LeftTrigger2 => state.Triggers.Left > 0.5f,
                                GamepadButton.RightTrigger1 => state.Buttons.RightShoulder == pressed,
                                GamepadButton.RightTrigger2 => state.Triggers.Right > 0.5f,
                                _ => false
                        };
                }
                catch
                {
                        return false;
                }
        }

        public static bool IsGamepadButtonUp(int gamepadIndex, GamepadButton b)
        {
                return !IsGamepadButtonDown(gamepadIndex, b);
        }

        public static bool IsGamepadButtonPressed(int gamepadIndex, GamepadButton b)
        {
                // Edge detection would require tracking previous states; treat pressed==down for now
                return IsGamepadButtonDown(gamepadIndex, b);
        }
}

        #pragma warning restore CA1416
