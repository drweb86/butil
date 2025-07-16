using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls;

public class FolderAndPortSectionViewModel : ObservableObject
{
    public FolderAndPortSectionViewModel(string folder, int port)
    {
        Folder = folder;
        Port = port;
    }

    #region Labels
    public static string LeftMenu_What => Resources.LeftMenu_What;
    public static string Field_Folder => Resources.Field_Folder;
    public static string Field_Folder_Browse => Resources.Field_Folder_Browse;
    public static string Server_Field_Port => Resources.Server_Field_Port;

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

    #region Port

    private int _port = 666;

    public int Port
    {
        get
        {
            return _port;
        }
        set
        {
            if (value == _port)
                return;
            _port = value;
            OnPropertyChanged(nameof(Port));
        }
    }

    #endregion
}
