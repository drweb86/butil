using BUtil.Core.Localization;
using BUtil.Tasks.ImportMedia;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace BUtil.UI.Controls;

public class ImportDataSelectionViewModel : ObservableObject
{
    public ImportDataSelectionViewModel(
        bool skipAlreadyImportedFiles,
        bool deleteCopiedDataOnSourceMedia,
        DateTime? fileLastWriteTimeMin,
        IReadOnlyList<string>? fileExtensions,
        bool isNew,
        bool isExpanded = false)
    {
        IsExpanded = isExpanded;
        SkipAlreadyImportedFiles = skipAlreadyImportedFiles;
        DeleteCopiedDataOnSourceMedia = deleteCopiedDataOnSourceMedia;
        _fileLastWriteTimeMin = fileLastWriteTimeMin;
        FileExtensionsText = isNew && (fileExtensions == null || fileExtensions.Count == 0)
            ? ImportMediaFileExtensions.FormatForEditor(ImportMediaFileExtensions.Default)
            : ImportMediaFileExtensions.FormatForEditor(fileExtensions);
    }

    public bool IsExpanded { get; }

    #region Labels

    public static string LeftMenu_What => Resources.LeftMenu_What;
    public static string File_LastWriteTime_Min_Field => Resources.File_LastWriteTime_Min_Field;
    public static string ImportMediaTask_SkipAlreadyImportedFiles => Resources.ImportMediaTask_SkipAlreadyImportedFiles;
    public static string ImportMediaTask_SkipAlreadyImportedFiles_Help => Resources.ImportMediaTask_SkipAlreadyImportedFiles_Help;
    public static string ImportMediaTask_DeleteCopiedDataOnSourceMedia => Resources.ImportMediaTask_DeleteCopiedDataOnSourceMedia;
    public static string ImportMediaTask_Field_FileExtensions => Resources.ImportMediaTask_Field_FileExtensions;
    public static string ImportMediaTask_Field_FileExtensions_Help => Resources.ImportMediaTask_Field_FileExtensions_Help;

    #endregion

    #region SkipAlreadyImportedFiles

    private bool _skipAlreadyImportedFiles;

    public bool SkipAlreadyImportedFiles
    {
        get => _skipAlreadyImportedFiles;
        set
        {
            if (value == _skipAlreadyImportedFiles) return;
            _skipAlreadyImportedFiles = value;
            OnPropertyChanged(nameof(SkipAlreadyImportedFiles));
        }
    }

    #endregion

    #region DeleteCopiedDataOnSourceMedia

    private bool _deleteCopiedDataOnSourceMedia;

    public bool DeleteCopiedDataOnSourceMedia
    {
        get => _deleteCopiedDataOnSourceMedia;
        set
        {
            if (value == _deleteCopiedDataOnSourceMedia) return;
            _deleteCopiedDataOnSourceMedia = value;
            OnPropertyChanged(nameof(DeleteCopiedDataOnSourceMedia));
        }
    }

    #endregion

    #region FileLastWriteTimeMin

    private DateTime? _fileLastWriteTimeMin;

    public DateTime? FileLastWriteTimeMin
    {
        get => _fileLastWriteTimeMin;
        set
        {
            if (value == _fileLastWriteTimeMin) return;
            _fileLastWriteTimeMin = value;
            OnPropertyChanged(nameof(FileLastWriteTimeMin));
        }
    }

    #endregion

    #region FileExtensionsText

    private string _fileExtensionsText = string.Empty;
    private string? _fileExtensionsError;

    public string FileExtensionsText
    {
        get => _fileExtensionsText;
        set
        {
            if (value == _fileExtensionsText) return;
            _fileExtensionsText = value;
            OnPropertyChanged(nameof(FileExtensionsText));
            FileExtensionsError = null;
        }
    }

    public string? FileExtensionsError
    {
        get => _fileExtensionsError;
        private set
        {
            if (value == _fileExtensionsError) return;
            _fileExtensionsError = value;
            OnPropertyChanged(nameof(FileExtensionsError));
        }
    }

    public List<string> GetFileExtensions() => ImportMediaFileExtensions.Parse(FileExtensionsText);

    #endregion
}