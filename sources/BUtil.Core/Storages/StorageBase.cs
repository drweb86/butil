
using BUtil.Core.Logs;
using System;
using System.IO;

namespace BUtil.Core.Storages;

public abstract class StorageBase<TStorageSettings>(ILog log, TStorageSettings settings) : IStorage
{
    protected readonly ILog Log = log;
    protected readonly TStorageSettings Settings = settings;

    public abstract IStorageUploadResult Upload(string sourceFile, string relativeFileName);
    public abstract string? Test();

    public abstract void Delete(string relativeFileName);
    public abstract void DeleteFolder(string relativeFolderName);
    public abstract string[] GetFolders(string relativeFolderName, string? mask = null);
    public abstract string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly);
    public abstract DateTime GetModifiedTime(string relativeFileName);
    public abstract void Download(string relativeFileName, string targetFileName);
    public abstract bool Exists(string relativeFileName);
    public abstract void Move(string fromRelativeFileName, string toRelativeFileName);
    public abstract void Dispose();
}
