using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;
using BUtil.Core;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.FileSystem;
using BUtil.Core.Options;
using BUtil.Core.PL;

using BUtil.Ghost.BL;
using BUtil.Ghost.Localization;
using System.Linq;

namespace BUtil.Ghost
{
	/// <summary>
	/// With this delegate controller can send messages to ui
	/// </summary>
	internal delegate void BackupNotification(string message);
	
	/// <summary>
	/// Controller of an Scheduler
	/// </summary>
    internal sealed class Controller
    {
        #region Private Fields
        
        readonly List<Scheduler> _schedulers;
        readonly ProgramOptions _options;
        BackupProcess _processor = null;

        #endregion
        
        #region Public Properties
        
        /// <summary>
        /// Occurs on backup event: starting, stopping, ending
        /// </summary>
        public event BackupNotification OnNotificationEvent;
        
        #endregion
        
        #region Constructor
        
        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="options">Options of a program</param>
        public Controller(ProgramOptions options)
        {
        	if (options == null)
        	{
        		throw new ArgumentNullException("options");
        	}

            _schedulers = new List<Scheduler>();
			_options = options;

            var backupTaskStoreService = new BackupTaskStoreService();
            var backupTasks = backupTaskStoreService.LoadAll();

            foreach (var backupTask in backupTasks)
            {
                if (backupTask.SchedulerDays.Any())
                {
                    var scheduler = new Scheduler(backupTask);
        			scheduler.DoAction += (p) => { DoBackup(p, true); };
                    scheduler.EightMinutesRemainedEventHandler += () => {  notifyUser(Resources.In8MinutesScheduledBackupWillStart); };
                    scheduler.Resume();

                    _schedulers.Add(scheduler);
                }
            }
        }
        
		#endregion

		#region Event Handlers
		
        /// <summary>
        /// Event occurs when backup completed or aborted
        /// </summary>
        void onBackupFinished()
		{
        	if (_processor != null)
        	{
	        	_processor.Dispose();
    	    	_processor = null;
    	    	notifyUser(Resources.BackupFinished);// that backup is finished
        	}

            foreach (var scheduler in _schedulers)
            {
                scheduler.Resume();
            }
        }
        
        /// <summary>
        /// stub. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void onNotificationReceived(object sender, EventArgs e)
		{
        	//HACK: find workaround of issue with missing event hadler
		}
        
        #endregion

        #region Private Methods
        
        /// <summary>
        /// Notifies the user via OnNotificationEvent about internal events of backup process: starting, ending, ...
        /// </summary>
        /// <param name="text">Text to pass to user</param>
        void notifyUser(string text)
        {
        	if (OnNotificationEvent != null)
        	{
        		OnNotificationEvent(text);
        	}
        }
        
        /// <summary>
        /// In this function backup process goes
        /// </summary>
        void thread_process(object parameter)
        {
            var scheduler = (Scheduler)parameter;
        	FileLog log = null; 
			try
			{
				log = new FileLog(_options.LogsFolder, _options.LoggingLevel, true);
				log.Open();
			}
			catch (LogException e)
			{
				// "Cannot open file log due to crytical error {0}"
				Messages.ShowErrorBox(string.Format(CultureInfo.InstalledUICulture, Resources.CannotOpenFileLogDueToCryticalError0, e.Message));
				onBackupFinished();
			}

            // initing objects that we should dispose
            _processor = new BackupProcess(new List<BackupTask>{scheduler.Task}, _options, log);
			_processor.BackupFinished += onBackupFinished;
			_processor.NotificationEventHandler += onNotificationReceived;
			_processor.Run();
			
			onBackupFinished();
        }

		#endregion

		#region Public Methods
		
        /// <summary>
        /// This is standard backup function that can be called from ui
        /// Does not check interface. Works fast
        /// </summary>
        /// <param name="scheduler">Scheduler</param>
        /// <param name="checkCpu">Flag shows wheather to do cpu usage check</param>
        public void DoBackup(Scheduler scheduler, bool checkCpu)
        {
            // we're starting new backup / scheduled backup only if none of previouds backups in process
            if (!IsBackupInProgress())
            {
                if (checkCpu)
                {
                    using (CpuUsage cpu = new CpuUsage())
                    {
                        cpu.WaitUntilCpuLoadingLess(_options.PuttingOffBackupCpuLoading);
                    }
                }

                notifyUser(Resources.BackupStarted);// that backup is started
                var paramThread = new ParameterizedThreadStart(thread_process);
                new Thread(paramThread).Start(scheduler);
            }
        }

        /// <summary>
        /// Shows if backup is in progress
        /// </summary>
        /// <returns>True if backup in progress</returns>
        public bool IsBackupInProgress()
        {
            return (_processor != null);
        }

        /// <summary>
        /// Gets the scheduling state
        /// </summary>
        /// <returns>String with information about scheduling state</returns>
        public string GetSchedulingState()
        {
            if (!IsBackupInProgress())
            {
                var zeroHour = DateTime.MaxValue;
                foreach (var scheduler in _schedulers)
                {
                    if (zeroHour > scheduler.ZeroHour)
                    {
                        zeroHour = scheduler.ZeroHour;
                    }
                }
                return string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.NewBackupWillStartIn0,
                    PrettyTimeSpanFormatter.Format(zeroHour));
            }
            else
            {
                return Resources.BackupNowInAProgress;
            }
        }

        /// <summary>
        /// Aborts backup that is in progress
        /// </summary>
        /// <exception cref="InvalidOperationException">Backup isn't in progress</exception>
        public void AbortBackup()
        {
        	if (_processor == null)
        	{
        		throw new InvalidOperationException("Backup isn't in progress");
        	}

        	notifyUser(Resources.StoppingTheBackup);// that cancelling in a progress
            
            // start: here refactoring the
            foreach (var scheduler in _schedulers)
            {
                scheduler.StopTimePlanning();
            }
            // -- end
            _processor.StopForcibly();
        }
        
        #endregion
    }
}
