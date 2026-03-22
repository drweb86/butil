using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Services;
using System;
using System.Threading.Tasks;

namespace BUtil.Core.TasksTree.Core;

internal class TimeoutTask(CommonServicesIoc ioc, TaskEvents events, long timeoutMinutes) : BuTaskV2(ioc.Log, events, Resources.TimeoutTask_Title)
{
    protected override void ExecuteInternal()
    {
        Task.Delay(timeoutMinutes > 0 ? TimeSpan.FromMinutes(timeoutMinutes) : TimeSpan.MaxValue).Wait();
    }
}
