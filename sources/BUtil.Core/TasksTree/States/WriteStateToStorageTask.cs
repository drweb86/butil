using BUtil.Core.BackupModels;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.Storage;
using System.Threading;

namespace BUtil.Core.TasksTree.States
{
    internal class WriteStateToStorageTask : BuTask
    {
        private readonly IncrementalBackupModelOptions _incrementalBackupModelOptions;
        private readonly string _password;
        private readonly CalculateIncrementedVersionForStorageTask _getIncrementedVersionTask;
        private readonly IStorageSettings _storageSettings;
        private readonly WriteSourceFilesToStorageTask _writeSourceFilesToStorageTask;

        public StorageFile StateStorageFile { get; private set; }

        public WriteStateToStorageTask(
            ILog log,
            BackupEvents events,
            CalculateIncrementedVersionForStorageTask getIncrementedVersionTask,
            WriteSourceFilesToStorageTask writeSourceFilesToStorageTask,
            IncrementalBackupModelOptions incrementalBackupModelOptions,
            IStorageSettings storageSettings,
            string password)
            : base(log, events, string.Format(Localization.Resources.WriteStateToStorage, storageSettings.Name), TaskArea.Hdd)
        {
            _incrementalBackupModelOptions = incrementalBackupModelOptions;
            _password = password;
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

            var service = new IncrementalBackupStateService(Log, _storageSettings);
            StateStorageFile = service.Write(token, _incrementalBackupModelOptions, _password, _getIncrementedVersionTask.IncrementalBackupState);
            IsSuccess = StateStorageFile != null;
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
