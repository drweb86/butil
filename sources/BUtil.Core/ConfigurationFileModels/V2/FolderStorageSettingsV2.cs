namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class FolderStorageSettingsV2 : IStorageSettingsV2
    {
        public string DestinationFolder { get; set; } = string.Empty;
        public long SingleBackupQuotaGb { get; set; }

        /// <summary>
        /// Its not powershell anymore on Ubuntu. SHould be renamed.
        /// </summary>
        public string MountPowershellScript { get; set; } = string.Empty;
        /// <summary>
        /// Its not powershell anymore on Ubuntu. SHould be renamed.
        /// </summary>
        public string UnmountPowershellScript { get; set; } = string.Empty;
    }
}
