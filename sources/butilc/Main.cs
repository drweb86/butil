using BUtil.ConsoleBackup.UI;
using BUtil.ConsoleBackup;
using BUtil.Core;
using BUtil.Core.Misc;
using System;
using Terminal.Gui;

NativeMethods.PreventSleep();
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine(CopyrightInfo.Copyright);
Console.WriteLine();

try
{
    if (args.Length != 0)
    {

        using var controller = new Controller();
        if (!controller.ParseCommandLineArguments(args))
            return 1;

        if (controller.Prepare())
            controller.Backup();

        return 0;
    }
    else
    {
        Application.Init();

        try
        {
            Application.Run(new MyView());
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