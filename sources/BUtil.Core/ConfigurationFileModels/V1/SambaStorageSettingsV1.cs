namespace BUtil.Core.ConfigurationFileModels.V1
{
    public class SambaStorageSettingsV1 : IStorageSettingsV1
    {
        public long SingleBackupQuotaGb { get; set; } = 0;
        public string Url { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordStorageMethod { get; set; } = string.Empty;
    }
}