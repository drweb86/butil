using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.State;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace butil_ui.Controls;

public class FileTreeNode : ObservableObject
{
    public FileTreeNode(SourceItemV2 sourceItem, string? virtualFolder, StorageFile? storageFile)
    {
        SourceItem = sourceItem;
        VirtualFolder = virtualFolder;
        StorageFile = storageFile;
        if (virtualFolder != null)
        {
            ImageSource = "/Assets/www.wefunction.com_FunctionFreeIconSet_Folder_48.png";
            Target = virtualFolder;
        }
        else if (storageFile != null)
        {
            ImageSource = "/Assets/CrystalClear_FileNew.png";
            Target = System.IO.Path.GetFileName(storageFile.FileState.FileName) ?? string.Empty;
        }
        else
        {
            ImageSource = "/Assets/CrystalProject_EveraldoCoelho_SourceItems48x48.png";
            Target = sourceItem.Target;
        }
    }
    public string ImageSource { get; }
    public string Target { get; }
    public SourceItemV2 SourceItem { get; }
    public StorageFile? StorageFile { get; }
    public string? VirtualFolder { get; }

    #region IsSelected

    private bool _isSelected;

    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            if (value == _isSelected)
                return;
            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
        }
    }

    #endregion

    public ObservableCollection<FileTreeNode> Nodes { get; } = new();
}
/*
using BUtil.Core.Localization;
using BUtil.Core.State;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace BUtil.Configurator.Restore
{
public partial class BlameForm : Form
{
    private Action<DateTime> _selectVersion;

    public BlameForm()
    {
        InitializeComponent();

        this._closeButton.Text = Resources.Button_Close;
        _versionsListToolStripLabel.Text = BUtil.Core.Localization.Resources.BackupVersion_Versions_Title;
        _openSelectedVersionToolStripButton.Text =
            _openSelectedVersionToolStripMenuItem.Text = BUtil.Core.Localization.Resources.BackupVersion_Select;
    }

    public void Init(
        IncrementalBackupState incrementalBackupState,
        string fileFullPath,
        Action<DateTime> selectVersion)
    {
        _selectVersion = selectVersion;
        this.Text = string.Format(BUtil.Core.Localization.Resources.BackupVersion_FileVersion_Title, fileFullPath);

        this._versionsListView.SuspendLayout();
        this._versionsListView.Items.Clear();

        var descendingVersions = incrementalBackupState.VersionStates
            .OrderByDescending(x => x.BackupDateUtc)
            .ToList();

        foreach (var versionState in descendingVersions)
        {
            foreach (var sourceItemChanges in versionState.SourceItemChanges)
            {
                foreach (var deletedFile in sourceItemChanges.DeletedFiles)
                {
                    if (deletedFile == fileFullPath)
                    {
                        _versionsListView.Items.Add(new ListViewItem(versionState.BackupDateUtc.ToString(), 1) { Tag = versionState.BackupDateUtc });
                        break;
                    }
                }

                foreach (var createdFile in sourceItemChanges.CreatedFiles)
                {
                    if (createdFile.FileState.FileName == fileFullPath)
                    {
                        _versionsListView.Items.Add(new ListViewItem(versionState.BackupDateUtc.ToString(), 0) { Tag = versionState.BackupDateUtc });
                        break;
                    }
                }

                foreach (var updatedFile in sourceItemChanges.UpdatedFiles)
                {
                    if (updatedFile.FileState.FileName == fileFullPath)
                    {
                        _versionsListView.Items.Add(new ListViewItem(versionState.BackupDateUtc.ToString(), 2) { Tag = versionState.BackupDateUtc });
                        break;
                    }
                }
            }
        }

        this._versionsListView.ResumeLayout();
    }

    private void OnClose(object sender, EventArgs e)
    {
        this.Close();
    }

    private void OnOpenSelectedVersion(object sender, EventArgs e)
    {
        if (_versionsListView.SelectedItems.Count != 1)
            return;

        _selectVersion((DateTime)_versionsListView.SelectedItems[0].Tag);
    }
}
}
*/