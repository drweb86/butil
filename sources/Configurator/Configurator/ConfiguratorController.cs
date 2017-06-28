using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BUtil.Configurator.BackupUiMaster.Forms;
using System.Diagnostics;
using System.Globalization;
using BULocalization;
using BUtil.Core.Misc;
using BUtil.Core;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.Core.PL;
using BUtil.RestorationMaster;
using BUtil.Configurator.LogsManagement;

namespace BUtil.Configurator.Configurator
{
    public class ConfiguratorController
    {
        #region Constants

        const string NoProfileOptions = "File with profile settings '{0}' absent. It will be set to defaults.\n\nReason:\n{1}";
        const string Error = "Error";

        #endregion

        ProgramOptions _profileOptions = ProgramOptionsManager.Default;
        
		readonly LanguagesManager _localsManager;
		
        public LanguagesManager LocalManager
        {
            get { return _localsManager; }
        }

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
			if (Program.PackageIsBroken || Program.SevenZipIsBroken)
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
		
		public void OpenBackupUiMaster(string[] taskTitles, bool runFormAsApplication)
		{
            if (taskTitles == null)
            {
                throw new ArgumentNullException("taskTitles");
            }

		    if (Program.PackageIsBroken || Program.SevenZipIsBroken)
            {
                return;
            }

            if (!runFormAsApplication)
            {
                var arguments = new StringBuilder(Arguments.RunBackupMaster);

                foreach (var taskTitle in taskTitles)
                {
                    arguments.Append(string.Format(" \"{0}={1}\"", Arguments.RunTask, taskTitle));
                }

                Process.Start(Application.ExecutablePath, arguments.ToString());
                return;
            }

		    LoadSettings();

            //TODO: now we suppoprt execution of just one task. But it will be great if we could execute each tasl one by one
            // here among checked in task selection form

		    var backupTasksChain = new List<BackupTask>();
		    foreach (var taskTitle in taskTitles)
		    {
                BackupTask backupTask = null;
                foreach (var task in ProgramOptions.BackupTasks)
                {
                    if (task.Key == taskTitle)
                    {
                        backupTask = task.Value;
                    }
                }
                if (backupTask == null)
                {
                    Messages.ShowErrorBox(string.Format("Missing task '{0}' is missing.", taskTitle));
                }
                else
                {
                    backupTasksChain.Add(backupTask);
                }
		    }

// This must be refactored in order to use something like Tool pattern
            using (var form = new BackupMasterForm(_profileOptions, backupTasksChain))
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
		
		public ConfiguratorController(LanguagesManager manager)
		{
			_localsManager = manager;
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

            try
            {
                ProgramOptionsManager.ValidateOptions(_profileOptions);
            }
            catch (InvalidDataException exc)
            {
            	Messages.ShowErrorBox(exc.Message);
                return false;
            }
            catch (ArgumentNullException exc)
            {
                Messages.ShowErrorBox(exc.Message);
                return false;
            }

            bool taskNeedScheduling = false;
            foreach (var pair in _profileOptions.BackupTasks)
            {
                if (pair.Value.EnableScheduling)
                {
                    taskNeedScheduling = true;
                }
            }

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

		/// <summary>
		/// Loads language settigns
		/// Constraints: should be called only form main form!
		/// </summary>
		public void LoadLanguage(ToolStripMenuItem chooseLanguagesMenu)
		{
			try
			{
                _localsManager.Init();
                _localsManager.Apply();
                _localsManager.GenerateMenuWithLanguages(chooseLanguagesMenu);
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message, Error, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
			}
		}
    }
}
