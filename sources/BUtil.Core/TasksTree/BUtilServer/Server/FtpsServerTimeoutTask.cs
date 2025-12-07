using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.TasksTree.Core;
using System;
using System.Threading.Tasks;

namespace BUtil.Core.TasksTree.BUtilServer.Server;

internal class FtpsServerTimeoutTask : BuTaskV2
{
    private readonly long _timeoutMinutes;

    public FtpsServerTimeoutTask(FtpsServerIoc ioc, TaskEvents events, long timeoutMinutes) :
        base(ioc.Common.Log, events, Resources.BUtilServerWaitForClientTask_Title)
    {
        _timeoutMinutes = timeoutMinutes;
    }

    protected override void ExecuteInternal()
    {
        Task.Delay(_timeoutMinutes > 0 ? TimeSpan.FromMinutes(_timeoutMinutes) : TimeSpan.MaxValue).Wait();
    }
}
