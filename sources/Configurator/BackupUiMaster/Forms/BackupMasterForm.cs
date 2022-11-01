using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

using BUtil.Core.PL;
using BUtil.Core;
using BUtil.Core.Options;
using BUtil.Core.Misc;
using BUtil.Core.Logs;
using BUtil.BackupUiMaster.Controls;
using BUtil.Core.Storages;
using BUtil.Configurator.Localization;
using System.Linq;
using BUtil.Core.Events;
using System.IO;

namespace BUtil.Configurator.BackupUiMaster.Forms
{
	internal delegate void Stub();
	
	internal sealed partial class BackupMasterForm : Form
	{
		#region Types
		
		enum GroupEnum
		{
			CompressionOfFolders = 0,
			CompressionOfFiles = 1,
			Storages = 2,
			OtherTasks = 3,
			BeforeBackupChain = 4,
			AfterBackupChain = 5
		}
		
		enum ImagesEnum
		{
			Folder = 0,
			File = 1,
			Ftp = 2,
			Hdd = 3,
			Network = 4,
			CompressIntoAnImage = 5,
			ProgramInRunBeforeAfterBackupChain = 6
		}
		
		#endregion
		
		#region Fields
		
		bool _firstTimeApplicationInTray = true;
		bool _trayModeActivated;
		bool _backupInProgress;
		bool _strongIntentionToCancelBackup;
		readonly BackupUiMaster _controller;
		readonly BackupTask _task;
		BackupProgressUserControl _backupProgressUserControl;
		
		#endregion

        public BackupMasterForm(ProgramOptions options, BackupTask task)
		{
			InitializeComponent();
			
			if (Program.PackageIsBroken)
			{
				throw new InvalidOperationException("Tried to perform operation that requires package state is ok.");
			}

			tasksListView.Columns[0].Width = tasksListView.Width - 40;
			processingStateInformationColumnHeader.Width = 0;

            //TODO: please move to controller
            if (task == null)
            {
                var backupTaskStoreService = new BackupTaskStoreService();
				var backupTasks = backupTaskStoreService.LoadAll();

				using var form = new SelectTaskToRunForm(backupTasks.ToDictionary(t => t.Name, t => t));
				if (form.ShowDialog() == DialogResult.OK)
				{
					task = form.TaskToRun;
				}
				else
				{
					Environment.Exit(-1);
				}
			}

            _task = task;
            _controller = new BackupUiMaster(_task, options);
            _controller.BackupFinished += OnBackupFinsihed;
            CompressionItemsListViewResize(null, null);
            
            ApplyLocalization();
		}
		
		void OnBackupFinsihed()
		{
			if (!InvokeRequired)
			{
				_backupInProgress = false;
				cancelButton.Enabled = false; 
				
				if (_backupProgressUserControl != null)
				{
					_backupProgressUserControl.Stop();
				}
				ReturnFromTray();
			}
			else
			{
				Invoke(new Stub(OnBackupFinsihed));
			}
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

		private void OnSourceItemStatusUpdate(object sender, SourceItemStatusEventArgs e)
		{
			UpdateListViewItem(e.Item, e.Status);
		}

		private void UpdateListViewItem(object tag, ProcessingStatus status)
		{
            if (InvokeRequired)
            {
                Invoke(UpdateListViewItem, new[] { tag, status });
                return;
            }

            foreach (ListViewItem item in tasksListView.Items)
            {
                if (item.Tag == tag)
                {
                    item.SubItems[2].Text = LocalsHelper.ToString(status);
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
			}

			// applying settings
			tasksListView.CheckBoxes = false;


			

			processingStateInformationColumnHeader.Width = 154;
			CompressionItemsListViewResize(null, null);

			settingsUserControl.GetSettingsFromUi(out PowerTask task, out bool beepWhenCompleted);
			_controller.PowerTask = task;
			_controller.HearSoundWhenBackupCompleted = beepWhenCompleted;
			
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
			_controller.PrepareBackup();

			foreach (var customTag in _controller.BackupClass.CustomJobs)
			{
				var listItem = new ListViewItem(customTag.Value)
				{
					ImageIndex = (int)ImagesEnum.CompressIntoAnImage
				};
				listItem.SubItems.Add("-");
				listItem.SubItems.Add(string.Empty);
				listItem.Group = tasksListView.Groups[(int)GroupEnum.OtherTasks];
				listItem.Tag = customTag.Key;
				listItem.Checked = true;

				tasksListView.Items.Add(listItem);
			}

            foreach (ListViewItem item in tasksListView.Items)
            {
                if (!item.Checked)
                {
                    item.BackColor = Color.Gray;
                    item.SubItems[2].Text = Resources.Disabled;
                }
            }

            _controller.BackupClass.Events.OnSourceItemStatusUpdate += OnSourceItemStatusUpdate;
            _controller.BackupClass.Events.OnStorageStatusUpdate += OnStorageStatusUpdate;
            _controller.BackupClass.Events.OnCustomUpdate += OnCustomUpdate;
            _controller.BackupClass.Events.OnExecuteProgramStatusUpdate += OnExecuteProgramStatusUpdate;
			backupBackgroundWorker.RunWorkerAsync();
		}

		private void OnExecuteProgramStatusUpdate(object sender, ExecuteProgramStatusEventArgs e)
		{
            UpdateListViewItem(e.TaskInfo, e.Status);
        }

		private void OnCustomUpdate(object sender, CustomEventArgs e)
		{
            UpdateListViewItem(e.Tag, e.Status);
        }

		private void OnStorageStatusUpdate(object sender, StorageStatusEventArgs e)
		{
            UpdateListViewItem(e.Settings, e.Status);
        }

		void ApplyLocalization()
		{
			settingsUserControl.ApplyLocalization();
            toolTip.SetToolTip(startButton, Resources.Start);
            closeButton.Text = Resources.Close;

            taskNameColumnHeader.Text = Resources.Tasks;
            informationAboutTaskColumnHeader.Text = Resources.Information;
            processingStateInformationColumnHeader.Text = Resources.ProcessingState;

            Text = Resources.WellcomeToBackupWizard;
            toolTip.SetToolTip(cancelButton, Resources.Cancel);
            tasksListView.Groups[0].Header = Resources.PackingFolders;
            tasksListView.Groups[1].Header = Resources.PackingFiles;
            tasksListView.Groups[2].Header = Resources.CopyingToStorages;
            tasksListView.Groups[3].Header = Resources.OtherTasks;
            tasksListView.Groups[(int)GroupEnum.BeforeBackupChain].Header = Resources.ChainOfProgramsToExecuteBeforeBackup; // before backup event chain
            tasksListView.Groups[5].Header = Resources.ChainOfProgramsToExecuteAfterBackup;

            notifyIcon.Text = Resources.BackupIsInProgress;
		}
		
		void LoadForm(object sender, EventArgs e)
		{
			// displaying the current settings

            settingsUserControl.SetSettingsToUi(_controller.Options, PowerTask.None, _task, false, ThreadPriority.BelowNormal);

			tasksListView.BeginUpdate();
            ReadOnlyCollection<SourceItem> items = _controller.ListOfFiles;
            foreach(SourceItem item in items)
            {
            	var listItem = new ListViewItem(item.Target, item.IsFolder ? (int)ImagesEnum.Folder : (int)ImagesEnum.File);
            	listItem.SubItems.Add(LocalsHelper.ToString(item.CompressionDegree));
            	listItem.SubItems.Add(string.Empty);
            	listItem.Group = item.IsFolder ? tasksListView.Groups[(int)GroupEnum.CompressionOfFolders] : tasksListView.Groups[(int)GroupEnum.CompressionOfFiles];
            	listItem.Tag = item;
            	listItem.Checked = true;
            	tasksListView.Items.Add(listItem);
            }
            
            foreach(var storageSettings in _task.Storages)
            {
				var listItem = new ListViewItem(storageSettings.Name)
				{
					ImageIndex = storageSettings.ProviderName switch
					{
						StorageProviderNames.Hdd => (int)ImagesEnum.Hdd,
						StorageProviderNames.Ftp => (int)ImagesEnum.Ftp,
						StorageProviderNames.Samba => (int)ImagesEnum.Network,
						_ => throw new InvalidDataException(nameof(storageSettings.ProviderName)),
					}
				};
				listItem.SubItems.Add(String.Empty);
            	listItem.SubItems.Add(string.Empty);
            	listItem.Group = tasksListView.Groups[(int)GroupEnum.Storages];
            	listItem.Tag = storageSettings;
            	listItem.Checked = true;
            	tasksListView.Items.Add(listItem);
            }

            foreach (ExecuteProgramTaskInfo taskInfo in _task.ExecuteBeforeBackup)
            {
				var listItem = new ListViewItem(taskInfo.Program)
				{
					ImageIndex = (int)ImagesEnum.ProgramInRunBeforeAfterBackupChain
				};

				listItem.SubItems.Add(taskInfo.Arguments);
            	listItem.SubItems.Add(string.Empty);
            	listItem.Group = tasksListView.Groups[(int) GroupEnum.BeforeBackupChain];
            	listItem.Tag = taskInfo;
            	listItem.Checked = true;
            	tasksListView.Items.Add(listItem);
            }
            
            foreach (ExecuteProgramTaskInfo taskInfo in _task.ExecuteAfterBackup)
            {
				var listItem = new ListViewItem(taskInfo.Program)
				{
					ImageIndex = (int)ImagesEnum.ProgramInRunBeforeAfterBackupChain
				};

				listItem.SubItems.Add(taskInfo.Arguments);
            	listItem.SubItems.Add(string.Empty);
            	listItem.Group = tasksListView.Groups[(int) GroupEnum.AfterBackupChain];
            	listItem.Tag = taskInfo;
            	listItem.Checked = true;
            	tasksListView.Items.Add(listItem);
            }
            
            tasksListView.EndUpdate();

            // logical verifying of settings
            // If they are not correct, just closing the Master
            if (items.Count == 0)
            {
            	Messages.ShowInformationBox(Resources.ThereAreNoItemsToBackupNNyouCanSpecifyTheDataToBackupInConfiguratorInWhatSettingsGroup);
            	Close();
            }
            
            if (_task.Storages.Count < 1)
            {
            	Messages.ShowInformationBox(Resources.ThereAreNoSpecifiedPlacesWhereToStoreBackupNNyouCanAddSomeStoragesInConfiguratorInWhereSettingsGroup);
            	Close();
            }
		}
	
		void CompressionItemsListViewResize(object sender, EventArgs e)
		{
			taskNameColumnHeader.Width = 180;
			
			int newWidth = tasksListView.Width - 35 - informationAboutTaskColumnHeader.Width - processingStateInformationColumnHeader.Width;
			taskNameColumnHeader.Width = newWidth < 35 ? 35 : newWidth;
		}
		
		#region Tray Interaction

		FormWindowState _previousFormState = FormWindowState.Maximized;
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
		
		void BackupBackgroundWorkerDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			_controller.BackupClass.Run();
		}
		
		void BackupBackgroundWorkerRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			_backupInProgress = false;
            if (_controller.Options.LoggingLevel == LogLevel.Support)
            {
				_controller.OpenLogFileInBrowser();
            }

            if (_controller.PowerTask == PowerTask.None && _controller.ErrorsOrWarningsRegistered)
            {
            	cancelButton.Enabled = false;
            	return;
            }
            
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
			_controller.Abort();
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
