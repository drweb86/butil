namespace BUtil.Core.Synchronization;

using System.Diagnostics;

[DebuggerDisplay("{RelativeFileName}: ActualFileAction = {ActualFileAction}, ActualFileAction={ActualFileAction}, ForceUpdateState={ForceUpdateState}")]
class SynchronizationConsolidatedFileInfo
{
    public SynchronizationConsolidatedFileInfo() { } // deserialization

    public SynchronizationConsolidatedFileInfo(string relativeFileName)
    {
        RelativeFileName = relativeFileName;
    }

    public string RelativeFileName { get; set; } = null!;

    // facts
    public SynchronizationStateFile? ActualFile { get; set; }
    public SynchronizationStateFile? LocalState { get; set; }
    public SynchronizationStateFile? RemoteState { get; set; }

    // relations
    public SynchronizationRelation ActualFileToLocalStateRelation { get; set; }
    public SynchronizationRelation RemoteStateToLocalStateRelation { get; set; }

    // actions
    public bool ExistsLocally => ActualFile != null;
    public SynchronizationDecision ActualFileAction { get; set; }
    public SynchronizationDecision RemoteAction { get; set; }

    // special cases
    public bool ForceUpdateState { get; set; }
}
