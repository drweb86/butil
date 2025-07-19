using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using System;

namespace BUtil.Core.TasksTree.BUtilServer.Client;

internal class BUtilClientRootTask : SequentialBuTask
{
    private readonly BUtilClientIoc _ioc;
    private readonly GetStateOfSourceItemTask _getStateOfSourceItemTask;
    private readonly BUtilClientConnectTask _butilClientConnectTask;
    private readonly BUtilClientModelOptionsV2 _options;

    public BUtilClientRootTask(ILog log, TaskEvents taskEvents, TaskV2 backupTask, Action<string?> onGetLastMinuteMessage)
        : base(log, taskEvents, string.Empty, null)
    {
        _options = (BUtilClientModelOptionsV2)backupTask.Model;
        _ioc = new BUtilClientIoc(log, _options.Folder, _options.Password, onGetLastMinuteMessage);

        var sourceItem = new SourceItemV2 { IsFolder = true, Target = _options.Folder };
        _getStateOfSourceItemTask = new GetStateOfSourceItemTask(taskEvents, sourceItem, Array.Empty<string>(), _ioc.Common);
        _butilClientConnectTask = new BUtilClientConnectTask(_ioc, taskEvents, _options);

        Children = new BuTask[] 
        {
            _getStateOfSourceItemTask,
            _butilClientConnectTask,
            new BUtilClientUploadToServerFolderTask(_ioc, taskEvents, _options, _getStateOfSourceItemTask, _butilClientConnectTask)
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
