using BUtil.Core.Events;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;

namespace BUtil.Core.TasksTree.Synchronization;

internal class SynchronizationRemoteStateLoadTask: BuTask
{
    private readonly SynchronizationServices _synchronizationServices;

    public SynchronizationState? SynchronizationState { get; private set; }

    public SynchronizationRemoteStateLoadTask(SynchronizationServices synchronizationServices, TaskEvents events)
        : base(synchronizationServices.Log, events, "Load remote state")
    {
        _synchronizationServices = synchronizationServices;
    }

    public override void Execute()
    {
        this.UpdateStatus(ProcessingStatus.InProgress);
        try
        {
            SynchronizationState = _synchronizationServices.RemoteStateService.Load();
            IsSuccess = true;
            this.UpdateStatus(ProcessingStatus.FinishedSuccesfully);
        }
        catch (Exception ex)
        {
            this.LogError(ex.ToString());
            IsSuccess = false;
            this.UpdateStatus(ProcessingStatus.FinishedWithErrors);
        }
    }
}