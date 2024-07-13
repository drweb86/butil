using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.Synchronization;
internal class SynchronizationLocalStateLoadTask: BuTaskV2
{
    private readonly SynchronizationServices _synchronizationServices;

    public SynchronizationState? SynchronizationState { get; private set; }

    public SynchronizationLocalStateLoadTask(SynchronizationServices synchronizationServices, TaskEvents events) 
        : base(synchronizationServices.CommonServices.Log, events, Resources.Local_State_Get)
    {
        _synchronizationServices = synchronizationServices;
    }

    protected override void ExecuteInternal()
    {
        SynchronizationState = _synchronizationServices.LocalStateService.Load();
    }
}
