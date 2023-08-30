using BUtil.ConsoleBackup.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using System;
using Terminal.Gui;

namespace BUtil.ConsoleBackup.UI
{
    public partial class EditStorageSettingsDialog
    {
        public IStorageSettings StorageSettings { get; private set; }

        internal EditStorageSettingsDialog(IStorageSettings source) 
        {
            Title = BUtil.ConsoleBackup.Localization.Resources.SpecifyLocation;
            InitializeComponent();

            if (source == null)
            {
                _tabView.SelectedTab = _folderStorageTab;
            }
            else
            {
                if (source is FolderStorageSettings)
                {
                    _tabView.SelectedTab = _folderStorageTab;
                    _folderStorageFolderTextField.Text = ((FolderStorageSettings)source).DestinationFolder;
                } if (source is FtpsStorageSettings)
                {
                    _tabView.SelectedTab = _ftpsStorageTab;
                    var storageSettings = (FtpsStorageSettings)source;
                    _hostFtpsStorageFolderTextField.Text = storageSettings.Host;
                    _portFtpsStorageFolderTextField.Text = storageSettings.Port.ToString();
                    _userFtpsStorageFolderTextField.Text = storageSettings.User;
                    _pwdFtpsStorageFolderTextField.Text = storageSettings.Password;
                    _folderFtpsStorageFolderTextField.Text = storageSettings.Folder;
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

        private void OnSave()
        {
            IStorageSettings storageSettings = null;
            if (_tabView.SelectedTab == _folderStorageTab)
            {
                storageSettings = new FolderStorageSettings
                {
                    DestinationFolder = this._folderStorageFolderTextField.Text.ToString(),
                };
            }
            else if (_tabView.SelectedTab == _ftpsStorageTab)
            {
                storageSettings = new FtpsStorageSettings
                {
                    Host = _hostFtpsStorageFolderTextField.Text.ToString(),
                    Port = int.TryParse(_portFtpsStorageFolderTextField.Text.ToString(), out var port) ? port : -1,
                    User = _userFtpsStorageFolderTextField.Text.ToString(),
                    Folder = _folderFtpsStorageFolderTextField.Text.ToString(),
                    Password = _pwdFtpsStorageFolderTextField.Text.ToString()
                };
            }
            else
            {
                throw new NotSupportedException();
            }

            var error = StorageFactory.Test(new StubLog(), storageSettings);
            if (error != null)
            {
                MessageBox.ErrorQuery(string.Empty, error, Resources.Close);
                return;
            }

            Canceled = false;
            StorageSettings = storageSettings;
            Application.RequestStop();
        }
    }
}
