using BUtil.ConsoleBackup.UI;
using BUtil.ConsoleBackup;
using BUtil.Core;
using BUtil.Core.Misc;
using System;
using Terminal.Gui;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine(CopyrightInfo.Copyright);
Console.WriteLine();

try
{
    NativeMethods.PreventSleep();
    using var controller = new Controller();

    if (args.Length != 0)
    {
        if (!controller.ParseCommandLineArguments(args))
            return 1;

        if (controller.Prepare())
            controller.Backup();

        return 0;
    }
    else
    {
        Application.Init();

        var taskNames = controller.BackupTaskStoreService.GetNames();
        try
        {
            Application.Run(new MainView(controller));
        }
        finally
        {
            Application.Shutdown();
        }
        return 0;
    }
}
catch (Exception e)
{
    ImproveIt.ProcessUnhandledException(e);
    return 1;
}