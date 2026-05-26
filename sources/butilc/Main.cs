using BUtil.Core;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// First access to Instance triggers platform experience initialization (registers all storages).
PlatformSpecificExperience.Instance.OsSleepPreventionService.PreventSleep();
new MigrationService(new LocalFileSystem()).RunAll();

Console.WriteLine(CopyrightInfo.Copyright);
Console.WriteLine();

try
{
    new Controller()
        .ParseCommandLineArguments(args)
        .Launch();
}
catch (Exception e)
{
    ImproveIt.ProcessUnhandledException(e);
    Environment.Exit(-1);
}
