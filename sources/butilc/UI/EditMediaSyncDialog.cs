namespace BUtil.ConsoleBackup.UI{
    using BUtil.Core.BackupModels;
    using BUtil.Core.Options;
    using BUtil.Core.Storages;
    using System;
    using System.IO;
    using Terminal.Gui;

    public partial class EditMediaSyncDialog
    {
        internal EditMediaSyncDialog(BackupTask task) {
            InitializeComponent();

            if (task == null)
            {
                mediaSourceTextField.Text = string.Empty;
                destinationTextField.Text = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    "Camera Roll");
                transformFileNameTextField.Text = "{INPUT NAME ONCE}\\{DATE:dddd, dd MMMM yyyy}";
                Title = "Create";
            }
            else
            {
                mediaSourceTextField.Text = task.Items[0].Target;
                destinationTextField.Text = ((FolderStorageSettings)task.Storages[0]).DestinationFolder;
                transformFileNameTextField.Text = ((MediaSyncBackupModelOptions)task.Model).TransformFileName;
                Title = $"Edit {task.Name}";
                titleTextField.Text = task.Name;
            }
            
        }

        public bool Canceled { get; private set; } = true;

        private void OnCancel()
        {
            Canceled = true;
            Application.RequestStop();
        }

        private void OnSave()
        {
            // validations:...

            //

            Canceled = false;
            Application.RequestStop();
        }

        public BackupTask BackupTask { get { return new BackupTask() { 
            Name = titleTextField.Text.ToString(),
            Items = new System.Collections.Generic.List<SourceItem> { new SourceItem(mediaSourceTextField.Text.ToString(), true) },
            Storages = new System.Collections.Generic.List<IStorageSettings> { new FolderStorageSettings { DestinationFolder = destinationTextField.Text.ToString() } },
            Model = new MediaSyncBackupModelOptions { TransformFileName = transformFileNameTextField.Text.ToString() }
        }; } }
    }
}
