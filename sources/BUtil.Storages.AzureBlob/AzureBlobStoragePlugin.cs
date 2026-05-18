using BUtil.Core.Storages;

namespace BUtil.Storages.AzureBlob;

public static class AzureBlobStoragePlugin
{
    public static void Register()
    {
        StorageProviderRegistry.Register(
            "AzureBlob",
            "Azure Blob",
            new AzureBlobStorageSettingsProvider(),
            typeof(AzureBlobStorageSettingsV2),
            (log, s, _) => new AzureBlobStorage(log, (AzureBlobStorageSettingsV2)s));
    }
}
