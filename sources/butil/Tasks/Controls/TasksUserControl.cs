
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using BUtil.Core.Options;
using BUtil.Configurator.Configurator.Forms;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Misc;
using BUtil.Core;

namespace BUtil.Configurator.Configurator.Controls
{
    public partial class TasksUserControl : Core.PL.BackUserControl
    {
        private const int _displacementToBorder = 40;

        public TasksUserControl()
        {
            InitializeComponent();
            SetLastExecutionColumnWidth();
        }

        private void SetLastExecutionColumnWidth()
        {
            using var graphics = Graphics.FromHwnd(IntPtr.Zero);
            graphics.PageUnit = GraphicsUnit.Pixel;
            _lastExecutionStateColumn.Width = (int)graphics.MeasureString($"❌❌❌❌❌❌{DateTime.Now}", Font).Width;
        }

        public override void ApplyLocalization()
        {
            SetHintForControl(_addButton, Resources.Task_Create_Hint);

            _addToolStripMenuItem.Text = Resources.Task_Create;
            _removeToolStripMenuItem.Text = Resources.Task_Delete;
            _editToolStripMenuItem.Text = Resources.Task_Edit;
            _executeToolStripMenuItem.Text = Resources.Task_Launch;
            _recoverToolStripMenuItem.Text = Resources.Task_Restore;

            _createImportMultimediaTaskToolStripMenuItem.Text =
                _createImportMultimediaTaskToolStripMenuItem2.Text = Resources.ImportMediaTask_Create;
            _createIncrementalBackupTaskToolStripMenuItem.Text =
                _createIncrementalBackupTaskToolStripMenuItem2.Text = BUtil.Core.Localization.Resources.IncrementalBackupTask_Create;

            _nameColumn.Text = Resources.Name_Title;
        }

        public override void SetOptionsToUi(object settings)
        {
        }

        void OnCreateIncrementalBackupTask(object sender, EventArgs e)
        {
            var task = new TaskV2
            {
                Name = Resources.Task_Field_Name_NewDefaultValue,
                Model = new IncrementalBackupModelOptionsV2()
            };
            var scheduleInfo = new ScheduleInfo();
            using var form = new EditIncrementalBackupTaskForm(task, scheduleInfo, Tasks.TaskEditorPageEnum.SourceItems);
            if (form.ShowDialog() == DialogResult.OK)
            {
                var backupTaskStoreService = new TaskV2StoreService();
                backupTaskStoreService.Save(task);

                var schedulerService = PlatformSpecificExperience.Instance.GetTaskSchedulerService();
                schedulerService?.Schedule(task.Name, scheduleInfo);
            }
        }
        private void OnEditTask(object sender, EventArgs e)
        {
            if (_tasksListView.SelectedItems.Count == 0)
            {
                return;
            }

            var taskName = _tasksListView.SelectedItems[0].Text;
            var storeService = new TaskV2StoreService();

            var task = storeService.Load(taskName);
            if (task == null)
            {
                Messages.ShowErrorBox(Resources.Task_Validation_NotSupported);
                return;
            }

            if (task.Model is ImportMediaTaskModelOptionsV2)
            {
                using var form = new EditImportMediaTaskForm(task, Tasks.TaskEditorPageEnum.SourceItems, false);
                if (form.ShowDialog() != DialogResult.OK)
                    return;

            }
            else if (task.Model is IncrementalBackupModelOptionsV2)
            {
                var schedulerService = PlatformSpecificExperience.Instance.GetTaskSchedulerService();
                var schedule = schedulerService?.GetSchedule(taskName) ?? new ScheduleInfo();

                using var form = new EditIncrementalBackupTaskForm(task, schedule, Tasks.TaskEditorPageEnum.SourceItems);
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                schedulerService.Schedule(task.Name, schedule);
            }

            storeService.Delete(taskName);
            storeService.Save(task);
        }

        void ExecuteRequest(object sender, EventArgs e)
        {
            var selectedTasks = new List<string>();
            foreach (ListViewItem taskToExecute in _tasksListView.SelectedItems)
            {
                selectedTasks.Add(taskToExecute.Text);
            }

            if (selectedTasks.Any())
                SupportManager.LaunchTask(selectedTasks.First());
        }

        void RefreshTaskControls(object sender, EventArgs e)
        {
                _recoverToolStripMenuItem.Enabled =
                _removeToolStripMenuItem.Enabled =
                _editToolStripMenuItem.Enabled =
                _executeToolStripMenuItem.Enabled =
                    _tasksListView.SelectedItems.Count > 0;

        }

        private void OnCreateImportMultimediaTask(object sender, EventArgs e)
        {
            var task = new TaskV2
            {
                Name = Resources.Task_Field_Name_NewDefaultValue,
                Model = new ImportMediaTaskModelOptionsV2(),
            };

            using var form = new EditImportMediaTaskForm(task, Tasks.TaskEditorPageEnum.Storages, true);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            new TaskV2StoreService()
                .Save(task);
        }

        private void OnAddButtonOpenMenu(object sender, EventArgs e)
        {
            var btnSender = (Control)sender;
            Point ptLowerLeft = new(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            _createTaskContextMenuStrip.Show(ptLowerLeft);
        }
    }
}
