using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Storages.S3;

public class S3StorageSettingsV2 : IStorageSettingsV2
{
    public long SingleBackupQuotaGb { get; set; }

    /// <summary>
    /// One of: AWSS3, BackblazeB2, Wasabi, CloudflareR2, DigitalOceanSpaces, Custom
    /// </summary>
    public string Provider { get; set; } = "Custom";
    public string ServiceUrl { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string PathPrefix { get; set; } = string.Empty;
    public string? MountPowershellScript { get; set; }
    public string? UnmountPowershellScript { get; set; }
}
