using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.TasksTree.Core;
using System;
using System.Diagnostics;
using System.Threading;

namespace BUtil.Core.TasksTree.Apps
{
    internal class ExecuteProgramTask : BuTask
    {
        private readonly ExecuteProgramTaskInfo _executeProgramTaskInfo;
        private readonly ProcessPriorityClass _processPriority;

        public ExecuteProgramTask(ILog log, BackupEvents events, ExecuteProgramTaskInfo executeProgramTaskInfo, ProcessPriorityClass processPriority)
            : base(log, events, executeProgramTaskInfo.Name, TaskArea.ProgramInRunBeforeAfterBackupChain)
        {
            _executeProgramTaskInfo = executeProgramTaskInfo;
            _processPriority = processPriority;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            LogDebug($"Program: {_executeProgramTaskInfo.Program} at working directort {_executeProgramTaskInfo.WorkingDirectory} with {_processPriority} priority");
            UpdateStatus(ProcessingStatus.InProgress);
            try
            {
                ProcessHelper.Execute(_executeProgramTaskInfo.Program, _executeProgramTaskInfo.Arguments, _executeProgramTaskInfo.WorkingDirectory,
                    _processPriority, token,
                    out var stdOutput, out var error, out var returnCode);

                LogDebug($"Exit code: {returnCode}");
                LogDebug(stdOutput);
                LogError(error);
                UpdateStatus(returnCode == 0 ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
                IsSuccess = returnCode == 0;
            }
            catch (Exception e)
            {
                LogError(e.ToString());
                UpdateStatus(ProcessingStatus.FinishedWithErrors);
            }

        }
    }
}
