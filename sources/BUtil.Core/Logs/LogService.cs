using BUtil.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BUtil.Core.Logs;

public class LogService
{
    private const string dateMask = "yyyy-MM-dd HH-mm-ss";

    public string GetFileName(string taskName, DateTime _dateTime, bool? isSuccess)
    {
        var postfix = isSuccess.HasValue ? (isSuccess.Value ? BUtil.Core.Localization.Resources.LogFile_Marker_Successful : BUtil.Core.Localization.Resources.LogFile_Marker_Errors) : BUtil.Core.Localization.Resources.Task_Status_Unknown;
        return Path.Combine(Directories.LogsFolder,
            $"{_dateTime.ToString(dateMask, CultureInfo.CurrentUICulture)} {taskName} ({postfix}).txt");
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

        var date = fileName.Substring(0, dateMask.Length);
        var createdAt = DateTime.ParseExact(date, dateMask, CultureInfo.CurrentUICulture);

        var taskName = fileName.Substring(dateMask.Length + 1);
        taskName = taskName.Substring(0, taskName.LastIndexOf(" ("));

        var isError = fileName.EndsWith($"({Localization.Resources.LogFile_Marker_Errors}).txt");
        var isSuccess = fileName.EndsWith($"({Localization.Resources.LogFile_Marker_Successful}).txt");
        return new LogFileInfo { TaskName = taskName, CreatedAt = createdAt, IsSuccess = isSuccess ? true : (isError ? false : null) };
    }
}