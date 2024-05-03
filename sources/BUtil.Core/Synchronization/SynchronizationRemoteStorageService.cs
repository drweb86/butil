using System.IO;

namespace BUtil.Core.Synchronization;

class SynchronizationRemoteStorageService
{
    private readonly string _remoteStorageFolder;

    public SynchronizationRemoteStorageService(string remoteStorageFolder)
    {
        _remoteStorageFolder = remoteStorageFolder;
    }

    public void Upload(string localFolder, string relativeFileName)
    {
        var localFile = Path.Combine(localFolder, relativeFileName);
        var remoteFile = Path.Combine(_remoteStorageFolder, relativeFileName);

        var remoteFolder = Path.GetDirectoryName(remoteFile);
        if (remoteFolder != null)
        {
            if (!Directory.Exists(remoteFolder))
                Directory.CreateDirectory(remoteFolder);
        }
        File.Copy(localFile, remoteFile, true);
    }

    internal void Delete(string relativeFileName)
    {
        var remoteFile = Path.Combine(_remoteStorageFolder, relativeFileName);
        File.Delete(remoteFile);
    }

    internal void Download(string localFolder, string relativeFileName)
    {
        var localFile = Path.Combine(localFolder, relativeFileName);
        var remoteFile = Path.Combine(_remoteStorageFolder, relativeFileName);

        var localFolderStr = Path.GetDirectoryName(localFile);
        if (localFolderStr != null)
        {
            if (!Directory.Exists(localFolderStr))
                Directory.CreateDirectory(localFolderStr);
        }
        File.Copy(remoteFile, localFile, true);
    }

    internal bool Exists(string fileName)
    {
        var remoteFile = Path.Combine(_remoteStorageFolder, fileName);
        return File.Exists(remoteFile);
    }

    internal string ReadAllText(string fileName)
    {
        var remoteFile = Path.Combine(_remoteStorageFolder, fileName);
        return File.ReadAllText(remoteFile);
    }

    internal void WriteAllText(string fileName, string content)
    {
        var remoteFile = Path.Combine(_remoteStorageFolder, fileName);
        File.WriteAllText(remoteFile, content);
    }
}
