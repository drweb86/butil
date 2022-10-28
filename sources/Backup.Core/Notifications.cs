using System;
using BUtil.Core.Options;
using BUtil.Core.Storages;

namespace BUtil.Core
{
	/// <summary>
	/// Event argument that is posted by CompressionJob class
	/// </summary>
	public sealed class PackingNotificationEventArgs: EventArgs
	{
		readonly ProcessingState _state;
		readonly SourceItem _item;

		public SourceItem Item
		{
			get { return _item; }
		}
		
		public ProcessingState State
		{
			get { return _state; }
		}
		
		public PackingNotificationEventArgs(SourceItem item, ProcessingState state)
			:base()
		{
			_item = item;
			_state = state;
		}
	}
	
	/// <summary>
	/// Event argument that is posted by ImageCreationJob class
	/// </summary>
	public sealed class ImagePackingNotificationEventArgs: EventArgs
	{
		readonly ProcessingState _state;

		public ProcessingState State
		{
			get { return _state; }
		}
		
		public ImagePackingNotificationEventArgs(ProcessingState state)
			:base()
		{
			_state = state;
		}
	}
	
	/// <summary>
	/// Event argument that is posted by anchestors of StorageBase class
	/// </summary>
	public sealed class CopyingToStorageNotificationEventArgs: EventArgs
	{
		readonly ProcessingState _state;
		readonly string _storageName;

		public string StorageName
		{
			get { return _storageName; }
		}
		
		public ProcessingState State
		{
			get { return _state; }
		}
		
		public CopyingToStorageNotificationEventArgs(string storageName, ProcessingState state)
			:base()
		{
			_storageName = storageName;
			_state = state;
		}
	}
	
	/// <summary>
	/// Event argument that is posted by examples of BeforeBackupTask
	/// </summary>
	public sealed class RunProgramBeforeOrAfterBackupEventArgs: EventArgs
	{
		#region Private Fields
		
		readonly ProcessingState _state;
		readonly ExecuteProgramTaskInfo _taskInfo;

		#endregion
		
		#region Public Properties
		
		/// <summary>
		/// The task
		/// </summary>
		public ExecuteProgramTaskInfo TaskInfo
		{
			get { return _taskInfo; }
		}
		
		/// <summary>
		/// The State of processing
		/// </summary>
		public ProcessingState State
		{
			get { return _state; }
		}
		
		#endregion
		
		#region Contructors
		
		/// <summary>
		/// The Constructor
		/// </summary>
		/// <param name="taskInfo">The task is served now</param>
		/// <param name="state">The state of an operation</param>
		/// <exception cref="ArgumentNullException">taskInfo is null</exception>
		public RunProgramBeforeOrAfterBackupEventArgs(ExecuteProgramTaskInfo taskInfo, ProcessingState state)
			:base()
		{
			if (taskInfo == null)
			{
				throw new ArgumentNullException("taskInfo");
			}

			_taskInfo = taskInfo;
			_state = state;
		}
		
		#endregion
	}
	
	
	/// <summary>
	/// Processing state for tasks
	/// </summary>
	public enum ProcessingState
	{
		NotStarted,
		InProgress,
		FinishedSuccesfully,
		FinishedWithErrors
	}
}
