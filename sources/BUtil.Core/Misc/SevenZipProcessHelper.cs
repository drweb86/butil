using BUtil.Core.Logs;
using System.Diagnostics;
using System.Threading;

namespace BUtil.Core.Misc
{
    public static class SevenZipProcessHelper
    {
        public static bool Extract(
            ILog log,

            string archive,
            string password,
            string outputDirectory,

            ProcessPriorityClass processPriority,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return false;

            var passwordIsSet = !string.IsNullOrWhiteSpace(password);
            string arguments;
            if (!passwordIsSet)
            {
                arguments = $@"x -y ""{archive}"" -o""{outputDirectory}""";
                log.WriteLine(LoggingEvent.Debug, $"Extracting \"{archive}\" to \"{outputDirectory}\"");
            }
            else
            {
                arguments = $@"x -y ""{archive}"" -o""{outputDirectory}"" -p""{password}""";
                log.WriteLine(LoggingEvent.Debug, $"Extracting \"{archive}\" to \"{outputDirectory}\" with password");
            }

            ProcessHelper.Execute(FileSystem.Files.SevenZipPacker, arguments, System.IO.Path.GetDirectoryName(FileSystem.Files.SevenZipPacker),
                processPriority,
                cancellationToken,
                out var stdOutput,
                out var stdError,
                out var returnCode);

            var isSuccess = returnCode == 0;
            if (!string.IsNullOrWhiteSpace(stdOutput))
                log.ProcessPackerMessage(stdOutput, isSuccess);
            if (!string.IsNullOrWhiteSpace(stdError))
                log.ProcessPackerMessage(stdError, isSuccess);
            if (isSuccess)
                log.WriteLine(LoggingEvent.Debug, "Unpack successfull.");
            if (isSuccess)
                log.WriteLine(LoggingEvent.Error, "Unpack failed.");
            return isSuccess;
        }

        public static bool CompressFile(
            ILog log,

            string file,
            string password,
            string archive,

            ProcessPriorityClass processPriority,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return false;

            string arguments;
            if (string.IsNullOrWhiteSpace(password))
            {
                arguments = $@"a -y ""{archive}"" ""{file}"" -t7z -m0=lzma2 -ms=on -mx=9";
                log.WriteLine(LoggingEvent.Debug, $"Compressing \"{file}\" to \"{archive}\"");
            }
            else
            {
                arguments = $@"a -y ""{archive}"" ""{file}"" -p""{password}"" -t7z -m0=lzma2 -ms=on -mx=9 -mhe=on";
                log.WriteLine(LoggingEvent.Debug, $"Compressing \"{file}\" to \"{archive}\" with password");
            }

            ProcessHelper.Execute(FileSystem.Files.SevenZipPacker, arguments, System.IO.Path.GetDirectoryName(FileSystem.Files.SevenZipPacker),
                processPriority,
                cancellationToken,
                out var stdOutput,
                out var stdError,
                out var returnCode);

            var isSuccess = returnCode == 0;
            if (!string.IsNullOrWhiteSpace(stdOutput))
                log.ProcessPackerMessage(stdOutput, isSuccess);
            if (!string.IsNullOrWhiteSpace(stdError))
                log.ProcessPackerMessage(stdError, isSuccess);
            if (isSuccess)
                log.WriteLine(LoggingEvent.Debug, "Pack successfull.");
            if (isSuccess)
                log.WriteLine(LoggingEvent.Error, "Pack failed.");
            return isSuccess;
        }
    }
}
