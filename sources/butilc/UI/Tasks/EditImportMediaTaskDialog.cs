using BUtil.Core.Localization;
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

        internal EditImportMediaTaskDialog(TaskV2 task) 
        {
            InitializeComponent();

            if (task == null)
            {
                _destinationFolderTextField.Text = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    "Camera Roll");
                _transformFileNameTextField.Text = "{DATE:yyyy}\\{DATE:yyyy'-'MM', 'MMMM}\\{DATE:yyyy'-'MM'-'dd', 'dddd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}";
                Title = Resources.ImportMediaTask_Create;
                _skipAlreadyImportedFilesCheckBox.Checked = true;
            }
            else
            {
                var options = (ImportMediaTaskModelOptionsV2)task.Model;
                _from = options.From;
                _destinationFolderTextField.Text = options.DestinationFolder;
                _transformFileNameTextField.Text = options.TransformFileName;
                Title = string.Format(Resources.ImportMediaTask_Edit_Title, task.Name);
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
                MessageBox.ErrorQuery(string.Empty, BUtil.Core.Localization.Resources.Name_Field_Validation, Resources.Button_Close);
                return;
            }

            if (!TaskModelStrategyFactory.TryVerify(new StubLog(), BackupTask.Model, out var error))
            {
                MessageBox.ErrorQuery(string.Empty, error, Resources.Button_Close);
                return;
            }

            Canceled = false;
            Application.RequestStop();
        }

        private void OnSpecifySource()
        {
            var dialog = new EditStorageSettingsDialog(_from, BUtil.Core.Localization.Resources.ImportMediaTask_ImportDataSource);
            Application.Run(dialog);

            if (dialog.Canceled)
                return;
            _from = dialog.StorageSettings;
        }

        public TaskV2 BackupTask 
        {
            get 
            {
                return new TaskV2
                {
                    Name = _titleTextField.Text.ToString(),
                    Model = new ImportMediaTaskModelOptionsV2
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
                _transformFileNameLabel.Text = BUtil.Core.Localization.Resources.ImportMediaTask_Field_TransformFileName_Validation_Invalid;
            }
        }
    }
}
