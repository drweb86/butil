using BUtil.Core.ConfigurationFileModels.V2;
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

        public GetStateOfFileTask(ILog log, TaskEvents events, CommonServicesIoc servicesIoc, SourceItemV2 item, string fileName) :
            base(log, events, null, TaskArea.File)
        {
            _servicesIoc = servicesIoc;
            _fileName = fileName;
            Title = string.Format(BUtil.Core.Localization.Resources.GetStateOfFileFileName, item.Target == fileName ? fileName: fileName.Substring(item.Target.Length + 1));
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
