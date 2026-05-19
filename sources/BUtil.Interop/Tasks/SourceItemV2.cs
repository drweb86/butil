namespace BUtil.Interop.Tasks;

public class SourceItemV2(string target, bool isFolder)
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public SourceItemV2() : this(string.Empty, false) { }

    public string Target { get; set; } = target;

    public bool IsFolder { get; set; } = isFolder;

    public bool CompareTo(SourceItemV2 x)
    {
        return
            x.Target == Target &&
            x.IsFolder == IsFolder;
    }
}
