using BUtil.ConsoleBackup;
using BUtil.Core;
using BUtil.Core.Misc;
using System;

NativeMethods.PreventSleep();
Console.WriteLine(CopyrightInfo.Copyright);

try
{
    var controller = new Controller();
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
