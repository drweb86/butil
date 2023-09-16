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
        BackupTaskV2 _task;

        public WhereUserControl()
        {
            InitializeComponent();

            _limitUploadLabelV2.Text = _limitUploadLabel.Text = BUtil.Core.Localization.Resources.UploadLimitGB;
            _userLabel.Text = BUtil.Core.Localization.Resources.User;
            _passwordLabel.Text = BUtil.Core.Localization.Resources.Password;
            _shareLabel.Text = BUtil.Core.Localization.Resources.Url;


            whereToStoreBackupLabel.Text = Resources.SpecifyTheFolderWhereToStoreBackUp;
            _scriptsLabel.Text = BUtil.Core.Localization.Resources.HelpMountUnmountScript;
            _mountScriptLabel.Text = BUtil.Core.Localization.Resources.Mount;
            _unmountScriptLabel.Text = BUtil.Core.Localization.Resources.Unmount;
            _mountButton.Text = _unmountButton.Text = BUtil.Core.Localization.Resources.Run;
        }

        #region Overrides

        public override void ApplyLocalization()
        {
            _hddStorageTabPage.Text = Resources.HardDriveStorage;
        }

        public override void SetOptionsToUi(object settings)
        {
            _task = (BackupTaskV2)settings;
            var options = (IncrementalBackupModelOptionsV2)_task.Model;

            var storage = options.To;
            if (storage == null)
                return;

            if (storage is FolderStorageSettingsV2)
            {
                var folderSettings = storage as FolderStorageSettingsV2;
                _storageTypesTabControl.SelectedTab = _hddStorageTabPage;

                destinationFolderTextBox.Text = folderSettings.DestinationFolder;
                _uploadLimitGbNumericUpDownV2.Value = folderSettings.SingleBackupQuotaGb;
                _mountTextBox.Text = folderSettings.MountPowershellScript;
                _unmountTextBox.Text = folderSettings.UnmountPowershellScript;
            }
            else if (storage is SambaStorageSettingsV2)
            {
                var sambaSettings = storage as SambaStorageSettingsV2;
                _storageTypesTabControl.SelectedTab = _sambaTabPage;

                _shareTextBox.Text = sambaSettings.Url;
                _uploadLimitGbNumericUpDown.Value = sambaSettings.SingleBackupQuotaGb;
                _userTextBox.Text = sambaSettings.User;
                _passwordTextBox.Text = sambaSettings.Password;
            }
        }

        private IStorageSettingsV2 GetStorageSettings()
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
            if (storageSettings == null)
                throw new NotImplementedException();

            return storageSettings;
        }

        public override void GetOptionsFromUi()
        {
            var options = (IncrementalBackupModelOptionsV2)_task.Model;
            options.To = GetStorageSettings();
        }
        #endregion

        public override bool ValidateUi()
        {
            var storageSettings = GetStorageSettings();
            if (storageSettings is SambaStorageSettingsV2)
            {
                var sambaStorageSettings = storageSettings as SambaStorageSettingsV2;

                if (string.IsNullOrWhiteSpace(sambaStorageSettings.Url))
                {
                    Messages.ShowErrorBox(BUtil.Core.Localization.Resources.ShareNameIsNotSpecified);
                    return false;
                }
            }
            else if (storageSettings is FolderStorageSettingsV2)
            {
                var folderStorageSettings = storageSettings as FolderStorageSettingsV2;

                if (string.IsNullOrWhiteSpace(folderStorageSettings.DestinationFolder))
                {
                    Messages.ShowErrorBox(BUtil.Core.Localization.Resources.DestinationFolderIsNotSpecified);
                    return false;
                }

                if (folderStorageSettings.DestinationFolder.StartsWith(@"\\", StringComparison.InvariantCulture))
                {
                    //"Network storages are not allowed to be pointed here!"
                    Messages.ShowErrorBox(Resources.NetworkStoragesAreNotAllowedToBePointedHere);
                    return false;
                }
            }

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
            Messages.ShowInformationBox(Resources.UploadLimitDescription);
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
                Messages.ShowInformationBox(BUtil.Core.Localization.Resources.FinishedSuccesfully);
            else
                Messages.ShowErrorBox(BUtil.Core.Localization.Resources.FinishedWithErrors);
        }

        private void OnUnmount(object sender, EventArgs e)
        {
            if (PowershellProcessHelper.Execute(new StubLog(), _unmountTextBox.Text))
                Messages.ShowInformationBox(BUtil.Core.Localization.Resources.FinishedSuccesfully);
            else
                Messages.ShowErrorBox(BUtil.Core.Localization.Resources.FinishedWithErrors);
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