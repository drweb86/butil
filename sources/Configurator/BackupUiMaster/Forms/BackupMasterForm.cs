using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using BUtil.Core.PL;
using BUtil.Core;
using BUtil.Core.Options;
using BUtil.Core.Misc;
using BUtil.Core.Logs;
using BUtil.BackupUiMaster.Controls;
using BUtil.Configurator.Localization;
using BUtil.Core.Events;
using BUtil.Core.TasksTree;
using BUtil.Core.BackupModels;

namespace BUtil.Configurator.BackupUiMaster.Forms
{
	internal delegate void Stub();
	
	internal sealed partial class BackupMasterForm : Form
	{
        private bool _firstTimeApplicationInTray = true;
        private bool _trayModeActivated;
        private bool _backupInProgress;
        private bool _strongIntentionToCancelBackup;
        private readonly BackupTask _backupTask;
		BackupProgressUserControl _backupProgressUserControl;
		private readonly ProgramOptions _programOptions;
        private readonly CancellationTokenSource _cancelTokenSource = new ();

        public BackupMasterForm(ProgramOptions programOptions, BackupTask backupTask)
		{
			InitializeComponent();
			
			if (Program.PackageIsBroken)
			{
				throw new InvalidOperationException("Tried to perform operation that requires package state is ok.");
			}

            _backupTask = backupTask;
			_programOptions = programOptions;
            OnTasksListViewResize(this, new EventArgs());
            ApplyLocalization();
		}

		void OnBackupFinsihed(object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				Invoke(OnBackupFinsihed);
				return;
			}

			_backupInProgress = false;
			cancelButton.Enabled = false;

			if (_backupProgressUserControl != null)
			{
				_backupProgressUserControl.Stop();
			}
			ReturnFromTray();
		}
		
		static Color GetResultColor(ProcessingStatus state)
		{
			return state switch
			{
				ProcessingStatus.FinishedSuccesfully => Color.LightGreen,
				ProcessingStatus.FinishedWithErrors => Color.LightCoral,
				ProcessingStatus.InProgress => Color.Yellow,
				ProcessingStatus.NotStarted => throw new InvalidOperationException(state.ToString()),
				_ => throw new NotImplementedException(state.ToString()),
			};
		}

		private void UpdateListViewItem(Guid taskId, ProcessingStatus status)
		{
            if (InvokeRequired)
            {
                Invoke(UpdateListViewItem, taskId, status);
                return;
            }

            foreach (ListViewItem item in tasksListView.Items)
            {
                if ((Guid)item.Tag == taskId)
                {
                    item.SubItems[1].Text = LocalsHelper.ToString(status);
                    if (status != ProcessingStatus.NotStarted)
                    {
                        item.BackColor = GetResultColor(status);
                    }
					break;
                }
            }
        }

		void CloseButtonClick(object sender, EventArgs e)
		{
		    Close();
		}
		
		void StartButtonClick(object _, EventArgs e)
		{
			// here some logival verification
			/*
			_task.Items.Clear();
			_task.Storages.Clear();
			_task.ExecuteBeforeBackup.Clear();
			_task.ExecuteAfterBackup.Clear();
			
			foreach (ListViewItem item in tasksListView.CheckedItems)
			{
				if (item.Tag is SourceItem)
				{
					_task.Items.Add((SourceItem) item.Tag);
				}
				else if (item.Tag is StorageSettings)
				{
					_task.Storages.Add((StorageSettings) item.Tag);
				}
				else if (item.Tag is ExecuteProgramTaskInfo)
				{
					int groupIndex = tasksListView.Groups.IndexOf(item.Group);
					if (groupIndex == (int)GroupEnum.BeforeBackupChain)
					{
						_task.ExecuteBeforeBackup.Add((ExecuteProgramTaskInfo) item.Tag);
					}
					else if (groupIndex == (int)GroupEnum.AfterBackupChain)
					{
						_task.ExecuteAfterBackup.Add((ExecuteProgramTaskInfo) item.Tag);
					}
					else
					{
						throw new NotImplementedException();
					}
				}
			}
			
			if (!_task.Items.Any())
			{
				Messages.ShowInformationBox(Resources.PleaseCheckItemsToCompress);
				return;
			}
			
			if (_task.Storages.Count < 1)
			{
				Messages.ShowInformationBox(Resources.PleaseCheckStoragesWhereToCopyBackupImage);
				return;
			} */

			settingsUserControl.GetSettingsFromUi(out PowerTask task, out bool beepWhenCompleted);
			// _controller.PowerTask = task;
			// _controller.HearSoundWhenBackupCompleted = beepWhenCompleted;
			
			// performing changes in UI
			_backupProgressUserControl = new BackupProgressUserControl();
			Controls.Add(_backupProgressUserControl);

			_backupProgressUserControl.ApplyLocalization();
			_backupProgressUserControl.Left = settingsUserControl.Left;
			_backupProgressUserControl.Width = settingsUserControl.Width;
			_backupProgressUserControl.Top = settingsUserControl.Bottom - _backupProgressUserControl.MinimumSize.Height;
			_backupProgressUserControl.Height = _backupProgressUserControl.MinimumSize.Height;
			_backupProgressUserControl.Anchor = settingsUserControl.Anchor;
			tasksListView.Height += settingsUserControl.Height - _backupProgressUserControl.Height;
			settingsUserControl.Hide();
			startButton.Enabled = false;
			cancelButton.Enabled = true;
			
			_trayModeActivated = true;
			SwapToTray(true);
			
			// starting machinery
			_backupInProgress = true;
			// _controller.PrepareBackup();

			_backgroundWorker.RunWorkerAsync();
		}

		void ApplyLocalization()
		{
			settingsUserControl.ApplyLocalization();
            toolTip.SetToolTip(startButton, Resources.Start);
            closeButton.Text = Resources.Close;

            taskNameColumnHeader.Text = Resources.Tasks;
            processingStateInformationColumnHeader.Text = Resources.ProcessingState;

            Text = Resources.WellcomeToBackupWizard;
            toolTip.SetToolTip(cancelButton, Resources.Cancel);

            notifyIcon.Text = Resources.BackupIsInProgress;
		}

		void LoadForm(object sender, EventArgs e)
		{
			_log = new FileLog(_programOptions.LogsFolder, _programOptions.LoggingLevel, false);
            _logFile = _log.LogFilename;
			_log.Open();
            _strategy = BackupModelStrategyFactory.Create(_log, _backupTask, _programOptions);
			_backupEvents = new BackupEvents();
			_backupEvents.OnTaskProgress += OnTaskProgress;
            _backupEvents.OnDuringExecutionTasksAdded += OnDuringExecutionTasksAdded;
            _rootTask = _strategy.GetTask(_backupEvents);
            
            settingsUserControl.SetSettingsToUi(_programOptions, PowerTask.None, _backupTask, false, ThreadPriority.BelowNormal);

            var allTasks = _rootTask.GetChildren();
            foreach (var task in allTasks)
			{
				var listItem = new ListViewItem(task.Title, (int)task.TaskArea);
                listItem.SubItems.Add(LocalsHelper.ToString(ProcessingStatus.NotStarted));
                listItem.Tag = task.Id;
                tasksListView.Items.Add(listItem);
            }
			
            if (_backupTask.Items.Count == 0)
            {
            	Messages.ShowInformationBox(Resources.ThereAreNoItemsToBackupNNyouCanSpecifyTheDataToBackupInConfiguratorInWhatSettingsGroup);
            	Close();
            }
            
            if (_backupTask.Storages.Count < 1)
            {
            	Messages.ShowInformationBox(Resources.ThereAreNoSpecifiedPlacesWhereToStoreBackupNNyouCanAddSomeStoragesInConfiguratorInWhereSettingsGroup);
            	Close();
            }
		}

		private void OnDuringExecutionTasksAdded(object sender, DuringExecutionTasksAddedEventArgs e)
		{
			if (InvokeRequired)
			{
				Invoke(OnDuringExecutionTasksAdded, e);
				return;
			}

			tasksListView.BeginUpdate();

			int index = 0;
			foreach (ListViewItem item in tasksListView.Items)
			{
				index++;
				if ((Guid)item.Tag == e.TaskId)
					break;
			}

            foreach (var task in e.Tasks)
            {
                var listItem = new ListViewItem(task.Title, (int)task.TaskArea);
                listItem.SubItems.Add(LocalsHelper.ToString(ProcessingStatus.NotStarted));
                listItem.Tag = task.Id;
                tasksListView.Items.Insert(index, listItem);
				index++;
            }
			tasksListView.EndUpdate();
        }

		private void OnTaskProgress(object sender, TaskProgressEventArgs e)
		{
			UpdateListViewItem(e.TaskId, e.Status);
		}

		private void OnTasksListViewResize(object sender, EventArgs e)
		{
			int newWidth = tasksListView.Width - 35 - processingStateInformationColumnHeader.Width;
			taskNameColumnHeader.Width = newWidth < 35 ? 35 : newWidth;
		}
		
		#region Tray Interaction

		FormWindowState _previousFormState = FormWindowState.Maximized;
		private FileLog _log;
		private object _logFile;
		private IBackupModelStrategy _strategy;
		private BackupEvents _backupEvents;
		private BuTask _rootTask;

		void SwapToTray(bool changeFormState)
		{
			if (_trayModeActivated)
			{
				if (!notifyIcon.Visible)
					notifyIcon.Visible = true;

                if (!ShowInTaskbar)
                {
                    ShowInTaskbar = false;
                }

			    if (changeFormState)
				{
	    	        WindowState = FormWindowState.Minimized;
	    	        
				}
        	    if (_firstTimeApplicationInTray)
            	{
	            	notifyIcon.ShowBalloonTip(5000, Resources.Backup, Resources.WhileBackupIsInProgressYouCanContinueWorkNNtoRestoreProgressFormJustClickOnThisIconInTray, ToolTipIcon.Info);
    	        	_firstTimeApplicationInTray = false;
        	    }
				Hide();        	    
			}
		}
		
		void ReturnFromTray()
		{
			if (_trayModeActivated)
			{
				Show();

				notifyIcon.Visible = false;
				ShowInTaskbar = true;
        	    WindowState = _previousFormState; 
			}
		}
		
		void NotifyIconClick(object sender, EventArgs e)
		{
			ReturnFromTray();
		}
		
		void ResizeForm(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
   	        {
				SwapToTray(false);
   	        } 
			else
			{
				_previousFormState = WindowState;
			}
		}
		#endregion
		
		void OnDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			_rootTask.Execute(_cancelTokenSource.Token);
		}
		
		void OnRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			_backupInProgress = false;
            /*if (_controller.Options.LoggingLevel == LogLevel.Support)
            {
        	SupportManager.OpenWebLink(_fileLogFile);
            }

            if (_controller.PowerTask == PowerTask.None && _controller.ErrorsOrWarningsRegistered)
            {
            	cancelButton.Enabled = false;
            	return;
            }
            */

            /*if (HearSoundWhenBackupCompleted)
                Miscellaneous.DoBeeps();
            
            // in support mode we always opening log and do not perform power task
            if (_options.LoggingLevel == LogLevel.Support)
            {
				SupportManager.OpenWebLinkAsync(_fileLogFile);
            	
				Environment.Exit(0);
            }
            else
            {
				if (ErrorsOrWarningsRegistered)
				{
	                // user chose to shutdown PC or logoff from it. In this case we should
	                // add a registry key in RunOnce section to show him log in browser
	                // of backup when he will login into the system next time
	                if ((PowerTask == PowerTask.Shutdown) || 
	                    (PowerTask == PowerTask.Reboot) || 
	                    (PowerTask == PowerTask.LogOff))
	                {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            NativeMethods.ScheduleOpeningFileAfterLoginOfUserIntoTheSystem(_fileLogFile);
                        }
                        else
{
    _temporaryLog.WriteLine(LoggingEvent.Error, "Cannot schedule opening file after logging user into the system");
}
	                }

                    else
// Hibernate, Sleep, Nothing
// we should open browser and perform required power operation
{
    SupportManager.OpenWebLinkAsync(_fileLogFile);
}
	            }
                // No problems during backup registered. In this case we should notify
                // user that's all is ok
                else
{
    // user is here and we can show him the message
    if (PowerTask == PowerTask.None)
    {
        MessageBox.Show(Resources.BackupProcessCompletedSuccesfully, ";-)", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);
    }
}

PowerPC.DoTask(PowerTask);

if (PowerTask == PowerTask.None && ErrorsOrWarningsRegistered)
{
    ;
}
else
{
    Environment.Exit(0);
}*/

            // TODO: Close();
		}
		
		void CancelButtonClick(object sender, EventArgs e)
		{
			_strongIntentionToCancelBackup = true;
			Close();
		}
		
		void ClosingForm(object sender, FormClosingEventArgs e)
		{
			if (abortBackupBackgroundWorker.IsBusy)
			{
				e.Cancel = true;
				return;
			}
			
            if (_backupInProgress)
			{
				if (e.CloseReason == CloseReason.UserClosing)
				{
					if (_strongIntentionToCancelBackup)
					{
						cancelButton.Enabled = false;
						abortBackupBackgroundWorker.RunWorkerAsync();
					}
					// questioning of user if he is sure he knows what happened if he closes this significant form
					else if (MessageBox.Show(Resources.DoYouReallyWantToStopBackupProcess, Resources.AreYouSure, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, 0 ) == DialogResult.OK)
					{
						cancelButton.Enabled = false;
						abortBackupBackgroundWorker.RunWorkerAsync();
					}
					
					e.Cancel = true;
				}
				else
				{
					if (_backupInProgress)
					{
						e.Cancel = true;
					}
				}
			}
		}
		
		#region Aborting backup operation
		
		void AbortBackupBackgroundWorkerDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			_cancelTokenSource.Cancel();
		}
		
		void AbortBackupBackgroundWorkerRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
            _backupInProgress = false;
			DialogResult = DialogResult.Cancel;
			Close();
		}
		
		#endregion
		
		void HelpButtonClick(object sender, EventArgs e)
		{
            SupportManager.DoSupport(SupportRequest.BackupWizard);
		}
	}
}
