using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Services;
using System;
using System.Threading.Tasks;

namespace BUtil.Core.TasksTree.Core;

internal class TimeoutTask : BuTaskV2
{
    private readonly long _timeoutMinutes;

    public TimeoutTask(CommonServicesIoc ioc, TaskEvents events, long timeoutMinutes) :
        base(ioc.Log, events, Resources.TimeoutTask_Title)
    {
        _timeoutMinutes = timeoutMinutes;
    }

    protected override void ExecuteInternal()
    {
        Task.Delay(_timeoutMinutes > 0 ? TimeSpan.FromMinutes(_timeoutMinutes) : TimeSpan.MaxValue).Wait();
    }
}
