using Avalonia;
using Avalonia.Media;
using BUtil.Core.BackupModels;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace butil_ui.ViewModels;

public class LaunchTaskViewModel : PageViewModelBase
{
    public string TaskName { get; set; }

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
            this.OnPropertyChanged(nameof(SelectedPowerTask));
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
            this.OnPropertyChanged(nameof(IsStartButtonVisible));
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
            this.OnPropertyChanged(nameof(IsStopButtonEnabled));
        }
    }

    #endregion

    public void StartButtonClick()
    {
        IsStartButtonVisible = false;
        IsStopButtonEnabled  = true;

        // TODO: backupProgressUserControl.Start();
        // TODO: _backgroundWorker.RunWorkerAsync();
    }

    public void CloseButtonClick()
    {
        Environment.Exit(-1);
    }

    public void StopButtonClick()
    {
        Environment.Exit(0);
    }

    #region Labels

    public string Task_Launch_Hint => Resources.Task_Launch_Hint;

    public string Button_Cancel => Resources.Button_Cancel;

    public string AfterTaskSelection_Field => Resources.AfterTaskSelection_Field;

    public string Task_List => Resources.Task_List;
    public string Button_Close => Resources.Button_Close;
    public string AfterTaskSelection_Help => Resources.AfterTaskSelection_Help;

    public string[] PowerTasks
    {
        get => new[] { Resources.AfterTaskSelection_ShutdownPc,
                Resources.AfterTaskSelection_LogOff,
                Resources.AfterTaskSelection_Reboot,
                Resources.AfterTaskSelection_DoNothing };
        protected set => throw new NotSupportedException();
    }

    #endregion

    #region Items

    private ObservableCollection<ListViewItem> _items = new();
    public ObservableCollection<ListViewItem> Items
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
            this.OnPropertyChanged(nameof(Items));
        }
    }

    #endregion

    private TaskV2 _backupTask;
    private ConcurrentQueue<Action> _listViewUpdates = new();
    
    private List<string> _lastMinuteMessagesToUser = new List<string>();
    private HashSet<Guid> _ended = new HashSet<Guid>();
    private FileLog _log;
    private ITaskModelStrategy _strategy;
    private TaskEvents _backupEvents = new TaskEvents();
    private BuTask _rootTask;

    public void Load()
    {
        _backupTask = new TaskV2StoreService().Load(this.TaskName);

        // TODO: OnTasksListViewResize(this, new EventArgs());
        NativeMethods.PreventSleep();

        _log = new FileLog(_backupTask.Name);
        _log.Open();
        _strategy = TaskModelStrategyFactory.Create(_log, _backupTask);
        _backupEvents.OnTaskProgress += OnTaskProgress;
        _backupEvents.OnDuringExecutionTasksAdded += OnDuringExecutionTasksAdded;
        _backupEvents.OnMessage += OnAddLastMinuteMessageToUser;
        _rootTask = _strategy.GetTask(_backupEvents);

            var allTasks = _rootTask.GetChildren();
            foreach (var task in allTasks)
            {
                var listItem = new ListViewItem { Text = task.Title };
                listItem.Tag = task.Id;
                _items.Add(listItem);
            }
            // TODO: tasksListView.VirtualListSize = _items.Count;

        if (!TaskModelStrategyFactory.TryVerify(this._log, _backupTask.Model, out var error))
        {
            Messages.ShowErrorBox(error);
            this.CloseButtonClick();
        }
    }


    private void OnTaskProgress(object sender, TaskProgressEventArgs e)
    {
        if (e.TaskId == _rootTask.Id)
            return;

        if (e.Status == ProcessingStatus.FinishedWithErrors ||
            e.Status == ProcessingStatus.FinishedSuccesfully)
            _ended.Add(e.TaskId);
        _listViewUpdates.Enqueue(() => UpdateListViewItem(e.TaskId, e.Status));
    }

    private void UpdateListViewItem(Guid taskId, ProcessingStatus status)
    {
        foreach (ListViewItem item in _items)
        {
            if ((Guid)item.Tag == taskId)
            {
                if (status != ProcessingStatus.NotStarted)
                {
                    item.BackColor = GetResultColor(status);
                }
                break;
            }
        }
    }

    static Color GetResultColor(ProcessingStatus state)
    {
        return state switch
        {
            ProcessingStatus.FinishedSuccesfully => Colors.LightGreen,
            ProcessingStatus.FinishedWithErrors => Colors.LightCoral,
            ProcessingStatus.InProgress => Colors.Yellow,
            ProcessingStatus.NotStarted => throw new InvalidOperationException(state.ToString()),
            _ => throw new NotImplementedException(state.ToString()),
        };
    }

    private void OnDuringExecutionTasksAdded(object sender, DuringExecutionTasksAddedEventArgs e)
    {
        _listViewUpdates.Enqueue(() => OnDuringExecutionTasksAddedInternal(sender, e));
    }

    private void OnDuringExecutionTasksAddedInternal(object sender, DuringExecutionTasksAddedEventArgs e)
    {
        int index = 0;
        foreach (var item in _items)
        {
            index++;
            if ((Guid)item.Tag == e.TaskId)
                break;
        }

        foreach (var task in e.Tasks)
        {
            var listItem = new ListViewItem { Text = task.Title };
            listItem.Tag = task.Id;
            _items.Insert(index, listItem);
            index++;
        }
        // TODO: tasksListView.VirtualListSize = _items.Count;
    }


    private void OnAddLastMinuteMessageToUser(object sender, MessageEventArgs e)
    {
        _lastMinuteMessagesToUser.Add(e.Message);
    }
}

public class ListViewItem: AvaloniaObject
{
    public Guid Tag { get; set; }
    public string Text { get; set; }
    public Color BackColor { get; set; }

}

class Messages
{
    public static void ShowErrorBox(string error)
    {
        
        // TODO:
    }
}
/*
        private void OnTasksListViewResize(object sender, EventArgs e)
        {
            int newWidth = tasksListView.Width - 35;
            taskNameColumnHeader.Width = newWidth < 35 ? 35 : newWidth;
        }

        private void OnDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _rootTask.Execute();
        }

        private void OnRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            cancelButton.Enabled = false;
            _log.Close();

            var powerTask = (PowerTask)_powerTaskComboBox.SelectedIndex;

            var appStaysAlive = powerTask == PowerTask.None;
            NativeMethods.StopPreventSleep();
            if (appStaysAlive)
            {
                PowerPC.DoTask(powerTask);
                string lastMinuteConsolidatedMessage = GetLastMinuteConsolidatedMessage();

                if (_log.HasErrors)
                {
                    ProcessHelper.ShellExecute(_log.LogFilename);
                    backupProgressUserControl.Stop(lastMinuteConsolidatedMessage, Resources.Task_Status_FailedSeeLog, true);
                }
                else
                {
                    backupProgressUserControl.Stop(lastMinuteConsolidatedMessage, Resources.Task_Status_Succesfull, false);
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    BUtil.BackupUiMaster.NativeMethods.FlashWindow.Flash(this, 10);
                return;
            }
            if (_log.HasErrors &&
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                BUtil.BackupUiMaster.NativeMethods.ScheduleOpeningFileAfterLoginOfUserIntoTheSystem(_log.LogFilename);
            PowerPC.DoTask(powerTask);
            Close();
        }

        private string GetLastMinuteConsolidatedMessage()
        {
            var messages = string.Join(Environment.NewLine, _lastMinuteMessagesToUser.ToArray());
            if (!string.IsNullOrEmpty(messages))
                messages = messages + Environment.NewLine;
            return messages;
        }

        private void OnListViewFlushUpdates(object sender, EventArgs e)
        {
            tasksListView.BeginUpdate();
            while (_listViewUpdates.TryDequeue(out var action))
                action();
            backupProgressUserControl.SetProgress(_ended.Count, _items.Count);
            tasksListView.EndUpdate();
        }

        private void OnRetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex >= 0 && e.ItemIndex < _items.Count)
            {
                e.Item = _items[e.ItemIndex];
            }
        }

    }
}
*/