using BUtil.Core.BackupModels;

namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class BackupTaskV2
    {
        public BackupTaskV2()
        {
            Model = new IncrementalBackupModelOptionsV2();
        }

        public IBackupModelOptionsV2 Model { get; set; }

        public string Name { get; set; }
    }
}