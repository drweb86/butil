using BUtil.Tasks.Synchronization.Synchronization;

namespace BUtil.Tasks.Synchronization.TasksTree.Synchronization;
class SynchronizationLocalState
{
    public string LocalFolder { get; set; } = null!;
    public string? RepositorySubfolder { get; set; }
    public SynchronizationState SynchronizationState { get; set; } = null!;
}
