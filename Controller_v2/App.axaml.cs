using System.Configuration;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Blizzard_Controller;
using Blizzard_Controller.Input;
using Controller_v2.ViewModels;
using Controller_v2.Views;

#if MACOS
using static Blizzard_Controller.Platform.MacOS.NativeInvoke;
#endif

namespace Controller_v2;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };


            // start up our threads for controller processing and overlay drawing
            if (!Design.IsDesignMode)
            {
#if MACOS
                AXUIElementCreateSystemWide();
                if (!AXIsProcessTrusted())
                    Process.Start("open", "x-apple.systempreferences:com.apple.preference.security?Privacy_Accessibility");
#endif
                Task.Run(async () =>
                {
                    await Task.WhenAll(
                        ControllerInputs.CheckGameProc(),
                        Task.Run(async () =>
                        {
                            while (!ControllerInputs.shuttingDown)
                            {
                                ControllerState.IsGamepadConnected();
                                await Task.Delay(1000); // Check every second
                            }
                        }),
                        Task.Run(async () =>
                        {
                            while (!ControllerInputs.shuttingDown)
                            {
                                // Process input at a higher frequency for responsiveness
                                if (ControllerState.IsGamepadConnected())
                                {
                                    ControllerInputs.processButtons();
                                    ControllerInputs.processJoysticks();
                                }
                                await Task.Delay(16); // ~60Hz for smooth input
                            }
                        })
                    );
                });
#if LINUX || WINDOWS
                Task.Run(() =>
                {
                    var game = new OverlayWindowMonoGame();
                    game.Run();
                });

#else // macos
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    var game = new OverlayWindowMonoGame();
                    game.Run();
                });
#endif
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                var mainViewModel = new MainViewModel();

                singleViewPlatform.MainView = new MainView
                {
                    DataContext = mainViewModel
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
