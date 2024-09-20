using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.TasksTree.Core;
using System;
using System.Linq;

namespace BUtil.Core.TasksTree;

internal class DeleteUnversionedFilesStorageTask(StorageSpecificServicesIoc services, TaskEvents events, RemoteStateLoadTask getStateOfStorageTask) : BuTaskV2(services.CommonServices.Log, events, Localization.Resources.DataStorage_Maintenance)
{
    public StorageSpecificServicesIoc _services = services;
    private readonly RemoteStateLoadTask _getStateOfStorageTask = getStateOfStorageTask;
    public IStorageSettingsV2 StorageSettings { get; } = services.StorageSettings;

    protected override void ExecuteInternal()
    {
        var allowedFolders = (_getStateOfStorageTask.StorageState ?? throw new Exception())
          .VersionStates
          .Select(x => SourceItemHelper.GetVersionFolder(x.BackupDateUtc));

        var relativeFolders = this._services.Storage.GetFolders(string.Empty, SourceItemHelper.GetVersionFolderMask());
        var foldersToDelete = relativeFolders.Except(allowedFolders);
        foreach (var folderToDelete in foldersToDelete)
        {
            this._services.Storage.DeleteFolder(folderToDelete);
        }
    }
}
