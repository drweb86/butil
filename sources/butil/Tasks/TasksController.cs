using System;
using System.Windows.Forms;
using System.IO;
using BUtil.Core.Options;
using BUtil.RestorationMaster;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Configurator.Configurator
{
    public static class TasksController
    {
        public static void OpenRestorationMaster(string file, bool runFormAsApplication, string taskName)
		{
            TaskV2 backupTask = null;
            if (taskName != null)
            {
                var service = new TaskV2StoreService();
                backupTask = service.Load(taskName);
            } else if (file != null)
            {
                backupTask = new TaskV2();
                var options = (IncrementalBackupModelOptionsV2)backupTask.Model;
                options.To = new FolderStorageSettingsV2() { DestinationFolder = Path.GetDirectoryName(file) };
            }

            using var form = new OpenBackupForm(backupTask);
            if (runFormAsApplication)
            {
                Application.Run(form);
                Environment.Exit(0);
            }
            else
            {
                form.ShowDialog();
            }
		}
    }
}
