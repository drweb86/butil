using System;
using System.Windows.Forms;
using System.IO;
using BUtil.Configurator.BackupUiMaster.Forms;
using System.Diagnostics;
using System.Globalization;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.RestorationMaster;
using BUtil.Configurator.LogsManagement;
using static System.Windows.Forms.Design.AxImporter;

namespace BUtil.Configurator.Configurator
{
    public class ConfiguratorController
    {
        
        private ProgramOptions _profileOptions = ProgramOptionsManager.Default;
        private ProgramOptions _optionsDuringProgramLoad = ProgramOptionsManager.Default;

        public void OpenRestorationMaster(string file, bool runFormAsApplication)
		{
			if (Program.PackageIsBroken)
			{
				return;
			}

            using var form = new OpenBackupForm(file != null ? Path.GetDirectoryName(file) : null);
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
		
        public void RemoveLocalUserSettings()
		{
			// we're deleting only default logs folder location
			if (Directory.Exists(Directories.LogsFolder))
				Directory.Delete(Directories.LogsFolder, true);
		               	
			if (Directory.Exists(Directories.UserDataFolder))
				Directory.Delete(Directories.UserDataFolder, true);
		}
		
		public void OpenBackupUi(string taskName)
        {
            if (Program.PackageIsBroken)
            {
                return;
            }

            LoadSettings();
            var task = GetBackupTaskToExecute(taskName);
            using var form = new BackupMasterForm(_profileOptions, task);
            Application.Run(form);
            Environment.Exit(0);
        }

        public void LaunchBackupUIToolInSeparateProcess(string taskName)
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

        public ProgramOptions ProgramOptions
        {
            get { return _profileOptions; }
        }

        public void LoadSettings()
        {

            var programOptionsStoreService = new ProgramOptionsStoreService();
            _profileOptions = programOptionsStoreService.Load();

            _optionsDuringProgramLoad = _profileOptions.Clone() as ProgramOptions;
        }

        public bool ProgramOptionsChanged()
        {
            return !_optionsDuringProgramLoad.CompareTo(_profileOptions); // bug in .Net 7
        }
        
        public bool StoreSettings()
        {
            try
            {
                var programOptionsStoreService = new ProgramOptionsStoreService();
                programOptionsStoreService.Save(_profileOptions);
                _optionsDuringProgramLoad = _profileOptions.Clone() as ProgramOptions;
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
