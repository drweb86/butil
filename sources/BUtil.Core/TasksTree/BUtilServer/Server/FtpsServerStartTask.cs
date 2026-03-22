using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.BUtilServer.Server;

internal class FtpsServerStartTask(FtpsServerIoc ioc, TaskEvents events, BUtilServerModelOptionsV2 options) : BuTaskV2(ioc.Common.Log, events, Resources.FtpsServerStartTask_Name)
{
    protected override void ExecuteInternal()
    {
        LogDebug("Starting the machinery.");
        ioc.Server = new FtpsServerLibrary.FtpsServer(
            new FtpsServerLog(ioc.Common.Log),
            new FtpsServerLibrary.FtpsServerConfiguration
            {
                ServerSettings = new FtpsServerLibrary.FtpsServerSettings
                {
                    Ip = "0.0.0.0",
                    Port = options.Port,
                    MaxConnections = 10,
                },
                Users =
                [
                    new FtpsServerLibrary.FtpsServerUserAccount
                    {
                        Login = string.IsNullOrWhiteSpace(options.Username) ? BUtilServerModelOptionsV2.DefaultUsername: options.Username, // backward compatibility
                        Password = options.Password,
                        Folder = options.Folder,
                        Read = true,
                        Write = true
                    }
                ]
            });

        ioc.Server.Start();
    }
}
