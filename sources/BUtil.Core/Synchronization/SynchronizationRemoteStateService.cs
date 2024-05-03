namespace BUtil.Core.Synchronization;

using System.Text.Json;

class SynchronizationRemoteStateService
{
    private readonly SynchronizationRemoteStorageService _remoteStorageService;

    public SynchronizationRemoteStateService(SynchronizationRemoteStorageService remoteStorageService)
    {
        _remoteStorageService = remoteStorageService;
    }

    private string GetFileName()
    {
        return $"remote file state - V1.json";
    }

    public SynchronizationState? Load()
    {
        var fileName = GetFileName();
        if (!_remoteStorageService.Exists(fileName))
            return null;

        var content = _remoteStorageService.ReadAllText(fileName);

        return JsonSerializer.Deserialize<SynchronizationState>(content);
    }

    public void Save(SynchronizationState localFileState)
    {
        var fileName = GetFileName();

        if (_remoteStorageService.Exists(fileName))
            _remoteStorageService.Delete(fileName);

        _remoteStorageService.WriteAllText(fileName, JsonSerializer.Serialize(localFileState, new JsonSerializerOptions
        {
            WriteIndented = true,
        }));
    }
}
