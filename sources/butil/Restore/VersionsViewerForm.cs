using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Windows.Forms;
using BUtil.Configurator;
using BUtil.Configurator.Localization;
using BUtil.Configurator.Restore;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.RestorationMaster
{
    internal partial class VersionsViewerForm : Form
    {
        private readonly IncrementalBackupState _incrementalBackupState;
        private readonly IStorageSettings _storageSettings;

        public VersionsViewerForm(IStorageSettings storageSettings = null, IncrementalBackupState incrementalBackupState = null)
        {
            InitializeComponent();
            _incrementalBackupState = incrementalBackupState;
            _storageSettings = storageSettings;

            _selectVersionToolStripLabel.Text = BUtil.Configurator.Localization.Resources.SelectVersion;
            _selectedVersionToolStripLabel.Text = BUtil.Configurator.Localization.Resources.Files;
            _changesToolStripLabel.Text = BUtil.Configurator.Localization.Resources.Changes;
            _journalSelectedToolStripMenuItem.Text =
                _journalSelectedToolStripButton.Text =
                _treeViewJournalSelectedToolStripButton.Text =
                    _treeJournalSelectedToolStripMenuItem.Text =
                        BUtil.Configurator.Localization.Resources.JournalSelected;
            _toolStripStatusLabel.Text = BUtil.Configurator.Localization.Resources.ClickOnItemYouWantToRestoreAndOpenContextMenuByRightClick;
            recoverToolStripMenuItem.Text = _recoverToolStripButton.Text = BUtil.Configurator.Localization.Resources.RecoverSelected;
            this.Text = Resources.RestorationMaster;
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            _versionsListView.BeginUpdate();

            var versionsDesc = _incrementalBackupState.VersionStates
                .OrderByDescending(x => x.BackupDateUtc)
                .ToList();

            foreach (var version in versionsDesc)
            {
                var totalSize = GetSizeOfVersion(version);
                var title = $"{version.BackupDateUtc} ({BytesToString(totalSize)})";
                _versionsListView.Items.Add(new ListViewItem(title) { Tag = version, ImageIndex = 6 });
            }

            _versionsListView.EndUpdate();

            _versionsListView.Items[0].Selected = true;

            var storageSize = _incrementalBackupState.VersionStates
                .SelectMany(x => x.SourceItemChanges)
                .SelectMany(x =>
                {
                    var storageFiles = new List<StorageFile>();
                    storageFiles.AddRange(x.UpdatedFiles);
                    storageFiles.AddRange(x.CreatedFiles);
                    return storageFiles;
                })
                .GroupBy(x => x.StorageFileName)
                .Select(x => x.First().StorageFileNameSize)
                .Sum();
            _storageToolStripLabel.Text = string.Format(BUtil.Configurator.Localization.Resources.StorageSize, BytesToString(storageSize));
        }

        private static long GetSizeOfVersion(VersionState version)
        {
            var versionFolder = SourceItemHelper.GetVersionFolder(version.BackupDateUtc);
            return version.SourceItemChanges
                .SelectMany(x =>
                {
                    var storageFiles = new List<StorageFile>();
                    storageFiles.AddRange(x.UpdatedFiles);
                    storageFiles.AddRange(x.CreatedFiles);
                    return storageFiles;
                })
                .Where(x => x.StorageRelativeFileName.StartsWith(versionFolder))
                .GroupBy(x => x.StorageFileName)
                .Select(x => x.First().StorageFileNameSize)
                .Sum();
        }

        private static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
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

        private static List<Tuple<SourceItem, List<StorageFile>>> GetTreeViewFiles(
            IncrementalBackupState state,
            VersionState selectedVersion)
        {
            var result = new List<Tuple<SourceItem, List<StorageFile>>>();

            var sourceItems = selectedVersion.SourceItemChanges
                .Select(a => a.SourceItem)
                .OrderBy(x => x.Target)
                .ToList();

            foreach (var sourceItem in sourceItems)
            {
                result.Add(new Tuple<SourceItem, List<StorageFile>>(
                    sourceItem,
                    BuildVersionFiles(state, sourceItem, selectedVersion)
                    ));
            }

            return result;
        }

        private void RefreshTreeView(List<Tuple<SourceItem, List<StorageFile>>> treeViewFiles)
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

        private static List<StorageFile> BuildVersionFiles(IncrementalBackupState state, SourceItem sourceItem, VersionState selectedVersion)
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

        private void AddAsLeaves(TreeNode sourceItemNode, SourceItem sourceItem, StorageFile storageFile)
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
        private static IEnumerable<Tuple<ChangeState, string>> GetChangesViewItems(VersionState state)
        {
            var result = new List<Tuple<ChangeState, string>>();

            foreach (var sourceItemChanges in state.SourceItemChanges
                .OrderBy(x => x.SourceItem.Target)
                .ToList())
            {
                if (!sourceItemChanges.CreatedFiles.Any() &&
                    !sourceItemChanges.UpdatedFiles.Any() &&
                    !sourceItemChanges.DeletedFiles.Any())
                    continue;

                sourceItemChanges.UpdatedFiles
                    .OrderBy(x => x.FileState.FileName)
                    .ToList()
                    .ForEach(updateFile => result.Add(new Tuple<ChangeState, string>(ChangeState.Updated, updateFile.FileState.FileName)));

                sourceItemChanges.CreatedFiles
                    .OrderBy(x => x.FileState.FileName)
                    .ToList()
                    .ForEach(updateFile => result.Add(new Tuple<ChangeState, string>(ChangeState.Created, updateFile.FileState.FileName)));

                sourceItemChanges.DeletedFiles
                    .OrderBy(x => x)
                    .ToList()
                    .ForEach(deletedFile => result.Add(new Tuple<ChangeState, string>(ChangeState.Deleted, deletedFile)));
            }
            return result;
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

                Recover(tags, destinationFolder, rootNode.Tag as SourceItem);
            }
        }

        private void Recover(List<StorageFile> storageFiles, string destinationFolder, SourceItem sourceItem)
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

            Messages.ShowInformationBox(BUtil.Configurator.Localization.Resources.RestorationIsCompleted);
            services.Dispose();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _filesTreeView.SelectedNode = _filesTreeView.GetNodeAt(e.X, e.Y);
        }

        private void OnVersionChanged(object sender, EventArgs e)
        {
            if (_versionsListView.SelectedItems.Count != 1)
            {
                return;
            }
            var selectedVersion = _versionsListView.SelectedItems[0].Tag as VersionState;
            IEnumerable<Tuple<ChangeState, string>> changes = null;
            List<Tuple<SourceItem, List<StorageFile>>> treeViewFiles = null;
            using var progressForm = new ProgressForm(reportProgress =>
            {
                reportProgress(15);
                changes = GetChangesViewItems(selectedVersion);
                reportProgress(65);
                treeViewFiles = GetTreeViewFiles(_incrementalBackupState, selectedVersion);

            });
            progressForm.ShowDialog();

            _selectedVersionToolStripLabel.Text = string.Format("{0} {1}", BUtil.Configurator.Localization.Resources.Files, selectedVersion.BackupDateUtc);
            _changesToolStripLabel.Text = string.Format("{0} {1}", BUtil.Configurator.Localization.Resources.Changes, selectedVersion.BackupDateUtc);
            RefreshChanges(changes);
            RefreshTreeView(treeViewFiles);
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
                ! (_filesTreeView.SelectedNode.Tag is StorageFile))
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
