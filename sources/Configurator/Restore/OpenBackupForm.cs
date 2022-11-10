using System;
using System.Windows.Forms;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;
using System.IO;
using BUtil.Core.Options;
using System.Text.Json;
using BUtil.Core.State;

namespace BUtil.RestorationMaster
{
	public partial class OpenBackupForm : Form
	{
		public OpenBackupForm(string backupFolder = null)
		{
			InitializeComponent();

			if (backupFolder != null)
			{
				SetBackupLocation(backupFolder);
			}

            ApplyLocals();
		}

        private void ApplyLocals()
        { 
			closeButton.Text = Resources.Close;
			passwordLabel.Text = Resources.IfYourBackupIsPasswordProtectedPleaseTypePasswordHere;
			continueButton.Text = Resources.Continue;
			this.Text = Resources.RestorationMaster;
			continueButton.Left = closeButton.Left - continueButton.Width - 10;
        }

        private void SetBackupLocation(string backupLocation)
		{
			continueButton.Enabled = true;
			_backupLocationTextBox.Text = backupLocation;
		}

		private void OnSelectBackupLocationClick(object sender, EventArgs e)
		{
			if (_fbd.ShowDialog() == DialogResult.OK)
				SetBackupLocation(_fbd.SelectedPath);
		}

        private void OnCloseButtonClick(object sender, EventArgs e)
        {
        	DialogResult = DialogResult.OK;
        	Close();
        }

        private void OnNextButtonClick(object sender, EventArgs e)
		{
			var backupFolder = _backupLocationTextBox.Text;
            var backupFileName = Path.Combine(backupFolder, IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile);
            var json = File.ReadAllText(backupFileName);
			var incrementalBackupState = JsonSerializer.Deserialize<IncrementalBackupState>(json);

			using var restoreForm = new VersionsViewerForm(backupFolder, incrementalBackupState);
            restoreForm.ShowDialog();
		}

        private void OnHelpClick(object sender, EventArgs e)
		{
            SupportManager.DoSupport(SupportRequest.RestorationWizard);
        }
	}
}
