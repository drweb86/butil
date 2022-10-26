using System;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.ObjectModel;
using System.IO;

using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.Core.Logs;
using BUtil.Core;
using BUtil.Core.Misc;
using BUtil.ConsoleBackup.Localization;
using BUtil.Core.PL;
using System.Linq;

namespace BUtil.ConsoleBackup.Controller
{
	internal class ConsoleBackupController: IDisposable
	{
        #region Constructor

        public ConsoleBackupController()
        {
            LoadAndValidateOptions();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses the command line arguments
        /// </summary>
        /// <param name="args">command line arguments</param>
        /// <returns>true when all arguments were recognized</returns>
        public bool ParseCommandLineArguments(string[] args)
		{
            args = args ?? new string[] { };

            foreach (string argument in args)
            {
                if (argument.StartsWith(TaskCommandLineArgument, StringComparison.InvariantCultureIgnoreCase))
                {
                    _backupTaskTitles.Add(argument.Substring(TaskCommandLineArgument.Length));
                    continue;
                }
                else if (ArgumentIs(argument, Shutdown))
                {
                    _powerTask = PowerTask.Shutdown;
                }
                else if (ArgumentIs(argument, LogOff))
                {
                    _powerTask = PowerTask.LogOff;
                }
                else if (ArgumentIs(argument, Reboot))
                {
                    _powerTask = PowerTask.Reboot;
                }
                else if (ArgumentIs(argument, Suspend))
                {
                    _powerTask = PowerTask.Suspend;
                }
                else if (ArgumentIs(argument, Hibernate))
                {
                    _powerTask = PowerTask.Hibernate;
                }
                else if (ArgumentIs(argument, UseFileLog))
                {
                    _useFileLog = true;
                }
                else if (ArgumentIs(argument, Auto))
                {
                    Console.Title = Constants.ConsoleBackupTitle;
                    _useFileLog = true;
                    NativeMethods.SetWindowVisibility(false, Constants.ConsoleBackupTitle);
                }
                else if (ArgumentIs(argument, HelpCommand))
                {
                    Console.WriteLine(Resources.UsageVariantsNNbackupExeTaskMyTaskTitleNRunningWithoutParametersOutputsInformationToConsoleNNbackupExeTaskMyTask1TitleTaskMyTask2TitleTaskMyTask3TitleNRunsSeveralTasksOneByOneNNbackupExeTaskMyTaskTitleUsefilelogNOutputsInformationInFileLogNNbackupExeTaskMyTaskTitleShutdownNbackupExeTaskMyTaskTitleLogoffNbackupExeTaskMyTaskTitleSuspendNbackupExeTaskMyTaskTitleRebootNbackupExeTaskMyTaskTitleHibernateNNbackupExeHelpNOutputsBriefHelpN);
                    return false;
                }
                else
                {
                    Console.WriteLine(argument);
                    ShowInvalidUsageAndQuit(Resources.ErrorInvalidCommandParametersSpecified);
                    return false;
                }
            }

            return true;
		}

        /// <summary>
        /// Prepares the backup
        /// </summary>
        /// <returns>True when preparation finished OK, otherwise we should exit</returns>
		public bool Prepare()
        {
            var log = OpenLog();

            if (_backupTaskTitles.Count == 0)
            {
                log.WriteLine(LoggingEvent.Error, Resources.PleaseSpecifyTheBackupTaskTitleUsingTheCommandLineArgument0MyBackupTaskTitleNexampleBackupExe0MyBackupTitle, TaskCommandLineArgument);
                return false;
            }

            var backupTaskStoreService = new BackupTaskStoreService();
            var backupTasks = backupTaskStoreService.Load(_backupTaskTitles, out var missingTasks);
            foreach (var missingTask in missingTasks)
                log.WriteLine(LoggingEvent.Error, Resources.TherereNoBackupTaskWithTitle0, missingTask);

            if (missingTasks.Any())
                return false;
            
            _backup = new BackupProcess(backupTasks.ToList(), _options, log);

            return true;
		}

        /// <summary>
        /// Backups the task
        /// </summary>
		public void Backup()
		{
			_backup.Run();

            if (!_useFileLog && _backup.ErrorsOrWarningsRegistered)
            {
                Console.ReadKey();
            }

            PowerPC.DoTask(_powerTask);
        }

        #endregion

        #region Private Methods

        private static void ShowErrorAndQuit(Exception exception)
        {
            ShowErrorAndQuit(exception.ToString());
        }

        private static void ShowErrorAndQuit(string message)
		{
			Console.WriteLine("\n{0}", message);
			Environment.Exit(-1);
		}

        private static void ShowInvalidUsageAndQuit(string error)
        {
            Console.WriteLine(Resources.UsageVariantsNNbackupExeTaskMyTaskTitleNRunningWithoutParametersOutputsInformationToConsoleNNbackupExeTaskMyTask1TitleTaskMyTask2TitleTaskMyTask3TitleNRunsSeveralTasksOneByOneNNbackupExeTaskMyTaskTitleUsefilelogNOutputsInformationInFileLogNNbackupExeTaskMyTaskTitleShutdownNbackupExeTaskMyTaskTitleLogoffNbackupExeTaskMyTaskTitleSuspendNbackupExeTaskMyTaskTitleRebootNbackupExeTaskMyTaskTitleHibernateNNbackupExeHelpNOutputsBriefHelpN);
            ShowErrorAndQuit(error);
        }

		private void LoadAndValidateOptions()
		{
            PerformCriticalChecks();

            LoadSettings();
		}

        private void LoadSettings()
        {
            try
            {
                _options = ProgramOptionsManager.LoadSettings();
            }
            catch (OptionsException e)
            {
                ShowErrorAndQuit(e);
            }
        }

        private static void PerformCriticalChecks()
        {
            try
            {
                Directories.CriticalFoldersCheck();
                Files.CriticalFilesCheck();
            }
            catch (DirectoryNotFoundException e)
            {
                ShowErrorAndQuit(e);
            }
            catch (FileNotFoundException e)
            {
                ShowErrorAndQuit(e);
            }
        }

        private static bool ArgumentIs(string enteredArg, string expectedArg)
        {
            return string.Compare(enteredArg, expectedArg, true, CultureInfo.InvariantCulture) == 0;
        }

        private LogBase OpenLog()
        {
            LogBase result = null;

            if (_useFileLog)
            {
                try
                {
                    result = new FileLog(_options.LogsFolder, _options.LoggingLevel, true);
                    result.Open();
                }
                catch (LogException e)
                {
                    // "Cannot open file log due to crytical error {0}"
                    ShowErrorAndQuit(string.Format(CultureInfo.InstalledUICulture, Resources.CannotOpenFileLogDueToCryticalError0, e.Message));
                }
            }
            else
            {
                result = new ConsoleLog(_options.LoggingLevel);
                result.Open();
            }
            return result;
        }

        #endregion

        #region IDisposable Implementation

        ~ConsoleBackupController()
        {
            ((IDisposable)this).Dispose();
        }

        bool _isDisposed; // auto: false;
        void IDisposable.Dispose()
        {
            if (!_isDisposed)
            {
                if (_backup != null)
                    _backup.Dispose();

                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }
        
        #endregion

		#region Constants

        const string UseFileLog = "UseFileLog";
		const string HelpCommand = "Help";
		const string Shutdown = "ShutDown";
        const string LogOff = "LogOff";
        const string Suspend = "Suspend";
        const string Hibernate = "Hibernate";
        const string Reboot = "Reboot";
        const string Auto = "Auto";
        const string TaskCommandLineArgument = "Task=";

		#endregion

        #region Fields

        bool _useFileLog;
        ProgramOptions _options;
		BackupProcess _backup;
        readonly List<string> _backupTaskTitles = new List<string>();
        PowerTask _powerTask = PowerTask.None;

        #endregion
    }
}
