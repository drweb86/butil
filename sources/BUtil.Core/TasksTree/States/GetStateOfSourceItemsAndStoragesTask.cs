﻿using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree.States
{
    internal class GetStateOfSourceItemsAndStoragesTask : ParallelBuTask
    {
        public IEnumerable<GetStateOfSourceItemTask> GetSourceItemStateTasks { get; }
        public IEnumerable<GetStateOfStorageTask> StorageStateTasks { get; }

        public GetStateOfSourceItemsAndStoragesTask(
            ILog log,
            BackupEvents events, 
            IEnumerable<SourceItem> sourceItems,
            IEnumerable<IStorageSettings> storageSettings,
            IEnumerable<string> fileExcludePatterns,
            string password)
            : base(log, events, Localization.Resources.GetStateOfSourceItemsAndStorages, TaskArea.File, null)
        {
            var childTasks = new List<BuTask>();
            var setSourceItemStateTasks = new List<GetStateOfSourceItemTask>();

            foreach (var item in sourceItems)
            {
                var getSourceItemStateTask = new GetStateOfSourceItemTask(log, Events, item, fileExcludePatterns);
                setSourceItemStateTasks.Add(getSourceItemStateTask);
                childTasks.Add(getSourceItemStateTask);
            }
            GetSourceItemStateTasks = setSourceItemStateTasks;

            var storageStateTasks = new List<GetStateOfStorageTask>();
            foreach (var storage in storageSettings)
            {
                if (!storage.Enabled)
                    continue;

                var getStorageStateTask = new GetStateOfStorageTask(Log, Events, storage, password);
                storageStateTasks.Add(getStorageStateTask);
                childTasks.Add(getStorageStateTask);
            }

            StorageStateTasks = storageStateTasks;

            Children = childTasks;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);

            var storageTasksExecuter = new ParallelExecuter(StorageStateTasks, 10);
            storageTasksExecuter.Wait();

            var sourceItemGroupTasks = GetSourceItemStateTasks
                .GroupBy(x => Directory.GetDirectoryRoot(x.SourceItem.Target))
                .Select(x => new SequentialBuTask(new StubLog(), new BackupEvents(), string.Empty, TaskArea.File, x.ToList()))
                .ToList();
            var sourceItemGroupTasksExecuter = new ParallelExecuter(sourceItemGroupTasks, 10);

            sourceItemGroupTasksExecuter.Wait();
            storageTasksExecuter.Wait();

            IsSuccess = Children.All(x => x.IsSuccess);
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
