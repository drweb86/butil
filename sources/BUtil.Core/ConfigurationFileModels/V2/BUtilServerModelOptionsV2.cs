namespace BUtil.Core.ConfigurationFileModels.V2;

public class BUtilServerModelOptionsV2 : ITaskModelOptionsV2
{
    public const string DefaultIp = "0.0.0.0";
    public const long DefaultDuration = 30;
    public const int DefaultPort = 10999;
    public const string DefaultUsername = "user";

    public BUtilServerModelOptionsV2() // deserialization
    {

    }

    public BUtilServerModelOptionsV2(
        string serverAddress,
        int port,
        string username,
        string password,
        string folder,
        long durationMinutes)
    {
        ServerAddress = serverAddress;
        Port = port;
        Folder = folder;
        Username = username;
        Password = password;
        DurationMinutes = durationMinutes;
    }

    public string? ServerAddress { get; set; } = DefaultIp;
    public int Port { get; set; } = DefaultPort;
    public string Username { get; set; } = DefaultUsername;
    public string Password { get; set; } = null!;
    public string Folder { get; set; } = null!;
    public long DurationMinutes { get; set; } = DefaultDuration;
}
