using BUtil.Core.Storages;

namespace BUtil.Core.BackupModels
{
    public class ImportMediaBackupModelOptions : IBackupModelOptions
    {
        public string TransformFileName { get; set; }

        public IStorageSettings From { get; set; }
        public string DestinationFolder { get; set; }
    }
}
