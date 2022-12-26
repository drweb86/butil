using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using System.Diagnostics;
using System.IO;

namespace BUtil.Core.Misc
{
    public static class PowershellProcessHelper
    {
        public static bool Execute(ILog log, string script)
        {
            using (var tempDir = new TempFolder())
            {
                var scriptFile = Path.Combine(tempDir.Folder, "script.ps1");
                File.WriteAllText(scriptFile, script);

                log.WriteLine(LoggingEvent.Debug, $"Executing powershell script");

                ProcessHelper.Execute("powershell.exe",
                    $"& \"{scriptFile}\"",
                    null,
                    false,
                    ProcessPriorityClass.Idle,
                    
                    out var stdOutput,
                    out var stdError,
                    out var returnCode);

                var isSuccess = returnCode  == 0;
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
}
