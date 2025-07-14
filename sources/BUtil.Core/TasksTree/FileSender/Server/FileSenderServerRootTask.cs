using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace BUtil.Core.TasksTree.FileSender.Server;

internal class FileSenderServerRootTask : SequentialBuTask
{
    private readonly FileSenderServerIoc _ioc;
    private readonly GetStateOfSourceItemTask _getStateOfSourceItemTask;
    private readonly FileSenderServerModelOptionsV2 _options;

    public FileSenderServerRootTask(ILog log, TaskEvents taskEvents, TaskV2 backupTask, Action<string?> onGetLastMinuteMessage)
        : base(log, taskEvents, $"File Server {((FileSenderServerModelOptionsV2)backupTask.Model).Port}", null)
    {
        _options = (FileSenderServerModelOptionsV2)backupTask.Model;
        _options.Folder = new DirectoryInfo(_options.Folder).FullName;

        LogDebug($"Server working directory: {_options.Folder} (will be created if not exists)");
        Directory.CreateDirectory(_options.Folder);

        _ioc = new FileSenderServerIoc(log, _options.Folder, _options.Password, onGetLastMinuteMessage);

        _getStateOfSourceItemTask = new GetStateOfSourceItemTask(Events, new SourceItemV2 { IsFolder = true, Target = _options.Folder }, Array.Empty<string>(), _ioc.Common);
        Children = new List<BuTask>
        {
            _getStateOfSourceItemTask,
            new FileSenderServerStartTask(_ioc, Events, _options),
            new FileSenderServerClientAcceptorTask(_ioc, Events),
            new FileSenderServerProcessClientCommandsTask(_ioc, Events, _options, () => _getStateOfSourceItemTask.SourceItemState!),
            new FileSenderServerStopTask(_ioc, Events)
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
