using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Storages.AzureBlob;

public class AzureBlobStorageSettingsV2 : IStorageSettingsV2
{
    public long SingleBackupQuotaGb { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountKey { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
    public string PathPrefix { get; set; } = string.Empty;
    public string? MountPowershellScript { get; set; }
    public string? UnmountPowershellScript { get; set; }
}
