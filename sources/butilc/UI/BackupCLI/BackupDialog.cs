using BUtil.ConsoleBackup.Localization;
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
using System.Linq;
using Terminal.Gui;

namespace BUtil.ConsoleBackup.UI
{
    public partial class BackupDialog
    {
        private readonly BackupTask _task;
        private readonly List<string> _lastMinuteMessagesToUser = new ();
        private readonly FileLog _log;
        private readonly BuTask _rootTask;

        internal BackupDialog(BackupTask task) 
        {
            InitializeComponent();

            this.Title = task.Name;
            _task = task;

            _log = new FileLog(_task.Name);
            _log.Open();

            _rootTask = BackupModelStrategyFactory
                .Create(_log, _task)
                .GetTask(_backupEvents);

            _dataSource.AddRange(_rootTask
                .GetChildren()
                .Select(x => new ListViewItem(x)));

            _backgroundWorker.RunWorkerAsync();
        }

        private void OnRenderRow(ListViewRowEventArgs args)
        {
            var item = _dataSource[args.Row];
            args.RowAttribute = new Terminal.Gui.Attribute(item.ForeColor, item.BackColor);
        }

        private void OnClickClose()
        {
            Application.RequestStop();
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
            foreach (var item in _dataSource)
            {
                index++;
                if (item.Id == e.TaskId)
                    break;
            }

            foreach (var task in e.Tasks)
            {
                var listItem = new ListViewItem(task);
                _dataSource.Insert(index, listItem);
                index++;
            }
            Application.MainLoop.Invoke(_listView.SetNeedsDisplay);
        }

        private void OnTaskProgress(object sender, TaskProgressEventArgs e)
        {
            if (e.TaskId == _rootTask.Id)
                return;

            _dataSource
                .Find(x => x.Id == e.TaskId)
                ?.SetStatus(e.Status);

            Application.MainLoop.Invoke(_listView.SetNeedsDisplay);
        }

        private void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            _rootTask.Execute();
        }

        private void OnBackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _log.Close();
            ShowCompletionMessage();
        }

        private void ShowCompletionMessage()
        {
            var lastMinuteConsolidatedMessage = GetLastMinuteConsolidatedMessage();
            if (_log.HasErrors)
            {
                ProcessHelper.ShellExecute(_log.LogFilename);
                MessageBox.ErrorQuery(string.Empty, $"{Resources.BackupFailedPleaseReviewOpenedLog}\n{lastMinuteConsolidatedMessage}", Resources.Close);
            }
            else
            {
                MessageBox.Query(string.Empty, $"{Resources.BackupProcessCompletedSuccesfully}\n{lastMinuteConsolidatedMessage}", Resources.Close);
            }
            Application.MainLoop.Invoke(_listView.SetNeedsDisplay);
        }
        
        private string GetLastMinuteConsolidatedMessage()
        {
            var messages = string.Join(Environment.NewLine, _lastMinuteMessagesToUser.ToArray());
            if (!string.IsNullOrEmpty(messages))
                messages = messages + Environment.NewLine;
            return messages;
        }
    }
}
