using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BULocalization;
using BUtil.Core.Storages;
using BUtil.Core.PL;

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
			deleteHereAllOtherBUtilImageFilesCheckbox.Text = Translation.Current[371];
			WhereToStoreBackupslabel.Text = Translation.Current[82];
			this.Text = Translation.Current[453];
			acceptButton.Text = Translation.Current[358];
			Cancelbutton.Text = Translation.Current[359];
			namelabel.Text = Translation.Current[360];
			OptionsgroupBox.Text = Translation.Current[361];
            _EncryptUnderLsaCheckBox.Text = Translation.Current[473];
            skipIfExceedsLimitCheckBox.Text = Translation.Current[486];

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
				Messages.ShowErrorBox(Translation.Current[452]);
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
			Messages.ShowInformationBox(Translation.Current[460]);
		}
		
		void requiredFieldsTextChanged(object sender, EventArgs e)
		{
			acceptButton.Enabled = (!string.IsNullOrEmpty(Caption));
		}
	}
}
