using BUtil.Core.Logs;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BUtil.Core.Misc
{
    public static class SevenZipProcessHelper
    {
        private static readonly object _lock = new object();

        public static bool Extract(
            ILog log,

            string archive,
            string password,
            string outputDirectory,

            CancellationToken cancellationToken)
        {
            lock (_lock) // 7-zip utilizes all CPU cores, parallel compression will freeze PC.
                // we explicitely avoid it via locking.
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
                    ProcessPriorityClass.Idle,
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
                if (!isSuccess)
                    log.WriteLine(LoggingEvent.Error, "Unpack failed.");
                return isSuccess;
            }
        }

        public static bool CompressFile(
            ILog log,

            string file,
            string password,
            string archive,

            CancellationToken cancellationToken)
        {
            var compressionLevel = GetCompressionLevel(Path.GetExtension(file).ToLowerInvariant());

            if (compressionLevel == 0)
                return CompressFileInternal(log, file, password, archive, cancellationToken);
            else
            {
                lock(_lock)
                    return CompressFileInternal(log, file, password, archive, cancellationToken);
            }
        }

        private static bool CompressFileInternal(
            ILog log,

            string file,
            string password,
            string archive,

            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return false;

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

            ProcessHelper.Execute(FileSystem.Files.SevenZipPacker, arguments, System.IO.Path.GetDirectoryName(FileSystem.Files.SevenZipPacker),
                ProcessPriorityClass.Idle,
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
            if (!isSuccess)
                log.WriteLine(LoggingEvent.Error, "Pack failed.");
            return isSuccess;
        }

        private static int GetCompressionLevel(string extension)
        {
            switch (extension)
            {
                case ".ico":
                case ".jpg":
                case ".png":
                    return 0;

                case ".mp4":
                case ".mov":
                case ".avi":
                case ".flv":
                case ".vob":
                case ".mkv":
                    return 0;

                case ".mp3":
                case ".m4a":
                case ".ogg":
                    return 0;

                case ".rar":
                case ".7z":
                case ".zip":
                    return 0;

                case ".docx":
                case ".pptx":
                case ".xlsx":
                    return 0;

                case ".chm":
                case ".pdf":
                case ".epub":
                    return 0;

                case ".raf":
                    return 5;

                default:
                    return 9;
            }
        }
    }
}
