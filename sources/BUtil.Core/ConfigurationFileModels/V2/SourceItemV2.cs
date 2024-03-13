using System;

namespace BUtil.Core.ConfigurationFileModels.V2;

public class SourceItemV2
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public SourceItemV2() : this(string.Empty, false) { } // deserialization

    public SourceItemV2(string target, bool isFolder)
    {
        Target = target;
        IsFolder = isFolder;
    }

    public string Target { get; set; }

    public bool IsFolder { get; set; }

    public bool CompareTo(SourceItemV2 x)
    {
        return
            x.Target == Target &&
            x.IsFolder == IsFolder;
    }
}
