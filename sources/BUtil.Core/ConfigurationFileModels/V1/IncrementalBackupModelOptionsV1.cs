namespace BUtil.Core.ConfigurationFileModels.V1;

public class IncrementalBackupModelOptionsV1 : IBackupModelOptionsV1
{
    public bool DisableCompressionAndEncryption { get; set; }

    public IncrementalBackupModelOptionsV1()
    {
        DisableCompressionAndEncryption = false;
    }
}