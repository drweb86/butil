using System.Collections.Generic;

namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class IncrementalBackupModelOptionsV2 : IBackupModelOptionsV2
    {
        public IncrementalBackupModelOptionsV2()
        {
            Items = new();
            FileExcludePatterns = new List<string>();
        }
        public IStorageSettingsV2 To { get; set; }
        public List<SourceItemV2> Items { get; set; }
        public List<string> FileExcludePatterns { get; set; }
        public string Password { get; set; }
    }
}
