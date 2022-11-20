using BUtil.Core.Events;
using BUtil.Core.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree.Core
{

    public class SequentialBuTask : BuTask
    {
        public IEnumerable<BuTask> Children { get; set; }

        public SequentialBuTask(ILog log, BackupEvents events, string title, TaskArea taskArea, IEnumerable<BuTask> children)
            : base(log, events, title, taskArea)
        {
            Children = children;
        }

        public override void Execute()
        {
            foreach (var child in Children)
            {
                child.Execute();
            }
            IsSuccess = Children.All(x => x.IsSuccess);
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
}
