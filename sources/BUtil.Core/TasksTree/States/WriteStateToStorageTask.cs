using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.Storage;
using System.Threading;

namespace BUtil.Core.TasksTree.States
{
    internal class WriteStateToStorageTask : BuTask
    {
        private readonly BackupTask task;
        private readonly CalculateIncrementedVersionForStorageTask _getIncrementedVersionTask;
        private readonly IStorageSettings _storageSettings;
        private readonly WriteSourceFilesToStorageTask _writeSourceFilesToStorageTask;

        public StorageFile UploadedVersionState { get; private set; }

        public WriteStateToStorageTask(
            ILog log,
            BackupEvents events,
            BackupTask task,
            CalculateIncrementedVersionForStorageTask getIncrementedVersionTask,
            IStorageSettings storageSettings, 
            Storage.WriteSourceFilesToStorageTask writeSourceFilesToStorageTask)
            : base(log, events, string.Format(BUtil.Core.Localization.Resources.WriteStateToStorage, storageSettings.Name), TaskArea.Hdd)
        {
            this.task = task;
            _getIncrementedVersionTask = getIncrementedVersionTask;
            _storageSettings = storageSettings;
            _writeSourceFilesToStorageTask = writeSourceFilesToStorageTask;
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

            if (!_writeSourceFilesToStorageTask.IsSuccess)
            {
                LogDebug("Writing source files to storage has failed. Skipping.");
                IsSuccess = false;
                UpdateStatus(ProcessingStatus.FinishedWithErrors);
                return;
            }

            var service = new IncrementalBackupStateService(Log, _storageSettings, task);
            UploadedVersionState = service.Write(token, _getIncrementedVersionTask.IncrementalBackupState);

            IsSuccess = UploadedVersionState != null;
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
