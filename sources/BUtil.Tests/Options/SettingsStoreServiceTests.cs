using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace BUtil.Tests.Options;

[TestClass]
public sealed class SettingsStoreServiceTests
{
    private sealed class MemoryFileSystem : ILocalFileSystem
    {
        public List<string> EnsuredFolders { get; } = [];
        public Dictionary<string, string> Files { get; } = new(StringComparer.OrdinalIgnoreCase);

        public void EnsureFolderCreated(string folder) => EnsuredFolders.Add(folder);

        public bool FileExists(string path) => Files.ContainsKey(path);

        public string ReadAllText(string path) => Files[path];

        public void WriteAllText(string path, string content) => Files[path] = content;

        public void DeleteFile(string path) => Files.Remove(path);

        public string[] GetFiles(string folder, string pattern) => [];
    }

    [TestMethod]
    public void Constructor_EnsuresSettingsDirectory()
    {
        var fs = new MemoryFileSystem();
        _ = new SettingsStoreService(fs);
        CollectionAssert.Contains(fs.EnsuredFolders, Directories.SettingsDir);
    }

    [TestMethod]
    public void Load_ReturnsDefault_WhenFileMissing()
    {
        var fs = new MemoryFileSystem();
        var svc = new SettingsStoreService(fs);
        Assert.AreEqual("def", svc.Load("MySetting", "def"));
    }

    [TestMethod]
    public void Load_ReturnsFileContent_WhenPresent()
    {
        var fs = new MemoryFileSystem();
        var path = Path.Combine(Directories.SettingsDir, "MySetting");
        fs.Files[path] = "stored";
        var svc = new SettingsStoreService(fs);
        Assert.AreEqual("stored", svc.Load("MySetting", "def"));
    }

    [TestMethod]
    public void Save_WritesValue()
    {
        var fs = new MemoryFileSystem();
        var svc = new SettingsStoreService(fs);
        svc.Save("MySetting", "v1");
        var path = Path.Combine(Directories.SettingsDir, "MySetting");
        Assert.AreEqual("v1", fs.Files[path]);
    }

    [TestMethod]
    public void Save_ReplacesExistingFile()
    {
        var fs = new MemoryFileSystem();
        var path = Path.Combine(Directories.SettingsDir, "MySetting");
        fs.Files[path] = "old";
        var svc = new SettingsStoreService(fs);
        svc.Save("MySetting", "new");
        Assert.AreEqual("new", fs.Files[path]);
    }

    [TestMethod]
    public void Load_Throws_WhenNameContainsPathTraversal()
    {
        var fs = new MemoryFileSystem();
        var svc = new SettingsStoreService(fs);
        Assert.ThrowsExactly<ArgumentException>(() => _ = svc.Load("..\\x", "d"));
    }
}
