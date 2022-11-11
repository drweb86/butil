using System;
using System.Threading;
using System.Diagnostics;

namespace BUtil.Core.Options
{
    public sealed class ProgramOptions: ICloneable
	{
    	const string _INVALID_CPU_LOADING = "CPU loading is out of bounds of valid values!";
    	
        byte _puttingOffBackupCpuLoading;
	  
		public int AmountOfStoragesToProcessSynchronously {get; set;}
		public int AmountOf7ZipProcessesToProcessSynchronously { get; set; }
		public string LogsFolder { get; set; }
	    public ThreadPriority Priority { get; set; }
		public ProcessPriorityClass ProcessPriority
		{
			get
			{
				switch (Priority)
				{
					case ThreadPriority.AboveNormal:
						return ProcessPriorityClass.AboveNormal;
					case ThreadPriority.BelowNormal:
						return ProcessPriorityClass.BelowNormal;
					case ThreadPriority.Normal:
						return ProcessPriorityClass.Normal;
					case ThreadPriority.Lowest:
						return ProcessPriorityClass.Idle;
					default:
						// Highest case
						throw new NotSupportedException(Priority.ToString());
				}
			}
		}
        
        public byte PuttingOffBackupCpuLoading
		{
            get { return _puttingOffBackupCpuLoading; }
            set 
            { 
				if ((value < Constants.MinimumCpuLoading) || 
                  (value > Constants.MaximumCpuLoading))
            	{
            		throw new ArgumentException(_INVALID_CPU_LOADING);
            	}
            	_puttingOffBackupCpuLoading = value;
            }
		}

        public object Clone()
        {
			return this.MemberwiseClone();
        }

        public bool CompareTo(ProgramOptions other)
        {
			return other.LogsFolder == LogsFolder &&
				other.PuttingOffBackupCpuLoading == PuttingOffBackupCpuLoading &&
				other.AmountOf7ZipProcessesToProcessSynchronously == AmountOf7ZipProcessesToProcessSynchronously &&
				other.AmountOfStoragesToProcessSynchronously == AmountOfStoragesToProcessSynchronously &&
                other.Priority == Priority;

        }
    }
}
