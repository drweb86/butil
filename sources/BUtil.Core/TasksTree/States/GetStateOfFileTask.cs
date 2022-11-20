using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using System.IO;
using System.Threading;

namespace BUtil.Core.TasksTree.States
{
    internal class GetStateOfFileTask : BuTask
    {
        private string _fileName;
        public FileState State { get; private set; }

        public GetStateOfFileTask(ILog log, BackupEvents events, string fileName) :
            base(log, events, string.Format(BUtil.Core.Localization.Resources.GetStateOfFileFileName, fileName), TaskArea.File)
        {
            _fileName = fileName;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);
            var fileInfo = new FileInfo(_fileName);

            State = new FileState(_fileName, fileInfo.LastWriteTimeUtc, fileInfo.Length, HashHelper.GetSha512(_fileName));

            IsSuccess = true;
            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
        }
    }
}
