using System;
using System.Windows.Forms;
using BUtil.Core.Options;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;
using BUtil.Core.BackupModels;

namespace BUtil.Configurator.BackupUiMaster.Controls
{
    internal sealed partial class SettingsUserControl : BUtil.Core.PL.BackUserControl
    {
        public SettingsUserControl()
		{
			InitializeComponent();

			encryptionUserControl1.BorderStyle = BorderStyle.None;
			encryptionUserControl1.DrawAtractiveBorders = false;
		}
		
		#region Locals
		
		public override void ApplyLocalization()
		{
			encryptionUserControl1.ApplyLocalization();
            
			hearBeepsCheckBox.Text = Resources.BeepSeveralTimes;
			afterEndOfBackupGroupBox.Text = Resources.AfterCompletionOfBackup;
			
			var previousIndex = jobAfterOkBackupComboBox.SelectedIndex;
			jobAfterOkBackupComboBox.Items.Clear();
			jobAfterOkBackupComboBox.Items.AddRange(new [] { Resources.ShutdownPc, Resources.LogOff, Resources.Reboot, Resources.DoNothing});
			jobAfterOkBackupComboBox.SelectedIndex = previousIndex;
			
			titleLabel.Text = Resources.Settings;
		}
		
		#endregion
		
		public void SetSettingsToUi(PowerTask task, BackupTask backupTask, bool beepWhenFinished)
		{
			encryptionUserControl1.UpdateModel(backupTask);
			if (backupTask.Model is IncrementalBackupModelOptions)
			{
				var options = backupTask.Model as IncrementalBackupModelOptions;

				encryptionUserControl1.Visible = !options.DisableCompressionAndEncryption;
            }


            jobAfterOkBackupComboBox.SelectedIndex = (int)task;
            hearBeepsCheckBox.Checked = beepWhenFinished;
		}
		
		public void GetSettingsFromUi(out PowerTask task, out bool beepWhenFinished)
		{
			encryptionUserControl1.GetOptionsFromUi();
			task = (PowerTask)jobAfterOkBackupComboBox.SelectedIndex;
            beepWhenFinished = hearBeepsCheckBox.Checked;
		}
		
		void HelpActionAfterBackupButtonClick(object sender, EventArgs e)
		{
			Messages.ShowInformationBox(Resources.IfYouChooseSomethingOtherThanDoNothingNprogramWillConfigureOsToShowYouReportNonNextYourLogonToTheSystemIfAnyErrorsNorWarningsWillBeRegisteredDuringTheBackup);
		}
	}
}
