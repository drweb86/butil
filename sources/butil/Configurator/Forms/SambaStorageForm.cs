using System;
using System.Windows.Forms;
using BUtil.Core.Storages;
using BUtil.Configurator.Localization;
using BUtil.Configurator.Configurator;
using System.Collections.Generic;
using System.Linq;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.RestorationMaster;

namespace BUtil.Configurator
{
    internal sealed partial class SambaStorageForm : Form, IStorageConfigurationForm
	{
        private readonly IEnumerable<string> _forbiddenNames;

		private SambaStorageSettings GetSambaStorageSettings()
		{ 
			return new SambaStorageSettings
            {
                Name = Caption,
                Url = _shareTextBox.Text,
				Enabled= _enabledCheckBox.Checked,
				SingleBackupQuotaGb = (long)_uploadLimitGbNumericUpDown.Value,
				User = _userTextBox.Text,
				Password = _passwordTextBox.Text,
            };
        }

        public IStorageSettings GetStorageSettings()
		{
			return GetSambaStorageSettings();
		}
        
        string Caption
        {
        	get { return _nameTextBox.Text.Trim(); }
        }
		
        
		public SambaStorageForm(SambaStorageSettings settings, IEnumerable<string> forbiddenNames)
		{
			InitializeComponent();
			
			if (settings != null)
			{
				_nameTextBox.Text = settings.Name;
				_shareTextBox.Text = settings.Url;
				acceptButton.Enabled = true;
				_enabledCheckBox.Checked = settings.Enabled;
				_uploadLimitGbNumericUpDown.Value= settings.SingleBackupQuotaGb;
				_userTextBox.Text = settings.User;
                _passwordTextBox.Text = settings.Password;
            }
			
			acceptButton.Text = Resources.Ok;
			cancelButton.Text = Resources.Cancel;
			captionLabel.Text = Resources.Title;

			_limitUploadLabel.Text = BUtil.Configurator.Localization.Resources.UploadLimitGB;
			_enabledCheckBox.Text = BUtil.Configurator.Localization.Resources.Enabled;
			_userLabel.Text = BUtil.Configurator.Localization.Resources.User;
            _passwordLabel.Text = BUtil.Configurator.Localization.Resources.Password;
            _shareLabel.Text = BUtil.Configurator.Localization.Resources.Url;

            this._forbiddenNames = forbiddenNames;
		}
				
		void acceptButtonClick(object sender, EventArgs e)
		{
            if (string.IsNullOrWhiteSpace(_nameTextBox.Text))
            {
                Messages.ShowErrorBox(Resources.NameIsEmpty);
                return;
            }

            if (_nameTextBox.Text.StartsWith(@"\\", StringComparison.InvariantCulture))
			{
				//"Network storages are not allowed to be pointed here!"
				Messages.ShowErrorBox(Resources.NetworkStoragesAreNotAllowedToBePointedHere);
				return;
			}

            if (_forbiddenNames.Any(x => x == _nameTextBox.Text))
            {
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.ThisNameIsAlreadyTakenTryAnotherOne);
                return;
            }

			var storageSettings = GetStorageSettings();

			string error = null;
			using var progressForm = new ProgressForm(progress =>
			{
				progress(50);
                error = StorageFactory.Test(new StubLog(), storageSettings);
			});
            progressForm.ShowDialog();

            if (error != null)
            {
                Messages.ShowErrorBox(error);
                return;
            }

            this.DialogResult = DialogResult.OK;
		}
		
		void RequiredFieldsTextChanged(object sender, EventArgs e)
		{
			acceptButton.Enabled = (!string.IsNullOrEmpty(_shareTextBox.Text)) && (!string.IsNullOrEmpty(Caption));
		}

		private void OnNameChange(object sender, EventArgs e)
		{
            var trimmedText = TaskNameStringHelper.TrimIllegalChars(_nameTextBox.Text);
            if (trimmedText != _nameTextBox.Text)
            {
                _nameTextBox.Text = trimmedText;
            }

            RequiredFieldsTextChanged(sender, e);

        }

        private void OnRunMountScript(object sender, EventArgs e)
        {
            if (PowershellProcessHelper.Execute(new StubLog(), _userTextBox.Text))
                Messages.ShowInformationBox(BUtil.Configurator.Localization.Resources.FinishedSuccesfully);
            else
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.FinishedWithErrors);
        }

        private void OnMountRun(object sender, EventArgs e)
        {
            if (PowershellProcessHelper.Execute(new StubLog(), _passwordTextBox.Text))
                Messages.ShowInformationBox(BUtil.Configurator.Localization.Resources.FinishedSuccesfully);
            else
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.FinishedWithErrors);
        }

        private void OnSambaButtonClick(object sender, EventArgs e)
        {
			using var form = new SambaForm();
			if (form.ShowDialog() == DialogResult.OK)
			{
                _userTextBox.Text = form.MountScript;
                _passwordTextBox.Text = form.UnmountScript;
            }
        }

        private void OnUploadLimitClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
			Messages.ShowInformationBox(Resources.UploadLimitDescription);
        }
    }
}
