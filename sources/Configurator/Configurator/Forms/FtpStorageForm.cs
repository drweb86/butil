using System;
using System.Windows.Forms;
using BULocalization;
using BUtil.Core.Storages;
using BUtil.Core.PL;

namespace BUtil.Configurator
{
    internal sealed partial class FtpStorageForm : Form, IStorageConfigurationForm
	{
    	#region Properties
    	
    	string Caption
    	{
    		get { return captionTextBox.Text.Trim(); }
    		set { captionTextBox.Text = value; }
    	}
    	
    	string FtpServer
    	{
    		get { return ftpServerTextBox.Text.Trim(); }
    		set { ftpServerTextBox.Text = value; }
    	}
    	
    	string User
    	{
    		get { return userTextBox.Text.Trim(); }
    		set { userTextBox.Text = value; }
    	}
    	
    	string DestinationFolder
    	{
    		get { return destinationFolderTextBox.Text.Trim(); }
    		set { destinationFolderTextBox.Text = value; }
    	}
    	
    	string Password
    	{
    		get { return passwordTextBox.Text; }
    		set { passwordTextBox.Text = value; }
    	}
    	
        StorageBase IStorageConfigurationForm.Storage
		{
			get { 
				bool isActive = (connectionModeComboBox.SelectedIndex == 0);
				return new FtpStorage(Caption, DestinationFolder, deleteHereAllOtherBUtilImageFilesCheckbox.Checked,
			                            FtpServer, User, Password, isActive); }
		}
		
        #endregion

		public FtpStorageForm(FtpStorage storage)
		{
			InitializeComponent();
			
			// applying locals
			StorageNamelabel.Text = Translation.Current[368];
			destinationFolderGroupBox.Text = Translation.Current[370];
			deleteHereAllOtherBUtilImageFilesCheckbox.Text = Translation.Current[371];
			AuthorizationInformationGroupBox.Text = Translation.Current[373];
			userLabel.Text = Translation.Current[374];
			passwordLabel.Text = Translation.Current[375];
			RemoteServergroupBox.Text = Translation.Current[376];
			connectionModeComboBox.Items.Clear();
			connectionModeComboBox.Items.Add(Translation.Current[377]);
			connectionModeComboBox.Items.Add(Translation.Current[378]);
			dataTransferModeLabel.Text = Translation.Current[379];
			hostlabel.Text = Translation.Current[380];
			acceptButton.Text = Translation.Current[381];
			CANCELbutton.Text = Translation.Current[382];
			testButton.Text = Translation.Current[383];
			this.Text = Translation.Current[384];
			
			connectionModeComboBox.SelectedIndex = 1;
			
			if (storage != null)
			{
				Caption = storage.StorageName;
				DestinationFolder = storage.DestinationFolder;
				deleteHereAllOtherBUtilImageFilesCheckbox.Checked = storage.DeleteBUtilFilesInDestinationFolderBeforeBackup;
				FtpServer = storage.RemoteHostServer;
				User = storage.User;
				Password = storage.Password;
				connectionModeComboBox.SelectedIndex = storage.FtpModeIsActive ? 0 : 1;
			}

			requiredFieldsTextChanged(null, null);
		}
		
		void okButtonClick(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}
		
		void testButtonClick(object sender, EventArgs e)
		{
			bool ok = true;
			try
			{
                (((IStorageConfigurationForm)this).Storage as FtpStorage).Test();
			}
			catch (Exception exc)
			{
                Messages.ShowErrorBox(exc.Message);
				ok = false;
			}
			
			if (ok) 
			{
				// "Works fine!"
				Messages.ShowInformationBox(Translation.Current[385]);
			}
		}
		
		void helpButtonClick(object sender, EventArgs e)
		{
			Messages.ShowInformationBox(Translation.Current[369]);
		}
		
		void requiredFieldsTextChanged(object sender, EventArgs e)
		{
			bool enabled = (!string.IsNullOrEmpty(Caption)) && (!string.IsNullOrEmpty(FtpServer)) &&
				(!string.IsNullOrEmpty(User)) && (!string.IsNullOrEmpty(DestinationFolder)) &&
				(!string.IsNullOrEmpty(Password));
			
			acceptButton.Enabled = enabled;
			testButton.Enabled = enabled;
		}
	}
}
