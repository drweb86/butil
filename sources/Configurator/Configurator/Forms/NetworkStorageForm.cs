using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BUtil.Core.Storages;
using BUtil.Core.PL;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator
{
    internal sealed partial class NetworkStorageForm : Form, IStorageConfigurationForm
	{
    	#region Properties
    	
    	string Caption
    	{
    		get { return captionTextBox.Text.Trim(); }
    		set { captionTextBox.Text = value; }
    	}
    	
    	string UncLocation
    	{
    		get { return destinationFolderTextBox.Text.Trim(); }
    		set { destinationFolderTextBox.Text = value; }
    	}

        StorageBase IStorageConfigurationForm.Storage
		{
            get { return new NetworkStorage(Caption, UncLocation, deleteHereAllOtherBUtilImageFilesCheckbox.Checked, _EncryptUnderLsaCheckBox.Checked, skipIfExceedsLimitCheckBox.Checked, Convert.ToInt64(limitSizeNumericUpDown.Value)); }
		}

        #endregion

		public NetworkStorageForm(NetworkStorage storage)
		{
			InitializeComponent();

			if (storage != null)
			{
				Caption = storage.StorageName;
				UncLocation = storage.DestinationFolder;
				deleteHereAllOtherBUtilImageFilesCheckbox.Checked = storage.DeleteBUtilFilesInDestinationFolderBeforeBackup;
                _EncryptUnderLsaCheckBox.Checked = storage.EncryptUnderLocalSystemAccount;
                skipIfExceedsLimitCheckBox.Checked = storage.SkipCopyingToNetworkStorageAndWriteWarningInLogIfBackupExceeds;
                limitSizeNumericUpDown.Value = storage.SkipCopyingToNetworkStorageLimitMb;
                skipIfExceedsLimitCheckBox_CheckedChanged(null, null);
			}

			// locals
			deleteHereAllOtherBUtilImageFilesCheckbox.Text = Resources.DeleteHereAllOtherButilImageFiles;
			WhereToStoreBackupslabel.Text = Resources.SpecifyTheFolderWhereToStoreBackUp;
			this.Text = Resources.NetworkStorageConfiguration;
			acceptButton.Text = Resources.Ok;
			Cancelbutton.Text = Resources.Cancel;
			namelabel.Text = Resources.Title;
			OptionsgroupBox.Text = Resources.Options;
            _EncryptUnderLsaCheckBox.Text = Resources.AdditionalEncryptionUnderYourLocalSystemAccountNrequiresNtfsFileSystemAndWin2kOsOnRemoteMachineBackupDataWillBeAvailableOnlyForYourOsAccountSeeDocumentationForDetails;
            skipIfExceedsLimitCheckBox.Text = Resources.SkipCopyingAndWriteAWarningIntoLogIfBackupExceeds;

            requiredFieldsTextChanged(null, null);
		}

		void cancelButtonClick(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		void acceptButtonClick(object sender, EventArgs e)
		{
			if (!checkIfNetworkHelper(UncLocation))
			{
				return;
			}

			this.DialogResult = DialogResult.OK;
		}

		bool checkIfNetworkHelper(string uncPath)
		{
			if (!uncPath.StartsWith(@"\\", StringComparison.CurrentCulture))
			{
				Messages.ShowErrorBox(Resources.TargetFolderShouldHaveUncNameForExampleServer1Sharedresource1);
				return false;
			}
			else
				return true;
		}

		void searchbutton_Click(object sender, EventArgs e)
		{
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				if (checkIfNetworkHelper(fbd.SelectedPath))
				{
					UncLocation = fbd.SelectedPath;
				}
			}
		}

        void skipIfExceedsLimitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            limitSizeNumericUpDown.Enabled = skipIfExceedsLimitCheckBox.Checked;
        }
		
		void helpButtonClick(object sender, EventArgs e)
		{
			Messages.ShowInformationBox(Resources.ImportantNNbackupShouldBePasswordProtectedIfNotProgramWillSkipItsCopyingToThisStorage);
		}
		
		void requiredFieldsTextChanged(object sender, EventArgs e)
		{
			acceptButton.Enabled = (!string.IsNullOrEmpty(Caption));
		}
	}
}
