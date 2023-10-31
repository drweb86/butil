using Avalonia.Platform.Storage;
using BUtil.Core.Localization;
using BUtil.Core.TasksTree.MediaSyncBackupModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace butil_ui.Controls
{
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
            TransformFileNames = new[]
            {
                "{DATE:yyyy}\\{DATE:yyyy'-'MM', 'MMMM}\\{DATE:yyyy'-'MM'-'dd', 'dddd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
                "{DATE:yyyy}\\{DATE:MM}\\{DATE:yyyy'-'MM'-'dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
                "{DATE:yyyy}\\{DATE:MM}\\{DATE:dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
                "{DATE:yyyy}\\{DATE:yyyy'-'MM'-'dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
                "{DATE:yyyy'-'MM}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
                "{DATE:yyyy'-'MM'-'dd}\\{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
                "{DATE:yyyy'-'MM'-'dd' 'HH'-'mm'-'ss}",
            };
        }

        public IEnumerable<string> TransformFileNames { get; }

        #region Labels
        public string LeftMenu_Where => Resources.LeftMenu_Where;
        public string ImportMediaTask_Field_OutputFolder => Resources.ImportMediaTask_Field_OutputFolder;
        public string Field_Folder_Browse => Resources.Field_Folder_Browse;
        public string ImportMediaTask_SkipAlreadyImportedFiles => Resources.ImportMediaTask_SkipAlreadyImportedFiles;
        public string ImportMediaTask_Field_TransformFileName_Example => Resources.ImportMediaTask_Field_TransformFileName_Example;
        public string ImportMediaTask_Field_TransformFileName => Resources.ImportMediaTask_Field_TransformFileName;
        public string ImportMediaTask_Field_TransformFileName_Help => Resources.ImportMediaTask_Field_TransformFileName_Help;

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
}
