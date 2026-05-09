using BUtil.Core.Events;
using BUtil.Interop.Logs;

namespace BUtil.Core.TasksTree.Core;

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
