using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Core.TasksTree.Storage;

internal class MoveStorageFileTask(
    StorageSpecificServicesIoc services,
    TaskEvents events,
    string fromRelativeFileName,
    string toRelativeFileName) : BuTaskV2(services.CommonServices.Log, events, string.Format(Resources.File_Moving, fromRelativeFileName, toRelativeFileName))
{
    private readonly StorageSpecificServicesIoc _services = services;
    private readonly string _fromRelativeFileName = fromRelativeFileName;
    private readonly string _toRelativeFileName = toRelativeFileName;

    protected override void ExecuteInternal()
    {
        _services.Storage.Move(_fromRelativeFileName, _toRelativeFileName);
    }
}
