using BUtil.Core;
using BUtil.Core.Misc;
using System;

Console.OutputEncoding = System.Text.Encoding.UTF8;
PlatformSpecificExperience.Instance.OsSleepPreventionService.PreventSleep();

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