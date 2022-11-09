using System.Collections.Generic;
using System.Windows.Forms;
using BUtil.Core.Options;

namespace BUtil.Configurator.Controls
{
	/// <summary>
	/// Set of various uncategorized options
	/// </summary>
	internal sealed partial class OtherTaskOptionsUserControl : Core.PL.BackUserControl
	{
		#region Fields
		
		BackupTask _task;
		
		#endregion

        public OtherTaskOptionsUserControl()
		{
			InitializeComponent();
			
			beforeBackupEventsControl.Init(new List<ExecuteProgramTaskInfo>(), true);
			_afterBackupTasksChainToExecuteUserControl.Init(new List<ExecuteProgramTaskInfo>(), false);
		}
		
		#region Overrides
		
		public override void ApplyLocalization() 
		{
			beforeBackupEventsControl.ApplyLocalization();
			_afterBackupTasksChainToExecuteUserControl.ApplyLocalization();

			beforeBackupEventsControl.SetHintToControls(SetHintForControl);
            _afterBackupTasksChainToExecuteUserControl.SetHintToControls(SetHintForControl);
        }
	
		public override void SetOptionsToUi(object settings)
		{
			_task = (BackupTask)(((object[])settings)[0]);
			beforeBackupEventsControl.Init(_task.ExecuteBeforeBackup, true);
			_afterBackupTasksChainToExecuteUserControl.Init(_task.ExecuteAfterBackup, false);
		}

		public override void GetOptionsFromUi()
		{
			_task.ExecuteBeforeBackup = beforeBackupEventsControl.GetResultChainOfTasks();
			_task.ExecuteAfterBackup = _afterBackupTasksChainToExecuteUserControl.GetResultChainOfTasks();
		}

		#endregion
	}
}
