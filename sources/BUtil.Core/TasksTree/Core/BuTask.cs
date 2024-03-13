using BUtil.Core.Events;
using BUtil.Core.Logs;
using System;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.Core;

public abstract class BuTask
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; protected set; }
    protected readonly TaskEvents Events;
    protected readonly ILog Log;
    public virtual IEnumerable<BuTask> GetChildren()
    {
        return Array.Empty<BuTask>();
    }
    protected BuTask(ILog log, TaskEvents events, string title)
    {
        Log = log;
        Events = events;
        Title = title;
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

    protected void UpdateStatus(ProcessingStatus status)
    {
        if (status == ProcessingStatus.FinishedWithErrors)
            LogEvent(LoggingEvent.Error, LocalsHelper.ToString(status));
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
