
using System;
using System.Collections.Generic;

namespace BUtil.Core.Options;

public class ScheduleInfo
{
    public ScheduleInfo()
    {
        Days = [];
        Time = new TimeSpan(DateTime.Now.Hour, 0, 0);
    }

    public List<DayOfWeek> Days { get; set; }

    public TimeSpan Time { get; set; }

    public bool RunAtLogin { get; set; }
}