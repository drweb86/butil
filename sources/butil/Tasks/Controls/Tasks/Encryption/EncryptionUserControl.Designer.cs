
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
            _tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            passwordLabel.Location = new System.Drawing.Point(5, 4);
            passwordLabel.Margin = new System.Windows.Forms.Padding(0);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new System.Drawing.Size(115, 27);
            passwordLabel.TabIndex = 3;
            passwordLabel.Text = "Enter password:";
            passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // passwordTextBox
            // 
            passwordTextBox.BackColor = System.Drawing.SystemColors.Window;
            passwordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            passwordTextBox.Location = new System.Drawing.Point(122, 6);
            passwordTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '●';
            passwordTextBox.Size = new System.Drawing.Size(278, 23);
            passwordTextBox.TabIndex = 1;
            passwordTextBox.TextChanged += passwordTextBoxTextChanged;
            // 
            // passwordConfirmationTextBox
            // 
            passwordConfirmationTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            passwordConfirmationTextBox.BackColor = System.Drawing.SystemColors.Window;
            passwordConfirmationTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            passwordConfirmationTextBox.Location = new System.Drawing.Point(120, 46);
            passwordConfirmationTextBox.Margin = new System.Windows.Forms.Padding(0);
            passwordConfirmationTextBox.Name = "passwordConfirmationTextBox";
            passwordConfirmationTextBox.PasswordChar = '●';
            passwordConfirmationTextBox.Size = new System.Drawing.Size(282, 23);
            passwordConfirmationTextBox.TabIndex = 3;
            passwordConfirmationTextBox.TextChanged += passwordConfirmationTextBoxTextChanged;
            // 
            // passwordErrorMessageLabel
            // 
            passwordErrorMessageLabel.AutoSize = true;
            passwordErrorMessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            passwordErrorMessageLabel.ForeColor = System.Drawing.Color.Red;
            passwordErrorMessageLabel.Location = new System.Drawing.Point(124, 31);
            passwordErrorMessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            passwordErrorMessageLabel.Name = "passwordErrorMessageLabel";
            passwordErrorMessageLabel.Size = new System.Drawing.Size(274, 15);
            passwordErrorMessageLabel.TabIndex = 5;
            passwordErrorMessageLabel.Text = "<Message>";
            // 
            // confirmationErrorMessageLabel
            // 
            confirmationErrorMessageLabel.AutoSize = true;
            confirmationErrorMessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            confirmationErrorMessageLabel.ForeColor = System.Drawing.Color.Red;
            confirmationErrorMessageLabel.Location = new System.Drawing.Point(124, 69);
            confirmationErrorMessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            confirmationErrorMessageLabel.Name = "confirmationErrorMessageLabel";
            confirmationErrorMessageLabel.Size = new System.Drawing.Size(274, 15);
            confirmationErrorMessageLabel.TabIndex = 6;
            confirmationErrorMessageLabel.Text = "<Confirmation message>";
            // 
            // confirmPasswordLabel
            // 
            confirmPasswordLabel.AutoSize = true;
            confirmPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            confirmPasswordLabel.Location = new System.Drawing.Point(9, 46);
            confirmPasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            confirmPasswordLabel.Name = "confirmPasswordLabel";
            confirmPasswordLabel.Size = new System.Drawing.Size(107, 23);
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
            generatePasswordButton.Location = new System.Drawing.Point(407, 4);
            generatePasswordButton.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            generatePasswordButton.Name = "generatePasswordButton";
            generatePasswordButton.Size = new System.Drawing.Size(126, 27);
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
            _recommendationsLabel.Location = new System.Drawing.Point(8, 84);
            _recommendationsLabel.MaximumSize = new System.Drawing.Size(532, 0);
            _recommendationsLabel.Name = "_recommendationsLabel";
            _recommendationsLabel.Size = new System.Drawing.Size(522, 15);
            _recommendationsLabel.TabIndex = 1;
            _recommendationsLabel.Text = "Recommendations";
            // 
            // _tableLayoutPanel
            // 
            _tableLayoutPanel.AutoSize = true;
            _tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _tableLayoutPanel.ColumnCount = 3;
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _tableLayoutPanel.Controls.Add(confirmationErrorMessageLabel, 1, 3);
            _tableLayoutPanel.Controls.Add(passwordConfirmationTextBox, 1, 2);
            _tableLayoutPanel.Controls.Add(passwordTextBox, 1, 0);
            _tableLayoutPanel.Controls.Add(passwordErrorMessageLabel, 1, 1);
            _tableLayoutPanel.Controls.Add(confirmPasswordLabel, 0, 2);
            _tableLayoutPanel.Controls.Add(generatePasswordButton, 2, 0);
            _tableLayoutPanel.Controls.Add(passwordLabel, 0, 0);
            _tableLayoutPanel.Controls.Add(_recommendationsLabel, 0, 4);
            _tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _tableLayoutPanel.Name = "_tableLayoutPanel";
            _tableLayoutPanel.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            _tableLayoutPanel.RowCount = 6;
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            _tableLayoutPanel.Size = new System.Drawing.Size(538, 153);
            _tableLayoutPanel.TabIndex = 2;
            // 
            // EncryptionUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_tableLayoutPanel);
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            Name = "EncryptionUserControl";
            Size = new System.Drawing.Size(538, 153);
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
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
    }
}
