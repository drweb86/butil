

using BUtil.Core.Hashing;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.Synchronization;

internal class SynchronizationHelper
{
    private SynchronizationRemoteStorageService _remoteStorageService;
    private SynchronizationLocalStateService _localStateService;
    private SynchronizationActualFilesService _actualFilesService;
    private SynchronizationRemoteStateService _remoteStateService;

    public SynchronizationHelper(IHashService hashService, string taskName, string hiveFolder, string syncFolder)
    {
        _remoteStorageService = new SynchronizationRemoteStorageService(hiveFolder);
        _localStateService = new SynchronizationLocalStateService(hashService, taskName, syncFolder);
        _actualFilesService = new SynchronizationActualFilesService(hashService);
        _remoteStateService = new SynchronizationRemoteStateService(_remoteStorageService);
    }

    public void Sync(string taskName, string hiveFolder, string syncFolder)
    {
        var localState = _localStateService.Load();
        var actualFiles = _actualFilesService.Calculate(syncFolder);
        var remoteState = _remoteStateService.Load();

        if (remoteState == null)
        {
            UploadFirstRemoteVersion(syncFolder, actualFiles);
            return;
        }

        if (localState == null)
        {
            DownloadFirstVersion(syncFolder, remoteState);
            localState = _localStateService.Load()!;
            actualFiles = _actualFilesService.Calculate(syncFolder);
        }

        var syncService = new SynchronizationDecisionService();
        var syncItems = syncService.Decide(localState, actualFiles, remoteState);
        ExecuteActionsLocally(syncFolder, syncItems);
        ExecuteActionsRemotely(syncFolder, syncItems);

        if (syncItems.Any(x => x.RemoteAction != SynchronizationDecision.DoNothing) ||
            syncItems.Any(x => x.ActualFileAction != SynchronizationDecision.DoNothing))
        {
            var state = _actualFilesService.Calculate(syncFolder);
            _localStateService.Save(state);
            _remoteStateService.Save(state);
        }
    }

    private void ExecuteActionsRemotely(string syncFolder, IEnumerable<SynchronizationConsolidatedFileInfo> syncItems)
    {
        foreach (var item in syncItems)
        {
            switch (item.RemoteAction)
            {
                case SynchronizationDecision.DoNothing:
                    break;
                case SynchronizationDecision.Delete:
                    _remoteStorageService.Delete(item.RelativeFileName);
                    break;
                case SynchronizationDecision.Create:
                case SynchronizationDecision.Update:
                    _remoteStorageService.Upload(syncFolder, item.RelativeFileName);
                    break;

            }
        }
    }

    private void ExecuteActionsLocally(string syncFolder, IEnumerable<SynchronizationConsolidatedFileInfo> syncItems)
    {
        foreach (var item in syncItems)
        {
            switch (item.ActualFileAction)
            {
                case SynchronizationDecision.DoNothing:
                    break;
                case SynchronizationDecision.Delete:
                    File.Delete(Path.Combine(syncFolder, item.RelativeFileName));
                    break;
                case SynchronizationDecision.Create:
                case SynchronizationDecision.Update:
                    _remoteStorageService.Download(syncFolder, item.RelativeFileName);
                    break;

            }
        }
    }

    private void DownloadFirstVersion(string syncFolder, SynchronizationState remoteState)
    {
        foreach (var item in remoteState.FileSystemEntries)
        {
            _remoteStorageService.Download(syncFolder, item.RelativeFileName);
        }
        _localStateService.Save(remoteState);
    }

    private void UploadFirstRemoteVersion(string syncFolder, SynchronizationState actualFiles)
    {
        foreach (var item in actualFiles.FileSystemEntries)
        {
            _remoteStorageService.Upload(syncFolder, item.RelativeFileName);
        }
        _localStateService.Save(actualFiles);
        _remoteStateService.Save(actualFiles);
    }
}
