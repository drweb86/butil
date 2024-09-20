
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.States;

internal class GetStateOfSourceItemsAndStoragesTask : ParallelBuTask
{
    public IEnumerable<GetStateOfSourceItemTask> GetSourceItemStateTasks { get; }
    public RemoteStateLoadTask RemoteStateLoadTask { get; }
    public DeleteUnversionedFilesStorageTask DeleteUnversionedFilesStorageTask { get; }

    public GetStateOfSourceItemsAndStoragesTask(
        TaskEvents events,
        IEnumerable<SourceItemV2> sourceItems,
        CommonServicesIoc commonServicesIoc,
        StorageSpecificServicesIoc servicesIoc,
        IEnumerable<string> fileExcludePatterns,
        string password)
        : base(commonServicesIoc.Log, events, Localization.Resources.State_LoadFromEverywhere, null)
    {
        var childTasks = new List<BuTask>();
        var setSourceItemStateTasks = new List<GetStateOfSourceItemTask>();

        foreach (var item in sourceItems)
        {
            var getSourceItemStateTask = new GetStateOfSourceItemTask(Events, item, fileExcludePatterns, commonServicesIoc);
            setSourceItemStateTasks.Add(getSourceItemStateTask);
            childTasks.Add(getSourceItemStateTask);
        }
        GetSourceItemStateTasks = setSourceItemStateTasks;

        var getStorageStateTask = new RemoteStateLoadTask(servicesIoc, Events, password);
        RemoteStateLoadTask = getStorageStateTask;
        childTasks.Add(getStorageStateTask);

        var deleteUnversionedFilesStorageTask = new DeleteUnversionedFilesStorageTask(servicesIoc, Events, getStorageStateTask);
        DeleteUnversionedFilesStorageTask = deleteUnversionedFilesStorageTask;
        childTasks.Add(deleteUnversionedFilesStorageTask);

        Children = childTasks;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        var storageTasksExecuter = new ParallelExecuter([RemoteStateLoadTask], 1);
        var sourceItemGroupTasks = GetSourceItemStateTasks
            .GroupBy(x => Directory.GetDirectoryRoot(x.SourceItem.Target))
            .Select(x => new SequentialBuTask(Log, new TaskEvents(), string.Empty, x.ToList()))
            .ToList();
        var sourceItemGroupTasksExecuter = new ParallelExecuter(sourceItemGroupTasks, 10);

        sourceItemGroupTasksExecuter.Wait();
        storageTasksExecuter.Wait();

        var deleteUnversionedFilesStorageTasksExecuter = new ParallelExecuter([DeleteUnversionedFilesStorageTask], 1);
        deleteUnversionedFilesStorageTasksExecuter.Wait();

        IsSuccess = Children.All(x => x.IsSuccess);
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }
}
