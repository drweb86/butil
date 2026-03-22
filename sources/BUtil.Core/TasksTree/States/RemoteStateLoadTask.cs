using BUtil.Core.Events;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.States;

internal class RemoteStateLoadTask(StorageSpecificServicesIoc services, TaskEvents events, string password) : BuTaskV2(services.CommonServices.Log, events, Localization.Resources.DataStorage_State_Get)
{
    private IncrementalBackupState? _storageState;

    public IncrementalBackupState GetSuccessResult()
    {
        this.EnsureSuccess();

        return _storageState.EnsureNotNull();
    }

    protected override void ExecuteInternal()
    {
        if (!services.IncrementalBackupStateService.TryRead(password, out var state))
            throw new System.InvalidOperationException("Failed to read state!");
        _storageState = state;
    }
}
