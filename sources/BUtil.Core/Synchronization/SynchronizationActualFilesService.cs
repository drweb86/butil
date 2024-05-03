namespace BUtil.Core.Synchronization;

using BUtil.Core.Hashing;
using System.IO;

class SynchronizationActualFilesService
{
    private readonly IHashService _hashService;

    public SynchronizationActualFilesService(IHashService hashService)
    {
        _hashService = hashService;
    }

    public SynchronizationState Calculate(string folder)
    {
        var fileSystemState = new SynchronizationState();
        var files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var entry = new SynchronizationStateFile(
                file.Substring(folder.Length + 1),
                File.GetLastWriteTimeUtc(file),
                _hashService.GetSha512(file, true),
                new FileInfo(file).Length);

            fileSystemState.FileSystemEntries.Add(entry);
        }

        return fileSystemState;
    }

}
