using BUtil.ConsoleBackup.Localization;
using BUtil.Core.BackupModels;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.MediaSyncBackupModel;
using NStack;
using System;
using System.IO;
using System.Linq;
using Terminal.Gui;

namespace BUtil.ConsoleBackup.UI
{
    public partial class EditImportMediaTaskDialog
    {
        private IStorageSettings _from;

        internal EditImportMediaTaskDialog(BackupTask task) 
        {
            InitializeComponent();

            if (task == null)
            {
                _destinationFolderTextField.Text = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    "Camera Roll");
                _transformFileNameTextField.Text = "{DATE:yyyy}\\{DATE:yyyy'-'MM', 'MMMM}\\{DATE:yyyy'-'MM'-'dd', 'dddd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}";
                Title = Resources.CreateImportMediaTask;
                _skipAlreadyImportedFilesCheckBox.Checked = true;
            }
            else
            {
                var options = (ImportMediaBackupModelOptions)task.Model;
                _from = options.From;
                _destinationFolderTextField.Text = options.DestinationFolder;
                _transformFileNameTextField.Text = options.TransformFileName;
                Title = string.Format(Resources.EditImportMediaTask, task.Name);
                _titleTextField.Text = task.Name;
                _skipAlreadyImportedFilesCheckBox.Checked = options.SkipAlreadyImportedFiles;
            }
            OnTransformFileNameTextChanged(_transformFileNameTextField.Text);
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
            var dialog = new EditStorageSettingsDialog(_from);
            Application.Run(dialog);

            if (dialog.Canceled)
                return;
            _from = dialog.StorageSettings;
        }

        public BackupTask BackupTask 
        {
            get 
            {
                return new BackupTask
                {
                    Name = _titleTextField.Text.ToString(),
                    Model = new ImportMediaBackupModelOptions
                    {
                        TransformFileName = _transformFileNameTextField.Text.ToString(),
                        From = _from,
                        DestinationFolder = _destinationFolderTextField.Text.ToString(),
                        SkipAlreadyImportedFiles = _skipAlreadyImportedFilesCheckBox.Checked,
                    }
                }; 
            } 
        }

        private void OnTransformFileNameTextChanged(ustring ustring)
        {
            try
            {
                _transformFileNameLabel.Text = DateTokenReplacer.ParseString(ustring.ToString(), DateTime.Now); ;
            }
            catch 
            {
                _transformFileNameLabel.Text = BUtil.Core.Localization.Resources.InvalidTransformFileNameString;
            }
        }
    }
}
