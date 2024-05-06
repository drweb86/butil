using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.Synchronization;

internal class SynchronizationAllStatesReadTask : ParallelBuTask
{
    private readonly SynchronizationServices _synchronizationServices;

    public readonly SynchronizationLocalStateLoadTask SynchronizationLocalStateLoadTask;
    public readonly SynchronizationRemoteStateLoadTask SynchronizationRemoteStateLoadTask;
    public readonly SynchronizationReadActualFilesTask SynchronizationReadActualFilesTask;

    public SynchronizationAllStatesReadTask(SynchronizationServices synchronizationServices, TaskEvents events, TaskV2 task) : base(synchronizationServices.Log, events, "Read all states")
    {
        var tasks = new List<BuTask>();

        var options = (SynchronizationTaskModelOptionsV2)task.Model;
        _synchronizationServices = synchronizationServices;
        SynchronizationLocalStateLoadTask = new SynchronizationLocalStateLoadTask(_synchronizationServices, Events);
        SynchronizationRemoteStateLoadTask = new SynchronizationRemoteStateLoadTask(_synchronizationServices, Events);
        SynchronizationReadActualFilesTask = new SynchronizationReadActualFilesTask(_synchronizationServices, Events, options.LocalFolder);
        tasks.Add(SynchronizationLocalStateLoadTask);
        tasks.Add(SynchronizationRemoteStateLoadTask);
        tasks.Add(SynchronizationReadActualFilesTask);

        Children = tasks;
    }

    public override void Execute()
    {
        this.UpdateStatus(ProcessingStatus.InProgress);
        try
        {
            base.Execute();
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
