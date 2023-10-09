namespace BUtil.Core.ConfigurationFileModels.V1
{
    public class FolderStorageSettingsV1 : IStorageSettingsV1
    {
        public string DestinationFolder { get; set; } = string.Empty;
        public long SingleBackupQuotaGb { get; set; }
        public string MountPowershellScript { get; set; } = string.Empty;
        public string UnmountPowershellScript { get; set; } = string.Empty;
    }
}