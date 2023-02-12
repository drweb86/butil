using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System.IO;

namespace BUtil.Core.TasksTree.States
{
    internal class GetStateOfFileTask : BuTask
    {
        private readonly CommonServicesIoc _servicesIoc;
        private readonly string _fileName;
        public FileState State { get; private set; }

        public GetStateOfFileTask(ILog log, BackupEvents events, CommonServicesIoc servicesIoc, string fileName) :
            base(log, events, string.Format(BUtil.Core.Localization.Resources.GetStateOfFileFileName, fileName), TaskArea.File)
        {
            _servicesIoc = servicesIoc;
            _fileName = fileName;
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);
            var fileInfo = new FileInfo(_fileName);

            State = new FileState(_fileName, fileInfo.LastWriteTimeUtc, fileInfo.Length, _servicesIoc.HashService.GetSha512(_fileName, true));

            IsSuccess = true;
            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
        }
    }
}
