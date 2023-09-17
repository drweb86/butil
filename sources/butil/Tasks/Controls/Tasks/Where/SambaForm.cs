using System;
using System.Windows.Forms;
using BUtil.Core.Localization;

namespace BUtil.Configurator
{
    internal sealed partial class SambaForm : Form
	{
		public string MountScript { get; private set; }
		public string UnmountScript { get; private set; }

		public SambaForm()
		{
			InitializeComponent();

			acceptButton.Text = Resources.Button_OK;
			cancelButton.Text = Resources.Button_Cancel;

			_uriLabel.Text = BUtil.Core.Localization.Resources.Url;
			_userLabel.Text = BUtil.Core.Localization.Resources.User_Field;
            _passwordLabel.Text = BUtil.Core.Localization.Resources.Password_Field;
		}

        private void OnAccept(object sender, EventArgs e)
        {
			MountScript = string.IsNullOrWhiteSpace(_userTextBox.Text)
				? @$"net use H: ""{_urlTextBox.Text}"""
				: @$"net use H: ""{_urlTextBox.Text}"" ""/user:{_userTextBox.Text}"" ""{_passwordTextBox.Text}""";
			UnmountScript = @"net use /delete H: /y";

            DialogResult = DialogResult.OK;
			Close();
        }
    }
}
