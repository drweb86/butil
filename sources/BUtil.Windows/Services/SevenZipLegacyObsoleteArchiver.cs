using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Windows.Services;

class SevenZipLegacyObsoleteArchiver : ILegacyObsoleteArchiver
{
    private readonly ILog _log;
    private static readonly string? _sevenZipFolder;
    private static readonly string? _sevenZipPacker;

    static SevenZipLegacyObsoleteArchiver()
    {
        _sevenZipFolder = Resolve7ZipDirectory();
        if (_sevenZipFolder != null)
            _sevenZipPacker = Path.Combine(_sevenZipFolder, "7z.exe");
    }

    internal SevenZipLegacyObsoleteArchiver(ILog log)
    {
        _log = log;
    }

    private static string? Resolve7ZipDirectory()
    {
        var appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "7-zip");
        if (Directory.Exists(appDir))
            return appDir;

        var sevenZip = Path.Combine(Directories.BinariesDir, "7-zip");
        if (Directory.Exists(sevenZip))
            return sevenZip;

        return null;
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
            throw new InvalidDataException("7-zip was not found.");

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
