using BUtil.Core.BackupModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

namespace BUtil.ConsoleBackup.UI{

    partial class MainView 
    {
        private readonly Controller _controller;
        private List<string> _taskNames;
        
        internal MainView(Controller controller) 
        {
            InitializeComponent();

            _taskNames = controller.BackupTaskStoreService.GetNames().ToList();
            this.itemsListView.SetSource(_taskNames);
            _controller = controller;
        }

        public void OnRunSelectedBackupTask()
        {
            if (this.itemsListView.SelectedItem < 0 || this.itemsListView.SelectedItem >= _taskNames.Count)
                return;

            var taskName = _taskNames[this.itemsListView.SelectedItem];

            var task = _controller.BackupTaskStoreService.Load(taskName);
            var dialog = new BackupDialog(task);
            Application.Run(dialog);
        }

        public void OnEditSelectedBackupTask()
        {
            if (this.itemsListView.SelectedItem < 0 || this.itemsListView.SelectedItem >= _taskNames.Count)
                return;

            var taskName = _taskNames[this.itemsListView.SelectedItem];
            var task = _controller.BackupTaskStoreService.Load(taskName);
            if (!(task.Model is ImportMediaBackupModelOptions))
            {
                Terminal.Gui.MessageBox.ErrorQuery(string.Empty, BUtil.ConsoleBackup.Localization.Resources.YouCannotEditThisTypeOfTaskInCLI);
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
        }

        public void OnDeleteSelectedBackupTask()
        {
            if (this.itemsListView.SelectedItem < 0 || this.itemsListView.SelectedItem >= _taskNames.Count)
                return;

            var taskName = _taskNames[this.itemsListView.SelectedItem];
            if (Terminal.Gui.MessageBox.Query(string.Empty, string.Format(Localization.Resources.PleaseConfirmDeletionOf0, taskName),
                BUtil.ConsoleBackup.Localization.Resources.Delete, BUtil.Core.Localization.Resources.Cancel) != 0)
                return;

            _controller.BackupTaskStoreService.Delete(taskName);
            _taskNames.RemoveAll(x => string.Compare(x, taskName, System.StringComparison.OrdinalIgnoreCase) == 0);
            this.itemsListView.SetNeedsDisplay();
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
        }

        private void OnListShortcutKeyDown(KeyEventEventArgs e)
        {
            if (e.KeyEvent.Key == Key.DeleteChar)
            {
                e.Handled = true;
                Application.MainLoop.Invoke(OnDeleteSelectedBackupTask);
            }
        }
    }
}
