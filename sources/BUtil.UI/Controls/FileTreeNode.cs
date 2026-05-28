using BUtil.Interop.Tasks;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.State;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BUtil.UI.Controls;

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

    #region IsExpanded

    private bool _isExpanded;

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (value == _isExpanded)
                return;
            _isExpanded = value;
            OnPropertyChanged(nameof(IsExpanded));
        }
    }

    #endregion

    public ObservableCollection<FileTreeNode> Nodes { get; } = [];
}
