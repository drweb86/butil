namespace BUtil.Core.Storages
{
    public class SambaStorageSettings : IStorageSettings
    {
        public long SingleBackupQuotaGb { get; set; }
        public string Url { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string PasswordStorageMethod { get; set; }
    }
}
