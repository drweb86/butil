using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.Synchronization;
internal class SynchronizationLocalStateLoadTask(SynchronizationServices synchronizationServices, TaskEvents events) : BuTaskV2(synchronizationServices.CommonServices.Log, events, Resources.Local_State_Get)
{
    private readonly SynchronizationServices _synchronizationServices = synchronizationServices;

    public SynchronizationState? SynchronizationState { get; private set; }

    protected override void ExecuteInternal()
    {
        SynchronizationState = _synchronizationServices.LocalStateService.Load();
    }
}
