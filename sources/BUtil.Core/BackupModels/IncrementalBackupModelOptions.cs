using BUtil.Core.Options;
using BUtil.Core.Storages;
using System.Collections.Generic;

namespace BUtil.Core.BackupModels
{
    public class IncrementalBackupModelOptions: IBackupModelOptions
    {
        public IncrementalBackupModelOptions()
        {
            Items = new();
            FileExcludePatterns = new List<string>();
        }
        public IStorageSettings To { get; set; }
        public List<SourceItem> Items { get; set; }
        public List<string> FileExcludePatterns { get; set; }
    }
}
