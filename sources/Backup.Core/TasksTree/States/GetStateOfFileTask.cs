using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace BUtil.Core.TasksTree.States
{
    internal class GetStateOfFileTask : BuTask
    {
        private string _fileName;
        public FileState State { get; private set; }

        public GetStateOfFileTask(LogBase log, BackupEvents events, string fileName) :
            base(log, events, string.Format(BUtil.Core.Localization.Resources.GetStateOfFileFileName, fileName), TaskArea.File)
        {
            _fileName = fileName;
        }

        public override void Execute(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            UpdateStatus(ProcessingStatus.InProgress);
            var fileInfo = new FileInfo(_fileName);

            using var fileStream = File.OpenRead(_fileName);
            using var sha256Hash = SHA256.Create();

            var hash = sha256Hash.ComputeHash(fileStream);

            State = new FileState(_fileName, fileInfo.LastWriteTimeUtc, fileInfo.Length, HashToString(hash));

            UpdateStatus(ProcessingStatus.FinishedSuccesfully);
        }

        private string HashToString(byte[] hash)
        {
            var sBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
