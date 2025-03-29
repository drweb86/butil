using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.States;

internal class DataStorageMaintananceTask(
    StorageSpecificServicesIoc services,
    TaskEvents events, 
    RemoteStateLoadTask getStateOfStorageTask,
    IncrementalBackupModelOptionsV2 incrementalBackupModelOptionsV2) : BuTaskV2(services.CommonServices.Log, events, Localization.Resources.DataStorage_Maintenance)
{
    public StorageSpecificServicesIoc _services = services;
    private readonly RemoteStateLoadTask _getStateOfStorageTask = getStateOfStorageTask;
    private readonly IncrementalBackupModelOptionsV2 _incrementalBackupModelOptionsV2 = incrementalBackupModelOptionsV2;
    public IStorageSettingsV2 StorageSettings { get; } = services.StorageSettings;

    protected override void ExecuteInternal()
    {
        DeleteIncompletedStateVersions();
    }

    private void DeleteIncompletedStateVersions()
    {
        LogDebug("Delete incompleted state versions...");
        var allowedFolders = (_getStateOfStorageTask.StorageState ?? throw new Exception())
                  .VersionStates
                  .Select(x => SourceItemHelper.GetVersionFolder(x.BackupDateUtc));

        var relativeFolders = _services.Storage.GetFolders(string.Empty, SourceItemHelper.GetVersionFolderMask());
        var foldersToDelete = relativeFolders.Except(allowedFolders);
        foreach (var folderToDelete in foldersToDelete)
        {
            _services.Storage.DeleteFolder(folderToDelete);
        }
    }
}
