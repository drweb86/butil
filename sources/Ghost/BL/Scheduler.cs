using System;
using System.Timers;
using BUtil.Core.Options;
using System.Diagnostics;
using System.Linq;

namespace BUtil.Ghost.BL
{
	/// <summary>
	/// This is a scheduler delegate that called 8 minutes before scheduled backup
	/// </summary>
	internal delegate void EightMinutesBeforeScheduledBackupRemained();	
	
	/// <summary>
	/// This is a scheduler delegate that is called when the time of action is come
	/// </summary>
	internal delegate void DoAction(Scheduler scheduler);

	/// <summary>
	/// Class implements time switching and running delegate method after
	/// a period of time.
	/// </summary>
	internal sealed class Scheduler: IDisposable
	{
		#region Constants
		
        const int _TIMER_PERIOD_IN_SECONDS = 30000;
        
        #endregion

        #region Private Fields
        
        DateTime _zeroHour = DateTime.MinValue;// time when to stop
        readonly BackupTask _task;
        System.Timers.Timer _actionTimer = new Timer();
        
		#endregion
        
		#region Events
		
		/// <summary>
		/// Occurs 8 minutes before zero hour
		/// </summary>
        public event EightMinutesBeforeScheduledBackupRemained EightMinutesRemainedEventHandler;
		
        /// <summary>
        /// Occurs at zero hour
        /// </summary>
        public event DoAction DoAction;

        #endregion
        
        #region Public Properties
        
        /// <summary>
        /// Time when the action must occur
        /// </summary>
        public DateTime ZeroHour
        {
            get { return _zeroHour; }
        }

        /// <summary>
        /// Task name
        /// </summary>
        public BackupTask Task
        {
            get { return _task; }
        }
        
		#endregion        
        
		#region Constructors
		
		/// <summary>
		/// The constructor
		/// </summary>
		/// <param name="task">backup task to schedule</param>
		public Scheduler(BackupTask task)
		{
			//TODO: task must be replced with h, m, days

           	_task = task;
            _actionTimer.Elapsed += new ElapsedEventHandler(onTimedEvent);
		}
	    
		#endregion
		
		#region Public Methods
		
	    /// <summary>
	    /// When running timer, stops timer, replans, configures timer, runs it(if planned)
	    /// </summary>
	    /// <exception cref="ArgumentNullException">ActionProc not setted</exception>
	    public void Resume()
	    {
            if (!_task.SchedulerDays.Any()) return;
            if (!_actionTimer.Enabled) return;

	        double difference = -1;
			var now = DateTime.Now;
			_zeroHour = now + _task.SchedulerTime;
            
            if (_task.SchedulerDays.Contains(_zeroHour.DayOfWeek))
            {
                difference = _zeroHour.Subtract(now).TotalMilliseconds;
            }
            else
            {
                // this day cannot be scheduled
                difference = -1;
            }
			
			while (difference < 0)
			{
				// here we're searching for scheduled time
				do
				{
                    _zeroHour = _zeroHour.AddDays(1);
				}
                while (!_task.SchedulerDays.Contains(_zeroHour.DayOfWeek));

                difference = _zeroHour.Subtract(now).TotalMilliseconds;
			}
			
            _actionTimer.Interval = _TIMER_PERIOD_IN_SECONDS;
			
			lock(this)
	    	{
                _actionTimer.Enabled = true;
	    	}
		}
	    
	    /// <summary>
	    /// Stops time planning after eventhandler did it work
	    /// </summary>
	    public void StopTimePlanning()
	    {
	    	lock(this)
	    	{
                _actionTimer.Enabled = false;
	    	}
	    }
	    
	    #endregion
	    
	    #region Private Mthods
	    
	    void onTimedEvent(object source, ElapsedEventArgs e)
		{
            double difference = _zeroHour.Subtract(DateTime.Now).TotalMilliseconds;
            Debug.WriteLine(difference);
            if ((difference <= (480000 + _TIMER_PERIOD_IN_SECONDS)) && (difference > ((480000 - _TIMER_PERIOD_IN_SECONDS))))
            {
                if (EightMinutesRemainedEventHandler != null)
                    EightMinutesRemainedEventHandler.Invoke();
            }

            if ((difference <= _TIMER_PERIOD_IN_SECONDS) && (difference > 0))
			{
				lock(this)
				{
                    if (_actionTimer.Enabled)
					{
                        _actionTimer.Enabled = false;
                        if (DoAction != null)
                            DoAction(this);
                        _actionTimer.Enabled = true;
					}
				}
			}

            Resume();
		}
	    
	    #endregion
        
	    #region IDisposable implementation
	    
	    bool _isDisposed;
        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        void dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                    _actionTimer.Dispose();
            }
            _isDisposed = true;
        }

        ~Scheduler()
        {
            dispose(false);
        }
        
		#endregion
	}
}
