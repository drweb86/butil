using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class WriteSourceFileToStorageTask : BuTask
    {
        private readonly IStorageSettings _storageSettings;
        private readonly Quota _singleBackupQuotaGb;

        public StorageFile StorageFile { get; }
        public bool IsSkipped { get; private set; }

        public WriteSourceFileToStorageTask(
            ILog log,
            BackupEvents events,
            StorageFile storageFile,
            IStorageSettings storageSettings,
            Quota singleBackupQuotaGb) : 
            base(log, events, string.Format(BUtil.Core.Localization.Resources.WriteSourceFileToStorage, storageFile.FileState.FileName, storageSettings.Name), TaskArea.File)
        {
            StorageFile = storageFile;
            this._storageSettings = storageSettings;
            _singleBackupQuotaGb = singleBackupQuotaGb;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);

            if (_singleBackupQuotaGb.TryQuota(StorageFile.FileState.Size))
            {
                var service = new IncrementalBackupFileService(Log, _storageSettings);
                IsSuccess = service.Upload(StorageFile);
            }
            else
            {
                LogDebug("Skipped because of quota.");
                IsSkipped = true;
                IsSuccess = true;
            }
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
