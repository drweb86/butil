using System.Diagnostics;

using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;

namespace BUtil.Linux.Services;

class LinuxAptGetArchiver : ILegacyObsoleteArchiver
{
    private readonly ILog _log;
    private static readonly string? _sevenZipFolder;
    private static readonly string? _sevenZipPacker;

    static LinuxAptGetArchiver()
    {
        _sevenZipFolder = Resolve7ZipDirectory();
        if (_sevenZipFolder != null)
            _sevenZipPacker = Path.Combine(_sevenZipFolder, "7z");
    }

    internal LinuxAptGetArchiver(ILog log)
    {
        _log = log;
    }

    private static string? Resolve7ZipDirectory()
    {
        string exe = "7z";
        var result = (Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
            .Split(':') // because its ubuntu, babe.
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

    private static bool Extract(
        ILog log,

        string archive,
        string password,
        string outputDirectory)
    {
        if (_sevenZipPacker == null || _sevenZipFolder == null)
            throw new InvalidDataException("7-zip was not found. Use installation script.'");

        var passwordIsSet = !string.IsNullOrWhiteSpace(password);
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
        if (!isSuccess)
        {
            if (!string.IsNullOrWhiteSpace(stdOutput))
                log.LogProcessOutput(stdOutput, isSuccess);
            if (!string.IsNullOrWhiteSpace(stdError))
                log.LogProcessOutput(stdError, isSuccess);
        }
        if (isSuccess)
            log.WriteLine(LoggingEvent.Debug, "Unpack successfull.");
        if (!isSuccess)
            log.WriteLine(LoggingEvent.Error, "Unpack failed.");
        return isSuccess;
    }
}
