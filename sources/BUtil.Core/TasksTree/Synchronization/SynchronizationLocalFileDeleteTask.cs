using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System.IO;

namespace BUtil.Core.TasksTree.Synchronization;
internal class SynchronizationLocalFileDeleteTask : BuTaskV2
{
    private readonly string _localFolder;
    private readonly string _relativeFileName;

    public SynchronizationLocalFileDeleteTask(SynchronizationServices synchronizationServices, TaskEvents events,
        string localFolder, string relativeFileName)
        : base(synchronizationServices.Log, events, string.Format(Resources.File_Deleting, relativeFileName))
    {
        _localFolder = localFolder;
        _relativeFileName = relativeFileName;
    }

    protected override void ExecuteInternal()
    {
        File.Delete(FileHelper.Combine(_localFolder, _relativeFileName));
    }
}
