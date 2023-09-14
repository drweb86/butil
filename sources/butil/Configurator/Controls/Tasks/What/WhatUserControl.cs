using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using BUtil.Configurator.Localization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using BUtil.Configurator.Configurator.Controls.Tasks.What;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Configurator.Controls
{
    internal sealed partial class WhatUserControl : BUtil.Core.PL.BackUserControl
	{
		private readonly BackupTaskV2 _task;
		
		public WhatUserControl(BackupTaskV2 task)
		{
            _task = task;

			InitializeComponent();

            itemsToCompressColumnHeader.Text = Resources.ItemsToBackup;
            SetHintForControl(_itemsListView, Resources.DragAndDropHereFilesAndFoldersWhichYoureGoingToBackupForSettingCompressionPriorityUseMenu);
            SetHintForControl(addFoldersButton, Resources.AddFolders);
            addFoldersToolStripMenuItem.Text = Resources.AddFolders;

            SetHintForControl(removeButton, Resources.Remove);
            removeToolStripMenuItem.Text = Resources.Remove;

            SetHintForControl(addFilesButton, Resources.AddFiles);
            addFilesToolStripMenuItem.Text = Resources.AddFiles;

            _addFileExcludePatternToolStripMenuItem.Text = Resources.AddFileExcludePattern;
            _addFileExcludePatternToolStripMenuItem.ToolTipText = BUtil.Configurator.Localization.Resources.ExcludesFilesFromBackupByPattern;
            SetHintForControl(_addFIleExcludePatternButton, BUtil.Configurator.Localization.Resources.ExcludesFilesFromBackupByPattern);
            _editFileExcludePatternToolStripMenuItem.Text = BUtil.Configurator.Localization.Resources.EditFileExcludePattern;
            _openInExplorerToolStripMenuItem.Text = BUtil.Configurator.Localization.Resources.OpenInExplorer;

            var options = (IncrementalBackupModelOptionsV2)_task.Model;
            options.Items
                .Select(item => new WhatItemViewModel { Id = item.Id, Title = item.Target, Type = item.IsFolder ? WhatItemType.Folder : WhatItemType.File })
                .OrderBy(item => item.Title)
                .ToList()
                .ForEach(AddItem);
            (options.FileExcludePatterns ?? new List<string>())
                .Select(x => new WhatItemViewModel { Id = Guid.NewGuid(), Title = x, Type = WhatItemType.Exclude })
                .OrderBy(item => item.Title)
                .ToList()
                .ForEach(AddItem);
        }
		
	
		private void OnAddFoldersButtonClick(object sender, EventArgs e)
		{
            AddFolders();
		}

        private void OnAddFoldersToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddFolders();
        }

        private void AddFolders()
        {
            while (ofd.ShowDialog() == DialogResult.OK)
            {
                AddItem(new WhatItemViewModel { Id = Guid.NewGuid(), Title = ofd.SelectedPath, Type = WhatItemType.Folder });
            }
        }

        private void OnAddFilesButtonClick(object sender, EventArgs e)
		{
			AddFiles();
		}

        private void OnAddFilesToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddFiles();
        }

        private void AddFiles()
        {
            while (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openFileDialog
                    .FileNames
                    .Select(x => new WhatItemViewModel { Id = Guid.NewGuid(), Title = x, Type = WhatItemType.File })
                    .ToList()
                    .ForEach(AddItem);
            }
        }

        private void OnDragDrop(object sender, DragEventArgs e)
		{
			if( e.Data.GetDataPresent(DataFormats.FileDrop, false) )
			{
				((string[])e.Data.GetData(DataFormats.FileDrop))
                    .Select(x => new WhatItemViewModel 
                    {
                        Id = Guid.NewGuid(),
                        Title = x,
                        Type = Directory.Exists(x) ? WhatItemType.Folder : WhatItemType.File
                    })
                    .ToList()
                    .ForEach(AddItem);
			}
		}
		
		private void OnDragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.All;
		}
		
		private void OnSelectedIndexChanged(object sender, EventArgs e)
		{
			removeButton.Enabled = _itemsListView.SelectedItems.Count > 0;
		}
		
		private void OnMenuOpening(object sender, CancelEventArgs e)
		{
			removeToolStripMenuItem.Enabled = (_itemsListView.SelectedItems.Count > 0);
            var canOpenInExplorer = 
                _itemsListView.SelectedItems.Count == 1 &&
                ((_itemsListView.SelectedItems[0].Tag as WhatItemViewModel).Type == WhatItemType.File ||
                     (_itemsListView.SelectedItems[0].Tag as WhatItemViewModel).Type == WhatItemType.Folder);

            var canEditPattern =
                _itemsListView.SelectedItems.Count == 1 &&
                (_itemsListView.SelectedItems[0].Tag as WhatItemViewModel).Type == WhatItemType.Exclude;

            _openInExplorerToolStripMenuItem.Enabled = canOpenInExplorer;
            _editFileExcludePatternToolStripMenuItem.Enabled = canEditPattern;
        }

        private void OnRemoveItemsButtonClick(object sender, EventArgs e)
        {
            RemoveSelectedItems();
        }

        private void OnRemoveToolStripMenuItemClick(object sender, EventArgs e)
		{
			RemoveSelectedItems();
		}

        private void RemoveSelectedItems()
        {
            var itemsToDelete = new List<ListViewItem>();
            var indexes = new List<int>();
            foreach (ListViewItem item in _itemsListView.SelectedItems)
                itemsToDelete.Add(item);
            foreach (int index in _itemsListView.SelectedIndices)
                indexes.Add(index);
            itemsToDelete.ForEach(_itemsListView.Items.Remove);
            if (indexes.Any())
            {
                var minIndex = indexes.Min() - 1;
                if (minIndex < 0)
                    minIndex = 0;
                if (_itemsListView.Items.Count > minIndex)
                    _itemsListView.Items[minIndex].Selected = true;
            }

            OnSelectedIndexChanged(null, null);
        }

        private void OnResize(object sender, EventArgs e)
		{
			_itemsListView.Columns[0].Width = _itemsListView.Width - 40;
		}

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                foreach (ListViewItem item in _itemsListView.Items)
                    item.Selected = true;
            }

            if (e.KeyCode == Keys.Delete)
            {
                RemoveSelectedItems();
            }
        }

        private void OnItemDoubleClick(object sender, EventArgs e)
        {
            var items = new List<WhatItemViewModel>();
            foreach (ListViewItem item in _itemsListView.SelectedItems)
                items.Add(item.Tag as WhatItemViewModel);

            var itemToOpen = items.FirstOrDefault();

            if (itemToOpen != null)
			{
                if (itemToOpen.Type == WhatItemType.Folder)
                    Process.Start("explorer.exe", $"\"{itemToOpen.Title}\"");

                if (itemToOpen.Type == WhatItemType.File)
                    Process.Start("explorer.exe", $"/select,\"{itemToOpen.Title}\"");

                if (itemToOpen.Type == WhatItemType.Exclude)
                {
                    using FileExcludePatternForm form = new(itemToOpen.Title);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        itemToOpen.Title = form.Pattern;
                        _itemsListView.SelectedItems[0].Text = form.Pattern;
                    }
                }
            }
        }

		#region Overrides
				
		public override void GetOptionsFromUi()
		{
            var fileExcludePatterns = new List<string>();
            var sourceItems = new List<SourceItemV2>();
            foreach (ListViewItem item in _itemsListView.Items)
            {
                var model = item.Tag as WhatItemViewModel;

                if (model.Type == WhatItemType.Exclude)
                    fileExcludePatterns.Add(model.Title);
                if (model.Type == WhatItemType.File)
                    sourceItems.Add(new SourceItemV2 { Id = model.Id, IsFolder = false, Target = model.Title });
                if (model.Type == WhatItemType.Folder)
                    sourceItems.Add(new SourceItemV2 { Id = model.Id, IsFolder = true, Target = model.Title });
            }

            var options = (IncrementalBackupModelOptionsV2)_task.Model;
            options.Items = sourceItems;
            options.FileExcludePatterns = fileExcludePatterns;
        }

		public override bool ValidateUi()
		{
            GetOptionsFromUi();

            var options = (IncrementalBackupModelOptionsV2)_task.Model;
            if (options.Items.Count == 0)
			{
				Messages.ShowErrorBox(Resources.ThereAreNoItemsToBackupNNyouCanSpecifyTheDataToBackupInConfiguratorInWhatSettingsGroup);
                return false;
			}

            foreach (var item in options.Items)
            {
                if (item.IsFolder &&
                    !Directory.Exists(item.Target))
                {
                    Messages.ShowErrorBox(string.Format(BUtil.Configurator.Localization.Resources.SourceItemFailure, item.Target));
                    return false;
                }

                if (!item.IsFolder &&
                    !File.Exists(item.Target))
                {
                    Messages.ShowErrorBox(string.Format(BUtil.Configurator.Localization.Resources.SourceItemFailure, item.Target));
                    return false;
                }
            }

            return true;
        }

        #endregion

        private void AddItem(WhatItemViewModel item)
        {
            var listItem = new ListViewItem(item.Title) { Tag = item };
            switch (item.Type)
            {
                case WhatItemType.File:
                    listItem.ImageIndex = 1;
                    break;

                case WhatItemType.Folder:
                    listItem.ImageIndex = 0;
                    break;

                case WhatItemType.Exclude:
                    listItem.ImageIndex = 2;
                    break;
            }
            _itemsListView.Items.Add(listItem);
        }

        private void OnAddFileExcludePattern(object sender, EventArgs e)
        {
            using FileExcludePatternForm form = new();
            if (form.ShowDialog() == DialogResult.OK)
                AddItem(new WhatItemViewModel { Id = Guid.NewGuid(), Title = form.Pattern, Type = WhatItemType.Exclude });
        }
    }
}
