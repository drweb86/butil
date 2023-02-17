using System;
using System.Windows.Forms;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;
using BUtil.Core.FileSystem;

namespace BUtil.Configurator.Configurator.Forms
{
	internal partial class MainForm
	{
        public MainForm()
		{
			InitializeComponent();

            this._backupTasksUserControl.OnRequestToSaveOptions += OnRequestToSaveOptions;
            this._backupTasksUserControl.HelpLabel = helpToolStripStatusLabel;

			ApplyLocalization();
			ApplyOptionsToUi();

			restorationToolToolStripMenuItem.Enabled = !Program.PackageIsBroken;
		}

        private static void RunRestorationTool()
		{
            ConfiguratorController.OpenRestorationMaster(null, false);
		}

		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			GetOptionsFromUi();
            e.Cancel = false;
		}
		
        void ApplyLocalization()
        {
            _backupTasksUserControl.ApplyLocalization();
            Text = Resources.Configurator;
            restorationToolToolStripMenuItem.Text = Resources.RestoreData;
            _helpToolStripMenuItem.Text= Resources.Help;
            helpToolStripMenuItem.Text = Resources.Documentation;
            _aboutToolStripMenuItem.Text = Resources.About;
            _logsToolStripMenuItem.Text = Resources.Logging;
        }

		private void GetOptionsFromUi()
		{
            _backupTasksUserControl.GetOptionsFromUi();
        }

		private bool OnRequestToSaveOptions()
		{
            GetOptionsFromUi();
			return true;
        }

        private void ApplyOptionsToUi()
        {
            _backupTasksUserControl.SetOptionsToUi(null);
        }

		void RestorationToolToolStripMenuItemClick(object sender, EventArgs e)
		{
			RunRestorationTool();
		}
		
		void CancelButtonClick(object sender, EventArgs e)
		{
			Close();
		}

        private void OnHelpToolStripMenuItemClick(object sender, EventArgs e)
        {
            SupportManager.DoSupport(SupportRequest.Documentation);
        }

        private void OnAboutToolStripMenuItemClick(object sender, EventArgs e)
        {
			using var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void OnOpenLogsFolder(object sender, EventArgs e)
        {
            ProcessHelper.ShellExecute(Directories.LogsFolder);
        }
    }
}
