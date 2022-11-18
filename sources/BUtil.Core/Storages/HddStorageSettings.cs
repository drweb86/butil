namespace BUtil.Core.Storages
{
    public class HddStorageSettings: IStorageSettings
    {
		public string Name { get; set; }
		public string DestinationFolder { get; set; }
        public long UploadLimitGb { get; set; }
        public bool Enabled { get; set; }
    }
}
