using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using BUtil.Core.Options;
using BUtil.Configurator.Localization;
using System.Diagnostics;

namespace BUtil.Configurator.Controls
{
    internal sealed partial class WhatUserControl : BUtil.Core.PL.BackUserControl
	{
		private readonly BackupTask _task;
		
		public WhatUserControl(BackupTask task)
		{
            _task = task;

			InitializeComponent();

            itemsToCompressColumnHeader.Text = Resources.ItemsToBackup;
            SetHintForControl(compressionItemsListView, Resources.DragAndDropHereFilesAndFoldersWhichYoureGoingToBackupForSettingCompressionPriorityUseMenu);
            SetHintForControl(addFoldersButton, Resources.AddFolders);
            addFoldersToolStripMenuItem.Text = Resources.AddFolders;
            SetHintForControl(removeCompressionItemButton, Resources.Remove);
            removeFromListToolStripMenuItem.Text = Resources.Remove;
            SetHintForControl(addFilesButton, Resources.AddFiles);
            addFilesToolStripMenuItem.Text = Resources.AddFiles;

            foreach (SourceItem item in _task.Items)
            {
                addItem(item, false);
            }
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

                VerifyNewItems(files);
            }
        }

		void addFoldersToTheBackupItemsList()
        {
            while (ofd.ShowDialog() == DialogResult.OK)
            {
            	SourceItem folder = new SourceItem(ofd.SelectedPath, true);

				VerifyNewItem(folder);
            }
        }

		void removeSelectedBackupItemFromList()
		{
			if (compressionItemsListView.SelectedItems.Count > 0)
			{
                int newSelectedIndex = compressionItemsListView.SelectedIndices[0];
				RemoveItems(compressionItemsListView.SelectedIndices);

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

				VerifyNewItems(files);
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
                OpenItem(compressionItemsListView.SelectedIndices[0]);
            }
        }

		#region Overrides
				
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

            foreach (ListViewItem listItem in compressionItemsListView.Items)
            {
				var item = listItem.Text;
				if (!Directory.Exists(item) &&
					!File.Exists(item))
				{
					Messages.ShowErrorBox(string.Format(BUtil.Configurator.Localization.Resources.SourceItemFailure, item));
                    return false;
                }
            }

            return true;

        }

        #endregion

        private void updateGuiControlHelper()
        {
            compressionItemsListView.BeginUpdate();

            compressionItemsListView.Items.Clear();
            foreach (SourceItem item in _task.Items)
            {
                ListViewItem newlistViewItem = new ListViewItem(item.Target);
                newlistViewItem.ImageIndex = item.IsFolder ? 0 : 1;

                compressionItemsListView.Items.Add(newlistViewItem);

            }
            compressionItemsListView.EndUpdate();
        }

        void addItem(SourceItem newItem, bool add)
        {
            if (newItem.Target.StartsWith(@"\\", StringComparison.InvariantCulture))
            {
                // "Network places are not allowed to be added to the list of backup items!"
                MessageBox.Show(Resources.NetworkPlacesAreNotAllowedToBeAddedToTheListOfBackupItems, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);
                return;
            }

            if (add)
            {
                _task.Items.Add(newItem);
            }

            compressionItemsListView.BeginUpdate();

            ListViewItem newlistViewItem = new ListViewItem(newItem.Target);
            if (newItem.IsFolder)
            {
                newlistViewItem.ImageIndex = 0;
            }
            else
            {
                newlistViewItem.ImageIndex = 1;
            }

            compressionItemsListView.Items.Add(newlistViewItem);

            compressionItemsListView.EndUpdate();
        }

        public void OpenItem(int itemIndex)
        {
            if (_task.Items[itemIndex].IsFolder)
            {
                Process.Start("explorer.exe", _task.Items[itemIndex].Target);
            }
        }

        public void RemoveItems(ListView.SelectedIndexCollection indexes)
        {
            for (int i = (indexes.Count - 1); i > -1; i--)
            {
                _task.Items.RemoveAt(indexes[i]);
            }
            updateGuiControlHelper();
        }

        public void RemoveItem(int index)
        {
            _task.Items.RemoveAt(index);
            updateGuiControlHelper();
        }

        public void VerifyNewItem(SourceItem newItem)
        {
            VerifyNewItems(new SourceItem[] { newItem });
        }
        /// <summary>
        /// Required: all files or folders should be added from one folder level.
        /// </summary>
        public void VerifyNewItems(SourceItem[] newItems)
        {
            bool AddFlag;
            string itemrepresentation;
            string fflist;

            if (newItems.Length == 0) return;

            if (_task.Items.Count == 0)
            {
                for (int i = 0; i < newItems.Length; i++) addItem(newItems[i], true);
                return;
            }

            // Checking whether more global things added
            for (int itemid = 0; itemid < newItems.Length; itemid++)
            {
                itemrepresentation = newItems[itemid].Target;
                if (itemrepresentation.Length == 0) continue;

                int i = 0;
                while (i < _task.Items.Count)
                {

                    fflist = _task.Items[i].Target;

                    // Winfows
                    if (fflist.StartsWith(itemrepresentation + "\\", StringComparison.InvariantCulture))
                    {
                        RemoveItem(i);
                        continue;
                    }

                    // Linux, Unix, Networks
                    if (fflist.StartsWith(itemrepresentation + "/", StringComparison.InvariantCulture))
                    {
                        RemoveItem(i);
                        continue;
                    }
                    i++;
                }
            }

            for (int itemid = 0; itemid < newItems.Length; itemid++)
            {
                AddFlag = true;
                itemrepresentation = newItems[itemid].Target;
                if (string.IsNullOrEmpty(itemrepresentation))
                    continue;
                for (int i = 0; i < _task.Items.Count; i++)
                {
                    fflist = _task.Items[i].Target;

                    if (string.IsNullOrEmpty(fflist))
                        continue;
                    // repeatings are not allowed
                    if (fflist == itemrepresentation)
                    {
                        AddFlag = false;
                        break;
                    };
                    // inside-folder files aten't allowed
                    // win specific
                    if (itemrepresentation.StartsWith(fflist + "\\", StringComparison.InvariantCulture))
                    {
                        AddFlag = false;
                        break;
                    };

                    // inside-folder files aten't allowed
                    // linux and networks specific
                    if (itemrepresentation.StartsWith(fflist + "/", StringComparison.InvariantCulture))
                    {
                        AddFlag = false;
                        break;
                    };

                    // D:\ d:/ and so on
                    if ((fflist.EndsWith("\\", StringComparison.InvariantCulture) || fflist.EndsWith("/", StringComparison.InvariantCulture)) && itemrepresentation.StartsWith(fflist))
                    {
                        AddFlag = false;
                        break;
                    };
                }

                if (AddFlag)
                    addItem(newItems[itemid], true);
            }
        }
    }
}
