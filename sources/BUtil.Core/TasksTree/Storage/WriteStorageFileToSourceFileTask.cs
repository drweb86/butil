using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;

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
    private readonly StorageSpecificServicesIoc _storageSpecificServices = storageSpecificServices;
    private readonly SourceItemV2 _sourceItem = sourceItem;
    private readonly StorageFile _storageFile = storageFile;
    private readonly string _destinationFolder = destinationFolder;

    protected override void ExecuteInternal()
    {
        if (!_storageSpecificServices.IncrementalBackupFileService.Download(_sourceItem, _storageFile, _destinationFolder))
            throw new Exception("Download has failed!");
    }
}
