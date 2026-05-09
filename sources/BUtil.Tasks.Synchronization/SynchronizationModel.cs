using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.State;
using BUtil.Core.Synchronization;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.Synchronization;

class SynchronizationModel
{
    public IncrementalBackupState RemoteStorageState { get; set; } = null!;
    public SynchronizationState RemoteState { get; set; } = null!;
    public List<StorageFile> RemoteStorageFiles { get; set; } = null!;
    public SourceItemV2 RemoteSourceItem { get; set; } = null!;


    public SynchronizationState LocalState { get; set; } = null!;
    public SynchronizationState ActualFiles { get; set; } = null!;

    public SynchronizationTaskModelOptionsV2 TaskOptions { get; }

    public SynchronizationModel(SynchronizationTaskModelOptionsV2 taskOptions)
    {
        TaskOptions = taskOptions;
        LocalSourceItem = new SourceItemV2(TaskOptions.LocalFolder, true) { Id = SynchronizationHelper.SynchronizationSourceItemId };
    }

    public SourceItemV2 LocalSourceItem { get; }
    public static SourceItemV2 CreateVirtualSourceItem()
    {
        return new SourceItemV2("x:\\", true) { Id = SynchronizationHelper.SynchronizationSourceItemId };
    }
}
