using BUtil.Core.Logs;
using BUtil.Core.Misc;
using System.Diagnostics;
using System.Text;

namespace BUtil.Windows.Util;

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

    public static bool ExecuteProcess(ILog log, string executable, params string[] arguments)
    {
        log.WriteLine(LoggingEvent.Debug, $"Executing process: {executable}");

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = executable,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            }
        };

        foreach (var argument in arguments)
            process.StartInfo.ArgumentList.Add(argument);

        var stdOutputBuilder = new StringBuilder();
        var stdErrorBuilder = new StringBuilder();
        process.OutputDataReceived += (_, a) => stdOutputBuilder.AppendLine(a.Data);
        process.ErrorDataReceived += (_, a) => stdErrorBuilder.AppendLine(a.Data);

        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();
        process.WaitForExit();

        var stdOutput = stdOutputBuilder.ToString();
        var stdError = stdErrorBuilder.ToString();
        var isSuccess = process.ExitCode == 0;

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
