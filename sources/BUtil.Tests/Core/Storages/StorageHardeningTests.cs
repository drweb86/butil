using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Storages;
using System.Security;

namespace BUtil.Tests.Core.Storages;

[TestClass]
public class StorageHardeningTests
{
    [TestMethod]
    public void StoragePathSecurity_RejectsTraversalOutsideRoot()
    {
        var root = Path.Combine(Path.GetTempPath(), "butil-tests-root-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);

        try
        {
            _ = ExpectThrows<SecurityException>(() =>
                StoragePathSecurity.ResolveRelativePathInsideRoot(root, "..\\outside.txt", allowEmpty: false, nameof(root)));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public void StoragePathSecurity_AllowsRootWhenEmptyIsAllowed()
    {
        var root = Path.Combine(Path.GetTempPath(), "butil-tests-root-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);

        try
        {
            var resolved = StoragePathSecurity.ResolveRelativePathInsideRoot(root, "", allowEmpty: true, nameof(root));
            Assert.AreEqual(Path.GetFullPath(root), resolved);
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public void FolderStorage_Upload_RejectsPathTraversal()
    {
        var destinationRoot = Path.Combine(Path.GetTempPath(), "butil-tests-storage-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(destinationRoot);
        var sourceFile = Path.Combine(destinationRoot, "source.txt");
        File.WriteAllText(sourceFile, "payload");

        try
        {
            using var storage = new FolderStorage(new MemoryLog(), new FolderStorageSettingsV2
            {
                DestinationFolder = destinationRoot,
            });

            _ = ExpectThrows<SecurityException>(() =>
                storage.Upload(sourceFile, "..\\escape.txt"));
        }
        finally
        {
            if (File.Exists(sourceFile))
                File.Delete(sourceFile);
            if (Directory.Exists(destinationRoot))
                Directory.Delete(destinationRoot, true);
        }
    }

    [TestMethod]
    public void FolderStorage_GetFiles_RejectsPathTraversal()
    {
        var destinationRoot = Path.Combine(Path.GetTempPath(), "butil-tests-storage-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(destinationRoot);

        try
        {
            using var storage = new FolderStorage(new MemoryLog(), new FolderStorageSettingsV2
            {
                DestinationFolder = destinationRoot,
            });

            _ = ExpectThrows<SecurityException>(() =>
                storage.GetFiles("..\\outside", SearchOption.AllDirectories));
        }
        finally
        {
            if (Directory.Exists(destinationRoot))
                Directory.Delete(destinationRoot, true);
        }
    }

    [TestMethod]
    public void ExecuteFailover_ThrowsWithInnerException_ForAction()
    {
        var rootCause = new IOException("root cause");
        try
        {
            ExecuteFailover.TryNTimes(_ => { }, () => throw rootCause, times: 1, retryDelayMs: 0);
            Assert.Fail("Exception expected.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreSame(rootCause, ex.InnerException);
        }
    }

    [TestMethod]
    public void ExecuteFailover_ThrowsWithInnerException_ForFunc()
    {
        var rootCause = new IOException("root cause");
        try
        {
            _ = ExecuteFailover.TryNTimes<int>(_ => { }, () => throw rootCause, times: 1, retryDelayMs: 0);
            Assert.Fail("Exception expected.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreSame(rootCause, ex.InnerException);
        }
    }

    [TestMethod]
    public void ExecuteFailover_DoesNotRetryNonTransientException()
    {
        var attempts = 0;
        _ = ExpectThrows<ArgumentException>(() =>
            ExecuteFailover.TryNTimes(
                _ => { },
                () =>
                {
                    attempts++;
                    throw new ArgumentException("not transient");
                },
                times: 5,
                retryDelayMs: 0));
        Assert.AreEqual(1, attempts);
    }

    [TestMethod]
    public void ExecuteFailover_RetriesTransientException()
    {
        var attempts = 0;
        _ = ExpectThrows<InvalidOperationException>(() =>
            ExecuteFailover.TryNTimes(
                _ => { },
                () =>
                {
                    attempts++;
                    throw new IOException("transient");
                },
                times: 3,
                retryDelayMs: 0));
        Assert.AreEqual(3, attempts);
    }

    [TestMethod]
    public void FailoverStorageWrapper_Dispose_IsIdempotent()
    {
        var fakeStorage = new FakeStorage();
        var wrapper = new FailoverStorageWrapper(new MemoryLog(), fakeStorage, triesCount: 1);

        wrapper.Dispose();
        wrapper.Dispose();

        Assert.AreEqual(1, fakeStorage.DisposeCalls);
    }

    [TestMethod]
    public void FailoverStorageWrapper_ThrowsAfterDispose()
    {
        var fakeStorage = new FakeStorage();
        var wrapper = new FailoverStorageWrapper(new MemoryLog(), fakeStorage, triesCount: 1);
        wrapper.Dispose();

        _ = ExpectThrows<ObjectDisposedException>(() => wrapper.Exists("x"));
    }

    [TestMethod]
    public void StorageFactory_Create_ThrowsForNullSettings()
    {
        _ = ExpectThrows<ArgumentNullException>(() =>
            StorageFactory.Create(new MemoryLog(), null!, autodetectConnectionSettings: false));
    }

    [TestMethod]
    public void StorageHelper_WriteTest_ThrowsForNullStorage()
    {
        _ = ExpectThrows<ArgumentNullException>(() =>
            StorageHelper.WriteTest(null!));
    }

    [TestMethod]
    public void FolderStorage_GetFiles_IncludesExtensionlessFile()
    {
        var destinationRoot = Path.Combine(Path.GetTempPath(), "butil-tests-storage-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(destinationRoot);
        File.WriteAllText(Path.Combine(destinationRoot, "noextension"), "a");
        File.WriteAllText(Path.Combine(destinationRoot, "with.txt"), "b");

        try
        {
            using var storage = new FolderStorage(new MemoryLog(), new FolderStorageSettingsV2
            {
                DestinationFolder = destinationRoot,
            });

            var files = storage.GetFiles();
            CollectionAssert.Contains(files, "noextension");
            CollectionAssert.Contains(files, "with.txt");
        }
        finally
        {
            if (Directory.Exists(destinationRoot))
                Directory.Delete(destinationRoot, true);
        }
    }

    [TestMethod]
    public void FolderStorage_GetFolders_IncludesExtensionlessFolder()
    {
        var destinationRoot = Path.Combine(Path.GetTempPath(), "butil-tests-storage-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(destinationRoot, "plainfolder"));
        Directory.CreateDirectory(Path.Combine(destinationRoot, "folder.with.dot"));

        try
        {
            using var storage = new FolderStorage(new MemoryLog(), new FolderStorageSettingsV2
            {
                DestinationFolder = destinationRoot,
            });

            var folders = storage.GetFolders(string.Empty);
            CollectionAssert.Contains(folders, "plainfolder");
            CollectionAssert.Contains(folders, "folder.with.dot");
        }
        finally
        {
            if (Directory.Exists(destinationRoot))
                Directory.Delete(destinationRoot, true);
        }
    }

    [TestMethod]
    public void FolderStorage_CopyFailure_DoesNotDeleteExistingTarget()
    {
        var destinationRoot = Path.Combine(Path.GetTempPath(), "butil-tests-storage-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(destinationRoot);
        var sourceFile = Path.Combine(destinationRoot, "source.bin");
        var destinationFile = Path.Combine(destinationRoot, "target.bin");
        File.WriteAllText(sourceFile, "new-data");
        File.WriteAllText(destinationFile, "old-data");

        try
        {
            using var lockStream = new FileStream(destinationFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            using var storage = new FolderStorage(new MemoryLog(), new FolderStorageSettingsV2
            {
                DestinationFolder = destinationRoot,
            });

            _ = ExpectThrows<UnauthorizedAccessException>(() => storage.Copy(sourceFile, destinationFile));
        }
        finally
        {
            Assert.IsTrue(File.Exists(destinationFile));
            Assert.AreEqual("old-data", File.ReadAllText(destinationFile));
            if (Directory.Exists(destinationRoot))
                Directory.Delete(destinationRoot, true);
        }
    }

    private static TException ExpectThrows<TException>(Action action)
        where TException : Exception
    {
        try
        {
            action();
            Assert.Fail("Exception expected.");
        }
        catch (TException ex)
        {
            return ex;
        }

        throw new InvalidOperationException("Exception expected.");
    }

    private sealed class FakeStorage : IStorage
    {
        public int DisposeCalls { get; private set; }

        public IStorageUploadResult Upload(string sourceFile, string relativeFileName) => new();
        public void Delete(string relativeFileName) { }
        public void Move(string fromRelativeFileName, string toRelativeFileName) { }
        public void Download(string relativeFileName, string targetFileName) { }
        public bool Exists(string relativeFileName) => false;
        public void DeleteFolder(string relativeFolderName) { }
        public string[] GetFolders(string relativeFolderName, string? mask = null) => [];
        public string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly) => [];
        public DateTime GetModifiedTime(string relativeFileName) => DateTime.MinValue;
        public string? Test() => null;
        public void Dispose() => DisposeCalls++;
    }
}
