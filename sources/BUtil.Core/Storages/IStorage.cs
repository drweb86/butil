
using System;
using System.IO;

namespace BUtil.Core.Storages;

public interface IStorage : IDisposable
{
    IStorageUploadResult Upload(string sourceFile, string relativeFileName);
    void Delete(string relativeFileName);
    void Move(string fromRelativeFileName, string toRelativeFileName);
    void Download(string relativeFileName, string targetFileName);
    void Download(Stream outputStream, string relativeFileName);
    bool Exists(string relativeFileName);
    void DeleteFolder(string relativeFolderName);
    string[] GetFolders(string relativeFolderName, string? mask = null);
    string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly);
    DateTime GetModifiedTime(string relativeFileName);
    string? Test();
}
