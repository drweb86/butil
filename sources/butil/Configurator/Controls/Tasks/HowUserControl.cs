using BUtil.Configurator.Localization;
using BUtil.Core.BackupModels;
using BUtil.Core.Options;
using System;

namespace BUtil.Configurator.Controls
{
	internal sealed partial class HowUserControl : BUtil.Core.PL.BackUserControl
	{
		private readonly BackupTask _task;
        private readonly Action _modelSettingsUpdated;

        public HowUserControl(BackupTask task, Action modelSettingsUpdated)
		{
            this._modelSettingsUpdated = modelSettingsUpdated;
            this._task = task;

            InitializeComponent();

            _chooseBackupModel.Text = Resources.ChooseBackupModel;
            _incrementalBackupRadioButton.Text = Resources.IncrementalBackup;
            _backupModelLabel.Text = Resources.IncrementalBackupDescription;
            _disableCompressionEncryptionCheckBox.Text = Resources.DisableCompressionAndEncryption;
            _disableCompressionAndEncryptionUsagesLabel.Text = Resources.DisableCompressionAndEncryptionDescription;

            _incrementalBackupRadioButton.Checked = _task.Model is IncrementalBackupModelOptions;
            if (_task.Model is IncrementalBackupModelOptions)
            {
                var options = _task.Model as IncrementalBackupModelOptions;
                _disableCompressionEncryptionCheckBox.Checked = options.DisableCompressionAndEncryption;
            }
        }
			
		public override void GetOptionsFromUi()
		{
        }

        private void OnBackupModelUpdated(object sender, System.EventArgs e)
        {
            if (_incrementalBackupRadioButton.Checked)
            {
                _task.Model = new IncrementalBackupModelOptions { DisableCompressionAndEncryption = _disableCompressionEncryptionCheckBox.Checked };
                _modelSettingsUpdated();
            }
        }
    }
}
