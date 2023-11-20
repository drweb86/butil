
using System;
using System.Collections.Generic;

namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class IncrementalBackupModelOptionsV2 : ITaskModelOptionsV2
    {
        public IStorageSettingsV2 To { get; set; } = new FolderStorageSettingsV2();
        public List<SourceItemV2> Items { get; set; } = new List<SourceItemV2> {
                    new SourceItemV2(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) , true),
                    new SourceItemV2(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) , true),
                    new SourceItemV2(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) , true),
                    new SourceItemV2(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) , true),
                    new SourceItemV2(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) , true)};
        public List<string> FileExcludePatterns { get; set; } = new();
        public string Password { get; set; } = string.Empty;
    }
}
