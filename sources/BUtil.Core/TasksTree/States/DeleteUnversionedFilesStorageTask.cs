﻿using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Misc;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;
using System.Linq;

namespace BUtil.Core.TasksTree;

internal class DeleteUnversionedFilesStorageTask : BuTask
{
    public StorageSpecificServicesIoc _services;
    private readonly RemoteStateLoadTask _getStateOfStorageTask;
    public IStorageSettingsV2 StorageSettings { get; }

    public DeleteUnversionedFilesStorageTask(StorageSpecificServicesIoc services, TaskEvents events, RemoteStateLoadTask getStateOfStorageTask) :
        base(services.Log, events, Localization.Resources.DataStorage_Maintenance)
    {
        StorageSettings = services.StorageSettings;
        _services = services;
        _getStateOfStorageTask = getStateOfStorageTask;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        var allowedFolders = (_getStateOfStorageTask.StorageState ?? throw new Exception())
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
