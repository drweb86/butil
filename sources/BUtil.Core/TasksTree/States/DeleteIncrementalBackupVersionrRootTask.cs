﻿using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.States;

public class DeleteIncrementalBackupVersionrRootTask : SequentialBuTask
{
    private readonly CommonServicesIoc _commonServicesIoc;
    private readonly StorageSpecificServicesIoc _storageSpecificServicesIoc;

    public DeleteIncrementalBackupVersionrRootTask(
        ILog log,
        TaskEvents events,
        IncrementalBackupState state,
        IncrementalBackupModelOptionsV2 options,
        VersionState versionToDelete,
        IStorageSettingsV2 storageSettingsV2,
        Action<string?> onGetLastMinuteMessage)
        : base(log, events, $"Delete incremental backup version {versionToDelete.BackupDateUtc}")
    {
        _commonServicesIoc = new CommonServicesIoc(log, onGetLastMinuteMessage);
        _storageSpecificServicesIoc = new StorageSpecificServicesIoc(_commonServicesIoc, storageSettingsV2);

        DeleteVersionUtil.DeleteVersion(state, versionToDelete, out var storageFilesToDelete, out var storageFileMovements);

        var tasks = new List<BuTask>();

        storageFilesToDelete.ToList().ForEach(x => tasks.Add(new DeleteStorageFileTask(_storageSpecificServicesIoc, Events, x)));

        storageFileMovements.ToList().ForEach(x => tasks.Add(new MoveStorageFileTask(_storageSpecificServicesIoc, Events, x.Key, x.Value)));

        var saveStateTask = new SaveStateToStorageTask(_storageSpecificServicesIoc, Events, state, options.Password);
        tasks.Add(saveStateTask);

        tasks.Add(new WriteIntegrityVerificationScriptsToStorageTask(_storageSpecificServicesIoc, Events, () => true,
            () => state, saveStateTask, saveStateTask, () => saveStateTask.StateFile!));

        Children = [.. tasks];
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        base.Execute();
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        _commonServicesIoc.Dispose();
        _storageSpecificServicesIoc.Dispose();
    }
}
