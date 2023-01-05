using System;
using System.Drawing;
using System.Windows.Forms;
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using BUtil.Core.FileSystem;

namespace BUtil.Configurator.BackupUiMaster.Forms
{
	internal sealed partial class BackupMasterForm : Form
	{
        private readonly BackupTask _backupTask;
		BackupProgressUserControl _backupProgressUserControl;
        private readonly ConcurrentQueue<Action> _listViewUpdates = new();
        private readonly List<ListViewItem> _items = new();

        public BackupMasterForm(BackupTask backupTask)
		{
			InitializeComponent();
			
			if (Program.PackageIsBroken)
			{
				throw new InvalidOperationException("Tried to perform operation that requires package state is ok.");
			}

            _backupTask = backupTask;
            OnTasksListViewResize(this, new EventArgs());
            ApplyLocalization();
			NativeMethods.PreventSleep();
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
            foreach (ListViewItem item in _items)
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
            _log = new FileLog(_backupTask.Name);
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
                _items.Add(listItem);
            }
            tasksListView.VirtualListSize = _items.Count;

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
                var error = StorageFactory.Test(_log, storageSettings);
                if (error != null)
                {
                    Messages.ShowErrorBox(error);
                    Close();
                }
            }
        }

        private void OnDuringExecutionTasksAdded(object sender, DuringExecutionTasksAddedEventArgs e)
        {
            _listViewUpdates.Enqueue(() => OnDuringExecutionTasksAddedInternal(sender, e));
        }

        private void OnDuringExecutionTasksAddedInternal(object sender, DuringExecutionTasksAddedEventArgs e)
		{
            int index = 0;
			foreach (var item in _items)
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
                _items.Insert(index, listItem);
				index++;
            }
            tasksListView.VirtualListSize = _items.Count;
        }

		private void OnTaskProgress(object sender, TaskProgressEventArgs e)
		{
            _listViewUpdates.Enqueue(() => UpdateListViewItem(e.TaskId, e.Status));
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
			_rootTask.Execute();
		}
		
		private void OnRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
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
                if (_log.HasErrors)
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
			if (_log.HasErrors &&
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                BUtil.BackupUiMaster.NativeMethods.ScheduleOpeningFileAfterLoginOfUserIntoTheSystem(_log.LogFilename);
			PowerPC.DoTask(powerTask);
            Close();
        }
		
		void CancelButtonClick(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}
		
		void ClosingForm(object sender, FormClosingEventArgs e)
		{
            Environment.Exit(0);
		}
		void HelpButtonClick(object sender, EventArgs e)
		{
            SupportManager.DoSupport(SupportRequest.BackupWizard);
		}

        private void OnListViewFlushUpdates(object sender, EventArgs e)
        {
            tasksListView.BeginUpdate();
            while (_listViewUpdates.TryDequeue(out var action))
                action();
            tasksListView.EndUpdate();
        }

        private void OnRetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex >= 0 && e.ItemIndex < _items.Count)
            {
                e.Item = _items[e.ItemIndex];
            }
        }
    }
}
