using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Interop.Logs;
using BUtil.Interop.Tasks;
using BUtil.Interop.Tasks.Events;
using BUtil.Tasks.Common.States;
using BUtil.Tasks.Common.Storage;
using BUtil.Tasks.IncrementalBackup;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BUtil.Tests.Tasks.IncrementalBackup;

/// <summary>
/// Registers a real folder storage before any test class initializer replaces it
/// with a factory that refuses to open folders.
/// </summary>
internal static class IncrementalBackupVerificationModule
{
    [ModuleInitializer]
    internal static void RegisterFolderStorageAndIncrementalTask()
    {
        StorageProviderRegistry.Register(
            "Folder",
            "Folder",
            new FolderStorageSettingsProvider(),
            typeof(FolderStorageSettingsV2),
            (log, settings, _) => new FolderStorage(log, (FolderStorageSettingsV2)settings));
        IncrementalBackupTaskPlugin.Register();
    }
}

/// <summary>
/// End-to-end incremental backup of file shapes. Storage backends are covered elsewhere;
/// this uses a local folder so every check is about files, versions, the state package, and restore.
/// </summary>
[TestClass]
[DoNotParallelize]
public class IncrementalBackupFileScenarioTests
{
    private const string Password = "incremental-scenario-password";

    private static readonly byte[] HelloBytes = "hello\r\nworld"u8.ToArray();
    private static readonly byte[] DuplicateBytes = "same-bytes"u8.ToArray();
    private static readonly byte[] HardLinkBytes = "hard-link-body"u8.ToArray();

    private string? _root;

    public TestContext TestContext { get; set; } = null!;

    [TestCleanup]
    public void Cleanup()
    {
        if (_root == null || !Directory.Exists(_root))
            return;

        foreach (var file in Directory.EnumerateFiles(_root, "*", SearchOption.AllDirectories))
            File.SetAttributes(file, FileAttributes.Normal);
        Directory.Delete(_root, true);
    }

    [TestMethod]
    public void Lifecycle_FileShapesIncrementsUnpackAndRestoreEveryVersion()
    {
        var (storage, sourcePath, source) = CreateSingleSource("lifecycle");
        var versions = new List<Dictionary<string, List<FileSnap>>>();

        Step("empty folder does not publish a version");
        RunBackup(storage, [source]);
        AssertNoBackupWritten(storage);

        Step("first version stores every file shape");
        WriteInitialTree(sourcePath);
        var first = Snap(source);
        var state = Publish(storage, [source], versions, first, "first version");
        AssertSameBlob(state, first[source.Target], "dup-a.dat", "dup-b.dat");
        AssertSameBlob(state, first[source.Target], "hard-a.txt", "hard-b.txt");
        Assert.AreEqual(0, first[source.Target].Single(x => x.Relative == "empty.dat").Size);

        Step("unchanged tree does not rewrite the state");
        AssertUnchanged(storage, [source], versions, "unchanged");

        Step("create, update, delete, rename, move, timestamp, and file-to-directory");
        ApplyShapeChanges(sourcePath, first[source.Target]);
        state = Publish(storage, [source], versions, Snap(source), "shape changes");
        AssertReusedHistoricalBlob(state, "renamed file.txt");
        AssertReusedHistoricalBlob(state, Path.Combine("sub", "dir", "moved.txt"));
        AssertReusedHistoricalBlob(state, "pad16.bin");

        Step("content returns to an earlier blob, sizes change, and a batch is deleted");
        ApplyReturnShrinkGrowAndBatchDelete(sourcePath);
        Publish(storage, [source], versions, Snap(source), "return shrink grow");
        AssertReusedHistoricalBlob(stateFromStorage(storage), "hello.txt");
        AssertReusedHistoricalBlob(stateFromStorage(storage), "empty.dat");

        Step("deleting every file publishes a restorable empty version");
        DeleteAllFiles(sourcePath);
        Publish(storage, [source], versions, Snap(source), "delete all");

        Step("files created again restore, and matching historical bytes reuse the old blob");
        WriteFile(Path.Combine(sourcePath, "hello.txt"), HelloBytes);
        WriteFile(Path.Combine(sourcePath, "other.txt"), "other"u8.ToArray());
        state = Publish(storage, [source], versions, Snap(source), "recreate");
        AssertReusedHistoricalBlob(state, "hello.txt");

        Step("partial restore of a historical version");
        AssertPartialHistoricalRestore(storage, versions[0][source.Target]);
    }

    [TestMethod]
    public void MultipleSources_AddOmitAndCrossSourceDedup_UnpackAndRestoreEveryVersion()
    {
        var root = CreateRoot("sources");
        var storage = CreateStorage(root);
        var left = Source("left", root);
        var right = Source("right", root);
        var mid = Source("mid", root);
        var versions = new List<Dictionary<string, List<FileSnap>>>();
        var payload = Enumerable.Range(0, 64).Select(x => (byte)x).ToArray();

        WriteFile(Path.Combine(left.Target, "a.txt"), "left-a"u8.ToArray(), Stamp(1));
        WriteFile(Path.Combine(left.Target, "same.txt"), "from-left"u8.ToArray(), Stamp(2));
        WriteFile(Path.Combine(left.Target, "nested", "n.txt"), "L"u8.ToArray(), Stamp(3));
        WriteFile(Path.Combine(left.Target, "payload.bin"), payload, Stamp(4));
        WriteFile(Path.Combine(right.Target, "b.txt"), "right-b"u8.ToArray(), Stamp(5));
        WriteFile(Path.Combine(right.Target, "same.txt"), "from-right"u8.ToArray(), Stamp(6));
        WriteFile(Path.Combine(right.Target, "payload.bin"), payload, Stamp(7));

        Step("two sources in one version, including the same relative name");
        var state = Publish(storage, [left, right], versions, Snap(left, right), "two sources");
        var leftPayload = StoredBlob(state, left.Target, "payload.bin");
        var rightPayload = StoredBlob(state, right.Target, "payload.bin");
        TestContext.WriteLine($"Cross-source payload blobs on the first version: {leftPayload} and {rightPayload}");

        Step("unchanged sources do not publish a version");
        AssertUnchanged(storage, [left, right], versions, "sources unchanged");

        Step("dropping a source without any other edit keeps the previous version");
        AssertUnchanged(storage, [left], versions, "source omitted with no file edit");
        Assert.AreEqual(2, versions[0].Count);

        Step("an edit after a source was omitted publishes only the remaining source");
        WriteFile(Path.Combine(left.Target, "a.txt"), "left-a-edited"u8.ToArray());
        state = Publish(storage, [left], versions, Snap(left), "source omitted with edit");
        Assert.AreEqual(1, state.VersionStates[^1].SourceItemChanges.Count());
        Assert.AreEqual(left.Target, state.LastSourceItemStates.Single().SourceItem.Target);

        Step("a newly added source is created, and bytes already stored are reused");
        WriteFile(Path.Combine(mid.Target, "payload.bin"), payload, Stamp(8));
        WriteFile(Path.Combine(mid.Target, "c.txt"), "mid"u8.ToArray(), Stamp(9));
        state = Publish(storage, [left, mid], versions, Snap(left, mid), "source added");
        var reused = StoredBlob(state, mid.Target, "payload.bin");
        Assert.IsTrue(reused == leftPayload || reused == rightPayload, reused);

        Step("one run can update more than one source");
        WriteFile(Path.Combine(left.Target, "nested", "n.txt"), "L2"u8.ToArray());
        WriteFile(Path.Combine(mid.Target, "c.txt"), "mid-2"u8.ToArray());
        Publish(storage, [left, mid], versions, Snap(left, mid), "two sources edited");
    }

    [TestMethod]
    public void ExcludePatterns_OnlyIncludedFilesAreVersioned_UnpackAndRestoreEveryVersion()
    {
        var (storage, sourcePath, source) = CreateSingleSource("exclude");
        var versions = new List<Dictionary<string, List<FileSnap>>>();
        var included = new[]
        {
            "keep.txt",
            Path.Combine("nested", "keep.txt"),
            "notes.tmp.txt",
            Path.Combine("sub dir", "file.log"),
        };

        WriteFile(Path.Combine(sourcePath, "keep.txt"), "keep"u8.ToArray(), Stamp(1));
        WriteFile(Path.Combine(sourcePath, "skip.tmp"), "skip"u8.ToArray(), Stamp(2));
        WriteFile(Path.Combine(sourcePath, "FILE.TMP"), "upper"u8.ToArray(), Stamp(3));
        WriteFile(Path.Combine(sourcePath, "nested", "skip.tmp"), "nested-skip"u8.ToArray(), Stamp(4));
        WriteFile(Path.Combine(sourcePath, "nested", "keep.txt"), "nested-keep"u8.ToArray(), Stamp(5));
        WriteFile(Path.Combine(sourcePath, "notes.tmp.txt"), "not-a-tmp-file"u8.ToArray(), Stamp(6));
        WriteFile(Path.Combine(sourcePath, "secret", "hidden.txt"), "secret"u8.ToArray(), Stamp(7));
        WriteFile(Path.Combine(sourcePath, "secret", "nested", "x.txt"), "secret-nested"u8.ToArray(), Stamp(8));
        WriteFile(Path.Combine(sourcePath, "bin", "obj.dll"), [1, 2, 3], Stamp(9));
        WriteFile(Path.Combine(sourcePath, "sub dir", "file.log"), "nested-log"u8.ToArray(), Stamp(10));
        WriteFile(Path.Combine(sourcePath, "root.log"), "root-log"u8.ToArray(), Stamp(11));
        WriteFile(Path.Combine(sourcePath, "exclude-me.txt"), "excluded-by-absolute-pattern"u8.ToArray(), Stamp(12));

        var excludes = new List<string>
        {
            "**/*.tmp",
            "**/secret/**",
            "**/bin/**",
            "*.log",
            source.Target + Path.DirectorySeparatorChar + "exclude-me.txt",
        };

        Step("excluded files are absent and included files are in the first version");
        Publish(storage, [source], versions, SnapSelected(source, included), "excluded initial", excludes);
        AssertNamesAbsent(stateFromStorage(storage), "skip.tmp", "FILE.TMP", Path.Combine("secret", "hidden.txt"), Path.Combine("bin", "obj.dll"), "root.log", "exclude-me.txt");

        Step("editing only excluded files does not publish a version");
        WriteFile(Path.Combine(sourcePath, "skip.tmp"), "skip-edited"u8.ToArray());
        WriteFile(Path.Combine(sourcePath, "secret", "hidden.txt"), "secret-edited"u8.ToArray());
        AssertUnchanged(storage, [source], versions, "excluded edits", excludes);

        Step("a new included file is versioned and a new excluded file is not");
        WriteFile(Path.Combine(sourcePath, "added", "new.txt"), "new"u8.ToArray(), Stamp(13));
        WriteFile(Path.Combine(sourcePath, "added", "another.tmp"), "another"u8.ToArray(), Stamp(14));
        var withNew = included.Append(Path.Combine("added", "new.txt")).ToArray();
        Publish(storage, [source], versions, SnapSelected(source, withNew), "included create", excludes);
        AssertNamesAbsent(stateFromStorage(storage), Path.Combine("added", "another.tmp"));

        Step("unchanged included set does not publish a version");
        AssertUnchanged(storage, [source], versions, "included unchanged", excludes);

        Step("an included edit publishes a version and excluded files stay out");
        WriteFile(Path.Combine(sourcePath, "keep.txt"), "keep-edited"u8.ToArray());
        Publish(storage, [source], versions, SnapSelected(source, withNew), "included edit", excludes);
        AssertNamesAbsent(stateFromStorage(storage), "skip.tmp", "FILE.TMP", "root.log", "exclude-me.txt", Path.Combine("added", "another.tmp"));
    }

    private IncrementalBackupState Publish(
        string storage,
        IReadOnlyList<SourceItemV2> sources,
        List<Dictionary<string, List<FileSnap>>> versions,
        Dictionary<string, List<FileSnap>> after,
        string step,
        IReadOnlyList<string>? excludes = null)
    {
        if (versions.Count > 0)
            WaitForDistinctVersionFolder(versions.Count);
        var before = versions.LastOrDefault();
        RunBackup(storage, sources, excludes);
        versions.Add(after);
        return AssertArchive(storage, versions, step, before, after);
    }

    private void AssertUnchanged(
        string storage,
        IReadOnlyList<SourceItemV2> sources,
        List<Dictionary<string, List<FileSnap>>> versions,
        string step,
        IReadOnlyList<string>? excludes = null)
    {
        var before = CapturePackage(storage);
        RunBackup(storage, sources, excludes);
        var after = CapturePackage(storage);
        Assert.AreEqual(before.StateHash, after.StateHash, $"{step}: state file was rewritten");
        Assert.AreEqual(before.StateWriteTimeUtc, after.StateWriteTimeUtc, $"{step}: state timestamp changed");
        Assert.AreEqual(before.ScriptWriteTimeUtc, after.ScriptWriteTimeUtc, $"{step}: integrity script timestamp changed");
        AssertArchive(storage, versions, step, diff: false);
    }

    private IncrementalBackupState AssertArchive(
        string storage,
        IReadOnlyList<Dictionary<string, List<FileSnap>>> versions,
        string step,
        IReadOnlyDictionary<string, List<FileSnap>>? before = null,
        IReadOnlyDictionary<string, List<FileSnap>>? latest = null,
        bool diff = true)
    {
        var settings = Settings(storage);
        var manual = UnpackState(storage, step);
        if (diff)
            AssertVersionMatchesDiff(manual.VersionStates[^1], before, latest!, step);

        Assert.AreEqual(versions.Count, manual.VersionStates.Count, $"{step}: version count");
        AssertLastState(manual, versions[^1], step);
        AssertPackageFormat(storage, manual, Password, step);
        AssertStateReadersAgree(settings, manual, step);
        AssertVersionsIncrease(manual, step);

        for (var i = 0; i < manual.VersionStates.Count; i++)
            RestoreVersion(settings, manual, manual.VersionStates[i], versions[i], $"{step}: restore version {i + 1}");

        return manual;
    }

    private static void AssertVersionMatchesDiff(
        VersionState version,
        IReadOnlyDictionary<string, List<FileSnap>>? before,
        IReadOnlyDictionary<string, List<FileSnap>> after,
        string step)
    {
        CollectionAssert.AreEquivalent(
            after.Keys.ToArray(),
            version.SourceItemChanges.Select(x => x.SourceItem.Target).ToArray(),
            $"{step}: sources in the new version");

        foreach (var change in version.SourceItemChanges)
        {
            var previous = new List<FileSnap>();
            if (before != null && before.TryGetValue(change.SourceItem.Target, out var found))
                previous = found;
            var current = after[change.SourceItem.Target];
            var previousByPath = previous.ToDictionary(x => x.FullPath, StringComparer.Ordinal);
            var currentByPath = current.ToDictionary(x => x.FullPath, StringComparer.Ordinal);

            var created = current.Where(x => !previousByPath.ContainsKey(x.FullPath)).Select(x => x.FullPath).ToArray();
            var deleted = previous.Where(x => !currentByPath.ContainsKey(x.FullPath)).Select(x => x.FullPath).ToArray();
            var updated = current
                .Where(x => previousByPath.TryGetValue(x.FullPath, out var old) && !SameIdentity(old, x))
                .Select(x => x.FullPath)
                .ToArray();

            CollectionAssert.AreEquivalent(created, change.CreatedFiles.Select(x => x.FileState.FileName).ToArray(), $"{step}: created");
            CollectionAssert.AreEquivalent(deleted, change.DeletedFiles, $"{step}: deleted");
            CollectionAssert.AreEquivalent(updated, change.UpdatedFiles.Select(x => x.FileState.FileName).ToArray(), $"{step}: updated");

            foreach (var file in change.CreatedFiles.Concat(change.UpdatedFiles))
            {
                var snap = currentByPath[file.FileState.FileName];
                Assert.AreEqual(snap.Size, file.FileState.Size, $"{step}: size {snap.Relative}");
                Assert.AreEqual(snap.Sha512, file.FileState.Sha512, $"{step}: sha512 {snap.Relative}");
                Assert.AreEqual(snap.LastWriteTimeUtc, file.FileState.LastWriteTimeUtc, $"{step}: time {snap.Relative}");
            }
        }
    }

    private static void AssertLastState(IncrementalBackupState state, IReadOnlyDictionary<string, List<FileSnap>> expected, string step)
    {
        CollectionAssert.AreEquivalent(
            expected.Keys.ToArray(),
            state.LastSourceItemStates.Select(x => x.SourceItem.Target).ToArray(),
            $"{step}: last source items");

        foreach (var last in state.LastSourceItemStates)
        {
            var snap = expected[last.SourceItem.Target].ToDictionary(x => x.FullPath, StringComparer.Ordinal);
            CollectionAssert.AreEquivalent(snap.Keys.ToArray(), last.FileStates.Select(x => x.FileName).ToArray(), $"{step}: last files");
            foreach (var file in last.FileStates)
            {
                var item = snap[file.FileName];
                Assert.AreEqual(item.Size, file.Size, $"{step}: last size {item.Relative}");
                Assert.AreEqual(item.Sha512, file.Sha512, $"{step}: last sha512 {item.Relative}");
                Assert.AreEqual(item.LastWriteTimeUtc, file.LastWriteTimeUtc, $"{step}: last time {item.Relative}");
            }
        }
    }

    private static void AssertPackageFormat(string storage, IncrementalBackupState state, string password, string step)
    {
        var statePath = Path.Combine(storage, IncrementalBackupModelConstants.BrotliAes256V1StateFile);
        Assert.IsTrue(File.Exists(statePath), step);
        AssertAes256V1Header(statePath, step);
        Assert.AreEqual(1, Directory.GetFiles(storage, "State.*").Length, $"{step}: a single state file");

        var passwords = new Dictionary<string, string>(StringComparer.Ordinal);
        var knownBlobs = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
        foreach (var version in state.VersionStates)
        {
            Assert.AreEqual(DateTimeKind.Utc, version.BackupDateUtc.Kind, step);
            var folder = SourceItemHelper.GetVersionFolder(version.BackupDateUtc);
            Assert.IsTrue(Regex.IsMatch(folder, @"^\d{4}-\d{2}-\d{2}-T-\d{2}-\d{2}-\d{2}$"), folder);

            var introduced = new Dictionary<string, List<(string Path, string Source)>>(StringComparer.Ordinal);
            foreach (var change in version.SourceItemChanges)
            {
                foreach (var file in change.CreatedFiles.Concat(change.UpdatedFiles))
                {
                    AssertBlobRecord(storage, file, password, step);
                    var key = file.FileState.Size.ToString(CultureInfo.InvariantCulture) + "|" + file.FileState.Sha512;
                    if (knownBlobs.TryGetValue(key, out var known))
                    {
                        Assert.IsTrue(known.Contains(file.StorageRelativeFileName), $"{step}: dedup {file.FileState.FileName} -> {file.StorageRelativeFileName}");
                    }
                    else if (introduced.TryGetValue(key, out var peers))
                    {
                        var sameSource = peers.FirstOrDefault(x => string.Equals(x.Source, change.SourceItem.Target, StringComparison.Ordinal));
                        if (sameSource.Path != null)
                            Assert.AreEqual(sameSource.Path, file.StorageRelativeFileName, $"{step}: dedup inside source {file.FileState.FileName}");
                        peers.Add((file.StorageRelativeFileName, change.SourceItem.Target));
                    }
                    else
                    {
                        Assert.AreEqual(folder, Path.GetDirectoryName(file.StorageRelativeFileName), $"{step}: new blob folder {file.FileState.FileName}");
                        introduced[key] = [(file.StorageRelativeFileName, change.SourceItem.Target)];
                    }

                    if (passwords.TryGetValue(file.StorageRelativeFileName, out var storedPassword))
                        Assert.AreEqual(storedPassword, file.StoragePassword, file.StorageRelativeFileName);
                    else
                        passwords[file.StorageRelativeFileName] = file.StoragePassword;
                }
            }

            foreach (var pair in introduced)
            {
                if (!knownBlobs.TryGetValue(pair.Key, out var paths))
                    knownBlobs[pair.Key] = paths = new HashSet<string>(StringComparer.Ordinal);
                foreach (var item in pair.Value)
                    paths.Add(item.Path);
            }

            var hasNewBlob = version.SourceItemChanges
                .SelectMany(x => x.CreatedFiles.Concat(x.UpdatedFiles))
                .Any(x => string.Equals(Path.GetDirectoryName(x.StorageRelativeFileName), folder, StringComparison.OrdinalIgnoreCase));
            if (hasNewBlob)
                Assert.IsTrue(Directory.Exists(Path.Combine(storage, folder)), $"{step}: {folder}");
        }

        var expectedFolders = state.VersionStates
            .Select(x => SourceItemHelper.GetVersionFolder(x.BackupDateUtc))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var directory in Directory.GetDirectories(storage))
        {
            var name = Path.GetFileName(directory);
            if (Regex.IsMatch(name, @"^\d{4}-\d{2}-\d{2}-T-\d{2}-\d{2}-\d{2}$"))
                Assert.IsTrue(expectedFolders.Contains(name), $"{step}: unexpected version folder {name}");
        }

        AssertIntegrityScript(storage, state, step);
    }

    private static void AssertBlobRecord(string storage, StorageFile file, string taskPassword, string step)
    {
        Assert.AreEqual(StorageMethodNames.Aes256Encrypted, file.StorageMethod, file.FileState.FileName);
        Assert.AreEqual(StorageIntegrityMethod.Sha512, file.StorageIntegrityMethod, file.FileState.FileName);
        Assert.AreEqual(255, file.StoragePassword.Length, file.FileState.FileName);
        Assert.AreNotEqual(taskPassword, file.StoragePassword, file.FileState.FileName);
        Assert.IsTrue(
            Regex.IsMatch(Path.GetFileName(file.StorageRelativeFileName), @"^[0-9a-fA-F-]{36}\.aes256v1$"),
            file.StorageRelativeFileName);

        var full = Path.Combine(storage, file.StorageRelativeFileName);
        Assert.IsTrue(File.Exists(full), $"{step}: {file.StorageRelativeFileName}");
        var info = new FileInfo(full);
        Assert.AreEqual(file.StorageFileNameSize, info.Length, file.StorageRelativeFileName);
        Assert.IsTrue(info.Length > 80, file.StorageRelativeFileName);
        Assert.AreEqual(ShaOfFile(full), file.StorageIntegrityMethodInfo, file.StorageRelativeFileName);
        AssertAes256V1Header(full, step);
    }

    private static void AssertIntegrityScript(string storage, IncrementalBackupState state, string step)
    {
        var scripts = Directory.GetFiles(storage, "*.ps1", SearchOption.TopDirectoryOnly);
        Assert.HasCount(1, scripts, step);
        var script = File.ReadAllText(scripts[0]);
        StringAssert.StartsWith(script, "<#", $"{step}: integrity script is plaintext");

        var entries = new Dictionary<string, (long Size, string Hash)>(StringComparer.OrdinalIgnoreCase);
        foreach (Match match in Regex.Matches(script, @"\[FileInfo\]::new\(""([^""]+)"", (\d+), ""([0-9a-fA-F]+)""\)"))
        {
            var relative = match.Groups[1].Value;
            entries[Normalize(relative)] = (long.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture), match.Groups[3].Value.ToLowerInvariant());
        }

        var referenced = state.VersionStates
            .SelectMany(x => x.SourceItemChanges)
            .SelectMany(x => x.CreatedFiles.Concat(x.UpdatedFiles))
            .Select(x => x.StorageRelativeFileName)
            .Append(IncrementalBackupModelConstants.BrotliAes256V1StateFile)
            .Select(Normalize)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        Assert.IsTrue(referenced.SetEquals(entries.Keys), $"{step}: integrity script file list");

        foreach (var relative in referenced)
        {
            var full = Path.Combine(storage, relative);
            var entry = entries[Normalize(relative)];
            Assert.AreEqual(entry.Size, new FileInfo(full).Length, $"{step}: script size {relative}");
            Assert.AreEqual(entry.Hash, ShaOfFile(full), $"{step}: script hash {relative}");
        }

        var password = state.VersionStates
            .SelectMany(x => x.SourceItemChanges)
            .SelectMany(x => x.CreatedFiles.Concat(x.UpdatedFiles))
            .Select(x => x.StoragePassword)
            .FirstOrDefault();
        if (password != null)
            Assert.IsFalse(script.Contains(password, StringComparison.Ordinal), $"{step}: per-file password leaked into the script");

        foreach (var file in Directory.EnumerateFiles(storage, "*.aes256v1", SearchOption.AllDirectories))
        {
            var relative = Normalize(Path.GetRelativePath(storage, file));
            Assert.IsTrue(entries.ContainsKey(relative), $"{step}: blob missing from the script {relative}");
        }
    }

    private static void AssertStateReadersAgree(FolderStorageSettingsV2 settings, IncrementalBackupState manual, string step)
    {
        var log = new MemoryLog();
        using var common = new CommonServicesIoc(log, _ => { });
        using var services = new StorageSpecificServicesIoc(common, settings);
        Assert.IsTrue(services.IncrementalBackupStateService.TryRead(Password, out var viaService), $"{step}: {log}");
        Assert.IsNotNull(viaService);
        Assert.AreEqual(Signature(manual), Signature(viaService), step);
    }

    private static void AssertVersionsIncrease(IncrementalBackupState state, string step)
    {
        for (var i = 1; i < state.VersionStates.Count; i++)
        {
            Assert.IsTrue(state.VersionStates[i].BackupDateUtc > state.VersionStates[i - 1].BackupDateUtc, step);
            Assert.AreNotEqual(
                SourceItemHelper.GetVersionFolder(state.VersionStates[i].BackupDateUtc),
                SourceItemHelper.GetVersionFolder(state.VersionStates[i - 1].BackupDateUtc),
                $"{step}: version folders share a second");
        }
    }

    private static void RestoreVersion(
        FolderStorageSettingsV2 settings,
        IncrementalBackupState state,
        VersionState version,
        IReadOnlyDictionary<string, List<FileSnap>> expected,
        string step)
    {
        var opened = OpenState(settings, step);
        Assert.AreEqual(state.VersionStates.Count, opened.VersionStates.Count, step);

        foreach (var change in version.SourceItemChanges)
        {
            var files = SourceItemHelper.BuildVersionFiles(state, change.SourceItem, version);
            var destination = Path.Combine(Path.GetTempPath(), "butil-restore-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(destination);
            try
            {
                var log = new MemoryLog();
                var restore = new WriteStorageFilesToSourceFileRootTask(log, new TaskEvents(), settings, change.SourceItem, files, destination, _ => { });
                restore.Execute();
                Assert.IsTrue(restore.IsSuccess, $"{step}: {change.SourceItem.Target}{Environment.NewLine}{Tail(log)}");
                AssertRestored(destination, expected[change.SourceItem.Target], step);
            }
            finally
            {
                if (Directory.Exists(destination))
                {
                    foreach (var file in Directory.EnumerateFiles(destination, "*", SearchOption.AllDirectories))
                        File.SetAttributes(file, FileAttributes.Normal);
                    Directory.Delete(destination, true);
                }
            }
        }
    }

    private void AssertPartialHistoricalRestore(string storage, IReadOnlyList<FileSnap> firstVersion)
    {
        var settings = Settings(storage);
        var state = UnpackState(storage, "partial");
        var version = state.VersionStates[0];
        var sourceItem = version.SourceItemChanges.Single().SourceItem;
        var files = SourceItemHelper.BuildVersionFiles(state, sourceItem, version);
        var selected = files
            .Where(x => x.FileState.FileName.EndsWith(Path.DirectorySeparatorChar + "hello.txt", StringComparison.Ordinal)
                || x.FileState.FileName.EndsWith(Path.DirectorySeparatorChar + "empty.dat", StringComparison.Ordinal)
                || x.FileState.FileName.EndsWith(Path.DirectorySeparatorChar + "deep.txt", StringComparison.Ordinal))
            .ToList();
        Assert.HasCount(3, selected);

        var destination = Path.Combine(Path.GetTempPath(), "butil-partial-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(destination);
        try
        {
            var log = new MemoryLog();
            var restore = new WriteStorageFilesToSourceFileRootTask(log, new TaskEvents(), settings, sourceItem, selected, destination, _ => { });
            restore.Execute();
            Assert.IsTrue(restore.IsSuccess, Tail(log));
            var expected = firstVersion
                .Where(x => x.Relative is "hello.txt" or "empty.dat" || x.Relative == Path.Combine("sub", "dir", "deep.txt"))
                .ToList();
            AssertRestored(destination, expected, "partial");
        }
        finally
        {
            if (Directory.Exists(destination))
                Directory.Delete(destination, true);
        }
    }

    private static void AssertRestored(string destination, IReadOnlyList<FileSnap> expected, string step)
    {
        var actual = Directory.EnumerateFiles(destination, "*", SearchOption.AllDirectories).ToList();
        Assert.AreEqual(expected.Count, actual.Count, $"{step}: restored file count at {destination}");
        foreach (var snap in expected)
        {
            var path = Path.Combine(destination, snap.Relative);
            Assert.IsTrue(File.Exists(path), $"{step}: missing {snap.Relative}");
            CollectionAssert.AreEqual(snap.Content, File.ReadAllBytes(path), $"{step}: bytes {snap.Relative}");
            Assert.AreEqual(snap.LastWriteTimeUtc, File.GetLastWriteTimeUtc(path), $"{step}: restored time {snap.Relative}");
        }

        foreach (var directory in Directory.EnumerateDirectories(destination, "*", SearchOption.AllDirectories))
            Assert.IsTrue(Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories).Any(), $"{step}: empty directory restored {directory}");
    }

    private static IncrementalBackupState OpenState(FolderStorageSettingsV2 settings, string step)
    {
        var log = new MemoryLog();
        var open = new GetExistingVersionStateFromStorageRootTask(log, new TaskEvents(), settings, Password, _ => { });
        open.Execute();
        Assert.IsTrue(open.IsSuccess, $"{step}: open state{Environment.NewLine}{Tail(log)}");
        return open.StorageState ?? throw new InvalidOperationException(step);
    }

    private static IncrementalBackupState UnpackState(string storage, string step)
    {
        var statePath = Path.Combine(storage, IncrementalBackupModelConstants.BrotliAes256V1StateFile);
        Assert.IsTrue(File.Exists(statePath), step);
        AssertAes256V1Header(statePath, step);

        var temp = Path.Combine(Path.GetTempPath(), "butil-state-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temp);
        try
        {
            using var services = new CommonServicesIoc(new MemoryLog(), _ => { });
            var brotli = Path.Combine(temp, "state.brotli");
            var json = Path.Combine(temp, "state.json");
            services.EncryptionService.DecryptAes256File(statePath, brotli, Password);
            services.CompressionService.DecompressBrotliFile(brotli, json);
            var text = File.ReadAllText(json);
            Assert.IsTrue(text.TrimStart().StartsWith('{'), $"{step}: state json");
            StringAssert.Contains(text, "\"VersionStates\"", step);
            StringAssert.Contains(text, "\"LastSourceItemStates\"", step);
            StringAssert.Contains(text, "\"Encrypted AES256\"", step);
            return JsonSerializer.Deserialize<IncrementalBackupState>(text) ?? throw new InvalidOperationException(step);
        }
        finally
        {
            Directory.Delete(temp, true);
        }
    }

    private static void AssertAes256V1Header(string path, string step)
    {
        using var input = File.OpenRead(path);
        using var reader = new BinaryReader(input, Encoding.ASCII, leaveOpen: true);
        Assert.AreEqual(1, reader.ReadInt32(), $"{step}: version {path}");
        Assert.AreEqual(256, reader.ReadInt32(), $"{step}: key size {path}");
        Assert.AreEqual(128, reader.ReadInt32(), $"{step}: block size {path}");
        Assert.AreEqual(1000, reader.ReadInt32(), $"{step}: iterations {path}");
        Assert.AreEqual("SHA512", reader.ReadString(), $"{step}: kdf {path}");
        Assert.IsTrue(input.Length > input.Position + 32 + 16, $"{step}: header too short {path}");
    }

    private static void AssertNoBackupWritten(string storage)
    {
        Assert.IsFalse(File.Exists(Path.Combine(storage, IncrementalBackupModelConstants.BrotliAes256V1StateFile)));
        Assert.IsEmpty(Directory.GetFiles(storage, "*.ps1"));
        Assert.IsEmpty(Directory.GetDirectories(storage));
    }

    private static void AssertSameBlob(IncrementalBackupState state, IReadOnlyList<FileSnap> snaps, string relativeA, string relativeB)
    {
        var a = snaps.Single(x => x.Relative == relativeA);
        var b = snaps.Single(x => x.Relative == relativeB);
        var files = state.VersionStates[0].SourceItemChanges.Single().CreatedFiles;
        var storedA = files.Single(x => x.FileState.FileName == a.FullPath);
        var storedB = files.Single(x => x.FileState.FileName == b.FullPath);
        Assert.AreEqual(storedA.StorageRelativeFileName, storedB.StorageRelativeFileName, relativeA);
        Assert.AreEqual(storedA.StoragePassword, storedB.StoragePassword, relativeA);
    }

    private static void AssertReusedHistoricalBlob(IncrementalBackupState state, string relative)
    {
        StorageFile? latest = null;
        string? firstPath = null;
        foreach (var version in state.VersionStates)
        {
            foreach (var change in version.SourceItemChanges)
            {
                foreach (var file in change.CreatedFiles.Concat(change.UpdatedFiles))
                {
                    if (!file.FileState.FileName.EndsWith(Path.DirectorySeparatorChar + relative, StringComparison.Ordinal)
                        && !file.FileState.FileName.EndsWith(Path.AltDirectorySeparatorChar + relative, StringComparison.Ordinal))
                        continue;
                    firstPath ??= file.StorageRelativeFileName;
                    latest = file;
                }
            }
        }

        Assert.IsNotNull(latest, relative);
        Assert.AreEqual(firstPath, latest.StorageRelativeFileName, relative);
    }

    private static void AssertNamesAbsent(IncrementalBackupState state, params string[] relatives)
    {
        var names = state.VersionStates
            .SelectMany(x => x.SourceItemChanges)
            .SelectMany(x => x.CreatedFiles.Concat(x.UpdatedFiles).Select(file => file.FileState.FileName).Concat(x.DeletedFiles))
            .ToList();
        foreach (var relative in relatives)
        {
            var suffix = Path.DirectorySeparatorChar + relative;
            Assert.IsFalse(names.Any(x => x.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)), relative);
        }
    }

    private static string StoredBlob(IncrementalBackupState state, string source, string relative)
    {
        var suffix = Path.DirectorySeparatorChar + relative;
        foreach (var version in state.VersionStates)
        {
            foreach (var change in version.SourceItemChanges)
            {
                if (!string.Equals(change.SourceItem.Target, source, StringComparison.Ordinal))
                    continue;
                foreach (var file in change.CreatedFiles.Concat(change.UpdatedFiles))
                {
                    if (file.FileState.FileName.EndsWith(suffix, StringComparison.Ordinal))
                        return file.StorageRelativeFileName;
                }
            }
        }

        throw new InvalidOperationException(relative);
    }

    private static void WaitForDistinctVersionFolder(int existingVersions)
    {
        if (existingVersions == 0)
            return;
        Thread.Sleep(1200);
    }

    private void RunBackup(string storage, IReadOnlyList<SourceItemV2> sources, IReadOnlyList<string>? excludes = null)
    {
        var log = new MemoryLog();
        var task = new TaskV2
        {
            Name = "incremental-file-scenarios",
            Model = new IncrementalBackupModelOptionsV2
            {
                To = Settings(storage),
                Items = [.. sources],
                FileExcludePatterns = excludes?.ToList() ?? [],
                Password = Password,
            },
        };
        var root = TaskProviderRegistry.Create(log, task, new TaskEvents(), _ => { });
        root.Execute();
        Assert.IsTrue(root.IsSuccess, Tail(log));
    }

    private (string Storage, string SourcePath, SourceItemV2 Source) CreateSingleSource(string name)
    {
        var root = CreateRoot(name);
        var storage = CreateStorage(root);
        var source = Source("source", root);
        return (storage, source.Target, source);
    }

    private string CreateRoot(string name)
    {
        _root = Path.Combine(Path.GetTempPath(), "butil-incremental-" + name + "-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_root);
        return _root;
    }

    private static string CreateStorage(string root)
    {
        var storage = Path.Combine(root, "storage");
        Directory.CreateDirectory(storage);
        return new DirectoryInfo(storage).FullName;
    }

    private static SourceItemV2 Source(string name, string root)
    {
        var path = Directory.CreateDirectory(Path.Combine(root, name)).FullName;
        return new SourceItemV2(path, true);
    }

    private static FolderStorageSettingsV2 Settings(string storage) => new()
    {
        DestinationFolder = storage,
        SingleBackupQuotaGb = 0,
    };

    private static void WriteInitialTree(string source)
    {
        var index = 0;
        WriteFile(Path.Combine(source, "empty.dat"), [], Stamp(index++));
        WriteFile(Path.Combine(source, "one.bin"), [0x7F], Stamp(index++));
        WriteFile(Path.Combine(source, "pad15.bin"), Repeat(0x15, 15), Stamp(index++));
        WriteFile(Path.Combine(source, "pad16.bin"), Repeat(0x16, 16), Stamp(index++));
        WriteFile(Path.Combine(source, "pad17.bin"), Repeat(0x17, 17), Stamp(index++));
        WriteFile(Path.Combine(source, "binary.bin"), Enumerable.Range(0, 256).Select(x => (byte)x).ToArray(), Stamp(index++));
        var wide = new byte[200_003];
        for (var i = 0; i < wide.Length; i++)
            wide[i] = (byte)(i * 31);
        WriteFile(Path.Combine(source, "wide.bin"), wide, Stamp(index++));
        WriteFile(Path.Combine(source, "hello.txt"), HelloBytes, Stamp(index++));
        WriteFile(Path.Combine(source, "bom.txt"), [0xEF, 0xBB, 0xBF, (byte)'A'], Stamp(index++));
        WriteFile(Path.Combine(source, "файл.txt"), "Привет"u8.ToArray(), Stamp(index++));
        WriteFile(Path.Combine(source, "文件.dat"), "你好\0"u8.ToArray(), Stamp(index++));
        WriteFile(Path.Combine(source, "my file (1).txt"), "spaced"u8.ToArray(), Stamp(index++));
        WriteFile(Path.Combine(source, " spaced.txt"), "leading-space"u8.ToArray(), Stamp(index++));
        WriteFile(Path.Combine(source, "a+b#c%d.txt"), "punctuation"u8.ToArray(), Stamp(index++));
        WriteFile(Path.Combine(source, "o'brien.txt"), "apostrophe"u8.ToArray(), Stamp(index++));
        WriteFile(Path.Combine(source, "sub", "dir", "deep.txt"), "deep"u8.ToArray(), Stamp(index++));
        WriteFile(Path.Combine(source, "подпапка", "ещё", "文件.dat"), "вложенный"u8.ToArray(), Stamp(index++));
        WriteFile(Path.Combine(source, "sub dir", "my file.txt"), "dir space"u8.ToArray(), Stamp(index++));
        WriteFile(Path.Combine(source, "dup-a.dat"), DuplicateBytes, Stamp(index++));
        WriteFile(Path.Combine(source, "dup-b.dat"), DuplicateBytes, Stamp(index++));
        WriteFile(Path.Combine(source, "hard-a.txt"), HardLinkBytes, Stamp(index++));
        CreateHardLinkTo(Path.Combine(source, "hard-b.txt"), Path.Combine(source, "hard-a.txt"));
        WriteFile(Path.Combine(source, "noext"), "extensionless"u8.ToArray(), Stamp(index++));
        WriteFile(Path.Combine(source, ".dotfile"), "dot"u8.ToArray(), Stamp(index++));
        WriteFile(Path.Combine(source, "readonly.txt"), "readonly"u8.ToArray(), Stamp(index++), FileAttributes.ReadOnly);
        WriteFile(Path.Combine(source, "hidden.txt"), "hidden"u8.ToArray(), Stamp(index++), FileAttributes.Hidden);
        WriteFile(Path.Combine(source, "a", "b", "c", "d", "e", "f", "g", "leaf.txt"), "leaf"u8.ToArray(), Stamp(index++));
        for (var i = 0; i < 12; i++)
            WriteFile(Path.Combine(source, "batch", $"file-{i:00}.txt"), Encoding.UTF8.GetBytes($"batch-{i:00}"), Stamp(index++));
        Directory.CreateDirectory(Path.Combine(source, "empty-dir"));
    }

    private static void ApplyShapeChanges(string source, IReadOnlyList<FileSnap> before)
    {
        File.WriteAllText(before.Single(x => x.Relative == "hello.txt").FullPath, "changed-hello");
        File.Delete(before.Single(x => x.Relative == "empty.dat").FullPath);
        WriteFile(Path.Combine(source, "created.txt"), "created"u8.ToArray());

        var spaced = before.Single(x => x.Relative == "my file (1).txt");
        File.Move(spaced.FullPath, Path.Combine(source, "renamed file.txt"));

        var pad = before.Single(x => x.Relative == "pad16.bin");
        File.SetLastWriteTimeUtc(pad.FullPath, pad.LastWriteTimeUtc.AddHours(3));

        var deep = before.Single(x => x.Relative == Path.Combine("sub", "dir", "deep.txt"));
        File.Move(deep.FullPath, Path.Combine(source, "sub", "dir", "moved.txt"));

        File.WriteAllBytes(before.Single(x => x.Relative == "dup-a.dat").FullPath, [1, 2, 3, 4, 5]);

        var noext = before.Single(x => x.Relative == "noext");
        File.Delete(noext.FullPath);
        Directory.CreateDirectory(noext.FullPath);
        WriteFile(Path.Combine(noext.FullPath, "child.txt"), "child"u8.ToArray());

        File.WriteAllText(before.Single(x => x.Relative == Path.Combine("batch", "file-00.txt")).FullPath, "updated-batch-0");
        File.Delete(before.Single(x => x.Relative == Path.Combine("batch", "file-01.txt")).FullPath);
        WriteFile(Path.Combine(source, "batch", "file-12.txt"), "batch-12"u8.ToArray());
    }

    private static void ApplyReturnShrinkGrowAndBatchDelete(string source)
    {
        WriteFile(Path.Combine(source, "hello.txt"), HelloBytes);
        WriteFile(Path.Combine(source, "empty.dat"), []);
        WriteFile(Path.Combine(source, "wide.bin"), [9, 8, 7, 6, 5, 4, 3, 2, 1, 0]);
        var pad17 = Path.Combine(source, "pad17.bin");
        WriteFile(pad17, [.. File.ReadAllBytes(pad17), .. Repeat(0xAB, 100)]);
        for (var i = 6; i <= 11; i++)
            File.Delete(Path.Combine(source, "batch", $"file-{i:00}.txt"));
    }

    private static void DeleteAllFiles(string source)
    {
        foreach (var file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }
    }

    private static Dictionary<string, List<FileSnap>> Snap(params SourceItemV2[] sources)
    {
        var result = new Dictionary<string, List<FileSnap>>(StringComparer.Ordinal);
        foreach (var source in sources)
        {
            var root = source.Target.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var files = Directory.Exists(root)
                ? Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories).Select(path => ReadSnap(root, path)).ToList()
                : [];
            result[source.Target] = files;
        }
        return result;
    }

    private static Dictionary<string, List<FileSnap>> SnapSelected(SourceItemV2 source, IEnumerable<string> relatives)
    {
        var root = source.Target.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        return new Dictionary<string, List<FileSnap>>(StringComparer.Ordinal)
        {
            [source.Target] = relatives.Select(relative => ReadSnap(root, Path.Combine(root, relative))).ToList(),
        };
    }

    private static FileSnap ReadSnap(string root, string path)
    {
        var info = new FileInfo(path);
        var full = info.FullName;
        var bytes = File.ReadAllBytes(full);
        return new FileSnap(full, full[(root.Length + 1)..], bytes, info.LastWriteTimeUtc, Sha(bytes), info.Length);
    }

    private static void WriteFile(string path, byte[] content, DateTime? stamp = null, FileAttributes attributes = FileAttributes.Normal)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllBytes(path, content);
        if (stamp.HasValue)
            File.SetLastWriteTimeUtc(path, stamp.Value);
        if (attributes != FileAttributes.Normal)
            File.SetAttributes(path, attributes);
    }

    private static void CreateHardLinkTo(string link, string existing)
    {
        if (!CreateHardLink(link, existing, IntPtr.Zero))
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

    private static DateTime Stamp(int index) =>
        new DateTime(2020, 1, 15, 12, 0, 0, DateTimeKind.Utc).AddSeconds(index).AddTicks(index * 10L);

    private static byte[] Repeat(byte value, int count) => Enumerable.Repeat(value, count).ToArray();

    private static bool SameIdentity(FileSnap left, FileSnap right) =>
        left.Size == right.Size && left.Sha512 == right.Sha512 && left.LastWriteTimeUtc == right.LastWriteTimeUtc;

    private static string Sha(byte[] bytes) => Convert.ToHexString(SHA512.HashData(bytes)).ToLowerInvariant();

    private static string ShaOfFile(string path) => Sha(File.ReadAllBytes(path));

    private static string Normalize(string path) => path.Replace('/', '\\');

    private static string Signature(IncrementalBackupState state) =>
        string.Join("|", state.VersionStates.Select(version =>
            version.BackupDateUtc.Ticks.ToString(CultureInfo.InvariantCulture) + ":" +
            string.Join(",", version.SourceItemChanges.Select(change =>
                change.SourceItem.Target + "#" + change.CreatedFiles.Count + "#" + change.UpdatedFiles.Count + "#" + change.DeletedFiles.Count))));

    private static PackageMark CapturePackage(string storage)
    {
        var state = Path.Combine(storage, IncrementalBackupModelConstants.BrotliAes256V1StateFile);
        var script = Directory.GetFiles(storage, "*.ps1", SearchOption.TopDirectoryOnly).Single();
        return new PackageMark(Sha(File.ReadAllBytes(state)), File.GetLastWriteTimeUtc(state), File.GetLastWriteTimeUtc(script));
    }

    private IncrementalBackupState stateFromStorage(string storage) => UnpackState(storage, "lookup");

    private void Step(string description) => TestContext.WriteLine(description);

    private static string Tail(MemoryLog log)
    {
        var text = log.ToString();
        return text.Length <= 8000 ? text : text[^8000..];
    }

    private sealed record FileSnap(string FullPath, string Relative, byte[] Content, DateTime LastWriteTimeUtc, string Sha512, long Size);

    private sealed record PackageMark(string StateHash, DateTime StateWriteTimeUtc, DateTime ScriptWriteTimeUtc);
}
