using BUtil.Core.Storages;

namespace BUtil.Storages.Sftp;

/// <summary>
/// Registers the SFTP storage provider and factory with the StorageProviderRegistry.
/// Call Register() once at application startup before loading any tasks.
/// </summary>
public static class SftpStoragePlugin
{
    public static void Register()
    {
        StorageProviderRegistry.Register(
            new SftpStorageSettingsProvider(),
            typeof(SftpStorageSettingsV2),
            (log, s, autodetect) => new SftpStorage(log, (SftpStorageSettingsV2)s, autodetect));
    }
}
