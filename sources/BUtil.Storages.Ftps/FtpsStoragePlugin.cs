using BUtil.Core.Storages;

namespace BUtil.Storages.Ftps;

/// <summary>
/// Registers the FTPS storage provider and factory with the StorageProviderRegistry.
/// Call Register() once at application startup before loading any tasks.
/// </summary>
public static class FtpsStoragePlugin
{
    public static void Register()
    {
        StorageProviderRegistry.Register(
            new FtpsStorageSettingsProvider(),
            typeof(FtpsStorageSettingsV2),
            (log, s, autodetect) => new FtpsStorage(log, (FtpsStorageSettingsV2)s, autodetect));
    }
}
