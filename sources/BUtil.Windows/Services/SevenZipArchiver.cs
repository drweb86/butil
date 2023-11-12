using BUtil.Core.Compression;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Windows.Services
{
    class SevenZipArchiver : IArchiver
    {
        private readonly ILog _log;
        private static readonly string? _sevenZipFolder;
        private static readonly string? _sevenZipPacker;

        static SevenZipArchiver()
        {
            _sevenZipFolder = Resolve7ZipDirectory();
            if (_sevenZipFolder != null)
                _sevenZipPacker = Path.Combine(_sevenZipFolder, "7z.exe");
        }

        internal SevenZipArchiver(ILog log) 
        {
            _log = log;
        }

        private static string? Resolve7ZipDirectory()
        {
            var appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "7-zip");
            if (Directory.Exists(appDir))
                return appDir;

            if (System.Environment.Is64BitOperatingSystem)
            {
                appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "7-zip");
                if (Directory.Exists(appDir))
                    return appDir;
            }

            string exe = "7z.exe";
            var result = (Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
                .Split(';')
                .Where(s => File.Exists(Path.Combine(s, exe)))
                .FirstOrDefault();

            return result;
        }

        public bool Extract(
            string archive,
            string password,
            string outputDirectory)
        {
            return Extract(_log, archive, password, outputDirectory);
        }

        public bool CompressFile(
            string file,
            string password,
            string archive)
        {
            return CompressFile(_log, file, password, archive);
        }

        private static readonly object _lock = new();

        private static bool Extract(
            ILog log,

            string archive,
            string password,
            string outputDirectory)
        {
            if (_sevenZipPacker == null || _sevenZipFolder == null)
                throw new InvalidDataException("7-zip was not found.");

            lock (_lock) // 7-zip utilizes all CPU cores, parallel compression will freeze PC.
                         // we explicitely avoid it via locking.
            {
                var passwordIsSet = !string.IsNullOrWhiteSpace(password);
                string input = Environment.NewLine; // to handle wrong password
                string arguments;
                if (!passwordIsSet)
                {
                    arguments = $@"x -y ""{archive}"" -o""{outputDirectory}"" -sccUTF-8";
                    log.WriteLine(LoggingEvent.Debug, $"Extracting \"{archive}\" to \"{outputDirectory}\"");
                }
                else
                {
                    arguments = $@"x -y ""{archive}"" -o""{outputDirectory}"" -p""{password}"" -sccUTF-8";
                    log.WriteLine(LoggingEvent.Debug, $"Extracting \"{archive}\" to \"{outputDirectory}\" with password");
                }

                ProcessHelper.Execute(
                    _sevenZipPacker,
                    arguments,
                    _sevenZipFolder,
                    true,
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
                    log.WriteLine(LoggingEvent.Debug, "Unpack successfull.");
                if (!isSuccess)
                    log.WriteLine(LoggingEvent.Error, "Unpack failed.");
                return isSuccess;
            }
        }

        private static bool CompressFile(
            ILog log,

            string file,
            string password,
            string archive)
        {
            var compressionLevel = ArchiverUtil.GetCompressionLevel(Path.GetExtension(file).ToLowerInvariant());

            if (compressionLevel == 0)
                return CompressFileInternal(log, file, password, archive);
            else
            {
                lock (_lock)
                    return CompressFileInternal(log, file, password, archive);
            }
        }

        private static bool CompressFileInternal(
            ILog log,

            string file,
            string password,
            string archive)
        {
            if (_sevenZipPacker == null || _sevenZipFolder == null)
                throw new InvalidDataException("7-zip was not found.");


            var compressionLevel = ArchiverUtil.GetCompressionLevel(Path.GetExtension(file).ToLowerInvariant());
            string arguments;
            
            if (string.IsNullOrWhiteSpace(password))
            {
                arguments = $@"a -y ""{archive}"" ""{file}"" -t7z -m0=lzma2 -ms=on -mx={compressionLevel} -sccUTF-8 -ssw";
            }
            else
            {
                arguments = $@"a -y ""{archive}"" ""{file}"" -p""{password}"" -t7z -m0=lzma2 -ms=on -mx={compressionLevel} -mhe=on -sccUTF-8 -ssw";
            }

            ProcessHelper.Execute(_sevenZipPacker,
                arguments,
                _sevenZipFolder,
                false,
                ProcessPriorityClass.Idle,
                out var stdOutput,
                out var stdError,
                out var returnCode);

            var isSuccess = returnCode == 0;
            var prefix = string.IsNullOrWhiteSpace(password) ? $"Compressing \"{file}\" to \"{archive}\"" : $"Compressing \"{file}\" to \"{archive}\" with password";
            var eventType = isSuccess ? LoggingEvent.Debug : LoggingEvent.Error;
            var ended = isSuccess ? " successfull" : " failed";
            log.WriteLine(eventType, $"{prefix} {ended}");

            if (!isSuccess)
            {
                if (!string.IsNullOrWhiteSpace(stdOutput))
                    log.LogProcessOutput(stdOutput, isSuccess);
                if (!string.IsNullOrWhiteSpace(stdError))
                    log.LogProcessOutput(stdError, isSuccess);
            }

            return isSuccess;
        }
    }
}
