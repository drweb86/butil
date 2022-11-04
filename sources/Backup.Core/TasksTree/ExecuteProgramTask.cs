using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BUtil.Core.TasksTree
{
    internal class ExecuteProgramTask : BuTask
    {
        private readonly ExecuteProgramTaskInfo _executeProgramTaskInfo;
        private readonly ProcessPriorityClass _processPriority;

        public ExecuteProgramTask(LogBase log, BackupEvents events, ExecuteProgramTaskInfo executeProgramTaskInfo, ProcessPriorityClass processPriority) 
            :base(log, events, executeProgramTaskInfo.Name, TaskArea.ProgramInRunBeforeAfterBackupChain)
        {
            _executeProgramTaskInfo = executeProgramTaskInfo;
            _processPriority = processPriority;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            var workDir = Path.GetDirectoryName(_executeProgramTaskInfo.Program);
            Log.WriteLine(Logs.LoggingEvent.Debug, $"{Id} {Title}: program: {_executeProgramTaskInfo.Program} with arguments {_executeProgramTaskInfo.Arguments} at {workDir} with {_processPriority} priority");
            Events.TaskProgessUpdate(Id, Core.Events.ProcessingStatus.InProgress);
            try
            {
                ProcessHelper.Execute(_executeProgramTaskInfo.Program, _executeProgramTaskInfo.Arguments, workDir,
                    _processPriority, token,
                    out var stdOutput, out var error, out var returnCode);

                Log.WriteLine(Logs.LoggingEvent.Debug, $"{Id} {Title}: return code: {returnCode}");
                Log.WriteLine(Logs.LoggingEvent.Debug, $"{Id} {Title}: output: {stdOutput}");
                Log.WriteLine(Logs.LoggingEvent.Debug, $"{Id} {Title}: error: {error}");
                Events.TaskProgessUpdate(Id, returnCode == 0 ? Core.Events.ProcessingStatus.FinishedSuccesfully : Core.Events.ProcessingStatus.FinishedWithErrors);
            }
            catch (Exception e)
            {
                Log.WriteLine(Logs.LoggingEvent.Error, $"{Id} {Title}: failed: {e}");
                Events.TaskProgessUpdate(Id, Core.Events.ProcessingStatus.FinishedWithErrors);
            }
            
        }
    }
}
