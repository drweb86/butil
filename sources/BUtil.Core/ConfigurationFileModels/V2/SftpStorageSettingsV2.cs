namespace BUtil.Core.ConfigurationFileModels.V2;

public class SftpStorageSettingsV2 : IStorageSettingsV2
{
    public long SingleBackupQuotaGb { get; set; }

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string? Folder { get; set; }
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string TrustedFingerprint { get; set; } = null!;

}
