using System.Collections.Generic;

namespace BUtil.Core.State
{
    public class ImportMediaState
    {
        public ImportMediaState()
        {
            Version = 1;
            Files = new List<string>();
        }

        public ImportMediaState(IEnumerable<string> files)
        {
            Files.AddRange(files);
        }

        public int Version { get; set; }
        public List<string> Files { get; set; }
    }
}
