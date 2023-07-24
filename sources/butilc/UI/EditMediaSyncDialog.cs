namespace BUtil.ConsoleBackup.UI{
    using BUtil.Core.BackupModels;
    using BUtil.Core.Options;
    using BUtil.Core.Storages;
    using System;
    using System.IO;

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
                transformFileNameTextField.Text = "{DATE:dddd, dd MMMM yyyy}";
                Title = "Create";
            }
            else
            {
                mediaSourceTextField.Text = task.Items[0].Target;
                destinationTextField.Text = ((FolderStorageSettings)task.Storages[0]).DestinationFolder;
                transformFileNameTextField.Text = ((MediaSyncBackupModelOptions)task.Model).TransformFileName;
                Title = $"Edit {task.Name}";
            }
        }
    }
}
