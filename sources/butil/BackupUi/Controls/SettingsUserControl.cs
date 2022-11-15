using System;
using System.Windows.Forms;
using BUtil.Core.Options;
using BUtil.Core.Misc;

using TitledBackUserControl = BUtil.BackupUiMaster.Controls.TitledBackUserControl;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.BackupUiMaster.Controls
{
    /// <summary>
    /// Settings that can be adjusted just before backup
    /// </summary>
    internal sealed partial class SettingsUserControl : TitledBackUserControl
	{
		public SettingsUserControl()
		{
			InitializeComponent();

			encryptionUserControl.BorderStyle = BorderStyle.None;
			encryptionUserControl.DrawAtractiveBorders = false;
		}
		
		#region Locals
		
		public override void ApplyLocalization()
		{
			encryptionUserControl.ApplyLocalization();
            
			hearBeepsCheckBox.Text = Resources.BeepSeveralTimes;
			afterEndOfBackupGroupBox.Text = Resources.AfterCompletionOfBackup;
			
			var previousIndex = jobAfterOkBackupComboBox.SelectedIndex;
			jobAfterOkBackupComboBox.Items.Clear();
			jobAfterOkBackupComboBox.Items.AddRange(new [] { Resources.ShutdownPc, Resources.LogOff, Resources.Reboot, Resources.DoNothing});
			jobAfterOkBackupComboBox.SelectedIndex = previousIndex;
			
			Title = Resources.Settings;
		}
		
		#endregion
		
		public void SetSettingsToUi(PowerTask task, BackupTask backupTask, bool beepWhenFinished)
		{
			encryptionUserControl.UpdateModel(backupTask);
			
			jobAfterOkBackupComboBox.SelectedIndex = (int)task;
            hearBeepsCheckBox.Checked = beepWhenFinished;
		}
		
		public void GetSettingsFromUi(out PowerTask task, out bool beepWhenFinished)
		{
			encryptionUserControl.GetOptionsFromUi();
			task = (PowerTask)jobAfterOkBackupComboBox.SelectedIndex;
            beepWhenFinished = hearBeepsCheckBox.Checked;
		}
		
		void HelpActionAfterBackupButtonClick(object sender, EventArgs e)
		{
			Messages.ShowInformationBox(Resources.IfYouChooseSomethingOtherThanDoNothingNprogramWillConfigureOsToShowYouReportNonNextYourLogonToTheSystemIfAnyErrorsNorWarningsWillBeRegisteredDuringTheBackup);
		}
	}
}
