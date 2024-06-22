using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;
using System.IO;

namespace BUtil.Core.TasksTree.Synchronization;

[Obsolete("", true)]
internal class SynchronizationRemoteFileUpdateTaskObsolete : BuTaskV2
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly string _localFolder;
    private readonly string _relativeFileName;

    public SynchronizationRemoteFileUpdateTaskObsolete(SynchronizationServices synchronizationServices, TaskEvents events,
        string localFolder,
        string relativeFileName)
        : base(synchronizationServices.Log, events, $"[{Resources.DataStorage_Title}] {string.Format(Resources.File_Saving, relativeFileName)}")
    {
        _synchronizationServices = synchronizationServices;
        _localFolder = localFolder;
        _relativeFileName = relativeFileName;
    }

    protected override void ExecuteInternal()
    {
        _synchronizationServices.StorageSpecificServices.Storage.Upload(Path.Combine(_localFolder, _relativeFileName), _relativeFileName);
    }
}
