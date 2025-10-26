using SharpHook;
using SharpHook.Native;
using System.Drawing;
using Blizzard_Controller.Platform;
using PlatformKeyCode = Blizzard_Controller.Platform.KeyCode;
using SharpHookKeyCode = SharpHook.Data.KeyCode;

namespace Blizzard_Controller.Input;

/// <summary>
/// Handles low-level input simulation (mouse/keyboard events)
/// </summary>
public class InputSimulator
{
    private static readonly EventSimulator simulator = new();

    /// <summary>
    /// Simulates mouse movement to the specified position
    /// </summary>
    public static void SimulateMouseMovement(Point position)
    {
        simulator.SimulateMouseMovement((short)position.X, (short)position.Y);
    }

    /// <summary>
    /// Simulates a relative mouse movement (delta) using SharpHook's event simulator.
    /// SharpHook's simulator is capable of emitting relative mouse movements; this method
    /// provides a clear semantic wrapper so callers don't need to read or set absolute positions.
    /// </summary>
    public static void SimulateMouseMoveRelative(int dx, int dy)
    {
        // EventSimulator's SimulateMouseMovement accepts short values. We treat these
        // as deltas for relative movement. If SharpHook interprets them as absolute
        // on a particular platform, replace this with the platform-specific relative API.
        simulator.SimulateMouseMovement((short)dx, (short)dy);
    }

    /// <summary>
    /// Simulates mouse clicks using the platform's mouse click definitions
    /// </summary>
    public static void SimulateMouseClick(PlatformInvoke.MouseClicks btn)
    {
        switch (btn)
        {
            case PlatformInvoke.MouseClicks.left_down:
                simulator.SimulateMousePress(MouseButton.Button1);
                break;
            case PlatformInvoke.MouseClicks.left_up:
                simulator.SimulateMouseRelease(MouseButton.Button1);
                break;
            case PlatformInvoke.MouseClicks.left_BLCLK:
                simulator.SimulateMousePress(MouseButton.Button1);
                simulator.SimulateMouseRelease(MouseButton.Button1);
                break;
                
            case PlatformInvoke.MouseClicks.right_down:
                simulator.SimulateMousePress(MouseButton.Button3);
                break;
            case PlatformInvoke.MouseClicks.right_up:
                simulator.SimulateMouseRelease(MouseButton.Button3);
                break;
            case PlatformInvoke.MouseClicks.right_BLCLK:
                simulator.SimulateMousePress(MouseButton.Button3);
                simulator.SimulateMouseRelease(MouseButton.Button3);
                break;
                
            case PlatformInvoke.MouseClicks.middle_down:
                simulator.SimulateMousePress(MouseButton.Button2);
                break;
            case PlatformInvoke.MouseClicks.middle_up:
                simulator.SimulateMouseRelease(MouseButton.Button2);
                break;
            case PlatformInvoke.MouseClicks.middle_BLCLK:
                simulator.SimulateMousePress(MouseButton.Button2);
                simulator.SimulateMouseRelease(MouseButton.Button2);
                break;
        }
        Thread.Sleep(50); // Small delay to prevent accidental double-clicks
    }

    /// <summary>
    /// Simulates a key press
    /// </summary>
    public static void SimulateKeyPress(PlatformKeyCode key)
    {
        simulator.SimulateKeyPress(ConvertKeyCode(key));
    }

    /// <summary>
    /// Simulates a key release
    /// </summary>
    public static void SimulateKeyRelease(PlatformKeyCode key)
    {
        simulator.SimulateKeyRelease(ConvertKeyCode(key));
    }

    /// <summary>
    /// Simulates pressing a key while holding a modifier key (e.g., Ctrl+A)
    /// </summary>
    public static void SimulateModifiedKeyPress(PlatformKeyCode modifier, PlatformKeyCode key)
    {
        var sharpHookModifier = ConvertKeyCode(modifier);
        var sharpHookKey = ConvertKeyCode(key);

        simulator.SimulateKeyPress(sharpHookModifier);
        simulator.SimulateKeyPress(sharpHookKey);
        simulator.SimulateKeyRelease(sharpHookKey);
        simulator.SimulateKeyRelease(sharpHookModifier);
    }

    /// <summary>
    /// Converts our platform KeyCode to SharpHook's KeyCode
    /// </summary>
    private static SharpHookKeyCode ConvertKeyCode(PlatformKeyCode key)
    {
        // Our KeyCodes match Windows virtual key codes, which SharpHook also uses
        return (SharpHookKeyCode)(int)key;
    }
}