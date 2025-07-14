using BUtil.Core;
using BUtil.Core.Misc;
using System;
using butilc;
using BUtil.Core.TasksTree;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;

Console.OutputEncoding = System.Text.Encoding.UTF8;
PlatformSpecificExperience.Instance.OsSleepPreventionService.PreventSleep();

Console.WriteLine(CopyrightInfo.Copyright);
Console.WriteLine();

try
{
    if (args != null && args.Length > 0 && (args[0] == "send" || args[0] == "serve"))
    {
        DoTransfer(args);
        return;
    }

    new Controller()
        .ParseCommandLineArguments(args)
        .Launch();
}
catch (Exception e)
{
    ImproveIt.ProcessUnhandledException(e);
    Environment.Exit(-1);
}

static void DoTransfer(string[] args)
{
    var task = new TaskV2 { Name = "butilc" };
    var log = new ChainLog(task.Name);

    ITaskModelOptionsV2 options;
    if (args[0] == "send")
    {
        if (args.Length != 5)
        {
            PrintUsage(log);
            return;
        }
        string sourceFolder = args[1];
        string ip = args[2];
        int port = int.Parse(args[3]);
        string password = args[4];
        options = new FileSenderTransferModelOptionsV2(sourceFolder, FileSenderDirection.ToServer, ip, port, password);
    }
    else if (args[0] == "serve")
    {
        if (args.Length != 4)
        {
            PrintUsage(log);
            return;
        }
        string outputFolder = args[1];
        int port = int.Parse(args[2]);
        string password = args[3];
        options = new FileSenderServerModelOptionsV2(outputFolder, FileSenderServerPermissions.ReadWrite, password, port);
    }
    else
    {
        PrintUsage(log);
        return;
    }

    task.Model = options!;
    string? lastMinuteMessage = null;
    if (!RootTaskFactory.TryVerify(log, options, true, out lastMinuteMessage))
    {
        log.WriteLine(LoggingEvent.Error, lastMinuteMessage);
        log.Close(false);
        return;
    }
    var rootTask = RootTaskFactory.Create(log, task, new BUtil.Core.Events.TaskEvents(), x => lastMinuteMessage = x);
    rootTask.Execute();
    if (lastMinuteMessage != null)
        Console.WriteLine(lastMinuteMessage);
    log.Close(rootTask.IsSuccess);
}

static void PrintUsage(ILog log)
{
    log.WriteLine(LoggingEvent.Error, Resources.CommandLineArguments_Invalid);
    log.WriteLine(LoggingEvent.Debug, string.Format(Resources.CommandLineArguments_Help, ApplicationLinks.HomePage));
    log.Close(false);
}
