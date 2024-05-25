

using BUtil.Core.Hashing;
using BUtil.Core.Storages;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.Synchronization;

internal class SynchronizationHelper
{
    private IStorage _remoteStorage;
    private SynchronizationLocalStateService _localStateService;
    private SynchronizationActualFilesService _actualFilesService;
    private SynchronizationRemoteStateService _remoteStateService;

    public SynchronizationHelper(IHashService hashService, IStorage remoteStorage, string taskName, string localFolder, string? subfolder)
    {
        _remoteStorage = remoteStorage;
        _localStateService = new SynchronizationLocalStateService(taskName, localFolder, subfolder);
        _actualFilesService = new SynchronizationActualFilesService(hashService, localFolder);
        _remoteStateService = new SynchronizationRemoteStateService(_remoteStorage);
    }

    public void Sync(string taskName, string hiveFolder, string syncFolder)
    {
        // boom
        var localState = _localStateService.Load();
        var actualFiles = _actualFilesService.Calculate();
        var remoteState = _remoteStateService.Load();
        // done!

        if (remoteState == null)
        {
            UploadFirstRemoteVersion(syncFolder, actualFiles);
            return;
        }

        if (localState == null)
        {
            DownloadFirstVersion(syncFolder, remoteState);
            localState = _localStateService.Load()!;
            actualFiles = _actualFilesService.Calculate();
        }

        var syncService = new SynchronizationDecisionService();
        var syncItems = syncService.Decide(localState, actualFiles, remoteState);
        ExecuteActionsLocally(syncFolder, syncItems);
        ExecuteActionsRemotely(syncFolder, syncItems);

        if (syncItems.Any(x => x.RemoteAction != SynchronizationDecision.DoNothing ||
                    x.ActualFileAction != SynchronizationDecision.DoNothing ||
                    x.ForceUpdateState))
        {
            var state = _actualFilesService.Calculate();
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
                    _remoteStorage.Delete(item.RelativeFileName);
                    break;
                case SynchronizationDecision.Update:
                    _remoteStorage.Upload(Path.Combine(syncFolder, item.RelativeFileName), item.RelativeFileName);
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
                case SynchronizationDecision.Update:
                    _remoteStorage.Download(item.RelativeFileName, Path.Combine(syncFolder, item.RelativeFileName));
                    break;

            }
        }
    }

    private void DownloadFirstVersion(string syncFolder, SynchronizationState remoteState)
    {
        foreach (var item in remoteState.FileSystemEntries)
        {
            _remoteStorage.Download(item.RelativeFileName, Path.Combine(syncFolder, item.RelativeFileName));
        }
        _localStateService.Save(remoteState);
    }

    private void UploadFirstRemoteVersion(string syncFolder, SynchronizationState actualFiles)
    {
        foreach (var item in actualFiles.FileSystemEntries)
        {
            _remoteStorage.Upload(Path.Combine(syncFolder, item.RelativeFileName), item.RelativeFileName);
        }
        _localStateService.Save(actualFiles);
        _remoteStateService.Save(actualFiles);
    }
}
