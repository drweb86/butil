using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Services;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.States;
using BUtil.Core.TasksTree.Storage;
using System;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree;

internal class WriteIncrementedVersionTask : SequentialBuTask
{
    private readonly WriteSourceFilesToStorageTask _writeSourceFilesToStorageTask;

    public WriteIncrementedVersionTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        RemoteStateLoadTask storageStateTask,
        IEnumerable<GetStateOfSourceItemTask> getSourceItemStateTasks,
        IncrementalBackupModelOptionsV2 incrementalBackupModelOptions) :
        base(services.CommonServices.Log, events, Localization.Resources.IncrementalBackup_Version_Save)
    {
        var childTaks = new List<BuTask>();

        var calculateIncrementedVersionForStorageTask = new CalculateIncrementedVersionForStorageTask(Log, Events, storageStateTask, getSourceItemStateTasks);
        childTaks.Add(calculateIncrementedVersionForStorageTask);

        _writeSourceFilesToStorageTask = new WriteSourceFilesToStorageTask(services, events, calculateIncrementedVersionForStorageTask);
        childTaks.Add(_writeSourceFilesToStorageTask);

        var writeStateToStorageTask = new WriteStateToStorageTask(
            services,
            events,
            calculateIncrementedVersionForStorageTask,
            _writeSourceFilesToStorageTask,
            incrementalBackupModelOptions);

        childTaks.Add(writeStateToStorageTask);
#pragma warning disable CS8603 // Possible null reference return.
        childTaks.Add(new WriteIntegrityVerificationScriptsToStorageTask(services, events,
            () => calculateIncrementedVersionForStorageTask.VersionIsNeeded,
            getState: () => calculateIncrementedVersionForStorageTask.IncrementalBackupState,
            _writeSourceFilesToStorageTask,
            writeStateToStorageTask,
            () => writeStateToStorageTask.StateStorageFile));
#pragma warning restore CS8603 // Possible null reference return.

        Children = childTaks;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        base.Execute();
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }
}
