namespace BUtil.Core.BackupModels
{
    public class IncrementalBackupModelOptions
    {
        public bool DisableCompressionAndEncryption { get; set; }

        public IncrementalBackupModelOptions()
        {
            DisableCompressionAndEncryption = true;
        }
    }
}
