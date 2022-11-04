using System;
using System.Windows.Forms;
using System.IO;
using BUtil.Configurator.BackupUiMaster.Forms;
using System.Diagnostics;
using System.Globalization;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.Core.PL;
using BUtil.RestorationMaster;
using BUtil.Configurator.LogsManagement;
using System.Linq;

namespace BUtil.Configurator.Configurator
{
    public class ConfiguratorController
    {
        #region Constants

        const string NoProfileOptions = "File with profile settings '{0}' absent. It will be set to defaults.\n\nReason:\n{1}";
        const string Error = "Error";

        #endregion

        ProgramOptions _profileOptions = ProgramOptionsManager.Default;

		public void OpenRestorationMaster(string image, bool runFormAsApplication)
		{
			if (Program.PackageIsBroken)
			{
				return;
			}

			using (var form = new RestoreMasterMainForm())
			{
				form.ImageFileToShow = image;
				if (runFormAsApplication)
					Application.Run(form);
				else
					form.ShowDialog();
			}
			
			if (runFormAsApplication)
			{
				Environment.Exit(0);
			}
		}
		
		public void RemoveLocalUserSettings()
		{
			if (File.Exists(Files.ProfileFile))
				File.Delete(Files.ProfileFile);
		               	
			// we're deleting only default logs folder location
			if (Directory.Exists(Directories.LogsFolder))
				Directory.Delete(Directories.LogsFolder, true);
		               	
			if (Directory.Exists(Directories.UserDataFolder))
				Directory.Delete(Directories.UserDataFolder, true);
		}
		
		public void OpenBackupUiMaster(string taskName, bool runFormAsApplication)
        {
            if (Program.PackageIsBroken)
            {
                return;
            }

            if (!runFormAsApplication)
            {
                Process.Start(Application.ExecutablePath, $"{Arguments.RunBackupMaster} \"{Arguments.RunTask}={taskName}\"");
                return;
            }

            LoadSettings();

            var task = GetBackupTaskToExecute(taskName);

            using var form = new BackupMasterForm(_profileOptions, task);
            Application.Run(form);
            Environment.Exit(0);
        }

        private static BackupTask GetBackupTaskToExecute(string taskName)
        {
            BackupTask task = null;

            var backupTaskStoreService = new BackupTaskStoreService();
            if (taskName == null)
            {
                var backupTasks = backupTaskStoreService.LoadAll();

                using var selectTaskForm = new SelectTaskToRunForm(backupTasks.ToDictionary(t => t.Name, t => t));
                if (selectTaskForm.ShowDialog() == DialogResult.OK)
                    task = selectTaskForm.TaskToRun;
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

        public void OpenJournals(bool runFormAsApplication)
		{
			if (runFormAsApplication)
			{
				LoadSettings();
			}
			
			using (var form = new LogsViewerForm(new LogManagementConftroller(_profileOptions.LogsFolder)))
			{
				if (runFormAsApplication)
				{
					Application.Run(form);
				}
				else
					form.ShowDialog();
			}
			
			if (runFormAsApplication)
			{
				Environment.Exit(0);
			}
		}

        public ProgramOptions ProgramOptions
        {
            get { return _profileOptions; }
        }

        public void LoadSettings()
        {
            if (File.Exists(Files.ProfileFile))
            {
                try
                {
                    _profileOptions = ProgramOptionsManager.LoadSettings();
                }
                catch (OptionsException noOptions)
                {
                    MessageBox.Show(string.Format(CultureInfo.InvariantCulture, NoProfileOptions, Files.ProfileFile, noOptions.Message), Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _profileOptions = ProgramOptionsManager.Default;
                }
            }
            else
            {
                _profileOptions = ProgramOptionsManager.Default;
            }
        }
        
        public bool StoreSettings()
        {
            try
            {
                ProgramOptionsManager.StoreSettings(_profileOptions);
            }
            catch (Exception ee)
            {
            	Messages.ShowErrorBox(ee.Message);
                return false;
            }

            return true;
        }
    }
}
