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
            helpButton = new System.Windows.Forms.Button();
            _whereUserControl = new Configurator.Configurator.Controls.WhereUserControl();
            SuspendLayout();
            // 
            // closeButton
            // 
            closeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            closeButton.AutoSize = true;
            closeButton.Location = new System.Drawing.Point(1014, 630);
            closeButton.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            closeButton.Name = "closeButton";
            closeButton.Size = new System.Drawing.Size(123, 45);
            closeButton.TabIndex = 6;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += OnCloseButtonClick;
            // 
            // _passwordTextBox
            // 
            _passwordTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _passwordTextBox.Location = new System.Drawing.Point(19, 572);
            _passwordTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            _passwordTextBox.Name = "_passwordTextBox";
            _passwordTextBox.PasswordChar = '*';
            _passwordTextBox.Size = new System.Drawing.Size(1120, 31);
            _passwordTextBox.TabIndex = 3;
            // 
            // passwordLabel
            // 
            passwordLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new System.Drawing.Point(19, 532);
            passwordLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new System.Drawing.Size(532, 25);
            passwordLabel.TabIndex = 0;
            passwordLabel.Text = "If your backup is password protected, please type password here:";
            // 
            // continueButton
            // 
            continueButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            continueButton.Location = new System.Drawing.Point(790, 630);
            continueButton.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            continueButton.Name = "continueButton";
            continueButton.Size = new System.Drawing.Size(213, 45);
            continueButton.TabIndex = 5;
            continueButton.Text = "Continue >";
            continueButton.UseVisualStyleBackColor = true;
            continueButton.Click += OnNextButtonClick;
            // 
            // helpButton
            // 
            helpButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            helpButton.Location = new System.Drawing.Point(16, 620);
            helpButton.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            helpButton.Name = "helpButton";
            helpButton.Size = new System.Drawing.Size(54, 63);
            helpButton.TabIndex = 4;
            helpButton.Text = "?";
            helpButton.UseVisualStyleBackColor = true;
            helpButton.Click += OnHelpClick;
            // 
            // _whereUserControl
            // 
            _whereUserControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _whereUserControl.BackColor = System.Drawing.SystemColors.Window;
            _whereUserControl.HelpLabel = null;
            _whereUserControl.Location = new System.Drawing.Point(13, 20);
            _whereUserControl.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            _whereUserControl.MinimumSize = new System.Drawing.Size(474, 385);
            _whereUserControl.Name = "_whereUserControl";
            _whereUserControl.Size = new System.Drawing.Size(1124, 477);
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
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = closeButton;
            ClientSize = new System.Drawing.Size(1160, 700);
            Controls.Add(_whereUserControl);
            Controls.Add(_passwordTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(helpButton);
            Controls.Add(continueButton);
            Controls.Add(closeButton);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OpenBackupForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Restoration master";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.MaskedTextBox _passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Button continueButton;
        private Configurator.Configurator.Controls.WhereUserControl _whereUserControl;
    }
}
