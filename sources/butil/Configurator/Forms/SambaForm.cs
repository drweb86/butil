using System;
using System.Windows.Forms;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator
{
    internal sealed partial class SambaForm : Form
	{
		public string MountScript { get; private set; }
		public string UnmountScript { get; private set; }
		//валидация
		//	выбор диска
		//		выбор буквы
		//	поддержка без кредов.
		//	отдельная иконка

		//		формат хранилища. - файлы последовательные вместо гуидов.
		//		кнопка пуск РК - сделай SVG/заюзай икнку.
		//		выделить Result у тасок.
		public SambaForm()
		{
			InitializeComponent();

			acceptButton.Text = Resources.Ok;
			cancelButton.Text = Resources.Cancel;

			_hostTextBox.Text = "192.168.1.1";
			_shareNameTextBox.Text = "joshShare";
			_userTextBox.Text = "josh";
			_passwordTextBox.Text = "joshPassword";
			
			_hostLabel.Text = BUtil.Configurator.Localization.Resources.Host;
			_shareNameLabel.Text = BUtil.Configurator.Localization.Resources.ShareName;
			_userLabel.Text = BUtil.Configurator.Localization.Resources.User;
            _passwordLabel.Text = BUtil.Configurator.Localization.Resources.Password;
		}

        private void OnAccept(object sender, EventArgs e)
        {
			MountScript = @$"net use H: \\{_hostTextBox.Text}\{_shareNameTextBox.Text} /user:{_userTextBox.Text} {_passwordTextBox.Text}";
			UnmountScript = @"net use /delete H: /y";

            DialogResult = DialogResult.OK;
			Close();
        }
    }
}
