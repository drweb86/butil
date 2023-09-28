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
            Core.ConfigurationFileModels.V2.FolderStorageSettingsV2 folderStorageSettingsV21 = new Core.ConfigurationFileModels.V2.FolderStorageSettingsV2();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenBackupForm));
            closeButton = new System.Windows.Forms.Button();
            _passwordTextBox = new System.Windows.Forms.MaskedTextBox();
            passwordLabel = new System.Windows.Forms.Label();
            continueButton = new System.Windows.Forms.Button();
            _whereUserControl = new Configurator.Configurator.Controls.WhereUserControl();
            SuspendLayout();
            // 
            // closeButton
            // 
            closeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            closeButton.AutoSize = true;
            closeButton.Location = new System.Drawing.Point(710, 378);
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
            _passwordTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _passwordTextBox.Location = new System.Drawing.Point(13, 343);
            _passwordTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _passwordTextBox.Name = "_passwordTextBox";
            _passwordTextBox.PasswordChar = '*';
            _passwordTextBox.Size = new System.Drawing.Size(785, 23);
            _passwordTextBox.TabIndex = 3;
            // 
            // passwordLabel
            // 
            passwordLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new System.Drawing.Point(13, 319);
            passwordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new System.Drawing.Size(348, 15);
            passwordLabel.TabIndex = 0;
            passwordLabel.Text = "If your backup is password protected, please type password here:";
            // 
            // continueButton
            // 
            continueButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            continueButton.Location = new System.Drawing.Point(553, 378);
            continueButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            continueButton.Name = "continueButton";
            continueButton.Size = new System.Drawing.Size(149, 27);
            continueButton.TabIndex = 5;
            continueButton.Text = "Continue >";
            continueButton.UseVisualStyleBackColor = true;
            continueButton.Click += OnNextButtonClick;
            // 
            // _whereUserControl
            // 
            _whereUserControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _whereUserControl.BackColor = System.Drawing.SystemColors.Window;
            _whereUserControl.HelpLabel = null;
            _whereUserControl.Location = new System.Drawing.Point(9, 12);
            _whereUserControl.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            _whereUserControl.MinimumSize = new System.Drawing.Size(332, 231);
            _whereUserControl.Name = "_whereUserControl";
            _whereUserControl.Size = new System.Drawing.Size(787, 286);
            folderStorageSettingsV21.DestinationFolder = "";
            folderStorageSettingsV21.MountPowershellScript = "";
            folderStorageSettingsV21.SingleBackupQuotaGb = 0L;
            folderStorageSettingsV21.UnmountPowershellScript = "";
            _whereUserControl.StorageSettings = folderStorageSettingsV21;
            _whereUserControl.TabIndex = 7;
            // 
            // OpenBackupForm
            // 
            AcceptButton = continueButton;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = closeButton;
            ClientSize = new System.Drawing.Size(812, 420);
            Controls.Add(_whereUserControl);
            Controls.Add(_passwordTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(continueButton);
            Controls.Add(closeButton);
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

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.MaskedTextBox _passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Button continueButton;
        private Configurator.Configurator.Controls.WhereUserControl _whereUserControl;
    }
}
