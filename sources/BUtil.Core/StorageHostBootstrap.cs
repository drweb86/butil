using BUtil.Core.ConfigurationFileModels.V2;
using System.Threading;

namespace BUtil.Core.Storages;

internal static class StorageHostBootstrap
{
    private static int _wired;
    private static int _folderRegistered;

    /// <summary>
    /// Registers the folder built-in delegate with <see cref="StorageProviderRegistry"/>. Call before any registry use
    /// (host entry points wire this from <see cref="PlatformSpecificExperience"/>; tests wire from their module initializer).
    /// </summary>
    internal static void EnsureWired()
    {
        if (Interlocked.Exchange(ref _wired, 1) != 0)
            return;

        StorageProviderRegistry.HostEnsureBuiltIns = RegisterFolderBuiltInOnce;
    }

    private static void RegisterFolderBuiltInOnce()
    {
        if (Interlocked.Exchange(ref _folderRegistered, 1) != 0)
            return;

        StorageProviderRegistry.RegisterBuiltIn(
            new FolderStorageSettingsProvider(),
            typeof(FolderStorageSettingsV2),
            (log, s, _) => new FolderStorage(log, (FolderStorageSettingsV2)s));
    }
}
