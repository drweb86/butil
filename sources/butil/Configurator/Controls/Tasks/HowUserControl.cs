using BUtil.Configurator.Localization;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Configurator.Controls
{
    internal sealed partial class HowUserControl : BUtil.Core.PL.BackUserControl
	{
		private readonly BackupTaskV2 _task;

        public HowUserControl(BackupTaskV2 task)
		{
            this._task = task;

            InitializeComponent();

            _chooseBackupModel.Text = Resources.ChooseBackupModel;
            _incrementalBackupRadioButton.Text = Resources.IncrementalBackup;
            _backupModelLabel.Text = Resources.IncrementalBackupDescription;
            _incrementalBackupRadioButton.Checked = _task.Model is IncrementalBackupModelOptionsV2;
        }
			
		public override void GetOptionsFromUi()
		{
        }
    }
}
