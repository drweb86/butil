using BUtil.ConsoleBackup.Localization;
using BUtil.Core.BackupModels;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Storages;
using System;
using System.IO;
using System.Linq;
using Terminal.Gui;

namespace BUtil.ConsoleBackup.UI
{
    public partial class EditMediaSyncDialog
    {
        private IStorageSettings _from;

        internal EditMediaSyncDialog(BackupTask task) 
        {
            InitializeComponent();

            if (task == null)
            {
                _destinationFolderTextField.Text = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    "Camera Roll");
                _transformFileNameTextField.Text = "{DATE:yyyy}\\{DATE:yyyy'-'MM', 'MMMM}\\{DATE:yyyy'-'MM'-'dd', 'dddd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}";
                Title = Resources.CreatePhotosVideosMovalTask;
            }
            else
            {
                var options = (MediaSyncBackupModelOptions)task.Model;
                _from = options.From;
                _destinationFolderTextField.Text = ((FolderStorageSettings)options.To).DestinationFolder;
                _transformFileNameTextField.Text = options.TransformFileName;
                Title = string.Format(Resources.PhotosVideosMovalTask, task.Name);
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

            if (!BackupModelStrategyFactory.TryVerify(new StubLog(), BackupTask.Model, out var error))
            {
                MessageBox.ErrorQuery(string.Empty, error, Resources.Close);
                return;
            }

            Canceled = false;
            Application.RequestStop();
        }

        private void OnSpecifySource()
        {
            var dialog = new SpecifySourceDialog(_from);
            Application.Run(dialog);

            if (dialog.Canceled)
                return;
            _from = dialog.Source;
        }

        public BackupTask BackupTask 
        {
            get 
            {
                return new BackupTask
                {
                    Name = _titleTextField.Text.ToString(),
                    Model = new MediaSyncBackupModelOptions
                    {
                        TransformFileName = _transformFileNameTextField.Text.ToString(),
                        From = _from,
                        To = new FolderStorageSettings { DestinationFolder = _destinationFolderTextField.Text.ToString() }
                    }
                }; 
            } 
        }
    }
}
