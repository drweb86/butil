using System;
using System.IO;
using System.Globalization;
using BUtil.Core.FileSystem;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.Logs
{
    public class LogService
    {
        private const string dateMask = "yyyy-MM-dd HH-mm-ss";

        public string GetFileName(string taskName, DateTime _dateTime, bool? isSuccess)
        {
            var postfix = isSuccess.HasValue ? (isSuccess.Value ? BUtil.Core.Localization.Resources.LogFile_Marker_Successful : BUtil.Core.Localization.Resources.LogFile_Marker_Errors) : BUtil.Core.Localization.Resources.Task_Status_Unknown;
            var logs = PlatformSpecificExperience.Instance.GetFolderService().LogsFolder;
            return Path.Combine(logs,
                $"{_dateTime.ToString(dateMask, CultureInfo.CurrentUICulture)} {taskName} ({postfix}).html");
        }

        public IEnumerable<LogFileInfo> GetRecentLogs()
        {
            var logs = PlatformSpecificExperience.Instance.GetFolderService().LogsFolder;
            return Directory
                .GetFiles(logs, "????-??-?? ??-??-?? * (*).html")
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

            var isError = fileName.EndsWith($"({Localization.Resources.LogFile_Marker_Errors}).html");
            var isSuccess = fileName.EndsWith($"({Localization.Resources.LogFile_Marker_Successful}).html");
            return new LogFileInfo { TaskName = taskName, CreatedAt = createdAt, IsSuccess = isSuccess? true : (isError ? false : null )};
        }
    }
}