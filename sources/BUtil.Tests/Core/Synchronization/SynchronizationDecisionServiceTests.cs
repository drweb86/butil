﻿using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Synchronization;
using System.IO.Enumeration;

namespace BUtil.Tests.Core.Synchronization;

[TestClass]
public class SynchronizationDecisionServiceTests
{
    #region Two-Way Sync Decision Making


    [TestMethod("Normal update: Create file (actual) (#001)")]
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
        Assert.IsTrue(item.ExistsLocally == true);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.Created);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.NotChanged);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.DoNothing);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.Update);
    }

    [TestMethod("Normal update: No changes (#002)")]
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
        Assert.IsTrue(item.ExistsLocally == true);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.NotChanged);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.NotChanged);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.DoNothing);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.DoNothing);
    }

    [TestMethod("Normal update: Actual file: Exists (No changes comparing to state), Remote in Local: Changed (#003)")]
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
        Assert.IsTrue(item.ExistsLocally == true);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.NotChanged);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.Changed);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.Update);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.DoNothing);
    }

    [TestMethod("Normal update: Actual file: Exists (No changes comparing to state), Remote in Local: Deleted (#004)")]
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
        Assert.IsTrue(item.ExistsLocally == true);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.NotChanged);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.Deleted);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.Delete);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.DoNothing);
    }

    [TestMethod("Normal update: Actual file: Created (comparing to state), Remote in Local: Created (#005) - CONFLICT RESOLUTION - Wins Remote")]
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
        Assert.IsTrue(item.ExistsLocally == true);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.Created);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.Created);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.Update);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.DoNothing);
    }

    [TestMethod("Normal update: Actual file: Created (comparing to state), Remote in Local: Created (#005) - CONFLICT RESOLUTION - Wins Local")]
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
        Assert.IsTrue(item.ExistsLocally == true);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.Created);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.Created);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.DoNothing);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.Update);
    }

    [TestMethod("Normal update: Actual file: Modified (comparing to state), Remote in Local: Unchanged (#006)")]
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
        Assert.IsTrue(item.ExistsLocally == true);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.Changed);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.NotChanged);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.DoNothing);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.Update);
    }

    [TestMethod("Normal update: Actual file: Modified (comparing to state), Remote in Local: Modified - CONFLICTS - Remote Wins (#007)")]
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
        Assert.IsTrue(item.ExistsLocally == true);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.Changed);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.Changed);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.Update);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.DoNothing);
    }

    [TestMethod("Normal update: Actual file: Modified (comparing to state), Remote in Local: Modified - CONFLICTS - Local Wins (#007)")]
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
        Assert.IsTrue(item.ExistsLocally == true);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.Changed);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.Changed);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.DoNothing);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.Update);
    }

    [TestMethod("Normal update: Actual file: Modified (comparing to state), Remote in Local: Deleted (#008)")]
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
        Assert.IsTrue(item.ExistsLocally == true);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.Changed);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.Deleted);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.DoNothing);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.Update);
    }

    [TestMethod("Normal update: Actual file: Deleted (comparing to state), Remote in Local: Unchanged (#009)")]
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
        Assert.IsTrue(item.ExistsLocally == false);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.Deleted);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.NotChanged);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.DoNothing);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.Delete);
    }

    [TestMethod("Normal update: Actual file: Deleted (comparing to state), Remote in Local: Deleted (#011)")]
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
        Assert.IsTrue(item.ExistsLocally == false);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.Deleted);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.Deleted);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.DoNothing);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.DoNothing);
    }

    [TestMethod("Normal update: Actual file: Deleted (comparing to state), Remote in Local: Changed (#010)")]
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
        Assert.IsTrue(item.ExistsLocally == false);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.Deleted);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.Changed);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.Update);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.DoNothing);
    }

    [TestMethod("Subfolder matching")] // #012
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
        Assert.IsTrue(decisions.Count() == 3);
        Assert.IsTrue(decisions.All(x => x.ActualFileAction == SynchronizationDecision.Update));
    }

    #endregion

    #region Readonly Sync Decision Making

    [TestMethod("Readonly updates")]
    [DataRow(SynchronizationRelation.NotChanged, SynchronizationRelation.NotChanged, SynchronizationDecision.DoNothing)] // #1
    [DataRow(SynchronizationRelation.NotChanged, SynchronizationRelation.Created, SynchronizationDecision.Update)] // #2 ....
    [DataRow(SynchronizationRelation.NotChanged, SynchronizationRelation.Changed, SynchronizationDecision.Update)]
    [DataRow(SynchronizationRelation.NotChanged, SynchronizationRelation.Deleted, SynchronizationDecision.Delete)]
    [DataRow(SynchronizationRelation.Created, SynchronizationRelation.NotChanged, SynchronizationDecision.DoNothing)] // #5
    [DataRow(SynchronizationRelation.Created, SynchronizationRelation.Created, SynchronizationDecision.Update)]
    [DataRow(SynchronizationRelation.Created, SynchronizationRelation.Changed, SynchronizationDecision.Update)]
    [DataRow(SynchronizationRelation.Created, SynchronizationRelation.Deleted, SynchronizationDecision.Delete)]
    [DataRow(SynchronizationRelation.Changed, SynchronizationRelation.NotChanged, SynchronizationDecision.Update)] //#9
    [DataRow(SynchronizationRelation.Changed, SynchronizationRelation.Created, SynchronizationDecision.Update)]
    [DataRow(SynchronizationRelation.Changed, SynchronizationRelation.Changed, SynchronizationDecision.Update)]
    [DataRow(SynchronizationRelation.Changed, SynchronizationRelation.Deleted, SynchronizationDecision.Delete)]  // #12
    [DataRow(SynchronizationRelation.Deleted, SynchronizationRelation.NotChanged, SynchronizationDecision.Update)]
    [DataRow(SynchronizationRelation.Deleted, SynchronizationRelation.Created, SynchronizationDecision.Update)]
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
                actualFiles.FileSystemEntries.Add(fileSystemEntry);
                localState.FileSystemEntries.Add(fileSystemEntry);
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
                if (actualFile != SynchronizationRelation.Deleted)
                {
                    remoteState.FileSystemEntries.Add(fileSystemEntry);
                }
                break;
            case SynchronizationRelation.Created:
                break;
            case SynchronizationRelation.Changed:
                break;
            case SynchronizationRelation.Deleted:
                break;

        }

        // Act
        var decisions = service.Decide(SynchronizationTaskModelMode.Read, localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally == (actualFile != SynchronizationRelation.Deleted));
        Assert.IsTrue(item.ActualFileToLocalStateRelation == actualFile);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == remoteFile);
        Assert.IsTrue(item.ActualFileAction == actualFileAction);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.DoNothing);
    }

    #endregion
}
