using BUtil.Core;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Tasks.ImportMedia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public class ImportMediaTaskWhereTaskViewModel : ObservableObject
{
    public ImportMediaTaskWhereTaskViewModel(
        string outputFolder,
        string transformFileName,
        bool isExpanded = false
        )
    {
        IsExpanded = isExpanded;
        OutputFolder = outputFolder;
        TransformFileName = transformFileName;

        BrowseOutputFolderCommand = new AsyncRelayCommand(async () =>
        {
            if (BrowseOutputFolderAsync != null)
                await BrowseOutputFolderAsync();
        });
        OpenDateTimeFormatDocsCommand = new RelayCommand(OpenDateTimeFormatDocs, () => CanOpenLink);
        UpdateTransformFileNameHelp();
    }

    public bool IsExpanded { get; }
    public bool CanOpenLink { get; } = PlatformSpecificExperience.Instance.SupportManager.CanOpenLink;

    public IReadOnlyList<string> TransformFileNamePresets { get; } = ImportMediaTransformFileName.Presets;

    public Func<Task>? BrowseOutputFolderAsync { get; set; }
    public IAsyncRelayCommand BrowseOutputFolderCommand { get; }
    public IRelayCommand OpenDateTimeFormatDocsCommand { get; }

    #region Labels

    public static string LeftMenu_Where => Resources.LeftMenu_Where;
    public static string ImportMediaTask_Field_OutputFolder => Resources.ImportMediaTask_Field_OutputFolder;
    public static string Field_Folder_Browse => Resources.Field_Folder_Browse;
    public static string ImportMediaTask_Field_TransformFileName => Resources.ImportMediaTask_Field_TransformFileName;
    public static string ImportMediaTask_Field_TransformFileName_Documentation => Resources.ImportMediaTask_Field_TransformFileName_Documentation;

    #endregion

    #region TransformFileName

    private string _transformFileName = string.Empty;
    private string? _transformFileNameError;
    private string? _transformFileNameHelp;

    public string TransformFileName
    {
        get => _transformFileName;
        set
        {
            if (value == _transformFileName) return;
            _transformFileName = value ?? string.Empty;
            OnPropertyChanged(nameof(TransformFileName));
            TransformFileNameError = null;
            UpdateTransformFileNameHelp();
        }
    }

    public string? TransformFileNameError
    {
        get => _transformFileNameError;
        private set
        {
            if (value == _transformFileNameError) return;
            _transformFileNameError = value;
            OnPropertyChanged(nameof(TransformFileNameError));
        }
    }

    public string? TransformFileNameHelp
    {
        get => _transformFileNameHelp;
        private set
        {
            if (value == _transformFileNameHelp) return;
            _transformFileNameHelp = value;
            OnPropertyChanged(nameof(TransformFileNameHelp));
        }
    }

    #endregion

    #region OutputFolder

    private string _outputFolder = string.Empty;
    private string? _outputFolderError;

    public string OutputFolder
    {
        get => _outputFolder;
        set
        {
            if (value == _outputFolder) return;
            _outputFolder = value;
            OnPropertyChanged(nameof(OutputFolder));
            OutputFolderError = null;
            UpdateTransformFileNameHelp();
        }
    }

    public string? OutputFolderError
    {
        get => _outputFolderError;
        private set
        {
            if (value == _outputFolderError) return;
            _outputFolderError = value;
            OnPropertyChanged(nameof(OutputFolderError));
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
        OutputFolderError = string.IsNullOrWhiteSpace(OutputFolder)
            ? Resources.Field_Folder_Validation_Empty
            : !Directory.Exists(OutputFolder)
                ? Resources.Field_Folder_Validation_NotExist
                : null;

        TransformFileNameError = ImportMediaTransformFileName.Validate(TransformFileName);

        Error = OutputFolderError ?? TransformFileNameError;
        return Error is null;
    }

    private void UpdateTransformFileNameHelp()
    {
        TransformFileNameHelp = ImportMediaTransformFileName.TryBuildExample(TransformFileName, OutputFolder);
    }

    private void OpenDateTimeFormatDocs()
    {
        if (!CanOpenLink)
            return;

        PlatformSpecificExperience.Instance.SupportManager.OpenLink(ApplicationLinks.DotNetCustomDateTimeFormatStrings);
    }
}