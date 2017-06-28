#region Copyright
/*
 * Copyright (c)Cuchuk Sergey Alexandrovich, 2007-2008. All rights reserved
 * Project: BUtil
 * Link: http://www.sourceforge.net/projects/butil
 * License: GNU GPL or SPL with limitations
 * E-mail:
 * Cuchuk.Sergey@gmail.com
 * toCuchukSergey@yandex.ru
 */
#endregion



using System;
using System.Timers;
using BUtil.Scheduling; 

namespace BUtilService.BL
{
	/// <summary>
	/// Class implements time switching and running delegate method after
	/// a period.
	/// 
	/// tested completely
	/// </summary>
	public class Scheduler: IDisposable
	{
        private const string _MORETHENONEINSTANCECREATED = "Second call of Scheduler's constructor appeared";

        private const int _TIMERPERIODINSECOND = 30000;

        private DateTime _zeroHour = DateTime.MinValue;// time when to stop
		private ScheduleOptions _options = ScheduleOptionsManager.Default;
        private System.Timers.Timer _actionTimer = new Timer();
        private bool _disposed = false;

        public event System.EventHandler DoAction = null;
	    
	    public ScheduleOptions Options
	    {
            get
            {
                return _options;
            }
	    	set 
	    	{
	    		ScheduleOptionsManager.Validate(value);
	    		// if we has some options before
                if (value != null)
	    		{
	    			#if DEBUG
					Console.WriteLine("We had some previous options - so stopping time planning to set them carefully" +
	    			                  "\nif we have backupin a progress it will continue to be so - so here will be a stop");
					#endif
	    			
	    			StopTimePlanning();
                    _options = value; 
	    		}
	    	}
	    }
	    
		public Scheduler()
		{
            _actionTimer.Elapsed += new ElapsedEventHandler(onTimedEvent);
		}
	    
	    /// <summary>
	    /// Stops timer, replans, configures timer, runs it(if planned)
	    /// </summary>
	    /// <exception cref="ArgumentNullException">ActionProc not setted</exception>
	    public void Resume()
	    {
            if (!_options.EnableScheduling) return;
	    	
	        double difference = -1;
	        DateTime now = DateTime.Now;
	        
	        // getting new time
	        
	        // fixed bug in initialization
            _zeroHour = new DateTime(now.Year, now.Month, now.Day, _options.Hours, _options.Minutes, 0);
			
            if (_options.IsThisDayOfWeekScheduled(_zeroHour.DayOfWeek))
                difference = _zeroHour.Subtract(now).TotalMilliseconds;
			// this day cannot be scheduled
			else
				difference = -1;
			
			while (difference < 0)
			{
				// here we're searching for scheduled time
				do
				{
                    _zeroHour = _zeroHour.AddDays(1);
				}
                while (!_options.IsThisDayOfWeekScheduled(_zeroHour.DayOfWeek));

                difference = _zeroHour.Subtract(now).TotalMilliseconds;
			}
			
            _actionTimer.Interval = _TIMERPERIODINSECOND;
			
			lock(this)
	    	{
                _actionTimer.Enabled = true;
	    	}
			return;
		}
	    
	    /// <summary>
	    /// Stops time planning after eventhandler did it work
	    /// </summary>
	    public void StopTimePlanning()
	    {
	    	#if DEBUG
			Console.WriteLine("Stopping time planning");
			#endif

	    	lock(this)
	    	{
                _actionTimer.Enabled = false;
	    	}
	    	#if DEBUG
			Console.WriteLine("Time planning stopped");
			#endif
	    	
	    }
	    
	    void onTimedEvent(object source, ElapsedEventArgs e)
		{
            double difference = DateTime.Now.Subtract(_zeroHour).TotalMilliseconds;
            if ((difference <= _TIMERPERIODINSECOND) && (difference > 0))
			{
				lock(this)
				{
                    if (_actionTimer.Enabled)
					{
                        _actionTimer.Enabled = false;
                        if (DoAction != null)
                            DoAction.Invoke(this, null);
                        _actionTimer.Enabled = true;
					}
				}
			}
		}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                    _actionTimer.Dispose();
            }
            _disposed = true;
        }

        ~Scheduler()
        {
            Dispose(false);
        }
	}
}
