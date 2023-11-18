
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using System.Diagnostics;

namespace BUtil.Windows.Utils
{
    public static class WindowsCmdProcessHelper
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
                    log.LogProcessOutput(stdOutput, isSuccess);
                if (!string.IsNullOrWhiteSpace(stdError))
                    log.LogProcessOutput(stdError, isSuccess);
                if (isSuccess)
                    log.WriteLine(LoggingEvent.Debug, "Executing successfull.");
                if (!isSuccess)
                    log.WriteLine(LoggingEvent.Error, "Executing failed.");
                return isSuccess;
        }
    }
}
