using BUtil.Interop.Logs;
using BUtil.Interop.Tasks.Events;

namespace BUtil.Interop.Tasks.Core;

public class CompletedTask : BuTask
{
    public CompletedTask(ILog log, TaskEvents events, bool isSuccess) : base(log, events, string.Empty)
    {
        IsSuccess = isSuccess;
    }

    public override void Execute()
    {
    }
}
