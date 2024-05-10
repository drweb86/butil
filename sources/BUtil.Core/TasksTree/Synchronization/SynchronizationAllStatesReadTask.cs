using BUtil.Core.Events;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.Synchronization;

internal class SynchronizationAllStatesReadTask : ParallelBuTask
{
    private readonly SynchronizationServices _synchronizationServices;

    private readonly SynchronizationLocalStateLoadTask _synchronizationLocalStateLoadTask;
    private readonly SynchronizationRemoteStateLoadTask _synchronizationRemoteStateLoadTask;
    private readonly SynchronizationReadActualFilesTask _synchronizationReadActualFilesTask;

    public SynchronizationState? LocalState => _synchronizationLocalStateLoadTask.SynchronizationState;
    public SynchronizationState? RemoteState => _synchronizationRemoteStateLoadTask.SynchronizationState;
    public SynchronizationState ActualFiles => _synchronizationReadActualFilesTask.SynchronizationState!;


    public SynchronizationAllStatesReadTask(
        SynchronizationServices synchronizationServices, 
        TaskEvents events, string localFolder) : 
        base(synchronizationServices.Log, events, "Read all states")
    {
        _synchronizationServices = synchronizationServices;
        _synchronizationLocalStateLoadTask = new SynchronizationLocalStateLoadTask(_synchronizationServices, Events);
        _synchronizationRemoteStateLoadTask = new SynchronizationRemoteStateLoadTask(_synchronizationServices, Events);
        _synchronizationReadActualFilesTask = new SynchronizationReadActualFilesTask(_synchronizationServices, Events, localFolder);

        Children = new List<BuTask>
        {
            _synchronizationLocalStateLoadTask,
            _synchronizationRemoteStateLoadTask,
            _synchronizationReadActualFilesTask
        };
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        try
        {
            base.Execute();
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
        catch (Exception ex)
        {
            LogError(ex.ToString());
            UpdateStatus(ProcessingStatus.FinishedWithErrors);
        }
    }
}
