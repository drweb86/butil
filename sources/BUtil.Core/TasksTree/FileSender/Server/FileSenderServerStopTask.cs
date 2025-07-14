using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.FileSender;

internal class FileSenderServerStopTask : BuTaskV2
{
    private readonly FileSenderServerIoc _ioc;

    public FileSenderServerStopTask(FileSenderServerIoc ioc, TaskEvents events) :
        base(ioc.Common.Log, events, "Stop server")
    {
        _ioc = ioc;
    }

    protected override void ExecuteInternal()
    {
        _ioc.TcpListener.Stop();
        LogDebug("Server is stopped.");
    }
}
