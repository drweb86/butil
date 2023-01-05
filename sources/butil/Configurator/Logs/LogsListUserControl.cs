using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;
using System.Linq;
using BUtil.Core.FileSystem;
using System.IO;

namespace BUtil.Configurator.LogsManagement
{
	public partial class LogsListUserControl : UserControl
	{
		public LogsListUserControl()
		{
			InitializeComponent();
			
			OnListViewResize(this, null);
			
			
            toolTip.SetToolTip(refreshLogsButton, Resources.Refresh);
            toolTip.SetToolTip(openSelectedLogsButton, Resources.ViewSelectedLogs);

		    toolTip.SetToolTip(deleteSelectedLogsButton, Resources.RemoveSelectedLogs);
			removeSelectedLogsToolStripMenuItem.Text = 
				removeSelectedLogsToolStripMenuItem.ToolTipText = 
				Resources.RemoveSelectedLogs;

            toolTip.SetToolTip(removeSuccesfullLogsButton, Resources.RemoveSuccesfullLogs);
			journalsListView.Columns[0].Text = Resources.BackupJournals;
			viewSelectedLogsToolStripMenuItem.Text = 
				Resources.ViewSelectedLogs;
		}

		public void SetSettings()
		{
            RefreshList();
		}
		
		private void RefreshList()
		{
            var colors = new[] { Color.Green, Color.Red, Color.Brown };
            journalsListView.BeginUpdate();
            journalsListView.Items.Clear();
            
			GetLogsInformation()
				.OrderByDescending(x => x.CreatedAt)
				.Select(log => new ListViewItem(Path.GetFileName(
					log.File.Replace(Files.LogFilesExtension, string.Empty)),
					(int)log.Status)
                {
                    ForeColor = colors[(int)log.Status],
                    Tag = log
                })
				.ToList()
				.ForEach(x => journalsListView.Items.Add(x));

			journalsListView.EndUpdate();
			RefreshEnabledButtons();
		}
		
		private void RefreshEnabledButtons()
		{
			bool anyDataPresent = journalsListView.Items.Count > 0;
			
			journalsListView.Enabled =
                removeSuccesfullLogsButton.Enabled = anyDataPresent;
				
			UpdateLocalOperationsButtonsState(this, null);
		}

        private void OnListViewResize(object sender, EventArgs e)
		{
			_logsColumnHeader.Width = journalsListView.Width - 40;
		}
		
		private void Refresh(object sender, EventArgs e)
		{
			RefreshList();
		}

        private void RemoveSelected(object sender, EventArgs e)
		{
			ListView.SelectedListViewItemCollection items = journalsListView.SelectedItems;
			var infos = new List<LogInfo>();
			foreach (ListViewItem item in items)
			{
				infos.Add((LogInfo)item.Tag);
			}

			if (!Messages.ShowYesNoDialog(string.Format(Resources.PleaseConfirmDeletionOf0Logs, infos.Count)))
				return;

            foreach (var info in infos)
                System.IO.File.Delete(info.File);

            journalsListView.BeginUpdate();
			foreach (ListViewItem item in items)
			{
				journalsListView.Items.Remove(item);
			}
			journalsListView.EndUpdate();

			RefreshEnabledButtons();
		}

        private void JournalsListViewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				if (journalsListView.SelectedItems.Count > 0)
				{
					RemoveSelected(sender, e);
				}
			} 
			else if (e.KeyCode == Keys.Enter)
			{
				OpenSelectedLogsInBrowser(sender, e);
			}
			else if (e.Control && e.KeyCode == Keys.A)
			{
				foreach(ListViewItem item in journalsListView.Items)
				{
					item.Selected = true;
				}
			}
			else if (e.KeyCode == Keys.F5)
			{
				RefreshList();
			}
		}

        private void UpdateLocalOperationsButtonsState(object sender, EventArgs e)
		{
			bool enable = journalsListView.SelectedItems.Count > 0;
			
            deleteSelectedLogsButton.Enabled =
				removeSelectedLogsToolStripMenuItem.Enabled =
				
				viewSelectedLogsToolStripMenuItem.Enabled =
                openSelectedLogsButton.Enabled = enable;
		}

        private void RemoveSuccessfullLogs(object sender, EventArgs e)
		{
			ListView.ListViewItemCollection items = journalsListView.Items;

			var infos = new List<LogInfo>();
			foreach (ListViewItem item in items)
			{
				var info = (LogInfo)item.Tag;
				if (info.Status == BackupResult.Successfull)
				{
					infos.Add((info));
				}
			}

			if (!Messages.ShowYesNoDialog(string.Format(Resources.PleaseConfirmDeletionOf0Logs, infos.Count)))
				return;
            foreach (var info in infos)
                System.IO.File.Delete(info.File);
            journalsListView.BeginUpdate();
			foreach (ListViewItem item in items)
			{
				var info = (LogInfo)item.Tag;
				if (info.Status == BackupResult.Successfull)
				{
					journalsListView.Items.Remove(item);
				}
			}
			journalsListView.EndUpdate();

			RefreshEnabledButtons();

		}
		
		private void OpenSelectedLogsInBrowser(object sender, EventArgs e)
		{
			foreach(ListViewItem item in journalsListView.SelectedItems)
			{
                ProcessHelper.ShellExecute(((LogInfo)item.Tag).File);
			}
		}

        private static List<LogInfo> GetLogsInformation()
        {
            var result = new List<LogInfo>();

            if (!Directory.Exists(Directories.LogsFolder))
            {
                return result;
            }

            return Directory
				.GetFiles(Directories.LogsFolder, "*" + Files.LogFilesExtension)
				.Select(x => new LogInfo(x))
				.ToList();
        }
    }
}
