using System.Text.RegularExpressions;
using BUtil.Core.Misc;
using BUtil.Core.Options;

namespace BUtil.Linux.Services;

public class LinuxTaskSchedulerService : ITaskSchedulerService
{
    private const string CronCommentPrefix = "# BUtil Schedule: ";

    public ScheduleInfo GetSchedule(string taskName)
    {
        var crontab = GetCurrentCrontab();
        var schedule = ParseScheduleFromCrontab(crontab, taskName);
        schedule.RunAtLogin = File.Exists(SupportManager.GetLoginAutostartPath(taskName));
        return schedule;
    }

    public void Schedule(string taskName, ScheduleInfo scheduleInfo)
    {
        if (scheduleInfo.Days.Count == 0 && !scheduleInfo.RunAtLogin)
        {
            Unschedule(taskName);
            return;
        }

        var crontab = GetCurrentCrontab();
        crontab = RemoveTaskFromCrontab(crontab, taskName);

        if (scheduleInfo.Days.Count > 0)
        {
            var minute = scheduleInfo.Time.Minutes;
            var hour = scheduleInfo.Time.Hours;
            var daysOfWeek = string.Join(",", scheduleInfo.Days.Select(d => (int)d));
            var command = SupportManager.GetScheduledTaskConsoleCommand(taskName);
            var cronLine = $"{minute} {hour} * * {daysOfWeek} {command}";
            crontab += CronCommentPrefix + taskName + "\n" + cronLine + "\n";
        }

        SetCrontab(crontab);
        SetLoginAutostart(taskName, scheduleInfo.RunAtLogin);
    }

    public void Unschedule(string taskName)
    {
        var crontab = GetCurrentCrontab();
        crontab = RemoveTaskFromCrontab(crontab, taskName);
        SetCrontab(crontab);
        SetLoginAutostart(taskName, false);
    }

    private static string GetCurrentCrontab()
    {
        ProcessHelper.Execute("crontab", "-l", null, false, System.Diagnostics.ProcessPriorityClass.Normal,
            out var stdOutput, out _, out var returnCode);
        // crontab -l exits with 1 when no crontab exists
        if (returnCode != 0)
            return string.Empty;
        return stdOutput ?? string.Empty;
    }

    private static void SetCrontab(string content)
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"butil-crontab-{Guid.NewGuid():N}.tmp");
        try
        {
            File.WriteAllText(tempPath, content);
            ProcessHelper.Execute("crontab", tempPath, null, false, System.Diagnostics.ProcessPriorityClass.Normal,
                out _, out _, out _);
        }
        finally
        {
            try { File.Delete(tempPath); } catch { /* best effort */ }
        }
    }

    private static string RemoveTaskFromCrontab(string crontab, string taskName)
    {
        var lines = crontab.Split('\n').ToList();
        var result = new List<string>();
        var i = 0;
        while (i < lines.Count)
        {
            var line = lines[i];
            if (line.StartsWith(CronCommentPrefix, StringComparison.Ordinal))
            {
                var commentTaskName = line.Substring(CronCommentPrefix.Length).Trim();
                if (commentTaskName == taskName)
                {
                    i++;
                    if (i < lines.Count)
                        i++;
                    continue;
                }
            }
            if (IsOurCronLine(line, taskName))
            {
                i++;
                continue;
            }
            result.Add(line);
            i++;
        }
        var trimmed = string.Join("\n", result).TrimEnd();
        return string.IsNullOrEmpty(trimmed) ? string.Empty : trimmed + "\n";
    }

    private static bool IsOurCronLine(string line, string taskName)
    {
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
            return false;
        return line.Contains($"Task={taskName}\"", StringComparison.Ordinal) ||
               (line.Contains(SupportManager.ConsoleBackupTool, StringComparison.Ordinal) && line.Contains($"Task={taskName}", StringComparison.Ordinal));
    }

    private static ScheduleInfo ParseScheduleFromCrontab(string crontab, string taskName)
    {
        var schedule = new ScheduleInfo();
        var lines = crontab.Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (!line.StartsWith(CronCommentPrefix, StringComparison.Ordinal))
                continue;
            var commentTaskName = line.Substring(CronCommentPrefix.Length).Trim();
            if (commentTaskName != taskName)
                continue;
            i++;
            if (i >= lines.Length)
                break;
            var cronLine = lines[i];
            if (!ParseCronLine(cronLine, schedule))
                continue;
            if (IsOurCronLine(cronLine, taskName))
                return schedule;
        }
        for (var i = 0; i < lines.Length; i++)
        {
            if (IsOurCronLine(lines[i], taskName) && ParseCronLine(lines[i], schedule))
                return schedule;
        }
        return schedule;
    }

    /// <summary>
    /// Parse cron line "minute hour * * dayofweek command" and fill schedule. Day of week: 0=Sunday, 1=Monday, ... 6=Saturday.
    /// </summary>
    private static bool ParseCronLine(string line, ScheduleInfo schedule)
    {
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
            return false;
        var match = Regex.Match(line, @"^\s*(\d+)\s+(\d+)\s+\*\s+\*\s+([\d,]+)\s+(.+)$");
        if (!match.Success)
            return false;
        var minute = int.Parse(match.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
        var hour = int.Parse(match.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);
        schedule.Time = new TimeSpan(hour, minute, 0);
        schedule.Days.Clear();
        var dowPart = match.Groups[3].Value;
        foreach (var part in dowPart.Split(','))
        {
            var d = int.Parse(part.Trim(), System.Globalization.CultureInfo.InvariantCulture);
            if (d >= 0 && d <= 6)
                schedule.Days.Add((DayOfWeek)d);
        }
        return true;
    }

    private static void SetLoginAutostart(string taskName, bool enabled)
    {
        var autostartPath = SupportManager.GetLoginAutostartPath(taskName);
        if (!enabled)
        {
            if (File.Exists(autostartPath))
                File.Delete(autostartPath);
            return;
        }

        Directory.CreateDirectory(SupportManager.AutostartFolder);
        File.WriteAllText(autostartPath, SupportManager.CreateLoginAutostartEntry(taskName));
    }
}
