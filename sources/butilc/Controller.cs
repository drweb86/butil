using BUtil.Core;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.TasksTree;
using butilc;
using System;
using System.Globalization;

class Controller
{
    private string _taskName = string.Empty;
    private PowerTask _powerTask = PowerTask.None;

    public Controller ParseCommandLineArguments(string[]? args)
    {
        const string _shutdown = "ShutDown";
        const string _logOff = "LogOff";
        const string _reboot = "Reboot";
        const string _taskCommandLineArgument = "Task=";

        args ??= [];

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
                var log = new ConsoleLog();
                log.WriteLine(LoggingEvent.Debug, argument);
                log.WriteLine(LoggingEvent.Error, Resources.CommandLineArguments_Invalid);
                log.WriteLine(LoggingEvent.Debug, string.Format(Resources.CommandLineArguments_Help, ApplicationLinks.HomePage));
                log.Close(false);
                Environment.Exit(-1);
            }
        }

        if (string.IsNullOrWhiteSpace(_taskName))
        {
            var log = new ConsoleLog();
            log.WriteLine(LoggingEvent.Error, Resources.CommandLineArguments_Invalid);
            log.WriteLine(LoggingEvent.Debug, string.Format(Resources.CommandLineArguments_Help, ApplicationLinks.HomePage));
            log.Close(false);
            Environment.Exit(-1);
        }

        return this;
    }
    private static bool ArgumentIs(string enteredArg, string expectedArg)
    {
        return string.Compare(enteredArg, expectedArg, true, CultureInfo.InvariantCulture) == 0;
    }

    public void Launch()
    {
        var log = new ChainLog(_taskName);
        string? lastMinuteMessage = null;
        bool isSuccess = false;
        log.Open();
        try
        {
            var backupTaskStoreService = new TaskV2StoreService();
            var task = backupTaskStoreService.Load(_taskName, out var isNotFound, out var isNotSupported)!;
            if (isNotFound)
            {
                log.WriteLine(LoggingEvent.Error, string.Format(Resources.Task_Validation_NotFound, _taskName));
                Environment.Exit(-1);
            }
            if (isNotSupported)
            {
                log.WriteLine(LoggingEvent.Error, Resources.Task_Validation_NotSupported);
                Environment.Exit(-1);
            }

            var actualTask = RootTaskFactory.Create(log, task, new BUtil.Core.Events.TaskEvents(), x => lastMinuteMessage = x);
            actualTask.Execute();
            isSuccess = actualTask.IsSuccess;
            if (lastMinuteMessage != null)
                Console.WriteLine(lastMinuteMessage);
        }
        finally
        {
            log.Close(isSuccess);
        }
        PlatformSpecificExperience.Instance.SessionService.DoTask(_powerTask);
    }
}
