using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.Storage;
using System;

namespace BUtil.Core.TasksTree.States;

class WriteStateToStorageTask(
    StorageSpecificServicesIoc services,
    TaskEvents events,
    Func<(bool versionIsNeeded, IncrementalBackupState updatedState)> getIncrementedState,
    WriteSourceFilesToStorageTask writeSourceFilesToStorageTask,
    IncrementalBackupModelOptionsV2 incrementalBackupModelOptions) : BuTaskV2(services.CommonServices.Log, events, Localization.Resources.DataStorage_State_Saving)
{
    public StorageFile? StateStorageFile { get; private set; }

    protected override void ExecuteInternal()
    {
        var (versionIsNeeded, updatedState) = getIncrementedState();
        if (!versionIsNeeded)
        {
            LogDebug("Version is not needed.");
            IsSkipped = true;
            return;
        }

        if (!writeSourceFilesToStorageTask.IsSuccess)
            throw new Exception("Writing source files to storage has failed. Skipping.");

        StateStorageFile = services.IncrementalBackupStateService.Write(incrementalBackupModelOptions.Password, updatedState);
    }
}
