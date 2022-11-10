using System;
using System.Collections.Generic;

using System.Windows.Forms;
using BUtil.Configurator.Configurator;
using BUtil.Configurator.Configurator.Forms;
using BUtil.Core.Misc;
using BUtil.Core.PL;
using BUtil.Core.FileSystem;
using System.IO;
using System.Collections.ObjectModel;
using BUtil.Configurator.Localization;

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
                Files.CriticalFilesCheck();
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
		
		/// <summary>
		/// Processing command line argument
		/// </summary>
		/// <param name="args">Arguments</param>
		static void ProcessArguments(string[] args)
		{
			var controller = new ConfiguratorController();
		    args = args ?? new string[] {};
            if (args.Length == 0)
            {
                Application.Run(new MainForm(controller));
                return;
            }

		    if (args.Length == 1)
			{
			    string firstArgumentUpper = args[0].ToUpperInvariant();
                
                if (firstArgumentUpper == Arguments.RemoveLocalSettings)
				{
					controller.RemoveLocalUserSettings();
				}
                else if (firstArgumentUpper == Arguments.RunRestorationMaster)
				{
					controller.OpenRestorationMaster(null, true);
				}
                else if (args[0] == IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile)
				{
					controller.OpenRestorationMaster(args[0], true);
				}
                else if (firstArgumentUpper == Arguments.RunJournals)
				{
					controller.OpenJournals(true);
				}
                else if (firstArgumentUpper == Arguments.RunBackupMaster)
                {
                    controller.OpenBackupUiMaster(null, true);
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
				controller.OpenBackupUiMaster(taskName, true);
			}
			else if (args.Length > 1)
    		{
					Messages.ShowErrorBox(Resources.InvalidArgumentSPassedToTheProgramNNpleaseReferToManualForTheCompleteListOfParameters);
				}
				else
				{
					Application.Run(new MainForm(controller));
				}

				
		}
		
		#endregion
		
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.SetCompatibleTextRenderingDefault(false);

			ImproveIt.InitInfrastructure(true);
	
			CheckIntergrity();
			ProcessArguments(args);
		}
	}
}
