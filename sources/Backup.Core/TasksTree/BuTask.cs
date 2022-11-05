using BUtil.Core.Events;
using BUtil.Core.Logs;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BUtil.Core.TasksTree
{
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
}
