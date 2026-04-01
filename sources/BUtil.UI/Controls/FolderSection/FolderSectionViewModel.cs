using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls;

public class FolderSectionViewModel : ObservableObject
{
    public FolderSectionViewModel(string folder)
    {
        Folder = folder;
    }

    #region Labels
    public static string LeftMenu_What => Resources.LeftMenu_What;
    public static string Field_Folder => Resources.Field_Folder;
    public static string Field_Folder_Browse => Resources.Field_Folder_Browse;

    #endregion

    #region Folder

    private string _folder = string.Empty;

    public string Folder
    {
        get
        {
            return _folder;
        }
        set
        {
            if (value == _folder)
                return;
            _folder = value;
            OnPropertyChanged(nameof(Folder));
        }
    }

    #endregion
}
