
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
            this.passwordControlTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordConfirmationTextBox = new System.Windows.Forms.TextBox();
            this.passwordErrorMessageLabel = new System.Windows.Forms.Label();
            this.confirmationErrorMessageLabel = new System.Windows.Forms.Label();
            this.confirmPasswordLabel = new System.Windows.Forms.Label();
            this.generatePasswordButton = new System.Windows.Forms.Button();
            this._recommendationsLabel = new System.Windows.Forms.Label();
            this.passwordControlToolTip = new System.Windows.Forms.ToolTip(this.components);
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._passwordIfNeededLabel = new System.Windows.Forms.Label();
            this.passwordControlTableLayoutPanel.SuspendLayout();
            this._tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // passwordControlTableLayoutPanel
            // 
            this.passwordControlTableLayoutPanel.AutoSize = true;
            this.passwordControlTableLayoutPanel.ColumnCount = 2;
            this.passwordControlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.passwordControlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.passwordControlTableLayoutPanel.Controls.Add(this.passwordLabel, 0, 0);
            this.passwordControlTableLayoutPanel.Controls.Add(this.passwordTextBox, 1, 0);
            this.passwordControlTableLayoutPanel.Controls.Add(this.passwordConfirmationTextBox, 1, 2);
            this.passwordControlTableLayoutPanel.Controls.Add(this.passwordErrorMessageLabel, 1, 1);
            this.passwordControlTableLayoutPanel.Controls.Add(this.confirmationErrorMessageLabel, 1, 3);
            this.passwordControlTableLayoutPanel.Controls.Add(this.confirmPasswordLabel, 0, 2);
            this.passwordControlTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passwordControlTableLayoutPanel.Location = new System.Drawing.Point(4, 18);
            this.passwordControlTableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.passwordControlTableLayoutPanel.Name = "passwordControlTableLayoutPanel";
            this.passwordControlTableLayoutPanel.RowCount = 5;
            this.passwordControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.passwordControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.passwordControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.passwordControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.passwordControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.passwordControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.passwordControlTableLayoutPanel.Size = new System.Drawing.Size(511, 76);
            this.passwordControlTableLayoutPanel.TabIndex = 0;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passwordLabel.Location = new System.Drawing.Point(0, 0);
            this.passwordLabel.Margin = new System.Windows.Forms.Padding(0);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(115, 23);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Enter password:";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.passwordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passwordTextBox.Location = new System.Drawing.Point(115, 0);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '●';
            this.passwordTextBox.Size = new System.Drawing.Size(396, 23);
            this.passwordTextBox.TabIndex = 1;
            this.passwordTextBox.TextChanged += new System.EventHandler(this.passwordTextBoxTextChanged);
            // 
            // passwordConfirmationTextBox
            // 
            this.passwordConfirmationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordConfirmationTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.passwordConfirmationTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.passwordConfirmationTextBox.Location = new System.Drawing.Point(115, 38);
            this.passwordConfirmationTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.passwordConfirmationTextBox.Name = "passwordConfirmationTextBox";
            this.passwordConfirmationTextBox.PasswordChar = '●';
            this.passwordConfirmationTextBox.Size = new System.Drawing.Size(396, 23);
            this.passwordConfirmationTextBox.TabIndex = 3;
            this.passwordConfirmationTextBox.TextChanged += new System.EventHandler(this.passwordConfirmationTextBoxTextChanged);
            // 
            // passwordErrorMessageLabel
            // 
            this.passwordErrorMessageLabel.AutoSize = true;
            this.passwordErrorMessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passwordErrorMessageLabel.ForeColor = System.Drawing.Color.Red;
            this.passwordErrorMessageLabel.Location = new System.Drawing.Point(119, 23);
            this.passwordErrorMessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.passwordErrorMessageLabel.Name = "passwordErrorMessageLabel";
            this.passwordErrorMessageLabel.Size = new System.Drawing.Size(388, 15);
            this.passwordErrorMessageLabel.TabIndex = 5;
            this.passwordErrorMessageLabel.Text = "<Message>";
            // 
            // confirmationErrorMessageLabel
            // 
            this.confirmationErrorMessageLabel.AutoSize = true;
            this.confirmationErrorMessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.confirmationErrorMessageLabel.ForeColor = System.Drawing.Color.Red;
            this.confirmationErrorMessageLabel.Location = new System.Drawing.Point(119, 61);
            this.confirmationErrorMessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.confirmationErrorMessageLabel.Name = "confirmationErrorMessageLabel";
            this.confirmationErrorMessageLabel.Size = new System.Drawing.Size(388, 15);
            this.confirmationErrorMessageLabel.TabIndex = 6;
            this.confirmationErrorMessageLabel.Text = "<Confirmation message>";
            // 
            // confirmPasswordLabel
            // 
            this.confirmPasswordLabel.AutoSize = true;
            this.confirmPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.confirmPasswordLabel.Location = new System.Drawing.Point(4, 38);
            this.confirmPasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.confirmPasswordLabel.Name = "confirmPasswordLabel";
            this.confirmPasswordLabel.Size = new System.Drawing.Size(107, 23);
            this.confirmPasswordLabel.TabIndex = 4;
            this.confirmPasswordLabel.Text = "Confirm password:";
            this.confirmPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // generatePasswordButton
            // 
            this.generatePasswordButton.AutoSize = true;
            this.generatePasswordButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.generatePasswordButton.BackColor = System.Drawing.SystemColors.Control;
            this.generatePasswordButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.generatePasswordButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.generatePasswordButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.generatePasswordButton.Location = new System.Drawing.Point(389, 115);
            this.generatePasswordButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.generatePasswordButton.MaximumSize = new System.Drawing.Size(126, 25);
            this.generatePasswordButton.Name = "generatePasswordButton";
            this.generatePasswordButton.Size = new System.Drawing.Size(126, 25);
            this.generatePasswordButton.TabIndex = 2;
            this.generatePasswordButton.Text = "Generate password...";
            this.passwordControlToolTip.SetToolTip(this.generatePasswordButton, "Generate new random password");
            this.generatePasswordButton.UseVisualStyleBackColor = false;
            this.generatePasswordButton.Click += new System.EventHandler(this.generatePasswordButtonClick);
            // 
            // _recommendationsLabel
            // 
            this._recommendationsLabel.AutoSize = true;
            this._recommendationsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._recommendationsLabel.Location = new System.Drawing.Point(3, 97);
            this._recommendationsLabel.Name = "_recommendationsLabel";
            this._recommendationsLabel.Size = new System.Drawing.Size(513, 15);
            this._recommendationsLabel.TabIndex = 1;
            this._recommendationsLabel.Text = "Recommendations";
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.ColumnCount = 1;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.Controls.Add(this.passwordControlTableLayoutPanel, 0, 1);
            this._tableLayoutPanel.Controls.Add(this._passwordIfNeededLabel, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._recommendationsLabel, 0, 2);
            this._tableLayoutPanel.Controls.Add(this.generatePasswordButton, 0, 3);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 5;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.Size = new System.Drawing.Size(519, 184);
            this._tableLayoutPanel.TabIndex = 1;
            // 
            // _passwordIfNeededLabel
            // 
            this._passwordIfNeededLabel.AutoSize = true;
            this._passwordIfNeededLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._passwordIfNeededLabel.Location = new System.Drawing.Point(3, 0);
            this._passwordIfNeededLabel.Name = "_passwordIfNeededLabel";
            this._passwordIfNeededLabel.Size = new System.Drawing.Size(513, 15);
            this._passwordIfNeededLabel.TabIndex = 0;
            this._passwordIfNeededLabel.Text = "Password (if needed)";
            // 
            // EncryptionUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "EncryptionUserControl";
            this.Size = new System.Drawing.Size(519, 184);
            this.passwordControlTableLayoutPanel.ResumeLayout(false);
            this.passwordControlTableLayoutPanel.PerformLayout();
            this._tableLayoutPanel.ResumeLayout(false);
            this._tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Label confirmationErrorMessageLabel;
		private System.Windows.Forms.Label passwordErrorMessageLabel;
		private System.Windows.Forms.ToolTip passwordControlToolTip;
		private System.Windows.Forms.TableLayoutPanel passwordControlTableLayoutPanel;
		private System.Windows.Forms.TextBox passwordTextBox;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.Label confirmPasswordLabel;
		private System.Windows.Forms.TextBox passwordConfirmationTextBox;
		private System.Windows.Forms.Button generatePasswordButton;
        private System.Windows.Forms.Label _recommendationsLabel;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.Label _passwordIfNeededLabel;
    }
}
