using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.States;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class GetStateOfSourceItemTask : SequentialBuTask
    {
        private List<GetStateOfFileTask> _getFileStateTasks;
        public SourceItemState SourceItemState { get; private set; } 

        public SourceItem SourceItem { get; }

        public GetStateOfSourceItemTask(LogBase log, BackupEvents events, SourceItem sourceItem) : 
            base(log, events, string.Format(BUtil.Core.Localization.Resources.GetStateOfSourceItem, sourceItem.Target), sourceItem.IsFolder ? TaskArea.Folder : TaskArea.File, null)
        {
            SourceItem = sourceItem;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            Events.TaskProgessUpdate(Id, ProcessingStatus.InProgress);
            LogDebug("Reading content");
            var files = new List<string>();
            if (SourceItem.IsFolder)
            {
                files.AddRange(Directory.GetFiles(SourceItem.Target, "*.*", SearchOption.AllDirectories));
            }
            else
            {
                files.Add(SourceItem.Target);
            }

            _getFileStateTasks = files
                .Select(file => new GetStateOfFileTask(Log, Events, file))
                .ToList();
            Children = _getFileStateTasks;
            Events.DuringExecutionTasksAdded(Id, Children);

            base.Execute(token);

            SourceItemState = new SourceItemState(SourceItem,
                _getFileStateTasks
                .Select(x => x.State)
                .ToList());
        }
    }
}
