using BUtil.Configurator.Localization;
using BUtil.Core.BackupModels;
using BUtil.Core.Options;

namespace BUtil.Configurator.Controls
{
	internal sealed partial class HowUserControl : BUtil.Core.PL.BackUserControl
	{
		private readonly BackupTask _task;

        public HowUserControl(BackupTask task)
		{
            this._task = task;

            InitializeComponent();

            _chooseBackupModel.Text = Resources.ChooseBackupModel;
            _incrementalBackupRadioButton.Text = Resources.IncrementalBackup;
            _backupModelLabel.Text = Resources.IncrementalBackupDescription;
            _incrementalBackupRadioButton.Checked = _task.Model is IncrementalBackupModelOptions;
        }
			
		public override void GetOptionsFromUi()
		{
        }
    }
}
