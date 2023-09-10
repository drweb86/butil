using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace BUtil.Core.TasksTree.MediaSyncBackupModel
{
    class MoveFileTask : BuTask
    {
        private readonly string _fromFile;
        private readonly string _transformFileName;
        private readonly IStorage _fromStorage;
        private readonly IStorage _toStorage;

        public MoveFileTask(ILog log, BackupEvents backupEvents, string fromFile, IStorage fromStorage, IStorage toStorage, string transformFileName)
            : base(log, backupEvents, null, TaskArea.File)
        {
            _fromFile = fromFile;
            _fromStorage = fromStorage;
            _toStorage = toStorage;
            _transformFileName = transformFileName;

            Title = string.Format(Resources.MoveFileToDestFolder, Path.GetFileNameWithoutExtension(fromFile));
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);
            try
            {
                var destinationFileName = GetDestinationFileName();
                var actualFileName = GetActualDestinationFileName(destinationFileName);
                var destFolder = Path.GetDirectoryName(actualFileName);

                using (var temp = new TempFolder())
                {
                    var exchangeFile = Path.Combine(temp.Folder, Path.GetFileName(_fromFile));
                    _fromStorage.Download(_fromFile, exchangeFile);
                    _toStorage.Upload(exchangeFile, actualFileName);
                    _fromStorage.Delete(_fromFile);
                }

                IsSuccess = true;
                UpdateStatus(ProcessingStatus.FinishedSuccesfully);
            }
            catch (Exception ex)
            {
                IsSuccess = false;

                this.LogError(ex.Message);
                UpdateStatus(ProcessingStatus.FinishedWithErrors);
            }
        }

        private string GetActualDestinationFileName(string destFile)
        {
            var actualDestFile = destFile;
            var id = 0;
            while (_toStorage.Exists(actualDestFile))
            {
                id++;
                actualDestFile = Path.Combine(Path.GetDirectoryName(destFile), $"{Path.GetFileNameWithoutExtension(destFile)}_{id}{Path.GetExtension(destFile)}");
            }

            return actualDestFile;
        }

        private string GetDestinationFileName()
        {
            var lastWriteTime = _fromStorage.GetModifiedTime(_fromFile);

            var relativePath = ParseString(_transformFileName, lastWriteTime);
            var relativeDir = Path.GetDirectoryName(relativePath);
            foreach (var ch in Path.GetInvalidPathChars())
            {
                relativeDir = relativeDir.Replace(ch, '_');
            }

            var relativeFileName = Path.GetFileName(relativePath);
            foreach (var ch in Path.GetInvalidFileNameChars())
            {
                relativeFileName = relativeFileName.Replace(ch, '_');
            }

            var destFile = Path.Combine(relativeDir, $"{relativeFileName}{Path.GetExtension(_fromFile)}");

            return destFile;
        }

        private const string RegexIncludeBrackets = @"{(?<Param>.*?)}";

        public static string ParseString(string input, DateTime date)
        {
            return Regex.Replace(input, RegexIncludeBrackets, match =>
            {
                string cleanedString = match.Groups["Param"].Value;
                if (cleanedString.StartsWith("DATE:"))
                {
                    var format = cleanedString.Replace("DATE:", string.Empty);
                    return date.ToString(format, CultureInfo.CurrentUICulture);
                }
                return string.Empty;
            });
        }
    }
}
