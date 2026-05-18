using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.Storages;

public static class StorageProviderRegistry
{
    internal sealed class ProviderEntry
    {
        internal ProviderEntry(
            string jsonType,
            string displayName,
            IStorageSettingsProvider provider,
            Type settingsType,
            Func<ILog, IStorageSettingsV2, bool, IStorage> factory)
        {
            JsonType = jsonType;
            DisplayName = displayName;
            Provider = provider;
            SettingsType = settingsType;
            Factory = factory;
        }

        public string JsonType { get; }
        public string DisplayName { get; }
        public IStorageSettingsProvider Provider { get; }
        public Type SettingsType { get; }
        internal Func<ILog, IStorageSettingsV2, bool, IStorage> Factory { get; }
    }

    private static readonly List<ProviderEntry> Entries = [];

    private static readonly object EntriesLock = new();

    /// <summary>
    /// Registers a storage provider with its settings type and runtime factory.
    /// </summary>
    public static void Register(
        string jsonType,
        string displayName,
        IStorageSettingsProvider provider,
        Type settingsType,
        Func<ILog, IStorageSettingsV2, bool, IStorage> factory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(jsonType);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(settingsType);
        ArgumentNullException.ThrowIfNull(factory);

        lock (EntriesLock)
            Entries.Add(new ProviderEntry(jsonType, displayName, provider, settingsType, factory));
    }

    internal static IReadOnlyList<ProviderEntry> GetProviders()
    {
        lock (EntriesLock)
            return [.. Entries.OrderBy(e => e.DisplayName)];
    }

    internal static ProviderEntry? FindForSettings(IStorageSettingsV2 settings)
    {
        lock (EntriesLock)
            return Entries.FirstOrDefault(e => e.SettingsType.IsInstanceOfType(settings));
    }

    internal static Type? FindSettingsType(string jsonType)
    {
        lock (EntriesLock)
            return Entries.FirstOrDefault(e => e.JsonType == jsonType)?.SettingsType;
    }

    internal static string? FindJsonType(Type settingsType)
    {
        lock (EntriesLock)
            return Entries.FirstOrDefault(e => e.SettingsType == settingsType)?.JsonType;
    }

    /// <summary>
    /// Creates an IStorage instance for the given settings using the registered factory.
    /// </summary>
    internal static IStorage CreateStorage(ILog log, IStorageSettingsV2 settings, bool autodetectConnectionSettings)
    {
        lock (EntriesLock)
        {
            var entry = Entries.FirstOrDefault(e => e.SettingsType.IsInstanceOfType(settings));
            if (entry == null)
                throw new ArgumentOutOfRangeException(nameof(settings), $"No provider registered for settings type '{settings.GetType().Name}'.");
            return entry.Factory(log, settings, autodetectConnectionSettings);
        }
    }
}
