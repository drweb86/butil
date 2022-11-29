using System;
using System.Windows.Forms;
using BUtil.Core.Storages;
using BUtil.Configurator.Localization;
using BUtil.Configurator.Configurator;
using System.Collections.Generic;
using System.Linq;
using BUtil.Core.Logs;
using BUtil.Core.Misc;

namespace BUtil.Configurator
{
    internal sealed partial class FolderStorageForm : Form, IStorageConfigurationForm
	{
        private readonly IEnumerable<string> _forbiddenNames;

		private FolderStorageSettings GetFolderStorageSettings()
		{ 
			return new FolderStorageSettings
            {
                Name = Caption,
                DestinationFolder = destinationFolderTextBox.Text,
				Enabled= _enabledCheckBox.Checked,
				SingleBackupQuotaGb = (long)_uploadLimitGbNumericUpDown.Value,
				MountPowershellScript = _mountTextBox.Text,
				UnmountPowershellScript = _unmountTextBox.Text,
            };
        }

        public IStorageSettings GetStorageSettings()
		{
			return GetFolderStorageSettings();
		}
        
        string Caption
        {
        	get { return _nameTextBox.Text.Trim(); }
        }
		
        
		public FolderStorageForm(FolderStorageSettings folderStorageSettings, IEnumerable<string> forbiddenNames)
		{
			InitializeComponent();
			
			if (folderStorageSettings != null)
			{
				_nameTextBox.Text = folderStorageSettings.Name;
				destinationFolderTextBox.Text = folderStorageSettings.DestinationFolder;
				acceptButton.Enabled = true;
				_enabledCheckBox.Checked = folderStorageSettings.Enabled;
				_uploadLimitGbNumericUpDown.Value= folderStorageSettings.SingleBackupQuotaGb;
				_mountTextBox.Text = folderStorageSettings.MountPowershellScript;
                _unmountTextBox.Text = folderStorageSettings.UnmountPowershellScript;
            }
			
			whereToStoreBackupLabel.Text = Resources.SpecifyTheFolderWhereToStoreBackUp;
			this.Text = Resources.FolderStorageConfiguration;
			acceptButton.Text = Resources.Ok;
			cancelButton.Text = Resources.Cancel;
			captionLabel.Text = Resources.Title;

			_limitUploadLabel.Text = BUtil.Configurator.Localization.Resources.UploadLimitGB;
			_limitUploadDescriptionLabel.Text = BUtil.Configurator.Localization.Resources.UploadLimitDescription;
			_enabledCheckBox.Text = BUtil.Configurator.Localization.Resources.Enabled;
			_scriptsLabel.Text = BUtil.Configurator.Localization.Resources.HelpMountUnmountScript;
			_mountScriptLabel.Text = BUtil.Configurator.Localization.Resources.Mount;
            _unmountScriptLabel.Text = BUtil.Configurator.Localization.Resources.Unmount;
			_mountButton.Text = _unmountButton.Text = BUtil.Configurator.Localization.Resources.Run;

            this._forbiddenNames = forbiddenNames;
		}
		
		void searchButtonClick(object sender, EventArgs e)
		{
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				destinationFolderTextBox.Text = fbd.SelectedPath;
			}
		}
		
		void acceptButtonClick(object sender, EventArgs e)
		{
            if (string.IsNullOrWhiteSpace(_nameTextBox.Text))
            {
                Messages.ShowErrorBox(Resources.NameIsEmpty);
                return;
            }

            if (_nameTextBox.Text.StartsWith(@"\\", StringComparison.InvariantCulture))
			{
				//"Network storages are not allowed to be pointed here!"
				Messages.ShowErrorBox(Resources.NetworkStoragesAreNotAllowedToBePointedHere);
				return;
			}

            if (_forbiddenNames.Any(x => x == _nameTextBox.Text))
            {
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.ThisNameIsAlreadyTakenTryAnotherOne);
                return;
            }

			var storageSettings = GetStorageSettings();

			try
			{
				using (var storage = StorageFactory.Create(new StubLog(), storageSettings))
				{
					var error = storage.Test();

					if (error != null)
					{
						Messages.ShowErrorBox(error);
						return;
					}
				}
			}
			catch (Exception ex)
			{
                Messages.ShowErrorBox(ex.Message);
                return;
            }

            this.DialogResult = DialogResult.OK;
		}
		
		void requiredFieldsTextChanged(object sender, EventArgs e)
		{
			acceptButton.Enabled = (!string.IsNullOrEmpty(destinationFolderTextBox.Text)) && (!string.IsNullOrEmpty(Caption));
		}

		private void OnNameChange(object sender, EventArgs e)
		{
            var trimmedText = TaskNameStringHelper.TrimIllegalChars(_nameTextBox.Text);
            if (trimmedText != _nameTextBox.Text)
            {
                _nameTextBox.Text = trimmedText;
            }

            requiredFieldsTextChanged(sender, e);

        }

        private void OnRunMountScript(object sender, EventArgs e)
        {
            if (PowershellProcessHelper.Execute(new StubLog(), _mountTextBox.Text))
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.FinishedSuccesfully);
            else
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.FinishedWithErrors);
        }

        private void OnMountRun(object sender, EventArgs e)
        {
            if (PowershellProcessHelper.Execute(new StubLog(), _unmountTextBox.Text))
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.FinishedSuccesfully);
            else
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.FinishedWithErrors);
        }
    }
}
