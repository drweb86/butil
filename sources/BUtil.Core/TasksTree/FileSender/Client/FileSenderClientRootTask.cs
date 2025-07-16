using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using System;

namespace BUtil.Core.TasksTree.FileSender.Client;

internal class FileSenderClientRootTask : SequentialBuTask
{
    private readonly FileSenderClientIoc _ioc;
    private readonly GetStateOfSourceItemTask _getStateOfSourceItemTask;
    private readonly FileSenderClientModelOptionsV2 _options;

    public FileSenderClientRootTask(ILog log, TaskEvents taskEvents, TaskV2 backupTask, Action<string?> onGetLastMinuteMessage)
        : base(log, taskEvents, "File Sender Transfer", null)
    {
        _options = (FileSenderClientModelOptionsV2)backupTask.Model;
        _ioc = new FileSenderClientIoc(log, _options.Folder, _options.Password, onGetLastMinuteMessage);

        var sourceItem = new SourceItemV2 { IsFolder = true, Target = _options.Folder };
        _getStateOfSourceItemTask = new GetStateOfSourceItemTask(taskEvents, sourceItem, Array.Empty<string>(), _ioc.Common);

        Children = new BuTask[] 
        {
            _getStateOfSourceItemTask,
            new FileSenderClientConnectTask(_ioc, taskEvents, _options),
            new FileSenderClientUploadToServerFolderTask(_ioc, taskEvents, _options, () => _getStateOfSourceItemTask.SourceItemState!),
            new FileSenderClientDisconnectTask(_ioc, taskEvents)
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
