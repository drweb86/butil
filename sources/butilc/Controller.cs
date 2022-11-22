using System;
using System.Globalization;
using System.IO;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.Core.Logs;
using BUtil.Core;
using BUtil.Core.Misc;
using BUtil.ConsoleBackup.Localization;
using BUtil.Core.BackupModels;
using System.Threading;

namespace BUtil.ConsoleBackup
{
    internal class Controller : IDisposable
    {
        ILog _log;

        public Controller()
        {
            PerformCriticalChecks();

            LoadSettings();
        }

        public bool ParseCommandLineArguments(string[] args)
        {
            args ??= Array.Empty<string>();

            foreach (string argument in args)
            {
                if (argument.StartsWith(TaskCommandLineArgument, StringComparison.InvariantCultureIgnoreCase))
                {
                    _taskName = argument[TaskCommandLineArgument.Length..];
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

        
		public bool Prepare()
        {
            _log = OpenLog();

            if (string.IsNullOrWhiteSpace(_taskName))
            {
                _log.WriteLine(LoggingEvent.Error, string.Format(Resources.PleaseSpecifyTheBackupTaskTitleUsingTheCommandLineArgument0MyBackupTaskTitleNexampleBackupExe0MyBackupTitle, TaskCommandLineArgument));
                return false;
            }

            var backupTaskStoreService = new BackupTaskStoreService();
            var task = backupTaskStoreService.Load(_taskName);
            if (task == null)
            {
                _log.WriteLine(LoggingEvent.Error, string.Format(Resources.TherereNoBackupTaskWithTitle0, _taskName));
                return false;
            }

            _backup = BackupModelStrategyFactory.Create(_log, task);

            return true;
        }

		public void Backup()
        {
            var task = _backup.GetTask(new Core.Events.BackupEvents());
            task.Execute();
            PowerPC.DoTask(_powerTask);
        }

        private static void ShowErrorAndQuit(Exception exception)
        {
            ShowErrorAndQuit(exception.ToString());
        }

        private static void ShowErrorAndQuit(string message)
        {
            Console.Error.WriteLine("\n{0}", message);
            Environment.Exit(-1);
        }

        private static void ShowInvalidUsageAndQuit(string error)
        {
            Console.WriteLine(Resources.UsageVariantsNNbackupExeTaskMyTaskTitleNRunningWithoutParametersOutputsInformationToConsoleNNbackupExeTaskMyTask1TitleTaskMyTask2TitleTaskMyTask3TitleNRunsSeveralTasksOneByOneNNbackupExeTaskMyTaskTitleUsefilelogNOutputsInformationInFileLogNNbackupExeTaskMyTaskTitleShutdownNbackupExeTaskMyTaskTitleLogoffNbackupExeTaskMyTaskTitleSuspendNbackupExeTaskMyTaskTitleRebootNbackupExeTaskMyTaskTitleHibernateNNbackupExeHelpNOutputsBriefHelpN);
            ShowErrorAndQuit(error);
        }

        private void LoadSettings()
        {
            var programOptionsStoreService = new ProgramOptionsStoreService();
            _options = programOptionsStoreService.Load();
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

        private ILog OpenLog()
        {
            var log = new ChainLog(_options);
            try
            {
                log.Open();
            }
            catch (LogException e)
            {
                // "Cannot open file log due to crytical error {0}"
                ShowErrorAndQuit(string.Format(CultureInfo.InstalledUICulture, Resources.CannotOpenFileLogDueToCryticalError0, e.Message));
            }
            return log;
        }

        public void Dispose()
        {
            _log?.Close();
        }

        #region Constants

        const string HelpCommand = "Help";
        const string Shutdown = "ShutDown";
        const string LogOff = "LogOff";
        const string Reboot = "Reboot";
        const string TaskCommandLineArgument = "Task=";

        #endregion

        #region Fields

        ProgramOptions _options;
        IBackupModelStrategy _backup;
        string _taskName;
        PowerTask _powerTask = PowerTask.None;

        #endregion
    }
}
