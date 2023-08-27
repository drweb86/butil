using System;
using System.Windows.Forms;
using BUtil.Core.Storages;
using BUtil.Core.Options;
using BUtil.Configurator.Localization;
using System.Linq;
using BUtil.RestorationMaster;
using BUtil.Core.Logs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using BUtil.Core.Misc;
using BUtil.Core.BackupModels;

namespace BUtil.Configurator.Configurator.Controls
{
    internal sealed partial class WhereUserControl : Core.PL.BackUserControl
    {
        BackupTask _task;

        public WhereUserControl()
        {
            InitializeComponent();

            _limitUploadLabelV2.Text = _limitUploadLabel.Text = BUtil.Configurator.Localization.Resources.UploadLimitGB;
            _userLabel.Text = BUtil.Configurator.Localization.Resources.User;
            _passwordLabel.Text = BUtil.Configurator.Localization.Resources.Password;
            _shareLabel.Text = BUtil.Configurator.Localization.Resources.Url;


            whereToStoreBackupLabel.Text = Resources.SpecifyTheFolderWhereToStoreBackUp;
            _scriptsLabel.Text = BUtil.Configurator.Localization.Resources.HelpMountUnmountScript;
            _mountScriptLabel.Text = BUtil.Configurator.Localization.Resources.Mount;
            _unmountScriptLabel.Text = BUtil.Configurator.Localization.Resources.Unmount;
            _mountButton.Text = _unmountButton.Text = BUtil.Configurator.Localization.Resources.Run;
        }

        #region Overrides

        public override void ApplyLocalization()
        {
            _hddStorageTabPage.Text = Resources.HardDriveStorage;
        }

        public override void SetOptionsToUi(object settings)
        {
            _task = (BackupTask)settings;
            var options = (IncrementalBackupModelOptions)_task.Model;

            var storage = options.To;
            if (storage == null)
                return;

            if (storage is FolderStorageSettings)
            {
                var folderSettings = storage as FolderStorageSettings;
                _storageTypesTabControl.SelectedTab = _hddStorageTabPage;

                destinationFolderTextBox.Text = folderSettings.DestinationFolder;
                _uploadLimitGbNumericUpDownV2.Value = folderSettings.SingleBackupQuotaGb;
                _mountTextBox.Text = folderSettings.MountPowershellScript;
                _unmountTextBox.Text = folderSettings.UnmountPowershellScript;
            }
            else if (storage is SambaStorageSettings)
            {
                var sambaSettings = storage as SambaStorageSettings;
                _storageTypesTabControl.SelectedTab = _sambaTabPage;

                _shareTextBox.Text = sambaSettings.Url;
                _uploadLimitGbNumericUpDown.Value = sambaSettings.SingleBackupQuotaGb;
                _userTextBox.Text = sambaSettings.User;
                _passwordTextBox.Text = sambaSettings.Password;
            }
        }

        private IStorageSettings GetStorageSettings()
        {
            IStorageSettings storageSettings = null;
            if (_storageTypesTabControl.SelectedTab == _sambaTabPage)
            {
                storageSettings = new SambaStorageSettings
                {
                    Url = _shareTextBox.Text,
                    SingleBackupQuotaGb = (long)_uploadLimitGbNumericUpDown.Value,
                    User = _userTextBox.Text,
                    Password = _passwordTextBox.Text,
                };
            }
            else if (_storageTypesTabControl.SelectedTab == _hddStorageTabPage)
            {
                storageSettings = new FolderStorageSettings
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
            var options = (IncrementalBackupModelOptions)_task.Model;
            options.To = GetStorageSettings();
        }
        #endregion

        public override bool ValidateUi()
        {
            var storageSettings = GetStorageSettings();
            if (storageSettings is SambaStorageSettings)
            {
                var sambaStorageSettings = storageSettings as SambaStorageSettings;

                if (string.IsNullOrWhiteSpace(sambaStorageSettings.Url))
                {
                    Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.ShareNameIsNotSpecified);
                    return false;
                }
            }
            else if (storageSettings is FolderStorageSettings)
            {
                var folderStorageSettings = storageSettings as FolderStorageSettings;

                if (string.IsNullOrWhiteSpace(folderStorageSettings.DestinationFolder))
                {
                    Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.DestinationFolderIsNotSpecified);
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
                Messages.ShowInformationBox(BUtil.Configurator.Localization.Resources.FinishedSuccesfully);
            else
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.FinishedWithErrors);
        }

        private void OnUnmount(object sender, EventArgs e)
        {
            if (PowershellProcessHelper.Execute(new StubLog(), _unmountTextBox.Text))
                Messages.ShowInformationBox(BUtil.Configurator.Localization.Resources.FinishedSuccesfully);
            else
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.FinishedWithErrors);
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