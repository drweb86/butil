using BUtil.Tasks.Synchronization;
using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public class SynchronizationWhatViewModel : ObservableObject
{
    public SynchronizationWhatViewModel(
        string folder,
        SynchronizationTaskModelMode synchronizationMode,
        bool isExpanded = false)
    {
        IsExpanded = isExpanded;
        Folder = folder;
        SynchronizationMode = synchronizationMode;
        SelectedSynchronizationMode = ToDisplay(synchronizationMode);

        BrowseFolderCommand = new AsyncRelayCommand(async () =>
        {
            if (BrowseFolderAsync != null)
                await BrowseFolderAsync();
        });
    }

    public bool IsExpanded { get; }

    public Func<Task>? BrowseFolderAsync { get; set; }
    public IAsyncRelayCommand BrowseFolderCommand { get; }

    public IReadOnlyList<string> SynchronizationModeOptions { get; } =
    [
        Resources.SynchronizationMode_Field_TwoWay,
        Resources.SynchronizationMode_Field_Read
    ];

    #region Labels
    public static string LeftMenu_What => Resources.LeftMenu_What;
    public static string Field_Folder => Resources.Field_Folder;
    public static string Field_Folder_Browse => Resources.Field_Folder_Browse;

    public static string SynchronizationMode_Field => Resources.SynchronizationMode_Field;
    public static string SynchronizationMode_Field_TwoWay => Resources.SynchronizationMode_Field_TwoWay;
    public static string SynchronizationMode_Field_Read => Resources.SynchronizationMode_Field_Read;

    #endregion

    #region SynchronizationMode

    private SynchronizationTaskModelMode _synchronizationMode;
    private string _selectedSynchronizationMode = string.Empty;

    public SynchronizationTaskModelMode SynchronizationMode
    {
        get { return _synchronizationMode; }
        set
        {
            if (value == _synchronizationMode) return;
            _synchronizationMode = value;
            OnPropertyChanged(nameof(SynchronizationMode));
        }
    }

    public string SelectedSynchronizationMode
    {
        get => _selectedSynchronizationMode;
        set
        {
            if (value == _selectedSynchronizationMode) return;
            _selectedSynchronizationMode = value ?? string.Empty;
            OnPropertyChanged(nameof(SelectedSynchronizationMode));
            SynchronizationMode = FromDisplay(_selectedSynchronizationMode);
        }
    }

    private static string ToDisplay(SynchronizationTaskModelMode mode)
        => mode == SynchronizationTaskModelMode.Read
            ? Resources.SynchronizationMode_Field_Read
            : Resources.SynchronizationMode_Field_TwoWay;

    private static SynchronizationTaskModelMode FromDisplay(string? display)
        => display == Resources.SynchronizationMode_Field_Read
            ? SynchronizationTaskModelMode.Read
            : SynchronizationTaskModelMode.TwoWay;

    #endregion

    #region Folder

    private string _folder = string.Empty;
    private string? _folderError;

    public string Folder
    {
        get { return _folder; }
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
        FolderError = string.IsNullOrWhiteSpace(Folder)
            ? Resources.Field_Folder_Validation_Empty
            : !Directory.Exists(Folder)
                ? Resources.Field_Folder_Validation_NotExist
                : null;

        Error = FolderError;
        return Error is null;
    }
}
