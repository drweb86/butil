using System;
using System.Collections.Generic;

using System.Windows.Forms;
using BUtil.Configurator.Configurator;
using BUtil.Configurator.Configurator.Forms;
using BUtil.Core.Misc;
using BUtil.Core.FileSystem;
using System.IO;
using System.Collections.ObjectModel;
using BUtil.Core.Localization;
using System.Linq;
using System.Threading.Tasks;

namespace BUtil.Configurator
{
    public static class Program
	{
		#region Fields
		
		static bool _packageIsBroken;
		
		#endregion
		
		#region Properties
		
		public static bool PackageIsBroken
		{
			get { return _packageIsBroken; }
		}
		
		#endregion
		
		#region Private methods
		
		/// <summary>
		/// Checking the integrity: Checking if the all components present and Checking 7-zip intergrity
		/// </summary>
		static void CheckIntergrity()
		{
			try
            {
                Directories.CriticalFoldersCheck();
            }
            catch (DirectoryNotFoundException e)
            {
                _packageIsBroken = true;
                Messages.ShowErrorBox(string.Format(Resources.ButilSoftwarePackageComponent0IsMissingNNpleaseReinstallApplicationNNrestorationBackupAndHelpFunctionsWillBeUnavailable, e.Message));
            }
            catch (FileNotFoundException e)
            {
                _packageIsBroken = true;
                Messages.ShowErrorBox(string.Format(Resources.ButilSoftwarePackageComponent0IsMissingNNpleaseReinstallApplicationNNrestorationBackupAndHelpFunctionsWillBeUnavailable, e.Message));
            }
		}

		private static void ProcessArguments(string[] args)
		{
			args ??= Array.Empty<string>();
			if (args.Length == 0)
			{
                CheckForUpdates();
                Application.Run(new MainForm());
				return;
			}

			if (args.Length == 1)
			{
				string firstArgumentUpper = args[0].ToUpperInvariant();

				if (firstArgumentUpper == Arguments.RemoveLocalSettings)
				{
					ConfiguratorController.RemoveLocalUserSettings();
				}
				else if (firstArgumentUpper == Arguments.RunRestorationMaster)
				{
					ConfiguratorController.OpenRestorationMaster(null, true, null);
				}
				else if (IncrementalBackupModelConstants.Files.Any(x => args[0].EndsWith(x)))
				{
					ConfiguratorController.OpenRestorationMaster(args[0], true, null);
				}
				else if (firstArgumentUpper == Arguments.RunBackupMaster)
				{
					ConfiguratorController.OpenBackupUi(null);
				}
				else
				{
					Messages.ShowErrorBox(Resources.InvalidArgumentSPassedToTheProgramNNpleaseReferToManualForTheCompleteListOfParameters);
				}
			}
			else if (args.Length > 1 && args[0].ToUpperInvariant() == Arguments.RunBackupMaster)
			{
				string taskName = null;
				foreach (var argument in args)
				{
					if (argument.StartsWith(Arguments.RunTask) && argument.Length > Arguments.RunTask.Length)
					{
						taskName = argument.Substring(Arguments.RunTask.Length + 1);
					}
				}
				ConfiguratorController.OpenBackupUi(taskName);
			}
            else if (args.Length > 1 && args[0].ToUpperInvariant() == Arguments.RunRestorationMaster)
            {
                string taskName = null;
                foreach (var argument in args)
                {
                    if (argument.StartsWith(Arguments.RunTask) && argument.Length > Arguments.RunTask.Length)
                    {
                        taskName = argument.Substring(Arguments.RunTask.Length + 1);
                    }
                }
                ConfiguratorController.OpenRestorationMaster(null, true, taskName);
            }
            else if (args.Length > 1)
			{
				Messages.ShowErrorBox(Resources.InvalidArgumentSPassedToTheProgramNNpleaseReferToManualForTheCompleteListOfParameters);
			}
			else
			{
				CheckForUpdates();
				Application.Run(new MainForm());
			}
		}

        private static void CheckForUpdates()
        {
            try
            {
                var update = UpdateChecker.CheckForUpdate().GetAwaiter().GetResult(); // not supported by .Net (STA for WinForms)

                if (update.HasUpdate)
                {
                    Messages.ShowInformationBox(string.Format(Resources.New0VersionIsAvailableNNchangesAreN1NNprogramWillNowOpenBrowserWithTheDownloadPage, update.Version, update.Changes));
                    SupportManager.DoSupport(SupportRequest.Releases);
                }
            }
            catch (InvalidOperationException exc)
            {
                Messages.ShowErrorBox(exc.Message);
            }
        }

        #endregion

        [STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.SetCompatibleTextRenderingDefault(false);

			ImproveIt.HandleUiError = message => Messages.ShowErrorBox(message);
	
			CheckIntergrity();
			ProcessArguments(args);
		}
	}
}
