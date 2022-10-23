using System;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Text;
using System.IO;


using BUtil.Core.Storages;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;

namespace BUtil.Core.Misc
{
	/// <summary>
	/// The compression to an archive of a CompressionItem
	/// </summary>
	internal sealed class CompressionJob: IJob
	{
		#region Fields
		
		readonly ArchiveTask _packingParameter;
		string _compressionOutput = null;
		readonly bool _enableEncryption;
		readonly LogBase _log;
		EventHandler<JobThreadEventArgs> _finished;

		#endregion
		
		#region Properties
		
		public CompressionItem ItemToCompress
		{
			get { return _packingParameter.ItemToCompress; }
		}
		
		public EventHandler NotificationEventHandler = null;
		
		EventHandler<JobThreadEventArgs> IJob.TaskFinished
		{
			get { return _finished; }
			set { _finished = value; }
		}
		
		#endregion
		
		#region Constructors
		
		public CompressionJob(ArchiveTask parameter, bool enableEncryption, LogBase log)
		{
			_packingParameter = parameter;
			_enableEncryption = enableEncryption;
			_log = log;
		}
		
		#endregion
				
		#region Public methods
		
		public void DoJob()
		{
			_log.WriteLine(LoggingEvent.Debug, _packingParameter.ToString() + ":started");
			bool succesfull = false;
			using (Process compressionProcess = createCompressProcess(_packingParameter.Arguments))
			{
				try
				{
					compressionProcess.Start();
					Thread thread = new Thread(new ParameterizedThreadStart(readOutput));
					thread.Start(compressionProcess.StandardOutput);

					notify(new PackingNotificationEventArgs(_packingParameter.ItemToCompress, ProcessingState.InProgress));
					setPriorityHelper(compressionProcess);
	
					// FIXME: This is a workaround of .Net 2 bugs with processing ThreadAbortException and ThreadInterruptException
					// both bugs were posted to MS
					// this code is subject to be removed when those bugs will be fixed
					
					while (!compressionProcess.HasExited)
					{
						Thread.Sleep(100);
					}
					
					while (thread.IsAlive)
					{
						Thread.Sleep(1000);
					}

					succesfull = isSuccessfull7ZipPacking(compressionProcess.ExitCode);
					_log.ProcessPackerMessage(_compressionOutput, succesfull);
					notify(new PackingNotificationEventArgs(_packingParameter.ItemToCompress, succesfull ? ProcessingState.FinishedSuccesfully: ProcessingState.FinishedWithErrors));
				}
				catch(ThreadInterruptedException)
				{
					succesfull = false;
					_finished = null;
					_log.WriteLine(LoggingEvent.Debug, _packingParameter.ItemToCompress.Target + ": Packing task is aborting...");
						
					try
					{
						compressionProcess.Kill();
						_log.WriteLine(LoggingEvent.Debug, _packingParameter.ItemToCompress.Target + ": Packing task is aborted");
					}
					catch (InvalidOperationException e)
					{
						_log.WriteLine(LoggingEvent.Error,
				              string.Format(CultureInfo.CurrentCulture, _packingParameter.ItemToCompress.Target + ":" + Resources.CouldNotKillPackerProcess0, e.Message));
					}
					catch (Win32Exception e)
					{
						_log.WriteLine(LoggingEvent.Error,
				              string.Format(CultureInfo.CurrentCulture, _packingParameter.ItemToCompress.Target + ":" + Resources.CouldNotKillPackerProcess0, e.Message));
					}
				}			
				catch (ObjectDisposedException e)
				{
					_log.WriteLine(LoggingEvent.Error,
					               string.Format(CultureInfo.CurrentCulture,
					                             _packingParameter.ItemToCompress.Target + ": " + Resources.CouldNotStartPackerDueTo0AbortingBackup,
					                             e.Message));
				}
				catch (Win32Exception e)
				{
					_log.WriteLine(LoggingEvent.Error,
					               string.Format(CultureInfo.CurrentCulture,
					                             _packingParameter.ItemToCompress.Target + ": " + Resources.CouldNotStartPackerDueTo0AbortingBackup,
					                             e.Message));
				}
				finally
				{
					if (_finished != null)
						_finished.Invoke(this, new JobThreadEventArgs(Thread.CurrentThread));
				}
			}
			
			_log.WriteLine(LoggingEvent.Debug, _packingParameter.ToString() + ":finished");
		}
		
		#endregion
		
		#region Private Methods
		
		/// <summary>
		/// This is a workaround of .net issue with dead lock of 7-zip
		/// </summary>
		/// <param name="obj">StreamReader object</param>
		void readOutput(object obj)
		{
			StreamReader writer = (StreamReader)obj;
			_compressionOutput = writer.ReadToEnd();
		}
		
		void setPriorityHelper(Process process)
		{
			try
			{
				process.PriorityClass = _packingParameter.Priority;
				_log.WriteLine(LoggingEvent.Debug, _packingParameter.ItemToCompress.Target + ": Process priority set to " + _packingParameter.Priority);
			}
			catch (PlatformNotSupportedException)
			{
				_log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InvariantCulture, _packingParameter.ItemToCompress.Target + ": Could not set priority '{0}' - platform does not support it. Setting idle priority", _packingParameter.Priority.ToString()));
				_packingParameter.Priority = ProcessPriorityClass.Idle;
				setPriorityHelper(process);
			}
			catch (InvalidOperationException e)
			{
				_log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InvariantCulture, _packingParameter.ItemToCompress.Target + ": Could not set priority to process because it already exited: {0}", e.Message));
			}
			catch (Win32Exception e)
			{
				_log.WriteLine(LoggingEvent.Debug, string.Format(CultureInfo.InvariantCulture, _packingParameter.ItemToCompress.Target + ": Could not set priority because '{0}'", e.Message));
			}
		}	
		
		static Process createCompressProcess(string arguments)
		{
			Process result = new Process();
			result.StartInfo.UseShellExecute = false;
			result.StartInfo.RedirectStandardOutput = true;
			result.StartInfo.RedirectStandardError = false;
			result.StartInfo.CreateNoWindow = true;
			result.StartInfo.FileName = Files.SevenZipPacker;
			result.StartInfo.Arguments = arguments;
			
			return result;
		}
				
		void notify(EventArgs e)
		{
			if (NotificationEventHandler != null)
			{
				NotificationEventHandler.Invoke(this, e);
			}
		}
		
		bool isSuccessfull7ZipPacking(int code)
		{
			bool result = false;
			switch (code)
			{
				case (int)SevenZipReturnCodes.OK:
					_log.WriteLine(LoggingEvent.Debug, "Archivator did it work OK");
					result = true;
					break;
				case (int)SevenZipReturnCodes.ErrorsOccured: _log.WriteLine(LoggingEvent.Error, _packingParameter.ItemToCompress.Target + ": " + Resources.ArchivatorWarningNonFatalErrorSForExampleOneOrMoreFilesWereLockedBySomeOtherApplicationSoTheyWereNotCompressed); break;
				case (int)SevenZipReturnCodes.FatalErrorsOccured: _log.WriteLine(LoggingEvent.Error, _packingParameter.ItemToCompress.Target + ": " + Resources.ArchivatorFatalError); break;
				case (int)SevenZipReturnCodes.InvalidArguments: _log.WriteLine(LoggingEvent.Error, _packingParameter.ItemToCompress.Target + ": " + Resources.PleaseReportThisBugToMeArchivatorCommandLineError); break;
				case (int)SevenZipReturnCodes.NotEnoughMemory: _log.WriteLine(LoggingEvent.Error, _packingParameter.ItemToCompress.Target + ": " + Resources.ArchivatorNotEnoughMemoryForOperation); break;
				case (int)SevenZipReturnCodes.ExternalTermination: _log.WriteLine(LoggingEvent.Error, _packingParameter.ItemToCompress.Target + ": " + Resources.UserStoppedTheProcess); break;
				default:
					_log.WriteLine(LoggingEvent.Error, string.Format(CultureInfo.InvariantCulture, _packingParameter.ItemToCompress.Target + ": Abnormal 7-zip exit code: {0}. Please report this bug!", code.ToString(CultureInfo.InvariantCulture)));
					break;
			}
			
			return result;
		}
		
		#endregion
	}
}
