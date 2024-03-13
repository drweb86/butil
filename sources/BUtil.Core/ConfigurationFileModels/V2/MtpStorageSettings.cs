namespace BUtil.Core.ConfigurationFileModels.V2;

public class MtpStorageSettings : IStorageSettingsV2
{
    public long SingleBackupQuotaGb { get; set; }
    public string Device { get; set; } = string.Empty;
    public string Folder { get; set; } = string.Empty;

}
