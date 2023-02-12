using BUtil.ConsoleBackup;
using BUtil.Core;
using BUtil.Core.Misc;
using System;

NativeMethods.PreventSleep();
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine(CopyrightInfo.Copyright);
Console.WriteLine();

try
{
    using var controller = new Controller();
    if (!controller.ParseCommandLineArguments(args))
        return 1;

    if (controller.Prepare())
        controller.Backup();

    return 0;
}
catch (Exception e)
{
    ImproveIt.ProcessUnhandledException(e);
    return 1;
}
