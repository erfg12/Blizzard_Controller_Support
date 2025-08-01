using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Blizzard_Controller;
using Controller_v2.ViewModels;
using Controller_v2.Views;

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
            var mainViewModel = new MainViewModel();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };

            // start up our threads for controller processing and overlay drawing
            if (!Design.IsDesignMode)
            {
                Task.Run(ControllerInputs.CheckGameProc);

                Task.Run(ControllerInputs.CheckControllerStatus);

                Task.Run(() =>
                {
                    var ow = new OverlayWindow();
                    ow.Initialize();
                });
            }
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            var mainViewModel = new MainViewModel();
            
            singleViewPlatform.MainView = new MainView
            {
                DataContext = mainViewModel
            };

            // Initialize Controller static values with MainViewModel values
            ControllerInputs.deadzone = mainViewModel.Deadzone;
            ControllerInputs.mouseDistance = mainViewModel.CursorSpeed;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
