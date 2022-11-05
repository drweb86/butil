using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using BUtil.Core.ButilImage;
using BUtil.Core.FileSystem;
using BUtil.Configurator.Localization;

namespace BUtil.RestorationMaster
{
	/// <summary>
	/// Here a list of contents of image is accessible
	/// </summary>
	internal partial class RestoreForm : Form
	{
		readonly RestoreController _controller;
		readonly Collection<MetaRecord> _records;

		public RestoreForm(RestoreController controller)
		{
			_controller = controller;

			InitializeComponent();

			// locals
			itemsListView.Groups[0].Header = Resources.Folders;
			itemsListView.Groups[1].Header = Resources.Files;
			restoreToolStripMenuItem.Text = Resources.Restore;
			finishButton.Text = Resources.Finish;
			restoreButton.Text = Resources.Recover;
			//Resources.UseRightClickMouseOnSelectedItemToRestoreIt;
			itemsColumnHeader.Text = Resources.ItemsInImage;
			this.Text = string.Format(Resources._0Restoration, controller.ImageLocation);
			restoreButton.Left = finishButton.Left - restoreButton.Width - 10;

			_records = _controller.MetaRecords;

			itemsListView.BeginUpdate();

			foreach (MetaRecord record in _records)
			{
				ListViewItem listViewItem = new ListViewItem(record.InitialTarget);
				listViewItem.ToolTipText = record.InitialTarget;

				if (record.IsFolder)
				{
					listViewItem.Group = itemsListView.Groups[0];
					listViewItem.ImageIndex = 1;
				}
				else
				{
					listViewItem.Group = itemsListView.Groups[1];
					listViewItem.ImageIndex = 0;
				}

				itemsListView.Items.Add(listViewItem);
				
			}

			itemsListView.EndUpdate();
			itemsListViewResize(null, null);
		}
		
		void restoreContextMenuStripOpening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			restoreToolStripMenuItem.Enabled = (itemsListView.SelectedIndices.Count > 0);
		}
		
		void restoreButtonClick(object sender, EventArgs e)
		{
			restore();
		}
		
		void restore()
		{
			if (itemsListView.SelectedIndices.Count == 0) 
			{
				// "First, select an item!"
				MessageBox.Show(Resources.FirstSelectAnItem, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);
				return;
			}
			
			ListView.SelectedIndexCollection collection = itemsListView.SelectedIndices;
			
			foreach (int index in collection)
			{
				using (HowToRestoreForm form = new HowToRestoreForm(_records[index], _controller))
				{
					form.ShowDialog();
				}
			}
		}
		
		void restoreToolStripMenuItemClick(object sender, EventArgs e)
		{
			restore();
		}
		
		void itemsListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			restoreButton.Enabled = itemsListView.SelectedItems.Count > 0;
		}
		
		void itemsListViewDoubleClick(object sender, EventArgs e)
		{
			restore();
		}
		
		void itemsListViewResize(object sender, EventArgs e)
		{
			itemsListView.Columns[0].Width = itemsListView.Width - 40;
		}
	}
}
