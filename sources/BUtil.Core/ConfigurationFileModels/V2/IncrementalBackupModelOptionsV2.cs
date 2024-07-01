using BUtil.Core.FileSystem;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.ConfigurationFileModels.V2;

public class IncrementalBackupModelOptionsV2 : ITaskModelOptionsV2
{
    public IStorageSettingsV2 To { get; set; } = new FolderStorageSettingsV2();
    public List<SourceItemV2> Items { get; set; } = Directories.GetDefaultBackupFolders()
        .Select(x => new SourceItemV2(x, true))
        .ToList();
    public List<string> FileExcludePatterns { get; set; } = new();
    public string Password { get; set; } = string.Empty;
}
