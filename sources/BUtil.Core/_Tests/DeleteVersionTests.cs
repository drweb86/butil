
using BUtil.Core.State;
using BUtil.Core.TasksTree.IncrementalModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BUtil.Core._Tests
{
    [TestClass]
    public class DeleteVersionTests
    {
        private StorageFile StorageFileEx(int fileId, int storageFileId)
        {
            return new StorageFile(new FileState(fileId.ToString(), DateTime.MinValue, 0, string.Empty)) { StorageFileName = storageFileId.ToString() };
        }

        [TestMethod]
        public void DeleteLastVersion()
        {
            // set
            var sourceItem1 = new ConfigurationFileModels.V2.SourceItemV2(@"x:\test", true);

            var version1 = new VersionState(new System.DateTime(2000, 1, 1), new[] {
                new SourceItemChanges(
                    sourceItem1,
                    // deleted
                    new System.Collections.Generic.List<string>() { @"x:\test\deleted" },
                    // updated
                    new System.Collections.Generic.List<StorageFile> { StorageFileEx(1, 1) },
                    // created
                    new System.Collections.Generic.List<StorageFile> { StorageFileEx(2, 2) }
                ) } );

            var version2 = new VersionState(new System.DateTime(3000, 1, 1), new[] {
                new SourceItemChanges(
                    sourceItem1,
                    // deleted
                    new System.Collections.Generic.List<string>() { @"x:\test\updated1" },
                    // updated
                    new System.Collections.Generic.List<StorageFile> { StorageFileEx(2, 2) },
                    // created
                    new System.Collections.Generic.List<StorageFile> { StorageFileEx(2, 3) }
                ) });
            var version3 = new VersionState(new System.DateTime(4000, 1, 1), new[] {
                new SourceItemChanges(
                    sourceItem1,
                    // deleted
                    new System.Collections.Generic.List<string>() { @"x:\test\updated1" },
                    // updated
                    new System.Collections.Generic.List<StorageFile> { StorageFileEx(1, 3) },
                    // created
                    new System.Collections.Generic.List<StorageFile> { StorageFileEx(2, 4) }

                ) });

            var versions = new IncrementalBackupState();
            versions.VersionStates.Add(version1);
            versions.VersionStates.Add(version2);
            versions.VersionStates.Add(version3);

            // act
            DeleteVersionUtil.DeleteVersion(versions, version3, out var storageFilesToDelete);

            // assert
            Assert.AreEqual(versions.VersionStates.Count, 2);

            var filesToDelete = storageFilesToDelete.ToArray();
            Assert.AreEqual(filesToDelete.Length, 1);
            Assert.AreEqual(filesToDelete[0], 4.ToString());
        }



        [TestMethod]
        public void DeleteFirstVersion()
        {
            // set
            var sourceItem1 = new ConfigurationFileModels.V2.SourceItemV2(@"x:\test", true);

            var version1 = new VersionState(new System.DateTime(2000, 1, 1), new[] {
                new SourceItemChanges(
                    sourceItem1,
                    // deleted
                    new System.Collections.Generic.List<string>() { @"x:\test\deleted" },
                    // updated
                    new System.Collections.Generic.List<StorageFile> { StorageFileEx(1, 1) },
                    // created
                    new System.Collections.Generic.List<StorageFile> { StorageFileEx(2, 2) }
                ) });

            var version2 = new VersionState(new System.DateTime(3000, 1, 1), new[] {
                new SourceItemChanges(
                    sourceItem1,
                    // deleted
                    new System.Collections.Generic.List<string>() { @"x:\test\updated1" },
                    // updated
                    new System.Collections.Generic.List<StorageFile> { StorageFileEx(2, 2) },
                    // created
                    new System.Collections.Generic.List<StorageFile> { StorageFileEx(2, 3) }
                ) });
            var version3 = new VersionState(new System.DateTime(4000, 1, 1), new[] {
                new SourceItemChanges(
                    sourceItem1,
                    // deleted
                    new System.Collections.Generic.List<string>() { @"x:\test\updated1" },
                    // updated
                    new System.Collections.Generic.List<StorageFile> { StorageFileEx(1, 3) },
                    // created
                    new System.Collections.Generic.List<StorageFile> { StorageFileEx(2, 4) }

                ) });

            var versions = new IncrementalBackupState();
            versions.VersionStates.Add(version1);
            versions.VersionStates.Add(version2);
            versions.VersionStates.Add(version3);

            // act
            DeleteVersionUtil.DeleteVersion(versions, version1, out var storageFilesToDelete);

            // assert
            Assert.AreEqual(versions.VersionStates.Count, 2);

            var filesToDelete = storageFilesToDelete.ToArray();
            Assert.AreEqual(filesToDelete.Length, 1);
            Assert.AreEqual(filesToDelete[0], 4.ToString());
        }
    }
}
