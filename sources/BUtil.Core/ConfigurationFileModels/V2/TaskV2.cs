using BUtil.Core.BackupModels;

namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class TaskV2
    {
        public TaskV2()
        {
            Model = new IncrementalBackupModelOptionsV2();
        }

        public ITaskModelOptionsV2 Model { get; set; }

        public string Name { get; set; }
    }
}