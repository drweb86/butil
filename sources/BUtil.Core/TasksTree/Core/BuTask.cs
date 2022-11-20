using BUtil.Core.Events;
using BUtil.Core.Logs;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BUtil.Core.TasksTree.Core
{
    public abstract class BuTask
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; }
        public TaskArea TaskArea { get; }
        protected readonly BackupEvents Events;
        protected readonly ILog Log;
        public virtual IEnumerable<BuTask> GetChildren()
        {
            return Array.Empty<BuTask>();
        }
        protected BuTask(ILog log, BackupEvents events, string title, TaskArea taskArea)
        {
            Log = log;
            Events = events;
            Title = title;
            TaskArea = taskArea;
        }

        public abstract void Execute();

        protected void LogDebug(string message)
        {
            LogEvent(LoggingEvent.Debug, message);
        }

        protected void LogError(string message)
        {
            LogEvent(LoggingEvent.Error, message);
        }

        protected void LogWarning(string message)
        {
            LogEvent(LoggingEvent.Warning, message);
        }

        protected void UpdateStatus(ProcessingStatus status)
        {
            LogEvent(status == ProcessingStatus.FinishedWithErrors ? LoggingEvent.Error : LoggingEvent.Debug, LocalsHelper.ToString(status));
            Events.TaskProgessUpdate(Id, status);
        }

        protected void LogEvent(LoggingEvent logEvent, string message)
        {
            var lines = message
                .Replace("\r\n", "\n")
                .Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
                if (!string.IsNullOrWhiteSpace(line))
                    Log.WriteLine(logEvent, $"{Title}: {line}");
        }

        public bool IsSuccess { get; protected set; }
    }
}
