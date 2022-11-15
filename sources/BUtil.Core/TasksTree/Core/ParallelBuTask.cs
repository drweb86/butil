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
        public ProgramOptions ProgramOptions { get; }
        public IEnumerable<BuTask> Children { get; set; }

        protected ParallelBuTask(ILog log, BackupEvents events, string title, TaskArea taskArea, ProgramOptions programOptions, IEnumerable<BuTask> children)
            : base(log, events, title, taskArea)
        {
            ProgramOptions = programOptions;
            Children = children;
        }

        public override void Execute(CancellationToken token)
        {
            var children = (Children ?? Array.Empty<BuTask>()).ToList();
            if (children.Count == 0)
            {
                IsSuccess = true;
                return;
            }

            ThreadPool.SetMinThreads(ProgramOptions.Parallel, ProgramOptions.Parallel);
            ThreadPool.SetMaxThreads(ProgramOptions.Parallel, ProgramOptions.Parallel);

            using var resetEvent = new ManualResetEvent(false);
            int toProcess = children.Count;
            foreach (var child in children)
            {
                ThreadPool.QueueUserWorkItem(a =>
                {
                    try
                    {
                        if (!token.IsCancellationRequested)
                            child.Execute(token);
                    }
                    finally
                    {
                        if (Interlocked.Decrement(ref toProcess) == 0)
                            resetEvent.Set();
                    }
                });
            }

            resetEvent.WaitOne();
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
