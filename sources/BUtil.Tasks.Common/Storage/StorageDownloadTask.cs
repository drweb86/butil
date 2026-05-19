using BUtil.Interop.Tasks.Events;
using BUtil.Core.Localization;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Interop.Tasks.Core;

namespace BUtil.Tasks.Common.Storage;

internal class StorageDownloadTask(
    StorageSpecificServicesIoc storageSpecificServicesIoc,
    TaskEvents events,
    StorageFile storageFile,
    string destinationFile,
    string fileTitle) : BuTaskV2(storageSpecificServicesIoc.CommonServices.Log, events, string.Format(Resources.File_Saving, fileTitle))
{
    protected override void ExecuteInternal()
    {
        storageSpecificServicesIoc.ApplicationStorageService.Download(storageFile, destinationFile);
    }
}
