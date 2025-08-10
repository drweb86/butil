using BUtil.Core.Storages;

namespace BUtil.Core.ConfigurationFileModels.V2;

public class SftpStorageSettingsV2 : IStorageSettingsV2
{
    public long SingleBackupQuotaGb { get; set; }

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = SftpStorage.DefaultPort;
    public string Folder { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string? KeyFile { get; set; }
    public string FingerPrintSHA256 { get; set; } = string.Empty;

}
