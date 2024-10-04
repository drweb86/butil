using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.Storage;

internal class StorageDownloadTask : BuTaskV2
{
    private readonly StorageSpecificServicesIoc _storageSpecificServicesIoc;
    private readonly StorageFile _storageFile;
    private readonly string _destinationFile;

    public StorageDownloadTask(
        StorageSpecificServicesIoc storageSpecificServicesIoc,
        TaskEvents events,
        StorageFile storageFile, 
        string destinationFile,
        string fileTitle)
    : base(storageSpecificServicesIoc.CommonServices.Log, events, string.Format(Resources.File_Saving, fileTitle))
    {
        _storageSpecificServicesIoc = storageSpecificServicesIoc;
        _storageFile = storageFile;
        _destinationFile = destinationFile;
    }

    protected override void ExecuteInternal()
    {
        _storageSpecificServicesIoc.ApplicationStorageService.Download(_storageFile, _destinationFile);
    }
}
