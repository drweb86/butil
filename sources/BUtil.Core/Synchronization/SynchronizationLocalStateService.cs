namespace BUtil.Core.Synchronization;

using BUtil.Core.FileSystem;
using BUtil.Core.TasksTree.Synchronization;
using System.IO;
using System.Text.Json;

class SynchronizationLocalStateService(string taskName, string localFolder, string? repositorySubfolder)
{
    private readonly string _taskName = taskName;
    private readonly string _localFolder = localFolder;
    private readonly string? _repositorySubfolder = repositorySubfolder;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

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
            FileHelper.EnsureFolderCreated(directory);
        }

        if (!File.Exists(fileName))
            return null;

        var state = JsonSerializer.Deserialize<SynchronizationLocalState>(File.ReadAllText(fileName));
        if (state == null)
            return null;

        if (state.LocalFolder != _localFolder ||
            state.RepositorySubfolder != _repositorySubfolder)
            return null;

        return state.SynchronizationState;
    }

    public void Save(SynchronizationState localFileState)
    {

        var fileName = GetFileName();

        var directory = Path.GetDirectoryName(fileName);
        if (directory != null)
        {
            FileHelper.EnsureFolderCreated(directory);
        }

        if (File.Exists(fileName))
            File.Delete(fileName);

        var wrapper = new SynchronizationLocalState
        {
            RepositorySubfolder = _repositorySubfolder,
            LocalFolder = _localFolder,
            SynchronizationState = localFileState,
        };


        File.WriteAllText(fileName, JsonSerializer.Serialize(wrapper, _jsonSerializerOptions));
    }
}
