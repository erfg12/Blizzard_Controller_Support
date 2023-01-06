using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpDX.XInput;
using System.Drawing;
using Overlay.NET;
using Process.NET;
using System.Linq;
using Overlay.NET.Demo.Directx;
using Process.NET.Memory;
using System.Globalization;

namespace Overlay
{
    class GenerateOverlayWindow
    {
        #region init_vars
        public static System.Diagnostics.Process[] pname = null;
        public static Controller controller = null;

        private Rectangle resolution = Screen.PrimaryScreen.Bounds;

        public static string SC2ProcName = "SC2_x64";
        public static string WC3ProcName = "Warcraft III";
        public static string SC1ProcName = "StarCraft";
        public static System.Diagnostics.Process SC2Proc, SC1Proc, WC3Proc;
        functions f = new functions();

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out System.Drawing.Rectangle rect);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        private OverlayPlugin _directXoverlayPluginExample;
        private ProcessSharp _processSharp;
        #endregion

        public void Initialize()
        {
            Console.WriteLine($"Starting overlay...");
            SC2Proc = System.Diagnostics.Process.GetProcessesByName(SC2ProcName).FirstOrDefault();
            SC1Proc = System.Diagnostics.Process.GetProcessesByName(SC1ProcName).FirstOrDefault();
            WC3Proc = System.Diagnostics.Process.GetProcessesByName(WC3ProcName).FirstOrDefault();
            while (SC2Proc == null && SC1Proc == null && WC3Proc == null)
            {
                Console.WriteLine($"No processes by the names of {SC1ProcName}, {SC2ProcName} or {WC3ProcName} were found.");
                System.Threading.Thread.Sleep(1000);
                SC2Proc = System.Diagnostics.Process.GetProcessesByName(SC2ProcName).FirstOrDefault();
                SC1Proc = System.Diagnostics.Process.GetProcessesByName(SC1ProcName).FirstOrDefault();
                WC3Proc = System.Diagnostics.Process.GetProcessesByName(WC3ProcName).FirstOrDefault();
            }
            Console.WriteLine($"Starting overlay in 5...");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine($"Starting overlay in 4...");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine($"Starting overlay in 3...");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine($"Starting overlay in 2...");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine($"Starting overlay in 1...");
            System.Threading.Thread.Sleep(1000);
            _directXoverlayPluginExample = new DirectxOverlayPluginExample();
            if (SC2Proc != null)
            {
                _processSharp = new ProcessSharp(SC2Proc, MemoryType.Remote);
            } else if (SC1Proc != null)
            {
                _processSharp = new ProcessSharp(SC1Proc, MemoryType.Remote);
            } else if (WC3Proc != null)
            {
                _processSharp = new ProcessSharp(WC3Proc, MemoryType.Remote);
            }
            var fpsValid = int.TryParse(Convert.ToString("60", CultureInfo.InvariantCulture), NumberStyles.Any,
                NumberFormatInfo.InvariantInfo, out int fps);
            var d3DOverlay = (DirectxOverlayPluginExample)_directXoverlayPluginExample;
            d3DOverlay.Settings.Current.UpdateRate = 1000 / fps;
            _directXoverlayPluginExample.Initialize(_processSharp.WindowFactory.MainWindow);
            _directXoverlayPluginExample.Enable();
            var info = d3DOverlay.Settings.Current;
            Console.WriteLine($"Overlay Started.");
            while (true)
            {
                _directXoverlayPluginExample.Update();

                // check if game died.
                SC2Proc = System.Diagnostics.Process.GetProcessesByName(SC2ProcName).FirstOrDefault();
                SC1Proc = System.Diagnostics.Process.GetProcessesByName(SC1ProcName).FirstOrDefault();
                WC3Proc = System.Diagnostics.Process.GetProcessesByName(WC3ProcName).FirstOrDefault();
                if (SC2Proc == null && SC1Proc == null && WC3Proc == null)
                {
                    Console.WriteLine("Game has exited...");
                    Initialize();
                    break;
                }
            }
        }
    }
}
