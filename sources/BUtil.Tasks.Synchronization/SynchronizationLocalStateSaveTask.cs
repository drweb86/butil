using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.Synchronization;
internal class SynchronizationLocalStateSaveTask(
    IEnumerable<BuTask> dependantTasks,
    SynchronizationServices synchronizationServices,
    TaskEvents events,
    Func<SynchronizationState> getSynchronizationState) : BuTaskV2(synchronizationServices.CommonServices.Log, events, Resources.Local_State_Saving)
{
    private readonly IEnumerable<BuTask> _dependantTasks = dependantTasks;
    private readonly SynchronizationServices _synchronizationServices = synchronizationServices;
    private readonly Func<SynchronizationState> _getSynchronizationState = getSynchronizationState;

    protected override void ExecuteInternal()
    {
        if (_dependantTasks.Any(x => !x.IsSuccess))
        {
            throw new Exception("Dependant task are not ok");
        }

        var state = _getSynchronizationState();
        _synchronizationServices.LocalStateService.Save(state);
    }
}
