namespace BUtil.ConsoleBackup.UI{
    using BUtil.ConsoleBackup.Localization;
    using BUtil.Core.BackupModels;
    using BUtil.Core.Options;
    using BUtil.Core.Storages;
    using System;
    using System.IO;
    using System.Linq;
    using Terminal.Gui;

    public partial class EditMediaSyncDialog
    {
        internal EditMediaSyncDialog(BackupTask task) 
        {
            InitializeComponent();

            if (task == null)
            {
                _sourceFolderTextField.Text = string.Empty;
                _destinationFolderTextField.Text = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    "Camera Roll");
                _transformFileNameTextField.Text = "{DATE:yyyy}\\{DATE:yyyy'-'MM', 'MMMM}\\{DATE:yyyy'-'MM'-'dd', 'dddd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}";
                Title = BUtil.ConsoleBackup.Localization.Resources.CreatePhotosVideosMovalTask;
            }
            else
            {
                _sourceFolderTextField.Text = task.Items[0].Target;
                _destinationFolderTextField.Text = ((FolderStorageSettings)task.Storages[0]).DestinationFolder;
                _transformFileNameTextField.Text = ((MediaSyncBackupModelOptions)task.Model).TransformFileName;
                Title = string.Format(BUtil.ConsoleBackup.Localization.Resources.PhotosVideosMovalTask, task.Name);
                _titleTextField.Text = task.Name;
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
            if (string.IsNullOrWhiteSpace(this._titleTextField.Text.ToString()) ||
                Path.GetInvalidFileNameChars().Any(x => this._titleTextField.Text.Contains(x)) ||
                Path.GetInvalidPathChars().Any(x => this._titleTextField.Text.Contains(x)))
            {
                MessageBox.ErrorQuery(string.Empty, BUtil.ConsoleBackup.Localization.Resources.TitleIsEmptyOrContainsNotSupportedPathCharacters, Resources.Close);
                return;
            }

            if (!Directory.Exists(this._sourceFolderTextField.Text.ToString()))
            {
                MessageBox.ErrorQuery(string.Empty, Resources.SourceFolderDoesNotExist, Resources.Close);
                return;
            }

            if (!Directory.Exists(this._destinationFolderTextField.Text.ToString()))
            {
                MessageBox.ErrorQuery(string.Empty, Resources.DestinationDirectoryDoesNotExist, Resources.Close);
                return;
            }

            if (string.IsNullOrWhiteSpace(this._transformFileNameTextField.Text.ToString()))
            {
                MessageBox.ErrorQuery(string.Empty, Resources.TransformFileNameIsEmpty, Resources.Close);
                return;
            }

            Canceled = false;
            Application.RequestStop();
        }

        public BackupTask BackupTask { get { return new BackupTask() { 
            Name = _titleTextField.Text.ToString(),
            Items = new System.Collections.Generic.List<SourceItem> { new SourceItem(_sourceFolderTextField.Text.ToString(), true) },
            Storages = new System.Collections.Generic.List<IStorageSettings> { new FolderStorageSettings { DestinationFolder = _destinationFolderTextField.Text.ToString() } },
            Model = new MediaSyncBackupModelOptions { TransformFileName = _transformFileNameTextField.Text.ToString() }
        }; } }
    }
}
