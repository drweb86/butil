
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using System;
using System.IO;

namespace BUtil.Core.Storages;

public class FailoverStorageWrapper : IStorage
{
    private readonly ILog _log;
    private readonly IStorage _storage;

    public FailoverStorageWrapper(ILog log, IStorage storage)
    {
        _log = log;
        _storage = storage;
    }

    public void Delete(string file)
    {
        ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Delete \"{file}\": {error}"),
            () => _storage.Delete(file));
    }

    public void Dispose()
    {
        _storage.Dispose();
    }

    public void Download(string relativeFileName, string targetFileName)
    {
        ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Download \"{relativeFileName}\" to \"{targetFileName}\": {error}"),
            () => _storage.Download(relativeFileName, targetFileName));
    }

    public bool Exists(string relativeFileName)
    {
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Exists \"{relativeFileName}\": {error}"),
            () => _storage.Exists(relativeFileName));
    }

    public void DeleteFolder(string relativeFolderName)
    {
        ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Delete folder \"{relativeFolderName}\": {error}"),
            () => _storage.DeleteFolder(relativeFolderName));
    }

    public string[] GetFolders(string relativeFolderName, string? mask = null)
    {
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Get folders \"{relativeFolderName}\" by mask \"{mask}\": {error}"),
            () => _storage.GetFolders(relativeFolderName, mask));
    }

    public string? Test()
    {
        return _storage.Test();
    }

    public IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Upload \"{sourceFile}\" to \"{relativeFileName}\": {error}"),
            () => _storage.Upload(sourceFile, relativeFileName));
    }

    public string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Get files \"{relativeFolderName}\" with option \"{option}\": {error}"),
            () => _storage.GetFiles(relativeFolderName, option));
    }

    public DateTime GetModifiedTime(string relativeFileName)
    {
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Get file \"{relativeFileName}\" modified date: {error}"),
            () => _storage.GetModifiedTime(relativeFileName));
    }

    public void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Move file \"{fromRelativeFileName}\" to \"{toRelativeFileName}\": {error}"),
            () => _storage.Move(fromRelativeFileName, toRelativeFileName));
    }
}
