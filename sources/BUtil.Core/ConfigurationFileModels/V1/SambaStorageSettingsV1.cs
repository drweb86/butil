namespace BUtil.Core.ConfigurationFileModels.V1
{
    public class SambaStorageSettingsV1 : IStorageSettingsV1
    {
        public long SingleBackupQuotaGb { get; set; }
        public string Url { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string PasswordStorageMethod { get; set; }
    }
}