using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Storages.WebDav;

public class WebDavStorageSettingsV2 : IStorageSettingsV2
{
    public long SingleBackupQuotaGb { get; set; }
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool UseHttps { get; set; } = true;
    public string BasePath { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? MountPowershellScript { get; set; }
    public string? UnmountPowershellScript { get; set; }
}
