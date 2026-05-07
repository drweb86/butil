using BUtil.Core.ConfigurationFileModels.V2;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.Storages;

public static class StorageProviderRegistry
{
    private static readonly List<IStorageSettingsProvider> _providers = [];
    private static bool _defaultsRegistered;

    private static void EnsureDefaults()
    {
        if (_defaultsRegistered) return;
        _defaultsRegistered = true;
        _providers.Add(new FolderStorageSettingsProvider());
        _providers.Add(new SambaStorageSettingsProvider());
        _providers.Add(new SftpStorageSettingsProvider());
        _providers.Add(new FtpsStorageSettingsProvider());
    }

    // External assemblies call this to add new storage types.
    public static void Register(IStorageSettingsProvider provider)
    {
        EnsureDefaults();
        _providers.Add(provider);
    }

    public static IReadOnlyList<IStorageSettingsProvider> GetSupported()
    {
        EnsureDefaults();
        return [.. _providers.Where(p => p.IsSupported).OrderBy(p => p.Order)];
    }

    public static IStorageSettingsProvider? FindById(string storageId)
    {
        EnsureDefaults();
        return _providers.FirstOrDefault(p => p.StorageId == storageId);
    }

    public static IStorageSettingsProvider? FindForSettings(IStorageSettingsV2 settings)
    {
        EnsureDefaults();
        return _providers.FirstOrDefault(p => p.CanHandle(settings));
    }
}
