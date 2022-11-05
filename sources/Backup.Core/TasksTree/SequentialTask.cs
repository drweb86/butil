using BUtil.Core.Events;
using BUtil.Core.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    // icons tag
    public enum TaskArea
    {
        Folder = 0,
        File = 1,
        Ftp = 2,
        Hdd = 3,
        Network = 4,
        CompressIntoAnImage = 5,
        ProgramInRunBeforeAfterBackupChain = 6
    }

    public abstract class BuTask
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; }
        public TaskArea TaskArea { get; }
        protected BackupEvents Events { get; }
        protected LogBase Log { get; }
        public virtual IEnumerable<BuTask> GetChildren()
        {
            return Array.Empty<BuTask>();
        }
        protected BuTask(LogBase log, BackupEvents events, string title, TaskArea taskArea)
        {
            Log = log;
            Events = events;
            Title = title;
            TaskArea = taskArea;
        }

        public abstract void Execute(CancellationToken token);

        public void LogDebug(string message)
        {
            Log.WriteLine(LoggingEvent.Debug, $"{Id} {Title} {message}");
        }
    }

    public abstract class SequentialBuTask : BuTask
    {
        public IEnumerable<BuTask> Children { get; set; }

        protected SequentialBuTask(LogBase log, BackupEvents events, string title, TaskArea taskArea, IEnumerable<BuTask> children)
            :base(log, events, title, taskArea)
        {
            Children = children;
        }

        public override void Execute(CancellationToken token)
        {
            Events.TaskProgessUpdate(Id, ProcessingStatus.InProgress);
            foreach (var child in Children)
            {
                if (token.IsCancellationRequested)
                    break;

                child.Execute(token);
            }
            Events.TaskProgessUpdate(Id, ProcessingStatus.FinishedSuccesfully);
        }

        public override IEnumerable<BuTask> GetChildren()
        {
            var children = (Children ?? Array.Empty<BuTask>()).ToList();
            foreach (var child in Children)
            {
                children.AddRange(child.GetChildren());
            }
            return children;
        }
    }

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
            Events.TaskProgessUpdate(Id, ProcessingStatus.InProgress);
            foreach (var child in Children)
            {
                if (token.IsCancellationRequested)
                    break;

                child.Execute(token);
            }

            Events.TaskProgessUpdate(Id, ProcessingStatus.FinishedSuccesfully);
        }

        public override IEnumerable<BuTask> GetChildren()
        {
            var children = (Children ?? Array.Empty<BuTask>()).ToList();
            foreach (var child in children)
            {
                children.AddRange(child.GetChildren());
            }
            return children;
        }
    }
}
