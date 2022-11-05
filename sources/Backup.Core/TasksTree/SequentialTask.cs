﻿using BUtil.Core.Events;
using BUtil.Core.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BUtil.Core.TasksTree
{

    public abstract class SequentialBuTask : BuTask
    {
        public IEnumerable<BuTask> Children { get; set; }

        protected SequentialBuTask(LogBase log, BackupEvents events, string title, TaskArea taskArea, IEnumerable<BuTask> children)
            :base(log, events, title, taskArea)
        {
            Children = children;
        }

        public override void Execute(CancellationToken token)
        {
            Events.TaskProgessUpdate(Id, ProcessingStatus.InProgress);
            foreach (var child in Children)
            {
                if (token.IsCancellationRequested)
                    break;

                child.Execute(token);
            }
            Events.TaskProgessUpdate(Id, ProcessingStatus.FinishedSuccesfully);
        }

        public override IEnumerable<BuTask> GetChildren()
        {
            var children = (Children ?? Array.Empty<BuTask>()).ToList();
            foreach (var child in Children)
            {
                children.AddRange(child.GetChildren());
            }
            return children;
        }
    }
}
