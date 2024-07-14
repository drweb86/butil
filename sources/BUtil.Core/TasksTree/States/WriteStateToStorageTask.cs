using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.Storage;
using System;

namespace BUtil.Core.TasksTree.States;

internal class WriteStateToStorageTask : BuTaskV2
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
        : base(services.CommonServices.Log, events, Localization.Resources.DataStorage_State_Saving)
    {
        _incrementalBackupModelOptions = incrementalBackupModelOptions;
        _services = services;
        _getIncrementedVersionTask = getIncrementedVersionTask;
        _writeSourceFilesToStorageTask = writeSourceFilesToStorageTask;
    }

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
