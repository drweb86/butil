using BUtil.Core.Localization;
using BUtil.Core.TasksTree.MediaSyncBackupModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO;
using System.Linq;

namespace butil_ui.Controls;

public class ImportMediaTaskWhereTaskViewModel : ObservableObject
{
    public ImportMediaTaskWhereTaskViewModel(
        string outputFolder,
        bool skipAlreadyImportedFiles,
        string transformFileName
        )
    {
        OutputFolder = outputFolder;
        SkipAlreadyImportedFiles = skipAlreadyImportedFiles;
        TransformFileName = transformFileName;
        _transformFileNames =
        [
            "{DATE:yyyy}\\{DATE:yyyy'-'MM', 'MMMM}\\{DATE:yyyy'-'MM'-'dd', 'dddd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
            "{DATE:yyyy}\\{DATE:MM}\\{DATE:yyyy'-'MM'-'dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
            "{DATE:yyyy}\\{DATE:MM}\\{DATE:dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
            "{DATE:yyyy}\\{DATE:yyyy'-'MM'-'dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
            "{DATE:yyyy'-'MM}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
            "{DATE:yyyy'-'MM'-'dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
            "{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
        ];
    }

    private readonly string[] _transformFileNames;

    #region Labels
    public static string LeftMenu_Where => Resources.LeftMenu_Where;
    public static string ImportMediaTask_Field_OutputFolder => Resources.ImportMediaTask_Field_OutputFolder;
    public static string Field_Folder_Browse => Resources.Field_Folder_Browse;
    public static string ImportMediaTask_SkipAlreadyImportedFiles => Resources.ImportMediaTask_SkipAlreadyImportedFiles;
    public static string ImportMediaTask_Field_TransformFileName_Example => Resources.ImportMediaTask_Field_TransformFileName_Example;
    public static string ImportMediaTask_Field_TransformFileName => Resources.ImportMediaTask_Field_TransformFileName;
    public static string ImportMediaTask_Field_TransformFileName_Help => Resources.ImportMediaTask_Field_TransformFileName_Help;

    #endregion

    #region TransformFileName

    private string _transformFileName = string.Empty;

    public string TransformFileName
    {
        get
        {
            return _transformFileName;
        }
        set
        {
            if (value == _transformFileName)
                return;
            _transformFileName = value;
            OnPropertyChanged(nameof(TransformFileName));
            OnChangeTransormFileName();
        }
    }

    #endregion

    #region TransformFileNameExample

    private string _transformFileNameExample = string.Empty;

    public string TransformFileNameExample
    {
        get
        {
            return _transformFileNameExample;
        }
        set
        {
            if (value == _transformFileNameExample)
                return;
            _transformFileNameExample = value;
            OnPropertyChanged(nameof(TransformFileNameExample));
        }
    }

    #endregion

    #region OutputFolder

    private string _outputFolder = string.Empty;

    public string OutputFolder
    {
        get
        {
            return _outputFolder;
        }
        set
        {
            if (value == _outputFolder)
                return;
            _outputFolder = value;
            OnPropertyChanged(nameof(OutputFolder));
        }
    }

    #endregion

    #region SkipAlreadyImportedFiles

    private bool _skipAlreadyImportedFiles;

    public bool SkipAlreadyImportedFiles
    {
        get
        {
            return _skipAlreadyImportedFiles;
        }
        set
        {
            if (value == _skipAlreadyImportedFiles)
                return;
            _skipAlreadyImportedFiles = value;
            OnPropertyChanged(nameof(SkipAlreadyImportedFiles));
        }
    }

    #endregion

    #region Commands

    public void GoPreviousExampleCommand()
    {
        var index = Array.FindIndex(_transformFileNames, x => x == _transformFileName);
        if (index == -1)
        {
            TransformFileName = _transformFileNames[0];
            return;
        }
        TransformFileName = _transformFileNames[(index + _transformFileNames.Length - 1) % _transformFileNames.Length];
    }

    public void GoNextExampleCommand()
    {
        var index = Array.FindIndex(_transformFileNames.ToArray(), x => x == _transformFileName);
        if (index == -1)
        {
            TransformFileName = _transformFileNames[0];
            return;
        }
        TransformFileName = _transformFileNames[(index + _transformFileNames.Length + 1) % _transformFileNames.Length];
    }

    #endregion


    private void OnChangeTransormFileName()
    {
        try
        {
            var fileName = "DCIM001.jpg";
            var modifiedAt = DateTime.Now;

            TransformFileNameExample = string.Format(Resources.ImportMediaTask_Field_TransformFileName_Example,
                fileName, modifiedAt, OutputFolder.TrimEnd('\\').TrimEnd('/') + '\\' + DateTokenReplacer.ParseString(TransformFileName, modifiedAt) + Path.GetExtension(fileName));
        }
        catch
        {
            TransformFileNameExample = Resources.ImportMediaTask_Field_TransformFileName_Validation_Invalid;
        }
    }
}
