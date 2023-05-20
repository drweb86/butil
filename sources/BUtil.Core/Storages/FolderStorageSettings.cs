using System;

namespace BUtil.Core.Storages
{
    public class FolderStorageSettings: IStorageSettings
    {
		public string DestinationFolder { get; set; }
        public long SingleBackupQuotaGb { get; set; }
        public string MountPowershellScript { get; set; }
        public string UnmountPowershellScript { get; set; }
    }
}
