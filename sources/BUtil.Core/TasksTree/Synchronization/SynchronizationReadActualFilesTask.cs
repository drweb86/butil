using BUtil.Core.Events;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.Synchronization;
internal class SynchronizationReadActualFilesTask: SequentialBuTask
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly string _syncFolder;

    public SynchronizationState? SynchronizationState { get; private set; }

    public SynchronizationReadActualFilesTask(SynchronizationServices synchronizationServices, TaskEvents events, string syncFolder)
        : base(synchronizationServices.Log, events, "Read files state")
    {
        _synchronizationServices = synchronizationServices;
        _syncFolder = syncFolder;
    }

    public override void Execute()
    {
        this.UpdateStatus(ProcessingStatus.InProgress);
        try
        {
            var childTasks = Directory
                .GetFiles(_syncFolder, "*.*", SearchOption.AllDirectories)
                .Select(x => new SynchronizationReadActualFileTask(_synchronizationServices, Events, _syncFolder, x))
                .ToList();
            Children = childTasks;
            Events.DuringExecutionTasksAdded(this.Id, childTasks);

            base.Execute();

            SynchronizationState = new SynchronizationState();
            childTasks
                .Where(x => x.IsSuccess)
                .Select(x => x.StateFile!)
                .ToList()
                .ForEach(SynchronizationState.FileSystemEntries.Add);

            IsSuccess = childTasks.All(x => x.IsSuccess);
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
