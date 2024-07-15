using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Core.TasksTree.Storage;

internal class DeleteStorageFileTask(
    StorageSpecificServicesIoc services,
    TaskEvents events,
    string relativeFileName) : BuTaskV2(services.CommonServices.Log, events, string.Format(Resources.File_Deleting, relativeFileName))
{
    private readonly StorageSpecificServicesIoc _services = services;
    private readonly string _relativeFileName = relativeFileName;

    protected override void ExecuteInternal()
    {
        _services.Storage.Delete(_relativeFileName);
    }
}
