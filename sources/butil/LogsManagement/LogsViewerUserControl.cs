using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;

using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;
using BUtil.Core.Options;

namespace BUtil.Configurator.LogsManagement
{
	public partial class LogsViewerUserControl : UserControl
	{
		private const string ViewLogTime = "dd MMMM (dddd) HH:mm";
		
		private LogOperations _controller;
		
		public LogsViewerUserControl()
		{
			InitializeComponent();
			
			listViewResize(this, null);
			
			
            toolTip.SetToolTip(refreshLogsButton, Resources.Refresh);
            toolTip.SetToolTip(openRecentLogButton, Resources.ReviewLogOfLastBackup);
            toolTip.SetToolTip(openSelectedLogsButton, Resources.ViewSelectedLogs);
            toolTip.SetToolTip(helpButton, Resources.Help);

		    toolTip.SetToolTip(deleteSelectedLogsButton, Resources.RemoveSelectedLogs);
			removeSelectedLogsToolStripMenuItem.Text = 
				removeSelectedLogsToolStripMenuItem.ToolTipText = 
				Resources.RemoveSelectedLogs;

            toolTip.SetToolTip(removeSuccesfullLogsButton, Resources.RemoveSuccesfullLogs);
			
			journalsListView.Columns[0].Text = Resources.BackupJournals;
			journalsListView.Groups[(int) BackupResult.Successfull].Header = Resources.Succesfull;
            journalsListView.Groups[(int) BackupResult.Erroneous].Header = Resources.Erroneous;
            journalsListView.Groups[(int) BackupResult.Unknown].Header = Resources.Unknown;

            
			viewSelectedLogsToolStripMenuItem.Text = 
				Resources.ViewSelectedLogs;
			
		}
		

		public void SetSettings(ProgramOptions options)
		{
			_controller = new LogOperations(options);

            dataBind();

		}
		
		#region Private methods

				
		void dataBind()
		{
			List<LogInfo> infos = _controller.GetLogsInformation();
			
			var sortedByDateDecreasingLogs = new SortedDictionary<TimeSpan, LogInfo>();
			foreach (var info in infos)
			{
				TimeSpan span = DateTime.MaxValue - info.TimeStamp;
				while (sortedByDateDecreasingLogs.ContainsKey(span))
				{
					span = span.Add(new TimeSpan(1));
				}
				sortedByDateDecreasingLogs.Add(span, info);
			}
			
			var colors = new [] {Color.Green, Color.Red, Color.Brown};
			journalsListView.BeginUpdate();
			journalsListView.Items.Clear();
			
			foreach (KeyValuePair<TimeSpan, LogInfo> pair in sortedByDateDecreasingLogs)
			{
				var item = new ListViewItem( pair.Value.TimeStamp.ToString(ViewLogTime, CultureInfo.CurrentUICulture), (int)pair.Value.Result);
				item.ForeColor = colors[(int) pair.Value.Result];
				item.Group = journalsListView.Groups[(int) pair.Value.Result];
				item.Tag = pair.Value;
				
				journalsListView.Items.Add(item);
			}
			journalsListView.EndUpdate();
			
			updateLogsListButtonsState();
		}
		
		void updateLogsListButtonsState()
		{
			bool anyDataPresent = journalsListView.Items.Count > 0;
			
			journalsListView.Enabled =
                removeSuccesfullLogsButton.Enabled =
				openRecentLogButton.Enabled = anyDataPresent;
				
			updateLocalOperationsButtonsState(this, null);
		}
		
		void listViewResize(object sender, EventArgs e)
		{
			journalsColumnHeader.Width = journalsListView.Width - 40;
		}
		
		void refresh(object sender, EventArgs e)
		{
			dataBind();
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
                System.IO.File.Delete(info.LogFile);

            journalsListView.BeginUpdate();
			foreach (ListViewItem item in items)
			{
				journalsListView.Items.Remove(item);
			}
			journalsListView.EndUpdate();

			updateLogsListButtonsState();
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
				dataBind();
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
				if (info.Result == BackupResult.Successfull)
				{
					infos.Add((info));
				}
			}

			if (!Messages.ShowYesNoDialog(string.Format(Resources.PleaseConfirmDeletionOf0Logs, infos.Count)))
				return;
            foreach (var info in infos)
                System.IO.File.Delete(info.LogFile);
            journalsListView.BeginUpdate();
			foreach (ListViewItem item in items)
			{
				var info = (LogInfo)item.Tag;
				if (info.Result == BackupResult.Successfull)
				{
					journalsListView.Items.Remove(item);
				}
			}
			journalsListView.EndUpdate();

			updateLogsListButtonsState();

		}
		
		void openSelectedLogsInBrowser(object sender, EventArgs e)
		{
			foreach(ListViewItem item in journalsListView.SelectedItems)
			{
                ProcessHelper.ShellExecute(((LogInfo)item.Tag).LogFile);
			}
		}
		
		void openTheRecentLog(object sender, EventArgs e)
		{
			LogInfo latest = (LogInfo)journalsListView.Items[0].Tag;

			foreach(ListViewItem item in journalsListView.Items)
			{
				if (((LogInfo)item.Tag).TimeStamp > latest.TimeStamp)
				{
					latest = (LogInfo)item.Tag;
				}
			}

            ProcessHelper.ShellExecute(latest.LogFile);
		}
		
		void helpButtonClick(object sender, EventArgs e)
		{
            SupportManager.DoSupport(SupportRequest.ManageLogs);
        }		

		#endregion
	}
}
