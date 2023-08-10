namespace BUtil.ConsoleBackup.UI
{
    using BUtil.Core;
    using BUtil.Core.BackupModels;
    using BUtil.Core.Events;
    using BUtil.Core.Logs;
    using BUtil.Core.Misc;
    using BUtil.Core.Options;
    using BUtil.Core.TasksTree.Core;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Terminal.Gui;

    public partial class BackupDialog
    {
        private readonly BackgroundWorker _backgroundWorker = new () { WorkerSupportsCancellation = true };
        private readonly BackupTask _backupTask;
        private readonly ListViewItemDataSource _items = new();
        private readonly List<string> _lastMinuteMessagesToUser = new ();
        private readonly HashSet<Guid> _ended = new ();
        private BackupEvents _backupEvents = new();
        private FileLog _log;
        private BuTask _rootTask;

        internal BackupDialog(BackupTask backupTask) 
        {
            InitializeComponent();

            this.Title = backupTask.Name;
            _backupTask = backupTask;
            _backgroundWorker.DoWork += OnDoWork;
            _backgroundWorker.RunWorkerCompleted += OnRunWorkerCompleted;
            NativeMethods.PreventSleep();
            
            _log = new FileLog(_backupTask.Name);
            _log.Open();
            var strategy = BackupModelStrategyFactory.Create(_log, _backupTask);
            _backupEvents.OnTaskProgress += OnTaskProgress;
            _backupEvents.OnDuringExecutionTasksAdded += OnDuringExecutionTasksAdded;
            _backupEvents.OnMessage += OnAddLastMinuteMessageToUser;
            _rootTask = strategy.GetTask(_backupEvents);
            var allTasks = _rootTask.GetChildren();
            foreach (var task in allTasks)
            {
                var listItem = new ListViewItem(task);
                _items.Add(listItem);
            }
            _listView.SetSource(_items);
            _backgroundWorker.RunWorkerAsync();
        }


        private void OnRowRender(ListViewRowEventArgs args)
        {
            var listViewItem = _items[args.Row];
            args.RowAttribute = new Terminal.Gui.Attribute(Color.White, listViewItem.BackColor);
        }

        private void OnClose()
        {
            Application.RequestStop();
        }

        static Terminal.Gui.Color GetResultColor(ProcessingStatus state)
        {
            return state switch
            {
                ProcessingStatus.FinishedSuccesfully => Terminal.Gui.Color.BrightGreen,
                ProcessingStatus.FinishedWithErrors => Terminal.Gui.Color.BrightRed,
                ProcessingStatus.InProgress => Terminal.Gui.Color.BrightYellow,
                ProcessingStatus.NotStarted => throw new InvalidOperationException(state.ToString()),
                _ => throw new NotImplementedException(state.ToString()),
            };
        }

        private void UpdateListViewItem(Guid taskId, ProcessingStatus status)
        {
            foreach (ListViewItem item in _items)
            {
                if (item.Id == taskId)
                {
                    item.Status = LocalsHelper.ToString(status);
                    if (status != ProcessingStatus.NotStarted)
                    {
                        item.BackColor = GetResultColor(status);
                    }
                    break;
                }
            }
        }

        private void OnAddLastMinuteMessageToUser(object sender, MessageEventArgs e)
        {
            _lastMinuteMessagesToUser.Add(e.Message);
        }

        private void OnDuringExecutionTasksAdded(object sender, DuringExecutionTasksAddedEventArgs e)
        {
            OnDuringExecutionTasksAddedInternal(sender, e);
        }

        private void OnDuringExecutionTasksAddedInternal(object sender, DuringExecutionTasksAddedEventArgs e)
        {
            int index = 0;
            foreach (var item in _items)
            {
                index++;
                if (item.Id == e.TaskId)
                    break;
            }

            foreach (var task in e.Tasks)
            {
                var listItem = new ListViewItem(task);
                _items.Insert(index, listItem);
                index++;
            }
            Application.MainLoop.Invoke(_listView.SetNeedsDisplay);
        }

        private void OnTaskProgress(object sender, TaskProgressEventArgs e)
        {
            if (e.TaskId == _rootTask.Id)
                return;

            if (e.Status == ProcessingStatus.FinishedWithErrors ||
                e.Status == ProcessingStatus.FinishedSuccesfully)
                _ended.Add(e.TaskId);
            UpdateListViewItem(e.TaskId, e.Status);
            Application.MainLoop.Invoke(_listView.SetNeedsDisplay);
        }

        private void OnDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _rootTask.Execute();
        }

        private void OnRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _log.Close();

            NativeMethods.StopPreventSleep();

            if (_log.HasErrors)
            {
                ProcessHelper.ShellExecute(_log.LogFilename);
            }
        }
    }
}
