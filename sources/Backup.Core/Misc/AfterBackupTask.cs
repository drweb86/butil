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
	/// The after backup task that occurs in the middle of backup process after copying to the storages
	/// </summary>
	internal sealed class AfterBackupTask: IJob
	{
		#region Constants
		
		const string _REPLACE_ON_BACKUP_PARAMETER = "$BackupImageFile";
		
		#endregion
		
		#region Fields
		
		// log messages
		readonly string _taskName;
		readonly string _backupImageLocation;
		readonly BackupEventTaskInfo _taskInfo;
		readonly LogBase _log;
		EventHandler<JobThreadEventArgs> _finished;
		ProcessPriorityClass _priority;

		#endregion
		
		#region Events
		
		public EventHandler NotificationEventHandler = null;
		
		EventHandler<JobThreadEventArgs> IJob.TaskFinished
		{
			get { return _finished; }
			set { _finished = value; }
		}
		
		#endregion

		#region Constructors
		
		/// <summary>
		/// The default constructor
		/// </summary>
		/// <param name="taskInfo">The task to run</param>
		/// <param name="backupImageLocation">The exact image location in temp directory</param>
		/// <param name="log">The opened log instance</param>
		/// <exception cref="ArgumentNullException">taskInfo or log or backupImageLocation is null or empty</exception>
		/// <exception cref="InvalidOperationException">Log is not opened</exception>
		public AfterBackupTask(BackupEventTaskInfo taskInfo, string backupImageLocation, ProcessPriorityClass priority, LogBase log)
		{
			if (taskInfo == null)
			{
				throw new ArgumentNullException("taskInfo");
			}
			
			if (string.IsNullOrEmpty(backupImageLocation))
			{
				throw new ArgumentNullException("backupImageLocation");
			}
			
			if (log == null)
			{
				throw new ArgumentNullException("log");
			}
			
			if (!log.IsOpened)
			{
				throw new InvalidOperationException("Log is not opened");
			}
			
			_taskName = Resources.AfterBackupTask + taskInfo.ToString() + " :";
			_taskInfo = taskInfo;
			_backupImageLocation = backupImageLocation;
			_priority = priority;
			_log = log;
		}
		
		#endregion
				
		#region Public methods
		
		public void DoJob()
		{
			_log.WriteLine(LoggingEvent.Debug, _taskName);

			bool succesfull = false;
			Process program = new Process();
			program.StartInfo = new ProcessStartInfo
				(_taskInfo.Program, 
				 _taskInfo.Arguments.Replace(
				 	_REPLACE_ON_BACKUP_PARAMETER, 
				 	_backupImageLocation));

			program.StartInfo.WorkingDirectory=Path.GetDirectoryName(_taskInfo.Program);

			try
			{
				program.Start();

				notify(ProcessingState.InProgress);
				
				setPriorityHelper(program);

				// FIXME: This is a workaround of .Net 2 bugs with processing ThreadAbortException and ThreadInterruptException
				// both bugs were posted to MS
				// this code is subject to be removed when those bugs will be fixed or on updating to next .Net version
				while (!program.HasExited)
				{
					Thread.Sleep(100);
				}
				// program.WaitForExit();
				
				succesfull = true;
			}
			catch(ThreadInterruptedException)
			{
				succesfull = false;
				_finished = null;
				_log.WriteLine(LoggingEvent.Debug, _taskName + "Aborting...");
					
				try
				{
					program.Kill();
					_log.WriteLine(LoggingEvent.Debug, _taskName + "Aborted");
				}
				catch (InvalidOperationException e)
				{
					_log.WriteLine(LoggingEvent.Error,
			              string.Format(CultureInfo.CurrentCulture, _taskName + Resources.CouldNotKillProcess01, _taskInfo.Program, e.Message));
					_log.WriteLine(LoggingEvent.Debug, _taskName + e.ToString());
				}
				catch (Win32Exception e)
				{
					_log.WriteLine(LoggingEvent.Error,
			              string.Format(CultureInfo.CurrentCulture, _taskName + Resources.CouldNotKillProcess01, _taskInfo.Program, e.Message));
					_log.WriteLine(LoggingEvent.Debug, _taskName + e.ToString());
				}
			}			
			catch (ObjectDisposedException e)
			{
				_log.WriteLine(LoggingEvent.Error,
		              string.Format(CultureInfo.CurrentCulture, _taskName + Resources.CouldNotStartProcess01, _taskInfo.Program, e.Message));
				_log.WriteLine(LoggingEvent.Debug, _taskName + e.ToString());
			}
			catch (Win32Exception e)
			{
				_log.WriteLine(LoggingEvent.Error,
		              string.Format(CultureInfo.CurrentCulture, _taskName + Resources.CouldNotStartProcess01, _taskInfo.Program, e.Message));
				_log.WriteLine(LoggingEvent.Debug, _taskName + e.ToString());
			}
			finally
			{
				notify(succesfull ? ProcessingState.FinishedSuccesfully: ProcessingState.FinishedWithErrors);

				if (_finished != null)
					_finished.Invoke(this, new JobThreadEventArgs(Thread.CurrentThread));
			}
			
			_log.WriteLine(LoggingEvent.Debug, _taskName + "Finished");
		}
		
		#endregion
		
		#region Private Methods
		
		void setPriorityHelper(Process process)
		{
			try
			{
				process.PriorityClass = _priority;
				_log.WriteLine(LoggingEvent.Debug, _taskName + "Priority set to " + _priority);
			}
			catch (PlatformNotSupportedException)
			{
				_log.WriteLine(LoggingEvent.Debug, _taskName + string.Format(CultureInfo.InvariantCulture, "Could not set priority '{0}' - platform does not support it. Setting idle priority", _priority.ToString()));
				_priority = ProcessPriorityClass.Idle;
				setPriorityHelper(process);
			}
			catch (InvalidOperationException e)
			{
				_log.WriteLine(LoggingEvent.Debug, _taskName + string.Format(CultureInfo.InvariantCulture, "Could not set priority to process because it already exited: {0}", e.Message));
			}
			catch (Win32Exception e)
			{
				_log.WriteLine(LoggingEvent.Debug, _taskName + string.Format(CultureInfo.InvariantCulture, "Could not set priority because '{0}'", e.Message));
			}
		}	

		void notify(ProcessingState state)
		{
			if (NotificationEventHandler != null)
			{
				NotificationEventHandler.Invoke(this, new RunProgramBeforeOrAfterBackupEventArgs(_taskInfo, state));
			}
		}

		#endregion
	}
}
