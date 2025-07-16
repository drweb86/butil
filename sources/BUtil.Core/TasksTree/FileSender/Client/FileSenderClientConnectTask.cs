using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.TasksTree.Core;
using System.IO;
using System.Text;

namespace BUtil.Core.TasksTree.FileSender.Client;

internal class FileSenderClientConnectTask : BuTaskV2
{
    private readonly FileSenderClientIoc _ioc;
    private readonly FileSenderClientModelOptionsV2 _options;

    public FileSenderClientConnectTask(FileSenderClientIoc ioc, TaskEvents taskEvents, FileSenderClientModelOptionsV2 options)
        : base(ioc.Common.Log, taskEvents, $"Connect to server IP: {options.ServerIp}, port: {options.ServerPort}")
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
