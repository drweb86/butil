#nullable disable
using System;
using System.Windows.Forms;
using BUtil.Core.Storages;
using BUtil.Core.Localization;
using BUtil.RestorationMaster;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core;

namespace BUtil.Configurator.Configurator.Controls
{
    internal sealed partial class WhereUserControl : Core.PL.BackUserControl
    {
        public WhereUserControl()
        {
            InitializeComponent();

            _limitUploadLabelV2.Text = _limitUploadLabel.Text = Resources.DataStorage_Field_UploadQuota;
            _userLabel.Text = Resources.User_Field;
            _passwordLabel.Text = Resources.Password_Field;
            _shareLabel.Text = Resources.Url_Field;


            whereToStoreBackupLabel.Text = Resources.DirectoryStorage_Field_Directory;
            _scriptsLabel.Text = Resources.DataStorage_Script_Help;
            _mountScriptLabel.Text = Resources.DataStorage_Field_ConnectScript;
            _unmountScriptLabel.Text = Resources.DataStorage_Field_DisconnectionScript;

            _ftpsQuotaLabel.Text = Resources.DataStorage_Field_UploadQuota;
            _ftpsServerLabel.Text = Resources.Server_Field_Address;
            _ftpsPortLabel.Text = Resources.Server_Field_Port;
            _ftpsPortNumericUpDown.Value = 21;
            _ftpsUserLabel.Text = Resources.User_Field;
            _ftpsPasswordLabel.Text = Resources.Password_Field;
            _ftpsFolderLabel.Text = Resources.Field_Folder;

            _hddStorageTabPage.Text = Resources.DirectoryStorage;

            _ftpsEncryptionLabel.Text = Resources.Ftps_Field_Encryption;
            _ftpsEncryptionComboBox.Items.Clear();
            _ftpsEncryptionComboBox.Items.Add(Resources.Ftps_Encryption_Option_Explicit);
            _ftpsEncryptionComboBox.Items.Add(Resources.Ftps_Encryption_Option_Implicit);
            _ftpsEncryptionComboBox.SelectedIndex = 0;

            _mtpDeviceLabel.Text = BUtil.Core.Localization.Resources.Field_Device;
            _mtpFolderLabel.Text = Resources.Field_Folder;

            _specifyFolderDirectoryStorageButton.Text = Resources.Field_Folder_Browse;
            _mountButton.Text = Resources.Task_Launch;
            _unmountButton.Text = Resources.Task_Launch;
        }

        public IStorageSettingsV2 StorageSettings
        {
            get
            {
                IStorageSettingsV2 storageSettings = null;
                if (_storageTypesTabControl.SelectedTab == _sambaTabPage)
                {
                    storageSettings = new SambaStorageSettingsV2
                    {
                        Url = _shareTextBox.Text,
                        SingleBackupQuotaGb = (long)_uploadLimitGbNumericUpDown.Value,
                        User = _userTextBox.Text,
                        Password = _passwordTextBox.Text,
                    };
                }
                else if (_storageTypesTabControl.SelectedTab == _hddStorageTabPage)
                {
                    storageSettings = new FolderStorageSettingsV2
                    {
                        DestinationFolder = destinationFolderTextBox.Text,
                        SingleBackupQuotaGb = (long)_uploadLimitGbNumericUpDownV2.Value,
                        MountPowershellScript = _mountTextBox.Text,
                        UnmountPowershellScript = _unmountTextBox.Text,
                    };
                }
                else if (_storageTypesTabControl.SelectedTab == _ftpsTabPage)
                {
                    storageSettings = new FtpsStorageSettingsV2
                    {
                        Host = _ftpsServerTextBox.Text,
                        Encryption = (FtpsStorageEncryptionV2)_ftpsEncryptionComboBox.SelectedIndex,
                        Port = (int)_ftpsPortNumericUpDown.Value,
                        User = _ftpsUserTextBox.Text,
                        Password = _ftpsPasswordTextBox.Text,
                        Folder = _ftpsFolderTextBox.Text,
                        SingleBackupQuotaGb = (long)_ftpsQuotaNumericUpDown.Value,
                    };
                }
                else if (_storageTypesTabControl.SelectedTab == _mtpTabPage)
                {
                    storageSettings = new MtpStorageSettings
                    {
                        Device = _mtpDeviceComboBox.Text,
                        Folder = _mtpFolderTextBox.Text,
                    };
                }
                if (storageSettings == null)
                    throw new NotImplementedException();

                return storageSettings;
            }
            set
            {
                if (value == null)
                    return;

                if (value is FolderStorageSettingsV2)
                {
                    var folderSettings = value as FolderStorageSettingsV2;
                    _storageTypesTabControl.SelectedTab = _hddStorageTabPage;

                    destinationFolderTextBox.Text = folderSettings.DestinationFolder;
                    _uploadLimitGbNumericUpDownV2.Value = folderSettings.SingleBackupQuotaGb;
                    _mountTextBox.Text = folderSettings.MountPowershellScript;
                    _unmountTextBox.Text = folderSettings.UnmountPowershellScript;
                }
                else if (value is SambaStorageSettingsV2)
                {
                    var sambaSettings = value as SambaStorageSettingsV2;
                    _storageTypesTabControl.SelectedTab = _sambaTabPage;

                    _shareTextBox.Text = sambaSettings.Url;
                    _uploadLimitGbNumericUpDown.Value = sambaSettings.SingleBackupQuotaGb;
                    _userTextBox.Text = sambaSettings.User;
                    _passwordTextBox.Text = sambaSettings.Password;
                }
                else if (value is FtpsStorageSettingsV2)
                {
                    var ftpsSettings = value as FtpsStorageSettingsV2;
                    _storageTypesTabControl.SelectedTab = _ftpsTabPage;

                    _ftpsServerTextBox.Text = ftpsSettings.Host;
                    _ftpsEncryptionComboBox.SelectedIndex = (int)ftpsSettings.Encryption;
                    _ftpsPortNumericUpDown.Value = ftpsSettings.Port;
                    _ftpsUserTextBox.Text = ftpsSettings.User;
                    _ftpsPasswordTextBox.Text = ftpsSettings.Password;
                    _ftpsFolderTextBox.Text = ftpsSettings.Folder;
                    _ftpsQuotaNumericUpDown.Value = ftpsSettings.SingleBackupQuotaGb;
                }
                else if (value is MtpStorageSettings)
                {
                    var mtpSettings = value as MtpStorageSettings;
                    _storageTypesTabControl.SelectedTab = _mtpTabPage;

                    _mtpDeviceComboBox.Text = mtpSettings.Device;
                    _mtpFolderTextBox.Text = mtpSettings.Folder;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        public override bool ValidateUi()
        {
            var storageSettings = StorageSettings;

            string error = null;
            using var progressForm = new ProgressForm(progress =>
            {
                progress(50);
                error = StorageFactory.Test(new StubLog(), storageSettings);
            });
            progressForm.ShowDialog();

            if (error != null)
            {
                Messages.ShowErrorBox(error);
                return false;
            }
            return true;
        }

        private void OnUploadLimitClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Messages.ShowInformationBox(Resources.DataStorage_Field_UploadQuota_Help);
        }

        private void searchButtonClick(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                destinationFolderTextBox.Text = fbd.SelectedPath;
            }
        }

        private void OnMountScript(object sender, EventArgs e)
        {
            if (PowershellProcessHelper.Execute(new StubLog(), _mountTextBox.Text))
                Messages.ShowInformationBox(Resources.DataStorage_Field_DisconnectionScript_Ok);
            else
                Messages.ShowErrorBox(Resources.DataStorage_Field_DisconnectionScript_Bad);
        }

        private void OnUnmount(object sender, EventArgs e)
        {
            if (PowershellProcessHelper.Execute(new StubLog(), _unmountTextBox.Text))
                Messages.ShowInformationBox(Resources.DataStorage_Field_DisconnectionScript_Ok);
            else
                Messages.ShowErrorBox(Resources.DataStorage_Field_DisconnectionScript_Bad);
        }

        private void OnTabIndexChanged(object sender, EventArgs e)
        {
            if (_storageTypesTabControl.SelectedTab == _mtpTabPage)
            {
                _mtpDeviceComboBox.DataSource = new MtpService().GetItems();
            }
        }
    }
}