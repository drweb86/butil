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
        // TODO: empty versions?
        // TODO: 300+ versions

        UpgradeDataStorageFormat();

    }

    private void UpgradeDataStorageFormat()
    {
        LogDebug("Inspecting storage for outdated storage methods");

        var outdatedStorageFiles = (_getStateOfStorageTask.StorageState ?? throw new Exception())
                  .VersionStates
                  .SelectMany(x => x.SourceItemChanges)
                  .SelectMany(x =>
                  {
                      var storageFiles = new List<StorageFile>();
                      storageFiles.AddRange(x.CreatedFiles);
                      storageFiles.AddRange(x.UpdatedFiles);
                      return storageFiles;
                  })
                  .Where(x => x.StorageMethod == StorageMethodNames.SevenZipEncrypted)
                  .ToList();

        if (!outdatedStorageFiles.Any())
            return;

        var task = new UpgradeStorageFormatTask(_services, Events, _getStateOfStorageTask.StorageState!, _incrementalBackupModelOptionsV2, outdatedStorageFiles);
        
        Events.DuringExecutionTasksAdded(Id, task.GetChildren());
        Events.DuringExecutionTasksAdded(Id, [task]);

        var storageTasksExecuter = new ParallelExecuter([task], 1);
        storageTasksExecuter.Wait();
        if (!task.IsSuccess)
            throw new Exception("Upgrade storage failed!");
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
