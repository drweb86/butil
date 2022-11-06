using System;
using System.Windows.Forms;
using BUtil.Core.Storages;
using BUtil.Core.PL;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator
{
    internal sealed partial class HddStorageForm : Form, IStorageConfigurationForm
	{
		private HddStorageSettings GetHddStorageSettings()
		{ 
			return new HddStorageSettings
            {
                Name = Caption,
                DestinationFolder = destinationFolderTextBox.Text,
            };
        }

        public StorageSettings GetStorageSettings()
		{
			var hddStorageSettings = GetHddStorageSettings();
			return StorageFactory.CreateStorageSettings(hddStorageSettings);
		}
        
        string Caption
        {
        	get { return captionTextBox.Text.Trim(); }
        }
		
        
		public HddStorageForm(StorageSettings storageSettings)
		{
			InitializeComponent();
			
			if (storageSettings != null)
			{
				var hddStorageSettings = StorageFactory.CreateHddStorageSettings(storageSettings);

				captionTextBox.Text = hddStorageSettings.Name;
				destinationFolderTextBox.Text = hddStorageSettings.DestinationFolder;
				acceptButton.Enabled = true;
			}
			
			whereToStoreBackupLabel.Text = Resources.SpecifyTheFolderWhereToStoreBackUp;
			this.Text = Resources.HddStorageConfiguration;
			acceptButton.Text = Resources.Ok;
			cancelButton.Text = Resources.Cancel;
			captionLabel.Text = Resources.Title;
			optionsGroupBox.Text = Resources.Options;
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
            if (captionTextBox.Text.StartsWith(@"\\", StringComparison.InvariantCulture))
			{
				//"Network storages are not allowed to be pointed here!"
				Messages.ShowErrorBox(Resources.NetworkStoragesAreNotAllowedToBePointedHere);
				return;
			}
			this.DialogResult = DialogResult.OK;
		}
		
		void requiredFieldsTextChanged(object sender, EventArgs e)
		{
			acceptButton.Enabled = (!string.IsNullOrEmpty(destinationFolderTextBox.Text)) && (!string.IsNullOrEmpty(Caption));
		}
	}
}
