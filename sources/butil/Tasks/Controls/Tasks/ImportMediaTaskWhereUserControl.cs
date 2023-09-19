using BUtil.Core.BackupModels;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.MediaSyncBackupModel;
using System;

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

            OnChangeTransormFileName(this, new EventArgs());
        }

        public string TransformFileName
        {
            get => _transformFileTemplateTextBox.Text;
            set => _transformFileTemplateTextBox.Text = value;
        }
        public bool SkipAlreadyImportedFiles
        {
            get => _skipPreviouslyImportedFilesCheckbox.Checked;
            set => _skipPreviouslyImportedFilesCheckbox.Checked = value;
        }

        public string DestinationFolder
        {
            get => _outputFolderTextBox.Text;
            set => _outputFolderTextBox.Text = value;
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
                _outputFolderTextBox.Text = _folderBrowserDialog.SelectedPath;
        }

        private void OnChangeTransormFileName(object sender, System.EventArgs e)
        {
            try
            {
                _transformFIleNameExampleLabel.Text = DateTokenReplacer.ParseString(TransformFileName, DateTime.Now);
            }
            catch
            {
                _transformFIleNameExampleLabel.Text = BUtil.Core.Localization.Resources.ImportMediaTask_Field_TransformFileName_Validation_Invalid;
            }
        }
    }
}
