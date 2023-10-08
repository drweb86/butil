
using System.IO;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace BUtil.Core.Options
{
    public class CachedHash
    {
        public string File { get; set; }
        public DateTime Expiration { get; set; }
        public string Sha512 { get; set; }
        public DateTime LastWriteTimeUtc { get; set; }
        public long Size { get; set; }
    }

    public interface ICashedHashStoreService
    {
        IEnumerable<CachedHash> Load();
        void Save(IEnumerable<CachedHash> cachedHashes);
    }

    public class CashedHashStoreService: ICashedHashStoreService
    {
        private readonly string _folder;

        public CashedHashStoreService()
        {
#if DEBUG
            _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Cache-V1 - DEBUG");
#else
            _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Cache-V1");
#endif
            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);
        }

        public IEnumerable<CachedHash> Load()
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
            var json = JsonSerializer.Serialize(nonExpiredItems, new JsonSerializerOptions { WriteIndented = true });
            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllText(fileName, json);
        }
        private string GetFileName()
        {
            return Path.Combine(_folder, $"cache.json");
        }
    }
}
