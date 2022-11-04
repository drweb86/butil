using System.Diagnostics;
using System.Text;
using System.Threading;

namespace BUtil.Core.Misc
{
    static class ProcessHelper
    {
        public static void Execute(
            string executable,
            string args,
            string workingDirectory,

            ProcessPriorityClass processPriority,
            CancellationToken cancellationToken,

            out string stdOutput,
            out string stdError,
            out int returnCode)
        {
            var stdOutputBuilder = new StringBuilder();
            var stdErrorBuilder = new StringBuilder();

            var process = new Process();
            
            process.StartInfo.FileName = executable;
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.OutputDataReceived += (s, a) => stdOutputBuilder.Append(a.Data);
            process.ErrorDataReceived += (s, a) => stdErrorBuilder.Append(a.Data);

            process.Start();
            process.PriorityClass = processPriority;

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.WaitForExitAsync(cancellationToken).Wait();
            if (process.HasExited)
            {
                stdOutput = stdOutputBuilder.ToString();
                stdError = stdErrorBuilder.ToString();
                returnCode = process.ExitCode;
            }
            else
            {
                process.Kill();
                stdOutput = null;
                stdError = "Processed was killed due to cancellation.";
                returnCode = -1;
            }
        }
    }
}
