using BUtil.Core.BackupModels;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.MediaSyncBackupModel;
using System;
using System.IO;

namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    public partial class ImportMediaTaskWhereUserControl : Core.PL.BackUserControl
    {
        public ImportMediaTaskWhereUserControl()
        {
            InitializeComponent();

            _outputFolderLabel.Text = Resources.ImportMediaTask_Field_OutputFolder;
            _transformFileNameLabel.Text = Resources.ImportMediaTask_Field_TransformFileName;
            _helpTokensLabel.Text = Resources.ImportMediaTask_Field_TransformFileName_Help;
            _skipPreviouslyImportedFilesCheckbox.Text = Resources.ImportMediaTask_SkipAlreadyImportedFiles;
            _folderBrowseButton.Text = Resources.Field_Folder_Browse;
            OnChangeTransormFileName(this, new EventArgs());
        }

        public string TransformFileName
        {
            get => _transformFileTemplateTextBox.Text;
            set
            {
                _transformFileTemplateTextBox.Text = value;
                OnChangeTransormFileName(this, new EventArgs());
            }
        }
        public bool SkipAlreadyImportedFiles
        {
            get => _skipPreviouslyImportedFilesCheckbox.Checked;
            set => _skipPreviouslyImportedFilesCheckbox.Checked = value;
        }

        public string DestinationFolder
        {
            get => _outputFolderTextBox.Text;
            set
            {
                _outputFolderTextBox.Text = value;
                OnChangeTransormFileName(this, new EventArgs());
            }
        }

        public override bool ValidateUi()
        {
            if (!TaskModelStrategyFactory.TryVerify(new StubLog(), new ImportMediaTaskModelOptionsV2
            {
                DestinationFolder = DestinationFolder,
                From = new FolderStorageSettingsV2 { DestinationFolder = DestinationFolder }, // stub.
                SkipAlreadyImportedFiles = SkipAlreadyImportedFiles,
                TransformFileName = TransformFileName,
            }, out string error))
            {
                Messages.ShowErrorBox(error);
                return false;
            }

            return true;
        }

        private void OnFolderBrowseButtonClick(object sender, System.EventArgs e)
        {
            if (_folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                DestinationFolder = _folderBrowserDialog.SelectedPath;
        }

        private void OnChangeTransormFileName(object sender, System.EventArgs e)
        {
            try
            {
                var fileName = "DCIM001.jpg";
                var modifiedAt = new DateTime(2024, 5, 7);

                _transformFIleNameExampleLabel.Text = string.Format(Resources.ImportMediaTask_Field_TransformFileName_Example,
                    fileName, modifiedAt, DestinationFolder.TrimEnd('\\').TrimEnd('/') + '\\' + DateTokenReplacer.ParseString(TransformFileName, modifiedAt) + Path.GetExtension(fileName));
            }
            catch
            {
                _transformFIleNameExampleLabel.Text = Resources.ImportMediaTask_Field_TransformFileName_Validation_Invalid;
            }
        }
    }
}
