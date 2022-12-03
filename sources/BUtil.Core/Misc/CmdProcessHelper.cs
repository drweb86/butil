using BUtil.Core.Logs;
using System.Diagnostics;

namespace BUtil.Core.Misc
{
    public static class CmdProcessHelper
    {
        public static bool Execute(ILog log, string command)
        {
                log.WriteLine(LoggingEvent.Debug, $"Executing cmd script");

                ProcessHelper.Execute("cmd.exe",
                    $"/C {command}",
                    null,
                    false,
                    ProcessPriorityClass.Idle,

                    out var stdOutput,
                    out var stdError,
                    out var returnCode);

                var isSuccess = returnCode == 0;
                if (!string.IsNullOrWhiteSpace(stdOutput))
                    log.ProcessPackerMessage(stdOutput, isSuccess);
                if (!string.IsNullOrWhiteSpace(stdError))
                    log.ProcessPackerMessage(stdError, isSuccess);
                if (isSuccess)
                    log.WriteLine(LoggingEvent.Debug, "Executing successfull.");
                if (!isSuccess)
                    log.WriteLine(LoggingEvent.Error, "Executing failed.");
                return isSuccess;
        }
    }
}
