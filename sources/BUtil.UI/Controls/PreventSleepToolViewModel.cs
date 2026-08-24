using Avalonia.Threading;
using BUtil.Core;
using BUtil.Core.Localization;
using System;

namespace BUtil.UI.Controls;

public class PreventSleepToolViewModel : ViewModelBase
{
    private readonly DispatcherTimer _timer;
    private DateTime _startTime;
    private DateTime? _endTime;
    private long _durationMinutes;
    private bool _isRunning;
    private string _clockDisplay = "00:00:00";

    public PreventSleepToolViewModel()
    {
        WindowTitle = Resources.TechnicalTool_PreventSleep_Title;
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += OnTimerTick;
    }

    public long DurationMinutes
    {
        get => _durationMinutes;
        set
        {
            if (value == _durationMinutes)
                return;
            _durationMinutes = value < 0 ? 0 : value;
            OnPropertyChanged(nameof(DurationMinutes));
        }
    }

    public bool IsRunning
    {
        get => _isRunning;
        private set
        {
            if (value == _isRunning)
                return;
            _isRunning = value;
            OnPropertyChanged(nameof(IsRunning));
            OnPropertyChanged(nameof(CanStart));
            OnPropertyChanged(nameof(CanStop));
        }
    }

    public bool CanStart => !IsRunning;
    public bool CanStop => IsRunning;

    public string ClockDisplay
    {
        get => _clockDisplay;
        private set
        {
            if (value == _clockDisplay)
                return;
            _clockDisplay = value;
            OnPropertyChanged(nameof(ClockDisplay));
        }
    }

    public static string DurationMinutes_Field => Resources.DurationMinutes_Field;
    public static string DurationMinutes_Help => Resources.TechnicalTool_PreventSleep_Duration_Help;
    public static string Button_Start => Resources.Button_Start;
    public static string Button_Stop => Resources.Button_Stop;
    public static string Button_Close => Resources.Button_Close;

    public void StartCommand()
    {
        if (IsRunning)
            return;

        _startTime = DateTime.UtcNow;
        _endTime = DurationMinutes > 0
            ? _startTime.AddMinutes(DurationMinutes)
            : null;
        IsRunning = true;
        UpdateClockDisplay();
        PlatformSpecificExperience.Instance.OsSleepPreventionService.PreventSleep();
        _timer.Start();
    }

    public void StopCommand()
    {
        if (!IsRunning)
            return;

        _timer.Stop();
        PlatformSpecificExperience.Instance.OsSleepPreventionService.StopPreventSleep();
        IsRunning = false;
        ClockDisplay = FormatClock(TimeSpan.Zero);
    }

    public void CloseCommand()
    {
        StopCommand();
        WindowManager.SwitchView(new TasksViewModel());
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        PlatformSpecificExperience.Instance.OsSleepPreventionService.PreventSleep();
        UpdateClockDisplay();
    }

    private void UpdateClockDisplay()
    {
        if (_endTime is { } endTime)
        {
            var remaining = endTime - DateTime.UtcNow;
            if (remaining <= TimeSpan.Zero)
            {
                ClockDisplay = FormatClock(TimeSpan.Zero);
                StopCommand();
                return;
            }

            ClockDisplay = FormatClock(remaining);
            return;
        }

        ClockDisplay = FormatClock(DateTime.UtcNow - _startTime);
    }

    private static string FormatClock(TimeSpan span)
    {
        if (span < TimeSpan.Zero)
            span = TimeSpan.Zero;

        var hours = (int)span.TotalHours;
        return $"{hours:00}:{span.Minutes:00}:{span.Seconds:00}";
    }
}
