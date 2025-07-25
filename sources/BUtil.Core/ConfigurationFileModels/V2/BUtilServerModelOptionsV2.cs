﻿namespace BUtil.Core.ConfigurationFileModels.V2;

public class BUtilServerModelOptionsV2 : ITaskModelOptionsV2
{
    public BUtilServerModelOptionsV2() // deserialization
    {

    }

    public BUtilServerModelOptionsV2(string folder, FileSenderServerPermissions permissions, string password, int port)
    {
        Folder = folder;
        Permissions = permissions;
        Password = password;
        Port = port;
    }

    public string Folder { get; set; } = null!;
    public FileSenderServerPermissions Permissions { get; set; }
    public string Password { get; set; } = string.Empty;
    public int Port { get; set; }
}
