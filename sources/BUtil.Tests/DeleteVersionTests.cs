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
        var s1 = new Core.ConfigurationFileModels.V2.SourceItemV2(@"x:\test", true);

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
        DeleteVersionUtil.DeleteVersion(versionsDesc, version1initial, out var storageFilesToDelete);

        // assert
        Assert.AreEqual(versionsDesc.VersionStates.Count, 1);

        var filesToDelete = storageFilesToDelete.ToArray();
        Assert.AreEqual(filesToDelete.Length, 2);
        Assert.AreEqual(filesToDelete[0], "1");
        Assert.AreEqual(filesToDelete[1], "2");
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles.Count(), 3);
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles[0].FileState.FileName, @"x:\test\4");
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles[0].StorageRelativeFileName, @"4");
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles[1].FileState.FileName, @"x:\test\2");
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles[1].StorageRelativeFileName, @"21");
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles[2].FileState.FileName, @"x:\test\3");
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles[2].StorageRelativeFileName, @"3");
        Assert.AreEqual(version2.SourceItemChanges.First().DeletedFiles.Count(), 0);
        Assert.AreEqual(version2.SourceItemChanges.First().UpdatedFiles.Count(), 0);
    }

    [TestMethod]
    public void CheckIndexing()
    {
        // set
        var s1 = new Core.ConfigurationFileModels.V2.SourceItemV2(@"x:\test", true);

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
        DeleteVersionUtil.DeleteVersion(versionsDesc, version1initial, out var storageFilesToDelete);

        // assert
        Assert.AreEqual(versionsDesc.VersionStates.Count, 1);

        var filesToDelete = storageFilesToDelete.ToArray();
        Assert.AreEqual(filesToDelete.Length, 0);
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles.Count(), 3);
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles[0].FileState.FileName, @"x:\test\4");
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles[0].StorageRelativeFileName, @"2");
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles[1].FileState.FileName, @"x:\test\1");
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles[1].StorageRelativeFileName, @"1");
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles[2].FileState.FileName, @"x:\test\2");
        Assert.AreEqual(version2.SourceItemChanges.First().CreatedFiles[2].StorageRelativeFileName, @"1");
        Assert.AreEqual(version2.SourceItemChanges.First().DeletedFiles.Count(), 0);
        Assert.AreEqual(version2.SourceItemChanges.First().UpdatedFiles.Count(), 0);
    }
}
