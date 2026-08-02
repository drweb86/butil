using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BUtil.Tasks.BUtilClient.UI.Controls;

public class FolderSectionViewModel : ObservableObject
{
    public FolderSectionViewModel(string folder, bool skipExistingFiles, bool isExpanded)
    {
        IsExpanded = isExpanded;
        Folder = folder;
        SkipExistingFiles = skipExistingFiles;

        BrowseFolderCommand = new AsyncRelayCommand(async () =>
        {
            if (BrowseFolderAsync != null)
                await BrowseFolderAsync();
        });
    }

    public bool IsExpanded { get; }

    public Func<Task>? BrowseFolderAsync { get; set; }
    public IAsyncRelayCommand BrowseFolderCommand { get; }

    #region Labels
    public static string LeftMenu_What => Resources.LeftMenu_What;
    public static string Field_Folder => Resources.Field_Folder;
    public static string Field_Folder_Browse => Resources.Field_Folder_Browse;
    public static string UploadFolderTask_SkipExistingFiles => Resources.UploadFolderTask_SkipExistingFiles;
    public static string UploadFolderTask_SkipExistingFiles_Help => Resources.UploadFolderTask_SkipExistingFiles_Help;

    #endregion

    #region SkipExistingFiles

    private bool _skipExistingFiles;

    public bool SkipExistingFiles
    {
        get => _skipExistingFiles;
        set
        {
            if (value == _skipExistingFiles) return;
            _skipExistingFiles = value;
            OnPropertyChanged(nameof(SkipExistingFiles));
        }
    }

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
