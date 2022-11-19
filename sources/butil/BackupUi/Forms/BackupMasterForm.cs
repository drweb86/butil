using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using BUtil.Core;
using BUtil.Core.Options;
using BUtil.Core.Misc;
using BUtil.Core.Logs;
using BUtil.BackupUiMaster.Controls;
using BUtil.Configurator.Localization;
using BUtil.Core.Events;
using BUtil.Core.BackupModels;
using BUtil.Core.TasksTree.Core;
using System.Runtime.InteropServices;
using BUtil.Core.Storages;
using System.IO;
using System.Linq;

namespace BUtil.Configurator.BackupUiMaster.Forms
{
    internal delegate void Stub();
	
	internal sealed partial class BackupMasterForm : Form
	{
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
			
			_backupInProgress = true;
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
		}

		void LoadForm(object sender, EventArgs e)
        {
            _log = new FileLog(_programOptions.LogsFolder, false);
            _log.Open();
            _strategy = BackupModelStrategyFactory.Create(_log, _backupTask);
            _backupEvents = new BackupEvents();
            _backupEvents.OnTaskProgress += OnTaskProgress;
            _backupEvents.OnDuringExecutionTasksAdded += OnDuringExecutionTasksAdded;
            _rootTask = _strategy.GetTask(_backupEvents);

            settingsUserControl.SetSettingsToUi(PowerTask.None, _backupTask, false);

            var allTasks = _rootTask.GetChildren();
            foreach (var task in allTasks)
            {
                var listItem = new ListViewItem(task.Title, (int)task.TaskArea);
                listItem.SubItems.Add(LocalsHelper.ToString(ProcessingStatus.NotStarted));
                listItem.Tag = task.Id;
                tasksListView.Items.Add(listItem);
            }

            VerifyStorages();
            VerifySourceItems();
        }

        private void VerifySourceItems()
        {
            if (_backupTask.Items.Count == 0)
            {
                Messages.ShowInformationBox(Resources.ThereAreNoItemsToBackupNNyouCanSpecifyTheDataToBackupInConfiguratorInWhatSettingsGroup);
                Close();
            }

            foreach (var item in _backupTask.Items)
            {
                if (item.IsFolder)
                {
                    if (!Directory.Exists(item.Target))
                    {
                        Messages.ShowErrorBox(string.Format(BUtil.Configurator.Localization.Resources.SourceItemFailure, item.Target));
                        Close();
                    }
                }
                else
                {
                    if (!File.Exists(item.Target))
                    {
                        Messages.ShowErrorBox(string.Format(BUtil.Configurator.Localization.Resources.SourceItemFailure, item.Target));
                        Close();
                    }
                }
            }
        }

        private void VerifyStorages()
        {
            var enabledStorages = _backupTask.Storages
                            .Where(x => x.Enabled)
                            .ToList();

            if (enabledStorages.Count < 1)
            {
                Messages.ShowInformationBox(Resources.ThereAreNoSpecifiedPlacesWhereToStoreBackupNNyouCanAddSomeStoragesInConfiguratorInWhereSettingsGroup);
                Close();
            }

            foreach (var storageSettings in enabledStorages)
            {
                var storage = StorageFactory.Create(_log, storageSettings);
                var error = storage.Test();
                if (error != null)
                {
                    Messages.ShowErrorBox(error);
                    Close();
                }
            }
        }

        private void OnDuringExecutionTasksAdded(object sender, DuringExecutionTasksAddedEventArgs e)
		{
			if (InvokeRequired)
			{
				Invoke(OnDuringExecutionTasksAdded, sender, e);
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
			int newWidth = tasksListView.Width - processingStateInformationColumnHeader.Width - 35;
			taskNameColumnHeader.Width = newWidth < 35 ? 35 : newWidth;
		}
		
		private FileLog _log;
		private IBackupModelStrategy _strategy;
		private BackupEvents _backupEvents;
		private BuTask _rootTask;

        private void OnDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			_rootTask.Execute(_cancelTokenSource.Token);
		}
		
		private void OnRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
            _backupInProgress = false;
            cancelButton.Enabled = false;
            _backupProgressUserControl.Stop();
			_log.Close();

            settingsUserControl.GetSettingsFromUi(out PowerTask powerTask, out bool beepWhenCompleted);
            if (beepWhenCompleted)
                Miscellaneous.DoBeeps();

			var appStaysAlive = powerTask == PowerTask.None;

            if (appStaysAlive)
			{
                PowerPC.DoTask(powerTask);
                if (_log.ErrorsOrWarningsRegistered)
				{
					ProcessHelper.ShellExecute(_log.LogFilename);
                    Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.BackupFailedPleaseReviewOpenedLog);
                }
				else
				{
                    MessageBox.Show(Resources.BackupProcessCompletedSuccesfully, ";-)", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);
                }
				return;
            }
			if (_log.ErrorsOrWarningsRegistered &&
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                BUtil.BackupUiMaster.NativeMethods.ScheduleOpeningFileAfterLoginOfUserIntoTheSystem(_log.LogFilename);
			PowerPC.DoTask(powerTask);
            Close();
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
