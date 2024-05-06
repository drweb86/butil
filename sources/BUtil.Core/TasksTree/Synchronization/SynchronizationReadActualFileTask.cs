using BUtil.Core.Events;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;

namespace BUtil.Core.TasksTree.Synchronization;
internal class SynchronizationReadActualFileTask : BuTask
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly string _file;

    public SynchronizationStateFile? StateFile { get; private set; }

    public SynchronizationReadActualFileTask(SynchronizationServices synchronizationServices, TaskEvents events, string path, string file)
        : base(synchronizationServices.Log, events, $"Get state of {file.Substring(path.Length + 1)}")
    {
        _synchronizationServices = synchronizationServices;
        _file = file;
    }

    public override void Execute()
    {
        this.UpdateStatus(ProcessingStatus.InProgress);
        try
        {
            StateFile = _synchronizationServices.ActualFilesService.CalculateItem(_file);
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
