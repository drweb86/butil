
namespace BUtil.ConsoleBackup.UI {
    using Terminal.Gui;
    
    public partial class EditMediaSyncDialog : Terminal.Gui.Dialog
    {
        private Terminal.Gui.TextField _titleTextField;
        private Terminal.Gui.TextField _sourceFolderTextField;
        private Terminal.Gui.TextField _destinationFolderTextField;
        private Terminal.Gui.TextField _transformFileNameTextField;

        private void InitializeComponent() {
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.TextAlignment = Terminal.Gui.TextAlignment.Left;

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 0,
                Text = BUtil.ConsoleBackup.Localization.Resources.Title
            });

            _titleTextField = new TextField
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(0),
            };
            Add(_titleTextField);

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 3,
                Text = BUtil.ConsoleBackup.Localization.Resources.PhotosVideosFolderOnMediaDevice
            });

            _sourceFolderTextField = new TextField
            {
                Text = string.Empty,
                X = 0,
                Y = 4,
                Width = Dim.Fill(0),
            };
            Add(_sourceFolderTextField);

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 6,
                Text = BUtil.ConsoleBackup.Localization.Resources.DestinationFolder
            });

            _destinationFolderTextField = new TextField
            {
                Text = string.Empty,
                X = 0,
                Y = 7,
                Width = Dim.Fill(0),
            };
            Add(_destinationFolderTextField);

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 9,
                Text = BUtil.ConsoleBackup.Localization.Resources.FileNameTransformations
            });

            _transformFileNameTextField = new TextField
            {
                Text = string.Empty,
                X = 0,
                Y = 10,
                Width = Dim.Fill(0),
            };
            Add(_transformFileNameTextField);
            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 11,
                Text = BUtil.ConsoleBackup.Localization.Resources.HelpForTokens
            });


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
