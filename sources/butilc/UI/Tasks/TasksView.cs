using BUtil.Core.Localization;
using BUtil.Core.ConfigurationFileModels.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

namespace BUtil.ConsoleBackup.UI
{

    partial class TasksView 
    {
        private readonly Controller _controller;
        private List<string> _taskNames;
        
        internal TasksView(Controller controller) 
        {
            InitializeComponent();

            _taskNames = controller.BackupTaskStoreService.GetNames().ToList();
            this.itemsListView.SetSource(_taskNames);
            _controller = controller;
            UpdateSelectedItem();
        }

        public void OnRunSelectedBackupTask()
        {
            if (this.itemsListView.SelectedItem < 0 || this.itemsListView.SelectedItem >= _taskNames.Count)
                return;

            var taskName = _taskNames[this.itemsListView.SelectedItem];

            var task = _controller.BackupTaskStoreService.Load(taskName);
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
            if (this.itemsListView.SelectedItem < 0 || this.itemsListView.SelectedItem >= _taskNames.Count)
                return;

            var taskName = _taskNames[this.itemsListView.SelectedItem];
            var task = _controller.BackupTaskStoreService.Load(taskName);
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

            _controller.BackupTaskStoreService.Delete(task.Name);
            _taskNames.RemoveAll(x => string.Compare(x, task.Name, System.StringComparison.OrdinalIgnoreCase) == 0);

            var updatedTask = dialog.BackupTask;
            _controller.BackupTaskStoreService.Save(updatedTask);
            _taskNames.Add(updatedTask.Name);
            _taskNames.Sort(StringComparer.OrdinalIgnoreCase);
            this.itemsListView.SetNeedsDisplay();
            UpdateSelectedItem();
        }

        public void OnDeleteSelectedBackupTask()
        {
            if (this.itemsListView.SelectedItem < 0 || this.itemsListView.SelectedItem >= _taskNames.Count)
                return;

            var taskName = _taskNames[this.itemsListView.SelectedItem];
            if (Terminal.Gui.MessageBox.Query(string.Empty, string.Format(BUtil.Core.Localization.Resources.Task_Delete_Confirm, taskName),
                BUtil.Core.Localization.Resources.Task_Delete, BUtil.Core.Localization.Resources.Button_Cancel) != 0)
                return;

            _controller.BackupTaskStoreService.Delete(taskName);
            _taskNames.RemoveAll(x => string.Compare(x, taskName, System.StringComparison.OrdinalIgnoreCase) == 0);
            this.itemsListView.SetNeedsDisplay();
            UpdateSelectedItem();
        }

        public void OnCreateImportMediaTask()
        {
            var dialog = new EditImportMediaTaskDialog(null);
            Application.Run(dialog);

            if (dialog.Canceled)
                return;

            var task = dialog.BackupTask;
            _controller.BackupTaskStoreService.Save(task);

            _taskNames.RemoveAll(x => string.Compare(x, task.Name, System.StringComparison.OrdinalIgnoreCase) == 0);
            _taskNames.Add(task.Name);
            _taskNames.Sort(StringComparer.OrdinalIgnoreCase);
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
            if (this.itemsListView.SelectedItem < 0 || this.itemsListView.SelectedItem >= _taskNames.Count)
                return;

            var taskName = _taskNames[this.itemsListView.SelectedItem];
            selectedItemInfoFrameView.Title = taskName;
        }
    }
}
