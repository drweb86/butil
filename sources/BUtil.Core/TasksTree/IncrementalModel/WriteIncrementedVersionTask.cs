using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.States;
using BUtil.Core.TasksTree.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.IncrementalModel;

class WriteIncrementedVersionTask : SequentialBuTask
{
    private readonly WriteSourceFilesToStorageTask _writeSourceFilesToStorageTask;

    public WriteIncrementedVersionTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        Func<IncrementalBackupState> getRemoteState,
        IEnumerable<GetStateOfSourceItemTask> getSourceItemStateTasks,
        IncrementalBackupModelOptionsV2 incrementalBackupModelOptions) :
        base(services.CommonServices.Log, events, Localization.Resources.IncrementalBackup_Version_Save)
    {
        var childTaks = new List<BuTask>();

        var calculateIncrementedVersionForStorageTask = new CalculateIncrementedStateTask(Log, Events, getRemoteState, () => GetLocalStates(getSourceItemStateTasks));
        childTaks.Add(calculateIncrementedVersionForStorageTask);

        _writeSourceFilesToStorageTask = new WriteSourceFilesToStorageTask(services, events, calculateIncrementedVersionForStorageTask.GetSuccessResult);
        childTaks.Add(_writeSourceFilesToStorageTask);

        var writeStateToStorageTask = new WriteStateToStorageTask(
            services,
            events,
            calculateIncrementedVersionForStorageTask.GetSuccessResult,
            _writeSourceFilesToStorageTask,
            incrementalBackupModelOptions);

        childTaks.Add(writeStateToStorageTask);
#pragma warning disable CS8603 // Possible null reference return.
        childTaks.Add(new WriteIntegrityVerificationScriptsToStorageTask(services, events,
            calculateIncrementedVersionForStorageTask.GetSuccessResult,
            _writeSourceFilesToStorageTask,
            writeStateToStorageTask,
            () => writeStateToStorageTask.StateStorageFile));
#pragma warning restore CS8603 // Possible null reference return.

        Children = childTaks;
    }

    private static IEnumerable<SourceItemState> GetLocalStates(IEnumerable<GetStateOfSourceItemTask> getStateOfSourceItemTasks)
    {
        return [.. getStateOfSourceItemTasks.Select(x => x.EnsureSuccess().SourceItemState.EnsureNotNull())];
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        base.Execute();
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }
}
