using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using System.IO;

namespace BUtil.Core.TasksTree.IncrementalModel
{
    class MoveFileTask : BuTask
    {
        private readonly string _from;
        private readonly string _toFolder;
        private readonly string _transformFileName;

        public MoveFileTask(ILog log, BackupEvents backupEvents, string from, string toFolder, string transformFileName)
            : base(log, backupEvents, string.Format("Move {0} to {1}", from, toFolder), TaskArea.File)
        {
            _from = from;
            _toFolder = toFolder;
            _transformFileName = transformFileName;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);

            // TODO:
            //var destFile = Path.Combine(_toFolder, _transformFileName.Replace(""))
            //File.Move(_from, _toFolder);
            
            UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        }
    }
}
