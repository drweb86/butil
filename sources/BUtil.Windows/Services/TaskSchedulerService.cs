using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using Microsoft.Win32.TaskScheduler;

namespace BUtil.Windows.Services;

public class TaskSchedulerService : ITaskSchedulerService
{
    public ScheduleInfo GetSchedule(string taskName)
    {
        var schedulerTaskName = GetScheduledTaskName(taskName);
        var scheduledTask = TaskService.Instance.FindTask(schedulerTaskName, false);
        if (scheduledTask == null)
            return new ScheduleInfo();
        var weeklyTrigger = (WeeklyTrigger)scheduledTask.Definition.Triggers[0];

        var scheduledInfo = new ScheduleInfo
        {
            Time = new TimeSpan(weeklyTrigger.StartBoundary.Hour, weeklyTrigger.StartBoundary.Minute, 0)
        };
        if ((weeklyTrigger.DaysOfWeek & DaysOfTheWeek.Monday) == DaysOfTheWeek.Monday)
            scheduledInfo.Days.Add(DayOfWeek.Monday);

        if ((weeklyTrigger.DaysOfWeek & DaysOfTheWeek.Tuesday) == DaysOfTheWeek.Tuesday)
            scheduledInfo.Days.Add(DayOfWeek.Tuesday);

        if ((weeklyTrigger.DaysOfWeek & DaysOfTheWeek.Wednesday) == DaysOfTheWeek.Wednesday)
            scheduledInfo.Days.Add(DayOfWeek.Wednesday);

        if ((weeklyTrigger.DaysOfWeek & DaysOfTheWeek.Thursday) == DaysOfTheWeek.Thursday)
            scheduledInfo.Days.Add(DayOfWeek.Thursday);

        if ((weeklyTrigger.DaysOfWeek & DaysOfTheWeek.Friday) == DaysOfTheWeek.Friday)
            scheduledInfo.Days.Add(DayOfWeek.Friday);

        if ((weeklyTrigger.DaysOfWeek & DaysOfTheWeek.Saturday) == DaysOfTheWeek.Saturday)
            scheduledInfo.Days.Add(DayOfWeek.Saturday);

        if ((weeklyTrigger.DaysOfWeek & DaysOfTheWeek.Sunday) == DaysOfTheWeek.Sunday)
            scheduledInfo.Days.Add(DayOfWeek.Sunday);


        return scheduledInfo;
    }

    public void Schedule(string taskName, ScheduleInfo scheduleInfo)
    {
        Unschedule(taskName);

        if (!scheduleInfo.Days.Any())
            return;

        var schedulerTaskName = GetScheduledTaskName(taskName);

        TaskService.Instance.AddTask(
            schedulerTaskName,
            new WeeklyTrigger
            {
                DaysOfWeek = GetDaysOfTheWeek(scheduleInfo.Days),
                StartBoundary = DateTime.Today + scheduleInfo.Time,
                WeeksInterval = 1,
            },
            new ExecAction
            {
                WorkingDirectory = Directories.BinariesDir,
                Arguments = $"Task={taskName}",
                Path = WindowsSupportManager.ConsoleBackupTool
            });
    }

    public void Unschedule(string taskName)
    {
        var schedulerTaskName = GetScheduledTaskName(taskName);
        TaskService.Instance.RootFolder.DeleteTask(schedulerTaskName, false);
    }

    private DaysOfTheWeek GetDaysOfTheWeek(List<DayOfWeek> days)
    {
        DaysOfTheWeek result = 0;
        foreach (var day in days)
        {
            if (day == DayOfWeek.Monday)
                result |= DaysOfTheWeek.Monday;
            if (day == DayOfWeek.Tuesday)
                result |= DaysOfTheWeek.Tuesday;
            if (day == DayOfWeek.Thursday)
                result |= DaysOfTheWeek.Thursday;
            if (day == DayOfWeek.Wednesday)
                result |= DaysOfTheWeek.Wednesday;
            if (day == DayOfWeek.Sunday)
                result |= DaysOfTheWeek.Sunday;
            if (day == DayOfWeek.Saturday)
                result |= DaysOfTheWeek.Saturday;
            if (day == DayOfWeek.Friday)
                result |= DaysOfTheWeek.Friday;
        }

        return result;
    }

    private string GetScheduledTaskName(string taskName)
    {
        return $"BUtil {taskName} task";
    }
}
