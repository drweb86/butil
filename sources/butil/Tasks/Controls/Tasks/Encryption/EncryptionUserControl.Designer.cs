
namespace BUtil.Core.PL
{
    partial class EncryptionUserControl
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the control.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            passwordLabel = new System.Windows.Forms.Label();
            passwordTextBox = new System.Windows.Forms.TextBox();
            passwordConfirmationTextBox = new System.Windows.Forms.TextBox();
            passwordErrorMessageLabel = new System.Windows.Forms.Label();
            confirmationErrorMessageLabel = new System.Windows.Forms.Label();
            confirmPasswordLabel = new System.Windows.Forms.Label();
            generatePasswordButton = new System.Windows.Forms.Button();
            _recommendationsLabel = new System.Windows.Forms.Label();
            passwordControlToolTip = new System.Windows.Forms.ToolTip(components);
            _passwordIfNeededLabel = new System.Windows.Forms.Label();
            _tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            passwordLabel.Location = new System.Drawing.Point(8, 38);
            passwordLabel.Margin = new System.Windows.Forms.Padding(0);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new System.Drawing.Size(214, 43);
            passwordLabel.TabIndex = 3;
            passwordLabel.Text = "Enter password:";
            passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // passwordTextBox
            // 
            passwordTextBox.BackColor = System.Drawing.SystemColors.Window;
            passwordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            passwordTextBox.Location = new System.Drawing.Point(225, 42);
            passwordTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '●';
            passwordTextBox.Size = new System.Drawing.Size(463, 35);
            passwordTextBox.TabIndex = 1;
            passwordTextBox.TextChanged += passwordTextBoxTextChanged;
            // 
            // passwordConfirmationTextBox
            // 
            passwordConfirmationTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            passwordConfirmationTextBox.BackColor = System.Drawing.SystemColors.Window;
            passwordConfirmationTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            passwordConfirmationTextBox.Location = new System.Drawing.Point(222, 111);
            passwordConfirmationTextBox.Margin = new System.Windows.Forms.Padding(0);
            passwordConfirmationTextBox.Name = "passwordConfirmationTextBox";
            passwordConfirmationTextBox.PasswordChar = '●';
            passwordConfirmationTextBox.Size = new System.Drawing.Size(469, 35);
            passwordConfirmationTextBox.TabIndex = 3;
            passwordConfirmationTextBox.TextChanged += passwordConfirmationTextBoxTextChanged;
            // 
            // passwordErrorMessageLabel
            // 
            passwordErrorMessageLabel.AutoSize = true;
            passwordErrorMessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            passwordErrorMessageLabel.ForeColor = System.Drawing.Color.Red;
            passwordErrorMessageLabel.Location = new System.Drawing.Point(229, 81);
            passwordErrorMessageLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            passwordErrorMessageLabel.Name = "passwordErrorMessageLabel";
            passwordErrorMessageLabel.Size = new System.Drawing.Size(455, 30);
            passwordErrorMessageLabel.TabIndex = 5;
            passwordErrorMessageLabel.Text = "<Message>";
            // 
            // confirmationErrorMessageLabel
            // 
            confirmationErrorMessageLabel.AutoSize = true;
            confirmationErrorMessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            confirmationErrorMessageLabel.ForeColor = System.Drawing.Color.Red;
            confirmationErrorMessageLabel.Location = new System.Drawing.Point(229, 146);
            confirmationErrorMessageLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            confirmationErrorMessageLabel.Name = "confirmationErrorMessageLabel";
            confirmationErrorMessageLabel.Size = new System.Drawing.Size(455, 30);
            confirmationErrorMessageLabel.TabIndex = 6;
            confirmationErrorMessageLabel.Text = "<Confirmation message>";
            // 
            // confirmPasswordLabel
            // 
            confirmPasswordLabel.AutoSize = true;
            confirmPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            confirmPasswordLabel.Location = new System.Drawing.Point(15, 111);
            confirmPasswordLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            confirmPasswordLabel.Name = "confirmPasswordLabel";
            confirmPasswordLabel.Size = new System.Drawing.Size(200, 35);
            confirmPasswordLabel.TabIndex = 4;
            confirmPasswordLabel.Text = "Confirm password:";
            confirmPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // generatePasswordButton
            // 
            generatePasswordButton.AutoSize = true;
            generatePasswordButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            generatePasswordButton.BackColor = System.Drawing.SystemColors.Control;
            generatePasswordButton.Dock = System.Windows.Forms.DockStyle.Fill;
            generatePasswordButton.Location = new System.Drawing.Point(699, 38);
            generatePasswordButton.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            generatePasswordButton.Name = "generatePasswordButton";
            generatePasswordButton.Size = new System.Drawing.Size(215, 43);
            generatePasswordButton.TabIndex = 2;
            generatePasswordButton.Text = "Generate password...";
            passwordControlToolTip.SetToolTip(generatePasswordButton, "Generate new random password");
            generatePasswordButton.UseVisualStyleBackColor = true;
            generatePasswordButton.Click += generatePasswordButtonClick;
            // 
            // _recommendationsLabel
            // 
            _recommendationsLabel.AutoSize = true;
            _tableLayoutPanel.SetColumnSpan(_recommendationsLabel, 3);
            _recommendationsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _recommendationsLabel.Location = new System.Drawing.Point(13, 176);
            _recommendationsLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            _recommendationsLabel.MaximumSize = new System.Drawing.Size(912, 0);
            _recommendationsLabel.Name = "_recommendationsLabel";
            _recommendationsLabel.Size = new System.Drawing.Size(896, 30);
            _recommendationsLabel.TabIndex = 1;
            _recommendationsLabel.Text = "Recommendations";
            // 
            // _passwordIfNeededLabel
            // 
            _passwordIfNeededLabel.AutoSize = true;
            _passwordIfNeededLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _passwordIfNeededLabel.Location = new System.Drawing.Point(13, 8);
            _passwordIfNeededLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            _passwordIfNeededLabel.Name = "_passwordIfNeededLabel";
            _passwordIfNeededLabel.Size = new System.Drawing.Size(204, 30);
            _passwordIfNeededLabel.TabIndex = 0;
            _passwordIfNeededLabel.Text = "Password (if needed)";
            _passwordIfNeededLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _tableLayoutPanel
            // 
            _tableLayoutPanel.AutoSize = true;
            _tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _tableLayoutPanel.ColumnCount = 3;
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _tableLayoutPanel.Controls.Add(confirmationErrorMessageLabel, 1, 4);
            _tableLayoutPanel.Controls.Add(passwordConfirmationTextBox, 1, 3);
            _tableLayoutPanel.Controls.Add(passwordTextBox, 1, 1);
            _tableLayoutPanel.Controls.Add(passwordErrorMessageLabel, 1, 2);
            _tableLayoutPanel.Controls.Add(confirmPasswordLabel, 0, 3);
            _tableLayoutPanel.Controls.Add(generatePasswordButton, 2, 1);
            _tableLayoutPanel.Controls.Add(passwordLabel, 0, 1);
            _tableLayoutPanel.Controls.Add(_passwordIfNeededLabel, 0, 0);
            _tableLayoutPanel.Controls.Add(_recommendationsLabel, 0, 5);
            _tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _tableLayoutPanel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            _tableLayoutPanel.Name = "_tableLayoutPanel";
            _tableLayoutPanel.Padding = new System.Windows.Forms.Padding(8);
            _tableLayoutPanel.RowCount = 7;
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.Size = new System.Drawing.Size(922, 306);
            _tableLayoutPanel.TabIndex = 2;
            // 
            // EncryptionUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_tableLayoutPanel);
            Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
            Name = "EncryptionUserControl";
            Size = new System.Drawing.Size(922, 306);
            _tableLayoutPanel.ResumeLayout(false);
            _tableLayoutPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label confirmationErrorMessageLabel;
        private System.Windows.Forms.Label passwordErrorMessageLabel;
        private System.Windows.Forms.ToolTip passwordControlToolTip;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label confirmPasswordLabel;
        private System.Windows.Forms.TextBox passwordConfirmationTextBox;
        private System.Windows.Forms.Button generatePasswordButton;
        private System.Windows.Forms.Label _recommendationsLabel;
        private System.Windows.Forms.Label _passwordIfNeededLabel;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
    }
}
