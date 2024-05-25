namespace BUtil.Core.Synchronization;

using BUtil.Core.FileSystem;
using BUtil.Core.Hashing;
using BUtil.Core.TasksTree.Synchronization;
using System.IO;
using System.Text.Json;

class SynchronizationLocalStateService
{
    private readonly string _taskName;
    private readonly string _localFolder;
    private readonly string? _subfolder;

    public SynchronizationLocalStateService(string taskName, string localFolder, string? subfolder)
    {
        _taskName = taskName;
        _localFolder = localFolder;
        _subfolder = subfolder;
    }

    private string GetFileName()
    {
        return Path.Combine(Directories.StateFolder, $"{_taskName}-SynchronizationLocalState.v1.json");
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

        var state = JsonSerializer.Deserialize<SynchronizationLocalState>(File.ReadAllText(fileName));
        if (state == null)
            return null;

        if (state.LocalFolder != _localFolder ||
            state.Subfolder != _subfolder)
            return null;

        return state.SynchronizationState;
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

        var wrapper = new SynchronizationLocalState
        {
            Subfolder = _subfolder,
            LocalFolder = _localFolder,
            SynchronizationState = localFileState,
        };


        File.WriteAllText(fileName, JsonSerializer.Serialize(wrapper, new JsonSerializerOptions
        {
            WriteIndented = true,
        }));
    }
}
