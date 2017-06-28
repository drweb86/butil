using System;
using System.Drawing;
using System.Windows.Forms;
using BULocalization;
using BUtil.Core.Options;
using BUtil.Core.Storages;
using BUtil.Core.PL;

namespace BUtil.Configurator
{
	/// <summary>
	/// Description of HDDStorageForm.
	/// </summary>
    internal sealed partial class HddStorageForm : Form, IStorageConfigurationForm
	{
    	#region Properties
    	
        StorageBase IStorageConfigurationForm.Storage
		{
			get { return new HddStorage(Caption, destinationFolderTextBox.Text, deleteHereAllOtherBUtilImageFilesCheckbox.Checked); }
		}
        
        string Caption
        {
        	get { return captionTextBox.Text.Trim(); }
        }
		
        #endregion
		
		public HddStorageForm(HddStorage storage)
		{
			InitializeComponent();
			
			if (storage != null)
			{
				captionTextBox.Text = storage.StorageName;
				destinationFolderTextBox.Text = storage.DestinationFolder;
				deleteHereAllOtherBUtilImageFilesCheckbox.Checked = storage.DeleteBUtilFilesInDestinationFolderBeforeBackup;
				acceptButton.Enabled = true;
			}
			
			// locals
			deleteHereAllOtherBUtilImageFilesCheckbox.Text = Translation.Current[371];
			whereToStoreBackupLabel.Text = Translation.Current[82];
			this.Text = Translation.Current[357];
			acceptButton.Text = Translation.Current[358];
			cancelButton.Text = Translation.Current[359];
			captionLabel.Text = Translation.Current[360];
			optionsGroupBox.Text = Translation.Current[361];
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
				Messages.ShowErrorBox(Translation.Current[450]);
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
