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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenBackupForm));
            _openBackupFolderButton = new System.Windows.Forms.Button();
            closeButton = new System.Windows.Forms.Button();
            _passwordTextBox = new System.Windows.Forms.MaskedTextBox();
            passwordLabel = new System.Windows.Forms.Label();
            continueButton = new System.Windows.Forms.Button();
            _backupFolderLabel = new System.Windows.Forms.Label();
            _backupLocationTextBox = new System.Windows.Forms.TextBox();
            helpButton = new System.Windows.Forms.Button();
            _fbd = new System.Windows.Forms.FolderBrowserDialog();
            _helpLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // _openBackupFolderButton
            // 
            _openBackupFolderButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _openBackupFolderButton.AutoSize = true;
            _openBackupFolderButton.BackColor = System.Drawing.SystemColors.Control;
            _openBackupFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _openBackupFolderButton.Location = new System.Drawing.Point(445, 80);
            _openBackupFolderButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _openBackupFolderButton.Name = "_openBackupFolderButton";
            _openBackupFolderButton.Size = new System.Drawing.Size(141, 23);
            _openBackupFolderButton.TabIndex = 1;
            _openBackupFolderButton.Text = "Open folder...";
            _openBackupFolderButton.UseVisualStyleBackColor = true;
            _openBackupFolderButton.Click += OnSelectBackupLocationClick;
            // 
            // closeButton
            // 
            closeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            closeButton.AutoSize = true;
            closeButton.Location = new System.Drawing.Point(500, 187);
            closeButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            closeButton.Name = "closeButton";
            closeButton.Size = new System.Drawing.Size(86, 27);
            closeButton.TabIndex = 6;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += OnCloseButtonClick;
            // 
            // _passwordTextBox
            // 
            _passwordTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _passwordTextBox.Location = new System.Drawing.Point(11, 141);
            _passwordTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _passwordTextBox.Name = "_passwordTextBox";
            _passwordTextBox.PasswordChar = '*';
            _passwordTextBox.Size = new System.Drawing.Size(575, 23);
            _passwordTextBox.TabIndex = 3;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new System.Drawing.Point(11, 117);
            passwordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new System.Drawing.Size(348, 15);
            passwordLabel.TabIndex = 0;
            passwordLabel.Text = "If your backup is password protected, please type password here:";
            // 
            // continueButton
            // 
            continueButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            continueButton.Location = new System.Drawing.Point(343, 187);
            continueButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            continueButton.Name = "continueButton";
            continueButton.Size = new System.Drawing.Size(149, 27);
            continueButton.TabIndex = 5;
            continueButton.Text = "Continue >";
            continueButton.UseVisualStyleBackColor = true;
            continueButton.Click += OnNextButtonClick;
            // 
            // _backupFolderLabel
            // 
            _backupFolderLabel.AutoSize = true;
            _backupFolderLabel.Location = new System.Drawing.Point(11, 62);
            _backupFolderLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _backupFolderLabel.Name = "_backupFolderLabel";
            _backupFolderLabel.Size = new System.Drawing.Size(83, 15);
            _backupFolderLabel.TabIndex = 9;
            _backupFolderLabel.Text = "Backup folder:";
            // 
            // _backupLocationTextBox
            // 
            _backupLocationTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _backupLocationTextBox.Location = new System.Drawing.Point(11, 80);
            _backupLocationTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _backupLocationTextBox.Name = "_backupLocationTextBox";
            _backupLocationTextBox.Size = new System.Drawing.Size(426, 23);
            _backupLocationTextBox.TabIndex = 0;
            // 
            // helpButton
            // 
            helpButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            helpButton.Location = new System.Drawing.Point(11, 181);
            helpButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            helpButton.Name = "helpButton";
            helpButton.Size = new System.Drawing.Size(38, 38);
            helpButton.TabIndex = 4;
            helpButton.Text = "?";
            helpButton.UseVisualStyleBackColor = true;
            helpButton.Click += OnHelpClick;
            // 
            // _helpLabel
            // 
            _helpLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _helpLabel.Location = new System.Drawing.Point(11, 9);
            _helpLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _helpLabel.Name = "_helpLabel";
            _helpLabel.Size = new System.Drawing.Size(575, 53);
            _helpLabel.TabIndex = 12;
            _helpLabel.Text = "Mount your backup location as disk or copy it to any folder and specify its location.";
            // 
            // OpenBackupForm
            // 
            AcceptButton = continueButton;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = closeButton;
            ClientSize = new System.Drawing.Size(602, 229);
            Controls.Add(_helpLabel);
            Controls.Add(_passwordTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(helpButton);
            Controls.Add(_backupLocationTextBox);
            Controls.Add(_backupFolderLabel);
            Controls.Add(continueButton);
            Controls.Add(closeButton);
            Controls.Add(_openBackupFolderButton);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OpenBackupForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Restoration master";
            ResumeLayout(false);
            PerformLayout();
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
