using System;
using System.Windows.Forms;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;
using System.IO;
using BUtil.Core.Options;
using System.Text.Json;
using BUtil.Core.State;
using BUtil.Configurator;

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
			_helpLabel.Text = BUtil.Configurator.Localization.Resources.MountYourBackupLocationAsDiskOrCopyItToAnyFolderAndSpecifyItsLocation;
            _backupFolderLabel.Text = BUtil.Configurator.Localization.Resources.BackupFolder;
			_openBackupFolderButton.Text = BUtil.Configurator.Localization.Resources.OpenFolder;
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

			if (!Directory.Exists(backupFolder))
			{
				Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.BackupDirectoryDoesNotExist);
				return;
			}

            var backupFileName = Path.Combine(backupFolder, IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile);
            
			
			if (!File.Exists(backupFileName))
			{
                Messages.ShowErrorBox(string.Format(BUtil.Configurator.Localization.Resources.CannotLocateFile0InDirectoryPointToADirectoryContainingThisFile, IncrementalBackupModelConstants.StorageIncrementedNonEncryptedNonCompressedStateFile));
                return;
            }

			var json = File.ReadAllText(backupFileName);
			var incrementalBackupState = JsonSerializer.Deserialize<IncrementalBackupState>(json);
			Hide();
			using var restoreForm = new VersionsViewerForm(backupFolder, incrementalBackupState);
            restoreForm.ShowDialog();
			Close();
		}

        private void OnHelpClick(object sender, EventArgs e)
		{
            SupportManager.DoSupport(SupportRequest.RestorationWizard);
        }
	}
}
