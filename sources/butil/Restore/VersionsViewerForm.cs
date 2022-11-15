using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BUtil.Configurator.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.State;
using BUtil.Core.Storages;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BUtil.RestorationMaster
{
	internal partial class VersionsViewerForm : Form
	{
		private readonly IncrementalBackupState _incrementalBackupState;
        private readonly string _backupLocation;

        public VersionsViewerForm(string backupLocation = null, IncrementalBackupState incrementalBackupState = null)
		{
			InitializeComponent();
			_incrementalBackupState = incrementalBackupState;
            _backupLocation = backupLocation;

            _versionsLabel.Text = BUtil.Configurator.Localization.Resources.SelectVersion;
            _dataLabel.Text = BUtil.Configurator.Localization.Resources.StateOfSourceItemsAtSelectedVersion;
            _changesLabel.Text = BUtil.Configurator.Localization.Resources.Changes;
            _toolStripStatusLabel.Text = BUtil.Configurator.Localization.Resources.ClickOnItemYouWantToRestoreAndOpenContextMenuByRightClick;
            recoverToolStripMenuItem.Text = BUtil.Configurator.Localization.Resources.Recover;
            this.Text = Resources.RestorationMaster;
        }

		private void OnLoad(object sender, System.EventArgs e)
		{
			_versionsListBox.BeginUpdate();

			var versionsDesc = _incrementalBackupState.VersionStates
				.OrderByDescending(x => x.BackupDateUtc)
				.ToList();

			_versionsListBox.DataSource = versionsDesc;
            _versionsListBox.DisplayMember = nameof(VersionState.BackupDateUtc);
            _versionsListBox.FormatString = "dd MMMM (dddd) HH:mm";
            _versionsListBox.FormatInfo = CultureInfo.CurrentUICulture;
            _versionsListBox.EndUpdate();

            _versionsListBox.SelectedItem = versionsDesc.First();
            this.OnVersionListChange(sender, e);
        }

		private void OnVersionListChange(object sender, System.EventArgs e)
		{
            RefreshChangesView();
            RefreshTreeView();
        }

        private const int _fileImageIndex = 0;
        private const int _folderImageIndex = 1;
        private const int _storageImageIndex = 2;

        private void RefreshTreeView()
        {
            var selectedVersion = _versionsListBox.SelectedItem as VersionState;

            _filesTreeView.BeginUpdate();
            _filesTreeView.Nodes.Clear();

            var sourceItems = selectedVersion.SourceItemChanges
                .Select(a => a.SourceItem)
                .OrderBy(x => x.Target)
                .ToList();

            foreach (var sourceItem in sourceItems)
            {
                var sourceItemNode = new TreeNode(sourceItem.Target)
                {
                    Tag = sourceItem,
                    ImageIndex = _storageImageIndex,
                    SelectedImageIndex = _storageImageIndex,
                };
                _filesTreeView.Nodes.Add(sourceItemNode);


                var storageFiles = BuildVersionFiles(sourceItem, selectedVersion);
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

        private void RefreshChangesView()
		{
            var selectedVersion = _versionsListBox.SelectedItem as VersionState;

            var builder = new StringBuilder();
            foreach (var sourceItemChanges in selectedVersion.SourceItemChanges
                .OrderBy(x => x.SourceItem.Target)
                .ToList())
            {
                if (!sourceItemChanges.CreatedFiles.Any() &&
                    !sourceItemChanges.UpdatedFiles.Any() &&
                    !sourceItemChanges.DeletedFiles.Any())
                    continue;

                builder.AppendLine(string.Format(BUtil.Configurator.Localization.Resources.SourceItemChanges, sourceItemChanges.SourceItem.Target));

                sourceItemChanges.UpdatedFiles
                    .OrderBy(x => x.FileState.FileName)
                    .ToList()
                    .ForEach(updateFile => builder.AppendLine(string.Format(BUtil.Configurator.Localization.Resources.UpdatedFile, updateFile.FileState.FileName)));

                sourceItemChanges.CreatedFiles
                    .OrderBy(x => x.FileState.FileName)
                    .ToList()
                    .ForEach(updateFile => builder.AppendLine(string.Format(BUtil.Configurator.Localization.Resources.AddedFile, updateFile.FileState.FileName)));

                sourceItemChanges.DeletedFiles
                    .OrderBy(x => x)
                    .ToList()
                    .ForEach(deletedFile => builder.AppendLine(string.Format(BUtil.Configurator.Localization.Resources.DeletedFile, deletedFile)));
            }

            _changesTextBox.Text = builder.ToString();
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

            var log = new StubLog();
            IStorageSettings storageSettings = new HddStorageSettings
            { 
                Name = string.Empty, 
                DestinationFolder = _backupLocation
            };

            var service = new IncrementalBackupFileService(log, storageSettings, ProgramOptionsManager.Default);
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            using var form = new ProgressForm(reportProgress =>
            {
                foreach (var storageFile in storageFiles)
                {
                    int percent = ((storageFiles.IndexOf(storageFile) + 1) * 100) / storageFiles.Count;
                    reportProgress(percent);
                    service.Download(token, sourceItem, storageFile, destinationFolder);
                }
            });
            form.ShowDialog();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _filesTreeView.SelectedNode = _filesTreeView.GetNodeAt(e.X, e.Y);
        }
    }
}
