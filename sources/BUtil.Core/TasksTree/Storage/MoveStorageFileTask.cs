using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Core.TasksTree.Storage;

internal class MoveStorageFileTask : BuTaskV2
{
    private readonly StorageSpecificServicesIoc _services;
    private readonly string _fromRelativeFileName;
    private readonly string _toRelativeFileName;

    public MoveStorageFileTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        string fromRelativeFileName,
        string toRelativeFileName) :
        base(services.Log, events, string.Format(Resources.File_Moving, fromRelativeFileName, toRelativeFileName))
    {
        _services = services;
        _fromRelativeFileName = fromRelativeFileName;
        _toRelativeFileName = toRelativeFileName;
    }

    protected override void ExecuteInternal()
    {
        _services.Storage.Move(_fromRelativeFileName, _toRelativeFileName);
    }
}
