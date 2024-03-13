using BUtil.Core;
using BUtil.Core.Localization;
using BUtil.Core.Options;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls;

public class WhenTaskViewModel : ObservableObject
{
    public WhenTaskViewModel(ScheduleInfo scheduleInfo)
    {
        Monday = scheduleInfo.Days.Contains(System.DayOfWeek.Monday);
        Tuesday = scheduleInfo.Days.Contains(System.DayOfWeek.Tuesday);
        Wednesday = scheduleInfo.Days.Contains(System.DayOfWeek.Wednesday);
        Thursday = scheduleInfo.Days.Contains(System.DayOfWeek.Thursday);
        Friday = scheduleInfo.Days.Contains(System.DayOfWeek.Friday);
        Saturday = scheduleInfo.Days.Contains(System.DayOfWeek.Saturday);
        Sunday = scheduleInfo.Days.Contains(System.DayOfWeek.Sunday);
        Hours = scheduleInfo.Time.Hours;
        Minutes = scheduleInfo.Time.Minutes;
        IsWhenAvailable = PlatformSpecificExperience.Instance.GetTaskSchedulerService() != null;
    }

    public ScheduleInfo GetScheduleInfo()
    {
        var scheduleInfo = new ScheduleInfo();

        if (Monday) scheduleInfo.Days.Add(System.DayOfWeek.Monday);
        if (Tuesday) scheduleInfo.Days.Add(System.DayOfWeek.Tuesday);
        if (Wednesday) scheduleInfo.Days.Add(System.DayOfWeek.Wednesday);
        if (Thursday) scheduleInfo.Days.Add(System.DayOfWeek.Thursday);
        if (Friday) scheduleInfo.Days.Add(System.DayOfWeek.Friday);
        if (Saturday) scheduleInfo.Days.Add(System.DayOfWeek.Saturday);
        if (Sunday) scheduleInfo.Days.Add(System.DayOfWeek.Sunday);

        scheduleInfo.Time = new System.TimeSpan(Hours, Minutes, 0);

        return scheduleInfo;
    }

    public bool IsWhenAvailable { get; }

    #region Labels
    public string LeftMenu_When => Resources.LeftMenu_When;
    public string Time_Field_Hour => Resources.Time_Field_Hour;
    public string Time_Field_Minute => Resources.Time_Field_Minute;
    public string Days_Field_Choose => Resources.Days_Field_Choose;


    public string Days_Monday => Resources.Days_Monday;
    public string Days_Tuesday => Resources.Days_Tuesday;
    public string Days_Wednesday => Resources.Days_Wednesday;
    public string Days_Thursday => Resources.Days_Thursday;
    public string Days_Friday => Resources.Days_Friday;
    public string Days_Saturday => Resources.Days_Saturday;
    public string Days_Sunday => Resources.Days_Sunday;

    #endregion

    #region Hours

    private int _hours;

    public int Hours
    {
        get
        {
            return _hours;
        }
        set
        {
            if (value == _hours)
                return;
            _hours = value;
            OnPropertyChanged(nameof(Hours));
        }
    }

    #endregion

    #region Minutes

    private int _minutes;

    public int Minutes
    {
        get
        {
            return _minutes;
        }
        set
        {
            if (value == _minutes)
                return;
            _minutes = value;
            OnPropertyChanged(nameof(Minutes));
        }
    }

    #endregion

    #region Monday

    private bool _monday;

    public bool Monday
    {
        get
        {
            return _monday;
        }
        set
        {
            if (value == _monday)
                return;
            _monday = value;
            OnPropertyChanged(nameof(Monday));
        }
    }

    #endregion

    #region Tuesday

    private bool _tuesday;

    public bool Tuesday
    {
        get
        {
            return _tuesday;
        }
        set
        {
            if (value == _tuesday)
                return;
            _tuesday = value;
            OnPropertyChanged(nameof(Tuesday));
        }
    }

    #endregion

    #region Wednesday

    private bool _wednesday;

    public bool Wednesday
    {
        get
        {
            return _wednesday;
        }
        set
        {
            if (value == _wednesday)
                return;
            _wednesday = value;
            OnPropertyChanged(nameof(Wednesday));
        }
    }

    #endregion

    #region Thursday

    private bool _thursday;

    public bool Thursday
    {
        get
        {
            return _thursday;
        }
        set
        {
            if (value == _thursday)
                return;
            _thursday = value;
            OnPropertyChanged(nameof(Thursday));
        }
    }

    #endregion

    #region Friday

    private bool _friday;

    public bool Friday
    {
        get
        {
            return _friday;
        }
        set
        {
            if (value == _friday)
                return;
            _friday = value;
            OnPropertyChanged(nameof(Friday));
        }
    }

    #endregion

    #region Saturday

    private bool _saturday;

    public bool Saturday
    {
        get
        {
            return _saturday;
        }
        set
        {
            if (value == _saturday)
                return;
            _saturday = value;
            OnPropertyChanged(nameof(Saturday));
        }
    }

    #endregion

    #region Sunday

    private bool _sunday;

    public bool Sunday
    {
        get
        {
            return _sunday;
        }
        set
        {
            if (value == _sunday)
                return;
            _sunday = value;
            OnPropertyChanged(nameof(Sunday));
        }
    }

    #endregion
}
