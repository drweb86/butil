using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.TasksTree.Core;
using System.IO;
using System.Text;

namespace BUtil.Core.TasksTree.FileSender;

internal class FileSenderServerClientAcceptorTask : BuTaskV2
{
    private readonly FileSenderServerIoc _ioc;

    public FileSenderServerClientAcceptorTask(FileSenderServerIoc ioc, TaskEvents events) :
        base(ioc.Common.Log, events, $"Accepting single client")
    {
        _ioc = ioc;
    }

    protected override void ExecuteInternal()
    {
        _ioc.Client = _ioc.TcpListener.AcceptTcpClient(); // for now only 1 client. In future multithreaded something can be added. But not now.
        LogDebug("Client connected.");
        _ioc.Stream = _ioc.Client.GetStream();
        _ioc.Reader = new BinaryReader(_ioc.Stream, Encoding.UTF8, true);
        _ioc.FileSenderServerProtocol.ReadCheckProtocolVersion(_ioc.Reader);
    }
}
