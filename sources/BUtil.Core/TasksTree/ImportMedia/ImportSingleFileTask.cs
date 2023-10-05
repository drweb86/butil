using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.States;
using System;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.MediaSyncBackupModel
{
    class ImportSingleFileTask : BuTask
    {
        private readonly string _transformFileName;
        private readonly IStorage _fromStorage;
        private readonly IStorage _toStorage;
        private readonly SourceItemState _state;
        private readonly CommonServicesIoc _commonServicesIoc;

        public string File { get; private set; }

        public ImportSingleFileTask(
            ILog log,
            TaskEvents backupEvents,
            string fromFile,
            IStorage fromStorage,
            IStorage toStorage,
            string transformFileName,
            SourceItemState state,
            CommonServicesIoc commonServicesIoc)
            : base(log, backupEvents, null)
        {
            File = fromFile;
            _state = state;
            _commonServicesIoc = commonServicesIoc;
            _fromStorage = fromStorage;
            _toStorage = toStorage;
            _transformFileName = transformFileName;

            Title = string.Format(BUtil.Core.Localization.Resources.ImportMediaTask_File, Path.GetFileNameWithoutExtension(fromFile));
        }

        public override void Execute()
        {
            UpdateStatus(ProcessingStatus.InProgress);
            try
            {
                var lastWriteTime = _fromStorage.GetModifiedTime(this.File);
                var destinationFileName = GetDestinationFileName(lastWriteTime);
                var actualFileName = GetActualDestinationFileName(destinationFileName);
                var destFolder = Path.GetDirectoryName(actualFileName);

                using (var temp = new TempFolder())
                {
                    var exchangeFile = Path.Combine(temp.Folder, Path.GetFileName(this.File));
                    _fromStorage.Download(this.File, exchangeFile);
                    System.IO.File.SetCreationTime(exchangeFile, lastWriteTime);
                    System.IO.File.SetLastWriteTime(exchangeFile, lastWriteTime);

                    var getState = new GetStateOfFileTask(Log, new TaskEvents(), _commonServicesIoc, _state.SourceItem,  exchangeFile);
                    getState.Execute();
                    if (!getState.IsSuccess)
                    {
                        IsSuccess = false;
                        UpdateStatus(ProcessingStatus.FinishedWithErrors);
                        return;
                    }
                    if (_state.FileStates.Any(x => x.CompareTo(getState.State, true)))
                    {
                        Log.WriteLine(LoggingEvent.Debug, $"File {File} is already exists in destination folder. Skipping.");
                        IsSuccess = true;
                        UpdateStatus(ProcessingStatus.FinishedSuccesfully);
                        return;
                    }

                    var uploadedFileName = _toStorage.Upload(exchangeFile, actualFileName);
                    System.IO.File.SetCreationTime(uploadedFileName.StorageFileName, lastWriteTime);
                    System.IO.File.SetLastWriteTime(uploadedFileName.StorageFileName, lastWriteTime);
                }

                IsSuccess = true;
                UpdateStatus(ProcessingStatus.FinishedSuccesfully);
            }
            catch (Exception ex)
            {
                IsSuccess = false;

                this.LogError(ex.Message);
                UpdateStatus(ProcessingStatus.FinishedWithErrors);
            }
        }

        private string GetActualDestinationFileName(string destFile)
        {
            var actualDestFile = destFile;
            var id = 0;
            while (_toStorage.Exists(actualDestFile))
            {
                id++;
                actualDestFile = Path.Combine(Path.GetDirectoryName(destFile), $"{Path.GetFileNameWithoutExtension(destFile)}_{id}{Path.GetExtension(destFile)}");
            }

            return actualDestFile;
        }

        private string GetDestinationFileName(DateTime lastWriteTime)
        {
            var relativePath = DateTokenReplacer.ParseString(_transformFileName, lastWriteTime);
            var relativeDir = Path.GetDirectoryName(relativePath);
            foreach (var ch in Path.GetInvalidPathChars())
            {
                relativeDir = relativeDir.Replace(ch, '_');
            }

            var relativeFileName = Path.GetFileName(relativePath);
            foreach (var ch in Path.GetInvalidFileNameChars())
            {
                relativeFileName = relativeFileName.Replace(ch, '_');
            }

            var destFile = Path.Combine(relativeDir, $"{relativeFileName}{Path.GetExtension(this.File)}");

            return destFile;
        }
    }
}
