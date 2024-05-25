using BUtil.Core.Synchronization;

namespace BUtil.Core.TasksTree.Synchronization;
class SynchronizationLocalState
{
    public string LocalFolder { get; set; } = null!;
    public string? Subfolder { get; set; }
    public SynchronizationState SynchronizationState { get; set; } = null!;
}
