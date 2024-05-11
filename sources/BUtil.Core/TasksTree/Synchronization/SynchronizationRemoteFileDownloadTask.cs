using BUtil.Core.Events;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;
using System.IO;

namespace BUtil.Core.TasksTree.Synchronization;
internal class SynchronizationRemoteFileDownloadTask : BuTaskV2
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly string _localFolder;
    private readonly string _relativeFileName;

    public SynchronizationRemoteFileDownloadTask(SynchronizationServices synchronizationServices, TaskEvents events,
        string localFolder, string relativeFileName)
        : base(synchronizationServices.Log, events, $"Download {relativeFileName}")
    {
        _synchronizationServices = synchronizationServices;
        _localFolder = localFolder;
        _relativeFileName = relativeFileName;
    }

    protected override void ExecuteInternal()
    {
        _synchronizationServices.RemoteStorage.Download(_relativeFileName, Path.Combine(_localFolder, _relativeFileName));
    }
}
