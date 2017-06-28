using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BUtil.Core;
using BUtil.Core.PL;
using BUtil.Core.Options;
using BULocalization;

namespace BUtil.Core.PL
{
	/// <summary>
	/// Encryption control. 
	/// </summary>
	public sealed partial class EncryptionUserControl : BackUserControl
	{
		bool _passwordIsValid;
		bool _confirmationOfPasswordIsValid;
		bool _dontCareAboutPasswordLength;		
		ProgramOptions _profileOptions;
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
		
		public bool DontCareAboutPasswordLength
		{
			get { return _dontCareAboutPasswordLength; }
			set 
			{
				_dontCareAboutPasswordLength = value; 
				
				string confirmation = passwordConfirmationTextBox.Text;
				applyToUi(Behaviour.PasswordChanged(passwordTextBox.Text, _dontCareAboutPasswordLength));
				passwordConfirmationTextBox.Text = confirmation;
				applyToUi(Behaviour.ConfirmationOfPasswordChanged(passwordTextBox.Text, passwordConfirmationTextBox.Text));
			}
		}
		
		public EncryptionUserControl()
		{
			InitializeComponent();
			
			_defaultTextboxColor = passwordTextBox.BackColor;
			applyToUi(Result.PasswordIsValid);
			applyToUi(Result.ConfirmationOfPasswordIsValid);
		}
		
		public override void ApplyLocalization() 
		{
			passwordGroupBox.Text = Translation.Current[85];
			passwordControlToolTip.SetToolTip(generatePasswordButton, Translation.Current[86]);
            confirmPasswordLabel.Text = Translation.Current[87];
            passwordLabel.Text = Translation.Current[88];
		}
	
		public override void SetOptionsToUi(object settings)
		{
			object[] objects = (object[]) settings;
			_profileOptions = (ProgramOptions)objects[0];
			_task = (BackupTask)objects[1];
			
			passwordTextBox.Text = _task.SecretPassword;
            passwordConfirmationTextBox.Text = _task.SecretPassword;
		}
		
		public override void GetOptionsFromUi()
		{
			if (! (_passwordIsValid && _confirmationOfPasswordIsValid))
            {
            	Messages.ShowErrorBox(Translation.Current[59]);
				passwordTextBox.Text = string.Empty;
			}
            
            _task.SecretPassword = passwordTextBox.Text;
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
					passwordErrorMessageLabel.Text = Translation.Current[553];
					_passwordIsValid = false;
					break;
					
				case Result.PasswordHasInvalidSize:
					passwordTextBox.BackColor = _defaultTextboxColor;
					passwordErrorMessageLabel.Text = string.Format(Translation.Current[554], Constants.MinimumPasswordLength, Constants.MaximumPasswordLength);
					_passwordIsValid = false;
					break;
					
				case Result.ConfirmationOfPasswordIsValid:
					passwordConfirmationTextBox.BackColor = _greenColor;
					confirmationErrorMessageLabel.Text = string.Empty;
					_confirmationOfPasswordIsValid = true;
					break;
					
				case Result.ConfirmationIsNotEqualToPassword:
					passwordConfirmationTextBox.BackColor = _defaultTextboxColor;
					confirmationErrorMessageLabel.Text = Translation.Current[555];
					_confirmationOfPasswordIsValid = false;
					break;
					
				default:
					throw new NotImplementedException(result.ToString());
			}
		}
		
		void passwordTextBoxTextChanged(object sender, EventArgs e)
		{
			applyToUi(Behaviour.PasswordChanged(passwordTextBox.Text, _dontCareAboutPasswordLength));
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
