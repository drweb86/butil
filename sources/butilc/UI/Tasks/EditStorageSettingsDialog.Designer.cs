
namespace BUtil.ConsoleBackup.UI {
    using BUtil.Core.Localization;
    using System;
    using System.Collections.Generic;
    using Terminal.Gui;
    
    public partial class EditStorageSettingsDialog : Terminal.Gui.Dialog
    {
        private Terminal.Gui.ComboBox _transportSelectionComboBox;

        private List<Terminal.Gui.View> _folderStorageControls = new List<View>();
        private Terminal.Gui.TextField _folderDirectoryTextField;
        private Terminal.Gui.TextField _folderQuotaTextField;
        private Terminal.Gui.TextView _folderConnectionScriptTextField;
        private Terminal.Gui.TextView _folderDisconnectionScriptTextField;

        private List<Terminal.Gui.View> _ftpsStorageControls = new List<View>();
        private Terminal.Gui.TextField _ftpsHostTextField;
        private Terminal.Gui.ComboBox _ftpsEncryptionComboBox;
        private Terminal.Gui.TextField _ftpsPortTextField;
        private Terminal.Gui.TextField _ftpsUserTextField;
        private Terminal.Gui.TextField _ftpPasswordTextField;
        private Terminal.Gui.TextField _ftpsFolderTextField;
        private Terminal.Gui.TextField _ftpsQuotaTextField;

        private List<Terminal.Gui.View> _sambaStorageControls = new List<View>();
        private Terminal.Gui.TextField _sambaUrlTextField;
        private Terminal.Gui.TextField _sambaUserTextField;
        private Terminal.Gui.TextField _sambaPasswordTextField;
        private Terminal.Gui.TextField _sambaQuotaTextField;

        private void InitializeComponent() {
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.TextAlignment = Terminal.Gui.TextAlignment.Left;

            Add(new Label
            {
                AutoSize = true,
                X = 0,
                Y = 0,
                Text = BUtil.Core.Localization.Resources.LeftMenu_Where,
            });
            var transports = new List<string> { BUtil.Core.Localization.Resources.DirectoryStorage, "FTPS", "SMB/CIFS" };
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

            _folderDirectoryTextField = AddTextField(_folderStorageControls, BUtil.Core.Localization.Resources.Field_Folder, 3);
            _folderQuotaTextField = AddTextField(_folderStorageControls, Resources.DataStorage_Field_UploadQuota, 6);
            _folderConnectionScriptTextField = AddTextView(_folderStorageControls, Resources.DataStorage_Field_ConnectScript, 9, 6);
            _folderDisconnectionScriptTextField = AddTextView(_folderStorageControls, Resources.DataStorage_Field_DisconnectionScript, 17, 6);

            _sambaUrlTextField = AddTextField(_sambaStorageControls, BUtil.Core.Localization.Resources.Url_Field, 3);
            _sambaUserTextField = AddTextField(_sambaStorageControls, Resources.User_Field, 6);
            _sambaPasswordTextField = AddTextField(_sambaStorageControls, Resources.Password_Field, 9);
            _sambaQuotaTextField = AddTextField(_sambaStorageControls, Resources.DataStorage_Field_UploadQuota, 12);

            _ftpsHostTextField = AddTextField(_ftpsStorageControls, BUtil.Core.Localization.Resources.Server_Field_Address, 3);
            _ftpsPortTextField = AddTextField(_ftpsStorageControls, BUtil.Core.Localization.Resources.Server_Field_Port, 6);
            _ftpsUserTextField = AddTextField(_ftpsStorageControls, BUtil.Core.Localization.Resources.User_Field, 9);
            _ftpPasswordTextField = AddTextField(_ftpsStorageControls, BUtil.Core.Localization.Resources.Password_Field, 12);
            _ftpsFolderTextField = AddTextField(_ftpsStorageControls, BUtil.Core.Localization.Resources.Field_Folder, 15);
            var ftpsEncryptionLabel = new Label
            {
                AutoSize = true,
                X = 0,
                Y = 18,
                Text = Resources.Ftps_Field_Encryption,
            };
            _ftpsStorageControls.Add(ftpsEncryptionLabel);
            Add(ftpsEncryptionLabel);

            var ftpsEncryptionProps = new List<string> { BUtil.Core.Localization.Resources.Ftps_Encryption_Option_Explicit, Resources.Ftps_Encryption_Option_Implicit };
            _ftpsEncryptionComboBox = new ComboBox
            {
                X = 0,
                Y = 19,
                Width = Dim.Fill(),
                Height = ftpsEncryptionProps.Count + 1,
                HideDropdownListOnClick = true,
                ReadOnly = true,
            };
            _ftpsEncryptionComboBox.SetSource(ftpsEncryptionProps);
            Add(_ftpsEncryptionComboBox);
            _ftpsStorageControls.Add(_ftpsEncryptionComboBox);

            _ftpsQuotaTextField = AddTextField(_ftpsStorageControls, Resources.DataStorage_Field_UploadQuota, 21);

            var saveButton = new Button
            {
                Text = BUtil.Core.Localization.Resources.Button_OK,
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

        private TextView AddTextView(List<Terminal.Gui.View> items, string label, int y, int height)
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

            var field = new TextView
            {
                X = 0,
                Y = y + 1,
                Width = Dim.Fill(0),
                Height = height
            };
            items.Add(field);
            this.Add(field);
            return field;
        }
    }
}
