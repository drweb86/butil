using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.Core.TasksTree.Core
{
    public abstract class ParallelBuTask : BuTask
    {
        public IEnumerable<BuTask> Children { get; set; }

        protected ParallelBuTask(ILog log, BackupEvents events, string title, TaskArea taskArea, IEnumerable<BuTask> children)
            : base(log, events, title, taskArea)
        {
            Children = children;
        }

        public override void Execute()
        {
            var children = (Children ?? Array.Empty<BuTask>()).ToList();
            if (children.Count == 0)
            {
                IsSuccess = true;
                return;
            }

            var executer = new ParallelExecuter(children, Environment.ProcessorCount);
            executer.Wait();
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
