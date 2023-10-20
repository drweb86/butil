using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BUtil.Core.Localization;
using BUtil.Configurator.Restore;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.RestorationMaster
{
    internal partial class VersionsViewerForm : Form
    {
        private readonly IncrementalBackupState _incrementalBackupState;
        private readonly IStorageSettingsV2 _storageSettings;

        public VersionsViewerForm(IStorageSettingsV2 storageSettings = null, IncrementalBackupState incrementalBackupState = null)
        {
            InitializeComponent();
            _incrementalBackupState = incrementalBackupState;
            _storageSettings = storageSettings;

            _selectedVersionToolStripLabel.Text = BUtil.Core.Localization.Resources.BackupVersion_Files_Title;
            _changesToolStripLabel.Text = BUtil.Core.Localization.Resources.BackupVersion_Changes_Title;
            _journalSelectedToolStripMenuItem.Text =
                _journalSelectedToolStripButton.Text =
                _treeViewJournalSelectedToolStripButton.Text =
                    _treeJournalSelectedToolStripMenuItem.Text =
                        BUtil.Core.Localization.Resources.BackupVersion_FileVersions_Open;
            _toolStripStatusLabel.Text = BUtil.Core.Localization.Resources.BackupVersion_Viewer_Help;
            recoverToolStripMenuItem.Text = _recoverToolStripButton.Text = BUtil.Core.Localization.Resources.Task_Restore;
            this.Text = Resources.Task_Restore;
        }
        
        private void RefreshChanges(IEnumerable<Tuple<ChangeState, string>> changes)
        {
            _changesListView.BeginUpdate();
            _changesListView.Items.Clear();

            foreach (var item in changes)
            {
                var listViewItem = new ListViewItem(item.Item2);
                listViewItem.ImageIndex = (int)item.Item1;
                listViewItem.Tag = item.Item2;
                _changesListView.Items.Add(listViewItem);
            }

            _changesListView.EndUpdate();
        }

        private const int _fileImageIndex = 0;
        private const int _folderImageIndex = 1;
        private const int _storageImageIndex = 2;

        private static List<Tuple<SourceItemV2, List<StorageFile>>> GetTreeViewFiles(
            IncrementalBackupState state,
            VersionState selectedVersion)
        {
            var result = new List<Tuple<SourceItemV2, List<StorageFile>>>();

            var sourceItems = selectedVersion.SourceItemChanges
                .Select(a => a.SourceItem)
                .OrderBy(x => x.Target)
                .ToList();

            foreach (var sourceItem in sourceItems)
            {
                result.Add(new Tuple<SourceItemV2, List<StorageFile>>(
                    sourceItem,
                    BuildVersionFiles(state, sourceItem, selectedVersion)
                    ));
            }

            return result;
        }

        private void RefreshTreeView(List<Tuple<SourceItemV2, List<StorageFile>>> treeViewFiles)
        {
            _filesTreeView.BeginUpdate();
            _filesTreeView.Nodes.Clear();

            foreach (var treeViewFileTuple in treeViewFiles)
            {
                var sourceItem = treeViewFileTuple.Item1;

                var sourceItemNode = new TreeNode(sourceItem.Target)
                {
                    Tag = sourceItem,
                    ImageIndex = _storageImageIndex,
                    SelectedImageIndex = _storageImageIndex,
                };
                _filesTreeView.Nodes.Add(sourceItemNode);

                var storageFiles = treeViewFileTuple.Item2;
                foreach (var storageFile in storageFiles)
                {
                    AddAsLeaves(sourceItemNode, sourceItem, storageFile);
                }
            }

            if (_filesTreeView.Nodes.Count == 1)
                _filesTreeView.Nodes[0].Expand();

            _filesTreeView.EndUpdate();
        }

        private static List<StorageFile> BuildVersionFiles(IncrementalBackupState state, SourceItemV2 sourceItem, VersionState selectedVersion)
        {
            List<StorageFile> result = null;

            foreach (var versionState in state.VersionStates)
            {
                var sourceItemChanges = versionState.SourceItemChanges.FirstOrDefault(x => x.SourceItem.CompareTo(sourceItem));
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

        private void AddAsLeaves(TreeNode sourceItemNode, SourceItemV2 sourceItem, StorageFile storageFile)
        {
            var sourceItemDir = sourceItem.IsFolder ?
                            sourceItem.Target :
                            Path.GetDirectoryName(sourceItem.Target);

            var sourceItemRelativeFileName = storageFile.FileState.FileName.Substring(sourceItemDir.Length);

            AddLeaf(sourceItemRelativeFileName, storageFile, sourceItemNode);
        }

        private void AddLeaf(string relativePath, StorageFile storageFile, TreeNode sourceItemNode)
        {
            string[] names = relativePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            TreeNode node = null;
            for (int i = 0; i < names.Length; i++)
            {
                TreeNodeCollection nodes = node == null ? sourceItemNode.Nodes : node.Nodes;
                node = FindNode(nodes, names[i]);
                if (node == null)
                {
                    node = nodes.Add(names[i]);
                    if (i == names.Length - 1)
                    {
                        node.ImageIndex = _fileImageIndex;
                        node.SelectedImageIndex = _fileImageIndex;
                        node.Tag = storageFile;
                    }
                    else
                    {
                        node.ImageIndex = _folderImageIndex;
                        node.SelectedImageIndex = _folderImageIndex;
                    }
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

        private enum ChangeState
        {
            Created = 3, // match image indexes
            Deleted = 4,
            Updated = 5
        }


        private IEnumerable<TreeNode> GetChildren(TreeNode Parent)
        {
            return Parent.Nodes.Cast<TreeNode>().Concat(
                   Parent.Nodes.Cast<TreeNode>().SelectMany(GetChildren));
        }

        private void OnRecover(object sender, EventArgs e)
        {
            if (_filesTreeView.SelectedNode == null)
                return;

            var storageFiles = new List<StorageFile>();

            if (_fbdialog.ShowDialog() == DialogResult.OK)
            {
                var destinationFolder = _fbdialog.SelectedPath;
                //D:\reco-1
                var tags = GetChildren(_filesTreeView.SelectedNode)
                    .Select(x => x.Tag as StorageFile)
                    .Where(x => x != null)
                    .ToList();
                if (_filesTreeView.SelectedNode.Tag != null &&
                    _filesTreeView.SelectedNode.Tag is StorageFile)
                    tags.Add(_filesTreeView.SelectedNode.Tag as StorageFile);

                TreeNode rootNode = _filesTreeView.SelectedNode;
                while (rootNode.Parent != null)
                {
                    rootNode = rootNode.Parent;
                }

                Recover(tags, destinationFolder, rootNode.Tag as SourceItemV2);
            }
        }

        private void Recover(List<StorageFile> storageFiles, string destinationFolder, SourceItemV2 sourceItem)
        {
            if (!storageFiles.Any())
                return;

            var commonServicesIoc = new CommonServicesIoc();
            var services = new Core.TasksTree.IncrementalModel.StorageSpecificServicesIoc(new StubLog(), _storageSettings, commonServicesIoc.HashService);
            using var form = new ProgressForm(reportProgress =>
            {
                foreach (var storageFile in storageFiles)
                {
                    int percent = ((storageFiles.IndexOf(storageFile) + 1) * 100) / storageFiles.Count;
                    reportProgress(percent);
                    services.IncrementalBackupFileService.Download(sourceItem, storageFile, destinationFolder);
                }
            });
            form.ShowDialog();

            //Messages.ShowInformationBox(Resources.Task_Status_Succesfull);
            services.Dispose();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _filesTreeView.SelectedNode = _filesTreeView.GetNodeAt(e.X, e.Y);
        }

        private void OnJournalSelectedChangesView(object sender, EventArgs e)
        {
            if (_changesListView.SelectedItems.Count != 1)
                return;

            var blameForm = new BlameForm();
            blameForm.Init(_incrementalBackupState, _changesListView.SelectedItems[0].Tag.ToString(),
                versionDate =>
                {
                    _versionsListView.BeginUpdate();
                    _versionsListView.SelectedItems.Clear();
                    foreach (ListViewItem item in _versionsListView.Items)
                    {
                        if (((VersionState)item.Tag).BackupDateUtc == versionDate)
                            item.Selected = true;
                    }
                    _versionsListView.EndUpdate();
                }
                );
            blameForm.Show();
        }

        private void OnTreeJournalSelected(object sender, EventArgs e)
        {
            if (_filesTreeView.SelectedNode == null ||
                !(_filesTreeView.SelectedNode.Tag is StorageFile))
                return;

            var storageFile = (StorageFile)_filesTreeView.SelectedNode.Tag;

            var blameForm = new BlameForm();
            blameForm.Init(_incrementalBackupState, storageFile.FileState.FileName,
                versionDate =>
                {
                    _versionsListView.BeginUpdate();
                    _versionsListView.SelectedItems.Clear();
                    foreach (ListViewItem item in _versionsListView.Items)
                    {
                        if (((VersionState)item.Tag).BackupDateUtc == versionDate)
                            item.Selected = true;
                    }
                    _versionsListView.EndUpdate();
                }
                );
            blameForm.Show();
        }
    }
}
