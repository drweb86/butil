using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BUtil.Configurator.AddBackupTaskWizard.Controller;
using BUtil.Configurator.AddBackupTaskWizard.View;
using BUtil.Core.Options;

namespace BUtil.Configurator.AddBackupTaskWizard
{
    class AddBackupTaskWizard
    {
        public static BackupTask OpenAddBackupTaskWizard(ProgramOptions options, bool runFormAsApplication)
        {
            var controller = new AddBackupTaskWizardController(options);
            using (var form = new AddBackupTaskWizardForm(controller))
            {
                if (runFormAsApplication)
                    Application.Run(form);
                else
                    form.ShowDialog();

                if (form.DialogResult == DialogResult.OK && !runFormAsApplication)
                {
                    return controller.Task;
                }
                    
            }

            if (runFormAsApplication)
            {
                Environment.Exit(0);
            }

            return null;
        }
    }
}
