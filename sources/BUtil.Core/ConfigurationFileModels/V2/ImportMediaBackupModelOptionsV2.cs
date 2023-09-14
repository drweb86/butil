namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class ImportMediaBackupModelOptionsV2 : IBackupModelOptionsV2
    {
        public string TransformFileName { get; set; }

        public IStorageSettingsV2 From { get; set; }
        public string DestinationFolder { get; set; }
        public bool SkipAlreadyImportedFiles { get; set; }
    }
}
