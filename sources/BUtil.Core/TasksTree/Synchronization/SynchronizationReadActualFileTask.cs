using BUtil.Core.Events;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;

namespace BUtil.Core.TasksTree.Synchronization;
internal class SynchronizationReadActualFileTask : BuTaskV2
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly string _file;

    public SynchronizationStateFile? StateFile { get; private set; }

    public SynchronizationReadActualFileTask(SynchronizationServices synchronizationServices, TaskEvents events, string localFolder, string fullPath)
        : base(synchronizationServices.Log, events, $"Get state of {fullPath.Substring(localFolder.Length + 1)}")
    {
        _synchronizationServices = synchronizationServices;
        _file = fullPath;
    }

    protected override void ExecuteInternal()
    {
        StateFile = _synchronizationServices.ActualFilesService.CalculateItem(_file);
    }
}
