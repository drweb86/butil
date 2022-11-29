using BUtil.Core.Events;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Core.TasksTree
{
    internal class WriteSourceFileToStorageTask : BuTask
    {
        private readonly StorageSpecificServicesIoc _services;
        private readonly Quota _singleBackupQuotaGb;

        public StorageFile StorageFile { get; }
        public bool IsSkipped { get; private set; }

        public WriteSourceFileToStorageTask(
            StorageSpecificServicesIoc services,
            BackupEvents events,
            StorageFile storageFile,
            Quota singleBackupQuotaGb) : 
            base(services.Log, events, string.Format(BUtil.Core.Localization.Resources.WriteSourceFileToStorage, storageFile.FileState.FileName, services.StorageSettings.Name), TaskArea.File)
        {
            _services = services;
            StorageFile = storageFile;
            _singleBackupQuotaGb = singleBackupQuotaGb;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);

            if (_singleBackupQuotaGb.TryQuota(StorageFile.FileState.Size))
            {
                try
                {
                    IsSuccess = _services.IncrementalBackupFileService.Upload(StorageFile);
                }
                catch
                {
                    IsSkipped = true; // because some files can be locked
                    IsSuccess = true;
                }
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
