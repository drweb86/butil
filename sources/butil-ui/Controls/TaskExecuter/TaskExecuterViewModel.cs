using Avalonia.Media;
using Avalonia.Threading;
using BUtil.Core;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.TasksTree.Core;
using butil_ui.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace butil_ui.Controls;

public class TaskExecuterViewModel : ObservableObject
{
    private DateTime _startTime;
    private readonly System.Timers.Timer _timer = new(1000);
    private readonly HashSet<Guid> _endedTasks = new();
    private readonly Action<bool> _onTaskComplete = null!;
    private FileLog? _log;
    private BuTask _threadTask = null!;
    private Thread? _thread;

    public TaskExecuterViewModel(string error)
    {
        ProgressGenericTitle = error;
        _progressGenericForeground = ColorPalette.GetBrush(SemanticColor.Error);
        CanClose = true;
        IsStartButtonVisible = false;
        TaskNotCompleted = false;
    }

    public TaskExecuterViewModel(
        TaskEvents taskEvents,
        string logName,
        Func<ILog, TaskEvents, Action<string?>, BuTask> createTask,
        Action<bool> onTaskComplete)
    {
        _progressGenericForeground = ColorPalette.GetBrush(SemanticColor.Normal);

        taskEvents.OnTaskProgress += OnTaskProgress;
        taskEvents.OnDuringExecutionTasksAdded += OnDuringExecutionTasksAdded;

        _timer.Enabled = false;
        _timer.Elapsed += OnElapsedTimerTick;

        _startTime = DateTime.Now;

        _log = new FileLog(logName);
        _log.Open();
        _threadTask = createTask(_log, taskEvents, OnPopulateLastMinuteMessage);


        _threadTask
            .GetChildren()
            .Select(x => new LaunchTaskViewItem(x, ProcessingStatus.NotStarted))
            .ToList()
            .ForEach(x =>
            {
                _items.Add(x);
                _itemsDict.AddOrUpdate(x.Tag, x, (a, b) => b);
            });

        _onTaskComplete = onTaskComplete;
    }

    private string? _lastMinuteMessage;
    private void OnPopulateLastMinuteMessage(string? message)
    {
        _lastMinuteMessage = message;
    }

    #region IsCollapsed

    private bool _isCollapsed = false;
    public bool IsCollapsed
    {
        get
        {
            return _isCollapsed;
        }
        set
        {
            if (value == _isCollapsed)
                return;
            _isCollapsed = value;
            OnPropertyChanged(nameof(IsCollapsed));
        }
    }

    #endregion

    #region TotalTasksCount

    private int _totalTasksCount = 0;
    public int TotalTasksCount
    {
        get
        {
            return _totalTasksCount;
        }
        set
        {
            if (value == _totalTasksCount)
                return;
            _totalTasksCount = value;
            OnPropertyChanged(nameof(TotalTasksCount));
        }
    }

    #endregion

    #region ProgressGenericForeground

    private SolidColorBrush _progressGenericForeground;
    public SolidColorBrush ProgressGenericForeground
    {
        get
        {
            return _progressGenericForeground;
        }
        set
        {
            if (value == _progressGenericForeground)
                return;
            _progressGenericForeground = value;
            OnPropertyChanged(nameof(ProgressGenericForeground));
        }
    }

    #endregion

    #region CanClose

    private bool _canClose = false;
    public bool CanClose
    {
        get
        {
            return _canClose;
        }
        set
        {
            if (value == _canClose)
                return;
            _canClose = value;
            OnPropertyChanged(nameof(CanClose));
        }
    }

    #endregion

    #region CompletedTasksCount

    private int _completedTasksCount = 0;
    public int CompletedTasksCount
    {
        get
        {
            return _completedTasksCount;
        }
        set
        {
            if (value == _completedTasksCount)
                return;
            _completedTasksCount = value;
            OnPropertyChanged(nameof(CompletedTasksCount));
        }
    }

    #endregion

    #region Items

    private ConcurrentDictionary<Guid, LaunchTaskViewItem> _itemsDict = new ConcurrentDictionary<Guid, LaunchTaskViewItem>();
    private ObservableCollection<LaunchTaskViewItem> _items = new();
    public ObservableCollection<LaunchTaskViewItem> Items
    {
        get
        {
            return _items;
        }
        set
        {
            if (value == _items)
                return;
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    #endregion

    #region ElapsedLabel

    private string _elapsedLabel = string.Empty;
    public string ElapsedLabel
    {
        get
        {
            return _elapsedLabel;
        }
        set
        {
            if (value == _elapsedLabel)
                return;
            _elapsedLabel = value;
            OnPropertyChanged(nameof(ElapsedLabel));
        }
    }

    #endregion

    #region IsStopButtonEnabled

    private bool _isStopButtonEnabled = false;
    public bool IsStopButtonEnabled
    {
        get
        {
            return _isStopButtonEnabled;
        }
        set
        {
            if (value == _isStopButtonEnabled)
                return;
            _isStopButtonEnabled = value;
            OnPropertyChanged(nameof(IsStopButtonEnabled));
        }
    }

    #endregion

    #region IsStartButtonVisible

    private bool _isStartButtonVisible = true;
    public bool IsStartButtonVisible
    {
        get
        {
            return _isStartButtonVisible;
        }
        set
        {
            if (value == _isStartButtonVisible)
                return;
            _isStartButtonVisible = value;
            OnPropertyChanged(nameof(IsStartButtonVisible));
        }
    }

    #endregion

    #region TaskNotCompleted

    private bool _taskNotCompleted = true;
    public bool TaskNotCompleted
    {
        get
        {
            return _taskNotCompleted;
        }
        set
        {
            if (value == _taskNotCompleted)
                return;
            _taskNotCompleted = value;
            OnPropertyChanged(nameof(TaskNotCompleted));
        }
    }

    #endregion

    #region ProgressGenericTitle

    private string _progressGenericTitle = Resources.Task_List;
    public string ProgressGenericTitle
    {
        get
        {
            return _progressGenericTitle;
        }
        set
        {
            if (value == _progressGenericTitle)
                return;
            _progressGenericTitle = value;
            OnPropertyChanged(nameof(ProgressGenericTitle));
        }
    }

    #endregion

    #region SelectedPowerTask

    private PowerTask _selectedPowerTask = PowerTask.None;

    public PowerTask SelectedPowerTask
    {
        get
        {
            return _selectedPowerTask;
        }
        set
        {
            if (value == _selectedPowerTask)
                return;
            _selectedPowerTask = value;
            OnPropertyChanged(nameof(SelectedPowerTask));
        }
    }

    #endregion

    #region Commands

    public void StartTaskCommand()
    {
        IsStartButtonVisible = false;
        IsStopButtonEnabled = true;
        ProgressGenericTitle = Resources.Task_Status_InProgress;

        _startTime = DateTime.Now;
        _timer.Enabled = true;

        _thread = new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            PlatformSpecificExperience.Instance.OsSleepPreventionService.PreventSleep();
            _threadTask?.Execute();
            Dispatcher.UIThread.Invoke(() => OnTaskCompleted(_threadTask?.IsSuccess ?? false));
        });
        _thread.Start();
    }

    #endregion

    private void UpdateListViewItem(Guid taskId, ProcessingStatus status)
    {
        if (status == ProcessingStatus.NotStarted)
            return;

        if (_itemsDict.TryGetValue(taskId, out var item))
        {
            item.Status = status;
        }
    }

    private void OnDuringExecutionTasksAddedInternal(object? sender, DuringExecutionTasksAddedEventArgs e)
    {
        int index = 0;
        foreach (var item in _items)
        {
            index++;
            if (item.Tag == e.TaskId)
                break;
        }

        foreach (var task in e.Tasks)
        {
            var listItem = new LaunchTaskViewItem(task, ProcessingStatus.NotStarted);
            _items.Insert(index, listItem);
            _itemsDict.AddOrUpdate(listItem.Tag, listItem, (a, b) => b);
            index++;
        }
    }

    private void OnTaskProgress(object? sender, TaskProgressEventArgs e)
    {
        if (e.TaskId == _threadTask?.Id)
            return;

        if (e.Status == ProcessingStatus.FinishedWithErrors ||
            e.Status == ProcessingStatus.FinishedSuccesfully)
            lock (_endedTasks)
            {
                _endedTasks.Add(e.TaskId);
            }

        ScheduleUpdate(() => UpdateListViewItem(e.TaskId, e.Status));
    }

    private void OnElapsedTimerTick(object? sender, EventArgs e)
    {
        TimeSpan span = DateTime.Now.Subtract(_startTime);
        ElapsedLabel = $"{Resources.Time_Field_Elapsed} {TimeSpanToStringHelper(span)} ({CompletedTasksCount}/{TotalTasksCount})";
    }

    private void OnDuringExecutionTasksAdded(object? sender, DuringExecutionTasksAddedEventArgs e)
    {
        ScheduleUpdate(() => OnDuringExecutionTasksAddedInternal(sender, e));
    }

    private void ScheduleUpdate(Action update)
    {
        Dispatcher.UIThread.Invoke(update);
        SetProgress(_endedTasks.Count, _items.Count);
    }

    private string TimeSpanToStringHelper(TimeSpan timeSpan)
    {
        if (timeSpan.Days > 0)
            return $"{timeSpan.Days}:{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        else
        {
            if (timeSpan.Hours > 0)
                return $"{timeSpan.Hours}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            else
            {
                if (timeSpan.Minutes > 0)
                    return $"{timeSpan.Minutes}:{timeSpan.Seconds:00}";
                else
                    return $"{timeSpan.Seconds}";
            }
        }
    }

    private void SetProgress(int ended, int total)
    {
        CompletedTasksCount = ended;
        TotalTasksCount = total;
    }

    private string? GetLastMinuteConsolidatedMessage()
    {
        return _lastMinuteMessage;
    }

    private void OnTaskCompleted(bool isSuccess)
    {
        if (_log == null)
            return;

        _log.Close(isSuccess);

        _timer.Enabled = false;
        _timer.Stop();

        IsStopButtonEnabled = false;
        TaskNotCompleted = false;
        CanClose = true;

        var lastMinuteMessage = GetLastMinuteConsolidatedMessage();
        if (!isSuccess)
        {
            ProgressGenericTitle = $"{Resources.Task_Status_FailedSeeLog} ({TimeSpanToStringHelper(DateTime.Now.Subtract(_startTime))})";
            if (!string.IsNullOrEmpty(lastMinuteMessage))
            {
                ProgressGenericTitle += Environment.NewLine + lastMinuteMessage;
            }

            ProgressGenericForeground = ColorPalette.GetBrush(SemanticColor.Error);
        }
        else
        {
            ProgressGenericTitle = $"{Resources.Task_Status_Succesfull} ({TimeSpanToStringHelper(DateTime.Now.Subtract(_startTime))})";
            if (!string.IsNullOrEmpty(lastMinuteMessage))
            {
                ProgressGenericTitle += Environment.NewLine + lastMinuteMessage;
            }
            ProgressGenericForeground = ColorPalette.GetBrush(SemanticColor.Success);
        }


        var appStaysAlive = _selectedPowerTask == PowerTask.None;
        if (appStaysAlive)
        {
            if (!isSuccess)
            {
                ProcessHelper.ShellExecute(_log.LogFilename);
            }

            PlatformSpecificExperience.Instance.UiService.Blink();
            PlatformSpecificExperience.Instance.OsSleepPreventionService.StopPreventSleep();
            _onTaskComplete(isSuccess);
            return;
        }

        if (!isSuccess)
        {
            PlatformSpecificExperience.Instance
                .GetShowLogOnSystemLoginService()
                ?.ShowLogOnSystemLoginService(_log.LogFilename);
        }

        PlatformSpecificExperience.Instance.OsSleepPreventionService.StopPreventSleep();
        PlatformSpecificExperience.Instance.SessionService.DoTask(_selectedPowerTask);
        _onTaskComplete(isSuccess);

        Environment.Exit(!isSuccess ? -1 : 0);
    }
}
