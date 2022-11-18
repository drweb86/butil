using System;
using System.Windows.Forms;
using BUtil.Core.Storages;
using BUtil.Configurator.Localization;
using BUtil.Configurator.Configurator;
using System.Collections.Generic;
using System.Linq;
using BUtil.Core.Logs;

namespace BUtil.Configurator
{
    internal sealed partial class HddStorageForm : Form, IStorageConfigurationForm
	{
        private readonly IEnumerable<string> _forbiddenNames;

		private HddStorageSettings GetHddStorageSettings()
		{ 
			return new HddStorageSettings
            {
                Name = Caption,
                DestinationFolder = destinationFolderTextBox.Text,
				Enabled= _enabledCheckBox.Checked,
				UploadLimitGb = (long)_uploadLimitGbNumericUpDown.Value,
            };
        }

        public IStorageSettings GetStorageSettings()
		{
			return GetHddStorageSettings();
		}
        
        string Caption
        {
        	get { return _nameTextBox.Text.Trim(); }
        }
		
        
		public HddStorageForm(HddStorageSettings hddStorageSettings, IEnumerable<string> forbiddenNames)
		{
			InitializeComponent();
			
			if (hddStorageSettings != null)
			{
				_nameTextBox.Text = hddStorageSettings.Name;
				destinationFolderTextBox.Text = hddStorageSettings.DestinationFolder;
				acceptButton.Enabled = true;
				_enabledCheckBox.Checked = hddStorageSettings.Enabled;
				_uploadLimitGbNumericUpDown.Value= hddStorageSettings.UploadLimitGb;
			}
			
			whereToStoreBackupLabel.Text = Resources.SpecifyTheFolderWhereToStoreBackUp;
			this.Text = Resources.HddStorageConfiguration;
			acceptButton.Text = Resources.Ok;
			cancelButton.Text = Resources.Cancel;
			captionLabel.Text = Resources.Title;

			_limitUploadLabel.Text = BUtil.Configurator.Localization.Resources.UploadLimitGB;
			_limitUploadDescriptionLabel.Text = BUtil.Configurator.Localization.Resources.UploadLimitDescription;
			_enabledCheckBox.Text = BUtil.Configurator.Localization.Resources.Enabled;

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
            var storage = StorageFactory.Create(new StubLog(), storageSettings);
			var error = storage.Test();

            if (error != null)
            {
                Messages.ShowErrorBox(error);
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
	}
}
