using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using System.IO;
using BUtil.Core.Options;
using BUtil.Configurator;
using BUtil.Core.ButilImage;
using BUtil.Core;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.Controls
{
	/// <summary>
	/// Manages source items
	/// </summary>
	internal sealed partial class SourceItemsUserControl : BUtil.Core.PL.BackUserControl
	{
		CompressionItemsKeeper _itemsToBackup;
		BackupTask _task;
		
		public SourceItemsUserControl()
		{
			InitializeComponent();
		}
		
		void addFilesToTheBackupItemsList()
        {
            while (openFileDialog.ShowDialog() == DialogResult.OK)
            {
            	CompressionItem[] files = new CompressionItem[openFileDialog.FileNames.Length];

                for (int i = 0; i < files.Length; i++)
                {
					files[i] = new CompressionItem(openFileDialog.FileNames[i], false, CompressionDegree.Normal);
                }

                _itemsToBackup.VerifyNewItems(files);
            }
        }

		void addFoldersToTheBackupItemsList()
        {
            while (ofd.ShowDialog() == DialogResult.OK)
            {
            	CompressionItem folder = new CompressionItem(ofd.SelectedPath, true, CompressionDegree.Normal);

				_itemsToBackup.VerifyNewItem(folder);
            }
        }

		void removeSelectedBackupItemFromList()
		{
			if (compressionItemsListView.SelectedItems.Count > 0)
			{
                int newSelectedIndex = compressionItemsListView.SelectedIndices[0];
				_itemsToBackup.RemoveItems(compressionItemsListView.SelectedIndices);

                // checking the new item
                if (compressionItemsListView.Items.Count > 0)
                {
                    if (compressionItemsListView.Items.Count > newSelectedIndex)
                    {
                        compressionItemsListView.Items[newSelectedIndex].Selected = true;
                    }
                    else
                    {
                        compressionItemsListView.Items[
                            compressionItemsListView.Items.Count - 1].Selected = true;
                    }
                }

                compressionItemsListViewSelectedIndexChanged(null, null);

			}
		}
		
		void removeCompressionItemButtonClick(object sender, EventArgs e)
		{
			removeSelectedBackupItemFromList();
		}
		
		void addFoldersButtonClick(object sender, EventArgs e)
		{
			addFoldersToTheBackupItemsList();
		}
		
		void addFilesButtonClick(object sender, EventArgs e)
		{
			addFilesToTheBackupItemsList();
		}
		
		void compressionItemsListViewDragDrop(object sender, DragEventArgs e)
		{
			if( e.Data.GetDataPresent(DataFormats.FileDrop, false) )
			{
				string[] newitems = (string[])e.Data.GetData(DataFormats.FileDrop);
				CompressionItem[] files = new CompressionItem[newitems.Length];
				for (int i = 0; i < newitems.Length; i++)
				{
					files[i] = new CompressionItem(newitems[i], Directory.Exists(newitems[i]), CompressionDegree.Normal);
				}

				_itemsToBackup.VerifyNewItems(files);
			}
		}
		
		void compressionItemsListViewDragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.All;
		}
		
		void compressionItemsListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			removeCompressionItemButton.Enabled = compressionItemsListView.SelectedItems.Count > 0;
		}
		
		void filesFoldersContextMenuStripOpening(object sender, CancelEventArgs e)
		{
			removeFromListToolStripMenuItem.Enabled = (compressionItemsListView.SelectedItems.Count > 0);
			setCompressionDegreeToolStripMenuItem.Enabled = (compressionItemsListView.SelectedItems.Count > 0);
		}
		
		void storeToolStripMenuItemClick(object sender, EventArgs e)
		{
			_itemsToBackup.UpdateCompressionLevelForSelectedItems(0);
		}
		
		void fastestToolStripMenuItemClick(object sender, EventArgs e)
		{
			_itemsToBackup.UpdateCompressionLevelForSelectedItems(1);
		}
		
		void fastToolStripMenuItemClick(object sender, EventArgs e)
		{
			_itemsToBackup.UpdateCompressionLevelForSelectedItems(2);
		}
		
		void normalToolStripMenuItemClick(object sender, EventArgs e)
		{
			_itemsToBackup.UpdateCompressionLevelForSelectedItems(3);
		}
		
		void maximumToolStripMenuItemClick(object sender, EventArgs e)
		{
			_itemsToBackup.UpdateCompressionLevelForSelectedItems(4);
		}
		
		void ultraToolStripMenuItemClick(object sender, EventArgs e)
		{
			_itemsToBackup.UpdateCompressionLevelForSelectedItems(5);
		}
		
		void removeFromListToolStripMenuItemClick(object sender, EventArgs e)
		{
			removeSelectedBackupItemFromList();
		}
		
		void addFilesToolStripMenuItemClick(object sender, EventArgs e)
		{
			addFilesToTheBackupItemsList();
		}
		
		void addFoldersToolStripMenuItemClick(object sender, EventArgs e)
		{
			addFoldersToTheBackupItemsList();
		}

		void compressionItemsListViewResize(object sender, EventArgs e)
		{
			compressionItemsListView.Columns[0].Width = compressionItemsListView.Width - 40;
		}

        void compressionItemsListViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                foreach (ListViewItem item in compressionItemsListView.Items)
                {
                    item.Selected = true;
                }
            }

            if (e.KeyCode == Keys.Delete)
            {
                removeSelectedBackupItemFromList();
            }
        }

        void compressionItemsListViewDoubleClick(object sender, EventArgs e)
        {
            if (compressionItemsListView.SelectedItems.Count > 0)
			{
                _itemsToBackup.OpenItem(compressionItemsListView.SelectedIndices[0]);
            }
        }

		#region Overrides
		
		public override void ApplyLocalization() 
		{
			if (_itemsToBackup != null)
			{
				_itemsToBackup.ApplyNewDegreesOfCompression();
			}

			storeToolStripMenuItem.Text = LocalsHelper.ToString(CompressionDegree.Store);
			fastestToolStripMenuItem.Text = LocalsHelper.ToString(CompressionDegree.Fastest);
			fastToolStripMenuItem.Text = LocalsHelper.ToString(CompressionDegree.Fast);
			normalToolStripMenuItem.Text = LocalsHelper.ToString(CompressionDegree.Normal);
			maximumToolStripMenuItem.Text = LocalsHelper.ToString(CompressionDegree.Maximum);
			ultraToolStripMenuItem.Text = LocalsHelper.ToString(CompressionDegree.Ultra);
			
			itemsToCompressColumnHeader.Text = Resources.ItemsToBackup;
			setCompressionDegreeToolStripMenuItem.Text = Resources.SetCompressionDegree;
			
			SetHintForControl(compressionItemsListView, Resources.DragAndDropHereFilesAndFoldersWhichYoureGoingToBackupForSettingCompressionPriorityUseMenu);
            SetHintForControl(addFoldersButton, Resources.AddFolders);
            addFoldersToolStripMenuItem.Text = Resources.AddFolders;
            SetHintForControl(removeCompressionItemButton, Resources.Remove);
            removeFromListToolStripMenuItem.Text = Resources.Remove;
            SetHintForControl(addFilesButton, Resources.AddFiles);
            addFilesToolStripMenuItem.Text = Resources.AddFiles;
		}
	
		public override void SetOptionsToUi(object settings)
		{
            _task = (BackupTask)settings;
				
			_itemsToBackup = new CompressionItemsKeeper(compressionItemsListView, _task.FilesFoldersList);
			_itemsToBackup.ApplyNewDegreesOfCompression();
			_itemsToBackup.InitWith();
		}
		
		public override void GetOptionsFromUi()
		{
			;
		}
		
		#endregion
	}
}
