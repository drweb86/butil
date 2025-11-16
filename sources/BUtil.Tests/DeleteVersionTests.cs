using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.State;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Tests;

[TestClass]
public class DeleteVersionTests
{
    private StorageFile StorageFileEx(SourceItemV2 item, int fileId, int storageFileId)
    {
        return new StorageFile(
            new FileState(Path.Combine(item.Target, fileId.ToString()), DateTime.MinValue, 0, string.Empty)) { 
            StorageRelativeFileName = storageFileId.ToString() };
    }

    [TestMethod]
    public void Delete2Version()
    {
        // set
        var s1 = new SourceItemV2(@"x:\test", true);

        var version1initial = new VersionState(new DateTime(2000, 1, 1), [
            new SourceItemChanges(
                s1,
                new List<string>() { }, // deleted
                new List<StorageFile> { }, // updated
                // created
                new List<StorageFile>
                {
                    StorageFileEx(s1, 1, 1),
                    StorageFileEx(s1, 2, 2),
                    StorageFileEx(s1, 3, 3),
                }
            ) ]);

        var version2 = new VersionState(new DateTime(3000, 1, 1), new[] {
            new SourceItemChanges(
                s1,
                // deleted
                new List<string>() {
                    Path.Combine(s1.Target, @"1"),
                },
                // updated
                new List<StorageFile> { StorageFileEx(s1, 2, 21) },
                // created
                new List<StorageFile> { StorageFileEx(s1, 4, 4) }
            ) });

        var versionsDesc = new IncrementalBackupState();
        versionsDesc.VersionStates.Add(version1initial);
        versionsDesc.VersionStates.Add(version2);

        // act
        DeleteVersionUtil.DeleteVersion(versionsDesc, version1initial, out var storageFilesToDelete, out var patch);
        var filesToDelete = storageFilesToDelete.ToArray();

        // assert
        Assert.HasCount(1, versionsDesc.VersionStates);
        Assert.HasCount(2, filesToDelete);
        Assert.AreEqual("1", filesToDelete[0]);
        Assert.AreEqual("2", filesToDelete[1]);
        Assert.AreEqual(3, version2.SourceItemChanges.First().CreatedFiles.Count());
        Assert.AreEqual(@"x:\test\4", version2.SourceItemChanges.First().CreatedFiles[0].FileState.FileName);
        Assert.AreEqual(@"4", version2.SourceItemChanges.First().CreatedFiles[0].StorageRelativeFileName);
        Assert.AreEqual(@"x:\test\2", version2.SourceItemChanges.First().CreatedFiles[1].FileState.FileName);
        Assert.AreEqual(@"21", version2.SourceItemChanges.First().CreatedFiles[1].StorageRelativeFileName);
        Assert.AreEqual(@"x:\test\3", version2.SourceItemChanges.First().CreatedFiles[2].FileState.FileName);
        Assert.AreEqual(@"3", version2.SourceItemChanges.First().CreatedFiles[2].StorageRelativeFileName);
        Assert.AreEqual(0, version2.SourceItemChanges.First().DeletedFiles.Count());
        Assert.AreEqual(0, version2.SourceItemChanges.First().UpdatedFiles.Count());
    }

    [TestMethod]
    public void CheckIndexing()
    {
        // set
        var s1 = new SourceItemV2(@"x:\test", true);

        var version1initial = new VersionState(new DateTime(2000, 1, 1), [
            new SourceItemChanges(
                s1,
                new List<string>() { }, // deleted
                new List<StorageFile> { }, // updated
                // created
                new List<StorageFile>
                {
                    StorageFileEx(s1, 1, 1),
                    StorageFileEx(s1, 2, 2),
                }
            ) ]);

        var version2 = new VersionState(new DateTime(3000, 1, 1), new[] {
            new SourceItemChanges(
                s1,
                // deleted
                new List<string>(),
                // updated
                new List<StorageFile> { StorageFileEx(s1, 2, 1) },
                // created
                new List<StorageFile> { StorageFileEx(s1, 4, 2) }
            ) });

        var versionsDesc = new IncrementalBackupState();
        versionsDesc.VersionStates.Add(version1initial);
        versionsDesc.VersionStates.Add(version2);

        // act
        DeleteVersionUtil.DeleteVersion(versionsDesc, version1initial, out var storageFilesToDelete, out var patch);
        var filesToDelete = storageFilesToDelete.ToArray();

        // assert
        Assert.HasCount(1, versionsDesc.VersionStates);
        Assert.IsEmpty(filesToDelete);
        Assert.AreEqual(3, version2.SourceItemChanges.First().CreatedFiles.Count());
        Assert.AreEqual(@"x:\test\4", version2.SourceItemChanges.First().CreatedFiles[0].FileState.FileName);
        Assert.AreEqual(@"2", version2.SourceItemChanges.First().CreatedFiles[0].StorageRelativeFileName);
        Assert.AreEqual(@"x:\test\1", version2.SourceItemChanges.First().CreatedFiles[1].FileState.FileName);
        Assert.AreEqual(@"1", version2.SourceItemChanges.First().CreatedFiles[1].StorageRelativeFileName);
        Assert.AreEqual(@"x:\test\2", version2.SourceItemChanges.First().CreatedFiles[2].FileState.FileName);
        Assert.AreEqual(@"1", version2.SourceItemChanges.First().CreatedFiles[2].StorageRelativeFileName);
        Assert.AreEqual(0, version2.SourceItemChanges.First().DeletedFiles.Count());
        Assert.AreEqual(0, version2.SourceItemChanges.First().UpdatedFiles.Count());
    }
}
