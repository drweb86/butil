using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls;

public class FolderAndPortSectionViewModel : ObservableObject
{
    public FolderAndPortSectionViewModel(string? ftpServer, int port, string ftpsUser, string ftpsPassword, string folder, long durationMinutes)
    {
        FtpsServer = ftpServer;
        Port = port;
        FtpsUser = ftpsUser;
        FtpsPassword = ftpsPassword;
        Folder = folder;
        DurationMinutes = durationMinutes;
    }

    #region Labels
    public static string LeftMenu_What => Resources.LeftMenu_What;
    public static string Field_Folder => Resources.Field_Folder;
    public static string Field_Folder_Browse => Resources.Field_Folder_Browse;
    public static string Server_Field_Port => Resources.Server_Field_Port;
    public static string Server_Field_Address => Resources.Server_Field_Address;
    public static string User_Field => Resources.User_Field;
    public static string Password_Field => Resources.Password_Field;
    public static string DurationMinutes_Field => Resources.DurationMinutes_Field;

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

    #region FtpsUser

    private string? _ftpsUser;

    public string? FtpsUser
    {
        get
        {
            return _ftpsUser;
        }
        set
        {
            if (value == _ftpsUser)
                return;
            _ftpsUser = value;
            OnPropertyChanged(nameof(FtpsUser));
        }
    }

    #endregion


    #region FtpsPassword

    private string? _ftpsPassword;

    public string? FtpsPassword
    {
        get
        {
            return _ftpsPassword;
        }
        set
        {
            if (value == _ftpsPassword)
                return;
            _ftpsPassword = value;
            OnPropertyChanged(nameof(FtpsPassword));
        }
    }

    #endregion


    #region FtpsServer

    private string? _ftpsServer;

    public string? FtpsServer
    {
        get
        {
            return _ftpsServer;
        }
        set
        {
            if (value == _ftpsServer)
                return;
            _ftpsServer = value;
            OnPropertyChanged(nameof(FtpsServer));
        }
    }

    #endregion

    #region DurationMinutes

    private long _durationMinutes = BUtilServerModelOptionsV2.DefaultDuration;

    public long DurationMinutes
    {
        get
        {
            return _durationMinutes;
        }
        set
        {
            if (value == _durationMinutes)
                return;
            _durationMinutes = value;
            OnPropertyChanged(nameof(DurationMinutes));
        }
    }

    #endregion

}
