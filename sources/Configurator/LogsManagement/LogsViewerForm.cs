using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;

using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.LogsManagement
{
	/// <summary>
	/// The form where we can manage the logs
	/// </summary>
	internal sealed partial class LogsViewerForm : Form
	{
		#region Constants
		
		const string ViewLogTime = "dd MMMM (dddd) HH:mm";
		
		#endregion
		
		#region Fields
		
		readonly LogManagementConftroller _controller;
		
		#endregion
		
		#region Constructors
		
		public LogsViewerForm(LogManagementConftroller controller)
		{
			InitializeComponent();

			_controller = controller;
			
			listViewResize(this, null);
			
			// applying localization
			Text = Resources.BackupJournals;
			
            toolTip.SetToolTip(openLogsFolderButton, Resources.OpenLogsFolderInExplorer);
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
			
//? subj to be removed			actionsPanel.Header = Resources.Actions;
			
			dataBind();
		}
		
		#endregion
		
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
		
		void openLogsFolderInExplorer(object sender, EventArgs e)
		{
			_controller.OpenLogsFolderInExplorer();
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
			foreach(ListViewItem item in items)
			{
				infos.Add((LogInfo)item.Tag);
			}
			
			if (_controller.DeleteSetOfLogs(infos))
			{
				journalsListView.BeginUpdate();
				foreach(ListViewItem item in items)
				{
					journalsListView.Items.Remove(item);
				}
				journalsListView.EndUpdate();
	
				updateLogsListButtonsState();
			}
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
			foreach(ListViewItem item in items)
			{
				var info = (LogInfo)item.Tag;
				if (info.Result == BackupResult.Successfull)
				{
					infos.Add((info));
				}
			}
			
			if (_controller.DeleteSetOfLogs(infos))
			{
				journalsListView.BeginUpdate();
				foreach(ListViewItem item in items)
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
		}

// Method excluded due to overloaded ui and because it is dangerous
//		void removeAllLogs(object sender, EventArgs e)
//		{
//			ListView.ListViewItemCollection items = journalsListView.Items;
//			
//			var infos = new List<LogInfo>();
//			foreach(ListViewItem item in items)
//			{
//				infos.Add((LogInfo)item.Tag);
//			}
//			
//			if (_controller.DeleteSetOfLogs(infos))
//			{
//				journalsListView.BeginUpdate();
//			
//				foreach(ListViewItem item in items)
//				{
//					journalsListView.Items.Remove(item);
//				}
//				journalsListView.EndUpdate();
//	
//				updateLogsListButtonsState();
//			}
//		}
		
		void openSelectedLogsInBrowser(object sender, EventArgs e)
		{
			foreach(ListViewItem item in journalsListView.SelectedItems)
			{
				_controller.OpenLogInBrowser((LogInfo)item.Tag);
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
			
			_controller.OpenLogInBrowser(latest);
		}
		
		void helpButtonClick(object sender, EventArgs e)
		{
            SupportManager.DoSupport(SupportRequest.ManageLogs);
        }		

		#endregion
	}
}
