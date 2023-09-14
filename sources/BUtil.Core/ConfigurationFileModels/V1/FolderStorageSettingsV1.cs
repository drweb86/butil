namespace BUtil.Core.ConfigurationFileModels.V1
{
    public class FolderStorageSettingsV1 : IStorageSettingsV1
    {
        public string DestinationFolder { get; set; }
        public long SingleBackupQuotaGb { get; set; }
        public string MountPowershellScript { get; set; }
        public string UnmountPowershellScript { get; set; }
    }
}