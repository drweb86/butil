using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core;
using BUtil.Core.PL;
using NativeMethods = BUtil.BackupUiMaster.NativeMethods;
using BUtil.Configurator.Localization;
using System.Runtime.InteropServices;

[assembly: CLSCompliant(true)]
namespace BUtil.Configurator.BackupUiMaster
{
    internal class BackupUiMaster: IDisposable
    {
        public delegate void OnBackupFinished ();

        public OnBackupFinished BackupFinished;

        #region Fields

        private readonly ProgramOptions _options;
        private readonly BackupTask _task;
        private BackupProcess _backupper;
        private string _fileLogFile;

        #endregion

        #region disposing

        private bool _isDisposed;
		void IDisposable.Dispose()
		{
			Dispose(false);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool flag)
		{
			if (!_isDisposed)
			{
                if (_backupper != null)
                {
                    _backupper.Dispose();
                }
				_isDisposed = true;
			}
		}
		
		~BackupUiMaster()
		{
			Dispose(true);
		}
		#endregion

		#region Properties

        public PowerTask PowerTask { get; set; }

        public bool ErrorsOrWarningsRegistered
        {
            get { return _backupper.ErrorsOrWarningsRegistered; }
        }

        public bool HearSoundWhenBackupCompleted { get; set; }

        public ProgramOptions Options
        {
            get { return _options; }
        }

        public BackupProcess BackupClass
        {
            get { return _backupper; }
        }
        public ReadOnlyCollection<CompressionItem> ListOfFiles
        {
        	get { return new ReadOnlyCollection<CompressionItem>(_task.What); }
		}

		#endregion
		
		public BackupUiMaster(BackupTask task, ProgramOptions options)
		{
		    PowerTask = PowerTask.None;
		    _options = options;
            _task = task;
		}
		
        public void OpenLogFileInBrowser()
        {
        	SupportManager.OpenWebLink(_fileLogFile);
        }
        
        public void Abort()
        {
        	BackupClass.StopForcibly();
        }

        private void onBackupFinsihedHelper()
        {
        	BackupClass.Dispose();

            if (BackupFinished != null)
                BackupFinished.Invoke();

            if (HearSoundWhenBackupCompleted) 
                Miscellaneous.DoBeeps();
            
            // in support mode we always opening log and do not perform power task
            if (_options.LoggingLevel == LogLevel.Support)
            {
				SupportManager.OpenWebLinkAsync(_fileLogFile);
            	
				Environment.Exit(0);
            }
            else
            {
				if (ErrorsOrWarningsRegistered)
				{
	                // user chose to shutdown PC or logoff from it. In this case we should
	                // add a registry key in RunOnce section to show him log in browser
	                // of backup when he will login into the system next time
	                if ((PowerTask == PowerTask.Shutdown) || 
	                    (PowerTask == PowerTask.Reboot) || 
	                    (PowerTask == PowerTask.LogOff))
	                {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            NativeMethods.ScheduleOpeningFileAfterLoginOfUserIntoTheSystem(_fileLogFile);
                        }
                        else
                        {
                            _backupper.Log.WriteLine(LoggingEvent.Error, "Cannot schedule opening file after logging user into the system");
                        }
	                }
	                else
	                // Hibernate, Sleep, Nothing
	                // we should open browser and perform required power operation
	                {
						SupportManager.OpenWebLinkAsync(_fileLogFile);
	                }
	            }
	            // No problems during backup registered. In this case we should notify
	            // user that's all is ok
	            else
	            {
                    // user is here and we can show him the message
					if (PowerTask == PowerTask.None)
					{
						MessageBox.Show(Resources.BackupProcessCompletedSuccesfully, ";-)", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);
					}
				}
				
	            PowerPC.DoTask(PowerTask);
				
	            if (PowerTask == PowerTask.None && ErrorsOrWarningsRegistered)
	            {
	            	;
	            }
	            else
	            {
	            	Environment.Exit(0);
	            }
            }
        }

        public void PrepareBackup()
        {
            var log = new FileLog(_options.LogsFolder, _options.LoggingLevel, false);
            _fileLogFile = log.LogFilename;
            log.Open();
            _backupper = new BackupProcess(new List<BackupTask>{_task}, Options, log);

            _backupper.BackupFinished +=
				new BackupFinished(onBackupFinsihedHelper);
            _backupper.CriticalErrorOccur +=
				new CriticalErrorOccur(Messages.ShowErrorBox);
        }
    }
}
