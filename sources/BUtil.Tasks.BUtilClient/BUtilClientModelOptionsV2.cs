using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Tasks;

namespace BUtil.Tasks.BUtilClient;

public class BUtilClientModelOptionsV2 : ITaskModelOptionsV2
{
    public BUtilClientModelOptionsV2()
    {
    }

    public BUtilClientModelOptionsV2(string folder, FileSenderDirection direction, IStorageSettingsV2 to)
    {
        Folder = folder;
        Direction = direction;
        To = to;
    }

    public IStorageSettingsV2 To { get; set; } = new FolderStorageSettingsV2();

    public string Folder { get; set; } = null!;
    public FileSenderDirection Direction { get; set; }
}
