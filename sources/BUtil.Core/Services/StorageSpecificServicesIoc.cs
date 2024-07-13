using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Hashing;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.Synchronization;
using System;

namespace BUtil.Core.TasksTree.IncrementalModel;

public class StorageSpecificServicesIoc : IDisposable
{
    public ILog Log { get; }
    public IStorageSettingsV2 StorageSettings { get; }

    private readonly Lazy<IStorage> _storage;
    public IStorage Storage { get { return _storage.Value; } }

    private readonly Lazy<IncrementalBackupStateService> _incrementalBackupStateService;
    public IncrementalBackupStateService IncrementalBackupStateService { get { return _incrementalBackupStateService.Value; } }

    private readonly Lazy<IncrementalBackupFileService> _incrementalBackupFileService;
    public IncrementalBackupFileService IncrementalBackupFileService { get { return _incrementalBackupFileService.Value; } }

    public StorageSpecificServicesIoc(CommonServicesIoc commonServices, IStorageSettingsV2 storageSettings, bool autodetectConnectionSettings = false)
    {
        Log = commonServices.Log;
        StorageSettings = storageSettings;
        _storage = new Lazy<IStorage>(() => StorageFactory.Create(Log, storageSettings, autodetectConnectionSettings));
        _incrementalBackupStateService = new Lazy<IncrementalBackupStateService>(() => new IncrementalBackupStateService(this, commonServices.HashService));
        _incrementalBackupFileService = new Lazy<IncrementalBackupFileService>(() => new IncrementalBackupFileService(commonServices.HashService, this));
    }

    public void Dispose()
    {
        if (_storage.IsValueCreated)
            _storage.Value.Dispose();
    }
}
