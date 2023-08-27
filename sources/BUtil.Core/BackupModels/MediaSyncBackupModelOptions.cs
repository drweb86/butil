using BUtil.Core.Storages;

namespace BUtil.Core.BackupModels
{
    public class MediaSyncBackupModelOptions : IBackupModelOptions
    {
        public string TransformFileName { get; set; }

        public IStorageSettings From { get; set; }
        public IStorageSettings To { get; set; }
    }
}
