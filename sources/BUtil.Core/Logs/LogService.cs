using BUtil.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BUtil.Core.Logs;

public class LogService
{
    private const string _dateMask = "yyyy-MM-dd HH-mm-ss";

    public static string GetFileName(string taskName, DateTime _dateTime, bool? isSuccess)
    {
        var postfix = isSuccess.HasValue ? (isSuccess.Value ? BUtil.Core.Localization.Resources.LogFile_Marker_Successful : BUtil.Core.Localization.Resources.LogFile_Marker_Errors) : BUtil.Core.Localization.Resources.Task_Status_Unknown;
        return Path.Combine(Directories.LogsFolder,
            $"{_dateTime.ToString(_dateMask, CultureInfo.CurrentUICulture)} {taskName} ({postfix}).txt");
    }

    public IEnumerable<LogFileInfo> GetRecentLogs()
    {
        return Directory
            .GetFiles(Directories.LogsFolder, "????-??-?? ??-??-?? * (*).txt")
            .OrderByDescending(x => x)
            .Select(ParseFileName)
            .GroupBy(x => x.TaskName)
            .Select(x => x.First())
            .ToList();
    }

    private LogFileInfo ParseFileName(string logFileName)
    {
        var fileName = Path.GetFileName(logFileName);

        var date = fileName[.._dateMask.Length];
        var createdAt = DateTime.ParseExact(date, _dateMask, CultureInfo.CurrentUICulture);

        var taskName = fileName[(_dateMask.Length + 1)..];
        taskName = taskName[..taskName.LastIndexOf(" (")];

        var isError = fileName.EndsWith($"({Localization.Resources.LogFile_Marker_Errors}).txt");
        var isSuccess = fileName.EndsWith($"({Localization.Resources.LogFile_Marker_Successful}).txt");
        return new LogFileInfo { TaskName = taskName, CreatedAt = createdAt, IsSuccess = isSuccess ? true : (isError ? false : null) };
    }
}