using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using BUtil.Core.BackupModels;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.OSSpecific;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;

namespace butil_ui.ViewModels;

public class LaunchTaskViewModel : PageViewModelBase
{
    public LaunchTaskViewModel(string taskName)
    {
        _taskName = taskName;
        WindowTitle = taskName;

        _taskEvents.OnTaskProgress += OnTaskProgress;
        _taskEvents.OnDuringExecutionTasksAdded += OnDuringExecutionTasksAdded;
        _taskEvents.OnMessage += OnAddLastMinuteMessageToUser;

        _timer.Enabled = false;
        _timer.Elapsed += OnElapsedTimerTick;
    }

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

    #region ProgressGenericBackground

    private SolidColorBrush _progressGenericBackground = new SolidColorBrush(Colors.White);
    public SolidColorBrush ProgressGenericBackground
    {
        get
        {
            return _progressGenericBackground;
        }
        set
        {
            if (value == _progressGenericBackground)
                return;
            _progressGenericBackground = value;
            OnPropertyChanged(nameof(ProgressGenericBackground));
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

    #region Items

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

    #region Commands

    public void StartTaskCommand()
    {
        IsStartButtonVisible = false;
        IsStopButtonEnabled  = true;
        ProgressGenericTitle = Resources.Task_Status_InProgress;

        _startTime = DateTime.Now;
        _timer.Enabled = true;

        _thread = new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            NativeMethods.PreventSleep();
            _threadTask?.Execute();
            Dispatcher.UIThread.Invoke(OnTaskCompleted);
        });
        _thread.Start();
    }

    public void CloseCommand()
    {
        Environment.Exit(0);
    }

    public void StopTaskCommand()
    {
        Environment.Exit(-1);
    }

    #endregion

    #region Labels
    public string Task_Launch_Hint => Resources.Task_Launch_Hint;
    public string Button_Cancel => Resources.Button_Cancel;
    public string AfterTaskSelection_Field => Resources.AfterTaskSelection_Field;
    public string Button_Close => Resources.Button_Close;
    public string AfterTaskSelection_Help => Resources.AfterTaskSelection_Help;
    public string[] PowerTasks
    {
        get => new[] {
            Resources.AfterTaskSelection_ShutdownPc,
            Resources.AfterTaskSelection_LogOff,
            Resources.AfterTaskSelection_Reboot,
            Resources.AfterTaskSelection_DoNothing,
        };
    }

    #endregion

    private readonly List<string> _lastMinuteMessagesToUser = new();
    private readonly HashSet<Guid> _endedTasks = new();
    private readonly TaskEvents _taskEvents = new();
    private readonly string _taskName;
    private TaskV2? _task;
    private FileLog? _log;
    private BuTask? _threadTask;
    private Thread? _thread;
    private DateTime _startTime = DateTime.Now;
    private readonly System.Timers.Timer _timer = new (1000);
    private readonly Color _redColor = Color.FromRgb(222, 98, 89);
    private readonly Color _greenColor = Color.FromRgb(147, 199,93);

    public void Initialize()
    {
        _task = new TaskV2StoreService().Load(_taskName);
        if (_task == null)
        {
            ProgressGenericTitle = BUtil.Core.Localization.Resources.Task_Validation_NotSupported;
            ProgressGenericBackground = new SolidColorBrush(_redColor);
            CanClose = true;
            IsStartButtonVisible = false;
            TaskNotCompleted = false;
            return;
        }

        if (!TaskModelStrategyFactory.TryVerify(new StubLog(), _task.Model, out var error))
        {
            ProgressGenericTitle = error;
            ProgressGenericBackground = new SolidColorBrush(_redColor);
            CanClose = true;
            IsStartButtonVisible = false;
            TaskNotCompleted = false;
            return;
        }

        _log = new FileLog(_task.Name);
        _log.Open();

        _threadTask = TaskModelStrategyFactory
            .Create(_log, _task)
            .GetTask(_taskEvents);

        _threadTask
            .GetChildren()
            .Select(x => new LaunchTaskViewItem(x))
            .ToList()
            .ForEach(_items.Add);
    }

    private void OnTaskProgress(object? sender, TaskProgressEventArgs e)
    {
        if (e.TaskId == _threadTask?.Id)
            return;

        if (e.Status == ProcessingStatus.FinishedWithErrors ||
            e.Status == ProcessingStatus.FinishedSuccesfully)
            _endedTasks.Add(e.TaskId);
        ScheduleUpdate(() => UpdateListViewItem(e.TaskId, e.Status));
    }

    private void UpdateListViewItem(Guid taskId, ProcessingStatus status)
    {
        foreach (LaunchTaskViewItem item in _items)
        {
            if (item.Tag == taskId)
            {
                if (status != ProcessingStatus.NotStarted)
                {
                    item.BackColor = GetResultColor(status);
                }
                break;
            }
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
            var listItem = new LaunchTaskViewItem(task);
            _items.Insert(index, listItem);
            index++;
        }
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

    private SolidColorBrush GetResultColor(ProcessingStatus state)
    {
        return state switch
        {
            ProcessingStatus.FinishedSuccesfully => new SolidColorBrush(_greenColor),
            ProcessingStatus.FinishedWithErrors => new SolidColorBrush(_redColor),
            ProcessingStatus.InProgress => new SolidColorBrush(Colors.Yellow),
            ProcessingStatus.NotStarted => throw new InvalidOperationException(state.ToString()),
            _ => throw new NotImplementedException(state.ToString()),
        };
    }

    private void OnTaskCompleted()
    {
        if (_log == null)
            return;

        _log.Close();

        _timer.Enabled = false;
        _timer.Stop();

        IsStopButtonEnabled = false;
        TaskNotCompleted = false;
        CanClose = true;

        var lastMinuteMessage = GetLastMinuteConsolidatedMessage();
        if (_log.HasErrors)
        {
            WindowTitle = $"{_taskName} - {Resources.Task_Status_FailedSeeLog}";
            ProgressGenericTitle = $"{Resources.Task_Status_FailedSeeLog} ({TimeSpanToStringHelper(DateTime.Now.Subtract(_startTime))})";
            if (!string.IsNullOrEmpty(lastMinuteMessage))
            {
                ProgressGenericTitle += Environment.NewLine + lastMinuteMessage;
            }

            ProgressGenericBackground = new SolidColorBrush(_redColor);
        }
        else
        {
            WindowTitle = $"{_taskName} - {Resources.Task_Status_Succesfull}";
            ProgressGenericTitle = $"{Resources.Task_Status_Succesfull} ({TimeSpanToStringHelper(DateTime.Now.Subtract(_startTime))})";
            if (!string.IsNullOrEmpty(lastMinuteMessage))
            {
                ProgressGenericTitle += Environment.NewLine + lastMinuteMessage;
            }
            ProgressGenericBackground = new SolidColorBrush(_greenColor);
        }

        var appStaysAlive = _selectedPowerTask == PowerTask.None;
        if (appStaysAlive)
        {
            if (_log.HasErrors)
            {
                ProcessHelper.ShellExecute(_log.LogFilename);
            }
            OSSpecific.FlashWindow.Flash(10);
            NativeMethods.StopPreventSleep();
            return;
        }
        
        if (_log.HasErrors)
            OSSpecific.ScheduleOpeningFileAfterLoginOfUserIntoTheSystem(_log.LogFilename);

        NativeMethods.StopPreventSleep();
        PowerPC.DoTask(_selectedPowerTask);
        Environment.Exit(_log.HasErrors ? -1 : 0);
    }

    private void OnAddLastMinuteMessageToUser(object? sender, MessageEventArgs e)
    {
        _lastMinuteMessagesToUser.Add(e.Message);
    }

    private string GetLastMinuteConsolidatedMessage()
    {
        return string.Join(Environment.NewLine, _lastMinuteMessagesToUser.ToArray());
    }

    public void SetProgress(int ended, int total)
    {
        CompletedTasksCount = ended;
        TotalTasksCount = total;
    }

    private void OnElapsedTimerTick(object sender, EventArgs e)
    {
        TimeSpan span = DateTime.Now.Subtract(_startTime);
        ElapsedLabel = $"{Resources.Time_Field_Elapsed} {TimeSpanToStringHelper(span)} ({CompletedTasksCount}/{TotalTasksCount})";
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
}
