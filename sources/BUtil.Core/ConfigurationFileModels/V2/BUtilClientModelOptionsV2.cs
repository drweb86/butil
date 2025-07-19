namespace BUtil.Core.ConfigurationFileModels.V2;
public class BUtilClientModelOptionsV2 : ITaskModelOptionsV2
{
    public BUtilClientModelOptionsV2() // deserialization
    {
        
    }

    public BUtilClientModelOptionsV2(string folder, FileSenderDirection direction, string serverHost, int serverPort, string password)
    {
        Folder = folder;
        Direction = direction;
        ServerHost = serverHost;
        ServerPort = serverPort;
        Password = password;
    }

    public string Folder { get; set; } = null!;
    public FileSenderDirection Direction { get; set; }
    public string ServerHost {  get; set; } = null!;
    public int ServerPort { get; set; }
    public string Password { get; set; } = string.Empty;
}
