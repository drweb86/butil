using System;
using System.Globalization;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Localization;
using BUtil.Core.BackupModels;

namespace BUtil.ConsoleBackup
{
    internal class Controller : IDisposable
    {
        ILog _log;

        public Controller()
        {
            Directories.EnsureFoldersCreated();
        }

        public TaskV2StoreService BackupTaskStoreService => new TaskV2StoreService();

        public bool ParseCommandLineArguments(string[] args)
        {
            args ??= Array.Empty<string>();

            foreach (string argument in args)
            {
                if (argument.StartsWith(_taskCommandLineArgument, StringComparison.InvariantCultureIgnoreCase))
                {
                    _taskName = argument[_taskCommandLineArgument.Length..];
                    continue;
                }
                else if (ArgumentIs(argument, _shutdown))
                {
                    _powerTask = PowerTask.Shutdown;
                }
                else if (ArgumentIs(argument, _logOff))
                {
                    _powerTask = PowerTask.LogOff;
                }
                else if (ArgumentIs(argument, _reboot))
                {
                    _powerTask = PowerTask.Reboot;
                }
                else
                {
                    Console.WriteLine(argument);
                    _log.WriteLine(LoggingEvent.Error, Resources.CommandLineArguments_Invalid);
                    Console.WriteLine(string.Format(Resources.CommandLineArguments_Help, SupportManager.GetLink(SupportRequest.Homepage)));
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
                _log.WriteLine(LoggingEvent.Error, Resources.CommandLineArguments_Invalid);
                Console.WriteLine(string.Format(Resources.CommandLineArguments_Help, SupportManager.GetLink(SupportRequest.Homepage)));
                return false;
            }

            var backupTaskStoreService = new TaskV2StoreService();
            var task = backupTaskStoreService.Load(_taskName);
            if (task == null)
            {
                _log.WriteLine(LoggingEvent.Error, string.Format(Resources.Task_Validation_NotFound, _taskName));
                return false;
            }

            _backup = TaskModelStrategyFactory.Create(_log, task);

            return true;
        }

		public void Backup()
        {
            var task = _backup.GetTask(new Core.Events.TaskEvents());
            task.Execute();
            PowerPC.DoTask(_powerTask);
        }

        private static void ShowErrorAndQuit(string message)
        {
            Console.Error.WriteLine("\n{0}", message);
            Environment.Exit(-1);
        }

        private static void ShowInvalidUsageAndQuit(string error)
        {
            Console.WriteLine(string.Format(Resources.CommandLineArguments_Help, SupportManager.GetLink(SupportRequest.Homepage)));
            ShowErrorAndQuit(error);
        }

        private static bool ArgumentIs(string enteredArg, string expectedArg)
        {
            return string.Compare(enteredArg, expectedArg, true, CultureInfo.InvariantCulture) == 0;
        }

        private ILog OpenLog()
        {
            var log = new ChainLog(_taskName);
            log.Open();
            return log;
        }

        public void Dispose()
        {
            _log?.Close();
        }

        #region Constants

        private const string _shutdown = "ShutDown";
        private const string _logOff = "LogOff";
        private const string _reboot = "Reboot";
        private const string _taskCommandLineArgument = "Task=";

        #endregion

        #region Fields

        private ITaskModelStrategy _backup;
        private string _taskName;
        private PowerTask _powerTask = PowerTask.None;

        #endregion
    }
}
