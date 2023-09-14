using System;
using System.Windows.Forms;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;
using BUtil.Core.FileSystem;
using System.Diagnostics;
using BUtil.Core;

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
            Process.Start(Application.ExecutablePath, $"{Arguments.RunRestorationMaster}");
		}

		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			GetOptionsFromUi();
            e.Cancel = false;
		}
		
        void ApplyLocalization()
        {
            _backupTasksUserControl.ApplyLocalization();
            Text = Resources.Configurator + " " + CopyrightInfo.Version.ToString();
            restorationToolToolStripMenuItem.Text = Resources.RestoreData;
            _helpToolStripMenuItem.Text = Resources.Help;
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
		
        private void OnAboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            SupportManager.DoSupport(SupportRequest.Homepage);
        }

        private void OnOpenLogsFolder(object sender, EventArgs e)
        {
            ProcessHelper.ShellExecute(Directories.LogsFolder);
        }
    }
}
