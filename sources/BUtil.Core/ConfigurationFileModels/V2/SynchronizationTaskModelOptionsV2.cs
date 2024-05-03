namespace BUtil.Core.ConfigurationFileModels.V2;

public class SynchronizationTaskModelOptionsV2 : ITaskModelOptionsV2
{
    public IStorageSettingsV2 To { get; set; } = new FolderStorageSettingsV2();
    public string LocalFolder { get; set; } = PlatformSpecificExperience.Instance.GetFolderService().GetDefaultSynchronizationFolder();
    public string Password { get; set; } = string.Empty;
}
