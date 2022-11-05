using System;
using System.Windows.Forms;
using BUtil.Core.Misc;
using BUtil.Configurator.Localization;

namespace BUtil.RestorationMaster
{
	public partial class RestoreMasterMainForm : Form
	{
		private readonly RestoreController _controller = new ();
		
		public RestoreMasterMainForm(string backupFolder = null)
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
			_controller.OpenImage(_backupLocationTextBox.Text, passwordMaskedTextBox.Text);
		}

        private void OnHelpClick(object sender, EventArgs e)
		{
            SupportManager.DoSupport(SupportRequest.RestorationWizard);
        }
	}
}
