namespace BUtil.Core.BackupModels
{
    public class IncrementalBackupModelOptions: IBackupModelOptions
    {
        public bool DisableCompressionAndEncryption { get; set; }

        public IncrementalBackupModelOptions()
        {
            DisableCompressionAndEncryption = true;
        }
    }
}
