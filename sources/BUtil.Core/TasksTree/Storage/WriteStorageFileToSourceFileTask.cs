using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;

namespace BUtil.Core.TasksTree.Storage;

public class WriteStorageFileToSourceFileTask : BuTaskV2
{
    private readonly StorageSpecificServicesIoc _storageSpecificServices;
    private readonly SourceItemV2 _sourceItem;
    private readonly StorageFile _storageFile;
    private readonly string _destinationFolder;

    public WriteStorageFileToSourceFileTask(
        StorageSpecificServicesIoc storageSpecificServices,
        TaskEvents events,
        SourceItemV2 sourceItem,
        StorageFile storageFile,
        string destinationFolder)
        : base(
            storageSpecificServices.Log,
            events,
            string.Format(Resources.File_Saving, SourceItemHelper.GetFriendlyFileName(sourceItem, storageFile.FileState.FileName)))
    {
        _storageSpecificServices = storageSpecificServices;
        _sourceItem = sourceItem;
        _storageFile = storageFile;
        _destinationFolder = destinationFolder;
    }

    protected override void ExecuteInternal()
    {
        if (!_storageSpecificServices.IncrementalBackupFileService.Download(_sourceItem, _storageFile, _destinationFolder))
            throw new Exception("Download has failed!");
    }
}
