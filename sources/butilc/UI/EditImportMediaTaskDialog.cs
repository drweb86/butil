using BUtil.ConsoleBackup.Localization;
using BUtil.Core.BackupModels;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
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
        private IStorageSettingsV2 _from;

        internal EditImportMediaTaskDialog(BackupTaskV2 task) 
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
                var options = (ImportMediaBackupModelOptionsV2)task.Model;
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
            var dialog = new EditStorageSettingsDialog(_from, BUtil.ConsoleBackup.Localization.Resources.ImportFilesFrom);
            Application.Run(dialog);

            if (dialog.Canceled)
                return;
            _from = dialog.StorageSettings;
        }

        public BackupTaskV2 BackupTask 
        {
            get 
            {
                return new BackupTaskV2
                {
                    Name = _titleTextField.Text.ToString(),
                    Model = new ImportMediaBackupModelOptionsV2
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
