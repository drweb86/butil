using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using System.Collections.Concurrent;
using System.Text.Json;

namespace BUtil.Tests;

[TestClass]
[DoNotParallelize]
public class CachedHashServiceTests
{
    private static readonly JsonSerializerOptions _jsonOptions = new();
    private byte[]? _previousCacheFileContent;
    private bool _previousCacheFileExists;

    private static string CacheFilePath => Path.Combine(Directories.StateFolder, "sha512-cache.json");

    [TestInitialize]
    public void Setup()
    {
        _previousCacheFileExists = File.Exists(CacheFilePath);
        _previousCacheFileContent = _previousCacheFileExists ? File.ReadAllBytes(CacheFilePath) : null;
        File.Delete(CacheFilePath);
    }

    [TestCleanup]
    public void Cleanup()
    {
        File.Delete(CacheFilePath);
        if (_previousCacheFileExists && _previousCacheFileContent != null)
        {
            File.WriteAllBytes(CacheFilePath, _previousCacheFileContent);
        }
    }

    [TestMethod]
    public void GetSha512_WhenCacheFileCorrupted_RebuildsCacheWithoutThrowing()
    {
        var file = CreateTempFile("cache-corrupt-test.txt", "data-v1");
        File.WriteAllText(CacheFilePath, "{invalid-json");

        using (var services = CreateServices())
        {
            var hashCached = services.CachedHashService.GetSha512(file, true);
            var hashDirect = services.CachedHashService.GetSha512(file, false);
            Assert.AreEqual(hashDirect, hashCached);
        }

        var cachedItems = ReadCacheItems();
        Assert.HasCount(1, cachedItems);
        Assert.AreEqual(Path.GetFullPath(file), cachedItems[0].File, StringComparer.OrdinalIgnoreCase);
    }

    [TestMethod]
    public void GetSha512_WhenFileChanged_RefreshesCachedValue()
    {
        var file = CreateTempFile("cache-update-test.txt", "first");

        using var services = CreateServices();
        var firstHash = services.CachedHashService.GetSha512(file, true);

        File.WriteAllText(file, "second");
        File.SetLastWriteTimeUtc(file, DateTime.UtcNow.AddSeconds(2));

        var secondHash = services.CachedHashService.GetSha512(file, true);
        var directHash = services.CachedHashService.GetSha512(file, false);

        Assert.AreNotEqual(firstHash, secondHash);
        Assert.AreEqual(directHash, secondHash);
    }

    [TestMethod]
    public void GetSha512_ConcurrentAccess_DoesNotThrowAndPersistsSingleEntry()
    {
        var file = CreateTempFile("cache-concurrency-test.txt", "content");
        var errors = new ConcurrentBag<Exception>();

        using (var services = CreateServices())
        {
            Parallel.For(0, 128, i =>
            {
                try
                {
                    var path = file;
                    if (OperatingSystem.IsWindows() && i % 2 == 1)
                    {
                        path = file.ToUpperInvariant();
                    }

                    services.CachedHashService.GetSha512(path, true);
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
            });
        }

        Assert.IsFalse(errors.Any(), string.Join(Environment.NewLine, errors.Select(x => x.ToString())));

        var cachedItems = ReadCacheItems();
        var fullPath = Path.GetFullPath(file);
        var comparer = OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        var matchCount = cachedItems.Count(x => comparer.Equals(x.File, fullPath));
        Assert.AreEqual(1, matchCount);
    }

    private static CommonServicesIoc CreateServices()
    {
        return new CommonServicesIoc(new NoopLog(), _ => { });
    }

    private static string CreateTempFile(string fileName, string content)
    {
        var folder = Path.Combine(Path.GetTempPath(), "BUtil-CachedHashServiceTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(folder);
        var path = Path.Combine(folder, fileName);
        File.WriteAllText(path, content);
        return path;
    }

    private static List<CachedHash> ReadCacheItems()
    {
        using var stream = new FileStream(CacheFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return JsonSerializer.Deserialize<List<CachedHash>>(stream, _jsonOptions) ?? [];
    }

    private sealed class NoopLog : ILog
    {
        public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
        {
        }

        public void Open()
        {
        }

        public void Close(bool isSuccess)
        {
        }

        public void WriteLine(LoggingEvent loggingEvent, string message)
        {
        }
    }
}
