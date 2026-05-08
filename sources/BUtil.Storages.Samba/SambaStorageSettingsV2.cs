using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Storages.Samba;

public class SambaStorageSettingsV2 : IStorageSettingsV2
{
    public long SingleBackupQuotaGb { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? User { get; set; }
    public string? Password { get; set; }
    public string? MountPowershellScript { get; set; }
    public string? UnmountPowershellScript { get; set; }
}
