namespace BUtil.Core.Synchronization;

using BUtil.Core.Hashing;
using System;
using System.IO;
using System.Text.Json;

class SynchronizationLocalStateService
{
    private readonly string _taskId;

    public SynchronizationLocalStateService(IHashService hashService, string taskName, string syncFolder)
    {
        _taskId = taskName + " " + hashService.GetSha512(syncFolder);
    }

    private string GetFileName()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BUtil", "POC", $"{_taskId} - local file state - V1.json");
    }

    public SynchronizationState? Load()
    {
        var fileName = GetFileName();

        var directory = Path.GetDirectoryName(fileName);
        if (directory != null)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        if (!File.Exists(fileName))
            return null;

        return JsonSerializer.Deserialize<SynchronizationState>(File.ReadAllText(fileName));
    }

    public void Save(SynchronizationState localFileState)
    {
        var fileName = GetFileName();

        var directory = Path.GetDirectoryName(fileName);
        if (directory != null)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        if (File.Exists(fileName))
            File.Delete(fileName);

        File.WriteAllText(fileName, JsonSerializer.Serialize(localFileState, new JsonSerializerOptions
        {
            WriteIndented = true,
        }));
    }
}
