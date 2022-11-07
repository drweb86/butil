using System;
using System.Collections.Generic;
using System.Windows.Forms;

using BUtil.Core.Options;
using BUtil.Configurator.Configurator.Forms;
using BUtil.Configurator.Localization;
using BUtil.Configurator.AddBackupTaskWizard.View;

namespace BUtil.Configurator.Configurator.Controls
{
    internal delegate bool RequestToSaveOptions();

    public partial class BackupTasksUserControl : Core.PL.BackUserControl
    {
        public BackupTasksUserControl(ConfiguratorController controller)
        {
            InitializeComponent();

            _controller = controller;

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

            _addToolStripMenuItem.Text =Resources.Add;
            _removeToolStripMenuItem.Text = Resources.Remove;
            _editToolStripMenuItem.Text=Resources.Modify;
            _executeToolStripMenuItem.Text = Resources.Run;

            _nameColumn.Text = Resources.Name;
        }

        public override void SetOptionsToUi(object settings)
        {
            _programOptions = (ProgramOptions)settings;
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
            _tasksListView.BeginUpdate();
            _tasksListView.Items.Clear();
            foreach (var taskName in taskNames)
            {
                _tasksListView.Items.Add(new ListViewItem(taskName, 0));
            }
            _tasksListView.EndUpdate();
        }

        void AddTaskRequest(object sender, EventArgs e)
        {
            using var form = new CreateBackupTaskWizardForm(_programOptions);
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

            var backupTaskSchedulerService = new BackupTaskSchedulerService();
            var scheduleInfo = backupTaskSchedulerService.GetSchedule(taskName);
            using var form = new EditBackupTaskForm(_programOptions, task, scheduleInfo);
            if (form.ShowDialog() == DialogResult.OK)
            {
                backupTaskStoreService.Delete(taskName);
                backupTaskStoreService.Save(task);

                backupTaskSchedulerService.Schedule(task.Name, scheduleInfo);

                ReloadTasks();

                if (form.ExecuteTask)
                {
                    ExecuteRequest(this, e);
                }
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
                if (MessageBox.Show(string.Format(Resources.WouldYouLileToRemoveTheBackupTask0, selectedTask.Text), BUtil.Core.Localization.Resources.QuestionButil, MessageBoxButtons.OKCancel) == DialogResult.OK)
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

            _controller.OpenBackupUiMaster(selectedTasks[0], false);
        }

        void RefreshTaskControls(object sender, EventArgs e)
        {
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
            _nameColumn.Width = _tasksListView.Width > DisplacementToBorder ? _tasksListView.Width - DisplacementToBorder : DisplacementToBorder;
        }

        #endregion

        #region Fields

        ProgramOptions _programOptions;
        readonly ConfiguratorController _controller;

        #endregion

        #region Constants

        const int DisplacementToBorder = 40;

        #endregion
    }
}
