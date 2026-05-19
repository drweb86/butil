using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Tasks;

namespace BUtil.Tasks.IncrementalBackup;

public class IncrementalBackupModelOptionsV2 : ITaskModelOptionsV2
{
    public IStorageSettingsV2 To { get; set; } = new FolderStorageSettingsV2();
    public List<SourceItemV2> Items { get; set; } = PlatformSpecificExperience.Instance.GetFolderService().GetDefaultBackupFolders()
        .Select(x => new SourceItemV2(x, true))
        .ToList();
    public List<string> FileExcludePatterns { get; set; } = [];
    public string Password { get; set; } = string.Empty;
}
