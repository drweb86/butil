using System;
using System.Windows.Forms;
using System.IO;
using BUtil.Configurator.BackupUiMaster.Forms;
using System.Diagnostics;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.RestorationMaster;

namespace BUtil.Configurator.Configurator
{
    public static class ConfiguratorController
    {
        public static void OpenRestorationMaster(string file, bool runFormAsApplication)
		{
			if (Program.PackageIsBroken)
			{
				return;
			}

            using var form = new OpenBackupForm(file == null ? null : Path.GetDirectoryName(file));
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
		
        public static void RemoveLocalUserSettings()
		{
			if (Directory.Exists(Directories.LogsFolder))
				Directory.Delete(Directories.LogsFolder, true);
		}
		
		public static void OpenBackupUi(string taskName)
        {
            if (Program.PackageIsBroken)
            {
                return;
            }

            var task = GetBackupTaskToExecute(taskName);
            using var form = new BackupMasterForm(task);
            Application.Run(form);
            Environment.Exit(0);
        }

        public static void LaunchBackupUIToolInSeparateProcess(string taskName)
        {
            Process.Start(Application.ExecutablePath, $"{Arguments.RunBackupMaster} \"{Arguments.RunTask}={taskName}\"");
        }

        private static BackupTask GetBackupTaskToExecute(string taskName)
        {
            BackupTask task = null;

            var backupTaskStoreService = new BackupTaskStoreService();
            if (taskName == null)
            {
                var backupTaskNames = backupTaskStoreService.GetNames();

                using var selectTaskForm = new SelectTaskToRunForm(backupTaskNames);
                if (selectTaskForm.ShowDialog() == DialogResult.OK)
                    task = backupTaskStoreService.Load(selectTaskForm.TaskToRun);
                else
                    Environment.Exit(-1);
            }
            else
            {
                task = backupTaskStoreService.Load(taskName);
            }

            if (task == null)
            {
                Messages.ShowErrorBox($"Task '{taskName}' is missing.");
                Environment.Exit(-1);
            }

            return task;
        }      
    }
}
