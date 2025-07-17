using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.TasksTree.Core;
using System;
using System.Linq;

namespace BUtil.Core.TasksTree.BUtilServer.Client;

internal class BUtilClientUploadToServerFolderTask : SequentialBuTaskV2
{
    private readonly BUtilClientIoc _ioc;
    private readonly BUtilClientModelOptionsV2 _options;
    private readonly GetStateOfSourceItemTask _getStateOfSourceItemTask;

    public BUtilClientUploadToServerFolderTask(BUtilClientIoc ioc, TaskEvents taskEvents, BUtilClientModelOptionsV2 options,
        GetStateOfSourceItemTask getStateOfSourceItemTask)
        : base(ioc.Common.Log, taskEvents, string.Format(Resources.File_Uploading, options.Folder))
    {
        _ioc = ioc;
        _options = options;
        _getStateOfSourceItemTask = getStateOfSourceItemTask;
    }

    protected override void ExecuteInternal()
    {
        if (_getStateOfSourceItemTask.IsSuccess)
            throw new InvalidOperationException("State of folder is invalid.");

        var sourceItemState = _getStateOfSourceItemTask.SourceItemState!;
        Children = sourceItemState.FileStates
            .Select(fileState => new BUtilClientUploadToServerFileTask(_ioc, Events, _options, fileState))
            .ToList();
        Events.DuringExecutionTasksAdded(Id, Children);

        base.ExecuteInternal();
    }
}
