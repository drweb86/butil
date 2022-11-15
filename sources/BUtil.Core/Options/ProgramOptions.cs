using System;

namespace BUtil.Core.Options
{
    public sealed class ProgramOptions: ICloneable
	{
    	const string _INVALID_CPU_LOADING = "CPU loading is out of bounds of valid values!";
    	
        byte _puttingOffBackupCpuLoading;
	  
		public int Parallel { get; set; }
		public string LogsFolder { get; set; }
        
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
				other.Parallel == Parallel;

        }
    }
}
