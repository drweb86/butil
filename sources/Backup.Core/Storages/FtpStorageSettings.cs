namespace BUtil.Core.Storages
{
    public class FtpStorageSettings
	{
        public string Name { get; set; }
		public string DestinationFolder { get; set; } 
		public bool DeleteBUtilFilesInDestinationFolderBeforeBackup { get; set; }
        public	string Host { get; set; } 
		public string User { get; set; } 
		public string Password { get; set; }
		public bool ActiveFtpMode { get; set; }
    }
}
