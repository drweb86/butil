using System.Collections.Generic;

namespace BUtil.Core.State
{
    public class IncrementalBackupState
    {
        public List<VersionState> VersionStates { get; set; }
        public IEnumerable<SourceItemState> LastSourceItemStates { get; set; }
        public IncrementalBackupState(): this(new List<VersionState>(), new List<SourceItemState>()) { }
        public IncrementalBackupState(List<VersionState> versionStates, IEnumerable<SourceItemState> lastSourceItemStates)
        {
            VersionStates = versionStates;
            LastSourceItemStates = lastSourceItemStates;
        }
    }
}
