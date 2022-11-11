using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BUtil.Core;
using BUtil.Core.Options;
using BUtil.Core.Localization;
using BUtil.Configurator;

namespace BUtil.Core.PL
{
    /// <summary>
    /// Encryption control. 
    /// </summary>
    public sealed partial class EncryptionUserControl : BackUserControl
	{
		bool _passwordIsValid;
		bool _confirmationOfPasswordIsValid;
		BackupTask _task;
		readonly Color _greenColor = Color.LightGreen;
		readonly Color _defaultTextboxColor;
		
		#region Types
		
		public enum Result
		{
			PasswordHasInvalidSize,
			PasswordContainsForbiddenCharacters,
			PasswordIsValid,
			ConfirmationIsNotEqualToPassword,
			ConfirmationOfPasswordIsValid
		}
			
		public static class Behaviour
		{
			public static Result PasswordChanged(string password, bool dontCareAboutPasswordLength)
			{
				if (string.IsNullOrEmpty(password))
				{
					return Result.PasswordIsValid;
				}
				else if (password.Contains(" "))
				{
					return Result.PasswordContainsForbiddenCharacters;
				}
				else if ( !dontCareAboutPasswordLength && ((password.Length < Constants.MinimumPasswordLength) || 
				                                          (password.Length > Constants.MaximumPasswordLength) ))
				{
					return Result.PasswordHasInvalidSize;
				}
				else
				{
					return Result.PasswordIsValid;
				}
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
		
		public EncryptionUserControl()
		{
			InitializeComponent();
			
			_defaultTextboxColor = passwordTextBox.BackColor;
			applyToUi(Result.PasswordIsValid);
			applyToUi(Result.ConfirmationOfPasswordIsValid);
		}
		
		public override void ApplyLocalization() 
		{
			passwordGroupBox.Text = Resources.PasswordIfNeeded;
			passwordControlToolTip.SetToolTip(generatePasswordButton, Resources.GenerateNewRandomPassword);
            confirmPasswordLabel.Text = Resources.ConfirmPassword;
            passwordLabel.Text = Resources.EnterPassword;
			generatePasswordButton.Text = BUtil.Configurator.Localization.Resources.GeneratePassword;
        }
	
		public override void SetOptionsToUi(object settings)
		{
			object[] objects = (object[]) settings;
			_task = (BackupTask)objects[1];
			
			passwordTextBox.Text = _task.Password;
            passwordConfirmationTextBox.Text = _task.Password;
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
					
				case Result.PasswordContainsForbiddenCharacters:
					passwordTextBox.BackColor = _defaultTextboxColor;
					passwordErrorMessageLabel.Text = Resources.PasswordContainsForbiddenCharactersPleaseRemoveSpaces;
					_passwordIsValid = false;
					break;
					
				case Result.PasswordHasInvalidSize:
					passwordTextBox.BackColor = _defaultTextboxColor;
					passwordErrorMessageLabel.Text = string.Format(Resources.PasswordHasInvalidLengthPasswordLengthShouldBeFrom0To1Characters, Constants.MinimumPasswordLength, Constants.MaximumPasswordLength);
					_passwordIsValid = false;
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
			applyToUi(Behaviour.PasswordChanged(passwordTextBox.Text, false));
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

            using (PasswordGeneratorForm form = new PasswordGeneratorForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    passwordTextBox.Text = form.Password;
                    passwordConfirmationTextBox.Text = form.Password;
                }
            }
		}
	}
}
