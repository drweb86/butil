using System;
using System.Drawing;
using System.Windows.Forms;
using BUtil.Core.Options;
using BUtil.Core.Localization;
using BUtil.Configurator;
using BUtil.Core.BackupModels;

namespace BUtil.Core.PL
{
    public sealed partial class EncryptionUserControl : BackUserControl
	{
		bool _passwordIsValid;
		bool _confirmationOfPasswordIsValid;
		private BackupTask _task;
		readonly Color _greenColor = Color.LightGreen;
		readonly Color _defaultTextboxColor;
		
		#region Types
		
		public enum Result
		{
			PasswordIsValid,
			ConfirmationIsNotEqualToPassword,
			ConfirmationOfPasswordIsValid
		}
			
		public static class Behaviour
		{
			public static Result PasswordChanged(string password)
			{
				if (string.IsNullOrEmpty(password))
				{
					return Result.PasswordIsValid;
				}
				
				return Result.PasswordIsValid;
			}
			
			public static Result ConfirmationOfPasswordChanged(string password, string confirmation)
			{
				if (string.IsNullOrEmpty(password) && (string.IsNullOrEmpty(confirmation)))
				{
					return Result.ConfirmationOfPasswordIsValid;
				}
				else if (password != confirmation) 
				{
					return Result.ConfirmationIsNotEqualToPassword;
				}
				else
				{
					return Result.ConfirmationOfPasswordIsValid;
				}
			}
		}
		
		#endregion
		
		public EncryptionUserControl(BackupTask task = null)
		{
			_task = task;

			InitializeComponent();
			
			_defaultTextboxColor = passwordTextBox.BackColor;

            applyToUi(Result.PasswordIsValid);
			applyToUi(Result.ConfirmationOfPasswordIsValid);

            _passwordIfNeededLabel.Text = Resources.PasswordIfNeeded;
            passwordControlToolTip.SetToolTip(generatePasswordButton, Resources.GenerateNewRandomPassword);
            confirmPasswordLabel.Text = Resources.ConfirmPassword;
            passwordLabel.Text = Resources.EnterPassword;
            generatePasswordButton.Text = BUtil.Configurator.Localization.Resources.GeneratePassword;
			_recommendationsLabel.Text = BUtil.Configurator.Localization.Resources.RecommendedMinimumPasswordLengthIs50CharactersAndMore;


            if (task != null)
				UpdateModel(task);
        }

		public void UpdateModel(BackupTask task)
		{
			if (task != null)
				_task = task;


			if (_task.Model is IncrementalBackupModelOptions)
			{
				var options = _task.Model as IncrementalBackupModelOptions;

                _tableLayoutPanel.Enabled = !options.DisableCompressionAndEncryption;

                if (options.DisableCompressionAndEncryption)
				{
					passwordTextBox.Text = string.Empty;
					passwordConfirmationTextBox.Text = string.Empty;
				}
				else
				{
                    passwordTextBox.Text = _task.Password;
                    passwordConfirmationTextBox.Text = _task.Password;
                }
            }
		}

        public override bool ValidateUi()
        {
            if (passwordTextBox.Text != passwordConfirmationTextBox.Text)
			{
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.PasswordsDoNotMatch);
                return false;
			}

            return true;
        }

        public override void GetOptionsFromUi()
		{
			if (! (_passwordIsValid && _confirmationOfPasswordIsValid))
            {
            	Messages.ShowErrorBox(Resources.PasswordIsInvalidNNitWasResetedN);
				passwordTextBox.Text = string.Empty;
			}
            
            _task.Password = passwordTextBox.Text;
		}
		
		void applyToUi(Result result)
		{
			switch (result)
			{
				case Result.PasswordIsValid:
					passwordTextBox.BackColor = _greenColor;
					passwordErrorMessageLabel.Text = string.Empty;
					_passwordIsValid = true;
					break;
				
				case Result.ConfirmationOfPasswordIsValid:
					passwordConfirmationTextBox.BackColor = _greenColor;
					confirmationErrorMessageLabel.Text = string.Empty;
					_confirmationOfPasswordIsValid = true;
					break;
					
				case Result.ConfirmationIsNotEqualToPassword:
					passwordConfirmationTextBox.BackColor = _defaultTextboxColor;
					confirmationErrorMessageLabel.Text = Resources.ConfirmationIsNotEqualToPassword;
					_confirmationOfPasswordIsValid = false;
					break;
					
				default:
					throw new NotImplementedException(result.ToString());
			}
		}
		
		void passwordTextBoxTextChanged(object sender, EventArgs e)
		{
			applyToUi(Behaviour.PasswordChanged(passwordTextBox.Text));
			passwordConfirmationTextBox.Text = string.Empty;
			applyToUi(Behaviour.ConfirmationOfPasswordChanged(passwordTextBox.Text, passwordConfirmationTextBox.Text));
		}
		
		void passwordConfirmationTextBoxTextChanged(object sender, EventArgs e)
		{
			applyToUi(Behaviour.ConfirmationOfPasswordChanged(passwordTextBox.Text, passwordConfirmationTextBox.Text));
		}

		void generatePasswordButtonClick(object sender, EventArgs e)
		{
			generatePasswordButton.Focus();

			using var form = new PasswordGeneratorForm();
			if (form.ShowDialog() == DialogResult.OK)
			{
				passwordTextBox.Text = form.Password;
				passwordConfirmationTextBox.Text = form.Password;
			}
		}
	}
}
