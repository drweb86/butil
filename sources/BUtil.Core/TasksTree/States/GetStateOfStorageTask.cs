using BUtil.Core.Events;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Core.TasksTree
{
    internal class GetStateOfStorageTask : BuTask
    {
        public StorageSpecificServicesIoc _services;
        private readonly string _password;

        public IStorageSettings StorageSettings { get; }
        public IncrementalBackupState StorageState { get; private set; }

        public GetStateOfStorageTask(StorageSpecificServicesIoc services, BackupEvents events, string password) : 
            base(services.Log, events, string.Format(Localization.Resources.GetStateOfStorage, services.StorageSettings.Name), TaskArea.Folder)
        {
            StorageSettings = services.StorageSettings;
            _services = services;
            this._password = password;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);
            IsSuccess = _services.IncrementalBackupStateService.TryRead(_password, out var state);
            StorageState = state;
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
