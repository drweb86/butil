using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.Storages;

public static class StorageProviderRegistry
{
    private sealed record ProviderEntry(
        IStorageSettingsProvider Provider,
        Type SettingsType,
        Func<ILog, IStorageSettingsV2, bool, IStorage>? Factory);

    private static readonly List<ProviderEntry> _entries = [];
    private static bool _defaultsRegistered;

    private static void EnsureDefaults()
    {
        if (_defaultsRegistered) return;
        _defaultsRegistered = true;
        // FolderStorage is the only built-in storage that stays in BUtil.Core.
        // All other storages (SFTP, FTPS, SMB/CIFS) are registered from their
        // own libraries at application startup via SftpStoragePlugin.Register(),
        // FtpsStoragePlugin.Register(), and CrossPlatformExperience.RegisterPlatformStorages().
        Register(new FolderStorageSettingsProvider(), typeof(FolderStorageSettingsV2),
            (log, s, _) => new FolderStorage(log, (FolderStorageSettingsV2)s));
    }

    /// <summary>
    /// Registers a storage provider with its settings type and runtime factory.
    /// The provider's StorageId is used as the JSON type discriminator.
    /// </summary>
    public static void Register(
        IStorageSettingsProvider provider,
        Type settingsType,
        Func<ILog, IStorageSettingsV2, bool, IStorage> factory)
    {
        EnsureDefaults();
        _entries.Add(new ProviderEntry(provider, settingsType, factory));
    }

    /// <summary>
    /// Registers a UI-only provider (no storage creation, no JSON serialization support).
    /// </summary>
    public static void Register(IStorageSettingsProvider provider)
    {
        EnsureDefaults();
        _entries.Add(new ProviderEntry(provider, typeof(object), null));
    }

    public static IReadOnlyList<IStorageSettingsProvider> GetSupported()
    {
        EnsureDefaults();
        return [.. _entries.Select(e => e.Provider).Where(p => p.IsSupported).OrderBy(p => p.Order)];
    }

    public static IStorageSettingsProvider? FindById(string storageId)
    {
        EnsureDefaults();
        return _entries.FirstOrDefault(e => e.Provider.StorageId == storageId)?.Provider;
    }

    public static IStorageSettingsProvider? FindForSettings(IStorageSettingsV2 settings)
    {
        EnsureDefaults();
        return _entries.FirstOrDefault(e => e.Provider.CanHandle(settings))?.Provider;
    }

    /// <summary>
    /// Returns the concrete settings type registered for the given JSON type discriminator, or null if not found.
    /// </summary>
    public static Type? GetSettingsType(string discriminator)
    {
        EnsureDefaults();
        return _entries.FirstOrDefault(e => e.Provider.StorageId == discriminator)?.SettingsType;
    }

    /// <summary>
    /// Returns the JSON type discriminator for the given concrete settings type, or null if not registered.
    /// </summary>
    public static string? GetDiscriminator(Type settingsType)
    {
        EnsureDefaults();
        return _entries.FirstOrDefault(e => e.SettingsType == settingsType)?.Provider.StorageId;
    }

    /// <summary>
    /// Creates an IStorage instance for the given settings using the registered factory.
    /// </summary>
    public static IStorage CreateStorage(ILog log, IStorageSettingsV2 settings, bool autodetectConnectionSettings)
    {
        EnsureDefaults();
        var entry = _entries.FirstOrDefault(e => e.Provider.CanHandle(settings));
        if (entry == null)
            throw new ArgumentOutOfRangeException(nameof(settings), $"No provider registered for settings type '{settings.GetType().Name}'.");
        if (entry.Factory == null)
            throw new InvalidOperationException($"Storage provider '{entry.Provider.StorageId}' was registered without a factory and cannot create IStorage instances.");
        return entry.Factory(log, settings, autodetectConnectionSettings);
    }
}
