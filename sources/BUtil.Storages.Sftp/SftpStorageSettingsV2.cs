using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Storages.Sftp;

public class SftpStorageSettingsV2 : IStorageSettingsV2
{
    public long SingleBackupQuotaGb { get; set; }

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 22;
    public string Folder { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string? KeyFile { get; set; }
    public string FingerPrintSHA256 { get; set; } = string.Empty;
    public string? MountPowershellScript { get; set; }
    public string? UnmountPowershellScript { get; set; }
}
