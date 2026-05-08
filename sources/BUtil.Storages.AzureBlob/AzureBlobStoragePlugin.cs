using BUtil.Core.Storages;

namespace BUtil.Storages.AzureBlob;

public static class AzureBlobStoragePlugin
{
    public static void Register()
    {
        StorageProviderRegistry.Register(
            new AzureBlobStorageSettingsProvider(),
            typeof(AzureBlobStorageSettingsV2),
            (log, s, _) => new AzureBlobStorage(log, (AzureBlobStorageSettingsV2)s));
    }
}
