using BUtil.Core.Logs;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BUtil.Core.Misc
{
    public static class SevenZipProcessHelper
    {
        private static readonly object _lock = new();

        public static bool Extract(
            ILog log,

            string archive,
            string password,
            string outputDirectory)
        {
            lock (_lock) // 7-zip utilizes all CPU cores, parallel compression will freeze PC.
                // we explicitely avoid it via locking.
            {
                var passwordIsSet = !string.IsNullOrWhiteSpace(password);
                string input = Environment.NewLine; // to handle wrong password
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

                ProcessHelper.Execute(
                    FileSystem.Files.SevenZipPacker, 
                    arguments, 
                    System.IO.Path.GetDirectoryName(FileSystem.Files.SevenZipPacker),
                    true,
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
                    log.WriteLine(LoggingEvent.Debug, "Unpack successfull.");
                if (!isSuccess)
                    log.WriteLine(LoggingEvent.Error, "Unpack failed.");
                return isSuccess;
            }
        }

        public static bool CompressFile(
            ILog log,

            string file,
            string password,
            string archive)
        {
            var compressionLevel = GetCompressionLevel(Path.GetExtension(file).ToLowerInvariant());

            if (compressionLevel == 0)
                return CompressFileInternal(log, file, password, archive);
            else
            {
                lock(_lock)
                    return CompressFileInternal(log, file, password, archive);
            }
        }

        private static bool CompressFileInternal(
            ILog log,

            string file,
            string password,
            string archive)
        {
            var compressionLevel = GetCompressionLevel(Path.GetExtension(file).ToLowerInvariant());
            string arguments;
            if (string.IsNullOrWhiteSpace(password))
            {
                arguments = $@"a -y ""{archive}"" ""{file}"" -t7z -m0=lzma2 -ms=on -mx={compressionLevel}";
                log.WriteLine(LoggingEvent.Debug, $"Compressing \"{file}\" to \"{archive}\"");
            }
            else
            {
                arguments = $@"a -y ""{archive}"" ""{file}"" -p""{password}"" -t7z -m0=lzma2 -ms=on -mx={compressionLevel} -mhe=on";
                log.WriteLine(LoggingEvent.Debug, $"Compressing \"{file}\" to \"{archive}\" with password");
            }

            ProcessHelper.Execute(FileSystem.Files.SevenZipPacker, 
                arguments, 
                System.IO.Path.GetDirectoryName(FileSystem.Files.SevenZipPacker),
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
                log.WriteLine(LoggingEvent.Debug, "Pack successfull.");
            if (!isSuccess)
                log.WriteLine(LoggingEvent.Error, "Pack failed.");
            return isSuccess;
        }

        private static int GetCompressionLevel(string extension)
        {
            return extension switch
            {
                ".ico" or ".jpg" or ".png" => 0,
                ".mp4" or ".mov" or ".avi" or ".flv" or ".vob" or ".mkv" => 0,
                ".mp3" or ".m4a" or ".ogg" => 0,
                ".rar" or ".7z" or ".zip" => 0,
                ".docx" or ".pptx" or ".xlsx" => 0,
                ".chm" or ".pdf" or ".epub" => 0,
                ".raf" => 5,
                _ => 9,
            };
        }
    }
}
