using System;
using System.Windows.Forms;

using BUtil.Core.Storages;
using BUtil.Core.PL;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator
{
    internal sealed partial class FtpStorageForm : Form, IStorageConfigurationForm
	{
    	
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

		FtpStorageSettings GetFtpStorageSettings()
		{
			return new FtpStorageSettings
			{
				ActiveFtpMode = connectionModeComboBox.SelectedIndex == 0,
				Name = Caption,
				DestinationFolder = DestinationFolder,
				DeleteBUtilFilesInDestinationFolderBeforeBackup = deleteHereAllOtherBUtilImageFilesCheckbox.Checked,
				Host = FtpServer,
				User = User,
				Password = Password
			};


		}


		StorageSettings IStorageConfigurationForm.GetStorageSettings()
		{
			var ftpStorageSettings = GetFtpStorageSettings();
			return StorageFactory.CreateStorageSettings(ftpStorageSettings);
		}        
		public FtpStorageForm(StorageSettings storageSettings)
		{
			InitializeComponent();
			
			// applying locals
			StorageNamelabel.Text = Resources.StorageName;
			destinationFolderGroupBox.Text = Resources.DestinationFolder;
			deleteHereAllOtherBUtilImageFilesCheckbox.Text = Resources.DeleteHereAllOtherButilImageFiles;
			AuthorizationInformationGroupBox.Text = Resources.AuthorizationInformation;
			userLabel.Text = Resources.User;
			passwordLabel.Text = Resources.Password;
			RemoteServergroupBox.Text = Resources.FtpServer;
			connectionModeComboBox.Items.Clear();
			connectionModeComboBox.Items.Add(Resources.Active);
			connectionModeComboBox.Items.Add(Resources.Passive);
			dataTransferModeLabel.Text = Resources.DataTransferMode;
			hostlabel.Text = Resources.DnsNameOrIpFtp;
			acceptButton.Text = Resources.Ok;
			CANCELbutton.Text = Resources.Cancel;
			testButton.Text = Resources.Test;
			this.Text = Resources.FtpStorageConfiguration;
			
			connectionModeComboBox.SelectedIndex = 1;
			
			if (storageSettings != null)
			{
				var ftpStorageSettings = StorageFactory.CreateFtpStorageSettings(storageSettings);

				Caption = ftpStorageSettings.Name;
				DestinationFolder = ftpStorageSettings.DestinationFolder;
				deleteHereAllOtherBUtilImageFilesCheckbox.Checked = ftpStorageSettings.DeleteBUtilFilesInDestinationFolderBeforeBackup;
				FtpServer = ftpStorageSettings.Host;
				User = ftpStorageSettings.User;
				Password = ftpStorageSettings.Password;
				connectionModeComboBox.SelectedIndex = ftpStorageSettings.ActiveFtpMode ? 0 : 1;
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
				var settings = GetFtpStorageSettings();
				var storage = StorageFactory.Create(settings);
				storage.Test();
			}
			catch (Exception exc)
			{
                Messages.ShowErrorBox(exc.Message);
				ok = false;
			}
			
			if (ok) 
			{
				// "Works fine!"
				Messages.ShowInformationBox(Resources.WorksFine);
			}
		}
		
		void helpButtonClick(object sender, EventArgs e)
		{
			Messages.ShowInformationBox(Resources._1BackupShouldBePasswordProtectedIfNotProgramWillSkipItsCopyingToThisStorageN2YourAuthorizationInformationIsGoingOverTheNetworkInUnprotectedModeAsPlainText);
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
