using BUtil.Interop.Logs;
using BUtil.Interop.Tasks.Events;

namespace BUtil.Interop.Tasks.Core;

public abstract class ParallelBuTask : BuTask
{
    public IEnumerable<BuTask> Children { get; set; } = [];

    protected ParallelBuTask(ILog log, TaskEvents events, string title, IEnumerable<BuTask>? children = null)
        : base(log, events, title)
    {
        if (children != null)
            Children = children;
    }

    public override void Execute()
    {
        var children = Children.ToList();
        if (children.Count == 0)
        {
            IsSuccess = true;
            return;
        }

        var executer = new ParallelExecuter(children, Environment.ProcessorCount);
        executer.Wait();
        IsSuccess = Children.All(x => x.IsSuccess);
        IsSkipped = Children.All(x => x.IsSkipped);
    }

    public override IEnumerable<BuTask> GetChildren()
    {
        var result = new List<BuTask>();
        foreach (var child in Children)
        {
            result.Add(child);
            result.AddRange(child.GetChildren());
        }
        return result;
    }
}
