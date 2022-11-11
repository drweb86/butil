namespace BUtil.Core.Storages
{
    public class SambaStorageSettings: IStorageSettings
    {
        public string Name { get; set; }
        public string DestinationFolder { get; set; }
        public bool EncryptUnderLocalSystemAccount { get; set; }
        public bool SkipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds { get; set; }
        public long SkipCopyingToNetworkStorageLimitMb { get; set; }
    }
}
