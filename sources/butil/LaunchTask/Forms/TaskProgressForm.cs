using System;
using System.Drawing;
using System.Windows.Forms;
using BUtil.Core;
using BUtil.Core.Misc;
using BUtil.Core.Logs;
using BUtil.Core.Localization;
using BUtil.Core.Events;
using BUtil.Core.BackupModels;
using BUtil.Core.TasksTree.Core;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Configurator.BackupUiMaster.Forms
{
    internal sealed partial class TaskProgressForm : Form
    {
        private readonly TaskV2 _backupTask;
        private readonly ConcurrentQueue<Action> _listViewUpdates = new();
        private readonly List<ListViewItem> _items = new();
        private readonly List<string> _lastMinuteMessagesToUser = new List<string>();
        private HashSet<Guid> _ended = new HashSet<Guid>();

        public TaskProgressForm(TaskV2 backupTask)
        {
            InitializeComponent();

            _backupTask = backupTask;
            OnTasksListViewResize(this, new EventArgs());
            ApplyLocalization();
            NativeMethods.PreventSleep();
        }

        static Color GetResultColor(ProcessingStatus state)
        {
            return state switch
            {
                ProcessingStatus.FinishedSuccesfully => Color.LightGreen,
                ProcessingStatus.FinishedWithErrors => Color.LightCoral,
                ProcessingStatus.InProgress => Color.Yellow,
                ProcessingStatus.NotStarted => throw new InvalidOperationException(state.ToString()),
                _ => throw new NotImplementedException(state.ToString()),
            };
        }

        private void UpdateListViewItem(Guid taskId, ProcessingStatus status)
        {
            foreach (ListViewItem item in _items)
            {
                if ((Guid)item.Tag == taskId)
                {
                    item.SubItems[1].Text = LocalsHelper.ToString(status);
                    if (status != ProcessingStatus.NotStarted)
                    {
                        item.BackColor = GetResultColor(status);
                    }
                    break;
                }
            }
        }

        void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        void StartButtonClick(object _, EventArgs e)
        {
            startButton.Visible = false;
            cancelButton.Enabled = true;

            backupProgressUserControl.Start();
            _backgroundWorker.RunWorkerAsync();
        }

        void ApplyLocalization()
        {
            toolTip.SetToolTip(startButton, Resources.Task_Launch_Hint);
            closeButton.Text = Resources.Button_Close;

            taskNameColumnHeader.Text = Resources.Task_List;

            Text = $"{_backupTask.Name} - {Resources.ApplicationName_LaunchTask}";
            toolTip.SetToolTip(cancelButton, Resources.Button_Cancel);

            _powerTaskLinkLabel.Text = Resources.AfterTaskSelection_Field;
            _powerTaskComboBox.Items.Clear();
            _powerTaskComboBox.Items.AddRange(new[] { 
                Resources.AfterTaskSelection_ShutdownPc, 
                Resources.AfterTaskSelection_LogOff, 
                Resources.AfterTaskSelection_Reboot, 
                Resources.AfterTaskSelection_DoNothing });

            backupProgressUserControl.ApplyLocalization();
        }

        void LoadForm(object sender, EventArgs e)
        {
            _log = new FileLog(_backupTask.Name);
            _log.Open();
            _strategy = TaskModelStrategyFactory.Create(_log, _backupTask);
            _backupEvents = new TaskEvents();
            _backupEvents.OnTaskProgress += OnTaskProgress;
            _backupEvents.OnDuringExecutionTasksAdded += OnDuringExecutionTasksAdded;
            _backupEvents.OnMessage += OnAddLastMinuteMessageToUser;
            _rootTask = _strategy.GetTask(_backupEvents);
            _powerTaskComboBox.SelectedIndex = (int)PowerTask.None;

            var allTasks = _rootTask.GetChildren();
            foreach (var task in allTasks)
            {
                var listItem = new ListViewItem(task.Title, (int)task.TaskArea);
                listItem.SubItems.Add(LocalsHelper.ToString(ProcessingStatus.NotStarted));
                listItem.Tag = task.Id;
                _items.Add(listItem);
            }
            tasksListView.VirtualListSize = _items.Count;

            VerifyModel();
        }

        private void OnAddLastMinuteMessageToUser(object sender, MessageEventArgs e)
        {
            _lastMinuteMessagesToUser.Add(e.Message);
        }

        private void VerifyModel()
        {
            if (!TaskModelStrategyFactory.TryVerify(this._log, _backupTask.Model, out var error))
            {
                Messages.ShowErrorBox(error);
                Close();
            }
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
                var listItem = new ListViewItem(task.Title, (int)task.TaskArea);
                listItem.SubItems.Add(LocalsHelper.ToString(ProcessingStatus.NotStarted));
                listItem.Tag = task.Id;
                _items.Insert(index, listItem);
                index++;
            }
            tasksListView.VirtualListSize = _items.Count;
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

        private void OnTasksListViewResize(object sender, EventArgs e)
        {
            int newWidth = tasksListView.Width - processingStateInformationColumnHeader.Width - 35;
            taskNameColumnHeader.Width = newWidth < 35 ? 35 : newWidth;
        }

        private FileLog _log;
        private ITaskModelStrategy _strategy;
        private TaskEvents _backupEvents;
        private BuTask _rootTask;

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

        void CancelButtonClick(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        void ClosingForm(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
        void HelpButtonClick(object sender, EventArgs e)
        {
            SupportManager.DoSupport(SupportRequest.BackupWizard);
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

        private void OnShowHelp(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Messages.ShowInformationBox(Resources.AfterTaskSelection_Help);
        }
    }
}