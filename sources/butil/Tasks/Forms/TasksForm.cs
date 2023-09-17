using System;
using System.Windows.Forms;
using BUtil.Core.Misc;
using BUtil.Core.Localization;
using BUtil.Core.FileSystem;
using System.Diagnostics;
using BUtil.Core;

namespace BUtil.Configurator.Configurator.Forms
{
	internal partial class TasksForm
	{
        public TasksForm()
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
            Process.Start(Application.ExecutablePath, Arguments.Restore);
		}

		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			GetOptionsFromUi();
            e.Cancel = false;
		}
		
        void ApplyLocalization()
        {
            _backupTasksUserControl.ApplyLocalization();
            Text = Resources.ApplicationName_Tasks + " " + CopyrightInfo.Version.ToString();
            restorationToolToolStripMenuItem.Text = Resources.RestoreData;
            _logsToolStripMenuItem.Text = Resources.LogFile_OpenLogs;
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
