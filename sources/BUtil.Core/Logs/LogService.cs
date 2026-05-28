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

    public static string GetTaskLogsFolder(string taskName) =>
        Path.Combine(Directories.LogsFolder, Files.GetSafeFileName(taskName));

    public static string GetFileName(string taskName, DateTime _dateTime, bool? isSuccess)
    {
        var postfix = isSuccess.HasValue ? (isSuccess.Value ? BUtil.Core.Localization.Resources.LogFile_Marker_Successful : BUtil.Core.Localization.Resources.LogFile_Marker_Errors) : BUtil.Core.Localization.Resources.Task_Status_Unknown;
        var folder = GetTaskLogsFolder(taskName);
        FileHelper.EnsureFolderCreated(folder);
        return Path.Combine(folder,
            $"{_dateTime.ToString(_dateMask, CultureInfo.CurrentUICulture)} {taskName} ({postfix}).txt");
    }

    private static string GetMask(string taskNamePart)
    {
        return $"????-??-?? ??-??-?? {taskNamePart} (*).txt";
    }

    public IEnumerable<LogFileInfo> GetRecentLogs()
    {
        return [.. EnumerateLogFiles()
            .OrderByDescending(x => x)
            .Select(ParseFileName)
            .GroupBy(x => x.TaskName.ToLowerInvariant())
            .Select(x => x.First())];
    }

    public static void MoveLogs(string oldTaskName, string newTaskName)
    {
        if (oldTaskName.Cmp(newTaskName))
            return;

        var oldFolder = GetTaskLogsFolder(oldTaskName);
        if (Directory.Exists(oldFolder))
        {
            foreach (var path in Directory.GetFiles(oldFolder, GetMask("*")).ToList())
                MoveLog(ParseFileName(path), newTaskName);

            if (Directory.Exists(oldFolder) && !Directory.EnumerateFileSystemEntries(oldFolder).Any())
                Directory.Delete(oldFolder);
        }

        foreach (var path in EnumerateFlatLogFiles(oldTaskName).ToList())
            MoveLog(ParseFileName(path), newTaskName);
    }

    public static void DeleteLogs(string taskName)
    {
        var taskFolder = GetTaskLogsFolder(taskName);
        if (Directory.Exists(taskFolder))
            Directory.Delete(taskFolder, recursive: true);

        foreach (var path in EnumerateFlatLogFiles(taskName).ToList())
            File.Delete(path);
    }

    /// <summary>Moves flat log files from <paramref name="flatLogsFolder"/> into per-task subfolders under <see cref="Directories.LogsFolder"/>.</summary>
    public static void MigrateFlatLogsToTaskFolders(string flatLogsFolder)
    {
        if (!Directory.Exists(flatLogsFolder))
            return;

        foreach (var path in Directory.GetFiles(flatLogsFolder, GetMask("*")))
        {
            var info = ParseFileName(path);
            var destination = GetFileName(info.TaskName, info.CreatedAt, info.IsSuccess);
            if (path.Cmp(destination))
                continue;

            FileHelper.EnsureFolderCreatedForFile(destination);
            if (File.Exists(destination))
                File.Delete(destination);
            File.Move(path, destination);
        }
    }

    private static IEnumerable<string> EnumerateLogFiles()
    {
        if (!Directory.Exists(Directories.LogsFolder))
            yield break;

        foreach (var file in Directory.GetFiles(Directories.LogsFolder, GetMask("*")))
            yield return file;

        foreach (var taskFolder in Directory.EnumerateDirectories(Directories.LogsFolder))
        foreach (var file in Directory.GetFiles(taskFolder, GetMask("*")))
            yield return file;
    }

    private static IEnumerable<string> EnumerateFlatLogFiles(string taskName)
    {
        if (!Directory.Exists(Directories.LogsFolder))
            yield break;

        foreach (var file in Directory.GetFiles(Directories.LogsFolder, GetMask("*")))
        {
            var info = ParseFileName(file);
            if (info.TaskName.Cmp(taskName))
                yield return file;
        }
    }

    private static void MoveLog(LogFileInfo logFileInfo, string newTaskName)
    {
        var oldFile = logFileInfo.File;
        if (!File.Exists(oldFile))
            oldFile = GetFileName(logFileInfo.TaskName, logFileInfo.CreatedAt, logFileInfo.IsSuccess);

        var newFile = GetFileName(newTaskName, logFileInfo.CreatedAt, logFileInfo.IsSuccess);
        if (oldFile.Cmp(newFile))
            return;

        FileHelper.EnsureFolderCreatedForFile(newFile);
        if (File.Exists(newFile))
            File.Delete(newFile);
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
        return new LogFileInfo(taskName, createdAt, isSuccess ? true : (isError ? false : null), logFileName);
    }
}
