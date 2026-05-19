using BUtil.Interop.Tasks;
namespace BUtil.Interop.Tasks;

public class TaskV2
{
    public required ITaskModelOptionsV2 Model { get; set; }

    public string Name { get; set; } = string.Empty;
}
