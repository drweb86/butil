using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using BUtil.Core.ButilImage;
using BUtil.Core.FileSystem;

using BULocalization;

namespace BUtil.RestorationMaster
{
	/// <summary>
	/// Here a list of contents of image is accessible
	/// </summary>
	internal partial class RestoreForm : Form
	{
		readonly RestorationMasterController _controller;
		readonly Collection<MetaRecord> _records;

		public RestoreForm(RestorationMasterController controller)
		{
			_controller = controller;

			InitializeComponent();

			// locals
			itemsListView.Groups[0].Header = Translation.Current[420];
			itemsListView.Groups[1].Header = Translation.Current[421];
			restoreToolStripMenuItem.Text = Translation.Current[422];
			finishButton.Text = Translation.Current[423];
			restoreButton.Text = Translation.Current[424];
			//Translation.Current[425];
			itemsColumnHeader.Text = Translation.Current[426];
			this.Text = string.Format(Translation.Current[427], controller.ImageLocation);
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
				MessageBox.Show(Translation.Current[419], string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);
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
