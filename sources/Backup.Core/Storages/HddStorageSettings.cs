namespace BUtil.Core.Storages
{
    public class HddStorageSettings
	{
		public string Name { get; set; }
		public string DestinationFolder { get; set; }
		public bool DeleteBUtilFilesInDestinationFolderBeforeBackup { get; set; }
    }
}
