using System;
using System.Threading;
using System.Collections.Generic;

namespace BUtil.Core.Misc
{
	/// <summary>
	/// This class makes possible parralel executing of some tasks
	/// </summary>
	internal sealed class TaskManager
	{
		#region Constants

		const int _DelayInTaskThreadBetweanChecksMsec = 100;
		const int _DelayWaitForFinishMsec = 100;
		
		#endregion
		
		#region Private Fields
		
		readonly string _maxThreadsOutOfRange =
			"Parameter 'MaxThreads' should be a positive number";

		readonly List<IJob> _poolOfInactuveJobs = new List<IJob>();
		readonly List<Thread> _poolOfThreads = new List<Thread>();
		readonly object _syncJobs = new object();
		readonly object _syncTaskThreadRequests = new object();
		readonly int _concurrentThreadsAmount;
		readonly Thread _taskThread;
		bool _abortTasks;
		
		#endregion

		#region Conctructor and Destructor
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="concurrentThreadsAmount">Amount of concurrent threads</param>
		/// <exception cref="ArgumentOutOfRangeException">concurrentThreadsAmount less than 1</exception>
		public TaskManager(int concurrentThreadsAmount)
		{
			if (concurrentThreadsAmount < 1)
				throw new ArgumentOutOfRangeException("concurrentThreadsAmount", _maxThreadsOutOfRange);

			_concurrentThreadsAmount = concurrentThreadsAmount;
			_taskThread = new Thread(jobRunner);
			_taskThread.IsBackground = true;
			_taskThread.Start();
		}
		
		~TaskManager()
		{
			Close();
		}

		#endregion

		#region Public Methods
		
		/// <summary>
		/// Adds job to the job's pool
		/// </summary>
		/// <param name="job"></param>
		public void AddJob(IJob job)
		{
			if (job != null)
			{
				lock (_syncJobs)
				{
					job.TaskFinished += new EventHandler<JobThreadEventArgs>(onFinished);
					_poolOfInactuveJobs.Add(job);
				}
			}
		}

		/// <summary>
		/// Aborts executing
		/// </summary>
		public void Abort()
		{
			_abortTasks = true;
			lock(_syncTaskThreadRequests) // waiting for termination of task thread
			{
				;
			}

			lock(_syncJobs)
			{
				_poolOfInactuveJobs.Clear();				
				
				foreach (Thread thread in _poolOfThreads)
				{
					thread.Interrupt();

					while (thread.IsAlive)
					{
						Thread.Sleep(100);
					}
				}
				_poolOfThreads.Clear();
			}
			
			GC.SuppressFinalize(this);
		}
		
		/// <summary>
		/// Frees resources
		/// </summary>
		public void Close()
		{
			Abort();
		}

		/// <summary>
		/// Waits untill all tasks are finished.
		/// This function should be called only after all tasks have been added.
		/// </summary>
		public void WaitUntilAllJobsAreDone()
		{
			while(true)
			{
				Thread.Sleep(_DelayWaitForFinishMsec);
				lock (_syncJobs)
				{
					if (_poolOfThreads.Count == 0 && _poolOfInactuveJobs.Count == 0)
					{
						return;
					}
				}
			}
		}
		
		#endregion
		
		#region Private Methods
		
		void jobRunner()
		{
			lock(_syncTaskThreadRequests)
			{
				while (!_abortTasks)
				{
					lock (_syncJobs)
					{
						if ((_poolOfThreads.Count < _concurrentThreadsAmount) && _poolOfInactuveJobs.Count > 0)
						{
							Thread thread = new Thread(_poolOfInactuveJobs[0].DoJob);
							thread.IsBackground = true;
							_poolOfInactuveJobs.RemoveAt(0);
							_poolOfThreads.Add(thread);
							thread.Start();
						}
					}
	
					Thread.Sleep(_DelayInTaskThreadBetweanChecksMsec);
				}
			}
		}
		
		void onFinished(object sender, JobThreadEventArgs e)
		{
			lock (_syncJobs)
			{
				_poolOfThreads.Remove(e.ExecutingThread);
			}
		}

		#endregion
	}
}
