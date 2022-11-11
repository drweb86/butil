using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace BUtil.Core.TasksTree.States
{
    internal class WriteStateToStorageTask : BuTask
    {
        private CalculateIncrementedVersionForStorageTask _getIncrementedVersionTask;
        private IStorageSettings _storageSettings;

        public WriteStateToStorageTask(ILog log, BackupEvents events,
            CalculateIncrementedVersionForStorageTask getIncrementedVersionTask, IStorageSettings storageSettings)
            : base(log, events, string.Format(BUtil.Core.Localization.Resources.WriteStateToStorage, storageSettings.Name), TaskArea.Hdd)
        {
            _getIncrementedVersionTask = getIncrementedVersionTask;
            _storageSettings = storageSettings;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            UpdateStatus(ProcessingStatus.InProgress);

            if (!_getIncrementedVersionTask.VersionIsNeeded)
            {
                LogDebug("Version is not needed.");
                IsSuccess = true;
                UpdateStatus(ProcessingStatus.FinishedSuccesfully);
                return;
            }

            var storage = StorageFactory.Create(Log, _storageSettings);
            var tempFile = Path.GetRandomFileName();
            var json = JsonSerializer.Serialize(_getIncrementedVersionTask.IncrementalBackupState, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(tempFile, json);
            storage.Upload(tempFile, IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile);
            File.Delete(tempFile);
            IsSuccess = true;
            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
        }
    }
}
