using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.FileSender;

namespace BUtil.Core.TasksTree.BUtilServer.Server;

internal class BUtilServerWaitForClientTask : BuTaskV2
{
    private readonly BUtilServerIoc _ioc;
    private readonly BUtilServerModelOptionsV2 _options;
    private readonly BUtilServerStartTask _serverStartTask;

    public BUtilServerWaitForClientTask(BUtilServerIoc ioc, TaskEvents events, BUtilServerModelOptionsV2 options, BUtilServerStartTask serverStartTask) :
        base(ioc.Common.Log, events, Resources.BUtilServerWaitForClientTask_Title)
    {
        _ioc = ioc;
        _options = options;
        _serverStartTask = serverStartTask;
    }

    protected override void ExecuteInternal()
    {
        if (!_serverStartTask.IsSuccess)
        { 
            IsSkipped = true;
            LogDebug("Connection failed, skipping");
            return;
        }

        while (true)
        {
            var client = _ioc.TcpListener.AcceptTcpClient();

            // for now only sequentiual
            var task = new BUtilServerProcessClientTask(_ioc, Events, client, _options);
            Events.DuringExecutionTasksAdded(null, new BuTask[] { task }); // null instead of Id because major task events rework is needed to handle properly.
            task.Execute();
        }
    }
}
