﻿namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class FolderStorageSettingsV2 : IStorageSettingsV2
    {
        public string DestinationFolder { get; set; } = string.Empty;
        public long SingleBackupQuotaGb { get; set; }
        public string MountPowershellScript { get; set; } = string.Empty;
        public string UnmountPowershellScript { get; set; } = string.Empty;
    }
}
