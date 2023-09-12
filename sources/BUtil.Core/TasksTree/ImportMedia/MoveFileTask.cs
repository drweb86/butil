using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System;
using System.IO;

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
                var lastWriteTime = _fromStorage.GetModifiedTime(_fromFile);
                var destinationFileName = GetDestinationFileName(lastWriteTime);
                var actualFileName = GetActualDestinationFileName(destinationFileName);
                var destFolder = Path.GetDirectoryName(actualFileName);

                using (var temp = new TempFolder())
                {
                    var exchangeFile = Path.Combine(temp.Folder, Path.GetFileName(_fromFile));
                    _fromStorage.Download(_fromFile, exchangeFile);
                    var uploadedFileName = _toStorage.Upload(exchangeFile, actualFileName);
                    File.SetCreationTime(uploadedFileName.StorageFileName, lastWriteTime);
                    File.SetLastWriteTime(uploadedFileName.StorageFileName, lastWriteTime);
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

        private string GetDestinationFileName(DateTime lastWriteTime)
        {
            var relativePath = DateTokenReplacer.ParseString(_transformFileName, lastWriteTime);
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
    }
}
