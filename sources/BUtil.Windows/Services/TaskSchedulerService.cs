using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.Core.Services;
using Microsoft.Win32.TaskScheduler;


namespace BUtil.Windows.Services;

public class TaskSchedulerService : ITaskSchedulerService
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public ScheduleInfo GetSchedule(string taskName)
    {
        var schedulerTaskName = GetScheduledTaskName(taskName);
        var scheduledTask = TaskService.Instance.FindTask(schedulerTaskName, false);
        if (scheduledTask == null)
            return new ScheduleInfo();
        var scheduledInfo = new ScheduleInfo();

        foreach (var trigger in scheduledTask.Definition.Triggers)
        {
            if (trigger is LogonTrigger)
                scheduledInfo.RunAtLogin = true;

            if (trigger is not WeeklyTrigger weeklyTrigger)
                continue;

            scheduledInfo.Time = new TimeSpan(weeklyTrigger.StartBoundary.Hour, weeklyTrigger.StartBoundary.Minute, 0);

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
        }

        return scheduledInfo;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public void Schedule(string taskName, ScheduleInfo scheduleInfo)
    {
        Unschedule(taskName);

        if (scheduleInfo.Days.Count == 0 && !scheduleInfo.RunAtLogin)
            return;

        var schedulerTaskName = GetScheduledTaskName(taskName);

        var taskDefinition = TaskService.Instance.NewTask();
        if (scheduleInfo.Days.Count > 0)
        {
            taskDefinition.Triggers.Add(new WeeklyTrigger
            {
                DaysOfWeek = GetDaysOfTheWeek(scheduleInfo.Days),
                StartBoundary = DateTime.Today + scheduleInfo.Time,
                WeeksInterval = 1,
            });
        }

        if (scheduleInfo.RunAtLogin)
            taskDefinition.Triggers.Add(new LogonTrigger());

        taskDefinition.Actions.Add(new ExecAction
        {
            WorkingDirectory = WindowsSupportManager.ExternalWorkingDirectory,
            Arguments = $"\"Task={taskName}\" {SchedulerLaunchArguments.HideConsole}",
            Path = WindowsSupportManager.ConsoleBackupTool
        });

        TaskService.Instance.RootFolder.RegisterTaskDefinition(schedulerTaskName, taskDefinition);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public void Unschedule(string taskName)
    {
        var schedulerTaskName = GetScheduledTaskName(taskName);
        TaskService.Instance.RootFolder.DeleteTask(schedulerTaskName, false);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    private static DaysOfTheWeek GetDaysOfTheWeek(List<DayOfWeek> days)
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    internal void RetargetScheduledTasks(IReadOnlyList<string> taskNames)
    {
        var desiredPath = WindowsSupportManager.ConsoleBackupTool;
        var desiredWorkDir = WindowsSupportManager.ExternalWorkingDirectory;
        foreach (var taskName in taskNames)
        {
            try
            {
                var scheduledTask = TaskService.Instance.FindTask(GetScheduledTaskName(taskName), false);
                if (scheduledTask == null)
                    continue;

                var changed = false;
                foreach (var action in scheduledTask.Definition.Actions)
                {
                    if (action is not ExecAction exec || !IsOurConsoleLaunch(exec))
                        continue;
                    if (!SameLocation(exec.Path, desiredPath))
                    {
                        exec.Path = desiredPath;
                        changed = true;
                    }
                    if (!SameLocation(exec.WorkingDirectory, desiredWorkDir))
                    {
                        exec.WorkingDirectory = desiredWorkDir;
                        changed = true;
                    }
                }

                if (changed)
                    scheduledTask.RegisterChanges();
            }
            catch
            {
                // One broken task must not stop the others from being repaired.
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    private static bool IsOurConsoleLaunch(ExecAction exec)
    {
        var fileName = Path.GetFileName(NormalizePath(exec.Path));
        var launchesConsole = fileName.Equals("butilc.exe", StringComparison.OrdinalIgnoreCase)
            || fileName.Equals(WindowsStorePackage.ConsoleExecutionAlias, StringComparison.OrdinalIgnoreCase);
        return launchesConsole
            && exec.Arguments?.Contains($"{TasksAppArguments.RunTask}=", StringComparison.Ordinal) == true;
    }

    private static bool SameLocation(string? left, string? right) =>
        string.Equals(NormalizePath(left), NormalizePath(right), StringComparison.OrdinalIgnoreCase);

    private static string NormalizePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return string.Empty;

        var trimmed = path.Trim().Trim('"');
        if (trimmed.Length == 0)
            return string.Empty;

        try
        {
            return Path.GetFullPath(trimmed);
        }
        catch
        {
            return trimmed;
        }
    }

    private static string GetScheduledTaskName(string taskName)
    {
        return $"BUtil {taskName} task";
    }
}
