using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Synchronization;
[assembly: Parallelize]
namespace BUtil.Tests.Core.Synchronization;

[TestClass]
public class SynchronizationDecisionServiceTests
{
    #region Two-Way Sync Decision Making


    [TestMethod(DisplayName = "Normal update: Create file (actual) (#001)")]
    public void NormalUpdate_CreatedFile()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var localState = new SynchronizationState();
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", DateTime.Now, "sha512", 5));
        var remoteState = new SynchronizationState();

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.Created, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.NotChanged, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.Update, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Normal update: No changes (#002)")]
    public void NormalUpdate_NotChangedFile()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var localState = new SynchronizationState();
        var timeStamp = DateTime.Now;
        localState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var remoteState = new SynchronizationState();
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.NotChanged, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.NotChanged, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Normal update: Actual file: Exists (No changes comparing to state), Remote in Local: Changed (#003)")]
    public void NormalUpdate_AF_Exists_NoChanges_Remote_Created()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var timeStamp = DateTime.Now;
        var localState = new SynchronizationState();
        localState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var remoteState = new SynchronizationState();
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp.AddSeconds(1), "sha512-Modified", 5));

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.NotChanged, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.Changed, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.Update, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Normal update: Actual file: Exists (No changes comparing to state), Remote in Local: Deleted (#004)")]
    public void NormalUpdate_AF_Exists_NoChanges_Remote_Deleted()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var timeStamp = DateTime.Now;
        var localState = new SynchronizationState();
        localState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var remoteState = new SynchronizationState();

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.NotChanged, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.Deleted, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.Delete, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Normal update: Actual file: Created (comparing to state), Remote in Local: Created (#005) - CONFLICT RESOLUTION - Wins Remote")]
    public void NormalUpdate_AF_Created_Remote_Created_Wins_Remote()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var timeStamp = DateTime.Now;
        var localState = new SynchronizationState();
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var remoteState = new SynchronizationState();
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp.AddSeconds(1), "sha512-modified", 5));

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.Created, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.Created, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.Update, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Normal update: Actual file: Created (comparing to state), Remote in Local: Created (#005) - CONFLICT RESOLUTION - Wins Local")]
    public void NormalUpdate_AF_Created_Remote_Created_Wins_Local()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var timeStamp = DateTime.Now;
        var localState = new SynchronizationState();
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var remoteState = new SynchronizationState();
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp.AddSeconds(-1), "sha512-modified", 5));

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.Created, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.Created, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.Update, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Normal update: Actual file: Modified (comparing to state), Remote in Local: Unchanged (#006)")]
    public void NormalUpdate_AF_Modified_Remote_Unchanged()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var timeStamp = DateTime.Now;
        var localState = new SynchronizationState();
        localState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512-2", 5));
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var remoteState = new SynchronizationState();
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512-2", 5));

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.Changed, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.NotChanged, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.Update, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Normal update: Actual file: Modified (comparing to state), Remote in Local: Modified - CONFLICTS - Remote Wins (#007)")]
    public void NormalUpdate_AF_Modified_Remote_Modified_Remote_Wins()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var timeStamp = DateTime.Now;
        var localState = new SynchronizationState();
        localState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512-2", 5));
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var remoteState = new SynchronizationState();
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp.AddSeconds(1), "sha512-3", 5));

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.Changed, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.Changed, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.Update, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Normal update: Actual file: Modified (comparing to state), Remote in Local: Modified - CONFLICTS - Local Wins (#007)")]
    public void NormalUpdate_AF_Modified_Remote_Modified_Local_Wins()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var timeStamp = DateTime.Now;
        var localState = new SynchronizationState();
        localState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512-2", 5));
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var remoteState = new SynchronizationState();
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp.AddSeconds(-1), "sha512-3", 5));

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.Changed, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.Changed, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.Update, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Normal update: Actual file: Modified (comparing to state), Remote in Local: Deleted (#008)")]
    public void NormalUpdate_AF_Modified_Remote_Deleted()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var timeStamp = DateTime.Now;
        var localState = new SynchronizationState();
        localState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512-2", 5));
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var remoteState = new SynchronizationState();

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.Changed, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.Deleted, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.Update, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Normal update: Actual file: Deleted (comparing to state), Remote in Local: Unchanged (#009)")]
    public void NormalUpdate_AF_Deleted_Remote_Unchanged()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var timeStamp = DateTime.Now;
        var localState = new SynchronizationState();
        localState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var actualFiles = new SynchronizationState();
        var remoteState = new SynchronizationState();
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsFalse(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.Deleted, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.NotChanged, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.Delete, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Normal update: Actual file: Deleted (comparing to state), Remote in Local: Deleted (#011)")]
    public void NormalUpdate_AF_Deleted_Remote_Deleted()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var timeStamp = DateTime.Now;
        var localState = new SynchronizationState();
        localState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var actualFiles = new SynchronizationState();
        var remoteState = new SynchronizationState();

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsFalse(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.Deleted, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.Deleted, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Normal update: Actual file: Deleted (comparing to state), Remote in Local: Changed (#010)")]
    public void NormalUpdate_AF_Deleted_Remote_Changed()
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var timeStamp = DateTime.Now;
        var localState = new SynchronizationState();
        localState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp, "sha512", 5));
        var actualFiles = new SynchronizationState();
        var remoteState = new SynchronizationState();
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", timeStamp.AddSeconds(1), "sha512-2", 5));


        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsFalse(item.ExistsLocally);
        Assert.AreEqual(SynchronizationRelation.Deleted, item.ActualFileToLocalStateRelation);
        Assert.AreEqual(SynchronizationRelation.Changed, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(SynchronizationDecision.Update, item.ActualFileAction);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.RemoteAction);
    }

    [TestMethod(DisplayName = "Subfolder matching")] // #012
    public void Subfolder_Matching()
    {
        // Arrange
        var service = new SynchronizationDecisionService("\\1/2///\\\\");
        var localState = new SynchronizationState();
        localState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", DateTime.Now, "sha512", 5));
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", DateTime.Now, "sha512", 5));
        var remoteState = new SynchronizationState();
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("1.txt", DateTime.Now, "sha512", 5));
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("1\\2.txt", DateTime.Now, "sha512", 5));
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("1/2.txt", DateTime.Now, "sha512", 5));
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("1/2/1.txt", DateTime.Now, "sha512", 5));
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("1\\2\\2.txt", DateTime.Now, "sha512", 5));
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("1\\2\\a.txt", DateTime.Now, "sha512-1", 6));

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.TwoWay, localState, actualFiles, remoteState);

        // Assert
        Assert.AreEqual(3, decisions.Count());
        Assert.IsTrue(decisions.All(x => x.ActualFileAction == SynchronizationDecision.Update));
    }

    #endregion

    #region Readonly Sync Decision Making

    [TestMethod(DisplayName = "Readonly updates")]
    [DataRow(SynchronizationRelation.NotChanged, SynchronizationRelation.NotChanged, SynchronizationDecision.DoNothing)] // #1
    [DataRow(SynchronizationRelation.NotChanged, SynchronizationRelation.Created, SynchronizationDecision.Update)] // #2 ....
    [DataRow(SynchronizationRelation.NotChanged, SynchronizationRelation.Changed, SynchronizationDecision.Update)]
    [DataRow(SynchronizationRelation.NotChanged, SynchronizationRelation.Deleted, SynchronizationDecision.Delete)]
    [DataRow(SynchronizationRelation.Created, SynchronizationRelation.NotChanged, SynchronizationDecision.DoNothing)] // #5
    [DataRow(SynchronizationRelation.Created, SynchronizationRelation.Created, SynchronizationDecision.Update)]
    //[DataRow(SynchronizationRelation.Created, SynchronizationRelation.Changed, SynchronizationDecision.Update)] // not possible
    //[DataRow(SynchronizationRelation.Created, SynchronizationRelation.Deleted, SynchronizationDecision.Delete)] // not possible
    [DataRow(SynchronizationRelation.Changed, SynchronizationRelation.NotChanged, SynchronizationDecision.Update)] //#9
    //[DataRow(SynchronizationRelation.Changed, SynchronizationRelation.Created, SynchronizationDecision.Update)] // not possible
    [DataRow(SynchronizationRelation.Changed, SynchronizationRelation.Changed, SynchronizationDecision.Update)]
    [DataRow(SynchronizationRelation.Changed, SynchronizationRelation.Deleted, SynchronizationDecision.Delete)]  // #12
    [DataRow(SynchronizationRelation.Deleted, SynchronizationRelation.NotChanged, SynchronizationDecision.Update)]
    //[DataRow(SynchronizationRelation.Deleted, SynchronizationRelation.Created, SynchronizationDecision.Update)] // not possible
    [DataRow(SynchronizationRelation.Deleted, SynchronizationRelation.Changed, SynchronizationDecision.Update)]
    [DataRow(SynchronizationRelation.Deleted, SynchronizationRelation.Deleted, SynchronizationDecision.DoNothing)] // #16
    public void ReadonlyUpdates(
        SynchronizationRelation actualFile,
        SynchronizationRelation remoteFile,

        SynchronizationDecision actualFileAction)
    {
        // Arrange
        var service = new SynchronizationDecisionService(null);
        var localState = new SynchronizationState();
        var actualFiles = new SynchronizationState();
        var remoteState = new SynchronizationState();

        var fileSystemEntry = new SynchronizationStateFile("a.txt", DateTime.MinValue, "sha", 1);

        switch (actualFile)
        {
            case SynchronizationRelation.NotChanged:
                if (remoteFile != SynchronizationRelation.Created)
                {
                    actualFiles.FileSystemEntries.Add(fileSystemEntry);
                    localState.FileSystemEntries.Add(fileSystemEntry);
                }
                break;
            case SynchronizationRelation.Created:
                actualFiles.FileSystemEntries.Add(fileSystemEntry);
                break;
            case SynchronizationRelation.Changed:
                var changed = fileSystemEntry.Clone();
                changed.Sha512 = "sha-actual-file";
                actualFiles.FileSystemEntries.Add(changed);
                localState.FileSystemEntries.Add(fileSystemEntry);
                break;
            case SynchronizationRelation.Deleted:
                localState.FileSystemEntries.Add(fileSystemEntry);
                break;
        }

        switch (remoteFile)
        {
            case SynchronizationRelation.NotChanged:
                if (actualFile != SynchronizationRelation.Deleted &&
                    actualFile != SynchronizationRelation.Created)
                {
                    remoteState.FileSystemEntries.Add(fileSystemEntry);
                }

                if (actualFile == SynchronizationRelation.Deleted)
                {
                    remoteState.FileSystemEntries.Add(fileSystemEntry);
                }
                break;
            case SynchronizationRelation.Created:
                var changed1 = fileSystemEntry.Clone();
                changed1.Sha512 = "sha-actual-file-remote";
                remoteState.FileSystemEntries.Add(changed1);
                break;
            case SynchronizationRelation.Changed:
                var changed = fileSystemEntry.Clone();
                changed.Sha512 = "sha-actual-file-remote";
                remoteState.FileSystemEntries.Add(changed);
                break;
            case SynchronizationRelation.Deleted:
                break;

        }

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.Read, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.AreEqual(actualFile != SynchronizationRelation.Deleted && !(actualFile == SynchronizationRelation.NotChanged && remoteFile == SynchronizationRelation.Created), item.ExistsLocally);
        Assert.AreEqual(item.ActualFileToLocalStateRelation, actualFile);
        Assert.AreEqual(remoteFile, item.RemoteStateToLocalStateRelation);
        Assert.AreEqual(item.ActualFileAction, actualFileAction);
        Assert.AreEqual(SynchronizationDecision.DoNothing, item.RemoteAction);
    }

    #endregion
}

