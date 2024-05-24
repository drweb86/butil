using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.Storage;
using System;

namespace BUtil.Core.TasksTree.States;

internal class WriteStateToStorageTask : BuTask
{
    private readonly IncrementalBackupModelOptionsV2 _incrementalBackupModelOptions;
    private readonly StorageSpecificServicesIoc _services;
    private readonly CalculateIncrementedVersionForStorageTask _getIncrementedVersionTask;
    private readonly WriteSourceFilesToStorageTask _writeSourceFilesToStorageTask;

    public StorageFile? StateStorageFile { get; private set; }

    public WriteStateToStorageTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        CalculateIncrementedVersionForStorageTask getIncrementedVersionTask,
        WriteSourceFilesToStorageTask writeSourceFilesToStorageTask,
        IncrementalBackupModelOptionsV2 incrementalBackupModelOptions)
        : base(services.Log, events, Localization.Resources.DataStorage_State_Saving)
    {
        _incrementalBackupModelOptions = incrementalBackupModelOptions;
        _services = services;
        _getIncrementedVersionTask = getIncrementedVersionTask;
        _writeSourceFilesToStorageTask = writeSourceFilesToStorageTask;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        if (!_getIncrementedVersionTask.VersionIsNeeded)
        {
            LogDebug("Version is not needed.");
            IsSuccess = true;
            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
            return;
        }

        if (!_writeSourceFilesToStorageTask.IsSuccess)
        {
            LogDebug("Writing source files to storage has failed. Skipping.");
            IsSuccess = false;
            UpdateStatus(ProcessingStatus.FinishedWithErrors);
            return;
        }

        try
        {
            StateStorageFile = _services.IncrementalBackupStateService.Write(_incrementalBackupModelOptions, _getIncrementedVersionTask.IncrementalBackupState ?? throw new InvalidOperationException());
            IsSuccess = StateStorageFile != null;
        }
        catch (Exception ex)
        {
            this.LogError(ex.Message);
            IsSuccess = false;
        }
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }
}
