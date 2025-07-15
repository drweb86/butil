using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.FileSender.Client;

internal class FileSenderClientDisconnectTask : BuTaskV2
{
    private readonly FileSenderClientIoc _ioc;

    public FileSenderClientDisconnectTask(FileSenderClientIoc ioc, TaskEvents taskEvents)
        : base(ioc.Common.Log, taskEvents, "Disconnect")
    {
        _ioc = ioc;
    }

    protected override void ExecuteInternal()
    {
        _ioc.FileSenderClientProtocol.WriteCommandForServer(_ioc.Writer, FileTransferProtocolServerCommand.Disconnect);
    }
}
