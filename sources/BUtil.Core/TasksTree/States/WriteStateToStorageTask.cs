using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.Storage;
using System;

namespace BUtil.Core.TasksTree.States;

internal class WriteStateToStorageTask(
    StorageSpecificServicesIoc services,
    TaskEvents events,
    CalculateIncrementedVersionForStorageTask getIncrementedVersionTask,
    WriteSourceFilesToStorageTask writeSourceFilesToStorageTask,
    IncrementalBackupModelOptionsV2 incrementalBackupModelOptions) : BuTaskV2(services.CommonServices.Log, events, Localization.Resources.DataStorage_State_Saving)
{
    private readonly IncrementalBackupModelOptionsV2 _incrementalBackupModelOptions = incrementalBackupModelOptions;
    private readonly StorageSpecificServicesIoc _services = services;
    private readonly CalculateIncrementedVersionForStorageTask _getIncrementedVersionTask = getIncrementedVersionTask;
    private readonly WriteSourceFilesToStorageTask _writeSourceFilesToStorageTask = writeSourceFilesToStorageTask;

    public StorageFile? StateStorageFile { get; private set; }

    protected override void ExecuteInternal()
    {
        if (!_getIncrementedVersionTask.VersionIsNeeded)
        {
            LogDebug("Version is not needed.");
            IsSkipped = true;
            return;
        }

        if (!_writeSourceFilesToStorageTask.IsSuccess)
            throw new Exception("Writing source files to storage has failed. Skipping.");

        StateStorageFile = _services.IncrementalBackupStateService.Write(_incrementalBackupModelOptions.Password, _getIncrementedVersionTask.IncrementalBackupState ?? throw new InvalidOperationException());
        if (StateStorageFile == null)
            throw new InvalidOperationException("Failed to upload state storage file.");
    }
}


internal class WriteStateToStorageDirectTask(
    StorageSpecificServicesIoc services,
    TaskEvents events,
    IncrementalBackupState incrementalBackupState,
    IncrementalBackupModelOptionsV2 incrementalBackupModelOptions) : BuTaskV2(services.CommonServices.Log, events, Localization.Resources.DataStorage_State_Saving)
{
    private readonly IncrementalBackupModelOptionsV2 _incrementalBackupModelOptions = incrementalBackupModelOptions;
    private readonly StorageSpecificServicesIoc _services = services;
    private readonly IncrementalBackupState _incrementalBackupState = incrementalBackupState;

    public StorageFile? StateStorageFile { get; private set; }

    protected override void ExecuteInternal()
    {
        StateStorageFile = _services.IncrementalBackupStateService.Write(_incrementalBackupModelOptions.Password, _incrementalBackupState) ?? 
            throw new InvalidOperationException("Failed to upload state storage file.");
    }
}
