using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Synchronization;
using System;

namespace BUtil.Core.TasksTree.IncrementalModel;

class SynchronizationModel
{
    public SynchronizationState RemoteState { get; internal set; } = null!;
    public SynchronizationState LocalState { get; internal set; } = null!;
    public SynchronizationState ActualFiles { get; internal set; } = null!;
    public SynchronizationTaskModelOptionsV2 TaskOptions { get; internal set; } = null!;

    public SourceItemV2 ToSourceItem()
    {
        return new SourceItemV2(TaskOptions.LocalFolder, true) { Id = Guid.Empty };
    }
}
