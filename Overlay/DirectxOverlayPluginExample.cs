using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Overlay.NET.Common;
using Overlay.NET.Demo.Internals;
using Overlay.NET.Directx;
using Process.NET.Windows;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using SharpDX.XInput;

namespace Overlay.NET.Demo.Directx {
    [RegisterPlugin("DirectXverlayDemo-1", "Jacob Kemple", "DirectXOverlayDemo", "0.0",
        "A basic demo of the DirectXoverlay.")]
    public class DirectxOverlayPluginExample : DirectXOverlayPlugin {
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, out Rect lpRect);

        private readonly TickEngine _tickEngine = new TickEngine();
        public readonly ISettings<DemoOverlaySettings> Settings = new SerializableSettings<DemoOverlaySettings>();
        private int _displayFps;
        private int _font, _font_small;
        private int _hugeFont;
        private int _i;
        private int _interiorBrush;
        private int _redBrush, _black, _gray, _red, _green, _blue;
        public static SharpDX.Direct2D1.Bitmap aBtnImg;
        public static SharpDX.Direct2D1.Bitmap xBtnImg, yBtnImg, bBtnImg, backBtnImg, rBtnImg, lBtnImg, ltBtnImg;
        private int _redOpacityBrush;
        private float _rotation;
        private Stopwatch _watch;
        public static Controller controller = null;

        public override void Initialize(IWindow targetWindow) {
            // Set target window by calling the base method
            base.Initialize(targetWindow);

            // For demo, show how to use settings
            var current = Settings.Current;
            var type = GetType();

            if (current.UpdateRate == 0)
                current.UpdateRate = 1000 / 60;

            current.Author = GetAuthor(type);
            current.Description = GetDescription(type);
            current.Identifier = GetIdentifier(type);
            current.Name = GetName(type);
            current.Version = GetVersion(type);

            // File is made from above info
            Settings.Save();
            Settings.Load();
            Console.Title = @"Overlay";

            OverlayWindow = new DirectXOverlayWindow(targetWindow.Handle, false);
            _watch = Stopwatch.StartNew();

            
            _redOpacityBrush = OverlayWindow.Graphics.CreateBrush(Color.FromArgb(80, 255, 0, 0));
            _interiorBrush = OverlayWindow.Graphics.CreateBrush(0x7FFFFF00);

            _hugeFont = OverlayWindow.Graphics.CreateFont("Arial", 50, true);

            _font = OverlayWindow.Graphics.CreateFont("Arial", 32);
            _font_small = OverlayWindow.Graphics.CreateFont("Arial", 16);
            _black = OverlayWindow.Graphics.CreateBrush(0x7F000000);
            _gray = OverlayWindow.Graphics.CreateBrush(0x7FCCCCCC);
            _red = OverlayWindow.Graphics.CreateBrush(0x7FFF0000);
            _green = OverlayWindow.Graphics.CreateBrush(0x7F008000);
            _blue = OverlayWindow.Graphics.CreateBrush(0x7F0000FF);

            aBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/a_btn.png");
            xBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/x_btn.png");
            yBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/y_btn.png");
            bBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/b_btn.png");
            backBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/back_btn.png");
            lBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/lb_btn.png");
            ltBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/lt_btn.png");
            rBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/rb_btn.png");

            //aBtnImg = Direct2DRenderer.LoadFromFile(Properties.Resources.a_btn.ToString());
            /*xBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.x_btn));
            yBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.y_btn));
            bBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.b_btn));
            backBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.back_btn));
            lBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.lb_btn));
            ltBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.lt_btn));
            rBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.rb_btn));*/

            _rotation = 0.0f;
            _displayFps = 0;
            _i = 0;
            // Set up update interval and register events for the tick engine.

            _tickEngine.PreTick += OnPreTick;
            _tickEngine.Tick += OnTick;
        }

        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        /// <summary>
		/// Draws an image to the given position and optional applies an alpha value.
		/// </summary>
		/// <param name="image">The Image to be drawn.</param>
		/// <param name="x">The x-coordinate upper-left corner of the image.</param>
		/// <param name="y">The y-coordinate upper-left corner of the image.</param>
		/// <param name="opacity">A value indicating the opacity of the image. (alpha)</param>
		public void DrawImage(SharpDX.Direct2D1.Bitmap image, float x, float y, float opacity = 1.0f)
        {
            float destRight = x + image.PixelSize.Width;
            float destBottom = y + image.PixelSize.Height;

            Direct2DRenderer._device.DrawBitmap(
                image,
                new RawRectangleF(x, y, destRight, destBottom),
                opacity,
                BitmapInterpolationMode.Linear);
        }

        private void OnTick(object sender, EventArgs e) {
            if (!OverlayWindow.IsVisible) {
                return;
            }

            OverlayWindow.Update();

            InternalRender();
        }

        private void OnPreTick(object sender, EventArgs e) {
            var targetWindowIsActivated = TargetWindow.IsActivated;
            if (!targetWindowIsActivated && OverlayWindow.IsVisible) {
                _watch.Stop();
                ClearScreen();
                OverlayWindow.Hide();
            }
            else if (targetWindowIsActivated && !OverlayWindow.IsVisible) {
                OverlayWindow.Show();
            }
        }

        // ReSharper disable once RedundantOverriddenMember
        public override void Enable() {
            _tickEngine.Interval = Settings.Current.UpdateRate.Milliseconds();
            _tickEngine.IsTicking = true;
            base.Enable();
        }

        // ReSharper disable once RedundantOverriddenMember
        public override void Disable() {
            _tickEngine.IsTicking = false;
            base.Disable();
        }

        public override void Update() => _tickEngine.Pulse();

        protected void InternalRender() {
            if (!_watch.IsRunning) {
                _watch.Start();
            }

            OverlayWindow.Graphics.BeginScene();
            OverlayWindow.Graphics.ClearScene();

            double wM = 1920.0 / OverlayWindow.Width;
            double hM = 1080.0 / OverlayWindow.Height;

            var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

            if (controllers != null && controllers.Length > 0)
            {
                foreach (var selectControler in controllers)
                {
                    if (selectControler == null)
                        break;

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
                    }

                    if (controller.IsConnected)
                    {
                        State state;

                        try
                        {
                            state = controller.GetState();
                        }
                        catch
                        {
                            controller = null;
                            return;
                        }

                        if (GenerateOverlayWindow.SC2Proc != null)
                        {
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
                            {
                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - 380, OverlayWindow.Height - 230, 350, 68, 2, _green);
                            }
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
                            {
                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - 380, OverlayWindow.Height - 160, 350, 68, 2, _green);
                            }
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 255 && state.Gamepad.RightTrigger == 0)
                            {
                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - 380, OverlayWindow.Height - 94, 350, 68, 2, _green);
                            }
                        }
                        else if (GenerateOverlayWindow.SC1Proc != null)
                        {
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
                            {
                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - 306, OverlayWindow.Height - 280, 290, 85, 2, _green);
                            }
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
                            {
                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - 306, OverlayWindow.Height - 190, 290, 85, 2, _green);
                            }
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 255 && state.Gamepad.RightTrigger == 0)
                            {
                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - 306, OverlayWindow.Height - 100, 290, 85, 2, _green);
                            }
                        }
                        else if (GenerateOverlayWindow.WC3Proc != null)
                        {
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
                            {
                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - 570, OverlayWindow.Height - 240, 305, 75, 2, _green);
                            }
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
                            {
                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - 570, OverlayWindow.Height - 163, 305, 75, 2, _green);
                            }
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 255 && state.Gamepad.RightTrigger == 0)
                            {
                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - 570, OverlayWindow.Height - 85, 305, 75, 2, _green);
                            }
                        }
                    }
                }
            }

            if (GenerateOverlayWindow.SC2Proc != null)
            {
                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(380 / wM), OverlayWindow.Height - Convert.ToInt32(230 / hM), Convert.ToInt32(350 / wM), Convert.ToInt32(205 / hM), 2, _redBrush);

                DrawImage(aBtnImg, OverlayWindow.Width - 380, OverlayWindow.Height - 300);
                DrawImage(bBtnImg, OverlayWindow.Width - 170, OverlayWindow.Height - 300);
                DrawImage(xBtnImg, OverlayWindow.Width - 310, OverlayWindow.Height - 300);
                DrawImage(yBtnImg, OverlayWindow.Width - 240, OverlayWindow.Height - 300);
                DrawImage(backBtnImg, OverlayWindow.Width - 100, OverlayWindow.Height - 300);

                DrawImage(lBtnImg, OverlayWindow.Width - 480, OverlayWindow.Height - 150);
                DrawImage(rBtnImg, OverlayWindow.Width - 480, OverlayWindow.Height - 220);
                DrawImage(ltBtnImg, OverlayWindow.Width - 450, OverlayWindow.Height - 100);
            }
            else if (GenerateOverlayWindow.SC1Proc != null)
            {
                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(306 / wM), OverlayWindow.Height - Convert.ToInt32(275 / hM), Convert.ToInt32(290 / wM), Convert.ToInt32(260 / hM), 2, _redBrush);

                DrawImage(aBtnImg, OverlayWindow.Width - 305, OverlayWindow.Height - 350);
                DrawImage(bBtnImg, OverlayWindow.Width - 200, OverlayWindow.Height - 350);
                //DrawImage(xBtnImg, OverlayWindow.Width - 290, OverlayWindow.Height - 300);
                DrawImage(yBtnImg, OverlayWindow.Width - 90, OverlayWindow.Height - 350);

                DrawImage(lBtnImg, OverlayWindow.Width - 400, OverlayWindow.Height - 170);
                DrawImage(rBtnImg, OverlayWindow.Width - 400, OverlayWindow.Height - 260);
                DrawImage(ltBtnImg, OverlayWindow.Width - 365, OverlayWindow.Height - 100);
            }
            else if (GenerateOverlayWindow.WC3Proc != null)
            {
                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(570 / wM), OverlayWindow.Height - Convert.ToInt32(240 / hM), Convert.ToInt32(305 / wM), Convert.ToInt32(230 / hM), 2, _redBrush);

                DrawImage(aBtnImg, OverlayWindow.Width - 570, OverlayWindow.Height - 310);
                DrawImage(bBtnImg, OverlayWindow.Width - 340, OverlayWindow.Height - 310);
                DrawImage(xBtnImg, OverlayWindow.Width - 490, OverlayWindow.Height - 310);
                DrawImage(yBtnImg, OverlayWindow.Width - 415, OverlayWindow.Height - 310);

                DrawImage(lBtnImg, OverlayWindow.Width - 660, OverlayWindow.Height - 150);
                DrawImage(rBtnImg, OverlayWindow.Width - 660, OverlayWindow.Height - 220);
                DrawImage(ltBtnImg, OverlayWindow.Width - 630, OverlayWindow.Height - 100);
            }

            if (_watch.ElapsedMilliseconds > 1000)
            {
                _i = _displayFps;
                _displayFps = 0;
                _watch.Restart();
            }

            OverlayWindow.Graphics.EndScene();
        }

        public override void Dispose() {
            OverlayWindow.Dispose();
            base.Dispose();
        }

        private void ClearScreen() {
            OverlayWindow.Graphics.BeginScene();
            OverlayWindow.Graphics.ClearScene();
            OverlayWindow.Graphics.EndScene();
        }
    }
}