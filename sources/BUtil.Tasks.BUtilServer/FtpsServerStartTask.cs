using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.TasksTree.Core;
using FtpsServerLibrary;

namespace BUtil.Core.TasksTree.BUtilServer.Server;

internal class FtpsServerStartTask(FtpsServerIoc ioc, TaskEvents events, BUtilServerModelOptionsV2 options) : BuTaskV2(ioc.Common.Log, events, Resources.FtpsServerStartTask_Name)
{
    protected override void ExecuteInternal()
    {
        LogDebug("Starting the machinery.");
        ioc.Server = new FtpsServer(
            new FtpsServerLog(ioc.Common.Log),
            new FtpsServerConfiguration
            {
                ServerSettings = new FtpsServerSettings
                {
                    Ip = "0.0.0.0",
                    Port = options.Port,
                    MaxConnections = 10,
                },
                Users =
                [
                    new FtpsServerUserAccount
                    {
                        Login = string.IsNullOrWhiteSpace(options.Username) ? BUtilServerModelOptionsV2.DefaultUsername: options.Username, // backward compatibility
                        Password = options.Password,
                        Folder = options.Folder,
                        Read = true,
                        Write = true
                    }
                ]
            },
            new FtpsServerFileSystemProvider());

        ioc.Server.StartAsync();
    }
}
