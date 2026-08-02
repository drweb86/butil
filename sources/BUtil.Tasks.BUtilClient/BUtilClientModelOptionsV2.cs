using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Tasks;

namespace BUtil.Tasks.BUtilClient;

public class BUtilClientModelOptionsV2 : ITaskModelOptionsV2
{
    public BUtilClientModelOptionsV2()
    {
    }

    public BUtilClientModelOptionsV2(string folder, FileSenderDirection direction, IStorageSettingsV2 to, bool skipExistingFiles)
    {
        Folder = folder;
        Direction = direction;
        To = to;
        SkipExistingFiles = skipExistingFiles;
    }

    public IStorageSettingsV2 To { get; set; } = new FolderStorageSettingsV2();

    public string Folder { get; set; } = null!;
    public FileSenderDirection Direction { get; set; }

    /// <summary>
    /// When true, files that already exist at the destination are skipped instead of overwritten.
    /// </summary>
    public bool SkipExistingFiles { get; set; }
}
