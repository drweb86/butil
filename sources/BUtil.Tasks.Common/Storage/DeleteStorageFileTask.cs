using BUtil.Interop.Tasks.Events;
using BUtil.Core.Localization;
using BUtil.Core.Services;
using BUtil.Interop.Tasks.Core;

namespace BUtil.Tasks.Common.Storage;

internal class DeleteStorageFileTask(
    StorageSpecificServicesIoc services,
    TaskEvents events,
    string relativeFileName) : BuTaskV2(services.CommonServices.Log, events, string.Format(Resources.File_Deleting, relativeFileName))
{
    protected override void ExecuteInternal()
    {
        services.Storage.Delete(relativeFileName);
    }
}
