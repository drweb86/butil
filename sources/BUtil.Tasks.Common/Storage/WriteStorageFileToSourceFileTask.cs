using BUtil.Interop.Tasks;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Tasks.Events;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Interop.Tasks.Core;

namespace BUtil.Tasks.Common.Storage;

public class WriteStorageFileToSourceFileTask(
    StorageSpecificServicesIoc storageSpecificServices,
    TaskEvents events,
    SourceItemV2 sourceItem,
    StorageFile storageFile,
    string destinationFolder) : BuTaskV2(
        storageSpecificServices.CommonServices.Log,
        events,
        string.Format(Resources.File_Saving, SourceItemHelper.GetFriendlyFileName(sourceItem, storageFile.FileState.FileName)))
{
    protected override void ExecuteInternal()
    {
        storageSpecificServices.ApplicationStorageService.Download(sourceItem, storageFile, destinationFolder);
    }
}
