using BUtil.Core.Events;
using BUtil.Core.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.Core;

public class SequentialBuTaskV2 : BuTaskV2
{
    private readonly bool _failOnFirstSkipped;
    private readonly bool _failOnFirstFailed;

    public IEnumerable<BuTask> Children { get; set; } = [];

    public SequentialBuTaskV2(ILog log, TaskEvents events, string title, IEnumerable<BuTask>? children = null, bool failOnFirstSkipped = true, bool failOnFirstFailed = true)
        : base(log, events, title)
    {
        if (children != null)
            Children = children;

        _failOnFirstSkipped = failOnFirstSkipped;
        _failOnFirstFailed = failOnFirstFailed;
    }

    protected override void ExecuteInternal()
    {
        foreach (var child in Children)
        {
            child.Execute();

            if (child.IsSkipped && _failOnFirstSkipped)
            {
                break;
            }
            if (!child.IsSuccess && _failOnFirstFailed)
            {
                break;
            }
        }
        if (Children.Any(x => x.IsSuccess))
        {
            throw new Exception("Some tasks were failed.");
        }
        IsSkipped = Children.All(x => x.IsSkipped);
    }

    public override IEnumerable<BuTask> GetChildren()
    {
        var actualSelfChildren = Children ?? [];
        var children = new List<BuTask>();
        foreach (var child in actualSelfChildren)
        {
            children.Add(child);
            children.AddRange(child.GetChildren());
        }
        return children;
    }
}

[Obsolete("SequentialBuTaskV2")]
public class SequentialBuTask : BuTask
{
    public IEnumerable<BuTask> Children { get; set; } = [];

    public SequentialBuTask(ILog log, TaskEvents events, string title, IEnumerable<BuTask>? children = null)
        : base(log, events, title)
    {
        if (children != null)
            Children = children;
    }

    public override void Execute()
    {
        foreach (var child in Children)
        {
            child.Execute();
        }
        IsSuccess = Children.All(x => x.IsSuccess);
        IsSkipped = Children.All(x => x.IsSkipped);
    }

    public override IEnumerable<BuTask> GetChildren()
    {
        var actualSelfChildren = Children ?? [];
        var children = new List<BuTask>();
        foreach (var child in actualSelfChildren)
        {
            children.Add(child);
            children.AddRange(child.GetChildren());
        }
        return children;
    }
}
