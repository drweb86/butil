using BUtil.Core.Localization;
using BUtil.Core.Services;
using BUtil.Interop.Tasks.Core;
using BUtil.Interop.Tasks.Events;
using System;
using System.Threading.Tasks;

namespace BUtil.Core.TasksTree.BUtilServer.Server;

internal class TimeoutTask(CommonServicesIoc ioc, TaskEvents events, long timeoutMinutes) : BuTaskV2(ioc.Log, events, Resources.TimeoutTask_Title)
{
    protected override void ExecuteInternal()
    {
        Task.Delay(timeoutMinutes > 0 ? TimeSpan.FromMinutes(timeoutMinutes) : TimeSpan.MaxValue).Wait();
    }
}
