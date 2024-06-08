using BUtil.Core.ConfigurationFileModels.V2;
using System;

namespace BUtil.Core.TasksTree.Synchronization;

internal static class SynchronizationHelper
{
    public static Guid SynchronizationSourceItemId = Guid.Empty;

    public static bool IsSynchronizationSourceItem(SourceItemV2 sourceItem)
    {
        return sourceItem.IsFolder && sourceItem.Id == SynchronizationSourceItemId;
    }
}
