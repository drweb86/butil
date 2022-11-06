using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BUtil.Core.Options;
using BUtil.Core.State;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BUtil.RestorationMaster
{
	internal partial class VersionsViewerForm : Form
	{
		private readonly IncrementalBackupState _incrementalBackupState;

		public VersionsViewerForm(IncrementalBackupState incrementalBackupState = null)
		{
			InitializeComponent();
			_incrementalBackupState = incrementalBackupState;

            //Resources.Recover;
			//Resources.UseRightClickMouseOnSelectedItemToRestoreIt;
		}
		
		private void Restore()
		{
				//using (HowToRestoreForm form = new HowToRestoreForm(_records[index], _controller))
				//{
				//	form.ShowDialog();
				//}
		}

		private void OnLoad(object sender, System.EventArgs e)
		{
			_versionsListBox.BeginUpdate();

			var versionsDesc = _incrementalBackupState.VersionStates
				.OrderByDescending(x => x.BackupDateUtc)
				.ToList();

			_versionsListBox.DataSource = versionsDesc;
            _versionsListBox.DisplayMember = nameof(VersionState.BackupDateUtc);
            _versionsListBox.EndUpdate();

            _versionsListBox.SelectedItem = versionsDesc.First();
            this.OnVersionListChange(sender, e);
        }

		private void OnVersionListChange(object sender, System.EventArgs e)
		{
            RefreshChangesView();
            RefreshTreeView();
        }

        private void RefreshTreeView()
        {
            var selectedVersion = _versionsListBox.SelectedItem as VersionState;

            _filesTreeView.BeginUpdate();
            _filesTreeView.Nodes.Clear();

            var sourceItems = selectedVersion.SourceItemChanges
                .Select(a => a.SourceItem)
                .ToList();

            foreach (var sourceItem in sourceItems)
            {
                List<StorageFile> storageFiles = BuildVersionFiles(sourceItem, selectedVersion);

                var sourceItemNode = new TreeNode(sourceItem.Target) { Tag = sourceItem };
                _filesTreeView.Nodes.Add(sourceItemNode);

                foreach (var storageFile in storageFiles)
                {
                    AddAsLeaves(sourceItemNode, sourceItem, storageFile);
                }
            }

            _filesTreeView.EndUpdate();
        }

        private List<StorageFile> BuildVersionFiles(SourceItem sourceItem, VersionState selectedVersion)
        {
            List<StorageFile> result = null;

            foreach (var versionState in _incrementalBackupState.VersionStates)
            {
                var sourceItemChanges = versionState.SourceItemChanges.FirstOrDefault(x => x.SourceItem == sourceItem);
                if (sourceItemChanges == null)
                {
                    result = null;
                }
                else
                {
                    if (result == null)
                    {
                        result = sourceItemChanges.CreatedFiles.ToList();
                    }
                    else
                    {
                        result.AddRange(sourceItemChanges.CreatedFiles);
                        foreach (var deletedFile in sourceItemChanges.DeletedFiles)
                        {
                            var itemToRemove = result.First(x => x.FileState.FileName == deletedFile);
                            result.Remove(itemToRemove);
                        }
                        foreach (var updatedFile in sourceItemChanges.UpdatedFiles)
                        {
                            var itemToRemove = result.First(x => x.FileState.FileName == updatedFile.FileState.FileName);
                            result.Remove(itemToRemove);

                            result.Add(updatedFile);
                        }
                    }
                }

                if (versionState == selectedVersion)
                    break;
            }

            return result
                .OrderBy(x => x.FileState.FileName)
                .ToList();
        }

        private void AddAsLeaves(TreeNode sourceItemNode, SourceItem sourceItem, StorageFile storageFile)
        {
            var sourceItemDir = sourceItem.IsFolder ?
                            sourceItem.Target :
                            Path.GetDirectoryName(sourceItem.Target);

            var sourceItemRelativeFileName = storageFile.FileState.FileName.Substring(sourceItemDir.Length);

            AddLeaf(sourceItemRelativeFileName, storageFile);
        }

        private void AddLeaf(string relativePath, StorageFile storageFile)
        {
            string[] names = relativePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            TreeNode node = null;
            for (int i = 0; i < names.Length; i++)
            {
                TreeNodeCollection nodes = node == null ? _filesTreeView.Nodes : node.Nodes;
                node = FindNode(nodes, names[i]);
                if (node == null)
                {
                    node = nodes.Add(names[i]);
                    if (i == names.Length - 1)
                        node.Tag = storageFile;
                }
            }
        }

        private TreeNode FindNode(TreeNodeCollection nodes, string p)
        {
            for (int i = 0; i < nodes.Count; i++)
                if (nodes[i].Text == p)
                    return nodes[i];
            return null;
        }

        private void RefreshChangesView()
		{
            var selectedVersion = _versionsListBox.SelectedItem as VersionState;

            _changesListBox.BeginUpdate();
            _changesListBox.Items.Clear();
            foreach (var sourceItemChanges in selectedVersion.SourceItemChanges)
            {
                _changesListBox.Items.Add($"{sourceItemChanges.SourceItem.Target}:");

                var updatedFiles = sourceItemChanges.UpdatedFiles
                    .OrderBy(x => x.StorageRelativeFileName)
                    .ToList();

                foreach (var updateFile in updatedFiles)
                {
                    _changesListBox.Items.Add($"Updated {updateFile.FileState.FileName} (size: {updateFile.FileState.Size}, modified at: {updateFile.FileState.LastWriteTimeUtc})");
                }

                var createdFiles = sourceItemChanges.CreatedFiles
                    .OrderBy(x => x.StorageRelativeFileName)
                    .ToList();

                foreach (var createdFile in createdFiles)
                {
                    _changesListBox.Items.Add($"Created {createdFile.StorageRelativeFileName}");
                }

                var deletedFiles = sourceItemChanges.DeletedFiles
                    .OrderBy(x => x)
                    .ToList();

                foreach (var deletedFile in deletedFiles)
                {
                    _changesListBox.Items.Add($"Deleted {deletedFile}");
                }
            }

            _changesListBox.EndUpdate();
        }
	}
}
