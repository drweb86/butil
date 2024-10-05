namespace BUtil.Core.ConfigurationFileModels.V2;

public class SynchronizationTaskModelOptionsV2 : ITaskModelOptionsV2
{
    public IStorageSettingsV2 To { get; set; } = new FolderStorageSettingsV2();
    public string LocalFolder { get; set; } = PlatformSpecificExperience.Instance.GetFolderService().GetDefaultSynchronizationFolder();
    /// <summary>
    /// Obsolete. Mapping to subfolder of repository. If partial data checkout to local folder is needed, this is to be specified.
    /// </summary>
    public string? RepositorySubfolder { get; set; }
    public SynchronizationTaskModelMode SynchronizationMode { get; set; }
    public string Password { get; set; } = string.Empty;
}
