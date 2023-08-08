namespace BUtil.ConsoleBackup.UI{
    using BUtil.Core.BackupModels;
    using BUtil.Core.FileSystem;
    using BUtil.Core.Options;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Terminal.Gui;

    public partial class MainView {
        private readonly Controller _controller;
        private List<string> _taskNames;
        
        internal MainView(Controller controller) {
            InitializeComponent();

            _taskNames = controller.BackupTaskStoreService.GetNames().ToList();
            this.itemsListView.SetSource(_taskNames);
            _controller = controller;
        }

        public void OnRunSelectedBackupTask()
        {
            if (this.itemsListView.SelectedItem == -1)
                return;

            var taskName = _taskNames[this.itemsListView.SelectedItem];

            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = Files.ConsoleBackupTool,
                Arguments = $"\"Task={taskName}\"",
            };
            Process.Start(processStartInfo);
        }

        public void OnEditSelectedBackupTask()
        {
            if (this.itemsListView.SelectedItem == -1)
                return;

            var taskName = _taskNames[this.itemsListView.SelectedItem];
            var task = _controller.BackupTaskStoreService.Load(taskName);
            if (!(task.Model is MediaSyncBackupModelOptions))
            {
                Terminal.Gui.MessageBox.ErrorQuery(
                    "Not supported!", 
                    "In Backup CLI UI you can only edit media sync tasks.");
                return;
            }
            
            var dialog = new EditMediaSyncDialog(task);
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
            if (this.itemsListView.SelectedItem == -1)
                return;

            var taskName = _taskNames[this.itemsListView.SelectedItem];
            if (Terminal.Gui.MessageBox.Query("Confirm deletion", $"Please confirm deletion of {taskName}",
                "Delete", "Cancel") != 0)
                return;

            _controller.BackupTaskStoreService.Delete(taskName);
            _taskNames.RemoveAll(x => string.Compare(x, taskName, System.StringComparison.OrdinalIgnoreCase) == 0);
            this.itemsListView.SetNeedsDisplay();
        }

        public void OnCreateBackupTask()
        {
            var dialog = new EditMediaSyncDialog(null);
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
