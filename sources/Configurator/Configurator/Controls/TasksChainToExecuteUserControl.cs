using System;
using System.Windows.Forms;
using System.Collections.Generic;

using BUtil.Core.Options;
using BUtil.Configurator.Forms;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.Controls
{
	/// <summary>
	/// TasksChainToExecuteUserControl is a control where user can manage the tasks queue.
	/// </summary>
	public partial class TasksChainToExecuteUserControl : UserControl
	{
		#region Fields
		
		bool _isBeforeBackupEvent;
		
		#endregion
		
		#region Constructors
		
		/// <summary>
		/// The default constructor
		/// </summary>
		public TasksChainToExecuteUserControl()
		{
			InitializeComponent();
		}
		
		#endregion
		
		#region Public Methods
		
		public List<BackupEventTaskInfo> GetResultChainOfTasks()
		{
			List<BackupEventTaskInfo> result = new List<BackupEventTaskInfo>();
			
			foreach (ListViewItem item in tasksListView.Items)
			{
				result.Add((BackupEventTaskInfo)item.Tag);
			}
			
			return result;
		}
		
		/// <summary>
		/// Initializes the component state
		/// </summary>
		/// <param name="list">The chain of items to edit</param>
		/// <param name="isBeforeBackupEvent">Shows wheather here before backup event tasks are edited</param>
		/// <exception cref="ArgumentNullException">list is nullable</exception>
		public void Init(List<BackupEventTaskInfo> list, bool isBeforeBackupEvent)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			_isBeforeBackupEvent = isBeforeBackupEvent;
			
			tasksListView.BeginUpdate();
			foreach(BackupEventTaskInfo item in list)
			{
				addTaskInfoToList(item);
			}
			tasksListViewResize(null, null);
			tasksListViewSelectedIndexChanged(null, null);
			tasksListView.EndUpdate();
		}
		
		/// <summary>
		/// Sets the configuration to ui
		/// </summary>
		public void ApplyLocalization()
		{
			headerGroupBox.Text = _isBeforeBackupEvent ? Resources.ChainOfProgramsToExecuteBeforeBackup : Resources.ChainOfProgramsToExecuteAfterBackup;
			programColumnHeader.Text = Resources.Program;
			argumentsColumnHeader.Text = Resources.Arguments;
			addToolStripMenuItem.Text = Resources.Add;
			editToolStripMenuItem.Text = Resources.Edit;
			removeToolStripMenuItem.Text = Resources.Remove;
			moveUpToolStripMenuItem.Text = Resources.MoveUp;
			moveDownToolStripMenuItem.Text = Resources.MoveDown;
		}
		
		#endregion
		
		#region Private Methods
		
		void addTaskInfoToList(BackupEventTaskInfo taskInfo)
		{
			ListViewItem listViewItem = new ListViewItem(new string[] {taskInfo.Program, taskInfo.Arguments});
			listViewItem.Tag = taskInfo;
			tasksListView.Items.Add(listViewItem);
		}
		
		void tasksListViewResize(object sender, EventArgs e)
		{
			tasksListView.Columns[1].Width = 100;
			tasksListView.Columns[0].Width = tasksListView.Width - tasksListView.Columns[1].Width - 40;
		}
		
		void tasksListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			bool anyItemIsSelected = tasksListView.SelectedItems.Count > 0;
			bool oneItemIsSelected = tasksListView.SelectedItems.Count == 1;

			removeToolStripMenuItem.Enabled = removeButton.Enabled = anyItemIsSelected;
			editButton.Enabled = editToolStripMenuItem.Enabled = oneItemIsSelected;			
			moveDownToolStripMenuItem.Enabled = moveDownButton.Enabled = oneItemIsSelected && tasksListView.Items.Count > 1 && (tasksListView.SelectedItems[0].Index != tasksListView.Items.Count - 1);
			moveUpToolStripMenuItem.Enabled = moveUpButton.Enabled = oneItemIsSelected && tasksListView.SelectedItems[0].Index != 0 && tasksListView.Items.Count > 1;
		}
		
		void addItem(object sender, EventArgs e)
		{
			using (BackupEventTaskInfoEditingForm form = new BackupEventTaskInfoEditingForm(_isBeforeBackupEvent))
			{
				if (form.ShowDialog() == DialogResult.OK)
				{
					addTaskInfoToList( form.EventTask );
				}
			}
			
			tasksListViewSelectedIndexChanged(sender, e);
		}
		
		void removeSelectedItems(object sender, EventArgs e)
		{
			ListView.SelectedListViewItemCollection itemsToRemove = tasksListView.SelectedItems;
			foreach (ListViewItem item in itemsToRemove)
			{
				tasksListView.Items.Remove(item);
			}
			
			tasksListViewSelectedIndexChanged(sender, e);
		}
		
		void moveUpSelectedItem(object sender, EventArgs e)
		{
			int indexOfItem = tasksListView.SelectedItems[0].Index;
			ListViewItem item = tasksListView.Items[indexOfItem - 1];
			tasksListView.Items.RemoveAt(indexOfItem - 1);
			
			tasksListView.Items.Insert(indexOfItem, item);
			
			tasksListViewSelectedIndexChanged(sender, e);
		}
		
		void moveDownSelectedItem(object sender, EventArgs e)
		{
			int indexOfItem = tasksListView.SelectedItems[0].Index;
			ListViewItem item = tasksListView.Items[indexOfItem + 1];
			tasksListView.Items.RemoveAt(indexOfItem + 1);
			
			tasksListView.Items.Insert(indexOfItem, item);
			
			tasksListViewSelectedIndexChanged(sender, e);
		}
		
		void editSelectedItem(object sender, EventArgs e)
		{
			if (tasksListView.SelectedItems.Count != 1)
			{
				return;
			}

			int indexOfItem = tasksListView.SelectedItems[0].Index;
			
			using (BackupEventTaskInfoEditingForm form = new BackupEventTaskInfoEditingForm(_isBeforeBackupEvent, (BackupEventTaskInfo)tasksListView.SelectedItems[0].Tag))
			{
				if (form.ShowDialog() == DialogResult.OK)
				{
					BackupEventTaskInfo task = form.EventTask;
					
					tasksListView.SelectedItems[0].SubItems[0].Text = task.Program;
					tasksListView.SelectedItems[0].SubItems[1].Text = task.Arguments;
					tasksListView.SelectedItems[0].Tag = task;					
				}
			}
			
		}
		
		#endregion
	}
}