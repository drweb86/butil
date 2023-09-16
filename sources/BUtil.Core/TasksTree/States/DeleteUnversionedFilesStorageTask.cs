using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System.Linq;

namespace BUtil.Core.TasksTree
{
    internal class DeleteUnversionedFilesStorageTask : BuTask
    {
        public StorageSpecificServicesIoc _services;
        private readonly string _password;
        private readonly GetStateOfStorageTask _getStateOfStorageTask;
        public IStorageSettingsV2 StorageSettings { get; }

        public DeleteUnversionedFilesStorageTask(StorageSpecificServicesIoc services, TaskEvents events, string password, GetStateOfStorageTask getStateOfStorageTask) :
            base(services.Log, events, Localization.Resources.DeleteUnversionedFilesStorage, TaskArea.Hdd)
        {
            StorageSettings = services.StorageSettings;
            _services = services;
            this._password = password;
            _getStateOfStorageTask = getStateOfStorageTask;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);

            var allowedFolders = _getStateOfStorageTask.StorageState
                .VersionStates
                .Select(x => SourceItemHelper.GetVersionFolder(x.BackupDateUtc));

            var relativeFolders = this._services.Storage.GetFolders(string.Empty, SourceItemHelper.GetVersionFolderMask());
            var foldersToDelete = relativeFolders.Except(allowedFolders);
            foreach (var folderToDelete in foldersToDelete)
            {
                this._services.Storage.DeleteFolder(folderToDelete);
            }

            IsSuccess = true;
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
