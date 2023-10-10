
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

            ApplyOptionsToUi();
        }

        private void ApplyOptionsToUi()
        {
            _backupTasksUserControl.SetOptionsToUi(null);
        }
    }
}
