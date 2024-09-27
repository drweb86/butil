
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using System;
using System.IO;

namespace BUtil.Core.Storages;

public class FailoverStorageWrapper(ILog log, IStorage storage, int? triesCount) : IStorage
{
    private readonly ILog _log = log;
    private readonly IStorage _storage = storage;
    private readonly int _triesCount = triesCount ?? 10;

    public void Delete(string file)
    {
        ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Delete \"{file}\": {error}"),
            () => _storage.Delete(file));
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        _storage.Dispose();
    }

    public void Download(string relativeFileName, string targetFileName)
    {
        ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Download \"{relativeFileName}\" to \"{targetFileName}\": {error}"),
            () => _storage.Download(relativeFileName, targetFileName),
            _triesCount);
    }

    public bool Exists(string relativeFileName)
    {
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Exists \"{relativeFileName}\": {error}"),
            () => _storage.Exists(relativeFileName),
            _triesCount);
    }

    public void DeleteFolder(string relativeFolderName)
    {
        ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Delete folder \"{relativeFolderName}\": {error}"),
            () => _storage.DeleteFolder(relativeFolderName),
            _triesCount);
    }

    public string[] GetFolders(string relativeFolderName, string? mask = null)
    {
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Get folders \"{relativeFolderName}\" by mask \"{mask}\": {error}"),
            () => _storage.GetFolders(relativeFolderName, mask),
            _triesCount);
    }

    public string? Test()
    {
        return _storage.Test();
    }

    public IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Upload \"{sourceFile}\" to \"{relativeFileName}\": {error}"),
            () => _storage.Upload(sourceFile, relativeFileName),
            _triesCount);
    }

    public string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Get files \"{relativeFolderName}\" with option \"{option}\": {error}"),
            () => _storage.GetFiles(relativeFolderName, option),
            _triesCount);
    }

    public DateTime GetModifiedTime(string relativeFileName)
    {
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Get file \"{relativeFileName}\" modified date: {error}"),
            () => _storage.GetModifiedTime(relativeFileName),
            _triesCount);
    }

    public void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Move file \"{fromRelativeFileName}\" to \"{toRelativeFileName}\": {error}"),
            () => _storage.Move(fromRelativeFileName, toRelativeFileName),
            _triesCount);
    }
}
