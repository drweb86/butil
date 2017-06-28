using System;

namespace BUtil.Core.Misc
{
	internal interface IJob
	{
		void DoJob();

		/// <summary>
		/// Should occur when exiting from task
		/// </summary>
		EventHandler<JobThreadEventArgs> TaskFinished
		{
			get;
			set;
		}
	}
}
