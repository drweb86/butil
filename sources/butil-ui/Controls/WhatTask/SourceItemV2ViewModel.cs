using Avalonia.Media.Imaging;
using Avalonia.Platform;
using BUtil.Core;
using BUtil.Core.Localization;
using System;
using System.Collections.ObjectModel;

namespace butil_ui.Controls;

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
        var src = !isFolder ? "/Assets/CrystalClear_FileNew.png" : "/Assets/www.wefunction.com_FunctionFreeIconSet_Folder_48.png";
        IconSource = LoadFromResource(new Uri("avares://butil-ui" + src));
    }

    public Bitmap? IconSource { get; }

    private static Bitmap LoadFromResource(Uri resourceUri)
    {
        return new Bitmap(AssetLoader.Open(resourceUri));
    }


    public string Target { get; set; }

    public bool IsFolder { get; set; }


    #region Commands
    public void SourceItemOpenInExplorerCommand()
    {
        var service = PlatformSpecificExperience.Instance.GetFolderService();
        if (IsFolder)
            service.OpenFolderInShell(Target);
        else
            service.OpenFileInShell(Target);
    }

    public void TaskDeleteCommand()
    {
        _items.Remove(this);
    }

    #endregion

    #region Labels
    public static string SourceItem_OpenInExplorer => Resources.SourceItem_OpenInExplorer;
    public static string Button_Remove => Resources.Button_Remove;

    #endregion
}
