using Avalonia.Media;
using Avalonia.Threading;
using BUtil.Core.BackupModels;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

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

    #region Commands

    public void StartTaskCommand()
    {
        IsStartButtonVisible = false;
        
        IsStopButtonEnabled  = true;

        _thread = new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            NativeMethods.PreventSleep();
            _threadTask?.Execute();
            OnTaskCompleted();
        });

        _thread.Start();
        // TODO: backupProgressUserControl.Start();
    }

    public void CloseCommand()
    {
        Environment.Exit(-1);
    }

    public void StopTaskCommand()
    {
        Environment.Exit(0);
    }

    #endregion

    #region Labels

    public string Task_Launch_Hint => Resources.Task_Launch_Hint;
    public string Button_Cancel => Resources.Button_Cancel;
    public string AfterTaskSelection_Field => Resources.AfterTaskSelection_Field;
    public string Task_List => Resources.Task_List;
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

    public void Initialize()
    {
        _task = new TaskV2StoreService().Load(_taskName);

        if (!TaskModelStrategyFactory.TryVerify(new StubLog(), _task.Model, out var error))
        {
            Messages.ShowErrorBox(error);
            this.CloseCommand();
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
        // backupProgressUserControl.SetProgress(_ended.Count, _items.Count);
    }

    private static SolidColorBrush GetResultColor(ProcessingStatus state)
    {
        return state switch
        {
            ProcessingStatus.FinishedSuccesfully => new SolidColorBrush(Colors.DarkGreen),
            ProcessingStatus.FinishedWithErrors => new SolidColorBrush(Colors.LightCoral),
            ProcessingStatus.InProgress => new SolidColorBrush(Colors.Yellow),
            ProcessingStatus.NotStarted => throw new InvalidOperationException(state.ToString()),
            _ => throw new NotImplementedException(state.ToString()),
        };
    }

    private void OnTaskCompleted()
    {
        if (_log == null)
            return;

        IsStopButtonEnabled = false;
        _log.Close();

        var powerTask = SelectedPowerTask;

        var appStaysAlive = powerTask == PowerTask.None;
        NativeMethods.StopPreventSleep();
        if (appStaysAlive)
        {
            PowerPC.DoTask(powerTask);
            string lastMinuteConsolidatedMessage = GetLastMinuteConsolidatedMessage();

            if (_log.HasErrors)
            {
                ProcessHelper.ShellExecute(_log.LogFilename);
                // backupProgressUserControl.Stop(lastMinuteConsolidatedMessage, Resources.Task_Status_FailedSeeLog, true);
            }
            else
            {
                // backupProgressUserControl.Stop(lastMinuteConsolidatedMessage, Resources.Task_Status_Succesfull, false);
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // BUtil.BackupUiMaster.NativeMethods.FlashWindow.Flash(this, 10);
            }
            return;
        }
       // if (_log.HasErrors &&
       //     RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
       //     BUtil.BackupUiMaster.NativeMethods.ScheduleOpeningFileAfterLoginOfUserIntoTheSystem(_log.LogFilename);
        PowerPC.DoTask(powerTask);
        CloseCommand();
    }

    private void OnAddLastMinuteMessageToUser(object? sender, MessageEventArgs e)
    {
        _lastMinuteMessagesToUser.Add(e.Message);
    }

    private string GetLastMinuteConsolidatedMessage()
    {
        var messages = string.Join(Environment.NewLine, _lastMinuteMessagesToUser.ToArray());
        if (!string.IsNullOrEmpty(messages))
            messages = messages + Environment.NewLine;
        return messages;
    }
}
