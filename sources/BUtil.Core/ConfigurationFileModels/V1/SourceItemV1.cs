using System;

namespace BUtil.Core.ConfigurationFileModels.V1;

public class SourceItemV1
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public SourceItemV1() : this(string.Empty, false) { } // deserialization

    public SourceItemV1(string target, bool isFolder)
    {
        Target = target;
        IsFolder = isFolder;
    }

    public string Target { get; set; }

    public bool IsFolder { get; set; }

    public bool CompareTo(SourceItemV1 x)
    {
        return
            x.Target == Target &&
            x.IsFolder == IsFolder;
    }
}