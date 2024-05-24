using BUtil.Core.Synchronization;

namespace BUtil.Tests.Core.Synchronization;

[TestClass]
public class SynchronizationDecisionServiceTests
{

    [TestMethod("Normal update: Create file (actual) (#001)")]
    public void NormalUpdate_CreatedFile()
    {
        // Arrange
        var service = new SynchronizationDecisionService();
        var localState = new SynchronizationState();
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", DateTime.Now, "sha512", 5));
        var remoteState = new SynchronizationState();

        // Act
        var decisions = service.Decide(localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally == true);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.Created);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.NotChanged);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.DoNothing);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.Create);
    }

    [TestMethod("Normal update: No changes (#002)")]
    public void NormalUpdate_NotChangedFile()
    {
        // Arrange
        var service = new SynchronizationDecisionService();
        var localState = new SynchronizationState();
        localState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", DateTime.Now, "sha512", 5));
        var actualFiles = new SynchronizationState();
        actualFiles.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", DateTime.Now, "sha512", 5));
        var remoteState = new SynchronizationState();
        remoteState.FileSystemEntries.Add(new SynchronizationStateFile("a.txt", DateTime.Now, "sha512", 5));

        // Act
        var decisions = service.Decide(localState, actualFiles, remoteState);

        // Assert
        var item = decisions.Single();
        Assert.IsTrue(item.ExistsLocally == true);
        Assert.IsTrue(item.ActualFileToLocalStateRelation == SynchronizationRelation.NotChanged);
        Assert.IsTrue(item.RemoteStateToLocalStateRelation == SynchronizationRelation.NotChanged);
        Assert.IsTrue(item.ActualFileAction == SynchronizationDecision.DoNothing);
        Assert.IsTrue(item.RemoteAction == SynchronizationDecision.DoNothing);
    }
}
