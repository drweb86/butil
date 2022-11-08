using System;
using System.ComponentModel;
using System.Windows.Forms;

using System.IO;
using BUtil.Core.Options;
using BUtil.Core.ButilImage;
using BUtil.Core;
using BUtil.Configurator.Localization;
using BUtil.Core.PL;

namespace BUtil.Configurator.Controls
{
	internal sealed partial class WhatUserControl : BUtil.Core.PL.BackUserControl
	{
		CompressionItemsKeeper _itemsToBackup;
		BackupTask _task;
		
		public WhatUserControl()
		{
			InitializeComponent();
		}
		
		void addFilesToTheBackupItemsList()
        {
            while (openFileDialog.ShowDialog() == DialogResult.OK)
            {
            	SourceItem[] files = new SourceItem[openFileDialog.FileNames.Length];

                for (int i = 0; i < files.Length; i++)
                {
					files[i] = new SourceItem(openFileDialog.FileNames[i], false);
                }

                _itemsToBackup.VerifyNewItems(files);
            }
        }

		void addFoldersToTheBackupItemsList()
        {
            while (ofd.ShowDialog() == DialogResult.OK)
            {
            	SourceItem folder = new SourceItem(ofd.SelectedPath, true);

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
				SourceItem[] files = new SourceItem[newitems.Length];
				for (int i = 0; i < newitems.Length; i++)
				{
					files[i] = new SourceItem(newitems[i], Directory.Exists(newitems[i]));
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
			itemsToCompressColumnHeader.Text = Resources.ItemsToBackup;
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
				
			_itemsToBackup = new CompressionItemsKeeper(compressionItemsListView, _task.Items);
			_itemsToBackup.InitWith();
		}
		
		public override void GetOptionsFromUi()
		{
			;
		}

		public override bool ValidateUi()
		{
            if (compressionItemsListView.Items.Count == 0)
			{
				Messages.ShowErrorBox(Resources.ThereAreNoItemsToBackupNNyouCanSpecifyTheDataToBackupInConfiguratorInWhatSettingsGroup);

                return false;
			}

			return true;

        }

		#endregion
	}
}
