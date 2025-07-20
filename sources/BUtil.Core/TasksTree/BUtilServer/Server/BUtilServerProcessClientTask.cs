using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.TasksTree.BUtilServer.Server;
using System.Net.Sockets;
using System.IO;
using System.Text;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using System.Net;

namespace BUtil.Core.TasksTree.FileSender;

internal class BUtilServerProcessClientTask : BuTaskV2
{
    private readonly BUtilServerIoc _ioc;
    private readonly TcpClient _client;
    private readonly BUtilServerModelOptionsV2 _options;

    private static string GetAddressFriendlyName(TcpClient client)
    {
        var ipEndpoint = client.Client.RemoteEndPoint as IPEndPoint;
        if (ipEndpoint == null)
        {
            return "?";
        }
        return ipEndpoint.Address.ToString();
    }

    public BUtilServerProcessClientTask(BUtilServerIoc ioc, TaskEvents events, TcpClient client, BUtilServerModelOptionsV2 options) :
        base(ioc.Common.Log, events, string.Format(Resources.BUtilServerProcessClientTask_Title, GetAddressFriendlyName(client)))
    {
        _ioc = ioc;
        _client = client;
        _options = options;
    }

    protected override void ExecuteInternal()
    {
        LogDebug($"Client connected: remote endpoint: {_client.Client.RemoteEndPoint?.ToString()}, local endpoint: {_client.Client.LocalEndPoint?.ToString()}");
        using (var stream = _client.GetStream())
        using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
        { 
            _ioc.Common.BUtilServerProtocol.ReadCheckProtocolVersion(reader);

            while (true)
            {
                LogDebug("Waiting for command");
                FileTransferProtocolServerCommand command;
                try
                {
                    command = _ioc.Common.BUtilServerProtocol.ReadCommandForServer(reader);
                }
                catch (System.Exception e)
                {
                    // disconnect
                    LogError("Disconnect");
                    _ioc.Common.Log.WriteLine(Logs.LoggingEvent.Debug, ExceptionHelper.ToString(e));
                    return;
                }
                LogDebug($"Command is {command}");
                if (command == FileTransferProtocolServerCommand.ReceiveFile)
                {
                    try
                    {
                        var remoteFileState = _ioc.Common.BUtilServerProtocol.ReadFileHeader(reader, _options.Password);
                        var childTask = new BUtilServerSaveFileTask(_ioc, Events, stream, reader, _options, remoteFileState, _client.Client.RemoteEndPoint?.ToString() ?? string.Empty);
                        Events.DuringExecutionTasksAdded(Id, new[] { childTask });
                        childTask.Execute();
                    }
                    catch (System.Security.Cryptography.CryptographicException e)
                    {
                        LogError("Passwords do not match");
                        _ioc.Common.Log.WriteLine(Logs.LoggingEvent.Error, ExceptionHelper.ToString(e));

                        var fakeTaskForUi = new FunctionBuTaskV2<bool>(_ioc.Common.Log, Events, Title + ": Encryption failed (passwords on client and server do not match?)", () => true);
                        Events.DuringExecutionTasksAdded(Id, new BuTask[] { fakeTaskForUi });
                        fakeTaskForUi.Execute();

                        break;
                    }
                }
            }
        }
        _client.Dispose();
    }
}
