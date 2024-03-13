using Avalonia.Media;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.State;
using BUtil.Core.TasksTree.IncrementalModel;
using butil_ui.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace butil_ui.Controls;

public class VersionsListViewModel : ObservableObject
{
    #region Labels

    public string Field_Version => Resources.Field_Version;
    public string BackupVersion_Changes_Title => Resources.BackupVersion_Changes_Title;
    public string BackupVersion_Files_Title => Resources.BackupVersion_Files_Title;
    public string Task_Restore => Resources.Task_Restore;
    public string BackupVersion_Viewer_Help => Resources.BackupVersion_Viewer_Help;

    #endregion

    #region SelectedNode

    private FileTreeNode? _selectedNode;

    public FileTreeNode? SelectedNode
    {
        get
        {
            return _selectedNode;
        }
        set
        {
            if (value == _selectedNode)
                return;
            if (_selectedNode != null)
                _selectedNode.IsSelected = false;
            _selectedNode = value;
            if (_selectedNode != null)
                _selectedNode.IsSelected = true;

            SelectedFileIsVisible = true;
            if (_selectedNode != null && _selectedNode.StorageFile != null)
            {
                SelectedFileIsVisible = true;
                BackupVersion_FileVersion_Title = string.Format(BUtil.Core.Localization.Resources.BackupVersion_FileVersion_Title, System.IO.Path.GetFileName(_selectedNode.StorageFile.FileState.FileName));
                InitBlameForSelectedFile();
            }
            else
            {
                SelectedFileIsVisible = false;
                BackupVersion_FileVersion_Title = null;
            }
            OnPropertyChanged(nameof(SelectedNode));
        }
    }

    #endregion

    #region BlameViewItems

    private ObservableCollection<BlameViewItem> _blameViewItems = new();

    public ObservableCollection<BlameViewItem> BlameViewItems
    {
        get
        {
            return _blameViewItems;
        }
        set
        {
            if (value == _blameViewItems)
                return;
            _blameViewItems = value;
            OnPropertyChanged(nameof(BlameViewItems));
        }
    }

    #endregion

    #region SelectedFileIsVisible

    private bool _selectedFileIsVisible;

    public bool SelectedFileIsVisible
    {
        get
        {
            return _selectedFileIsVisible;
        }
        set
        {
            if (value == _selectedFileIsVisible)
                return;
            _selectedFileIsVisible = value;
            OnPropertyChanged(nameof(SelectedFileIsVisible));
        }
    }

    #endregion

    #region BackupVersion_FileVersion_Title

    private string? _backupVersion_FileVersion_Title;

    public string? BackupVersion_FileVersion_Title
    {
        get
        {
            return _backupVersion_FileVersion_Title;
        }
        set
        {
            if (value == _backupVersion_FileVersion_Title)
                return;
            _backupVersion_FileVersion_Title = value;
            OnPropertyChanged(nameof(BackupVersion_FileVersion_Title));
        }
    }

    #endregion

    #region SelectedVersion

    private VersionViewItem _selectedVersion = new VersionViewItem(new VersionState());

    public VersionViewItem SelectedVersion
    {
        get
        {
            return _selectedVersion;
        }
        set
        {
            if (value == _selectedVersion)
                return;
            _selectedVersion = value;
            OnPropertyChanged(nameof(SelectedVersion));
            if (value != null)
                OnVersionChanged();
        }
    }

    #endregion

    #region Versions

    private ObservableCollection<VersionViewItem> _versions = new();

    public ObservableCollection<VersionViewItem> Versions
    {
        get
        {
            return _versions;
        }
        set
        {
            if (value == _versions)
                return;
            _versions = value;
            OnPropertyChanged(nameof(Versions));
        }
    }

    #endregion

    #region StorageSize

    private string _storageSize = string.Empty;

    public string StorageSize
    {
        get
        {
            return _storageSize;
        }
        set
        {
            if (value == _storageSize)
                return;
            _storageSize = value;
            OnPropertyChanged(nameof(StorageSize));
        }
    }

    #endregion

    #region FileChangeViewItems

    private ObservableCollection<FileChangeViewItem> _fileChangeViewItems = new();

    public ObservableCollection<FileChangeViewItem> FileChangeViewItems
    {
        get
        {
            return _fileChangeViewItems;
        }
        set
        {
            if (value == _fileChangeViewItems)
                return;
            _fileChangeViewItems = value;
            OnPropertyChanged(nameof(FileChangeViewItems));
        }
    }

    #endregion

    #region Files

    private ObservableCollection<FileTreeNode> _nodes = new();

    public ObservableCollection<FileTreeNode> Nodes
    {
        get
        {
            return _nodes;
        }
        set
        {
            if (value == _nodes)
                return;
            _nodes = value;
            OnPropertyChanged(nameof(Nodes));
        }
    }

    #endregion

    public SolidColorBrush HeaderBackground { get; } = ColorPalette.GetBrush(SemanticColor.HeaderBackground);

    public ProgressTaskViewModel ProgressTaskViewModel { get; } = new ProgressTaskViewModel();

    private IncrementalBackupState _state;
    private IStorageSettingsV2 _storageOptions;
    public void Initialize(IncrementalBackupState state, IStorageSettingsV2 storageOptions)
    {
        _state = state;
        _storageOptions = storageOptions;
        Versions = new ObservableCollection<VersionViewItem>(state.VersionStates
            .OrderByDescending(x => x.BackupDateUtc)
            .Select(x => new VersionViewItem(x))
            .ToList());

        SelectedVersion = Versions.First();

        var storageSize = state.VersionStates
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

        StorageSize = string.Format(BUtil.Core.Localization.Resources.BackupVersion_Storage_TitleWithSize, SizeHelper.BytesToString(storageSize));
    }

    private void OnVersionChanged()
    {
        ProgressTaskViewModel.Activate(async reportProgress =>
        {
            SelectedFileIsVisible = false;
            reportProgress(0);
            ProgressTaskViewModel.IsVisible = true;
            var changes = GetChangesViewItems(SelectedVersion.Version);
            reportProgress(25);
            var treeViewFiles = GetTreeViewFiles(_state, SelectedVersion.Version);
            reportProgress(45);
            RefreshChanges(changes);
            reportProgress(85);
            RefreshTreeView(treeViewFiles);
            reportProgress(100);
            await Task.Delay(3000);
            ProgressTaskViewModel.IsVisible = false;
        });
    }

    private void RefreshChanges(IEnumerable<Tuple<ChangeState, string>> changes)
    {
        FileChangeViewItems.Clear();

        changes
            .Select(x => new FileChangeViewItem(x.Item2, x.Item1))
            .ToList()
            .ForEach(FileChangeViewItems.Add);
    }


    private static List<StorageFile> BuildVersionFiles(IncrementalBackupState state, SourceItemV2 sourceItem, VersionState selectedVersion)
    {
        List<StorageFile>? result = null;

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
        var newNodes = new ObservableCollection<FileTreeNode>();

        foreach (var treeViewFileTuple in treeViewFiles)
        {
            var sourceItem = treeViewFileTuple.Item1;

            var sourceItemNode = new FileTreeNode(sourceItem, null, null);
            newNodes.Add(sourceItemNode);

            var storageFiles = treeViewFileTuple.Item2;
            foreach (var storageFile in storageFiles)
            {
                AddAsLeaves(sourceItemNode, sourceItem, storageFile);
            }
        }

        Nodes = newNodes;
        // TODO:
        //if (Nodes.Count == 1)
        //    Nodes[0].Expand();
    }

    private void AddLeaf(string relativePath, StorageFile storageFile, FileTreeNode sourceItemNode, SourceItemV2 sourceItem)
    {
        string[] names = relativePath.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
        FileTreeNode? node = null;
        for (int i = 0; i < names.Length; i++)
        {
            var nodes = node == null ? sourceItemNode.Nodes : node.Nodes;
            node = FindNode(nodes, names[i]);
            if (node == null)
            {
                node = i == names.Length - 1 ? new FileTreeNode(sourceItem, null, storageFile) : new FileTreeNode(sourceItem, names[i], null);
                nodes.Add(node);
            }
        }
    }


    private FileTreeNode? FindNode(ObservableCollection<FileTreeNode> nodes, string p)
    {
        for (int i = 0; i < nodes.Count; i++)
            if (nodes[i].Target == p)
                return nodes[i];
        return null;
    }

    private void AddAsLeaves(FileTreeNode sourceItemNode, SourceItemV2 sourceItem, StorageFile storageFile)
    {
        var sourceItemDir = sourceItem.IsFolder ?
                        sourceItem.Target :
                        System.IO.Path.GetDirectoryName(sourceItem.Target);

        var sourceItemRelativeFileName = storageFile.FileState.FileName.Substring(sourceItemDir.Length);

        AddLeaf(sourceItemRelativeFileName, storageFile, sourceItemNode, sourceItem);
    }

    #region Commands

    public void RecoverTo(string destinationFolder)
    {
        if (SelectedNode == null)
            return;

        var storageFiles = GetChildren(SelectedNode)
            .Where(x => x.StorageFile != null)
            .Select(x => x.StorageFile ?? throw new NullReferenceException())
            .ToList();

        if (SelectedNode.StorageFile != null)
            storageFiles.Add(SelectedNode.StorageFile);

        this.ProgressTaskViewModel.Progress = 0;
        this.ProgressTaskViewModel.IsVisible = true;
        this.ProgressTaskViewModel.Activate(async progress =>
        {
            if (!storageFiles.Any())
                return;

            var commonServicesIoc = new CommonServicesIoc();
            using var services = new BUtil.Core.TasksTree.IncrementalModel.StorageSpecificServicesIoc(new StubLog(),
                _storageOptions, commonServicesIoc.HashService);
            foreach (var storageFile in storageFiles)
            {
                int percent = ((storageFiles.IndexOf(storageFile) + 1) * 100) / storageFiles.Count;
                progress(percent);
                services.IncrementalBackupFileService.Download(SelectedNode.SourceItem, storageFile, destinationFolder);
            }

            await Task.Delay(3000);
            this.ProgressTaskViewModel.IsVisible = false;
        });
    }

    private IEnumerable<FileTreeNode> GetChildren(FileTreeNode Parent)
    {
        return Parent.Nodes.Cast<FileTreeNode>().Concat(
               Parent.Nodes.Cast<FileTreeNode>().SelectMany(GetChildren));
    }

    public void InitBlameForSelectedFile()
    {
        var items = new ObservableCollection<BlameViewItem>();
        var path = SelectedNode?.StorageFile?.FileState.FileName ?? throw new NullReferenceException();

        var descendingVersions = _state.VersionStates
            .OrderByDescending(x => x.BackupDateUtc)
            .ToList();

        foreach (var versionState in descendingVersions)
        {
            foreach (var sourceItemChanges in versionState.SourceItemChanges)
            {
                foreach (var deletedFile in sourceItemChanges.DeletedFiles)
                {
                    if (deletedFile == path)
                    {
                        items.Add(new BlameViewItem(versionState, ChangeState.Deleted));
                        break;
                    }
                }

                foreach (var createdFile in sourceItemChanges.CreatedFiles)
                {
                    if (createdFile.FileState.FileName == path)
                    {
                        items.Add(new BlameViewItem(versionState, ChangeState.Created));
                        break;
                    }
                }

                foreach (var updatedFile in sourceItemChanges.UpdatedFiles)
                {
                    if (updatedFile.FileState.FileName == path)
                    {
                        items.Add(new BlameViewItem(versionState, ChangeState.Updated));
                        break;
                    }
                }
            }
        }

        BlameViewItems = items;
    }

    #endregion
}

public class BlameViewItem
{
    public BlameViewItem(VersionState versionState, ChangeState state)
    {
        Title = versionState.BackupDateUtc.ToString();
        switch (state)
        {
            case ChangeState.Created:
                ImageSource = "/Assets/VC-Created.png";
                break;
            case ChangeState.Deleted:
                ImageSource = "/Assets/VC-Deleted.png";
                break;
            case ChangeState.Updated:
                ImageSource = "/Assets/VC-Updated.png";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state));
        }

    }
    public string Title { get; }
    public string ImageSource { get; }
}
