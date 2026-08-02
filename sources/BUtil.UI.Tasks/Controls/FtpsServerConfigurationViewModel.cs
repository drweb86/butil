using BUtil.Core;
using BUtil.Core.Localization;
using BUtil.Tasks.BUtilServer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public class FtpsServerConfigurationViewModel : ObservableObject
{
    public FtpsServerConfigurationViewModel(
        int port,
        string ftpsUser,
        string ftpsPassword,
        string folder,
        long durationMinutes,
        BUtilServerFolderAccess folderAccess = BUtilServerModelOptionsV2.DefaultFolderAccess,
        bool isExpanded = false)
    {
        IsExpanded = isExpanded;
        Port = port;
        FtpsUser = ftpsUser;
        FtpsPassword = ftpsPassword;
        Folder = folder;
        DurationMinutes = durationMinutes;
        FolderAccess = folderAccess;
        SelectedFolderAccess = ToDisplay(folderAccess);

        BrowseFolderCommand = new AsyncRelayCommand(async () =>
        {
            if (BrowseFolderAsync != null)
                await BrowseFolderAsync();
        });
    }

    public bool IsExpanded { get; }

    public Func<Task>? BrowseFolderAsync { get; set; }
    public IAsyncRelayCommand BrowseFolderCommand { get; }

    public long PortMinimum => PlatformSpecificExperience.Instance.MinimumListenerPort;

    public IReadOnlyList<string> FolderAccessOptions { get; } =
    [
        Resources.FolderAccess_Field_ReadWrite,
        Resources.FolderAccess_Field_ReadOnly
    ];

    #region Labels
    public static string FtpsServerConfiguration_Title => Resources.FtpsServerConfiguration_Title;
    public static string FtpsServerTask_Help => Resources.FtpsServerTask_Help;
    public static string Field_Folder => Resources.Field_Folder;
    public static string Field_Folder_Browse => Resources.Field_Folder_Browse;
    public static string FolderAccess_Field => Resources.FolderAccess_Field;
    public static string Server_Field_Port => Resources.Server_Field_Port;
    public static string User_Field => Resources.User_Field;
    public static string Password_Field => Resources.Password_Field;
    public static string DurationMinutes_Field => Resources.DurationMinutes_Field;
    public static string DurationMinutes_Field_Help => Resources.DurationMinutes_Field_Help;

    #endregion

    #region Port

    private long _port = BUtilServerModelOptionsV2.DefaultPort;
    private string? _portError;

    public long Port
    {
        get => _port;
        set
        {
            if (value == _port) return;
            _port = value;
            OnPropertyChanged(nameof(Port));
            PortError = null;
        }
    }

    public string? PortError
    {
        get => _portError;
        private set
        {
            if (value == _portError) return;
            _portError = value;
            OnPropertyChanged(nameof(PortError));
        }
    }

    #endregion

    #region FtpsUser

    private string? _ftpsUser;
    private string? _ftpsUserError;

    public string? FtpsUser
    {
        get => _ftpsUser;
        set
        {
            if (value == _ftpsUser) return;
            _ftpsUser = value;
            OnPropertyChanged(nameof(FtpsUser));
            FtpsUserError = null;
        }
    }

    public string? FtpsUserError
    {
        get => _ftpsUserError;
        private set
        {
            if (value == _ftpsUserError) return;
            _ftpsUserError = value;
            OnPropertyChanged(nameof(FtpsUserError));
        }
    }

    #endregion

    #region FtpsPassword

    private string? _ftpsPassword;
    private string? _ftpsPasswordError;

    public string? FtpsPassword
    {
        get => _ftpsPassword;
        set
        {
            if (value == _ftpsPassword) return;
            _ftpsPassword = value;
            OnPropertyChanged(nameof(FtpsPassword));
            FtpsPasswordError = null;
        }
    }

    public string? FtpsPasswordError
    {
        get => _ftpsPasswordError;
        private set
        {
            if (value == _ftpsPasswordError) return;
            _ftpsPasswordError = value;
            OnPropertyChanged(nameof(FtpsPasswordError));
        }
    }

    #endregion

    #region Folder

    private string _folder = string.Empty;
    private string? _folderError;

    public string Folder
    {
        get => _folder;
        set
        {
            if (value == _folder) return;
            _folder = value;
            OnPropertyChanged(nameof(Folder));
            FolderError = null;
        }
    }

    public string? FolderError
    {
        get => _folderError;
        private set
        {
            if (value == _folderError) return;
            _folderError = value;
            OnPropertyChanged(nameof(FolderError));
        }
    }

    #endregion

    #region FolderAccess

    private BUtilServerFolderAccess _folderAccess = BUtilServerModelOptionsV2.DefaultFolderAccess;
    private string _selectedFolderAccess = string.Empty;

    public BUtilServerFolderAccess FolderAccess
    {
        get => _folderAccess;
        set
        {
            if (value == _folderAccess) return;
            _folderAccess = value;
            OnPropertyChanged(nameof(FolderAccess));
        }
    }

    public string SelectedFolderAccess
    {
        get => _selectedFolderAccess;
        set
        {
            if (value == _selectedFolderAccess) return;
            _selectedFolderAccess = value ?? string.Empty;
            OnPropertyChanged(nameof(SelectedFolderAccess));
            FolderAccess = FromDisplay(_selectedFolderAccess);
        }
    }

    private static string ToDisplay(BUtilServerFolderAccess access)
        => access == BUtilServerFolderAccess.ReadOnly
            ? Resources.FolderAccess_Field_ReadOnly
            : Resources.FolderAccess_Field_ReadWrite;

    private static BUtilServerFolderAccess FromDisplay(string? display)
        => display == Resources.FolderAccess_Field_ReadOnly
            ? BUtilServerFolderAccess.ReadOnly
            : BUtilServerFolderAccess.ReadWrite;

    #endregion

    #region DurationMinutes

    private long _durationMinutes = BUtilServerModelOptionsV2.DefaultDuration;

    public long DurationMinutes
    {
        get => _durationMinutes;
        set
        {
            if (value == _durationMinutes) return;
            _durationMinutes = value;
            OnPropertyChanged(nameof(DurationMinutes));
        }
    }

    #endregion

    #region Error

    private string? _error;

    public string? Error
    {
        get => _error;
        private set
        {
            if (value == _error) return;
            _error = value;
            OnPropertyChanged(nameof(Error));
        }
    }

    #endregion

    public bool Validate()
    {
        PortError = Port < PortMinimum || Port > 65535
            ? Resources.Server_Field_Port_Validation + $"(Min port {PortMinimum})"
            : null;

        FtpsUserError = string.IsNullOrWhiteSpace(FtpsUser)
            ? Resources.User_Field_Validation
            : null;

        FtpsPasswordError = string.IsNullOrWhiteSpace(FtpsPassword)
            ? Resources.Password_Field_Validation_NotSpecified
            : null;

        FolderError = string.IsNullOrWhiteSpace(Folder)
            ? Resources.Field_Folder_Validation_Empty
            : !Directory.Exists(Folder)
                ? Resources.Field_Folder_Validation_NotExist
                : null;

        Error = PortError
            ?? FtpsUserError
            ?? FtpsPasswordError
            ?? FolderError;
        return Error is null;
    }
}
