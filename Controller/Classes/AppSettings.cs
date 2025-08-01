using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Blizzard_Controller;

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

    private double _deadzone = 0.05;
    private int _cursorSpeed = 10;
    private bool _variableCursorSpeed = true;
    private string _gameDetectLabel = "Not Running";
    private string _controllerDetectLabel = "Not Connected";

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
                ControllerInputs.deadzone = value;
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
                ControllerInputs.mouseDistance = value;
                ControllerInputs.mouseDistanceDefault = value;
            }
        }
    }

    /// <summary>
    /// Whether to use variable cursor speed based on joystick movement
    /// </summary>
    public bool VariableCursorSpeed
    {
        get => _variableCursorSpeed;
        set => SetProperty(ref _variableCursorSpeed, value);
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