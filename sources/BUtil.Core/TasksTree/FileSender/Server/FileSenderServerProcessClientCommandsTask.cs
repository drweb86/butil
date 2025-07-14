using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.TasksTree.FileSender.Server;
using BUtil.Core.State;
using System;

namespace BUtil.Core.TasksTree.FileSender;

internal class FileSenderServerProcessClientCommandsTask : BuTaskV2
{
    private readonly FileSenderServerIoc _ioc;
    private readonly FileSenderServerModelOptionsV2 _options;
    private readonly Func<SourceItemState> _getSourceItemState;

    public FileSenderServerProcessClientCommandsTask(FileSenderServerIoc ioc, TaskEvents events, FileSenderServerModelOptionsV2 options,
        Func<SourceItemState> getSourceItemState) :
        base(ioc.Common.Log, events, $"Processing client commands")
    {
        _ioc = ioc;
        _options = options;
        _getSourceItemState = getSourceItemState;
    }

    protected override void ExecuteInternal()
    {
        while (true)
        {
            LogDebug("Waiting for command");
            var command = _ioc.FileSenderServerProtocol.ReadCommandForServer(_ioc.Reader);
            LogDebug($"Command is {command}");
            if (command == FileTransferProtocolServerCommand.ReceiveFile)
            {
                var fileState = _ioc.FileSenderServerProtocol.ReadFileHeader(_ioc.Reader);
                LogDebug(fileState.ToString());
                var childTask = new FileSenderServerGetFileTask(_ioc, Events, fileState, _getSourceItemState(), _options);
                Events.DuringExecutionTasksAdded(Id, new [] { childTask });
                childTask.Execute();
            }
            else if (command == FileTransferProtocolServerCommand.Disconnect)
            {
                break;
            }
        }
    }
}
