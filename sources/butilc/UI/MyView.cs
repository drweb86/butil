namespace BUtil.ConsoleBackup.UI{
    using BUtil.Core.BackupModels;
    using BUtil.Core.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
        
    public partial class MyView {
        private readonly Controller _controller;
        private List<string> _taskNames;
        
        internal MyView(Controller controller) {
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
            var task = _controller.BackupTaskStoreService.Load(taskName);
            if (!(task.Model is MediaSyncBackupModelOptions))
            {
                Terminal.Gui.MessageBox.ErrorQuery("Not supported!", "In Backup CLI UI you can only launch media sync tasks.");
                return;
            }
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
            var task = new BackupTask
            {
                Name = "New Test!",
                Model = new MediaSyncBackupModelOptions(),
            };
            _controller.BackupTaskStoreService.Save(task);

            _taskNames.RemoveAll(x => string.Compare(x, task.Name, System.StringComparison.OrdinalIgnoreCase) == 0);
            _taskNames.Add(task.Name);
            _taskNames.Sort(StringComparer.OrdinalIgnoreCase);
        }
    }
}
