using System;
using System.Drawing;
using System.Windows.Forms;
using BUtil.Core.Localization;
using BUtil.Configurator;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Configurator.Tasks.Controls.Tasks.Encryption;

namespace BUtil.Core.PL
{
    public sealed partial class EncryptionUserControl : BackUserControl
    {
        bool _passwordIsValid;
        bool _confirmationOfPasswordIsValid;
        private TaskV2 _task;
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

        public EncryptionUserControl()
            : this(null)
        {

        }

        public EncryptionUserControl(TaskV2 task)
        {
            _task = task;

            InitializeComponent();

            _defaultTextboxColor = passwordTextBox.BackColor;

            applyToUi(Result.PasswordIsValid);
            applyToUi(Result.ConfirmationOfPasswordIsValid);

            confirmPasswordLabel.Text = Resources.Password_Field_Confirm;
            passwordLabel.Text = Resources.Password_Field;
            generatePasswordButton.Text = Resources.Password_Generate;
            _recommendationsLabel.Text = Resources.Password_Help;


            if (task != null)
                UpdateModel(task);
        }

        private void UpdateModel(TaskV2 task)
        {
            _task = task;

            if (_task.Model is IncrementalBackupModelOptionsV2)
            {
                var typedOptions = (IncrementalBackupModelOptionsV2)_task.Model;
                passwordTextBox.Text = typedOptions.Password;
                passwordConfirmationTextBox.Text = typedOptions.Password;
            }
        }

        public override bool ValidateUi()
        {
            if (string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                Messages.ShowErrorBox(BUtil.Core.Localization.Resources.Password_Field_Validation_NotSpecified);
                return false;
            } else if (passwordTextBox.Text != passwordConfirmationTextBox.Text)
            {
                Messages.ShowErrorBox(BUtil.Core.Localization.Resources.Password_Field_Validation_NotMatch);
                return false;
            }

            return true;
        }

        public override void GetOptionsFromUi()
        {
            if (!(_passwordIsValid && _confirmationOfPasswordIsValid))
            {
                Messages.ShowErrorBox(Resources.Password_Field_Validation_NotMatch);
                passwordTextBox.Text = string.Empty;
            }

            if (_task.Model is IncrementalBackupModelOptionsV2)
            {
                var typedOptions = (IncrementalBackupModelOptionsV2)_task.Model;
                typedOptions.Password = passwordTextBox.Text;
            }
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
                    confirmationErrorMessageLabel.Text = Resources.Password_Field_Validation_NotMatch;
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
