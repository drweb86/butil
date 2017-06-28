using System.Collections.Generic;
using BUtil.Core.Options;

namespace BUtil.Configurator.Controls
{
	/// <summary>
	/// Set of various uncategorized options
	/// </summary>
	internal sealed partial class TaskOtherOptionsUserControl : Core.PL.BackUserControl
	{
		#region Fields
		
		BackupTask _task;
		
		#endregion

        public TaskOtherOptionsUserControl()
		{
			InitializeComponent();
			
			beforeBackupEventsControl.Init(new List<BackupEventTaskInfo>(), true);
			_afterBackupTasksChainToExecuteUserControl.Init(new List<BackupEventTaskInfo>(), false);
		}
		
		#region Overrides
		
		public override void ApplyLocalization() 
		{
			beforeBackupEventsControl.ApplyLocalization();
			_afterBackupTasksChainToExecuteUserControl.ApplyLocalization();
		}
	
		public override void SetOptionsToUi(object settings)
		{
			_task = (BackupTask)(((object[])settings)[0]);
			beforeBackupEventsControl.Init(_task.BeforeBackupTasksChain, true);
			_afterBackupTasksChainToExecuteUserControl.Init(_task.AfterBackupTasksChain, false);
		}

		public override void GetOptionsFromUi()
		{
			_task.BeforeBackupTasksChain = beforeBackupEventsControl.GetResultChainOfTasks();
			_task.AfterBackupTasksChain = _afterBackupTasksChainToExecuteUserControl.GetResultChainOfTasks();
		}

		#endregion
	}
}
