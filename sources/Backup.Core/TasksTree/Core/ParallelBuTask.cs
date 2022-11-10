using BUtil.Core.Events;
using BUtil.Core.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree.Core
{
    public abstract class ParallelBuTask : BuTask
    {
        public IEnumerable<BuTask> Children { get; set; }

        protected ParallelBuTask(LogBase log, BackupEvents events, string title, TaskArea taskArea, IEnumerable<BuTask> children)
            : base(log, events, title, taskArea)
        {
            Children = children;
        }

        public override void Execute(CancellationToken token)
        {
            var children = (Children ?? Array.Empty<BuTask>()).ToList();
            UpdateStatus(ProcessingStatus.InProgress);
            foreach (var child in Children)
            {
                if (token.IsCancellationRequested)
                    break;

                child.Execute(token);
            }

            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
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
