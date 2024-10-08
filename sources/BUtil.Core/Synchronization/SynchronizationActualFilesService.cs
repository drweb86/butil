﻿namespace BUtil.Core.Synchronization;

using BUtil.Core.Hashing;
using System.IO;

class SynchronizationActualFilesService(ICachedHashService hashService, string folder)
{
    private readonly ICachedHashService _hashService = hashService;
    private readonly string _folder = folder;

    public SynchronizationStateFile CalculateItem(string file)
    {
        return new SynchronizationStateFile(
                file[(_folder.Length + 1)..],
                File.GetLastWriteTimeUtc(file),
                _hashService.GetSha512(file, true),
                new FileInfo(file).Length);
    }
    public SynchronizationState Calculate()
    {
        var fileSystemState = new SynchronizationState();
        var files = Directory.GetFiles(_folder, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var entry = CalculateItem(file);

            fileSystemState.FileSystemEntries.Add(entry);
        }

        return fileSystemState;
    }

}
