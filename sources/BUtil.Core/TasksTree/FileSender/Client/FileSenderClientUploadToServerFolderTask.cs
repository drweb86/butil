using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using System;
using System.Linq;

namespace BUtil.Core.TasksTree.FileSender.Client;

internal class FileSenderClientUploadToServerFolderTask : SequentialBuTaskV2
{
    private readonly FileSenderClientIoc _ioc;
    private readonly FileSenderTransferModelOptionsV2 _options;
    private readonly Func<SourceItemState> _getSourceItemState;

    public FileSenderClientUploadToServerFolderTask(FileSenderClientIoc ioc, TaskEvents taskEvents, FileSenderTransferModelOptionsV2 options,
        Func<SourceItemState> getSourceItemState)
        : base(ioc.Common.Log, taskEvents, "Upload folder to server")
    {
        _ioc = ioc;
        _options = options;
        _getSourceItemState = getSourceItemState;
    }

    protected override void ExecuteInternal()
    {
        var sourceItemState = _getSourceItemState();
        Children = sourceItemState.FileStates
            .Select(fileState => new FileSenderClientUploadToServerFileTask(_ioc, Events, _options, fileState))
            .ToList();
        Events.DuringExecutionTasksAdded(Id, Children);

        base.ExecuteInternal();
    }
}
