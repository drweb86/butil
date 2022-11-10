using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.TasksTree.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BUtil.Core.TasksTree.Apps
{
    internal class ExecuteProgramTask : BuTask
    {
        private readonly ExecuteProgramTaskInfo _executeProgramTaskInfo;
        private readonly ProcessPriorityClass _processPriority;

        public ExecuteProgramTask(LogBase log, BackupEvents events, ExecuteProgramTaskInfo executeProgramTaskInfo, ProcessPriorityClass processPriority)
            : base(log, events, executeProgramTaskInfo.Name, TaskArea.ProgramInRunBeforeAfterBackupChain)
        {
            _executeProgramTaskInfo = executeProgramTaskInfo;
            _processPriority = processPriority;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            Log.WriteLine(LoggingEvent.Debug, $"{Id} {Title}: program: {_executeProgramTaskInfo.Program} at working directort {_executeProgramTaskInfo.WorkingDirectory} with arguments {_executeProgramTaskInfo.Arguments} with {_processPriority} priority");
            Events.TaskProgessUpdate(Id, ProcessingStatus.InProgress);
            try
            {
                ProcessHelper.Execute(_executeProgramTaskInfo.Program, _executeProgramTaskInfo.Arguments, _executeProgramTaskInfo.WorkingDirectory,
                    _processPriority, token,
                    out var stdOutput, out var error, out var returnCode);

                Log.WriteLine(LoggingEvent.Debug, $"{Id} {Title}: return code: {returnCode}");
                Log.WriteLine(LoggingEvent.Debug, $"{Id} {Title}: output: {stdOutput}");
                Log.WriteLine(LoggingEvent.Debug, $"{Id} {Title}: error: {error}");
                Events.TaskProgessUpdate(Id, returnCode == 0 ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
            }
            catch (Exception e)
            {
                Log.WriteLine(LoggingEvent.Error, $"{Id} {Title}: failed: {e}");
                Events.TaskProgessUpdate(Id, ProcessingStatus.FinishedWithErrors);
            }

        }
    }
}
