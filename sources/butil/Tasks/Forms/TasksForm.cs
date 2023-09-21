using System;
using BUtil.Core.Misc;
using BUtil.Core.Localization;
using BUtil.Core;

namespace BUtil.Configurator.Configurator.Forms
{
    internal partial class TasksForm
    {
        public TasksForm()
        {
            InitializeComponent();

            this._backupTasksUserControl.HelpLabel = helpToolStripStatusLabel;
            _backupTasksUserControl.ApplyLocalization();
            Text = Resources.ApplicationName_Tasks + " " + CopyrightInfo.Version.ToString();
            restorationToolToolStripMenuItem.Text = Resources.Task_Restore;
            _logsToolStripMenuItem.Text = Resources.LogFile_OpenLogs;

            ApplyOptionsToUi();
        }

        private void ApplyOptionsToUi()
        {
            _backupTasksUserControl.SetOptionsToUi(null);
        }

        private void OnOpenRestorationApp(object sender, EventArgs e)
        {
            SupportManager.OpenRestorationApp();
        }

        private void OnOpenHomePage(object sender, EventArgs e)
        {
            SupportManager.OpenHomePage();
        }

        private void OnOpenLogsFolder(object sender, EventArgs e)
        {
            SupportManager.OpenLogs();
        }
    }
}
