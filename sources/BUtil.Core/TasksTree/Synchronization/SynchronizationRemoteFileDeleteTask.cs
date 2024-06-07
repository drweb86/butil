using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.Synchronization;
internal class SynchronizationRemoteFileDeleteTask : BuTaskV2
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly string _relativeFileName;

    public SynchronizationRemoteFileDeleteTask(SynchronizationServices synchronizationServices, TaskEvents events, 
        string relativeFileName)
        : base(synchronizationServices.Log, events, $"[{Resources.DataStorage_Title}] {string.Format(Resources.File_Deleting, relativeFileName)}")
    {
        _synchronizationServices = synchronizationServices;
        _relativeFileName = relativeFileName;
    }

    protected override void ExecuteInternal()
    {
        _synchronizationServices.StorageSpecificServices.Storage.Delete(_relativeFileName);
    }
}
