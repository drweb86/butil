
namespace BUtil.ConsoleBackup.UI {
    using System;
    using System.Collections.Generic;
    using Terminal.Gui;
    
    public partial class EditStorageSettingsDialog : Terminal.Gui.Dialog
    {
        private Terminal.Gui.ComboBox _transportSelectionComboBox;

        private List<Terminal.Gui.View> _folderStorageControls = new List<View>();
        private Terminal.Gui.TextField _folderStorageFolderTextField;
        
        private List<Terminal.Gui.View> _ftpsStorageControls = new List<View>();
        private Terminal.Gui.TextField _hostFtpsStorageFolderTextField;
        private Terminal.Gui.TextField _portFtpsStorageFolderTextField;
        private Terminal.Gui.TextField _userFtpsStorageFolderTextField;
        private Terminal.Gui.TextField _pwdFtpsStorageFolderTextField;
        private Terminal.Gui.TextField _folderFtpsStorageFolderTextField;

        private void InitializeComponent() {
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.TextAlignment = Terminal.Gui.TextAlignment.Left;

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 0,
                Text = BUtil.ConsoleBackup.Localization.Resources.SelectTransport,
            });
            var transports = new List<string> { BUtil.Core.Localization.Resources.FolderStorage, "FTPS" };
            _transportSelectionComboBox = new ComboBox
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = transports.Count + 1,
                HideDropdownListOnClick = true,
                ReadOnly = true,
            };
            _transportSelectionComboBox.SetSource(transports);
            _transportSelectionComboBox.SelectedItemChanged += OnTransportTypeSelection;
            Add(_transportSelectionComboBox);

            _folderStorageFolderTextField = AddTextField(_folderStorageControls, BUtil.ConsoleBackup.Localization.Resources.DestinationFolder, 3);

            _hostFtpsStorageFolderTextField = AddTextField(_ftpsStorageControls, BUtil.ConsoleBackup.Localization.Resources.HostField, 3);
            _portFtpsStorageFolderTextField = AddTextField(_ftpsStorageControls, BUtil.ConsoleBackup.Localization.Resources.PortField, 6);
            _userFtpsStorageFolderTextField = AddTextField(_ftpsStorageControls, BUtil.ConsoleBackup.Localization.Resources.UserField, 9);
            _pwdFtpsStorageFolderTextField = AddTextField(_ftpsStorageControls, BUtil.ConsoleBackup.Localization.Resources.PasswordField, 12);
            _folderFtpsStorageFolderTextField = AddTextField(_ftpsStorageControls, BUtil.ConsoleBackup.Localization.Resources.FolderField, 15);

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

        private TextField AddTextField(List<Terminal.Gui.View> items, string label, int y)
        {
            var labelView = new Label
            {
                AutoSize = true,
                X = 0,
                Y = y,
                Text = label,
            };

            items.Add(labelView);
            this.Add(labelView);

            var field = new TextField
            {
                X = 0,
                Y = y + 1,
                Width = Dim.Fill(0),
            };
            items.Add(field);
            this.Add(field);
            return field;
        }
    }
}
