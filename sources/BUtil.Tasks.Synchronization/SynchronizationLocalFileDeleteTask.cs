using BUtil.Interop.Tasks.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Tasks.Synchronization.Synchronization;
using BUtil.Interop.Tasks.Core;
using System.IO;

namespace BUtil.Core.TasksTree.Synchronization;
class SynchronizationLocalFileDeleteTask(SynchronizationServices synchronizationServices, TaskEvents events,
    string localFolder, string relativeFileName) : BuTaskV2(synchronizationServices.CommonServices.Log, events, string.Format(Resources.File_Deleting, relativeFileName))
{
    private readonly string _localFolder = localFolder;
    private readonly string _relativeFileName = relativeFileName;

    protected override void ExecuteInternal()
    {
        File.Delete(FileHelper.Combine(_localFolder, _relativeFileName));
    }
}
