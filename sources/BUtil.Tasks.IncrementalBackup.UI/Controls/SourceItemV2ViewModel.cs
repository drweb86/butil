using BUtil.Core;
using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;

namespace BUtil.Tasks.IncrementalBackup.UI.Controls;

public class SourceItemV2ViewModel
{
    private readonly ObservableCollection<SourceItemV2ViewModel> _items;

    public Guid Id { get; }

    public SourceItemV2ViewModel(Guid id, string target, bool isFolder,
        ObservableCollection<SourceItemV2ViewModel> items)
    {
        Id = id;
        Target = target;
        IsFolder = isFolder;
        _items = items;
        SourceItemOpenInExplorerCommand = new RelayCommand(OpenInExplorer, () => CanOpenLink);
        TaskDeleteCommand = new RelayCommand(Delete);
    }

    public string Target { get; set; }

    public bool IsFolder { get; set; }
    public bool CanOpenLink { get; } = PlatformSpecificExperience.Instance.SupportManager.CanOpenLink;

    #region Commands
    public IRelayCommand SourceItemOpenInExplorerCommand { get; }

    public IRelayCommand TaskDeleteCommand { get; }

    private void OpenInExplorer()
    {
        var service = PlatformSpecificExperience.Instance.GetFolderService();
        if (IsFolder)
            service.OpenFolderInShell(Target);
        else
            service.OpenFileInShell(Target);
    }

    private void Delete()
    {
        _items.Remove(this);
    }

    #endregion

    #region Labels
    public static string SourceItem_OpenInExplorer => Resources.SourceItem_OpenInExplorer;
    public static string Button_Remove => Resources.Button_Remove;

    #endregion
}
