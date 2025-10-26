using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Blizzard_Controller.Configuration;
using Blizzard_Controller.UI.Overlay;

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
                // to-do program the buttons
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
