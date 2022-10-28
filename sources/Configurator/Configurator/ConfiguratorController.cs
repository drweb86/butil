using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BUtil.Configurator.BackupUiMaster.Forms;
using System.Diagnostics;
using System.Globalization;

using BUtil.Core.Misc;
using BUtil.Core;
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
        
		static void CheckSchedulerStartup()
        {
			Process.Start(Files.Scheduler, SchedulerParameters.CREATE_STARTUP_SCRIPT);
        }
		
		void ManageSchedulerStartup(bool enable)
		{
			if (enable)
			{
				if (!_profileOptions.DontCareAboutSchedulerStartup)
				{
					CheckSchedulerStartup();
				}
			}
			else
			{
				Process.Start(Files.Scheduler, SchedulerParameters.REMOVE_STARTUP_SCRIPT_IF_ANY);
			}
		}

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
			Process.Start(Files.Scheduler, SchedulerParameters.REMOVE_STARTUP_SCRIPT_IF_ANY);
               			
			if (File.Exists(Files.ProfileFile))
				File.Delete(Files.ProfileFile);
		               	
			// we're deleting only default logs folder location
			if (Directory.Exists(Directories.LogsFolder))
				Directory.Delete(Directories.LogsFolder, true);
		               	
			if (Directory.Exists(Directories.UserDataFolder))
				Directory.Delete(Directories.UserDataFolder, true);
		}
		
		public void OpenBackupUiMaster(string[] taskNames, bool runFormAsApplication)
		{
            if (taskNames == null)
            {
                throw new ArgumentNullException("taskTitles");
            }

		    if (Program.PackageIsBroken)
            {
                return;
            }

            if (!runFormAsApplication)
            {
                var arguments = new StringBuilder(Arguments.RunBackupMaster);

                foreach (var taskTitle in taskNames)
                {
                    arguments.Append(string.Format(" \"{0}={1}\"", Arguments.RunTask, taskTitle));
                }

                Process.Start(Application.ExecutablePath, arguments.ToString());
                return;
            }

		    LoadSettings();

            //TODO: now we suppoprt execution of just one task. But it will be great if we could execute each tasl one by one
            // here among checked in task selection form

            var backupTaskStoreService = new BackupTaskStoreService();
            var backupTasks = backupTaskStoreService.Load(taskNames, out var missingTasks);
            if (missingTasks.Any())
                Messages.ShowErrorBox($"Missing task '{string.Join(",", missingTasks)}' is missing.");

// This must be refactored in order to use something like Tool pattern
            using (var form = new BackupMasterForm(_profileOptions, backupTasks.ToList()))
            {
                Application.Run(form);
            }

            Environment.Exit(0);
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
        	ProcessesKiller.FindAndKillProcess(Constants.TrayApplicationProcessName);

            var backupTaskStoreService = new BackupTaskStoreService();
            var tasks = backupTaskStoreService.LoadAll();
            bool taskNeedScheduling = tasks.Any(item => item.SchedulerDays.Any());

            try
            {
                ProgramOptionsManager.StoreSettings(_profileOptions);

                ManageSchedulerStartup(taskNeedScheduling && (!_profileOptions.DontCareAboutSchedulerStartup));
            }
            catch (Exception ee)
            {
            	Messages.ShowErrorBox(ee.Message);
                return false;
            }

            if (!_profileOptions.DontCareAboutSchedulerStartup)
            {
                if (taskNeedScheduling && !_profileOptions.DontNeedScheduler)
            	{
		            Process.Start(Files.Scheduler, SchedulerParameters.START_WITHOUT_MESSAGE);
            	}
            }

            return true;
        }
    }
}
