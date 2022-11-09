using System;
using System.Windows.Forms;
using System.Collections.Generic;
using BUtil.Core.Options;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.Controls
{
	public partial class ExecuteProgramTaskInfoListControl : UserControl
	{
		bool _isBeforeBackupEvent;
		
		public ExecuteProgramTaskInfoListControl()
		{
			InitializeComponent();
		}
		
		public List<ExecuteProgramTaskInfo> GetResultChainOfTasks()
		{
			List<ExecuteProgramTaskInfo> result = new List<ExecuteProgramTaskInfo>();
			
			foreach (ListViewItem item in tasksListView.Items)
			{
				result.Add((ExecuteProgramTaskInfo)item.Tag);
			}
			
			return result;
		}
		
		public void Init(List<ExecuteProgramTaskInfo> list, bool isBeforeBackupEvent)
		{
			_isBeforeBackupEvent = isBeforeBackupEvent;
			
			tasksListView.BeginUpdate();
			foreach(var item in list)
			{
				addTaskInfoToList(item);
			}
			tasksListViewResize(null, null);
			tasksListViewSelectedIndexChanged(null, null);
			tasksListView.EndUpdate();
		}
		
		public void SetHintToControls(Action<Control, string> setHint)
		{
			setHint(addButton, Resources.Add);
            setHint(editButton, Resources.Edit);
            setHint(removeButton, Resources.Remove);
            setHint(moveUpButton, Resources.MoveUp);
            setHint(moveDownButton, Resources.MoveDown);
        }

        public void ApplyLocalization()
		{
			headerGroupBox.Text = _isBeforeBackupEvent ? Resources.ChainOfProgramsToExecuteBeforeBackup : Resources.ChainOfProgramsToExecuteAfterBackup;
			_nameColumnHeader.Text = Resources.Name;
			addToolStripMenuItem.Text = Resources.Add;
			editToolStripMenuItem.Text = Resources.Edit;
			removeToolStripMenuItem.Text = Resources.Remove;
			moveUpToolStripMenuItem.Text = Resources.MoveUp;
			moveDownToolStripMenuItem.Text = Resources.MoveDown;
		}
		
		void addTaskInfoToList(ExecuteProgramTaskInfo taskInfo)
		{
			var listViewItem = new ListViewItem(taskInfo.Name)
			{
				Tag = taskInfo
			};
			tasksListView.Items.Add(listViewItem);
		}
		
		void tasksListViewResize(object sender, EventArgs e)
		{
			tasksListView.Columns[0].Width = tasksListView.Width - 24;
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
			using var form = new Forms.ExecuteProgramTaskInfoForm();
			if (form.ShowDialog() == DialogResult.OK)
				addTaskInfoToList( form.EventTask );
			
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

			using var form = new Forms.ExecuteProgramTaskInfoForm((ExecuteProgramTaskInfo)tasksListView.SelectedItems[0].Tag);
			if (form.ShowDialog() == DialogResult.OK)
			{
				var task = form.EventTask;

				tasksListView.SelectedItems[0].SubItems[0].Text = task.Name;
				tasksListView.SelectedItems[0].Tag = task;
			}
		}
	}
}