using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.BUtilServer.Server;

internal class FtpsServerStartTask : BuTaskV2
{
    private readonly FtpsServerIoc _ioc;
    private readonly BUtilServerModelOptionsV2 _options;

    public FtpsServerStartTask(FtpsServerIoc ioc, TaskEvents events, BUtilServerModelOptionsV2 options) :
        base(ioc.Common.Log, events, Resources.FtpsServerStartTask_Name)
    {
        _ioc = ioc;
        _options = options;
    }

    protected override void ExecuteInternal()
    {
        LogDebug("Starting the machinery.");
        _ioc.Server = new FtpsServerLibrary.FtpsServer(
            new FtpsServerLog(_ioc.Common.Log),
            new FtpsServerLibrary.FtpsServerConfiguration
            {
                ServerSettings = new FtpsServerLibrary.FtpsServerSettings
                {
                    Ip = "0.0.0.0",
                    Port = _options.Port,
                    MaxConnections = 10,
                },
                Users = new List<FtpsServerLibrary.FtpsServerUserAccount>
                {
                    new FtpsServerLibrary.FtpsServerUserAccount
                    {
                        Login = string.IsNullOrWhiteSpace(_options.Username) ? BUtilServerModelOptionsV2.DefaultUsername: _options.Username, // backward compatibility
                        Password = _options.Password,
                        Folder = _options.Folder,
                        Read = true,
                        Write = true
                    }
                }
            });

        _ioc.Server.Start();
    }
}
