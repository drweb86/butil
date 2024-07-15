using System;

namespace BUtil.Core.ConfigurationFileModels.V1;

public class SourceItemV1(string target, bool isFolder)
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public SourceItemV1() : this(string.Empty, false) { } // deserialization

    public string Target { get; set; } = target;

    public bool IsFolder { get; set; } = isFolder;

    public bool CompareTo(SourceItemV1 x)
    {
        return
            x.Target == Target &&
            x.IsFolder == IsFolder;
    }
}