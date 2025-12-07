using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using System;

namespace BUtil.Core.TasksTree.BUtilServer.Client;

internal class BUtilClientRootTask : SequentialBuTask
{
    private readonly BUtilClientIoc _ioc;
    private readonly GetStateOfSourceItemTask _getStateOfSourceItemTask;
    private readonly BUtilClientModelOptionsV2 _options;

    public BUtilClientRootTask(ILog log, TaskEvents taskEvents, TaskV2 backupTask, Action<string?> onGetLastMinuteMessage)
        : base(log, taskEvents, string.Empty, null)
    {
        _options = (BUtilClientModelOptionsV2)backupTask.Model;
        _ioc = new BUtilClientIoc(log, onGetLastMinuteMessage, _options.To);

        var sourceItem = new SourceItemV2 { IsFolder = true, Target = _options.Folder };
        _getStateOfSourceItemTask = new GetStateOfSourceItemTask(taskEvents, sourceItem, Array.Empty<string>(), _ioc.Common);

        Children = new BuTask[] 
        {
            _getStateOfSourceItemTask,
            new BUtilClientUploadToServerFolderTask(_ioc, taskEvents, _options, _getStateOfSourceItemTask)
        };
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        base.Execute();

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        _ioc.Dispose();
    }
}
