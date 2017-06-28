
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using BUtil.Core.Storages;

namespace BUtil.Core.Options
{
	/// <summary>
	/// Backup task is the main task of backup
	/// </summary>
    public sealed class BackupTask: ICloneable
	{
    	#region Constants
    	
    	// Do not localize
        const string _INVALID_HOUR = "Hour exceeds 23!";
        const string _INVALID_MINUTE = "Minute exceeds 59!";
        
        #endregion
    	
		#region Fields
		
		string _secretPassword;
        string _name;
		readonly Collection<CompressionItem> _filesFoldersList = new Collection<CompressionItem>();
		readonly Collection<StorageBase> _storages = new Collection<StorageBase>();		
		List<BackupEventTaskInfo> _beforeBackupTasksChain = new List<BackupEventTaskInfo>();
		List<BackupEventTaskInfo> _afterBackupTasksChain = new List<BackupEventTaskInfo>();

		byte _hours;
        byte _minutes;
        readonly bool[] _scheduledDays = new bool[7];
		
        #endregion
        
        #region Properties
		
        /// <summary>
        /// Chain of tasks to perform before backup
        /// </summary>
        public List<BackupEventTaskInfo> BeforeBackupTasksChain
        {
        	get { return _beforeBackupTasksChain; }
        	set { _beforeBackupTasksChain = value; }
        }
        
        /// <summary>
        /// Chain of tasks to perform after backup
        /// </summary>
        public List<BackupEventTaskInfo> AfterBackupTasksChain
        {
        	get { return _afterBackupTasksChain; }
        	set { _afterBackupTasksChain = value; }
        }
        
        /// <summary>
        /// Shows wheather scheduling was set up in task
        /// </summary>
		public bool EnableScheduling
		{
			get 
			{
				for (int i = 0; i < _scheduledDays.Length; i++)
				{
					if (_scheduledDays[i])
						return true;
				}

				return false;
			}
		}

		/// <summary>
		/// Zero hour for scheduler
		/// </summary>
		public byte Hours
		{
			get { return _hours; }
			set 
            {
                if (value > 23)
                    throw new ArgumentException(_INVALID_HOUR);

                _hours = value; 
            }
		}

		/// <summary>
		/// Zero minute for scheduler
		/// </summary>
		public byte Minutes
		{
			get { return _minutes; }
			set 
            {
                if (value > 59)
                    throw new ArgumentException(_INVALID_MINUTE);

                _minutes = value; 
            }
        }
        
		/// <summary>
		/// Backup task name
		/// </summary>
        public string Name
        {
        	get { return _name; }
        	set { _name = value; }
        }
        
        /// <summary>
		/// Places where to store backup
		/// </summary>
		public Collection<StorageBase> Storages
		{
			get { return _storages; }
		}
		
	    /// <summary>
	    /// Demands secure deletion of files
	    /// </summary>
	    public bool SecureDeletingOfFiles
	    {
            get { return EnableEncryption; }
	    }
	    
	    /// <summary>
	    /// Password
	    /// </summary>
	    public string SecretPassword
	    {
	        get { return _secretPassword; }
	        set { _secretPassword = value; }
	    }

	    /// <summary>
	    /// Shows if the password exists
	    /// </summary>
	    public bool EnableEncryption
	    {
	    	get { return !string.IsNullOrEmpty(_secretPassword); }
	    }
		
		/// <summary>
		/// List of items to compress
		/// </summary>
		public Collection<CompressionItem> FilesFoldersList
		{
            get { return _filesFoldersList; }
		}
		
		#endregion
		
		public void UnscheduleAllDays()
		{
			for(int i = 0; i < _scheduledDays.Length; i++)
			{
				_scheduledDays[i] = false;
			}
		}
		
		public bool IsThisDayOfWeekScheduled(DayOfWeek day)
	  	{
        	return this._scheduledDays[(int)day];
	  	}

        public void SetSchedulingStateOfDay(DayOfWeek day, bool isScheduled)
        {
        	_scheduledDays[(int)day] = isScheduled;
        }
		
        /// <summary>
        /// Deep cloning
        /// </summary>
        /// <returns>the copy</returns>
		public object Clone()
		{
			BackupTask profile = new BackupTask();
			profile._name = _name;
			profile._secretPassword = _secretPassword;
			profile._hours = _hours;
			profile._minutes = _minutes;

			for (int i = 0; i < profile._scheduledDays.Length; i++)
			{
				profile._scheduledDays[i] = _scheduledDays[i];
			}
			
			foreach (BackupEventTaskInfo info in _beforeBackupTasksChain)
			{
				profile._beforeBackupTasksChain.Add(info);
			}
			
			foreach (BackupEventTaskInfo info in _afterBackupTasksChain)
			{
				profile._afterBackupTasksChain.Add(info);
			}
			
			foreach (CompressionItem item in _filesFoldersList)
			{
				profile._filesFoldersList.Add(item);
			}
			
			foreach (StorageBase item in _storages)
			{
				profile._storages.Add(item);
			}
			
			return profile;
		}
	}
}