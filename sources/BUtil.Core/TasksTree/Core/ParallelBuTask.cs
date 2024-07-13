
using BUtil.Core.Events;
using BUtil.Core.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.Core;

public abstract class ParallelBuTask : BuTask
{
    public IEnumerable<BuTask> Children { get; set; } = new List<BuTask>();

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
        var actualSelfChildren = Children ?? Array.Empty<BuTask>();
        var children = new List<BuTask>();
        foreach (var child in actualSelfChildren)
        {
            children.Add(child);
            children.AddRange(child.GetChildren());
        }
        return children;
    }

}
