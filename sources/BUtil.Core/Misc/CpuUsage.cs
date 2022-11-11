using System;
using System.Diagnostics;
using System.Threading;

namespace BUtil.Core.Misc
{
	public sealed class CpuUsage:IDisposable
	{
		private const int _ASKCPUPERIODSEC = 10;
		private PerformanceCounter _cpuUsagePerformanceCounter;
        private bool _isDisposed; // false;

        public CpuUsage()
		{
            _cpuUsagePerformanceCounter = new PerformanceCounter();
            _cpuUsagePerformanceCounter.CategoryName = "Processor";
            _cpuUsagePerformanceCounter.CounterName = "% Processor Time";
            _cpuUsagePerformanceCounter.InstanceName = "_Total";
		}
		
		/// <summary>
		/// Current CPU activity
		/// </summary>
		/// <returns>CPU activity in %</returns>
		public float CpuActivity
		{
            get
            {
                return _cpuUsagePerformanceCounter.NextValue();
            }
		}
		
		/// <summary>
		/// Returns average CPU activity during a defined period of time
		/// </summary>
        /// <param name="timePeriod">Period if time in seconds. Should be in interval 1..60</param>
		/// <exception cref="ArgumentOutOfRangeException">TimePeriod was NOT in [1..60]</exception>
		/// <returns>CPU Usage in %</returns>
		public float GetNormalCpuUsage(int timePeriod)
		{
            if ((timePeriod < 1) || (timePeriod > 60)) throw new ArgumentOutOfRangeException("timePeriod");
			
			float activity = 0;

            int NormalDegree = timePeriod * 5;
			
			for (int i = 0; i < NormalDegree; i++)
			{
				activity += CpuActivity;
				Thread.Sleep(200);
			}
			
			return (activity/NormalDegree);
		
		}
		
		/// <summary>
		/// Waits until CPUloading decrising to Percents
		/// </summary>
		/// <param name="Percents">CPU loading should be [5..95] %</param>
		/// <exception cref="ArgumentOutOfRangeException">Percents is NOT in [5..95]</exception>
		public void WaitUntilCpuLoadingLess(int percents)
		{
            if ((percents < 5) || (percents > 95)) throw new ArgumentOutOfRangeException("percents");

            while (GetNormalCpuUsage(_ASKCPUPERIODSEC) > percents) ;
		}

        public void Dispose()
        {
            disposeResources();
            GC.SuppressFinalize(this);
        }

        private void disposeResources()
        {
            if (!_isDisposed)
            {
                _cpuUsagePerformanceCounter.Dispose();
                _isDisposed = true;
            }
        }

        ~CpuUsage()
        {
            this.disposeResources();
        }

	}
}
