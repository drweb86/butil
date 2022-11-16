namespace BUtil.RestorationMaster
{
	partial class OpenBackupForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenBackupForm));
            this._openBackupFolderButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this._passwordTextBox = new System.Windows.Forms.MaskedTextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.continueButton = new System.Windows.Forms.Button();
            this._backupFolderLabel = new System.Windows.Forms.Label();
            this._backupLocationTextBox = new System.Windows.Forms.TextBox();
            this.helpButton = new System.Windows.Forms.Button();
            this._fbd = new System.Windows.Forms.FolderBrowserDialog();
            this._helpLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _openBackupFolderButton
            // 
            this._openBackupFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._openBackupFolderButton.AutoSize = true;
            this._openBackupFolderButton.BackColor = System.Drawing.SystemColors.Control;
            this._openBackupFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._openBackupFolderButton.Location = new System.Drawing.Point(509, 107);
            this._openBackupFolderButton.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._openBackupFolderButton.Name = "_openBackupFolderButton";
            this._openBackupFolderButton.Size = new System.Drawing.Size(161, 31);
            this._openBackupFolderButton.TabIndex = 2;
            this._openBackupFolderButton.Text = "Open folder...";
            this._openBackupFolderButton.UseVisualStyleBackColor = true;
            this._openBackupFolderButton.Click += new System.EventHandler(this.OnSelectBackupLocationClick);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.AutoSize = true;
            this.closeButton.Location = new System.Drawing.Point(571, 249);
            this.closeButton.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(98, 36);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.OnCloseButtonClick);
            // 
            // _passwordTextBox
            // 
            this._passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._passwordTextBox.Location = new System.Drawing.Point(13, 188);
            this._passwordTextBox.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._passwordTextBox.Name = "_passwordTextBox";
            this._passwordTextBox.PasswordChar = '*';
            this._passwordTextBox.Size = new System.Drawing.Size(657, 27);
            this._passwordTextBox.TabIndex = 1;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(13, 156);
            this.passwordLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(439, 20);
            this.passwordLabel.TabIndex = 0;
            this.passwordLabel.Text = "If your backup is password protected, please type password here:";
            // 
            // continueButton
            // 
            this.continueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.continueButton.Location = new System.Drawing.Point(392, 249);
            this.continueButton.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(170, 36);
            this.continueButton.TabIndex = 8;
            this.continueButton.Text = "Continue >";
            this.continueButton.UseVisualStyleBackColor = true;
            this.continueButton.Click += new System.EventHandler(this.OnNextButtonClick);
            // 
            // _backupFolderLabel
            // 
            this._backupFolderLabel.AutoSize = true;
            this._backupFolderLabel.Location = new System.Drawing.Point(13, 83);
            this._backupFolderLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this._backupFolderLabel.Name = "_backupFolderLabel";
            this._backupFolderLabel.Size = new System.Drawing.Size(104, 20);
            this._backupFolderLabel.TabIndex = 9;
            this._backupFolderLabel.Text = "Backup folder:";
            // 
            // _backupLocationTextBox
            // 
            this._backupLocationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._backupLocationTextBox.Location = new System.Drawing.Point(13, 107);
            this._backupLocationTextBox.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._backupLocationTextBox.Name = "_backupLocationTextBox";
            this._backupLocationTextBox.Size = new System.Drawing.Size(486, 27);
            this._backupLocationTextBox.TabIndex = 10;
            this._backupLocationTextBox.TabStop = false;
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpButton.Location = new System.Drawing.Point(13, 241);
            this.helpButton.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(43, 51);
            this.helpButton.TabIndex = 11;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.OnHelpClick);
            // 
            // _helpLabel
            // 
            this._helpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._helpLabel.Location = new System.Drawing.Point(13, 12);
            this._helpLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this._helpLabel.Name = "_helpLabel";
            this._helpLabel.Size = new System.Drawing.Size(657, 71);
            this._helpLabel.TabIndex = 12;
            this._helpLabel.Text = "Mount your backup location as disk or copy it to any folder and specify its locat" +
    "ion.";
            // 
            // OpenBackupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 305);
            this.Controls.Add(this._helpLabel);
            this.Controls.Add(this._passwordTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this._backupLocationTextBox);
            this.Controls.Add(this._backupFolderLabel);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this._openBackupFolderButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenBackupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Restoration master";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Button helpButton;
		private System.Windows.Forms.Button _openBackupFolderButton;
        private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.MaskedTextBox _passwordTextBox;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.Button continueButton;
		private System.Windows.Forms.Label _backupFolderLabel;
		private System.Windows.Forms.TextBox _backupLocationTextBox;
        private System.Windows.Forms.FolderBrowserDialog _fbd;
        private System.Windows.Forms.Label _helpLabel;
    }
}
