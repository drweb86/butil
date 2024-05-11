using BUtil.Core.Events;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;

namespace BUtil.Core.TasksTree.Synchronization;

internal class SynchronizationRemoteStateLoadTask: BuTaskV2
{
    private readonly SynchronizationServices _synchronizationServices;

    public SynchronizationState? SynchronizationState { get; private set; }

    public SynchronizationRemoteStateLoadTask(SynchronizationServices synchronizationServices, TaskEvents events)
        : base(synchronizationServices.Log, events, "Load remote state")
    {
        _synchronizationServices = synchronizationServices;
    }

    protected override void ExecuteInternal()
    {
        SynchronizationState = _synchronizationServices.RemoteStateService.Load();
    }
}