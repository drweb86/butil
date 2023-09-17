﻿
namespace BUtil.ConsoleBackup.UI {
    using Terminal.Gui;
    
    public partial class EditImportMediaTaskDialog : Terminal.Gui.Dialog
    {
        private Terminal.Gui.TextField _titleTextField;
        private Terminal.Gui.TextField _destinationFolderTextField;
        private Terminal.Gui.TextField _transformFileNameTextField;
        private Terminal.Gui.Label _transformFileNameLabel;
        private Terminal.Gui.CheckBox _skipAlreadyImportedFilesCheckBox;

        private void InitializeComponent() {
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.TextAlignment = Terminal.Gui.TextAlignment.Left;

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 0,
                Text = BUtil.Core.Localization.Resources.ImportMediaTask_Help
            });

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 4,
                Text = BUtil.Core.Localization.Resources.Name_Field
            });

            _titleTextField = new TextField
            {
                X = 0,
                Y = 5,
                Width = Dim.Fill(0),
            };
            Add(_titleTextField);

            var sourceSelectButton = new Button
            {
                X = 0,
                Y = 7,
                Text = BUtil.Core.Localization.Resources.ImportMediaTask_ImportDataSource,
                IsDefault = true,
            };
            sourceSelectButton.Clicked += OnSpecifySource;
            Add(sourceSelectButton);

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 9,
                Text = BUtil.Core.Localization.Resources.ImportMediaTask_Field_OutputFolder
            });

            _destinationFolderTextField = new TextField
            {
                Text = string.Empty,
                X = 0,
                Y = 10,
                Width = Dim.Fill(0),
            };
            Add(_destinationFolderTextField);

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 12,
                Text = BUtil.Core.Localization.Resources.ImportMediaTask_Field_TransformFileName
            });

            _transformFileNameTextField = new TextField
            {
                Text = string.Empty,
                X = 0,
                Y = 13,
                Width = Dim.Fill(0),
            };
            _transformFileNameTextField.TextChanged += OnTransformFileNameTextChanged;
            Add(_transformFileNameTextField);
            _transformFileNameLabel = new Label
            {
                AutoSize = true,
                X = 0,
                Y = 14,
                Text = string.Empty
            };
            Add(_transformFileNameLabel);
            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 15,
                Text = BUtil.Core.Localization.Resources.ImportMediaTask_Field_TransformFileName_Help
            });

            _skipAlreadyImportedFilesCheckBox = new CheckBox
            {
                Text = BUtil.Core.Localization.Resources.ImportMediaTask_SkipAlreadyImportedFiles,
                X = 0,
                Y = 19,
                Width = Dim.Fill(0),
            };
            Add(_skipAlreadyImportedFilesCheckBox);

            var saveButton = new Button
            {
                Text = BUtil.Core.Localization.Resources.Button_Save,
                IsDefault = true,
            };
            saveButton.Clicked += OnSave;
            AddButton(saveButton);

            var cancelButton = new Button
            {
                Text = BUtil.Core.Localization.Resources.Button_Cancel,
            };
            cancelButton.Clicked += OnCancel;
            AddButton(cancelButton);
        }
    }
}
