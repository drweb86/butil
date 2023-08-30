namespace BUtil.Core.Storages
{
    public class FtpsStorageSettings : IStorageSettings
    {
        public long SingleBackupQuotaGb { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }
        public string Folder { get; set; }

        public string User { get; set; }
        public string Password { get; set; }
    }
}
