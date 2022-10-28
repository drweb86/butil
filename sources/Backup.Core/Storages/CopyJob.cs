using System;
using System.Globalization;
using System.Threading;


using BUtil.Core.Storages;
using BUtil.Core.Misc;
using BUtil.Core.Logs;
using BUtil.Core.Localization;

namespace BUtil.Core.Storages
{
	internal sealed class CopyJob: IJob
	{
		readonly StorageBase _storage;
		readonly string _imageFileToCopy;
		readonly LogBase _log;
		EventHandler<JobThreadEventArgs> _finished;

		public StorageBase Storage
		{
			get { return _storage; }
		}
		
		public EventHandler NotificationEventHandler = null;
		
		EventHandler<JobThreadEventArgs> IJob.TaskFinished
		{
			get { return _finished; }
			set { _finished = value; }
		}
		
		public CopyJob(StorageBase storage, string imageFileToCopy, LogBase log)
		{
			_storage = storage;
			_imageFileToCopy = imageFileToCopy;
			_log = log;
		}
		
		public void DoJob()
		{
			try
			{
				bool succesfull = true;


				_log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InstalledUICulture, "Processing storage '{0}'", _storage.StorageName));

				try
				{
					notify(new CopyingToStorageNotificationEventArgs(_storage.StorageName, ProcessingState.InProgress));
					_storage.Open(_log);
					_log.WriteLine(LoggingEvent.Debug, "Storage opened successfully");
					_storage.Process(_imageFileToCopy);
					_log.WriteLine(LoggingEvent.Debug, "File was copyied to storage successfully");
				}
				catch (LogException e)
				{
					_log.WriteLine(LoggingEvent.Error, string.Format(CultureInfo.InstalledUICulture, Resources.ErrorDiscoveredWhenOpeningStorage01, _storage.StorageName, e.Message));
					succesfull = false;
				}
				catch (Exception exc)// ok
				{
					if (exc is ThreadInterruptedException)
					{
						_log.WriteLine(LoggingEvent.Error, string.Format("Copying to storage {0} aborted by user", _storage.StorageName));
					}

					_log.WriteLine(LoggingEvent.Error, string.Format(CultureInfo.InstalledUICulture, Resources.ErrorDiscoveredWhenCopyingFileToStorage01, _storage.StorageName, exc.Message));
					succesfull = false;
				}

				notify(new CopyingToStorageNotificationEventArgs(_storage.StorageName, succesfull ? ProcessingState.FinishedSuccesfully : ProcessingState.FinishedWithErrors));
			}
			catch (ThreadInterruptedException)
			{
				_log.WriteLine(LoggingEvent.Error, string.Format("Copying to storage {0} aborted by user", _storage.StorageName));
			}
			finally
			{
				if (_finished != null)
					_finished.Invoke(this, new JobThreadEventArgs(Thread.CurrentThread));
			}
		}

		void notify(EventArgs e)
		{
			if (NotificationEventHandler != null)
			{
				NotificationEventHandler.Invoke(this, e);
			}
		}
	}
}
