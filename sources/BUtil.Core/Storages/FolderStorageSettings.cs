namespace BUtil.Core.Storages
{
    public class FolderStorageSettings: IStorageSettings
    {
		public string Name { get; set; }
		public string DestinationFolder { get; set; }
        public long SingleBackupQuotaGb { get; set; }
        public bool Enabled { get; set; }
    }
}
