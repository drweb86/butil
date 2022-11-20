using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class GetStateOfStorageTask : BuTask
    {
        private readonly string password;

        public IStorageSettings StorageSettings { get; }
        public IncrementalBackupState StorageState { get; private set; }

        public GetStateOfStorageTask(ILog log, BackupEvents events, IStorageSettings storageSettings, string password) : 
            base(log, events, string.Format(Localization.Resources.GetStateOfStorage, storageSettings.Name), TaskArea.Hdd)
        {
            StorageSettings = storageSettings;
            this.password = password;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);

            var service = new IncrementalBackupStateService(Log, StorageSettings);
            IsSuccess = service.TryRead(password, out var state);
            StorageState = state;
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
