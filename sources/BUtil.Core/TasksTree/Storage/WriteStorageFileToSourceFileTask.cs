using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.Storage;

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
