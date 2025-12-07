namespace BUtil.Core.ConfigurationFileModels.V2;
public class BUtilClientModelOptionsV2 : ITaskModelOptionsV2
{
    public BUtilClientModelOptionsV2() // deserialization
    {
        
    }

    public BUtilClientModelOptionsV2(string folder, FileSenderDirection direction, IStorageSettingsV2 to)
    {
        Folder = folder;
        Direction = direction;
        To = to;
    }

    public IStorageSettingsV2 To { get; set; } = new FtpsStorageSettingsV2();

    public string Folder { get; set; } = null!;
    public FileSenderDirection Direction { get; set; }
}
