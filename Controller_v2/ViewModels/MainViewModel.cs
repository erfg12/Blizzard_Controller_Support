using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Blizzard_Controller;
using MsBox.Avalonia;

namespace Controller_v2.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    /// <summary>
    /// Get the shared AppSettings instance that synchronizes with the Controller project
    /// </summary>
    public AppSettings Settings => AppSettings.Instance;

    /// <summary>
    /// Expose individual properties for easier binding (delegates to shared settings)
    /// </summary>
    public double Deadzone
    {
        get => Settings.Deadzone;
        set => Settings.Deadzone = value;
    }

    public int CursorSpeed
    {
        get => Settings.CursorSpeed;
        set => Settings.CursorSpeed = value;
    }

    public bool VariableCursorSpeed
    {
        get => Settings.VariableCursorSpeed;
        set => Settings.VariableCursorSpeed = value;
    }

    public string GameDetectLabel
    {
        get => Settings.GameDetectLabel;
        set => Settings.GameDetectLabel = value;
    }

    public string ControllerDetectLabel
    {
        get => Settings.ControllerDetectLabel;
        set => Settings.ControllerDetectLabel = value;
    }

    public MainViewModel()
    {
        // Subscribe to property changes from the shared settings to update the UI
        Settings.PropertyChanged += (sender, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(AppSettings.Deadzone):
                    OnPropertyChanged(nameof(Deadzone));
                    break;
                case nameof(AppSettings.CursorSpeed):
                    OnPropertyChanged(nameof(CursorSpeed));
                    break;
                case nameof(AppSettings.VariableCursorSpeed):
                    OnPropertyChanged(nameof(VariableCursorSpeed));
                    break;
                case nameof(AppSettings.GameDetectLabel):
                    OnPropertyChanged(nameof(GameDetectLabel));
                    break;
                case nameof(AppSettings.ControllerDetectLabel):
                    OnPropertyChanged(nameof(ControllerDetectLabel));
                    break;
            }
        };
    }

    [RelayCommand]
    public void OnExitButtonClick()
    {
        Environment.Exit(0);
    }

    [RelayCommand]
    public async void OnStartBattleNetButtonClick()
    {
        if (!GameSettings.startGame())
            await MessageBoxManager
            .GetMessageBoxStandard("ERROR", "Could not locate Battle.net directory.")
            .ShowAsync();
    }

    [RelayCommand]
    public void OnTerranGridLayoutButtonClick()
    {
        
    }

    [RelayCommand]
    public void OnZergGridLayoutButtonClick()
    {

    }

    [RelayCommand]
    public void OnRestoreDefaultHotkeysButtonClick()
    {
        
    }
}
