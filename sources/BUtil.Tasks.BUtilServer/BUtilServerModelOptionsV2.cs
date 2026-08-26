using BUtil.Interop.Tasks;

namespace BUtil.Tasks.BUtilServer;

public class BUtilServerModelOptionsV2 : ITaskModelOptionsV2
{
    public const long DefaultDuration = 30;
    // Task.Delay accepts at most uint.MaxValue-1 milliseconds (~49.7 days).
    public const long MaxDurationMinutes = (uint.MaxValue - 1L) / 60_000;
    public const int DefaultPort = 10999;
    public const string DefaultUsername = "user";
    public const BUtilServerFolderAccess DefaultFolderAccess = BUtilServerFolderAccess.ReadWrite;

    public BUtilServerModelOptionsV2()
    {
    }

    public BUtilServerModelOptionsV2(
        int port,
        string username,
        string password,
        string folder,
        long durationMinutes,
        BUtilServerFolderAccess folderAccess = DefaultFolderAccess)
    {
        Port = port;
        Folder = folder;
        Username = username;
        Password = password;
        DurationMinutes = durationMinutes;
        FolderAccess = folderAccess;
    }

    public int Port { get; set; } = DefaultPort;
    public string Username { get; set; } = DefaultUsername;
    public string Password { get; set; } = null!;
    public string Folder { get; set; } = null!;
    public BUtilServerFolderAccess FolderAccess { get; set; } = DefaultFolderAccess;
    public long DurationMinutes { get; set; } = DefaultDuration;
}
