using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace BUtil.Core.TasksTree.BUtilServer.Server;

internal class FtpsServerRootTask : SequentialBuTask
{
    private readonly FtpsServerIoc _ioc;
    private readonly BUtilServerModelOptionsV2 _options;

    public FtpsServerRootTask(ILog log, TaskEvents taskEvents, TaskV2 backupTask, Action<string?> onGetLastMinuteMessage)
        : base(log, taskEvents, string.Empty, null)
    {
        _options = (BUtilServerModelOptionsV2)backupTask.Model;
        _options.Folder = new DirectoryInfo(_options.Folder).FullName;

        LogDebug($"Server working directory: {_options.Folder} (will be created if not exists)");
        Directory.CreateDirectory(_options.Folder);

        _ioc = new FtpsServerIoc(log, onGetLastMinuteMessage);

        Children = new List<BuTask>
        {
            new FtpsServerStartTask(_ioc, Events, _options),
            new TimeoutTask(_ioc.Common, Events, _options.DurationMinutes),
        };
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        base.Execute();
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        _ioc.Server?.Stop();
        _ioc.Dispose();
    }
}
