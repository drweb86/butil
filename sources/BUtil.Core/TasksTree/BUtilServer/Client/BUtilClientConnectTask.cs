using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.TasksTree.Core;
using System.IO;
using System.Text;

namespace BUtil.Core.TasksTree.BUtilServer.Client;

internal class BUtilClientConnectTask : BuTaskV2
{
    private readonly BUtilClientIoc _ioc;
    private readonly BUtilClientModelOptionsV2 _options;

    public BUtilClientConnectTask(BUtilClientIoc ioc, TaskEvents taskEvents, BUtilClientModelOptionsV2 options)
        : base(ioc.Common.Log, taskEvents, string.Format(Resources.BUtilClientConnectTask_Title, options.ServerIp, options.ServerPort))
    {
        _ioc = ioc;
        _options = options;
    }

    protected override void ExecuteInternal()
    {
        _ioc.Client = new System.Net.Sockets.TcpClient();
        _ioc.Client.Connect(_options.ServerIp, _options.ServerPort);
        _ioc.Stream = _ioc.Client.GetStream();
        _ioc.Writer = new BinaryWriter(_ioc.Stream, Encoding.UTF8, true);
        _ioc.FileSenderClientProtocol.WriteProtocolVersion(_ioc.Writer);
    }
}
