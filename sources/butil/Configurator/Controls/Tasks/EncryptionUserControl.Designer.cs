
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
			if (disposing) {
				if (components != null) {
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
            this.components = new System.ComponentModel.Container();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordConfirmationTextBox = new System.Windows.Forms.TextBox();
            this.passwordErrorMessageLabel = new System.Windows.Forms.Label();
            this.confirmationErrorMessageLabel = new System.Windows.Forms.Label();
            this.confirmPasswordLabel = new System.Windows.Forms.Label();
            this.generatePasswordButton = new System.Windows.Forms.Button();
            this._recommendationsLabel = new System.Windows.Forms.Label();
            this.passwordControlToolTip = new System.Windows.Forms.ToolTip(this.components);
            this._passwordIfNeededLabel = new System.Windows.Forms.Label();
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passwordLabel.Location = new System.Drawing.Point(0, 15);
            this.passwordLabel.Margin = new System.Windows.Forms.Padding(0);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(123, 23);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Enter password:";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.passwordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passwordTextBox.Location = new System.Drawing.Point(123, 15);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '●';
            this.passwordTextBox.Size = new System.Drawing.Size(281, 23);
            this.passwordTextBox.TabIndex = 1;
            this.passwordTextBox.TextChanged += new System.EventHandler(this.passwordTextBoxTextChanged);
            // 
            // passwordConfirmationTextBox
            // 
            this.passwordConfirmationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordConfirmationTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.passwordConfirmationTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.passwordConfirmationTextBox.Location = new System.Drawing.Point(123, 53);
            this.passwordConfirmationTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.passwordConfirmationTextBox.Name = "passwordConfirmationTextBox";
            this.passwordConfirmationTextBox.PasswordChar = '●';
            this.passwordConfirmationTextBox.Size = new System.Drawing.Size(281, 23);
            this.passwordConfirmationTextBox.TabIndex = 3;
            this.passwordConfirmationTextBox.TextChanged += new System.EventHandler(this.passwordConfirmationTextBoxTextChanged);
            // 
            // passwordErrorMessageLabel
            // 
            this.passwordErrorMessageLabel.AutoSize = true;
            this.passwordErrorMessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passwordErrorMessageLabel.ForeColor = System.Drawing.Color.Red;
            this.passwordErrorMessageLabel.Location = new System.Drawing.Point(127, 38);
            this.passwordErrorMessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.passwordErrorMessageLabel.Name = "passwordErrorMessageLabel";
            this.passwordErrorMessageLabel.Size = new System.Drawing.Size(273, 15);
            this.passwordErrorMessageLabel.TabIndex = 5;
            this.passwordErrorMessageLabel.Text = "<Message>";
            // 
            // confirmationErrorMessageLabel
            // 
            this.confirmationErrorMessageLabel.AutoSize = true;
            this.confirmationErrorMessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.confirmationErrorMessageLabel.ForeColor = System.Drawing.Color.Red;
            this.confirmationErrorMessageLabel.Location = new System.Drawing.Point(127, 76);
            this.confirmationErrorMessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.confirmationErrorMessageLabel.Name = "confirmationErrorMessageLabel";
            this.confirmationErrorMessageLabel.Size = new System.Drawing.Size(273, 15);
            this.confirmationErrorMessageLabel.TabIndex = 6;
            this.confirmationErrorMessageLabel.Text = "<Confirmation message>";
            // 
            // confirmPasswordLabel
            // 
            this.confirmPasswordLabel.AutoSize = true;
            this.confirmPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.confirmPasswordLabel.Location = new System.Drawing.Point(4, 53);
            this.confirmPasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.confirmPasswordLabel.Name = "confirmPasswordLabel";
            this.confirmPasswordLabel.Size = new System.Drawing.Size(115, 23);
            this.confirmPasswordLabel.TabIndex = 4;
            this.confirmPasswordLabel.Text = "Confirm password:";
            this.confirmPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // generatePasswordButton
            // 
            this.generatePasswordButton.AutoSize = true;
            this.generatePasswordButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.generatePasswordButton.BackColor = System.Drawing.SystemColors.Control;
            this.generatePasswordButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generatePasswordButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.generatePasswordButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.generatePasswordButton.Location = new System.Drawing.Point(408, 18);
            this.generatePasswordButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.generatePasswordButton.MaximumSize = new System.Drawing.Size(126, 24);
            this.generatePasswordButton.Name = "generatePasswordButton";
            this._tableLayoutPanel.SetRowSpan(this.generatePasswordButton, 2);
            this.generatePasswordButton.Size = new System.Drawing.Size(126, 24);
            this.generatePasswordButton.TabIndex = 2;
            this.generatePasswordButton.Text = "Generate password...";
            this.passwordControlToolTip.SetToolTip(this.generatePasswordButton, "Generate new random password");
            this.generatePasswordButton.UseVisualStyleBackColor = false;
            this.generatePasswordButton.Click += new System.EventHandler(this.generatePasswordButtonClick);
            // 
            // _recommendationsLabel
            // 
            this._recommendationsLabel.AutoSize = true;
            this._tableLayoutPanel.SetColumnSpan(this._recommendationsLabel, 3);
            this._recommendationsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._recommendationsLabel.Location = new System.Drawing.Point(3, 91);
            this._recommendationsLabel.Name = "_recommendationsLabel";
            this._recommendationsLabel.Size = new System.Drawing.Size(532, 15);
            this._recommendationsLabel.TabIndex = 1;
            this._recommendationsLabel.Text = "Recommendations";
            // 
            // _passwordIfNeededLabel
            // 
            this._passwordIfNeededLabel.AutoSize = true;
            this._passwordIfNeededLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._passwordIfNeededLabel.Location = new System.Drawing.Point(3, 0);
            this._passwordIfNeededLabel.Name = "_passwordIfNeededLabel";
            this._passwordIfNeededLabel.Size = new System.Drawing.Size(117, 15);
            this._passwordIfNeededLabel.TabIndex = 0;
            this._passwordIfNeededLabel.Text = "Password (if needed)";
            this._passwordIfNeededLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.AutoSize = true;
            this._tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableLayoutPanel.ColumnCount = 3;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.Controls.Add(this.confirmationErrorMessageLabel, 1, 4);
            this._tableLayoutPanel.Controls.Add(this.passwordConfirmationTextBox, 1, 3);
            this._tableLayoutPanel.Controls.Add(this.passwordTextBox, 1, 1);
            this._tableLayoutPanel.Controls.Add(this.passwordErrorMessageLabel, 1, 2);
            this._tableLayoutPanel.Controls.Add(this.confirmPasswordLabel, 0, 3);
            this._tableLayoutPanel.Controls.Add(this.generatePasswordButton, 2, 1);
            this._tableLayoutPanel.Controls.Add(this.passwordLabel, 0, 1);
            this._tableLayoutPanel.Controls.Add(this._passwordIfNeededLabel, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._recommendationsLabel, 0, 5);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 7;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.Size = new System.Drawing.Size(538, 153);
            this._tableLayoutPanel.TabIndex = 2;
            // 
            // EncryptionUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "EncryptionUserControl";
            this.Size = new System.Drawing.Size(538, 153);
            this._tableLayoutPanel.ResumeLayout(false);
            this._tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
