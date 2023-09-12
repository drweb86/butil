
namespace BUtil.ConsoleBackup.UI {
    using NStack;
    using System;
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
                Text = BUtil.ConsoleBackup.Localization.Resources.ImportMediaHint
            });

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 4,
                Text = BUtil.ConsoleBackup.Localization.Resources.Title
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
                Text = BUtil.ConsoleBackup.Localization.Resources.ImportFilesFrom,
                IsDefault = true,
            };
            sourceSelectButton.Clicked += OnSpecifySource;
            Add(sourceSelectButton);

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 9,
                Text = BUtil.ConsoleBackup.Localization.Resources.DestinationFolder
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
                Text = BUtil.ConsoleBackup.Localization.Resources.FileNameTransformations
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
                Text = BUtil.ConsoleBackup.Localization.Resources.HelpForTokens
            });

            _skipAlreadyImportedFilesCheckBox = new CheckBox
            {
                Text = BUtil.ConsoleBackup.Localization.Resources.SkipAlreadyImportedFiles,
                X = 0,
                Y = 19,
                Width = Dim.Fill(0),
            };
            Add(_skipAlreadyImportedFilesCheckBox);

            var saveButton = new Button
            {
                Text = BUtil.ConsoleBackup.Localization.Resources.Save,
                IsDefault = true,
            };
            saveButton.Clicked += OnSave;
            AddButton(saveButton);

            var cancelButton = new Button
            {
                Text = BUtil.Core.Localization.Resources.Cancel,
            };
            cancelButton.Clicked += OnCancel;
            AddButton(cancelButton);
        }
    }
}
