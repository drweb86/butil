using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.TasksTree.Core;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace BUtil.Core.TasksTree.BUtilServer.Client;

internal class BUtilClientConnectTask : BuTaskV2
{
    private readonly BUtilClientIoc _ioc;
    private readonly BUtilClientModelOptionsV2 _options;

    public BUtilClientConnectTask(BUtilClientIoc ioc, TaskEvents taskEvents, BUtilClientModelOptionsV2 options)
        : base(ioc.Common.Log, taskEvents, string.Format(Resources.BUtilClientConnectTask_Title, options.ServerHost, options.ServerPort))
    {
        _ioc = ioc;
        _options = options;
    }

    protected override void ExecuteInternal()
    {
        _ioc.Client = new System.Net.Sockets.TcpClient();

        LogDebug($"Getting IPs of a host {_options.ServerHost}");
        var ips = NetworkHelper.GetIPsByName(_options.ServerHost, true, true);
        foreach (var ip in ips)
            LogDebug(ip.ToString());
        if (!ips.Any())
            throw new InvalidOperationException("There are no IPs found!");

        _ioc.Client.Connect(ips[0], _options.ServerPort);
        _ioc.Stream = _ioc.Client.GetStream();
        _ioc.Writer = new BinaryWriter(_ioc.Stream, Encoding.UTF8, true);
        _ioc.Common.BUtilServerClientProtocol.WriteProtocolVersion(_ioc.Writer);
    }
}
