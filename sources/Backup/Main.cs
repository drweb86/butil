using BUtil.ConsoleBackup;
using BUtil.Core;
using BUtil.Core.Misc;
using System;

ImproveIt.InitInfrastructure(false);
Console.WriteLine(CopyrightInfo.Copyright);

try
{
    using var controller = new Controller();
    if (!controller.ParseCommandLineArguments(args))
        return;

    if (controller.Prepare())
        controller.Backup();
}
catch (Exception e)
{
    ImproveIt.ProcessUnhandledException(e);
}
