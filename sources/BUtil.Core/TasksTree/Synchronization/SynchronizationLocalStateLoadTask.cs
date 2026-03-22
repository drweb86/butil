using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.Synchronization;

class SynchronizationLocalStateLoadTask(SynchronizationServices synchronizationServices, TaskEvents events) : BuTaskV2(synchronizationServices.CommonServices.Log, events, Resources.Local_State_Get)
{
    public SynchronizationState? GetSuccessResult()
    {
        this.EnsureSuccess();
        return _synchronizationState;
    }

    private SynchronizationState? _synchronizationState;

    protected override void ExecuteInternal()
    {
        _synchronizationState = synchronizationServices.LocalStateService.Load();
    }
}
