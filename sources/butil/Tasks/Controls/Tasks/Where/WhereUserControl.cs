using System;
using System.Windows.Forms;
using BUtil.Core.Storages;
using BUtil.Core.Localization;
using BUtil.RestorationMaster;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Configurator.Configurator.Controls
{
    internal sealed partial class WhereUserControl : Core.PL.BackUserControl
    {
        public WhereUserControl()
        {
            InitializeComponent();

            _limitUploadLabelV2.Text = _limitUploadLabel.Text = BUtil.Core.Localization.Resources.DataStorage_Field_UploadQuota;
            _userLabel.Text = BUtil.Core.Localization.Resources.User_Field;
            _passwordLabel.Text = BUtil.Core.Localization.Resources.Password_Field;
            _shareLabel.Text = BUtil.Core.Localization.Resources.Url_Field;


            whereToStoreBackupLabel.Text = Resources.DirectoryStorage_Field_Directory;
            _scriptsLabel.Text = BUtil.Core.Localization.Resources.DataStorage_Script_Help;
            _mountScriptLabel.Text = BUtil.Core.Localization.Resources.DataStorage_Field_ConnectScript;
            _unmountScriptLabel.Text = BUtil.Core.Localization.Resources.DataStorage_Field_DisconnectionScript;
            _mountButton.Text = _unmountButton.Text = BUtil.Core.Localization.Resources.Task_Launch;

            _ftpsQuotaLabel.Text = Resources.DataStorage_Field_UploadQuota;
            _ftpsServerLabel.Text = Resources.Server_Field_Address;
            _ftpsPortLabel.Text = Resources.Server_Field_Port;
            _ftpsUserLabel.Text = Resources.User_Field;
            _ftpsPasswordLabel.Text = Resources.Password_Field;
            _ftpsFolderLabel.Text = Resources.Field_Folder;

            _hddStorageTabPage.Text = Resources.DirectoryStorage;
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
                        Port = (int)_ftpsPortNumericUpDown.Value,
                        User = _ftpsUserTextBox.Text,
                        Password = _ftpsPasswordTextBox.Text,
                        Folder = _ftpsFolderTextBox.Text,
                        SingleBackupQuotaGb = (long)_ftpsQuotaNumericUpDown.Value,
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
                    _ftpsPortNumericUpDown.Value = ftpsSettings.Port;
                    _ftpsUserTextBox.Text = ftpsSettings.User;
                    _ftpsPasswordTextBox.Text = ftpsSettings.Password;
                    _ftpsFolderTextBox.Text = ftpsSettings.Folder;
                    _ftpsQuotaNumericUpDown.Value = ftpsSettings.SingleBackupQuotaGb;
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


        void searchButtonClick(object sender, EventArgs e)
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

        private void OnSambaButtonClick(object sender, EventArgs e)
        {
            using var form = new SambaForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _mountTextBox.Text = form.MountScript;
                _unmountTextBox.Text = form.UnmountScript;
            }
        }
    }
}