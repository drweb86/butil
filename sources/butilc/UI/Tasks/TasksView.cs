using BUtil.Core.Localization;
using BUtil.Core.ConfigurationFileModels.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;
using BUtil.Core.Misc;
using BUtil.Core.Logs;
using BUtil.ConsoleBackup.UI.Tasks;
using System.Data.Common;

namespace BUtil.ConsoleBackup.UI
{
    partial class TasksView 
    {
        private readonly Controller _controller;
        private List<LogFileInfo> _logs;
        private readonly TasksViewDataSource _dataSource = new TasksViewDataSource();
        
        internal TasksView(Controller controller) 
        {
            InitializeComponent();

            _logs = new LogService().GetRecentLogs().ToList();
            var taskNames = controller.BackupTaskStoreService.GetNames();
            _dataSource.AddRange(taskNames.Select(x => new TasksViewItem(x, _logs.FirstOrDefault(y => y.TaskName == x))));
            this.itemsListView.SetSource(_dataSource);

            _controller = controller;
            UpdateSelectedItem();
        }

        private TasksViewItem SelectedItem 
        {
            get
            {
                if (this.itemsListView.SelectedItem < 0 || this.itemsListView.SelectedItem >= _dataSource.Count)
                    return null;

                return _dataSource[this.itemsListView.SelectedItem];
            } 
        }

        public void OnRunSelectedBackupTask()
        {
            if (SelectedItem == null)
                return;

            var task = _controller.BackupTaskStoreService.Load(SelectedItem.TaskName);
            if (task == null)
            {
                Terminal.Gui.MessageBox.ErrorQuery(string.Empty, BUtil.Core.Localization.Resources.Task_Validation_NotSupported, Resources.Button_Close);
                return;
            }
            var dialog = new TaskProgressDialog(task);
            Application.Run(dialog);
        }

        public void OnEditSelectedBackupTask()
        {
            if (SelectedItem == null)
                return;

            var task = _controller.BackupTaskStoreService.Load(SelectedItem.TaskName);
            if (task == null)
            {
                Terminal.Gui.MessageBox.ErrorQuery(string.Empty, BUtil.Core.Localization.Resources.Task_Validation_NotSupported, Resources.Button_Close);
                return;
            }
            if (!(task.Model is ImportMediaTaskModelOptionsV2))
            {
                Terminal.Gui.MessageBox.ErrorQuery(string.Empty, BUtil.Core.Localization.Resources.Task_Edit_Validation_NoCLI, Resources.Button_Close);
                return;
            }
            
            var dialog = new EditImportMediaTaskDialog(task);
            Application.Run(dialog);

            if (dialog.Canceled)
                return;

            _controller.BackupTaskStoreService.Delete(SelectedItem.TaskName);
            var updatedTask = dialog.BackupTask;
            
            SelectedItem.TaskName = updatedTask.Name;
            _controller.BackupTaskStoreService.Save(updatedTask);
            SortDataSource();

            this.itemsListView.SetNeedsDisplay();
            UpdateSelectedItem();
        }

        private void SortDataSource()
        {
            _dataSource.Sort((x, y) => x.TaskName.CompareTo(y.TaskName));
        }

        public void OnDeleteSelectedBackupTask()
        {
            if (SelectedItem == null)
                return;

            var taskName = SelectedItem.TaskName;
            if (Terminal.Gui.MessageBox.Query(string.Empty, string.Format(BUtil.Core.Localization.Resources.Task_Delete_Confirm, taskName),
                BUtil.Core.Localization.Resources.Task_Delete, BUtil.Core.Localization.Resources.Button_Cancel) != 0)
                return;

            _controller.BackupTaskStoreService.Delete(taskName);
            _dataSource.Remove(SelectedItem);
            this.itemsListView.SetNeedsDisplay();
            UpdateSelectedItem();
        }

        private void OnRestoreSelectedTask()
        {
            if (SelectedItem == null)
                return;

            SupportManager.OpenRestorationApp(SelectedItem.TaskName);
        }

        public void OnOpenLogs()
        {
            SupportManager.OpenLogs();
        }

        private void OnOpenHomePage()
        {
            SupportManager.OpenHomePage();
        }

        private void OnOpenRestorationApp()
        {
            SupportManager.OpenRestorationApp();
        }

        public void OnCreateImportMediaTask()
        {
            var dialog = new EditImportMediaTaskDialog(null);
            Application.Run(dialog);

            if (dialog.Canceled)
                return;

            var task = dialog.BackupTask;
            _controller.BackupTaskStoreService.Save(task);

            _dataSource.RemoveAll(x => string.Compare(x.TaskName, task.Name, System.StringComparison.OrdinalIgnoreCase) == 0);
            _dataSource.Add(new TasksViewItem(task.Name, null));
            SortDataSource();
            this.itemsListView.SetNeedsDisplay();
            UpdateSelectedItem();
        }

        private void OnListShortcutKeyDown(KeyEventEventArgs e)
        {
            if (e.KeyEvent.Key == Key.DeleteChar)
            {
                e.Handled = true;
                Application.MainLoop.Invoke(OnDeleteSelectedBackupTask);
            }
        }
        private void OnSelectedItemChanged(ListViewItemEventArgs args)
        {
            UpdateSelectedItem();
        }

        private void UpdateSelectedItem()
        {
            if (SelectedItem == null)
                return;

            var taskInfo = _dataSource[this.itemsListView.SelectedItem];
            selectedItemInfoFrameView.Title = taskInfo.TaskName;
        }
    }
}
