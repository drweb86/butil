using System;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.BackupUiMaster.Controls
{
    internal sealed partial class SettingsUserControl : BUtil.Core.PL.BackUserControl
    {
        public SettingsUserControl()
        {
            InitializeComponent();
        }

        #region Locals

        public override void ApplyLocalization()
        {
            afterEndOfBackupGroupBox.Text = Resources.AfterCompletionOfBackup;

            var previousIndex = jobAfterOkBackupComboBox.SelectedIndex;
            jobAfterOkBackupComboBox.Items.Clear();
            jobAfterOkBackupComboBox.Items.AddRange(new[] { Resources.ShutdownPc, Resources.LogOff, Resources.Reboot, Resources.DoNothing });
            jobAfterOkBackupComboBox.SelectedIndex = previousIndex;

            titleLabel.Text = Resources.Settings;
        }

        #endregion

        public void SetSettingsToUi(PowerTask task)
        {
            jobAfterOkBackupComboBox.SelectedIndex = (int)task;
        }

        public void GetSettingsFromUi(out PowerTask task)
        {
            task = (PowerTask)jobAfterOkBackupComboBox.SelectedIndex;
        }

        void HelpActionAfterBackupButtonClick(object sender, EventArgs e)
        {
            Messages.ShowInformationBox(Resources.IfYouChooseSomethingOtherThanDoNothingNprogramWillConfigureOsToShowYouReportNonNextYourLogonToTheSystemIfAnyErrorsNorWarningsWillBeRegisteredDuringTheBackup);
        }
    }
}
