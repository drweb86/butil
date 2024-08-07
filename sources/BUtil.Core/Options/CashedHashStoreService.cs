﻿
using BUtil.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BUtil.Core.Options;

public class CachedHash
{
    public string File { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string Sha512 { get; set; } = string.Empty;
    public DateTime LastWriteTimeUtc { get; set; }
    public long Size { get; set; }
}

public interface ICashedHashStoreService
{
    IEnumerable<CachedHash>? Load();
    void Save(IEnumerable<CachedHash> cachedHashes);
}

public class CashedHashStoreService : ICashedHashStoreService
{
    private readonly string _folder;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };
    public CashedHashStoreService()
    {
#if DEBUG
        _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Cache-V1 - DEBUG");
#else
        _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Cache-V1");
#endif
        FileHelper.EnsureFolderCreated(_folder);
    }

    public IEnumerable<CachedHash>? Load()
    {
        var fileName = GetFileName();
        if (!File.Exists(fileName))
            return null;
        var json = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize<IEnumerable<CachedHash>>(json);
    }

    public void Save(IEnumerable<CachedHash> cachedHashes)
    {
        var fileName = GetFileName();
        var utcNow = DateTime.UtcNow;
        var nonExpiredItems = cachedHashes
            .Where(x => x.Expiration > utcNow)
            .ToList();
        var json = JsonSerializer.Serialize(nonExpiredItems, _jsonSerializerOptions);
        if (File.Exists(fileName))
            File.Delete(fileName);
        File.WriteAllText(fileName, json);
    }
    private string GetFileName()
    {
        return Path.Combine(_folder, $"cache.json");
    }
}
