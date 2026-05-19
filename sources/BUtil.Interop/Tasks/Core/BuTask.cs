using BUtil.Interop.Logs;
using BUtil.Interop.Tasks.Events;

namespace BUtil.Interop.Tasks.Core;

public abstract class BuTask(ILog log, TaskEvents events, string title)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; protected set; } = title;
    protected readonly TaskEvents Events = events;
    protected readonly ILog Log = log;

    public virtual IEnumerable<BuTask> GetChildren() => [];

    public abstract void Execute();

    protected void LogDebug(string message) => LogEvent(LoggingEvent.Debug, message);

    protected void LogError(string message) => LogEvent(LoggingEvent.Error, message);

    protected void UpdateStatus(ProcessingStatus status)
    {
        if (status == ProcessingStatus.FinishedSuccesfully)
            IsSuccess = true;
        if (status == ProcessingStatus.FinishedWithErrors)
            IsSuccess = false;
        if (status == ProcessingStatus.Skipped)
        {
            IsSuccess = true;
            IsSkipped = true;
        }

        if (status == ProcessingStatus.FinishedWithErrors)
            LogEvent(LoggingEvent.Error, "\u274c");
        Events.TaskProgessUpdate(Id, status, null);
    }

    protected void UpdateTitle(string title)
    {
        Title = title;
        Events.TaskProgessUpdate(Id, null, title);
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
    public bool IsSkipped { get; protected set; }
}

public class FunctionBuTaskV2<TResult>(ILog log, TaskEvents events, string title, Func<TResult> action) : BuTaskV2(log, events, title)
{
    private readonly Func<TResult> _action = action;
    public TResult? Result { get; protected set; }

    protected override void ExecuteInternal() => Result = _action();
}

public abstract class BuTaskV2(ILog log, TaskEvents events, string title) : BuTask(log, events, title)
{
    protected abstract void ExecuteInternal();

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        try
        {
            ExecuteInternal();
            IsSuccess = true;
            UpdateStatus(IsSkipped ? ProcessingStatus.Skipped : ProcessingStatus.FinishedSuccesfully);
        }
        catch (Exception ex)
        {
            LogError(ex.ToString());
            UpdateStatus(ProcessingStatus.FinishedWithErrors);
        }
    }
}
