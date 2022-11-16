using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;

using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;
using BUtil.Core.Options;
using System.Linq;
using BUtil.Core.FileSystem;
using System.IO;

namespace BUtil.Configurator.LogsManagement
{
	public partial class LogsListUserControl : UserControl
	{
		private const string ViewLogTime = "dd MMMM (dddd) HH:mm";
		
		private ProgramOptions _programOptions;
		
		public LogsListUserControl()
		{
			InitializeComponent();
			
			listViewResize(this, null);
			
			
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

		public void SetSettings(ProgramOptions options)
		{
            _programOptions = options;

            RefreshList();

		}
		
		private void RefreshList()
		{
            var colors = new[] { Color.Green, Color.Red, Color.Brown };
            journalsListView.BeginUpdate();
            journalsListView.Items.Clear();
            
			GetLogsInformation()
				.OrderByDescending(x => x.CreatedAt)
				.Select(log => new ListViewItem(log.CreatedAt.ToString(ViewLogTime, CultureInfo.CurrentUICulture), (int)log.Status)
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
				
			updateLocalOperationsButtonsState(this, null);
		}
		
		void listViewResize(object sender, EventArgs e)
		{
			_logsColumnHeader.Width = journalsListView.Width - 40;
		}
		
		void refresh(object sender, EventArgs e)
		{
			RefreshList();
		}

		void removeSelected(object sender, EventArgs e)
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
		
		void journalsListViewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				if (journalsListView.SelectedItems.Count > 0)
				{
					removeSelected(sender, e);
				}
			} 
			else if (e.KeyCode == Keys.Enter)
			{
				openSelectedLogsInBrowser(sender, e);
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
		
		void updateLocalOperationsButtonsState(object sender, EventArgs e)
		{
			bool enable = journalsListView.SelectedItems.Count > 0;
			
            deleteSelectedLogsButton.Enabled =
				removeSelectedLogsToolStripMenuItem.Enabled =
				
				viewSelectedLogsToolStripMenuItem.Enabled =
                openSelectedLogsButton.Enabled = enable;
		}

		void removeSuccessfullLogs(object sender, EventArgs e)
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
		
		void openSelectedLogsInBrowser(object sender, EventArgs e)
		{
			foreach(ListViewItem item in journalsListView.SelectedItems)
			{
                ProcessHelper.ShellExecute(((LogInfo)item.Tag).File);
			}
		}

        private List<LogInfo> GetLogsInformation()
        {
            var result = new List<LogInfo>();

            if (!Directory.Exists(_programOptions.LogsFolder))
            {
                return result;
            }

            var logsList = Directory.GetFiles(_programOptions.LogsFolder, "*" + Files.LogFilesExtension);

            foreach (var log in logsList)
            {
                result.Add(new LogInfo(log));
            }

            return result;
        }
    }
}
