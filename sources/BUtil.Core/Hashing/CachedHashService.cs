
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BUtil.Core.Hashing;

internal class CachedHashService() : ICachedHashService, IDisposable
{
    private readonly System.Threading.Lock _lock = new();
    private readonly List<CachedHash> _cachedHashes = [];
    private bool _isLoaded = false;

    public string GetSha512(string file, bool putIntoCache)
    {
        if (!putIntoCache)
        {
            return GetSha512Internal(file);
        }

        EnsureLoaded();

        return GetCreateOrUpdateCachedHash(file);
    }

    private string GetCreateOrUpdateCachedHash(string file)
    {
        lock (_lock)
        {
            var fileInfo = new FileInfo(file);
            var cachedEntity = _cachedHashes.SingleOrDefault(x => x.File == fileInfo.FullName);
            if (cachedEntity == null)
            {
                cachedEntity = new CachedHash
                {
                    File = fileInfo.FullName,
                };
                _cachedHashes.Add(cachedEntity);
            }
            else
            {
                if (cachedEntity.Size != fileInfo.Length ||
                    cachedEntity.LastWriteTimeUtc != fileInfo.LastWriteTimeUtc)
                {
                    cachedEntity.Sha512 = string.Empty;
                }
            }
            const int daysExpiration = 365;
            cachedEntity.Expiration = DateTime.UtcNow.AddDays(daysExpiration);
            cachedEntity.Size = fileInfo.Length;
            cachedEntity.LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
            if (string.IsNullOrWhiteSpace(cachedEntity.Sha512))
            {
                cachedEntity.Sha512 = GetSha512Internal(file);
            }

            return cachedEntity.Sha512;
        }
    }

    private void EnsureLoaded()
    {
        if (_isLoaded)
            return;

        lock (_lock)
        {
            if (!_isLoaded)
            {
                Load();
                _isLoaded = true;
            }
        }
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

    public void Dispose()
    {
        Save();
    }

    private void Load()
    {
        try
        {
            var file = GetFile();
            if (!File.Exists(file))
                return;

            using var stream = File.OpenRead(file);
            var items = JsonSerializer.Deserialize<List<CachedHash>>(stream) ?? new List<CachedHash>();
            items.ForEach(_cachedHashes.Add);
        }
        catch (System.Text.Json.JsonException)
        {
            // eating exception
        }
    }

    private void Save()
    {
        lock (this)
        {
            if (_cachedHashes == null)
                return;

            var file = GetFile();
            var utcNow = DateTime.UtcNow;

            var storeItems = _cachedHashes
                .ToList()
                .Where(x => x.Expiration > utcNow)
                .ToList();

            using var stream = File.Open(file, FileMode.Create);
            using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
            JsonSerializer.Serialize(writer, storeItems);
            writer.Flush();
            stream.Flush();
        }
    }

    private static string GetFile()
    {
        return Path.Combine(Directories.StateFolder, $"sha512-cache.json");
    }
}
