using System;
using System.Windows.Forms;
using System.IO;
using BUtil.Configurator.BackupUiMaster.Forms;
using System.Diagnostics;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.RestorationMaster;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Misc;

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
		
		public static void OpenBackupUi(string taskName)
        {
            var task = GetBackupTaskToExecute(taskName);
            using var form = new TaskProgressForm(task);
            Application.Run(form);
            Environment.Exit(0);
        }

        private static TaskV2 GetBackupTaskToExecute(string taskName)
        {
            TaskV2 task = null;

            var backupTaskStoreService = new TaskV2StoreService();
            if (taskName == null)
            {
                var backupTaskNames = backupTaskStoreService.GetNames();

                using var selectTaskForm = new SelectTaskForm(backupTaskNames);
                if (selectTaskForm.ShowDialog() == DialogResult.OK)
                {
                    task = backupTaskStoreService.Load(selectTaskForm.TaskToRun);
                    if (task == null)
                    {
                        Messages.ShowErrorBox(BUtil.Core.Localization.Resources.Task_Validation_NotSupported);
                        Environment.Exit(-1);
                    }
                }
                else
                    Environment.Exit(-1);
            }
            else
            {
                task = backupTaskStoreService.Load(taskName);
                if (task == null)
                {
                    Messages.ShowErrorBox(BUtil.Core.Localization.Resources.Task_Validation_NotSupported);
                    Environment.Exit(-1);
                }
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
