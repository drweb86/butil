using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Hashing;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Storages;
using System;

namespace BUtil.Core.Synchronization;

internal class SynchronizationServices : IDisposable
{
    public readonly ILog Log;
    public readonly IHashService HashService;
    public readonly ICashedHashStoreService CachedHashStoreService;
    public readonly SynchronizationLocalStateService LocalStateService;
    public readonly SynchronizationActualFilesService ActualFilesService;

    private readonly Lazy<SynchronizationRemoteStateService> _remoteStateService;
    public SynchronizationRemoteStateService RemoteStateService => _remoteStateService.Value;

    private readonly Lazy<IStorage> _remoteStorage;
    public IStorage RemoteStorage => _remoteStorage.Value;

    public SynchronizationServices(ILog log, string taskName, string localFolder, string? subfolder, IStorageSettingsV2 remoteStorageSettings, bool autodetectConnectionSettings)
    {
        Log = log;
        CachedHashStoreService = new CashedHashStoreService();
        HashService = new CachedHashService(CachedHashStoreService);
        LocalStateService = new SynchronizationLocalStateService(taskName, localFolder, subfolder);
        _remoteStorage = new Lazy<IStorage>(() => StorageFactory.Create(log, remoteStorageSettings, autodetectConnectionSettings));
        _remoteStateService = new Lazy<SynchronizationRemoteStateService>(() => new SynchronizationRemoteStateService(RemoteStorage));
        ActualFilesService = new SynchronizationActualFilesService(HashService, localFolder);
    }


    public void Dispose()
    {
        HashService.Dispose();
    }
}
