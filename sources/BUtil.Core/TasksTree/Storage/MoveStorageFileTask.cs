using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Services;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.Storage;

internal class MoveStorageFileTask(
    StorageSpecificServicesIoc services,
    TaskEvents events,
    string fromRelativeFileName,
    string toRelativeFileName) : BuTaskV2(services.CommonServices.Log, events, string.Format(Resources.File_Moving, fromRelativeFileName, toRelativeFileName))
{
    protected override void ExecuteInternal()
    {
        services.Storage.Move(fromRelativeFileName, toRelativeFileName);
    }
}
