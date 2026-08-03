using BUtil.Core.Logs;
using BUtil.Interop.Logs;
using BUtil.Interop.Tasks.Core;
using BUtil.Interop.Tasks.Events;
using System;
using System.Collections.Generic;

namespace butilc;

/// <summary>
/// Console-facing log used while a task runs. Per-item Debug detail is already captured in full by
/// FileLog, so here it is dropped; the console instead shows a single self-updating progress line
/// (elapsed time, completed/total tasks, current activity) driven by TaskEvents, plus any Errors.
/// </summary>
public sealed class ProgressConsoleLog : LogBase
{
    private readonly TaskEvents _events;
    private readonly object _sync = new();
    private readonly HashSet<Guid> _endedTasks = [];
    private readonly List<Guid> _activeTaskOrder = [];
    private readonly Dictionary<Guid, string> _titles = [];
    private readonly DateTime _startTime = DateTime.Now;
    private readonly bool _isInteractive = !Console.IsOutputRedirected;
    private static readonly TimeSpan MinRenderInterval = TimeSpan.FromMilliseconds(150);

    private Guid? _rootTaskId;
    private int _totalTasks;
    private string _lastRenderedLine = string.Empty;
    private DateTime _lastRender = DateTime.MinValue;

    public ProgressConsoleLog(TaskEvents events)
    {
        _events = events;
        _events.OnTaskProgress += OnTaskProgress;
        _events.OnDuringExecutionTasksAdded += OnDuringExecutionTasksAdded;
    }

    public void Attach(BuTask rootTask)
    {
        lock (_sync)
        {
            _rootTaskId = rootTask.Id;
            foreach (var task in rootTask.GetChildren())
            {
                _totalTasks++;
                _titles[task.Id] = task.Title;
            }
        }
        Render(force: true);
    }

    public override void Open() { }

    public override void Close(bool isSuccess)
    {
        _events.OnTaskProgress -= OnTaskProgress;
        _events.OnDuringExecutionTasksAdded -= OnDuringExecutionTasksAdded;
        lock (_sync)
            ClearLine();
    }

    public override void WriteLine(LoggingEvent loggingEvent, string message)
    {
        if (loggingEvent != LoggingEvent.Error)
            return;

        lock (_sync)
        {
            ClearLine();
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ForegroundColor = previousColor;
            _lastRenderedLine = string.Empty;
        }
        Render(force: true);
    }

    private void OnDuringExecutionTasksAdded(object? sender, DuringExecutionTasksAddedEventArgs e)
    {
        lock (_sync)
        {
            foreach (var task in e.Tasks)
            {
                _totalTasks++;
                _titles[task.Id] = task.Title;
            }
        }
        Render(force: false);
    }

    private void OnTaskProgress(object? sender, TaskProgressEventArgs e)
    {
        if (e.TaskId == _rootTaskId)
            return;

        lock (_sync)
        {
            if (e.Title != null)
                _titles[e.TaskId] = e.Title;

            if (e.Status == ProcessingStatus.InProgress)
            {
                _activeTaskOrder.Remove(e.TaskId);
                _activeTaskOrder.Add(e.TaskId);
            }
            else if (e.Status is ProcessingStatus.FinishedSuccesfully or ProcessingStatus.FinishedWithErrors or ProcessingStatus.Skipped)
            {
                _endedTasks.Add(e.TaskId);
                _activeTaskOrder.Remove(e.TaskId);
            }
        }
        Render(force: false);
    }

    private void Render(bool force)
    {
        lock (_sync)
        {
            if (!force && DateTime.Now - _lastRender < MinRenderInterval)
                return;
            _lastRender = DateTime.Now;

            var completed = _endedTasks.Count;
            var currentTitle = _activeTaskOrder.Count > 0 && _titles.TryGetValue(_activeTaskOrder[^1], out var title)
                ? title
                : string.Empty;
            var elapsed = FormatElapsed(DateTime.Now - _startTime);

            var line = _totalTasks > 0
                ? $"[{elapsed}] {completed}/{_totalTasks} ({Math.Min(100, completed * 100 / _totalTasks)}%) {currentTitle}"
                : $"[{elapsed}] {currentTitle}";

            if (line == _lastRenderedLine)
                return;
            _lastRenderedLine = line;

            if (_isInteractive)
            {
                var width = SafeWindowWidth();
                var truncated = line.Length >= width ? line[..(width - 1)] : line;
                Console.Write("\r" + truncated.PadRight(width - 1));
            }
            else
            {
                Console.WriteLine(line);
            }
        }
    }

    private void ClearLine()
    {
        if (!_isInteractive || _lastRenderedLine.Length == 0)
            return;
        Console.Write("\r" + new string(' ', SafeWindowWidth() - 1) + "\r");
    }

    private static int SafeWindowWidth()
    {
        try
        {
            var width = Console.WindowWidth;
            return width > 10 ? width : 80;
        }
        catch
        {
            return 80;
        }
    }

    private static string FormatElapsed(TimeSpan span)
    {
        return span.TotalHours >= 1
            ? $"{(int)span.TotalHours}:{span.Minutes:00}:{span.Seconds:00}"
            : $"{span.Minutes:00}:{span.Seconds:00}";
    }
}
