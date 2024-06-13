using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.State;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.Synchronization;

internal class SynchronizationRemoteFileDownloadTask : BuTaskV2
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly StorageFile _storageFile;
    private readonly string _destinationFile;

    public SynchronizationRemoteFileDownloadTask(SynchronizationServices synchronizationServices, TaskEvents events,
        SynchronizationModel model, string relativeFileName)
    : base(
        synchronizationServices.Log,
        events,
        string.Format(Resources.File_Saving, relativeFileName))
    {
        var actualRemoteRelativeFileName = FileHelper.Combine(FileHelper.NormalizeRelativePath(model.TaskOptions.RepositorySubfolder), relativeFileName);
        var actualRemoteFile = Path.Combine(model.RemoteSourceItem.Target, actualRemoteRelativeFileName);
        _storageFile = model.RemoteStorageFiles.Single(x => FileHelper.CompareFileNames(x.FileState.FileName, actualRemoteFile));
        _synchronizationServices = synchronizationServices;
        _destinationFile = Path.Combine(model.TaskOptions.LocalFolder, relativeFileName)!;
    }

    protected override void ExecuteInternal()
    {
        if (!_synchronizationServices.StorageSpecificServices.IncrementalBackupFileService.Download(_storageFile, _destinationFile))
        {
            throw new InvalidDataException($"Failed to download {_storageFile.StorageFileName}!");
        }
        File.SetLastWriteTimeUtc(_destinationFile, _storageFile.FileState.LastWriteTimeUtc);
    }
}
