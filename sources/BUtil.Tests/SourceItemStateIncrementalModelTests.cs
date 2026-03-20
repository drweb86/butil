using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.State;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Tests;

[TestClass]
public class SourceItemStateIncrementalModelTests
{
    [TestMethod]
    public void Compare_WhenNoChanges_ReturnsEmptyChanges()
    {
        var sourceItem = new SourceItemV2(@"x:\source", true);
        var baseline = new SourceItemState(sourceItem, [
            FileStateEx(sourceItem, "a.txt", "sha-a")
        ]);
        var current = new SourceItemState(sourceItem, [
            FileStateEx(sourceItem, "a.txt", "sha-a")
        ]);

        var result = SourceItemStateComparer.Compare([baseline], [current]);

        Assert.IsFalse(SourceItemStateComparer.IsNotEmpty(result));
        Assert.AreEqual(1, result.SourceItemChanges.Count());
        var changes = result.SourceItemChanges.Single();
        Assert.AreEqual(0, changes.DeletedFiles.Count);
        Assert.AreEqual(0, changes.UpdatedFiles.Count);
        Assert.AreEqual(0, changes.CreatedFiles.Count);
    }

    [TestMethod]
    public void Compare_WhenSourceItemAdded_ReportsCreatedFiles()
    {
        var sourceItem = new SourceItemV2(@"x:\source", true);
        var current = new SourceItemState(sourceItem, [
            FileStateEx(sourceItem, "new-1.txt", "sha-1"),
            FileStateEx(sourceItem, "new-2.txt", "sha-2")
        ]);

        var result = SourceItemStateComparer.Compare([], [current]);

        Assert.IsTrue(SourceItemStateComparer.IsNotEmpty(result));
        var changes = result.SourceItemChanges.Single();
        Assert.AreEqual(0, changes.DeletedFiles.Count);
        Assert.AreEqual(0, changes.UpdatedFiles.Count);
        Assert.AreEqual(2, changes.CreatedFiles.Count);
        CollectionAssert.AreEquivalent(
            new[]
            {
                Path.Combine(sourceItem.Target, "new-1.txt"),
                Path.Combine(sourceItem.Target, "new-2.txt"),
            },
            changes.CreatedFiles.Select(x => x.FileState.FileName).ToArray());
    }

    [TestMethod]
    public void Compare_WhenFilesChanged_ReportsCreateUpdateDelete()
    {
        var sourceItem = new SourceItemV2(@"x:\source", true);
        var baseline = new SourceItemState(sourceItem, [
            FileStateEx(sourceItem, "keep-and-update.txt", "sha-old"),
            FileStateEx(sourceItem, "to-delete.txt", "sha-delete")
        ]);
        var current = new SourceItemState(sourceItem, [
            FileStateEx(sourceItem, "keep-and-update.txt", "sha-new"),
            FileStateEx(sourceItem, "to-create.txt", "sha-create")
        ]);

        var result = SourceItemStateComparer.Compare([baseline], [current]);

        Assert.IsTrue(SourceItemStateComparer.IsNotEmpty(result));
        var changes = result.SourceItemChanges.Single();
        CollectionAssert.AreEqual(
            new[] { Path.Combine(sourceItem.Target, "to-delete.txt") },
            changes.DeletedFiles);
        Assert.AreEqual(1, changes.UpdatedFiles.Count);
        Assert.AreEqual(Path.Combine(sourceItem.Target, "keep-and-update.txt"), changes.UpdatedFiles[0].FileState.FileName);
        Assert.AreEqual("sha-new", changes.UpdatedFiles[0].FileState.Sha512);
        Assert.AreEqual(1, changes.CreatedFiles.Count);
        Assert.AreEqual(Path.Combine(sourceItem.Target, "to-create.txt"), changes.CreatedFiles[0].FileState.FileName);
    }

    [TestMethod]
    public void Build_WhenVersionContainsCreateUpdateDelete_ReconstructsSelectedState()
    {
        var sourceItem = new SourceItemV2(@"x:\source", true);

        var version1 = new VersionState(new DateTime(2024, 1, 1), [
            new SourceItemChanges(
                sourceItem,
                [],
                [],
                [
                    StorageFileEx(sourceItem, "a.txt", "sha-a-1"),
                    StorageFileEx(sourceItem, "b.txt", "sha-b-1")
                ])
        ]);
        var version2 = new VersionState(new DateTime(2024, 1, 2), [
            new SourceItemChanges(
                sourceItem,
                [Path.Combine(sourceItem.Target, "a.txt")],
                [StorageFileEx(sourceItem, "b.txt", "sha-b-2")],
                [StorageFileEx(sourceItem, "c.txt", "sha-c-1")])
        ]);

        var state = SourceItemStateBuilder.Build([version2, version1], version2);

        Assert.AreEqual(1, state.Count);
        var files = state.Single().FileStates;
        Assert.AreEqual(2, files.Count);
        CollectionAssert.AreEquivalent(
            new[]
            {
                Path.Combine(sourceItem.Target, "b.txt"),
                Path.Combine(sourceItem.Target, "c.txt"),
            },
            files.Select(x => x.FileName).ToArray());
        Assert.AreEqual("sha-b-2", files.Single(x => x.FileName.EndsWith("b.txt")).Sha512);
    }

    [TestMethod]
    public void Build_WhenSourceItemDisappearsAndReappears_ReturnsOnlyLatestLifecycleFiles()
    {
        var sourceItem = new SourceItemV2(@"x:\source", true);

        var version1 = new VersionState(new DateTime(2024, 1, 1), [
            new SourceItemChanges(sourceItem, [], [], [StorageFileEx(sourceItem, "old.txt", "sha-old")])
        ]);
        var versionWithoutSourceItem = new VersionState(new DateTime(2024, 1, 2), []);
        var version3 = new VersionState(new DateTime(2024, 1, 3), [
            new SourceItemChanges(sourceItem, [], [], [StorageFileEx(sourceItem, "new.txt", "sha-new")])
        ]);

        var state = SourceItemStateBuilder.Build([version1, versionWithoutSourceItem, version3], version3);

        Assert.AreEqual(1, state.Count);
        var files = state.Single().FileStates;
        Assert.AreEqual(1, files.Count);
        Assert.AreEqual(Path.Combine(sourceItem.Target, "new.txt"), files[0].FileName);
        Assert.AreEqual("sha-new", files[0].Sha512);
    }

    [TestMethod]
    public void Build_WhenDeleteReferencesMissingFile_Throws()
    {
        var sourceItem = new SourceItemV2(@"x:\source", true);

        var version1 = new VersionState(new DateTime(2024, 1, 1), [
            new SourceItemChanges(sourceItem, [], [], [StorageFileEx(sourceItem, "a.txt", "sha-a")])
        ]);
        var inconsistentVersion2 = new VersionState(new DateTime(2024, 1, 2), [
            new SourceItemChanges(
                sourceItem,
                [Path.Combine(sourceItem.Target, "missing.txt")],
                [],
                [])
        ]);

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            SourceItemStateBuilder.Build([version1, inconsistentVersion2], inconsistentVersion2));
    }

    private static readonly DateTime _fixedTimestamp = new(2024, 01, 01, 00, 00, 00, DateTimeKind.Utc);

    private static FileState FileStateEx(SourceItemV2 sourceItem, string name, string sha)
    {
        return new FileState(Path.Combine(sourceItem.Target, name), _fixedTimestamp, 100, sha);
    }

    private static StorageFile StorageFileEx(SourceItemV2 sourceItem, string name, string sha)
    {
        return new StorageFile(FileStateEx(sourceItem, name, sha))
        {
            StorageRelativeFileName = $"{name}.bin"
        };
    }
}
