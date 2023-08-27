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
                } else
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
