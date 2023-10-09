
using System.Collections.Generic;

namespace BUtil.Core.State
{
    public class ImportMediaState
    {
        public ImportMediaState()
        {
        }

        public ImportMediaState(IEnumerable<string> files)
        {
            Files.AddRange(files);
        }

        public List<string> Files { get; set; } = new();
    }
}
