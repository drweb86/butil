using BUtil.Core.Localization;
using BUtil.Core.Services;
using BUtil.Interop.Tasks.Core;
using BUtil.Interop.Tasks.Events;
using BUtil.Tasks.BUtilServer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.Core.TasksTree.BUtilServer.Server;

internal class TimeoutTask(CommonServicesIoc ioc, TaskEvents events, long timeoutMinutes) : BuTaskV2(ioc.Log, events, Resources.TimeoutTask_Title)
{
    protected override void ExecuteInternal()
    {
        Task.Delay(ToDelay(timeoutMinutes)).Wait();
    }

    private static TimeSpan ToDelay(long minutes)
    {
        if (minutes <= 0)
            return Timeout.InfiniteTimeSpan;

        if (minutes > BUtilServerModelOptionsV2.MaxDurationMinutes)
            throw new ArgumentOutOfRangeException(nameof(minutes));

        return TimeSpan.FromMinutes(minutes);
    }
}
