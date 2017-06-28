using System;
using System.Windows.Forms;
using System.Threading;
using BUtil.Core.Options;
using BUtil.Core.Misc;
using BUtil.Core.PL;
using BUtil.Core.Logs;
using BULocalization;
using BackUserControl = BUtil.BackupUiMaster.Controls.BackUserControl;

namespace BUtil.Configurator.BackupUiMaster.Controls
{
	/// <summary>
	/// Settings that can be adjusted just before backup
	/// </summary>
	internal sealed partial class SettingsUserControl : BackUserControl
	{
		ProgramOptions _options;
		
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
			int previousIndex = backupPriorityComboBox.SelectedIndex;
			backupPriorityComboBox.Items.Clear();
            backupPriorityComboBox.Items.AddRange(new [] { Translation.Current[172], Translation.Current[171], Translation.Current[170], Translation.Current[169] });
            backupPriorityComboBox.SelectedIndex = previousIndex;
            
			chooseBackUpPriorityLabel.Text = Translation.Current[173];
			hearBeepsCheckBox.Text = Translation.Current[410];
			afterEndOfBackupGroupBox.Text = Translation.Current[411];
			
			previousIndex = jobAfterOkBackupComboBox.SelectedIndex;
			jobAfterOkBackupComboBox.Items.Clear();
			jobAfterOkBackupComboBox.Items.AddRange(new [] { Translation.Current[412], Translation.Current[413], Translation.Current[414], Translation.Current[415], Translation.Current[574], Translation.Current[416]});
			jobAfterOkBackupComboBox.SelectedIndex = previousIndex;
			
			Title = Translation.Current[244];
		}
		
		#endregion
		
		public void SetSettingsToUi(ProgramOptions options, PowerTask task, BackupTask backupTask, bool beepWhenFinished, ThreadPriority priority)
		{
			_options = options;
			_options.Priority = priority;
			
			encryptionUserControl.SetOptionsToUi(new object[] {_options, backupTask});
			
			encryptionUserControl.DontCareAboutPasswordLength = _options.DontCareAboutPasswordLength;
			backupPriorityComboBox.SelectedIndex = (int)_options.Priority;
			jobAfterOkBackupComboBox.SelectedIndex = (int)task;
            hearBeepsCheckBox.Checked = beepWhenFinished;
            
            // if our logging level set to support
            // then user wants to get support with our log
            // so disabling power control
            if (_options.LoggingLevel == LogLevel.Support)
            {
                jobAfterOkBackupComboBox.SelectedIndex = (int)PowerTask.None;
                jobAfterOkBackupComboBox.Enabled = false;
            }
            
            backupPriorityComboBox.SelectedIndex = (int)_options.Priority;
			
		}
		
		public void GetSettingsFromUi(out PowerTask task, out bool beepWhenFinished)
		{
			encryptionUserControl.GetOptionsFromUi();
			task = (PowerTask)jobAfterOkBackupComboBox.SelectedIndex;
            _options.Priority = (ThreadPriority)backupPriorityComboBox.SelectedIndex;
            beepWhenFinished = hearBeepsCheckBox.Checked;
		}
		
		void HelpActionAfterBackupButtonClick(object sender, EventArgs e)
		{
			Messages.ShowInformationBox(Translation.Current[470]);
		}
	}
}
