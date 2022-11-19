using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.State
{
    public class IncrementalBackupState
    {
        public List<VersionState> VersionStates { get; set; }
        public IEnumerable<SourceItemState> LastSourceItemStates { get; set; }
        public IncrementalBackupState()
        {
            VersionStates = new List<VersionState>();
            LastSourceItemStates = new List<SourceItemState>();
        }

        public IncrementalBackupState ShallowClone()
        {
            return new IncrementalBackupState { 
                LastSourceItemStates = LastSourceItemStates
                    .Select(x => x.ShallowClone())
                    .ToList(), 
                
                VersionStates = VersionStates
            };
        }
    }
}
