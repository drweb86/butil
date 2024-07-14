using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Core.TasksTree.Storage;

internal class DeleteStorageFileTask : BuTaskV2
{
    private readonly StorageSpecificServicesIoc _services;
    private readonly string _relativeFileName;

    public DeleteStorageFileTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        string relativeFileName) :
        base(services.CommonServices.Log, events, string.Format(Resources.File_Deleting, relativeFileName))
    {
        _services = services;
        _relativeFileName = relativeFileName;
    }

    protected override void ExecuteInternal()
    {
        _services.Storage.Delete(_relativeFileName);
    }
}
