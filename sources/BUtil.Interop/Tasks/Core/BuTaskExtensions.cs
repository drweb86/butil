namespace BUtil.Interop.Tasks.Core;

public static class BuTaskExtensions
{
    public static TTask EnsureSuccess<TTask>(this TTask task)
        where TTask : BuTask
    {
        ArgumentNullException.ThrowIfNull(task);

        if (!task.IsSuccess)
            throw new InvalidOperationException($"{task.Title} failed!");

        return task;
    }
}
