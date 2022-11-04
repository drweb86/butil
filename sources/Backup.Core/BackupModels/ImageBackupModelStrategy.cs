using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Globalization;
using System.Collections.ObjectModel;
using BUtil.Core.Synchronization;
using BUtil.Core.ButilImage;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Core.Options;
using BUtil.Core.Misc;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using System.Linq;
using BUtil.Core.Events;
using BUtil.Core.Jobs;
using System.Threading.Tasks;
using BUtil.Core.TasksTree;

namespace BUtil.Core.BackupModels
{
    class IncrementalBackupModelStrategy : IBackupModelStrategy
    {
        #region locals

        string _backupAbortedByUser = "Aborted by user!";
        string _couldNotDeleteFileFormatString = "During deleting of a file '{0}' an error occured: {1}";
        string _noStoragesSpecified = "There is no storages specified in configuration. Compression of data was skipped";
        string _noDataToBackupSpecified = "There is no data to backup specified in configuration";

        #endregion

        #region Fields

        readonly ProgramOptions _options;
        private readonly IncrementalBackupModelOptions _modelOptions;
        readonly LogBase _log;
        Thread _job;
        readonly TaskManager _copyManager;
        readonly TaskManager _compressionManager;
        readonly TaskManager _imageCreationManager;
        bool _aborting;
        bool _runExecuted;
        readonly BackupTask _task;

        // keeps temporary file names
        readonly SyncedFiles _temporaryFiles = new();
        readonly SyncFile _imageFile = new();

        #endregion

        #region Public Properties

        public LogBase Log => _log;

        #endregion

        #region Constructors

        public IncrementalBackupModelStrategy(LogBase openedLog, BackupTask task, IncrementalBackupModelOptions modelOptions, ProgramOptions options)
        {
            if (!openedLog.IsOpened)
                throw new InvalidDataException("Log is not opened!");// do not localize!
            _modelOptions = modelOptions;
            _log = openedLog;
            _task = task;
            _options = options;
            _copyManager = new TaskManager(_options.AmountOfStoragesToProcessSynchronously);
            _compressionManager = new TaskManager(_options.AmountOf7ZipProcessesToProcessSynchronously);
            _imageCreationManager = new TaskManager(1);
            HtmlLogFormatter.IncludeLoggingEventPrefixes = options.LoggingLevel == LogLevel.Support;
            ApplyTranslation();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Shows that during backup some unexpected things occured.
        /// </summary>
        public bool ErrorsOrWarningsRegistered
        {
            get { return _log.ErrorsOrWarningsRegistered; }
        }

        #endregion

        #region Public methods

        public BuTask GetTask(BackupEvents events)
        {
            var task = new IncrementalBackupTask(_log, events, _task, _options);
            return task;
        }

        /// <summary>
        /// Non-reenterable
        /// </summary>
        public void Run()
        {
            if (_runExecuted)
            {
                throw new InvalidOperationException("Operation is not reenterable");
            }
            _runExecuted = true;

            SetPriority();

            BackupLiveCycle();
        }

        /// <summary>
        /// Stops backup process immidietly. This is not secure variant and works fine for storages only. For stopping compression it needs to be reworked
        /// </summary>
        public void StopForcibly()
        {
            _log.ProcedureCall("StopForcibly::start");

            _aborting = true;
            _log.WriteLine(LoggingEvent.Error, _backupAbortedByUser);
            _compressionManager.Abort();
            _copyManager.Abort();
            _imageCreationManager.Abort();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException">Do not handle!</exception>
        void BackupLiveCycle()
        {
            _log.ProcedureCall("BackupLiveCycle");

            // initialization of job thread instance
            Thread.CurrentThread.Priority = _options.Priority;

            _job = new Thread(BackupThreadJob)
            {
                IsBackground = true
            };
            _job.Start();

            while (_job.IsAlive)
                Thread.Sleep(1000);//?
        }


        /// <summary>
        /// Main function that performs in backup
        /// </summary>
        void BackupThreadJob()
        {
            _log.ProcedureCall("BackupThreadJob");

            _log.WriteLine(LoggingEvent.Debug, "Processing backup task {0}", _task.Name);

            if (BeforeBackup(out var metarecords, out var archiveParameters))
            {
                if (!_aborting) Start(metarecords, archiveParameters);
                if (!_aborting) AfterBackup();
            }

            _log.Close();
        }

        /// <summary>
        /// Called in the very beginning of backup lifetime cycle
        /// </summary>
        bool BeforeBackup(out Collection<MetaRecord> metarecords, out SourceItemCompressionJobOptions[] archiveParameters)
        {
            _log.ProcedureCall("BeforeBackUp");
            _log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InvariantCulture, "Temp folder: {0}", Directories.TempFolder));
            _log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InvariantCulture, "Task: {0}", _task.Name));

            if (_log is FileLog && _options.LoggingLevel == LogLevel.Support)
            {
                string[] options = File.ReadAllLines(Files.ProfileFile);

                foreach (string line in options)
                {
                    _log.WriteLine(LoggingEvent.Debug, line.Replace("<", "(").Replace(">", ")"));
                }
            }

            //foreach (var taskInfo in _task.ExecuteBeforeBackup)
            //    Events.ExecuteProgramStatusUpdate(taskInfo, ProcessingStatus.NotStarted);

            //foreach (var taskInfo in _task.ExecuteAfterBackup)
            //    Events.ExecuteProgramStatusUpdate(taskInfo, ProcessingStatus.NotStarted);

            //archiveParameters = CreateArgsForCompressionAndMetaForImage(out metarecords);
            //foreach (var archiveParameter in archiveParameters)
            //    Events.SourceItemStatusUpdate(archiveParameter.ItemToCompress, ProcessingStatus.NotStarted);

            //foreach (var storageSettings in _task.Storages)
            //    Events.StorageStatusUpdate(storageSettings, ProcessingStatus.NotStarted);

            metarecords = null;
            archiveParameters = null;
            return true; // IsAnySenceInPacking();
        }


        /// <summary>
        /// This procedure is called in the end of a backup life time cycle
        /// </summary>
        void AfterBackup()
        {
            _log.ProcedureCall("AfterBackUp");
            _log.WriteLine(LoggingEvent.Debug, "Deleting all temporary files...");

            RemoveAllFilesAtFolder(_temporaryFiles);
            RemoveAllFilesAtFolder(_imageFile);
        }

        SourceItemCompressionJobOptions[] CreateArgsForCompressionAndMetaForImage(out Collection<MetaRecord> metarecords)
        {
            metarecords = new Collection<MetaRecord>();
            var parametersSet = new SourceItemCompressionJobOptions[_task.Items.Count];

            int syncIndex = 0;
            int itemIndex = 0;
            foreach (SourceItem item in _task.Items)
            {
                // Compression item
                string tempFileName;
                do
                {
                    tempFileName = Directories.TempFolder + @"\butil_tmp_" + syncIndex.ToString(CultureInfo.InvariantCulture) + ".7z";
                    syncIndex += 1;
                }
                while (!_temporaryFiles.TryRegisterNewName(tempFileName));

                parametersSet[itemIndex] = new SourceItemCompressionJobOptions(_options.ProcessPriority, tempFileName, item, _task.Password);

                // Metainformation
                var record = new MetaRecord(item.IsFolder, item.Target)
                {
                    LinkedFile = tempFileName
                };
                metarecords.Add(record);
                itemIndex++;
            }

            return parametersSet;
        }

        bool IsAnySenceInPacking()
        {
            _log.ProcedureCall("isAnySenceInPacking");

            // nowhere to backup
            if (_task.Storages.Count < 1)
            {
                _log.WriteLine(LoggingEvent.Warning, _noStoragesSpecified);
                return false;
            }

            // nothing to backup
            if (!_task.Items.Any())
            {
                _log.WriteLine(LoggingEvent.Warning, _noDataToBackupSpecified);
                return false;
            }

            return true;
        }

        void Start(Collection<MetaRecord> metarecords, IEnumerable<SourceItemCompressionJobOptions> archiveParameters)
        {
            _log.ProcedureCall("Start");

            foreach (var archiveParameter in archiveParameters)
            {
                var job = new SourceItemCompressionJob(_log, archiveParameter, null);
                _compressionManager.AddJob(job);
            }
            _compressionManager.WaitUntilAllJobsAreDone();

            if (_aborting)
            {
                _log.ProcedureCall("Start::archiving aborted");
                return;
            }

            var imageCreationJob = new CreateButilImageJob(_log, null, "lala", _imageFile, metarecords);
            _imageCreationManager.AddJob(imageCreationJob);
            _imageCreationManager.WaitUntilAllJobsAreDone();

            if (_aborting)
            {
                _log.ProcedureCall("Start::Image creation aborted");
                return;
            }

            foreach (StorageSettings storageSettings in _task.Storages)
            {
                var job = new CopyFileToStorageJob(_log, null, storageSettings, _imageFile.FileName);
                _copyManager.AddJob(job);
            }
            _copyManager.WaitUntilAllJobsAreDone();

            if (_aborting)
            {
                _log.ProcedureCall("Start::copying storage aborted");
                return;
            }

            if (_aborting)
            {
                _log.ProcedureCall("Start::run programs chain after backup aborted");
                return;
            }

            File.Delete(_imageFile.FileName);
        }

        void RemoveAllFilesAtFolder(SyncedFiles files)
        {
            _log.ProcedureCall("RemoveAllFilesAtFolder");

            foreach (SyncFile file in files.SynchronizedFiles)
            {
                RemoveAllFilesAtFolder(file);
            }
        }

        /// <summary>
        /// Removes file
        /// </summary>
        /// <param name="file">sync object</param>
        /// <param name="secureDeleting">if true - does ovewrites file with zeros</param>
        void RemoveAllFilesAtFolder(SyncFile file)
        {
            _log.ProcedureCall("RemoveAllFilesAtFolder");

            file.Dispose();

            _log.WriteLine(LoggingEvent.Debug, "X " + file.FileName);

            if (!string.IsNullOrEmpty(file.FileName))
            {
                try
                {
                    File.Delete(file.FileName);
                }
                catch (DirectoryNotFoundException e)
                {
                    _log.WriteLine(LoggingEvent.Warning,
                                   string.Format(CultureInfo.CurrentCulture, _couldNotDeleteFileFormatString, file.FileName, e.Message));
                }
                catch (PathTooLongException e)
                {
                    _log.WriteLine(LoggingEvent.Warning,
                                   string.Format(CultureInfo.CurrentCulture, _couldNotDeleteFileFormatString, file.FileName, e.Message));
                }
                catch (FileNotFoundException e)
                {
                    _log.WriteLine(LoggingEvent.Warning,
                                   string.Format(CultureInfo.CurrentCulture, _couldNotDeleteFileFormatString, file.FileName, e.Message));
                }
                catch (UnauthorizedAccessException e)
                {
                    _log.WriteLine(LoggingEvent.Warning,
                                   string.Format(CultureInfo.CurrentCulture, _couldNotDeleteFileFormatString, file.FileName, e.Message));
                }
                catch (IOException e)
                {
                    _log.WriteLine(LoggingEvent.Warning,
                                   string.Format(CultureInfo.CurrentCulture, _couldNotDeleteFileFormatString, file.FileName, e.Message));
                }
            }
            else
                _log.WriteLine(LoggingEvent.Debug, "empty file");
        }

        /// <summary>
        /// Sets priority
        /// </summary>
        /// <exception cref="InvalidDataException">Invalid options. This exception should not be handled because them are opened outside</exception>
        void SetPriority()
        {
            _log.WriteLine(LoggingEvent.Debug, "VERSION = " + CopyrightInfo.Version);
            _log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InvariantCulture, "Operating System details: {0} {1} {2}", Environment.OSVersion.Platform, Environment.OSVersion.ServicePack, Environment.OSVersion.Version.ToString(3)));
            Thread.CurrentThread.Priority = _options.Priority;
            _log.WriteLine(LoggingEvent.Debug, "setted priorities to self " + _options.Priority);
        }

        void ApplyTranslation()
        {
            _backupAbortedByUser = Resources.AbortedByUser;
            //mProcStrOK = Resources.FileWasCopyiedToStorageSuccessfully;
            _couldNotDeleteFileFormatString = Resources.DuringDeletingOfAFile0AnErrorOccured1;
            _noStoragesSpecified = Resources.ThereIsNoStoragesSpecifiedInConfigurationCompressionOfDataWasSkipped;
            _noDataToBackupSpecified = Resources.ThereIsNoDataToBackupSpecifiedInConfiguration;
        }

        #endregion

        #region Dispose Implementation

        private bool _isDisposed;

        public void Dispose()
        {
            DoDispose(false);
            GC.SuppressFinalize(this);
        }

        void DoDispose(bool final)
        {
            if (final)
                if (_temporaryFiles == null) return;

            if (!_isDisposed)
            {
                _temporaryFiles.Dispose();
                _imageFile.Dispose();
                _isDisposed = true;

                if (_log.IsOpened)
                {
                    _log.Close();
                }
            }
        }

        ~IncrementalBackupModelStrategy()
        {
            DoDispose(true);
        }

        #endregion

    }
}
