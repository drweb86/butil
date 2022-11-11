using System;
using System.Threading;

namespace BUtil.Core.Misc
{
	/// <summary>
	/// This event argument is for notification from the thread that is used in TaskManager
	/// </summary>
	internal class JobThreadEventArgs: EventArgs
	{
		readonly Thread _executingThread;

		/// <summary>
		/// The thread object executing of which was terminated
		/// </summary>
		public Thread ExecutingThread
		{
			get { return _executingThread; }
		}

		/// <summary>
		/// The constructor
		/// </summary>
		/// <param name="executingThread">The current working thread</param>
		public JobThreadEventArgs(Thread executingThread)
		{
			_executingThread = executingThread;
		} 
	}
}
