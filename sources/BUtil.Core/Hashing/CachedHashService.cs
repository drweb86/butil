﻿
using BUtil.Core.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BUtil.Core.Hashing;

internal class CachedHashService(ICashedHashStoreService cashedHashStoreService) : IHashService, IDisposable
{
    private readonly ICashedHashStoreService _cashedHashStoreService = cashedHashStoreService;
    private readonly object _sync = new();
    private readonly ConcurrentBag<CachedHash> _cachedHashes = [];
    private bool _isCachedHashesLoaded = false;
    private const int _daysExpiration = 365;

    public string GetSha512(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hashedInputBytes = System.Security.Cryptography.SHA512.HashData(bytes);

        var hashedInputStringBuilder = new System.Text.StringBuilder(128);
        foreach (var b in hashedInputBytes)
            hashedInputStringBuilder.Append(b.ToString("X2"));
        return hashedInputStringBuilder.ToString();
    }

    public string GetSha512(string file, bool trySpeedupNextTime)
    {
        if (!trySpeedupNextTime)
        {
            return GetSha512Internal(file);
        }

        EnsureCacheIsLoaded();

        var cachedEntity = GetCreateOrUpdateCachedHash(file);
        return cachedEntity.Sha512;
    }

    private CachedHash GetCreateOrUpdateCachedHash(string file)
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
        cachedEntity.Expiration = DateTime.UtcNow.AddDays(_daysExpiration);
        cachedEntity.Size = fileInfo.Length;
        cachedEntity.LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
        if (string.IsNullOrWhiteSpace(cachedEntity.Sha512))
        {
            cachedEntity.Sha512 = GetSha512Internal(file);
        }

        return cachedEntity;
    }

    private void EnsureCacheIsLoaded()
    {
        lock (_sync)
        {
            if (!_isCachedHashesLoaded)
            {
                var cachedHashes = _cashedHashStoreService.Load() ?? [];
                foreach (var cachedHash in cachedHashes)
                    _cachedHashes.Add(cachedHash);
                _isCachedHashesLoaded = true;
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
        if (_cachedHashes != null)
        {
            var items = new CachedHash[_cachedHashes.Count];
            _cachedHashes.CopyTo(items, 0);
            _cashedHashStoreService.Save(_cachedHashes);
        }
    }
}
