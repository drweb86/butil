using BUtil.Core.Localization;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using System;
using Terminal.Gui;
using BUtil.Core;
using System.Linq;
using System.Collections.Generic;

namespace BUtil.ConsoleBackup.UI
{
    public partial class EditStorageSettingsDialog
    {
        private List<string> _mtpDevices;

        public IStorageSettingsV2 StorageSettings { get; private set; }

        private void SelectTransport(int transportId)
        {
            _folderStorageControls.ForEach(x => x.Visible = transportId == 0);
            _ftpsStorageControls.ForEach(x => x.Visible = transportId == 1);
            _sambaStorageControls.ForEach(x => x.Visible = transportId == 2);
            _mtpStorageControls.ForEach(x => x.Visible = transportId == 3);
        }

        internal EditStorageSettingsDialog(IStorageSettingsV2 source, string title) 
        {
            Title = title;
            InitializeComponent();

            if (source == null)
            {
                _transportSelectionComboBox.SelectedItem = 0;
                _ftpsEncryptionComboBox.SelectedItem = 0;
                _ftpsPortTextField.Text = "21";
                SelectTransport(0);
            }
            else
            {
                if (source is FolderStorageSettingsV2)
                {
                    _transportSelectionComboBox.SelectedItem = 0;
                    SelectTransport(0);
                    var folderSettings = (FolderStorageSettingsV2)source;
                    _folderDirectoryTextField.Text = folderSettings.DestinationFolder;
                    _folderQuotaTextField.Text = folderSettings.SingleBackupQuotaGb.ToString();
                    _folderConnectionScriptTextField.Text = folderSettings.MountPowershellScript;
                    _folderDisconnectionScriptTextField.Text = folderSettings.UnmountPowershellScript;
                } else if (source is FtpsStorageSettingsV2)
                {
                    _transportSelectionComboBox.SelectedItem = 1;
                    SelectTransport(1);
                    var storageSettings = (FtpsStorageSettingsV2)source;
                    _ftpsHostTextField.Text = storageSettings.Host;
                    _ftpsEncryptionComboBox.SelectedItem = (int)storageSettings.Encryption;
                    _ftpsPortTextField.Text = storageSettings.Port.ToString();
                    _ftpsUserTextField.Text = storageSettings.User;
                    _ftpPasswordTextField.Text = storageSettings.Password;
                    _ftpsFolderTextField.Text = storageSettings.Folder;
                    _ftpsQuotaTextField.Text = storageSettings.SingleBackupQuotaGb.ToString();
                }
                else if (source is SambaStorageSettingsV2)
                {
                    _transportSelectionComboBox.SelectedItem = 2;
                    SelectTransport(2);
                    var settings = (SambaStorageSettingsV2)source;
                    _sambaUrlTextField.Text = settings.Url;
                    _sambaUserTextField.Text = settings.User;
                    _sambaPasswordTextField.Text = settings.Password;
                    _sambaQuotaTextField.Text = settings.SingleBackupQuotaGb.ToString();
                }
                else if (source is MtpStorageSettings)
                {
                    _transportSelectionComboBox.SelectedItem = 3;
                    SelectTransport(3);
                    LoadMtpTransports();
                    var settings = (MtpStorageSettings)source;
                    SetMtpDevice(settings.Device);
                    _mtpFolderTextField.Text = settings.Folder;

                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        private void SetMtpDevice(string device)
        {
            var index = _mtpDevices.IndexOf(device);
            _mtpDeviceComboBox.SelectedItem = index;
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

            if (args.Item == 3)
            {
                LoadMtpTransports();
            }
        }

        private void LoadMtpTransports()
        {
            _mtpDevices = new MtpService()
                    .GetItems()
                    .ToList();
            _mtpDeviceComboBox.Height = _mtpDevices.Count + 1;
            _mtpDeviceComboBox.SetSource(_mtpDevices);
        }

        private void OnSave()
        {
            IStorageSettingsV2 storageSettings = null;
            if (_transportSelectionComboBox.SelectedItem == 0)
            {
                storageSettings = new FolderStorageSettingsV2
                {
                    DestinationFolder = this._folderDirectoryTextField.Text.ToString(),
                    SingleBackupQuotaGb = int.TryParse(_folderQuotaTextField.Text.ToString(), out var quota) ? Math.Abs(quota) : 0,
                    MountPowershellScript = _folderConnectionScriptTextField.Text.ToString(),
                    UnmountPowershellScript = _folderDisconnectionScriptTextField.Text.ToString(),
                };
            }
            else if (_transportSelectionComboBox.SelectedItem == 1)
            {
                storageSettings = new FtpsStorageSettingsV2
                {
                    Host = _ftpsHostTextField.Text.ToString(),
                    Encryption = (FtpsStorageEncryptionV2)_ftpsEncryptionComboBox.SelectedItem,
                    Port = int.TryParse(_ftpsPortTextField.Text.ToString(), out var port) ? Math.Abs(port) : -1,
                    User = _ftpsUserTextField.Text.ToString(),
                    Folder = _ftpsFolderTextField.Text.ToString(),
                    Password = _ftpPasswordTextField.Text.ToString(),
                    SingleBackupQuotaGb = int.TryParse(_ftpsQuotaTextField.Text.ToString(), out var quota) ? Math.Abs(quota): 0,
                };
            }
            else if (_transportSelectionComboBox.SelectedItem == 2)
            {
                storageSettings = new SambaStorageSettingsV2
                {
                    Url = _sambaUrlTextField.Text.ToString(),
                    User = _sambaUserTextField.Text.ToString(),
                    Password = _sambaPasswordTextField.Text.ToString(),
                    SingleBackupQuotaGb = int.TryParse(_sambaQuotaTextField.Text.ToString(), out var quota) ? Math.Abs(quota) : 0,
                };
            }
            else if (_transportSelectionComboBox.SelectedItem == 3)
            {
                storageSettings = new MtpStorageSettings
                {
                    Device = _mtpDeviceComboBox.SelectedItem < 0 ? null : _mtpDevices[_mtpDeviceComboBox.SelectedItem],
                    Folder = _mtpFolderTextField.Text.ToString(),
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
