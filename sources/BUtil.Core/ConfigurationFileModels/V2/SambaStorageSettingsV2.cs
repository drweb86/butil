namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class SambaStorageSettingsV2 : IStorageSettingsV2
    {
        public long SingleBackupQuotaGb { get; set; }
        public string Url { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string PasswordStorageMethod { get; set; }
    }
}
