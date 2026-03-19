
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using System;
using System.IO;

namespace BUtil.Core.Storages;

public class FailoverStorageWrapper(ILog log, IStorage storage, int? triesCount) : IStorage
{
    private readonly ILog _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IStorage _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    private readonly int _triesCount = triesCount ?? 10;
    private bool _isDisposed;

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
    }

    public void Delete(string file)
    {
        ThrowIfDisposed();
        ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Delete \"{file}\": {error}"),
            () => _storage.Delete(file),
            _triesCount);
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        if (_isDisposed)
            return;
        _storage.Dispose();
        _isDisposed = true;
    }

    public void Download(string relativeFileName, string targetFileName)
    {
        ThrowIfDisposed();
        ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Download \"{relativeFileName}\" to \"{targetFileName}\": {error}"),
            () => _storage.Download(relativeFileName, targetFileName),
            _triesCount);
    }

    public bool Exists(string relativeFileName)
    {
        ThrowIfDisposed();
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Exists \"{relativeFileName}\": {error}"),
            () => _storage.Exists(relativeFileName),
            _triesCount);
    }

    public void DeleteFolder(string relativeFolderName)
    {
        ThrowIfDisposed();
        ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Delete folder \"{relativeFolderName}\": {error}"),
            () => _storage.DeleteFolder(relativeFolderName),
            _triesCount);
    }

    public string[] GetFolders(string relativeFolderName, string? mask = null)
    {
        ThrowIfDisposed();
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Get folders \"{relativeFolderName}\" by mask \"{mask}\": {error}"),
            () => _storage.GetFolders(relativeFolderName, mask),
            _triesCount);
    }

    public string? Test()
    {
        ThrowIfDisposed();
        return _storage.Test();
    }

    public IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        ThrowIfDisposed();
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Upload \"{sourceFile}\" to \"{relativeFileName}\": {error}"),
            () => _storage.Upload(sourceFile, relativeFileName),
            _triesCount);
    }

    public string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        ThrowIfDisposed();
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Get files \"{relativeFolderName}\" with option \"{option}\": {error}"),
            () => _storage.GetFiles(relativeFolderName, option),
            _triesCount);
    }

    public DateTime GetModifiedTime(string relativeFileName)
    {
        ThrowIfDisposed();
        return ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Get file \"{relativeFileName}\" modified date: {error}"),
            () => _storage.GetModifiedTime(relativeFileName),
            _triesCount);
    }

    public void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        ThrowIfDisposed();
        ExecuteFailover.TryNTimes(
            error => _log.WriteLine(LoggingEvent.Error, $"Move file \"{fromRelativeFileName}\" to \"{toRelativeFileName}\": {error}"),
            () => _storage.Move(fromRelativeFileName, toRelativeFileName),
            _triesCount);
    }
}
