using System;
using System.Windows.Forms;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;
using System.IO;
using BUtil.Core.Options;
using System.Text.Json;
using BUtil.Core.State;
using BUtil.Configurator;
using System.Linq;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Core.BackupModels;
using System.Threading;

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

			if (!IncrementalBackupModelConstants.Files.Any(x => File.Exists(Path.Combine(backupFolder, x))))
			{
				var allowedFiles = string.Join(", ", IncrementalBackupModelConstants.Files);
                Messages.ShowErrorBox(string.Format(BUtil.Configurator.Localization.Resources.CannotLocateFile0InDirectoryPointToADirectoryContainingThisFile, allowedFiles));
                return;
            }

			var log = new StubLog();
			IStorageSettings storageSettings = new HddStorageSettings
			{
				Name = string.Empty,
				DestinationFolder = backupFolder
			};
			var task = new BackupTask
			{ 
				Password = _passwordTextBox.Text, 
				Model = new IncrementalBackupModelOptions()
			};
			var service = new IncrementalBackupStateService(log, storageSettings, task, ProgramOptionsManager.Default);
			var cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = cancellationTokenSource.Token;
			if (!service.TryRead(cancellationToken, out var state))
			{
				Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.CannotOpenBackupFolder);
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
