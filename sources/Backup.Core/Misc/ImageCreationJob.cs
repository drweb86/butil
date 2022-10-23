using System;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Collections.ObjectModel;


using BUtil.Core.Storages;
using BUtil.Core.Misc;
using BUtil.Core.Logs;
using BUtil.Core.FileSystem;
using BUtil.Core.Synchronization;
using BUtil.Core.ButilImage;
using BUtil.Core.Localization;

namespace BUtil.Core.Storages
{
	/// <summary>
	/// Packs compressed data to an image
	/// </summary>
	internal sealed class ImageCreationJob: IJob
	{
		#region Fields
		
		readonly SyncFile _imageFile;		
		readonly LogBase _log;
		EventHandler<JobThreadEventArgs> _finished;
		readonly Collection<MetaRecord> _metarecords;
		public EventHandler NotificationEventHandler = null;
		
		#endregion
		
		#region Properties
		
		EventHandler<JobThreadEventArgs> IJob.TaskFinished
		{
			get { return _finished; }
			set { _finished = value; }
		}
		
		public ImageCreationJob(LogBase log, SyncFile imageFile, Collection<MetaRecord> metarecords)
		{
			_imageFile = imageFile;
			_log = log;
			_metarecords = metarecords;
		}
		
		#endregion
		
		#region Public Methods
		
		public void DoJob()
		{
			bool succesfull = false;

			try
			{
				notify(new ImagePackingNotificationEventArgs(ProcessingState.InProgress));

				reserveFileForBackupImage();
				ImageCreator imagePacker = new ImageCreator(_imageFile.FileName, new ImageHeader(_metarecords));
				imagePacker.Pack(true);
				succesfull = true;
			}
			catch (FileNotFoundException e)
			{
				_log.WriteLine(LoggingEvent.Error, string.Format(Resources.PackingAbortedComponentOfAnImageFile0DoesNotExistIsItEnoughSpaceInTemporaryFolder,e.Message));
				_log.WriteLine(LoggingEvent.Debug, e.ToString());			
			}
			catch(IOException e)
			{
				_log.WriteLine(LoggingEvent.Error, string.Format(Resources.DuringPackingFilesToAnImage0AnErrorOccured1,_imageFile.FileName, e.Message));
				_log.WriteLine(LoggingEvent.Debug, e.ToString());			
			}
			catch(ThreadInterruptedException)
			{
				_log.WriteLine(LoggingEvent.Error, Resources.PackingOfDataToAnImageWasAbortedByUser);
			}
			finally
			{
				notify(new ImagePackingNotificationEventArgs(succesfull ? ProcessingState.FinishedSuccesfully: ProcessingState.FinishedWithErrors));

				if (_finished != null)
					_finished.Invoke(this, new JobThreadEventArgs(Thread.CurrentThread));
			}
		}
		
		#endregion

		#region Private Methods
		
		void notify(EventArgs e)
		{
			if (NotificationEventHandler != null)
			{
				NotificationEventHandler.Invoke(this, e);
			}
		}
		
		void reserveFileForBackupImage()
		{
			_log.ProcedureCall("reserveFileForBackupImage");

			string templateFileName = Path.Combine(Directories.TempFolder, 
			                                       "Backup{0}-" + DateTime.Now.ToString("dddd - dd - MMMM.yyyy HH.mm.ss", CultureInfo.InvariantCulture) + Files.ImageFilesExtension);

			for (int id = 0; id < Int32.MaxValue; id++) 
			{
				string fileName;
				if (id > 0) 
				{
					fileName = string.Format(templateFileName, id, CultureInfo.InvariantCulture);
				}
				else 
				{
					fileName = string.Format(templateFileName, string.Empty, CultureInfo.InvariantCulture);
				}

				if (_imageFile.TrySyncFile(fileName)) 
				{
					return;
				}
			}

			throw new InvalidOperationException("Fatal Error: could not reserve file name");
		}
		
		#endregion
	}
}
