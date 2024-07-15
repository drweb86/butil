
using System;
using System.Collections.Generic;

namespace BUtil.Core.Options;

public class ScheduleInfo
{
    public ScheduleInfo()
    {
        Days = [];
        Time = new TimeSpan(Constants.DefaultHours, Constants.DefaultMinutes, 0);
    }

    public List<DayOfWeek> Days { get; set; }

    public TimeSpan Time { get; set; }
}