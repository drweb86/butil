using BUtil.Core.Events;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.Synchronization;
[Obsolete("", true)]
internal class SynchronizationRemoteStateSaveTaskObsolete: BuTaskV2
{
    public SynchronizationRemoteStateSaveTaskObsolete(
        IEnumerable<BuTask> dependantTasks,
        SynchronizationServices synchronizationServices,
        TaskEvents events)
        : base(synchronizationServices.Log, events, Localization.Resources.DataStorage_State_Saving)
    {
    }

    protected override void ExecuteInternal()
    {
    }
}