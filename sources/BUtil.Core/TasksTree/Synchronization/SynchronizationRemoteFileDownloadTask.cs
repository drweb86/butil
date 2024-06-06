using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;
using System.IO;

namespace BUtil.Core.TasksTree.Synchronization;
internal class SynchronizationRemoteFileDownloadTask : BuTaskV2
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly string _localFolder;
    private readonly string _relativeFileName;
    private readonly DateTime? _lastWriteTimeUtc;
    private readonly string? _repositorySubfolder;

    public SynchronizationRemoteFileDownloadTask(SynchronizationServices synchronizationServices, TaskEvents events,
        string localFolder, 
        string relativeFileName,
        DateTime? lastWriteTimeUtc,
        string? repositorySubfolder)
        : base(synchronizationServices.Log, events, string.Format(Resources.File_Saving, relativeFileName))
    {
        _synchronizationServices = synchronizationServices;
        _localFolder = localFolder;
        _relativeFileName = relativeFileName;
        _lastWriteTimeUtc = lastWriteTimeUtc;
        _repositorySubfolder = repositorySubfolder;
    }

    protected override void ExecuteInternal()
    {
        var destinationFile = Path.Combine(_localFolder, _relativeFileName)!;
        FileHelper.EnsureFolderCreatedForFile(destinationFile);

        var actualRemoteFile = FileHelper.Combine(_repositorySubfolder, _relativeFileName);
        _synchronizationServices.RemoteStorage.Download(actualRemoteFile, destinationFile);
        if (_lastWriteTimeUtc != null)
        {
            File.SetLastWriteTimeUtc(destinationFile, _lastWriteTimeUtc.Value);
        }
    }
}
