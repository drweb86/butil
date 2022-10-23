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

namespace BUtil.Core
{
	public delegate void BackupFinished();
	public delegate void CriticalErrorOccur(string message);

	public sealed class BackupProcess: IDisposable
	{
		#region locals
		string _backupAbortedByUser = "Aborted by user!";
		string _couldNotVanishFileFormatString = "During overwriting file '{0}' with zeros an error occured: {1}";
		string _couldNotDeleteFileFormatString = "During deleting of a file '{0}' an error occured: {1}";
		string _noStoragesSpecified = "There is no storages specified in configuration. Compression of data was skipped";
		string _noDataToBackupSpecified = "There is no data to backup specified in configuration";
		
		#endregion

		#region Fields

		BackupFinished _onBackupFinished;
		CriticalErrorOccur _onCriticalErrorOccured;
		readonly ProgramOptions _options;
		readonly LogBase _log;
		Thread _job;
		readonly TaskManager _copyManager;
		readonly TaskManager _beforeBackupEventManager;
		readonly TaskManager _afterBackupEventManager;
		readonly TaskManager _compressionManager;
		readonly TaskManager _imageCreationManager;
		bool _aborting;
	    bool _runExecuted;
		BackupTask _task;
	    readonly List<BackupTask> _tasks;

		// keeps temporary file names
		readonly SyncedFiles _temporaryFiles = new SyncedFiles();
		readonly SyncFile _imageFile = new SyncFile();

		#endregion

        #region Constructors

        public BackupProcess(List<BackupTask> tasks, ProgramOptions options, LogBase openedLog)
        {
            if (!openedLog.IsOpened)
                throw new InvalidDataException("Log is not opened!");// do not localize!

            if (tasks == null || tasks.Count == 0)
            {
                throw new InvalidOperationException("tasks cannot be null or empty");
            }

            _log = openedLog;
            _tasks = tasks;
            _options = options;
            _beforeBackupEventManager = new TaskManager(1);
            _afterBackupEventManager = new TaskManager(1);
            _copyManager = new TaskManager(_options.AmountOfStoragesToProcessSynchronously);
            _compressionManager = new TaskManager(_options.AmountOf7ZipProcessesToProcessSynchronously);
            _imageCreationManager = new TaskManager(1);
            HtmlLogFormatter.IncludeLoggingEventPrefixes = (options.LoggingLevel == LogLevel.Support);
            ApplyTranslation();
        }

        #endregion

		#region Properties

		/// <summary>
		/// Notification event handler for processing storages and compression items
		/// </summary>
		public EventHandler NotificationEventHandler = null;
		
		/// <summary>
		/// Notification event handler that current backup was finished. Occurs in normal way
		/// </summary>
		public BackupFinished BackupFinished
		{
			get { return _onBackupFinished; }
			set { _onBackupFinished = value; }
		}

		/// <summary>
		/// Notification event handler that something critical happened and program cannot continue
		/// </summary>
		public CriticalErrorOccur CriticalErrorOccur
		{
			get { return _onCriticalErrorOccured; }
			set { _onCriticalErrorOccured = value; }
		}
		
		/// <summary>
		/// Shows that during backup some unexpected things occured.
		/// </summary>
		public bool ErrorsOrWarningsRegistered
		{
			get { return _log.ErrorsOrWarningsRegistered; }
		}

		#endregion

		#region Public methods
	
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
			// cleaning tasls about copying
			_afterBackupEventManager.Abort();
			_beforeBackupEventManager.Abort();
			_compressionManager.Abort();
			_copyManager.Abort();
			_imageCreationManager.Abort();

			// Alarming that all finished
			if (_onBackupFinished != null)
				_onBackupFinished.Invoke();
		}
		
		/// <summary>
		/// Overwrites the file with zeroes to make it unrecoverable. Does not use the log
		/// </summary>
		/// <param name="fileName">the file to vanish</param>
		public static void OverwriteFileWithZerosWithoutLogging(string fileName)
		{
			using (FileStream file = File.OpenWrite(fileName))
			{
				long size = file.Length;
				for (long i = 0; i <= size; i++)
				{
					file.WriteByte(0);
				}
			}
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

			_job = new Thread(BackupThreadJob);
			_job.IsBackground = true;
			_job.Start();
			
			while (_job.IsAlive)
				Thread.Sleep(1000);//?
		}
		
		
		/// <summary>
		/// Main function that performs in backup
		/// </summary>
		void BackupThreadJob ()
		{
			_log.ProcedureCall("BackupThreadJob");

		    foreach (var task in _tasks)
		    {
		        _task = task;
                _log.WriteLine(LoggingEvent.Debug, "Processing backup task {0}", task.Name);
                Collection<MetaRecord> metarecords;
                ArchiveTask[] archiveParameters;

                if (BeforeBackup(out metarecords, out archiveParameters))
                {
                    if (!_aborting) RunProgramsChainBeforeBackup();
                    if (!_aborting) Start(metarecords, archiveParameters);
                    if (!_aborting) AfterBackup();
                }
		    }

			_log.Close();

			// Alarming that all finished
			if (_onBackupFinished != null)
				_onBackupFinished.Invoke();
		}
		
		/// <summary>
		/// Occurs before any processing
		/// </summary>
		void RunProgramsChainBeforeBackup()
		{
			_log.ProcedureCall("runProgramsChainBeforeBackup");

			foreach (BackupEventTaskInfo taskInfo in _task.BeforeBackupTasksChain)
			{
				var task = new BeforeBackupTask(taskInfo, _options.ProcessPriority, _log);
                if (NotificationEventHandler != null)
                {
                    task.NotificationEventHandler += NotificationEventHandler;
                }
				_beforeBackupEventManager.AddJob(task);
			}
			_beforeBackupEventManager.WaitUntilAllJobsAreDone();
		}
		
		/// <summary>
		/// Occurs after copying to storages right before the deletion of backup image file from temporary folder
		/// </summary>
		void RunProgramsChainAfterBackup()
		{
			_log.ProcedureCall("runProgramsChainAfterBackup");
			
			foreach (BackupEventTaskInfo taskInfo in _task.AfterBackupTasksChain)
			{
				var task = new AfterBackupTask(taskInfo, _imageFile.FileName, _options.ProcessPriority, _log);
                if (NotificationEventHandler != null)
                {
                    task.NotificationEventHandler += NotificationEventHandler;
                }
				_afterBackupEventManager.AddJob(task);
			}
			_afterBackupEventManager.WaitUntilAllJobsAreDone();
		}
		
		/// <summary>
		/// Called in the very beginning of backup lifetime cycle
		/// </summary>
		bool BeforeBackup(out Collection<MetaRecord> metarecords, out ArchiveTask[] archiveParameters)
		{
			_log.ProcedureCall("BeforeBackUp");
			_log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InvariantCulture, "Temp folder: {0}", Directories.TempFolder));
			_log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InvariantCulture, "Task: {0}",_task.Name));

			if ( (_log is FileLog) && (_options.LoggingLevel == LogLevel.Support))
			{
				string[] options = File.ReadAllLines(Files.ProfileFile);
				
				foreach (string line in options)
				{
					_log.WriteLine(LoggingEvent.Debug, line.Replace("<","(").Replace(">",")"));
				}
			}

			foreach (BackupEventTaskInfo taskInfo in _task.BeforeBackupTasksChain)
			{
				Notify(new RunProgramBeforeOrAfterBackupEventArgs(taskInfo, ProcessingState.NotStarted));
			}

			foreach (BackupEventTaskInfo taskInfo in _task.AfterBackupTasksChain)
			{
				Notify(new RunProgramBeforeOrAfterBackupEventArgs(taskInfo, ProcessingState.NotStarted));
			}
			
			archiveParameters = CreateArgsForCompressionAndMetaForImage(out metarecords);			
			foreach (ArchiveTask archiveParameter in archiveParameters)
				Notify(new PackingNotificationEventArgs(archiveParameter.ItemToCompress, ProcessingState.NotStarted));

			foreach (StorageBase storage in _task.Storages)
				Notify(new CopyingToStorageNotificationEventArgs(storage, ProcessingState.NotStarted));
			
			Notify(new ImagePackingNotificationEventArgs(ProcessingState.NotStarted));
			
			try
			{
				MD5Class.Verify7ZipBinaries();
			}
			catch (InvalidSignException signException)
			{
				_log.WriteLine(LoggingEvent.Error, string.Format(Resources.ButilSoftwarePackage7ZipComponent0HasInvalidCheckSummNprobablyItWasDamagedByVirusesNNyouShouldReinstallApplicationNNrestorationAndBackupFunctionsWillBeUnavailable, signException.Message));
				return false;
			}
			
			return IsAnySenceInPacking();
		}
		
		
		/// <summary>
		/// This procedure is called in the end of a backup life time cycle
		/// </summary>
		void AfterBackup()
		{
			_log.ProcedureCall("AfterBackUp");
			_log.WriteLine(LoggingEvent.Debug, "Deleting all temporary files...");
			
			RemoveAllFilesAtFolder(_temporaryFiles, _task.SecureDeletingOfFiles);
			RemoveAllFilesAtFolder(_imageFile, _task.SecureDeletingOfFiles);
		}
		
		ArchiveTask[] CreateArgsForCompressionAndMetaForImage(out Collection<MetaRecord> metarecords)
		{
			metarecords = new Collection<MetaRecord>();
			Collection<CompressionItem> compressionItems = _task.FilesFoldersList;
			var parametersSet = new ArchiveTask[_task.FilesFoldersList.Count];

		    int syncIndex = 0;
			int itemIndex = 0;
			foreach (CompressionItem item in compressionItems)
			{
				// Compression item
			    string tempFileName;
			    do
				{
					tempFileName = Directories.TempFolder + @"\butil_tmp_" + syncIndex.ToString(CultureInfo.InvariantCulture) + ".7z";
					syncIndex += 1;
				}
				while(!_temporaryFiles.TryRegisterNewName(tempFileName));

				parametersSet[itemIndex] = _task.EnableEncryption ? new ArchiveTask(_options.ProcessPriority, tempFileName, item, _task.SecretPassword) : new ArchiveTask(_options.ProcessPriority, tempFileName, item);
				
				// Metainformation
				var record = new MetaRecord(item.IsFolder, item.Target);
				record.LinkedFile = tempFileName;
				metarecords.Add( record );
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
			if (_task.FilesFoldersList.Count < 1)
			{
				_log.WriteLine(LoggingEvent.Warning, _noDataToBackupSpecified);
				return false;
			}
			
			return true;
		}
		
		void Start(Collection<MetaRecord> metarecords, IEnumerable<ArchiveTask> archiveParameters)
		{
			_log.ProcedureCall("Start");

			foreach (ArchiveTask archiveParameter in archiveParameters)
			{
				var job = new CompressionJob(archiveParameter, _task.EnableEncryption, _log);

                if (NotificationEventHandler != null)
                {
                    job.NotificationEventHandler += NotificationEventHandler;
                }
				_compressionManager.AddJob(job);
			}
			_compressionManager.WaitUntilAllJobsAreDone();
			
			if (_aborting)
			{
				_log.ProcedureCall("Start::archiving aborted");
				return;
			}
			
			var imageCreationJob = new ImageCreationJob(_log, _imageFile, metarecords);
            if (NotificationEventHandler != null)
            {
                imageCreationJob.NotificationEventHandler += NotificationEventHandler;
            }
			_imageCreationManager.AddJob(imageCreationJob);
			_imageCreationManager.WaitUntilAllJobsAreDone();

			if (_aborting)
			{
				_log.ProcedureCall("Start::Image creation aborted");
				return;
			}
			
			foreach (StorageBase storage in _task.Storages)
			{
				var job = new CopyJob(storage, _imageFile.FileName, _task.EnableEncryption, _log);
                if (NotificationEventHandler != null)
                {
                    job.NotificationEventHandler += NotificationEventHandler;
                }
				_copyManager.AddJob(job);
			}
			_copyManager.WaitUntilAllJobsAreDone();
			
			if (_aborting)
			{
				_log.ProcedureCall("Start::copying storage aborted");
				return;
			}
			
			RunProgramsChainAfterBackup();
			
			if (_aborting)
			{
				_log.ProcedureCall("Start::run programs chain after backup aborted");
				return;
			}
			
			// removing
			if (_task.SecureDeletingOfFiles)
			{
				OverWriteFileWithZeros(_imageFile.FileName);
			}
			else
			{
				File.Delete(_imageFile.FileName);
			}
		}

		void Notify(EventArgs e)
		{
			if (NotificationEventHandler != null)
			{
				NotificationEventHandler(this, e);
			}
		}		
		
		/// <summary>
		/// Overwrites file with zeros
		/// </summary>
		void OverWriteFileWithZeros(string fileName)
		{
			_log.ProcedureCall("OverWriteFileWithZeros", fileName);

			if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
			{
				try
				{
					using (var file = File.OpenWrite(fileName))
					{
					    _log.WriteLine(LoggingEvent.Debug,
						               string.Format(CultureInfo.CurrentCulture,
						                             "Overwriting file '{0}' with zeros", fileName));


						long size = file.Length;

						for (long i = 0; i < size; i++)
						{
							file.WriteByte(0);
						}

						file.Flush();
					}
				}
				catch (DirectoryNotFoundException e)
				{
					_log.WriteLine(LoggingEvent.Warning, _couldNotVanishFileFormatString, fileName, e.Message);
				}
				catch (PathTooLongException e)
				{
					_log.WriteLine(LoggingEvent.Warning, _couldNotVanishFileFormatString, fileName, e.Message);
				}
				catch (FileNotFoundException e)
				{
					_log.WriteLine(LoggingEvent.Warning, _couldNotVanishFileFormatString, fileName, e.Message);
				}
				catch (UnauthorizedAccessException e)
				{
					_log.WriteLine(LoggingEvent.Warning, _couldNotVanishFileFormatString, fileName, e.Message);
				}
				catch (IOException e)
				{
					_log.WriteLine(LoggingEvent.Warning,
					               string.Format(CultureInfo.CurrentCulture, _couldNotVanishFileFormatString, fileName, e.Message));
				}
			}
			else
				_log.WriteLine(LoggingEvent.Debug, "Empty file to vanish");
		}
		
		/// <summary>
		/// Removes all specified files
		/// </summary>
		void RemoveAllFilesAtFolder(SyncedFiles files, bool secureDeleting)
		{
			_log.ProcedureCall("RemoveAllFilesAtFolder", secureDeleting.ToString());
			
			foreach (SyncFile file in files.SynchronizedFiles)
			{
				RemoveAllFilesAtFolder(file, secureDeleting);
			}
		}
		
		/// <summary>
		/// Removes file
		/// </summary>
		/// <param name="file">sync object</param>
		/// <param name="secureDeleting">if true - does ovewrites file with zeros</param>
		void RemoveAllFilesAtFolder(SyncFile file, bool secureDeleting)
		{
			_log.ProcedureCall("RemoveAllFilesAtFolder", secureDeleting.ToString());

			file.Dispose();

            if (secureDeleting)
            {
                OverWriteFileWithZeros(file.FileName);
            }

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
			_log.WriteLine(LoggingEvent.Debug, String.Format(CultureInfo.InvariantCulture, "Operating System details: {0} {1} {2}", Environment.OSVersion.Platform, Environment.OSVersion.ServicePack, Environment.OSVersion.Version.ToString(3)));
			Thread.CurrentThread.Priority = _options.Priority;
            _log.WriteLine(LoggingEvent.Debug, "setted priorities to self " + _options.Priority);
		}
		
		void ApplyTranslation()
		{
			//GUIMesSTARTOFArchMess = Resources.7ZMessage;
			_backupAbortedByUser = Resources.AbortedByUser;
			//mProcStrOK = Resources.FileWasCopyiedToStorageSuccessfully;
			_couldNotVanishFileFormatString = Resources.DuringOverwritingFile0WithZerosAnErrorOccured1;
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

		~BackupProcess()
		{
			DoDispose(true);
		}

		#endregion

	}
}
