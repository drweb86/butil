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
    private readonly string _localFolder;

    public SynchronizationState? SynchronizationState { get; private set; }

    public SynchronizationReadActualFilesTask(SynchronizationServices synchronizationServices, TaskEvents events, string localFolder)
        : base(synchronizationServices.Log, events, string.Format(BUtil.Core.Localization.Resources.SourceItem_State_Get, localFolder))
    {
        _synchronizationServices = synchronizationServices;
        _localFolder = localFolder;
    }

    public override void Execute()
    {
        this.UpdateStatus(ProcessingStatus.InProgress);
        try
        {
            var childTasks = Directory
                .GetFiles(_localFolder, "*.*", SearchOption.AllDirectories)
                .Select(x => new SynchronizationReadActualFileTask(_synchronizationServices, Events, _localFolder, x))
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
