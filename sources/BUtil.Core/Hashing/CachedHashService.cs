
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;

namespace BUtil.Core.Hashing;

internal class CachedHashService : ICachedHashService
{
    private readonly object _gate = new();
    private readonly Dictionary<string, CachedHash> _cachedHashes = new(GetPathComparer());
    private bool _isLoaded = false;

    public string GetSha512(string file, bool trySpeedupNextTime)
    {
        if (!trySpeedupNextTime)
        {
            return GetSha512Internal(file);
        }

        return GetCreateOrUpdateCachedHash(file);
    }

    private string GetCreateOrUpdateCachedHash(string file)
    {
        var fileInfo = new FileInfo(file);
        var fullName = fileInfo.FullName;
        var utcNow = DateTime.UtcNow;
        const int daysExpiration = 365;
        var expiration = utcNow.AddDays(daysExpiration);
        var size = fileInfo.Length;
        var lastWriteTimeUtc = fileInfo.LastWriteTimeUtc;

        lock (_gate)
        {
            EnsureLoadedLocked();

            if (!_cachedHashes.TryGetValue(fullName, out var cachedEntity))
            {
                cachedEntity = new CachedHash
                {
                    File = fullName,
                };
                _cachedHashes[fullName] = cachedEntity;
            }
            else if (cachedEntity.Size != size || cachedEntity.LastWriteTimeUtc != lastWriteTimeUtc)
            {
                cachedEntity.Sha512 = string.Empty;
            }

            cachedEntity.Expiration = expiration;
            cachedEntity.Size = size;
            cachedEntity.LastWriteTimeUtc = lastWriteTimeUtc;

            if (!string.IsNullOrWhiteSpace(cachedEntity.Sha512))
            {
                return cachedEntity.Sha512;
            }
        }

        var calculatedHash = GetSha512Internal(file);

        lock (_gate)
        {
            // Keep metadata and value in sync for the version of file we just hashed.
            if (!_cachedHashes.TryGetValue(fullName, out var cachedEntity))
            {
                cachedEntity = new CachedHash
                {
                    File = fullName,
                };
                _cachedHashes[fullName] = cachedEntity;
            }

            cachedEntity.Expiration = expiration;
            cachedEntity.Size = size;
            cachedEntity.LastWriteTimeUtc = lastWriteTimeUtc;
            cachedEntity.Sha512 = calculatedHash;
        }

        return calculatedHash;
    }

    private void EnsureLoadedLocked()
    {
        if (_isLoaded)
        {
            return;
        }

        LoadLocked();
        _isLoaded = true;
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
        return Convert.ToHexString(hash).ToLower(CultureInfo.InvariantCulture);
    }

    public void Dispose()
    {
        Save();
    }

    private void LoadLocked()
    {
        try
        {
            var file = GetFile();
            if (!File.Exists(file))
                return;

            using var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            var items = JsonSerializer.Deserialize<List<CachedHash>>(stream) ?? [];
            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.File))
                {
                    continue;
                }

                var fullPath = Path.GetFullPath(item.File);
                item.File = fullPath;
                _cachedHashes[fullPath] = item;
            }
        }
        catch (JsonException)
        {
            // eating
        }
        catch (IOException)
        {
            // eating
        }
        catch (UnauthorizedAccessException)
        {
            // eating
        }
    }

    private void Save()
    {
        List<CachedHash> storeItems;
        var file = GetFile();
        var utcNow = DateTime.UtcNow;

        lock (_gate)
        {
            storeItems = _cachedHashes
                .Values
                .Where(x => x.Expiration > utcNow)
                .ToList();
        }

        try
        {
            var tempFile = $"{file}.tmp";
            using (var stream = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
                JsonSerializer.Serialize(writer, storeItems);
                writer.Flush();
                stream.Flush(true);
            }

            if (File.Exists(file))
            {
                File.Replace(tempFile, file, null, true);
            }
            else
            {
                File.Move(tempFile, file);
            }
        }
        catch (IOException)
        {
            // eating.
        }
        catch (UnauthorizedAccessException)
        {
            // eating.
        }
    }

    private static string GetFile()
    {
        return Path.Combine(Directories.StateFolder, "sha512-cache.json");
    }

    private static StringComparer GetPathComparer()
    {
        return OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
    }
}
