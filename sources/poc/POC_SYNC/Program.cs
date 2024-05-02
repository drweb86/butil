// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

Console.WriteLine("POC sync with auto conflict resolution");

// TB state
var hiveFolder = @"E:\TEST\Hive";
var syncFolder = @"E:\TEST\Sync";

void Sync(string taskName, string hiveFolder, string syncFolder)
{
    var remoteStorageService = new RemoteStorageService(hiveFolder);

    var actualStoredStateService = new ActualStoredStateService(taskName, syncFolder);
    var actualStoredState = actualStoredStateService.Load();

    var actualStateService = new ActualStateService();
    var actualState = actualStateService.Load(syncFolder);

    var remoteStateService = new RemoteStateService(remoteStorageService);
    var remoteState = remoteStateService.Load();

    if (remoteState == null)
    {
        foreach (var item in actualState.FileSystemEntries)
        {
            remoteStorageService.Upload(syncFolder, item.RelativeFileName);
        }
        actualStoredStateService.Save(actualState);
        remoteStateService.Save(actualState);
        return;
    }

    if (actualStoredState == null)
    {
        foreach (var item in remoteState.FileSystemEntries)
        {
            remoteStorageService.Download(syncFolder, item.RelativeFileName);
        }
        actualStoredStateService.Save(remoteState);
        actualStoredState = actualStoredStateService.Load();
        actualState = actualStateService.Load(syncFolder);
    }

    var syncService = new SyncService();
    var syncItems = syncService.Decide(actualStoredState, actualState, remoteState);

    if (syncItems.Any(x => x.ActualFileAction != ActionType.DoNothing))
    {
        foreach (var item in syncItems)
        {
            switch (item.ActualFileAction)
            {
                case ActionType.DoNothing:
                    break;
                case ActionType.Delete:
                    File.Delete(Path.Combine(syncFolder, item.RelativeFileName));
                    break;
                case ActionType.Create:
                case ActionType.Update:
                    remoteStorageService.Download(syncFolder, item.RelativeFileName);
                    break;

            }
        }
    }

    if (syncItems.Any(x => x.RemoteAction != ActionType.DoNothing))
    {
        foreach (var item in syncItems)
        {
            switch (item.RemoteAction)
            {
                case ActionType.DoNothing:
                    break;
                case ActionType.Delete:
                    remoteStorageService.Delete(item.RelativeFileName);
                    break;
                case ActionType.Create:
                case ActionType.Update:
                    remoteStorageService.Upload(syncFolder, item.RelativeFileName);
                    break;

            }
        }
    }

    if (syncItems.Any(x => x.RemoteAction != ActionType.DoNothing) ||
        syncItems.Any(x => x.ActualFileAction != ActionType.DoNothing))
    {
        var state = actualStateService.Load(syncFolder);
        actualStoredStateService.Save(state);
        remoteStateService.Save(state);
    }
}

Sync("task-name", hiveFolder, syncFolder);


enum SyncItemRelation
{
    NotChanged,
    Created,
    Changed,
    Deleted
}

enum ActionType
{
    DoNothing,
    Delete,
    Update,
    Create
}

[DebuggerDisplay("{RelativeFileName}: ActualFileAction = {ActualFileAction}, ActualFileAction={ActualFileAction}")]
class SyncItem
{
    public string RelativeFileName { get; set; }

    // facts
    public FileSystemEntry? ActualFile { get; set; }
    public FileSystemEntry? LocalState { get; set; }
    public FileSystemEntry? RemoteState { get; set; }

    // relations
    public SyncItemRelation ActualFileToLocalStateRelation { get; set; }
    public SyncItemRelation RemoteStateToLocalStateRelation { get; set; }

    // actions
    public bool ExistsLocally { get; set; }
    public ActionType ActualFileAction { get; set; }
    public ActionType RemoteAction { get; set; }
}

class RemoteStorageService
{
    private readonly string _remoteStorageFolder;

    public RemoteStorageService(string remoteStorageFolder)
    {
        _remoteStorageFolder = remoteStorageFolder;
    }

    public void Upload(string localFolder, string relativeFileName)
    {
        var localFile = Path.Combine(localFolder, relativeFileName);
        var remoteFile = Path.Combine(_remoteStorageFolder, relativeFileName);

        var remoteFolder = Path.GetDirectoryName(remoteFile);
        if (!Directory.Exists(remoteFolder))
            Directory.CreateDirectory(remoteFolder);
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
        if (!Directory.Exists(localFolderStr))
            Directory.CreateDirectory(localFolderStr);
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

class SyncService
{
    public List<SyncItem> Decide(SyncState? storedSyncFolderState, SyncState syncFolderState, SyncState remoteSyncState)
    {
        var items = new Dictionary<string, SyncItem>();

        // fill data.
        foreach (var item in syncFolderState.FileSystemEntries)
        {
            items.Add(item.RelativeFileName, new SyncItem
            {
                RelativeFileName = item.RelativeFileName,
                ActualFile = item,
            });
        }

        if (storedSyncFolderState != null)
        {
            foreach (var item in storedSyncFolderState.FileSystemEntries)
            {
                if (!items.ContainsKey(item.RelativeFileName))
                    items.Add(item.RelativeFileName, new SyncItem { RelativeFileName = item.RelativeFileName });

                items[item.RelativeFileName].LocalState = item;
            }
        }

        foreach (var item in remoteSyncState.FileSystemEntries)
        {
            if (!items.ContainsKey(item.RelativeFileName))
                items.Add(item.RelativeFileName, new SyncItem { RelativeFileName = item.RelativeFileName });

            items[item.RelativeFileName].RemoteState = item;
        }

        // build relationships

        foreach (var pair in items)
        {
            pair.Value.ExistsLocally = pair.Value.ActualFile != null;
            pair.Value.ActualFileToLocalStateRelation = ResolveRelation(
                pair.Value.ActualFile,
                pair.Value.LocalState);
            pair.Value.RemoteStateToLocalStateRelation = ResolveRelation(
                pair.Value.RemoteState,
                pair.Value.LocalState);
        }

        // actions resolving.

        foreach (var pair in items)
        {
            var item = pair.Value;

            item.ActualFileAction = ActionType.DoNothing;
            item.RemoteAction = ActionType.DoNothing;

            if (item.ExistsLocally &&
                item.ActualFileToLocalStateRelation == SyncItemRelation.NotChanged &&
                item.RemoteStateToLocalStateRelation == SyncItemRelation.Created)
            {
                item.ActualFileAction = ActionType.Delete;
                item.RemoteAction = ActionType.Create;
            }

            if (item.ExistsLocally &&
                item.ActualFileToLocalStateRelation == SyncItemRelation.NotChanged &&
                item.RemoteStateToLocalStateRelation == SyncItemRelation.Changed)
            {
                item.ActualFileAction = ActionType.Delete;
                item.RemoteAction = ActionType.Update;
            }

            if (item.ExistsLocally &&
                item.ActualFileToLocalStateRelation == SyncItemRelation.NotChanged &&
                item.RemoteStateToLocalStateRelation == SyncItemRelation.Deleted)
            {
                item.ActualFileAction = ActionType.Delete;
            }

            if (item.ExistsLocally &&
                item.ActualFileToLocalStateRelation == SyncItemRelation.Created &&
                (item.RemoteStateToLocalStateRelation == SyncItemRelation.Created ||
                item.RemoteStateToLocalStateRelation == SyncItemRelation.Changed))
            {
                if (item.RemoteState.ModifiedAtUtc > item.ActualFile.ModifiedAtUtc) 
                {
                    item.ActualFileAction = ActionType.Update;
                }
                else
                {
                    item.RemoteAction = ActionType.Update;
                }
            }

            if (item.ExistsLocally &&
                item.ActualFileToLocalStateRelation == SyncItemRelation.Created &&
                item.RemoteStateToLocalStateRelation == SyncItemRelation.Deleted)
            {
                item.RemoteAction = ActionType.Create;
            }

            if (item.ExistsLocally &&
                item.ActualFileToLocalStateRelation == SyncItemRelation.Changed &&
                item.RemoteStateToLocalStateRelation == SyncItemRelation.NotChanged)
            {
                item.RemoteAction = ActionType.Update;
            }

            if (item.ExistsLocally &&
                item.ActualFileToLocalStateRelation == SyncItemRelation.Changed &&
                (item.RemoteStateToLocalStateRelation == SyncItemRelation.Created ||
                item.RemoteStateToLocalStateRelation == SyncItemRelation.Changed))
            {
                if (item.RemoteState.ModifiedAtUtc > item.ActualFile.ModifiedAtUtc)
                {
                    item.ActualFileAction = ActionType.Update;
                }
                else
                {
                    item.RemoteAction = ActionType.Update;
                }
            }

            if (item.ExistsLocally &&
                item.ActualFileToLocalStateRelation == SyncItemRelation.Changed &&
                item.RemoteStateToLocalStateRelation == SyncItemRelation.Deleted)
            {
                item.RemoteAction = ActionType.Create;
            }

            if (!item.ExistsLocally &&
                item.ActualFileToLocalStateRelation == SyncItemRelation.Deleted &&
                (item.RemoteStateToLocalStateRelation == SyncItemRelation.Created ||
                item.RemoteStateToLocalStateRelation == SyncItemRelation.Changed))
            {
                item.RemoteAction = ActionType.Create;
            }
        }

        return items.Values.ToList();
    }
    private SyncItemRelation ResolveRelation(FileSystemEntry? primary, FileSystemEntry? secondary)
    {
        if (primary == null)
        {
            if (secondary == null)
                return SyncItemRelation.NotChanged;

            return SyncItemRelation.Deleted;
        }

        if (secondary == null)
        {
            return SyncItemRelation.Created;
        }

        if (primary.Equal(secondary))
        {
            return SyncItemRelation.NotChanged;
        }

        return SyncItemRelation.Changed;
    }
}


class FileSystemEntry
{
    public string RelativeFileName { get; set; }
    public DateTime ModifiedAtUtc { get; set; }
    public string Sha512 { get; set; }
    public long Size { get; set; }

    public bool Equal(FileSystemEntry other, bool excludeModifiedAtUtc = true)
    {
        return other.RelativeFileName.Equals(RelativeFileName) &&
            other.Size.Equals(Size) &&
            (excludeModifiedAtUtc || (!excludeModifiedAtUtc && other.ModifiedAtUtc.Equals(ModifiedAtUtc)) )&&
            other.Sha512.Equals(Sha512);
    }
}

class SyncState
{
    public List<FileSystemEntry> FileSystemEntries { get; set; } = new List<FileSystemEntry>();
}

class ActualStateService
{
    public SyncState Load(string folder)
    { 
        var fileSystemState = new SyncState();
        var files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var entry = new FileSystemEntry();

            entry.RelativeFileName = file.Substring(folder.Length + 1);
            entry.ModifiedAtUtc = File.GetLastWriteTimeUtc(file);
            entry.Size = new FileInfo(file).Length;
            entry.Sha512 = GetSha512Internal(file);

            fileSystemState.FileSystemEntries.Add(entry);
        }

        return fileSystemState;
    }

    private static string GetSha512Internal(string file)
    {
        using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 16 * 1024 * 1024);
        using var sha512Hash = SHA512.Create();

        var hash = sha512Hash.ComputeHash(fileStream);
        return HashToString(hash);
    }

    private static string HashToString(byte[] hash)
    {
        var sBuilder = new StringBuilder();

        for (int i = 0; i < hash.Length; i++)
        {
            sBuilder.Append(hash[i].ToString("x2"));
        }

        return sBuilder.ToString();
    }
}

class ActualStoredStateService
{
    private readonly string _taskId;

    public ActualStoredStateService(string taskName, string syncFolder)
    {
        _taskId = taskName + " " + SHA512(syncFolder);
    }

    private static string SHA512(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        using (var hash = System.Security.Cryptography.SHA512.Create())
        {
            var hashedInputBytes = hash.ComputeHash(bytes);

            // Convert to text
            // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
            var hashedInputStringBuilder = new System.Text.StringBuilder(128);
            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            return hashedInputStringBuilder.ToString();
        }
    }

    private string GetFileName()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BUtil", "POC", $"{_taskId} - local file state - V1.json");
    }

    public SyncState Load()
    {
        var fileName = GetFileName();

        var directory = Path.GetDirectoryName(fileName);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        if (!File.Exists(fileName))
            return null;

        return JsonSerializer.Deserialize<SyncState>(File.ReadAllText(fileName));
    }

    public void Save(SyncState localFileState)
    {
        var fileName = GetFileName();

        var directory = Path.GetDirectoryName(fileName);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        if (File.Exists(fileName))
            File.Delete(fileName);

        File.WriteAllText(fileName, JsonSerializer.Serialize(localFileState, new JsonSerializerOptions
        {
            WriteIndented = true,
        }));
    }
}


class RemoteStateService
{
    private readonly RemoteStorageService _remoteStorageService;

    public RemoteStateService(RemoteStorageService remoteStorageService)
    {
        _remoteStorageService = remoteStorageService;
    }

    private string GetFileName()
    {
        return $"remote file state - V1.json";
    }

    public SyncState Load()
    {
        var fileName = GetFileName();
        if (!_remoteStorageService.Exists(fileName))
            return null;

        var content = _remoteStorageService.ReadAllText(fileName);

        return JsonSerializer.Deserialize<SyncState>(content);
    }

    public void Save(SyncState localFileState)
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
