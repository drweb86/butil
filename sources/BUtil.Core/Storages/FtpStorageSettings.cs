namespace BUtil.Core.Storages
{
    public class FtpStorageSettings: IStorageSettings
    {
        public string Name { get; set; }
		public string DestinationFolder { get; set; } 
        public	string Host { get; set; } 
		public string User { get; set; } 
		public string Password { get; set; }
		public bool ActiveFtpMode { get; set; }
    }
}
