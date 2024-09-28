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

    private static string GetMask(string taskNamePart)
    {
        return $"????-??-?? ??-??-?? {taskNamePart} (*).txt";
    }

    public IEnumerable<LogFileInfo> GetRecentLogs()
    {
        return Directory
            .GetFiles(Directories.LogsFolder, GetMask("*"))
            .OrderByDescending(x => x)
            .Select(ParseFileName)
            .GroupBy(x => x.TaskName.ToLowerInvariant())
            .Select(x => x.First())
            .ToList();
    }

    public static void MoveLogs(string oldTaskName, string newTaskName)
    {
        if (oldTaskName.Cmp(newTaskName))
            return;

        Directory
            .GetFiles(Directories.LogsFolder, GetMask("*"))
            .OrderByDescending(x => x)
            .Select(ParseFileName)
            .Where(x => x.TaskName.Cmp(oldTaskName))
            .ToList()
            .ForEach(x => MoveLog(x, newTaskName));

    }

    public static void DeleteLogs(string taskName)
    {
        Directory
            .GetFiles(Directories.LogsFolder, GetMask("*"))
            .OrderByDescending(x => x)
            .Select(ParseFileName)
            .Where(x => x.TaskName.Cmp(taskName))
            .ToList()
            .ForEach(DeleteLog);
    }

    private static void DeleteLog(LogFileInfo logFileInfo)
    {
        var file = GetFileName(logFileInfo.TaskName, logFileInfo.CreatedAt, logFileInfo.IsSuccess);
        File.Delete(file);
    }

    private static void MoveLog(LogFileInfo logFileInfo, string newTaskName)
    {
        var oldFile = GetFileName(logFileInfo.TaskName, logFileInfo.CreatedAt, logFileInfo.IsSuccess);
        var newFile = GetFileName(newTaskName, logFileInfo.CreatedAt, logFileInfo.IsSuccess);
        File.Move(oldFile, newFile);
    }

    private static LogFileInfo ParseFileName(string logFileName)
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