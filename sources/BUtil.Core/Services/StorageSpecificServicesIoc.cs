using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.State;
using BUtil.Core.Storages;
using System;

namespace BUtil.Core.Services;

public class StorageSpecificServicesIoc : IDisposable
{
    public CommonServicesIoc CommonServices { get; }
    public IStorageSettingsV2 StorageSettings { get; }

    private readonly Lazy<IStorage> _storage;
    public IStorage Storage { get { return _storage.Value; } }

    private readonly Lazy<IncrementalBackupStateService> _incrementalBackupStateService;
    public IncrementalBackupStateService IncrementalBackupStateService { get { return _incrementalBackupStateService.Value; } }

    private readonly Lazy<ApplicationStorageService> _incrementalBackupFileService;
    public ApplicationStorageService ApplicationStorageService { get { return _incrementalBackupFileService.Value; } }

    public StorageSpecificServicesIoc(CommonServicesIoc commonServices, IStorageSettingsV2 storageSettings, bool autodetectConnectionSettings = false)
    {
        CommonServices = commonServices;
        StorageSettings = storageSettings;
        _storage = new Lazy<IStorage>(() => StorageFactory.Create(commonServices.Log, storageSettings, autodetectConnectionSettings));
        _incrementalBackupStateService = new Lazy<IncrementalBackupStateService>(() => new IncrementalBackupStateService(this, commonServices.CachedHashService));
        _incrementalBackupFileService = new Lazy<ApplicationStorageService>(() => new ApplicationStorageService(commonServices.CachedHashService, this));
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        if (_storage.IsValueCreated)
            _storage.Value.Dispose();
    }
}
