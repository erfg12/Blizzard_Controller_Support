using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Blizzard_Controller.Configuration;

/// <summary>
/// Shared settings class that provides data binding support and synchronization
/// between the Controller core logic and Controller_v2 UI
/// </summary>
public class AppSettings : INotifyPropertyChanged
{
    private static AppSettings? _instance;
    private static readonly object _lock = new object();

    public static AppSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new AppSettings();
                }
            }
            return _instance;
        }
    }

    private double _deadzone = Convert.ToDouble(Properties.Settings.Default.Deadzone);
    private int _cursorSpeed = Convert.ToInt32(Properties.Settings.Default.cursorSpeed);
    private bool _variableCursorSpeed = Properties.Settings.Default.IncreaseCursorSpeed;
    private string _gameDetectLabel = "Not Running";
    private string _controllerDetectLabel = "Not Connected";
    private string _selectedButtonImageItem = Properties.Settings.Default.ButtonImages;

    /// <summary>
    /// Controller input deadzone (0.0 to 1.0)
    /// </summary>
    public double Deadzone
    {
        get => _deadzone;
        set
        {
            if (SetProperty(ref _deadzone, value))
            {
                // Update the static variable in ControllerInputs for backward compatibility
                Input.ControllerInputs.deadzone = value;
                Properties.Settings.Default.Deadzone = value;
                Properties.Settings.Default.Save();
            }
        }
    }

    /// <summary>
    /// Cursor movement speed/distance
    /// </summary>
    public int CursorSpeed
    {
        get => _cursorSpeed;
        set
        {
            if (SetProperty(ref _cursorSpeed, value))
            {
                // Update the static variable in ControllerInputs for backward compatibility
                Input.ControllerInputs.mouseDistance = value;
                Input.ControllerInputs.mouseDistanceDefault = value;

                Properties.Settings.Default.cursorSpeed = value;
                Properties.Settings.Default.Save();
            }
        }
    }

    /// <summary>
    /// Whether to use variable cursor speed based on joystick movement
    /// </summary>
    public bool VariableCursorSpeed
    {
        get => _variableCursorSpeed;
        set
        {
            SetProperty(ref _variableCursorSpeed, value);
            Properties.Settings.Default.IncreaseCursorSpeed = value;
            Properties.Settings.Default.Save();
        }
    }

    /// <summary>
    /// Current game detection status
    /// </summary>
    public string GameDetectLabel
    {
        get => _gameDetectLabel;
        set => SetProperty(ref _gameDetectLabel, value);
    }

    /// <summary>
    /// Current controller connection status
    /// </summary>
    public string ControllerDetectLabel
    {
        get => _controllerDetectLabel;
        set => SetProperty(ref _controllerDetectLabel, value);
    }

    /// <summary>
    /// Updates game detection status from the Controller core logic
    /// </summary>
    public void UpdateGameStatus(string status)
    {
        GameDetectLabel = status;
    }

    /// <summary>
    /// Updates controller connection status from the Controller core logic
    /// </summary>
    public void UpdateControllerStatus(bool connected)
    {
        ControllerDetectLabel = connected ? "Connected" : "Not Connected";
    }

    public string SelectedButtonImageItem
    {
        get => _selectedButtonImageItem;
        set
        {
            SetProperty(ref _selectedButtonImageItem, value);
            Properties.Settings.Default.ButtonImages = value;
            UI.Overlay.OverlayWindow.overlayBtns = value;
            Properties.Settings.Default.Save();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}