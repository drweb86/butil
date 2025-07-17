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

    public BUtilServerWaitForClientTask(BUtilServerIoc ioc, TaskEvents events, BUtilServerModelOptionsV2 options) :
        base(ioc.Common.Log, events, Resources.BUtilServerWaitForClientTask_Title)
    {
        _ioc = ioc;
        _options = options;
    }

    protected override void ExecuteInternal()
    {
        while (true)
        {
            var client = _ioc.TcpListener.AcceptTcpClient();

            // for now only sequentiual
            var task = new BUtilServerProcessClientTask(_ioc, Events, client, _options);
            Events.DuringExecutionTasksAdded(Id, new BuTask[] { task });
            task.Execute();
        }
    }
}
