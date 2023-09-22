using BUtil.Core.Localization;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using System;
using Terminal.Gui;

namespace BUtil.ConsoleBackup.UI
{
    public partial class EditStorageSettingsDialog
    {
        public IStorageSettingsV2 StorageSettings { get; private set; }

        private void SelectTransport(int transportId)
        {
            _folderStorageControls.ForEach(x => x.Visible = transportId == 0);
            _ftpsStorageControls.ForEach(x => x.Visible = transportId == 1);
        }

        internal EditStorageSettingsDialog(IStorageSettingsV2 source, string title) 
        {
            Title = title;
            InitializeComponent();

            if (source == null)
            {
                _transportSelectionComboBox.SelectedItem = 0;
                _ftpsEncryptionComboBox.SelectedItem = 0;
                _portFtpsStorageFolderTextField.Text = "21";
                SelectTransport(0);
            }
            else
            {
                if (source is FolderStorageSettingsV2)
                {
                    _transportSelectionComboBox.SelectedItem = 0;
                    SelectTransport(0);
                    _folderStorageFolderTextField.Text = ((FolderStorageSettingsV2)source).DestinationFolder;
                } else if (source is FtpsStorageSettingsV2)
                {
                    _transportSelectionComboBox.SelectedItem = 1;
                    SelectTransport(1);
                    var storageSettings = (FtpsStorageSettingsV2)source;
                    _hostFtpsStorageFolderTextField.Text = storageSettings.Host;
                    _ftpsEncryptionComboBox.SelectedItem = (int)storageSettings.Encryption;
                    _portFtpsStorageFolderTextField.Text = storageSettings.Port.ToString();
                    _userFtpsStorageFolderTextField.Text = storageSettings.User;
                    _pwdFtpsStorageFolderTextField.Text = storageSettings.Password;
                    _folderFtpsStorageFolderTextField.Text = storageSettings.Folder;
                    _ftpsQuotaTextField.Text = storageSettings.SingleBackupQuotaGb.ToString();
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        public bool Canceled { get; private set; } = true;

        private void OnCancel()
        {
            Canceled = true;
            Application.RequestStop();
        }

        private void OnTransportTypeSelection(ListViewItemEventArgs args)
        {
            SelectTransport(args.Item);
        }

        private void OnSave()
        {
            IStorageSettingsV2 storageSettings = null;
            if (_transportSelectionComboBox.SelectedItem == 0)
            {
                storageSettings = new FolderStorageSettingsV2
                {
                    DestinationFolder = this._folderStorageFolderTextField.Text.ToString(),
                };
            }
            else if (_transportSelectionComboBox.SelectedItem == 1)
            {
                storageSettings = new FtpsStorageSettingsV2
                {
                    Host = _hostFtpsStorageFolderTextField.Text.ToString(),
                    Encryption = (FtpsStorageEncryptionV2)_ftpsEncryptionComboBox.SelectedItem,
                    Port = int.TryParse(_portFtpsStorageFolderTextField.Text.ToString(), out var port) ? Math.Abs(port) : -1,
                    User = _userFtpsStorageFolderTextField.Text.ToString(),
                    Folder = _folderFtpsStorageFolderTextField.Text.ToString(),
                    Password = _pwdFtpsStorageFolderTextField.Text.ToString(),
                    SingleBackupQuotaGb = int.TryParse(_ftpsQuotaTextField.Text.ToString(), out var quota) ? Math.Abs(quota): 0,
                };
            }
            else
            {
                throw new NotSupportedException();
            }

            var error = StorageFactory.Test(new StubLog(), storageSettings);
            if (error != null)
            {
                MessageBox.ErrorQuery(string.Empty, error, Resources.Button_Close);
                return;
            }

            Canceled = false;
            StorageSettings = storageSettings;
            Application.RequestStop();
        }
    }
}
