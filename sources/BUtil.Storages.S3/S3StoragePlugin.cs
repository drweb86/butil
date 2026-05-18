using BUtil.Core.Storages;

namespace BUtil.Storages.S3;

public static class S3StoragePlugin
{
    public static void Register()
    {
        StorageProviderRegistry.Register(
            "S3",
            "S3",
            new S3StorageSettingsProvider(),
            typeof(S3StorageSettingsV2),
            (log, s, _) => new S3Storage(log, (S3StorageSettingsV2)s));
    }
}
