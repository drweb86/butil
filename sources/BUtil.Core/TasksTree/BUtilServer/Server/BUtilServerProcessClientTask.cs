using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.TasksTree.BUtilServer.Server;
using System.Net.Sockets;
using System.IO;
using System.Text;
using BUtil.Core.Localization;
using System.Reflection.PortableExecutable;

namespace BUtil.Core.TasksTree.FileSender;

internal class BUtilServerProcessClientTask : BuTaskV2
{
    private readonly BUtilServerIoc _ioc;
    private readonly TcpClient _client;
    private readonly BUtilServerModelOptionsV2 _options;

    public BUtilServerProcessClientTask(BUtilServerIoc ioc, TaskEvents events, TcpClient client, BUtilServerModelOptionsV2 options) :
        base(ioc.Common.Log, events, string.Format(Resources.BUtilServerProcessClientTask_Title, client.Client.RemoteEndPoint?.ToString() ?? string.Empty))
    {
        _ioc = ioc;
        _client = client;
        _options = options;
    }

    protected override void ExecuteInternal()
    {
        LogDebug("Client connected.");
        using (var stream = _client.GetStream())
        using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
        { 
            _ioc.FileSenderServerProtocol.ReadCheckProtocolVersion(reader);

            while (true)
            {
                LogDebug("Waiting for command");
                var command = _ioc.FileSenderServerProtocol.ReadCommandForServer(reader);
                LogDebug($"Command is {command}");
                if (command == FileTransferProtocolServerCommand.ReceiveFile)
                {
                    var remoteFileState = _ioc.FileSenderServerProtocol.ReadFileHeader(reader);
                    var childTask = new BUtilServerSaveFileTask(_ioc, Events, stream, reader, _options, remoteFileState, _client.Client.RemoteEndPoint?.ToString() ?? string.Empty);
                    Events.DuringExecutionTasksAdded(Id, new[] { childTask });
                    childTask.Execute();
                }
                else if (command == FileTransferProtocolServerCommand.Disconnect)
                {
                    break;
                }
            }
        }
        _client.Dispose();
    }
}
