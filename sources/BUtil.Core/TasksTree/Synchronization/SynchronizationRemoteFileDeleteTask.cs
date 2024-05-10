using BUtil.Core.Events;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.Synchronization;
internal class SynchronizationRemoteFileDeleteTask : BuTaskV2
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly string _relativeFileName;

    public SynchronizationRemoteFileDeleteTask(SynchronizationServices synchronizationServices, TaskEvents events, 
        string relativeFileName)
        : base(synchronizationServices.Log, events, $"Delete {relativeFileName}")
    {
        _synchronizationServices = synchronizationServices;
        _relativeFileName = relativeFileName;
    }

    protected override void ExecuteInternal()
    {
        _synchronizationServices.RemoteStorage.Delete(_relativeFileName);
    }
}
