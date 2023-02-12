using System;
using System.Windows.Forms;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;
using System.IO;
using BUtil.Core.State;
using BUtil.Configurator;
using System.Linq;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using System.Threading;
using BUtil.Core.TasksTree.IncrementalModel;

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
				Messages.ShowErrorBox(Resources.BackupDirectoryDoesNotExist);
				return;
			}

			if (!IncrementalBackupModelConstants.Files.Any(x => File.Exists(Path.Combine(backupFolder, x))))
			{
				var allowedFiles = string.Join(", ", IncrementalBackupModelConstants.Files);
                Messages.ShowErrorBox(string.Format(Resources.CannotLocateFile0InDirectoryPointToADirectoryContainingThisFile, allowedFiles));
                return;
            }

			var log = new StubLog();
			IStorageSettings storageSettings = new FolderStorageSettings
			{
				Name = string.Empty,
				DestinationFolder = backupFolder
			};
			var commonServicesIoc = new CommonServicesIoc();
			var services = new StorageSpecificServicesIoc(log, storageSettings, commonServicesIoc.HashService);
			if (!services.IncrementalBackupStateService.TryRead(_passwordTextBox.Text, out var state))
			{
				Messages.ShowErrorBox(Resources.CannotOpenBackupFolder);
				return;
			}

			Hide();
			using var restoreForm = new VersionsViewerForm(backupFolder, state);
            restoreForm.ShowDialog();
			Close();
		}

        private void OnHelpClick(object sender, EventArgs e)
		{
            SupportManager.DoSupport(SupportRequest.RestorationWizard);
        }
	}
}
