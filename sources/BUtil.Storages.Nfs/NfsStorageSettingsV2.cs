using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Storages.Nfs;

public class NfsStorageSettingsV2 : IStorageSettingsV2
{
    public long SingleBackupQuotaGb { get; set; }
    public string Host { get; set; } = string.Empty;
    public string SharePath { get; set; } = string.Empty;
    public string MountPoint { get; set; } = string.Empty;
    public string? MountOptions { get; set; }
    public string? MountPowershellScript { get; set; }
    public string? UnmountPowershellScript { get; set; }
}
