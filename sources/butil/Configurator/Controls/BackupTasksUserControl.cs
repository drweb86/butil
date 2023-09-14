using System;
using System.Collections.Generic;
using System.Windows.Forms;

using BUtil.Core.Options;
using BUtil.Configurator.Configurator.Forms;
using BUtil.Configurator.Localization;
using BUtil.Configurator.AddBackupTaskWizard.View;
using System.Linq;
using BUtil.Core.Logs;
using System.Diagnostics;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Configurator.Configurator.Controls
{
    internal delegate bool RequestToSaveOptions();

    public partial class BackupTasksUserControl : Core.PL.BackUserControl
    {
        public BackupTasksUserControl()
        {
            InitializeComponent();

            _lastBackupAt.Text = BUtil.Configurator.Localization.Resources.LastBackup;
            OnTasksListViewResize(this, null);
        }

        internal RequestToSaveOptions OnRequestToSaveOptions;

        #region Public Methods

        public override void ApplyLocalization()
        {
            SetHintForControl(_addButton, Resources.AddsTheNewBackupTask);
            SetHintForControl(_removeButton, Resources.RemovesTheSelectedBackupTask);
            SetHintForControl(_editButton, Resources.EditsTheSelectedBackupTask);
            SetHintForControl(_executeButton, Resources.RunsBackup);
            SetHintForControl(_recoverButton, Resources.Recover);

            _addToolStripMenuItem.Text = Resources.Add;
            _removeToolStripMenuItem.Text = Resources.Remove;
            _editToolStripMenuItem.Text = Resources.Modify;
            _executeToolStripMenuItem.Text = Resources.Run;
            _recoverToolStripMenuItem.Text = Resources.Recover;

            _nameColumn.Text = Resources.Name;
        }

        public override void SetOptionsToUi(object settings)
        {
            ReloadTasks();
            RefreshTaskControls(this, null);
        }

        public override void GetOptionsFromUi()
        {
        }

        #endregion

        #region Private Methods

        private void ReloadTasks()
        {
            var backupTaskStoreService = new BackupTaskStoreService();
            var taskNames = backupTaskStoreService.GetNames();

            var logsService = new LogService();
            var lastLogs = logsService.GetRecentLogs();

            _tasksListView.BeginUpdate();
            _tasksListView.Items.Clear();
            foreach (var taskName in taskNames)
            {
                var lastLogFile = lastLogs.FirstOrDefault(x => x.TaskName == taskName);
                string status = "-";
                if (lastLogFile != null)
                {
                    var postfix = lastLogFile.IsSuccess.HasValue ?
                        (lastLogFile.IsSuccess.Value ? BUtil.Core.Localization.Resources.Successful : BUtil.Core.Localization.Resources.Errors)
                        : BUtil.Core.Localization.Resources.Unknown;
                    status = $"{lastLogFile.CreatedAt} ({postfix})";
                }
                _tasksListView.Items.Add(new ListViewItem(new[] { taskName, status }, 0));
            }
            _tasksListView.EndUpdate();
        }

        void AddTaskRequest(object sender, EventArgs e)
        {
            using var form = new CreateBackupTaskWizardForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var backupTaskStoreService = new BackupTaskStoreService();
                backupTaskStoreService.Save(form.BackupTask);

                var backupTaskSchedulerService = new BackupTaskSchedulerService();
                backupTaskSchedulerService.Schedule(form.BackupTask.Name, form.ScheduleInfo);

                ReloadTasks();
                RefreshTaskControls(this, e);
            }
        }
        private void OnEditBackupTask(object sender, EventArgs e)
        {
            if (_tasksListView.SelectedItems.Count == 0)
            {
                return;
            }

            var taskName = _tasksListView.SelectedItems[0].Text;
            var backupTaskStoreService = new BackupTaskStoreService();
            var task = backupTaskStoreService.Load(taskName);
            if (!(task.Model is IncrementalBackupModelOptionsV2))
            {
                Messages.ShowErrorBox("To change this task launch console CLI");
                return;
            }

            var backupTaskSchedulerService = new BackupTaskSchedulerService();
            var scheduleInfo = backupTaskSchedulerService.GetSchedule(taskName);
            using var form = new EditBackupTaskForm(task, scheduleInfo);
            if (form.ShowDialog() == DialogResult.OK)
            {
                backupTaskStoreService.Delete(taskName);
                backupTaskStoreService.Save(task);

                backupTaskSchedulerService.Schedule(task.Name, scheduleInfo);

                ReloadTasks();
            }
        }

        void RemoveTaskRequest(object sender, EventArgs e)
        {
            var selectedTasks = new List<ListViewItem>();
            foreach (ListViewItem taskToRemove in _tasksListView.SelectedItems)
            {
                selectedTasks.Add(taskToRemove);
            }

            foreach (var selectedTask in selectedTasks)
            {
                if (Messages.ShowYesNoDialog(string.Format(Resources.WouldYouLileToRemoveTheBackupTask0, selectedTask.Text)))
                {
                    var backupTasksService = new BackupTaskStoreService();
                    backupTasksService.Delete(selectedTask.Text);
                    var backupTaskSchedulerService = new BackupTaskSchedulerService();
                    backupTaskSchedulerService.Unschedule(selectedTask.Text);
                }
            }
            ReloadTasks();
            RefreshTaskControls(this, e);
        }

        void ExecuteRequest(object sender, EventArgs e)
        {
            if (!OnRequestToSaveOptions())
            {
                return;
            }

            var selectedTasks = new List<string>();
            foreach (ListViewItem taskToExecute in _tasksListView.SelectedItems)
            {
                selectedTasks.Add(taskToExecute.Text);
            }

            if (selectedTasks.Any())
                ConfiguratorController.LaunchBackupUIToolInSeparateProcess(selectedTasks[0]);
        }

        void RefreshTaskControls(object sender, EventArgs e)
        {
            _recoverButton.Enabled =
                _recoverToolStripMenuItem.Enabled =
                _removeButton.Enabled =
                _removeToolStripMenuItem.Enabled =
                _editButton.Enabled =
                _editToolStripMenuItem.Enabled =
                _executeButton.Enabled =
                _executeToolStripMenuItem.Enabled =
                    _tasksListView.SelectedItems.Count > 0;

            _executeToolStripMenuItem.Enabled =
                _executeButton.Enabled = _executeButton.Enabled && !Program.PackageIsBroken;
        }

        private void OnTasksListViewResize(object sender, EventArgs e)
        {
            var total = Math.Max(_tasksListView.Width - _displacementToBorder, _displacementToBorder);
            _nameColumn.Width = (int)(0.7 * total);
            _lastBackupAt.Width = (int)(0.3 * total);
        }

        #endregion

        private const int _displacementToBorder = 40;

        private void OnRecover(object sender, EventArgs e)
        {
            var selectedTasks = new List<string>();
            foreach (ListViewItem taskToExecute in _tasksListView.SelectedItems)
            {
                selectedTasks.Add(taskToExecute.Text);
            }

            Process.Start(Application.ExecutablePath, $"{Arguments.RunRestorationMaster} \"{Arguments.RunTask}={selectedTasks.First()}\"");
        }
    }
}
