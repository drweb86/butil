#nullable disable
using System;
using System.Collections.Generic;

namespace BUtil.Core.State
{

    public class VersionState
    {
        public DateTime BackupDateUtc { get; set; }
        public IEnumerable<SourceItemChanges> SourceItemChanges { get; set;}
        public VersionState() { } // deserialization

        public VersionState(DateTime backupDateUtc, IEnumerable<SourceItemChanges> sourceItemChanges)
        {
            BackupDateUtc = backupDateUtc;
            SourceItemChanges = sourceItemChanges;
        }
    }
}
