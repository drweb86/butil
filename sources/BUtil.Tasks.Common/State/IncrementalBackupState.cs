using System.Collections.Generic;

namespace BUtil.Core.State;

public class IncrementalBackupState
{
    public List<VersionState> VersionStates { get; set; }
    public List<SourceItemState> LastSourceItemStates { get; set; }
    public IncrementalBackupState()
    {
        VersionStates = [];
        LastSourceItemStates = [];
    }
}