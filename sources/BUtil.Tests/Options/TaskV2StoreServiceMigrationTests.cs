using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.Core.Services;
using System.IO;
using System.Text.Json;

namespace BUtil.Tests.Options;

[TestClass]
public class TaskV2StoreServiceMigrationTests
{
    private const string ExtV2 = ".v2.json";
    private const string ExtV3 = ".v3.json";

    // ── Diagnostics ──────────────────────────────────────────────────────────

    /// <summary>
    /// Baseline: verifies that Save() writes a V3 entry into the file system.
    /// </summary>
    [TestMethod]
    public void Save_Baseline_CreatesV3File()
    {
        var fs = new InMemoryFileSystem();

        new TaskStore(fs).Save(MakeTask("save-baseline", "save-password"));

        Assert.IsTrue(fs.Files.ContainsKey("save-baseline" + ExtV3));
    }

    // ── MigrateAllToV3 ───────────────────────────────────────────────────────

    [TestMethod]
    public void MigrateAllToV3_SupportedV2Task_CreatesV3AndDeletesV2()
    {
        var fs = new InMemoryFileSystem();
        var name = "incremental-task";
        SaveThenRenameToV2(fs, name, "my-password");

        new TaskStore(fs).MigrateAllToV3();

        Assert.IsTrue(fs.Files.ContainsKey(name + ExtV3), "V3 file should be created");
        Assert.IsFalse(fs.Files.ContainsKey(name + ExtV2), "V2 file should be deleted");
    }

    [TestMethod]
    public void MigrateAllToV3_InvalidJsonV2Task_DeletesV2WithoutCreatingV3()
    {
        var fs = new InMemoryFileSystem();
        var name = "broken-task";
        fs.Files[name + ExtV2] = "not-valid-json";

        new TaskStore(fs).MigrateAllToV3();

        // The service reads and deletes V2 before deserializing; a parse failure
        // leaves V2 deleted and V3 absent.
        Assert.IsFalse(fs.Files.ContainsKey(name + ExtV2), "V2 file should be deleted");
        Assert.IsFalse(fs.Files.ContainsKey(name + ExtV3), "V3 file should not be created");
    }

    [TestMethod]
    public void MigrateAllToV3_UnknownModelV2Task_DeletesV2WithoutCreatingV3()
    {
        var fs = new InMemoryFileSystem();
        var name = "unsupported-task";
        fs.Files[name + ExtV2] = $$"""{"Model":null,"Name":"{{name}}"}""";

        new TaskStore(fs).MigrateAllToV3();

        // Same as invalid JSON: V2 is deleted before the model check, V3 is not written.
        Assert.IsFalse(fs.Files.ContainsKey(name + ExtV2), "V2 file should be deleted");
        Assert.IsFalse(fs.Files.ContainsKey(name + ExtV3), "V3 file should not be created");
    }

    [TestMethod]
    public void MigrateAllToV3_V3AlreadyExists_DeletesV2AndKeepsV3()
    {
        var fs = new InMemoryFileSystem();
        var name = "already-migrated";
        SaveThenRenameToV2(fs, name, "password");

        // Simulate a V3 already present alongside the V2
        fs.Files[name + ExtV3] = fs.Files[name + ExtV2];

        new TaskStore(fs).MigrateAllToV3();

        Assert.IsTrue(fs.Files.ContainsKey(name + ExtV3), "V3 file should still exist");
        Assert.IsFalse(fs.Files.ContainsKey(name + ExtV2), "V2 file should be deleted");
    }

    [TestMethod]
    public void MigrateAllToV3_Idempotent_RunningTwiceIsHarmless()
    {
        var fs = new InMemoryFileSystem();
        var name = "idempotent-task";
        SaveThenRenameToV2(fs, name, "password");

        var store = new TaskStore(fs);
        store.MigrateAllToV3();
        store.MigrateAllToV3(); // second run should be a no-op

        Assert.IsFalse(fs.Files.ContainsKey(name + ExtV2), "V2 file should not reappear");
        Assert.IsTrue(fs.Files.ContainsKey(name + ExtV3), "V3 file should still exist");
    }

    // ── GetNames ─────────────────────────────────────────────────────────────

    [TestMethod]
    public void GetNames_MixedV2AndV3Files_ReturnsDeduplicatedNames()
    {
        var fs = new InMemoryFileSystem();

        // task-a: only a V2 file (migration will promote it to V3)
        SaveThenRenameToV2(fs, "task-a", "pass");

        // task-b: only a V3 file
        new TaskStore(fs).Save(MakeTask("task-b", "pass"));

        // task-c: both V2 and V3 coexist (V3 wins, V2 gets cleaned up)
        new TaskStore(fs).Save(MakeTask("task-c", "pass"));
        fs.Files["task-c" + ExtV2] = fs.Files["task-c" + ExtV3]; // plant a V2 alongside V3

        var names = new TaskStore(fs).GetNames().ToList();

        CollectionAssert.Contains(names, "task-a");
        CollectionAssert.Contains(names, "task-b");
        CollectionAssert.Contains(names, "task-c");
        Assert.IsTrue(names.Count >= 3);
    }

    // ── Load ─────────────────────────────────────────────────────────────────

    [TestMethod]
    public void Load_V2FileOnly_LoadsSuccessfully()
    {
        var fs = new InMemoryFileSystem();
        var name = "load-v2-task";
        SaveThenRenameToV2(fs, name, "plain-password");

        var loaded = new TaskStore(fs).Load(name, out var isNotFound, out var isNotSupported);

        Assert.IsFalse(isNotFound);
        Assert.IsFalse(isNotSupported);
        Assert.IsNotNull(loaded);
        Assert.AreEqual(name, loaded.Name);
    }

    [TestMethod]
    public void Load_V3FileAfterMigration_LoadsSuccessfully()
    {
        var fs = new InMemoryFileSystem();
        var name = "load-v3-task";
        SaveThenRenameToV2(fs, name, "plain-password");

        var store = new TaskStore(fs);
        store.MigrateAllToV3();

        var loaded = store.Load(name, out var isNotFound, out var isNotSupported);

        Assert.IsFalse(isNotFound);
        Assert.IsFalse(isNotSupported);
        Assert.IsNotNull(loaded);
        Assert.AreEqual(name, loaded.Name);
    }

    [TestMethod]
    public void MigrateAllToV3_PasswordIsRestoredAfterMigration()
    {
        var fs = new InMemoryFileSystem();
        var name = "password-round-trip";
        SaveThenRenameToV2(fs, name, "secret123");

        var store = new TaskStore(fs);
        store.MigrateAllToV3();

        var loaded = store.Load(name, out _, out _);

        Assert.IsNotNull(loaded);
        var model = loaded.Model as IncrementalBackupModelOptionsV2;
        Assert.IsNotNull(model);
        Assert.AreEqual("secret123", model.Password);
    }

    [TestMethod]
    public void Load_BothV2AndV3Present_PrefersV3()
    {
        var fs = new InMemoryFileSystem();
        var name = "prefer-v3";

        SaveThenRenameToV2(fs, name, "v2-password");
        new TaskStore(fs).Save(MakeTask(name, "v3-password"));

        var loaded = new TaskStore(fs).Load(name, out _, out _);

        Assert.IsNotNull(loaded);
        Assert.AreEqual(name, loaded.Name);
        var model = loaded.Model as IncrementalBackupModelOptionsV2;
        Assert.IsNotNull(model);
        Assert.AreEqual("v3-password", model.Password);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static TaskV2 MakeTask(string name, string password) => new()
    {
        Name = name,
        Model = new IncrementalBackupModelOptionsV2
        {
            Password = password,
            Items = [],
            To = new FolderStorageSettingsV2 { DestinationFolder = string.Empty },
        },
    };

    /// <summary>
    /// Saves a task as V3 via the service, then renames the entry to V2 in the
    /// in-memory store to simulate a pre-migration task file.
    /// </summary>
    private static void SaveThenRenameToV2(InMemoryFileSystem fs, string name, string password)
    {
        new TaskStore(fs).Save(MakeTask(name, password));
        fs.Files[name + ExtV2] = fs.Files[name + ExtV3];
        fs.Files.Remove(name + ExtV3);
    }

    // ── InMemoryFileSystem ────────────────────────────────────────────────────

    private sealed class InMemoryFileSystem : ILocalFileSystem
    {
        /// <summary>
        /// Files keyed by their bare filename (e.g. "task.v3.json"), independent of
        /// the folder the service happens to use at runtime.
        /// </summary>
        public Dictionary<string, string> Files { get; } = new(StringComparer.OrdinalIgnoreCase);

        public void EnsureFolderCreated(string folder) { }

        public bool FileExists(string path) =>
            Files.ContainsKey(Path.GetFileName(path));

        public string ReadAllText(string path) =>
            Files[Path.GetFileName(path)];

        public void WriteAllText(string path, string content) =>
            Files[Path.GetFileName(path)] = content;

        public void DeleteFile(string path) =>
            Files.Remove(Path.GetFileName(path));

        public string[] GetFiles(string folder, string pattern)
        {
            var ext = pattern.TrimStart('*');
            return [.. Files.Keys
                .Where(f => f.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                .Select(f => Path.Combine(folder, f))];
        }
    }
}
