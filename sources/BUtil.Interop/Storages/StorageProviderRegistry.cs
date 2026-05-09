using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.Storages;

/// <summary>
/// Host applications bootstrap built-in providers from <c>BUtil.Core</c>; third-party storages register from
/// <see cref="IStoragePlugin"/> implementations. External packages target <c>BUtil.Interop</c> only (no dependency on Core).
/// </summary>
public static class StorageProviderRegistry
{
    internal static Action? HostEnsureBuiltIns { get; set; }

    private sealed record ProviderEntry(
        IStorageSettingsProvider Provider,
        Type SettingsType,
        Func<ILog, IStorageSettingsV2, bool, IStorage>? Factory);

    private static readonly List<ProviderEntry> Entries = [];

    private static readonly object EntriesLock = new();
    private static readonly object BootstrapLock = new();
    private static volatile bool _bootstrapDone;

    private static void EnsureBootstrapped()
    {
        if (_bootstrapDone)
            return;

        lock (BootstrapLock)
        {
            if (_bootstrapDone)
                return;

            HostEnsureBuiltIns?.Invoke();
            _bootstrapDone = true;
        }
    }

    internal static void RegisterBuiltIn(
        IStorageSettingsProvider provider,
        Type settingsType,
        Func<ILog, IStorageSettingsV2, bool, IStorage> factory)
    {
        lock (EntriesLock)
            Entries.Add(new ProviderEntry(provider, settingsType, factory));
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
        EnsureBootstrapped();

        lock (EntriesLock)
            Entries.Add(new ProviderEntry(provider, settingsType, factory));
    }

    /// <summary>
    /// Registers a UI-only provider (no storage creation, no JSON serialization support).
    /// </summary>
    public static void Register(IStorageSettingsProvider provider)
    {
        EnsureBootstrapped();

        lock (EntriesLock)
            Entries.Add(new ProviderEntry(provider, typeof(object), null));
    }

    public static IReadOnlyList<IStorageSettingsProvider> GetSupported()
    {
        EnsureBootstrapped();

        lock (EntriesLock)
            return [.. Entries.Select(e => e.Provider).Where(p => p.IsSupported).OrderBy(p => p.Order)];
    }

    public static IStorageSettingsProvider? FindById(string storageId)
    {
        EnsureBootstrapped();

        lock (EntriesLock)
            return Entries.FirstOrDefault(e => e.Provider.StorageId == storageId)?.Provider;
    }

    public static IStorageSettingsProvider? FindForSettings(IStorageSettingsV2 settings)
    {
        EnsureBootstrapped();

        lock (EntriesLock)
            return Entries.FirstOrDefault(e => e.Provider.CanHandle(settings))?.Provider;
    }

    /// <summary>
    /// Returns the concrete settings type registered for the given JSON type discriminator, or null if not found.
    /// </summary>
    public static Type? GetSettingsType(string discriminator)
    {
        EnsureBootstrapped();

        lock (EntriesLock)
            return Entries.FirstOrDefault(e => e.Provider.StorageId == discriminator)?.SettingsType;
    }

    /// <summary>
    /// Returns the JSON type discriminator for the given concrete settings type, or null if not registered.
    /// </summary>
    public static string? GetDiscriminator(Type settingsType)
    {
        EnsureBootstrapped();

        lock (EntriesLock)
            return Entries.FirstOrDefault(e => e.SettingsType == settingsType)?.Provider.StorageId;
    }

    /// <summary>
    /// Creates an IStorage instance for the given settings using the registered factory.
    /// </summary>
    public static IStorage CreateStorage(ILog log, IStorageSettingsV2 settings, bool autodetectConnectionSettings)
    {
        EnsureBootstrapped();

        lock (EntriesLock)
        {
            var entry = Entries.FirstOrDefault(e => e.Provider.CanHandle(settings));
            if (entry == null)
                throw new ArgumentOutOfRangeException(nameof(settings), $"No provider registered for settings type '{settings.GetType().Name}'.");
            if (entry.Factory == null)
                throw new InvalidOperationException($"Storage provider '{entry.Provider.StorageId}' was registered without a factory and cannot create IStorage instances.");
            return entry.Factory(log, settings, autodetectConnectionSettings);
        }
    }
}
