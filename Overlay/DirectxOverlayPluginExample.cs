//using System;
//using System.Diagnostics;
//using System.Drawing;
//using System.Runtime.InteropServices;
//using Process.NET.Windows;
//using DXNET.Mathematics.Interop;
//using DXNET.XInput;

//namespace Overlay.NET.Demo.Directx
//{
//    public class DirectxOverlayPluginExample : DirectXOverlayPlugin
//    {
//        [DllImport("user32.dll")]
//        public static extern int GetWindowRect(IntPtr hwnd, out Rect lpRect);

//        private readonly TickEngine _tickEngine = new TickEngine();
//        public readonly ISettings<DemoOverlaySettings> Settings = new SerializableSettings<DemoOverlaySettings>();
//        private int _displayFps;
//        private int _font, _font_small;
//        private int _hugeFont;
//        private int _i;
//        private int _interiorBrush;
//        private int _redBrush, _black, _gray, _red, _green, _blue;
//        public static DXNET.Direct2D1.Bitmap aBtnImg;
//        public static DXNET.Direct2D1.Bitmap xBtnImg, yBtnImg, bBtnImg, backBtnImg, rBtnImg, lBtnImg, ltBtnImg;
//        private int _redOpacityBrush;
//        private float _rotation;
//        private Stopwatch _watch;
//        public static Controller controller = null;

//        public override void Initialize(IWindow targetWindow)
//        {
//            // Set target window by calling the base method
//            base.Initialize(targetWindow);

//            // For demo, show how to use settings
//            var current = Settings.Current;
//            var type = GetType();

//            if (current.UpdateRate == 0)
//                current.UpdateRate = 1000 / 60;

//            current.Author = GetAuthor(type);
//            current.Description = GetDescription(type);
//            current.Identifier = GetIdentifier(type);
//            current.Name = GetName(type);
//            current.Version = GetVersion(type);

//            // File is made from above info
//            Settings.Save();
//            Settings.Load();
//            Console.Title = @"Overlay";

//            OverlayWindow = new DirectXOverlayWindow(targetWindow.Handle, false);
//            _watch = Stopwatch.StartNew();


//            _redOpacityBrush = OverlayWindow.Graphics.CreateBrush(Color.FromArgb(80, 255, 0, 0));
//            _interiorBrush = OverlayWindow.Graphics.CreateBrush(0x7FFFFF00);

//            _hugeFont = OverlayWindow.Graphics.CreateFont("Arial", 50, true);

//            _font = OverlayWindow.Graphics.CreateFont("Arial", 32);
//            _font_small = OverlayWindow.Graphics.CreateFont("Arial", 16);
//            _black = OverlayWindow.Graphics.CreateBrush(0x7F000000);
//            _gray = OverlayWindow.Graphics.CreateBrush(0x7FCCCCCC);
//            _red = OverlayWindow.Graphics.CreateBrush(0x7FFF0000);
//            _green = OverlayWindow.Graphics.CreateBrush(0x7F008000);
//            _blue = OverlayWindow.Graphics.CreateBrush(0x7F0000FF);

//            aBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/a_btn.png");
//            xBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/x_btn.png");
//            yBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/y_btn.png");
//            bBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/b_btn.png");
//            backBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/back_btn.png");
//            lBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/lb_btn.png");
//            ltBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/lt_btn.png");
//            rBtnImg = Direct2DRenderer.LoadFromFile(Direct2DRenderer._device, "Resources/rb_btn.png");

//            //aBtnImg = Direct2DRenderer.LoadFromFile(Properties.Resources.a_btn.ToString());
//            /*xBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.x_btn));
//            yBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.y_btn));
//            bBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.b_btn));
//            backBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.back_btn));
//            lBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.lb_btn));
//            ltBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.lt_btn));
//            rBtnImg = _graphics.CreateImage(f.pngToBytes(Properties.Resources.rb_btn));*/

//            _rotation = 0.0f;
//            _displayFps = 0;
//            _i = 0;
//            // Set up update interval and register events for the tick engine.

//            _tickEngine.PreTick += OnPreTick;
//            _tickEngine.Tick += OnTick;
//        }

//        public struct Rect
//        {
//            public int Left;
//            public int Top;
//            public int Right;
//            public int Bottom;
//        }

//        /// <summary>
//		/// Draws an image to the given position and optional applies an alpha value.
//		/// </summary>
//		/// <param name="image">The Image to be drawn.</param>
//		/// <param name="x">The x-coordinate upper-left corner of the image.</param>
//		/// <param name="y">The y-coordinate upper-left corner of the image.</param>
//		/// <param name="opacity">A value indicating the opacity of the image. (alpha)</param>
//		public void DrawImage(DXNET.Direct2D1.Bitmap image, float x, float y, float width = 0, float height = 0, float opacity = 1.0f)
//        {
//            float destRight = x + image.PixelSize.Width;
//            if (width != 0)
//                destRight = x + width;
//            float destBottom = y + image.PixelSize.Height;
//            if (height != 0)
//                destBottom = y + height;

//            Direct2DRenderer._device.DrawBitmap(
//                image,
//                new RawRectangleF(x, y, destRight, destBottom),
//                opacity,
//                BitmapInterpolationMode.Linear);
//        }

//        private void OnTick(object sender, EventArgs e)
//        {
//            if (!OverlayWindow.IsVisible)
//            {
//                return;
//            }

//            OverlayWindow.Update();

//            InternalRender();
//        }

//        private void OnPreTick(object sender, EventArgs e)
//        {
//            var targetWindowIsActivated = TargetWindow.IsActivated;
//            if (!targetWindowIsActivated && OverlayWindow.IsVisible)
//            {
//                _watch.Stop();
//                ClearScreen();
//                OverlayWindow.Hide();
//            }
//            else if (targetWindowIsActivated && !OverlayWindow.IsVisible)
//            {
//                OverlayWindow.Show();
//            }
//        }

//        // ReSharper disable once RedundantOverriddenMember
//        public override void Enable()
//        {
//            _tickEngine.Interval = Settings.Current.UpdateRate.Milliseconds();
//            _tickEngine.IsTicking = true;
//            base.Enable();
//        }

//        // ReSharper disable once RedundantOverriddenMember
//        public override void Disable()
//        {
//            _tickEngine.IsTicking = false;
//            base.Disable();
//        }

//        public override void Update() => _tickEngine.Pulse();

//        protected void InternalRender()
//        {
//            if (!_watch.IsRunning)
//            {
//                _watch.Start();
//            }

//            OverlayWindow.Graphics.BeginScene();
//            OverlayWindow.Graphics.ClearScene();

//            double wM = 1920.0 / OverlayWindow.Width;
//            double hM = 1080.0 / OverlayWindow.Height;

//            //Debug.WriteLine($"PrimaryScreenWidth:{System.Windows.SystemParameters.PrimaryScreenWidth} PrimaryScreenHeight:{System.Windows.SystemParameters.PrimaryScreenHeight}");

//            var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

//            if (controllers != null && controllers.Length > 0)
//            {
//                foreach (var selectControler in controllers)
//                {
//                    if (selectControler == null)
//                        break;

//                    if (selectControler.IsConnected)
//                    {
//                        controller = selectControler;
//                        break;
//                    }
//                }

//                if (controller != null)
//                {
//                    State previousState;

//                    try
//                    {
//                        previousState = controller.GetState();
//                    }
//                    catch
//                    {
//                        controller = null;
//                    }

//                    if (controller.IsConnected)
//                    {
//                        State state;

//                        try
//                        {
//                            state = controller.GetState();
//                        }
//                        catch
//                        {
//                            controller = null;
//                            return;
//                        }

//                        if (GenerateOverlayWindow.SC2Proc != null)
//                        {
//                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
//                            {
//                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(380 / wM), OverlayWindow.Height - Convert.ToInt32(230 / hM), Convert.ToInt32(350 / wM), Convert.ToInt32(68 / hM), 2, _green);
//                            }
//                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
//                            {
//                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(380 / wM), OverlayWindow.Height - Convert.ToInt32(160 / hM), Convert.ToInt32(350 / wM), Convert.ToInt32(68 / hM), 2, _green);
//                            }
//                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 255 && state.Gamepad.RightTrigger == 0)
//                            {
//                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(380 / wM), OverlayWindow.Height - Convert.ToInt32(94 / hM), Convert.ToInt32(350 / wM), Convert.ToInt32(68 / hM), 2, _green);
//                            }
//                        }
//                        else if (GenerateOverlayWindow.SC1Proc != null)
//                        {
//                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
//                            {
//                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(306 / wM), OverlayWindow.Height - Convert.ToInt32(280 / hM), Convert.ToInt32(290 / wM), Convert.ToInt32(85 / hM), 2, _green);
//                            }
//                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
//                            {
//                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(306 / wM), OverlayWindow.Height - Convert.ToInt32(190 / hM), Convert.ToInt32(290 / wM), Convert.ToInt32(85 / hM), 2, _green);
//                            }
//                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 255 && state.Gamepad.RightTrigger == 0)
//                            {
//                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(306 / wM), OverlayWindow.Height - Convert.ToInt32(100 / hM), Convert.ToInt32(290 / wM), Convert.ToInt32(85 / hM), 2, _green);
//                            }
//                        }
//                        else if (GenerateOverlayWindow.WC3Proc != null)
//                        {
//                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
//                            {
//                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(570 / wM), OverlayWindow.Height - Convert.ToInt32(240 / hM), Convert.ToInt32(305 / wM), Convert.ToInt32(75 / hM), 2, _green);
//                            }
//                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0 && state.Gamepad.LeftTrigger == 0 && state.Gamepad.RightTrigger == 0)
//                            {
//                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(570 / wM), OverlayWindow.Height - Convert.ToInt32(163 / hM), Convert.ToInt32(305 / wM), Convert.ToInt32(75 / hM), 2, _green);
//                            }
//                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0 && (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0 && state.Gamepad.LeftTrigger == 255 && state.Gamepad.RightTrigger == 0)
//                            {
//                                OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(570 / wM), OverlayWindow.Height - Convert.ToInt32(85 / hM), Convert.ToInt32(305 / wM), Convert.ToInt32(75 / hM), 2, _green);
//                            }
//                        }
//                    }
//                }
//            }

//            float transparency = 0.6f;

//            if (GenerateOverlayWindow.SC2Proc != null)
//            {
//                //OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(380 / wM), OverlayWindow.Height - Convert.ToInt32(230 / hM), Convert.ToInt32(350 / wM), Convert.ToInt32(205 / hM), 2, _redBrush);

//                DrawImage(aBtnImg, OverlayWindow.Width - Convert.ToInt32(380 / wM), OverlayWindow.Height - Convert.ToInt32(300 / hM), Convert.ToSingle(aBtnImg.PixelSize.Width / wM), Convert.ToSingle(aBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(bBtnImg, OverlayWindow.Width - Convert.ToInt32(170 / wM), OverlayWindow.Height - Convert.ToInt32(300 / hM), Convert.ToSingle(bBtnImg.PixelSize.Width / wM), Convert.ToSingle(bBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(xBtnImg, OverlayWindow.Width - Convert.ToInt32(310 / wM), OverlayWindow.Height - Convert.ToInt32(300 / hM), Convert.ToSingle(xBtnImg.PixelSize.Width / wM), Convert.ToSingle(xBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(yBtnImg, OverlayWindow.Width - Convert.ToInt32(240 / wM), OverlayWindow.Height - Convert.ToInt32(300 / hM), Convert.ToSingle(yBtnImg.PixelSize.Width / wM), Convert.ToSingle(yBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(backBtnImg, OverlayWindow.Width - Convert.ToInt32(100 / wM), OverlayWindow.Height - Convert.ToInt32(300 / hM), Convert.ToSingle(backBtnImg.PixelSize.Width / wM), Convert.ToSingle(backBtnImg.PixelSize.Height / hM), transparency);

//                DrawImage(lBtnImg, OverlayWindow.Width - Convert.ToInt32(480 / wM), OverlayWindow.Height - Convert.ToInt32(150 / hM), Convert.ToSingle(lBtnImg.PixelSize.Width / wM), Convert.ToSingle(lBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(rBtnImg, OverlayWindow.Width - Convert.ToInt32(480 / wM), OverlayWindow.Height - Convert.ToInt32(220 / hM), Convert.ToSingle(rBtnImg.PixelSize.Width / wM), Convert.ToSingle(rBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(ltBtnImg, OverlayWindow.Width - Convert.ToInt32(450 / wM), OverlayWindow.Height - Convert.ToInt32(100 / hM), Convert.ToSingle(ltBtnImg.PixelSize.Width / wM), Convert.ToSingle(ltBtnImg.PixelSize.Height / hM), transparency);
//            }
//            else if (GenerateOverlayWindow.SC1Proc != null)
//            {
//                //OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(306 / wM), OverlayWindow.Height - Convert.ToInt32(275 / hM), Convert.ToInt32(290 / wM), Convert.ToInt32(260 / hM), 2, _redBrush);

//                DrawImage(aBtnImg, OverlayWindow.Width - Convert.ToInt32(305 / wM), OverlayWindow.Height - Convert.ToInt32(350 / hM), Convert.ToSingle(aBtnImg.PixelSize.Width / wM), Convert.ToSingle(aBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(bBtnImg, OverlayWindow.Width - Convert.ToInt32(200 / wM), OverlayWindow.Height - Convert.ToInt32(350 / hM), Convert.ToSingle(bBtnImg.PixelSize.Width / wM), Convert.ToSingle(bBtnImg.PixelSize.Height / hM), transparency);
//                //DrawImage(xBtnImg, OverlayWindow.Width - 290, OverlayWindow.Height - 300);
//                DrawImage(yBtnImg, OverlayWindow.Width - Convert.ToInt32(90 / wM), OverlayWindow.Height - Convert.ToInt32(350 / hM), Convert.ToSingle(yBtnImg.PixelSize.Width / wM), Convert.ToSingle(yBtnImg.PixelSize.Height / hM), transparency);

//                DrawImage(lBtnImg, OverlayWindow.Width - Convert.ToInt32(400 / wM), OverlayWindow.Height - Convert.ToInt32(170 / hM), Convert.ToSingle(lBtnImg.PixelSize.Width / wM), Convert.ToSingle(lBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(rBtnImg, OverlayWindow.Width - Convert.ToInt32(400 / wM), OverlayWindow.Height - Convert.ToInt32(260 / hM), Convert.ToSingle(rBtnImg.PixelSize.Width / wM), Convert.ToSingle(rBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(ltBtnImg, OverlayWindow.Width - Convert.ToInt32(365 / wM), OverlayWindow.Height - Convert.ToInt32(100 / hM), Convert.ToSingle(ltBtnImg.PixelSize.Width / wM), Convert.ToSingle(ltBtnImg.PixelSize.Height / hM), transparency);
//            }
//            else if (GenerateOverlayWindow.WC3Proc != null)
//            {
//                //OverlayWindow.Graphics.DrawRectangle(OverlayWindow.Width - Convert.ToInt32(570 / wM), OverlayWindow.Height - Convert.ToInt32(240 / hM), Convert.ToInt32(305 / wM), Convert.ToInt32(230 / hM), 2, _redBrush);

//                DrawImage(aBtnImg, OverlayWindow.Width - Convert.ToInt32(570 / wM), OverlayWindow.Height - Convert.ToInt32(310 / hM), Convert.ToSingle(aBtnImg.PixelSize.Width / wM), Convert.ToSingle(aBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(bBtnImg, OverlayWindow.Width - Convert.ToInt32(340 / wM), OverlayWindow.Height - Convert.ToInt32(310 / hM), Convert.ToSingle(bBtnImg.PixelSize.Width / wM), Convert.ToSingle(bBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(xBtnImg, OverlayWindow.Width - Convert.ToInt32(490 / wM), OverlayWindow.Height - Convert.ToInt32(310 / hM), Convert.ToSingle(xBtnImg.PixelSize.Width / wM), Convert.ToSingle(xBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(yBtnImg, OverlayWindow.Width - Convert.ToInt32(415 / wM), OverlayWindow.Height - Convert.ToInt32(310 / hM), Convert.ToSingle(yBtnImg.PixelSize.Width / wM), Convert.ToSingle(yBtnImg.PixelSize.Height / hM), transparency);

//                DrawImage(lBtnImg, OverlayWindow.Width - Convert.ToInt32(660 / wM), OverlayWindow.Height - Convert.ToInt32(150 / hM), Convert.ToSingle(lBtnImg.PixelSize.Width / wM), Convert.ToSingle(lBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(rBtnImg, OverlayWindow.Width - Convert.ToInt32(660 / wM), OverlayWindow.Height - Convert.ToInt32(220 / hM), Convert.ToSingle(rBtnImg.PixelSize.Width / wM), Convert.ToSingle(rBtnImg.PixelSize.Height / hM), transparency);
//                DrawImage(ltBtnImg, OverlayWindow.Width - Convert.ToInt32(630 / wM), OverlayWindow.Height - Convert.ToInt32(100 / hM), Convert.ToSingle(ltBtnImg.PixelSize.Width / wM), Convert.ToSingle(ltBtnImg.PixelSize.Height / hM), transparency);
//            }

//            if (_watch.ElapsedMilliseconds > 1000)
//            {
//                _i = _displayFps;
//                _displayFps = 0;
//                _watch.Restart();
//            }

//            OverlayWindow.Graphics.EndScene();
//        }

//        public override void Dispose()
//        {
//            OverlayWindow.Dispose();
//            base.Dispose();
//        }

//        private void ClearScreen()
//        {
//            OverlayWindow.Graphics.BeginScene();
//            OverlayWindow.Graphics.ClearScene();
//            OverlayWindow.Graphics.EndScene();
//        }
//    }
//}