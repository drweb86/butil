namespace BUtil.Core.Synchronization;

using BUtil.Core.FileSystem;
using BUtil.Core.Storages;
using System.IO;
using System.Text.Json;

class SynchronizationRemoteStateService
{
    private readonly IStorage _storage;

    public SynchronizationRemoteStateService(IStorage storage)
    {
        _storage = storage;
    }

    private readonly string _remoteFile = "Remote State - V1.json";

    public void Save(SynchronizationState localFileState)
    {
        if (_storage.Exists(_remoteFile))
            _storage.Delete(_remoteFile);

        using var tempFolder = new TempFolder();
        var localFile = Path.Combine(tempFolder.Folder, _remoteFile);
        var content = JsonSerializer.Serialize(localFileState, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(localFile, content);

        _storage.Upload(localFile, _remoteFile);
    }
}
