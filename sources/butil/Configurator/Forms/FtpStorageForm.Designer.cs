namespace BUtil.Configurator
{
	partial class FtpStorageForm
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
            this.captionTextBox = new System.Windows.Forms.TextBox();
            this.StorageNamelabel = new System.Windows.Forms.Label();
            this.destinationFolderGroupBox = new System.Windows.Forms.GroupBox();
            this.destinationFolderTextBox = new System.Windows.Forms.TextBox();
            this.AuthorizationInformationGroupBox = new System.Windows.Forms.GroupBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.RemoteServergroupBox = new System.Windows.Forms.GroupBox();
            this.connectionModeComboBox = new System.Windows.Forms.ComboBox();
            this.dataTransferModeLabel = new System.Windows.Forms.Label();
            this.ftpServerTextBox = new System.Windows.Forms.TextBox();
            this.hostlabel = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.CANCELbutton = new System.Windows.Forms.Button();
            this.testButton = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.destinationFolderGroupBox.SuspendLayout();
            this.AuthorizationInformationGroupBox.SuspendLayout();
            this.RemoteServergroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // captionTextBox
            // 
            this.captionTextBox.Location = new System.Drawing.Point(158, 6);
            this.captionTextBox.Name = "captionTextBox";
            this.captionTextBox.Size = new System.Drawing.Size(178, 20);
            this.captionTextBox.TabIndex = 0;
            this.captionTextBox.TextChanged += new System.EventHandler(this.requiredFieldsTextChanged);
            // 
            // StorageNamelabel
            // 
            this.StorageNamelabel.BackColor = System.Drawing.Color.Transparent;
            this.StorageNamelabel.Location = new System.Drawing.Point(12, 9);
            this.StorageNamelabel.Name = "StorageNamelabel";
            this.StorageNamelabel.Size = new System.Drawing.Size(140, 20);
            this.StorageNamelabel.TabIndex = 11;
            this.StorageNamelabel.Text = "Caption:";
            this.StorageNamelabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // destinationFolderGroupBox
            // 
            this.destinationFolderGroupBox.Controls.Add(this.destinationFolderTextBox);
            this.destinationFolderGroupBox.Location = new System.Drawing.Point(12, 198);
            this.destinationFolderGroupBox.Name = "destinationFolderGroupBox";
            this.destinationFolderGroupBox.Size = new System.Drawing.Size(337, 84);
            this.destinationFolderGroupBox.TabIndex = 7;
            this.destinationFolderGroupBox.TabStop = false;
            this.destinationFolderGroupBox.Text = "Destination folder";
            // 
            // destinationFolderTextBox
            // 
            this.destinationFolderTextBox.Location = new System.Drawing.Point(15, 19);
            this.destinationFolderTextBox.Name = "destinationFolderTextBox";
            this.destinationFolderTextBox.Size = new System.Drawing.Size(309, 20);
            this.destinationFolderTextBox.TabIndex = 8;
            this.destinationFolderTextBox.Text = "/mybackup";
            this.destinationFolderTextBox.TextChanged += new System.EventHandler(this.requiredFieldsTextChanged);
            // 
            // AuthorizationInformationGroupBox
            // 
            this.AuthorizationInformationGroupBox.Controls.Add(this.userLabel);
            this.AuthorizationInformationGroupBox.Controls.Add(this.passwordTextBox);
            this.AuthorizationInformationGroupBox.Controls.Add(this.userTextBox);
            this.AuthorizationInformationGroupBox.Controls.Add(this.passwordLabel);
            this.AuthorizationInformationGroupBox.Location = new System.Drawing.Point(12, 111);
            this.AuthorizationInformationGroupBox.Name = "AuthorizationInformationGroupBox";
            this.AuthorizationInformationGroupBox.Size = new System.Drawing.Size(337, 81);
            this.AuthorizationInformationGroupBox.TabIndex = 4;
            this.AuthorizationInformationGroupBox.TabStop = false;
            this.AuthorizationInformationGroupBox.Text = "Authorization information";
            // 
            // userLabel
            // 
            this.userLabel.Location = new System.Drawing.Point(6, 22);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(134, 23);
            this.userLabel.TabIndex = 4;
            this.userLabel.Text = "User:";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(146, 45);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(178, 20);
            this.passwordTextBox.TabIndex = 6;
            this.passwordTextBox.Text = "butil";
            this.passwordTextBox.UseSystemPasswordChar = true;
            this.passwordTextBox.TextChanged += new System.EventHandler(this.requiredFieldsTextChanged);
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(146, 19);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(178, 20);
            this.userTextBox.TabIndex = 5;
            this.userTextBox.Text = "anonymous";
            this.userTextBox.TextChanged += new System.EventHandler(this.requiredFieldsTextChanged);
            // 
            // passwordLabel
            // 
            this.passwordLabel.Location = new System.Drawing.Point(6, 48);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(134, 23);
            this.passwordLabel.TabIndex = 6;
            this.passwordLabel.Text = "Password:";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RemoteServergroupBox
            // 
            this.RemoteServergroupBox.BackColor = System.Drawing.Color.Transparent;
            this.RemoteServergroupBox.Controls.Add(this.connectionModeComboBox);
            this.RemoteServergroupBox.Controls.Add(this.dataTransferModeLabel);
            this.RemoteServergroupBox.Controls.Add(this.ftpServerTextBox);
            this.RemoteServergroupBox.Controls.Add(this.hostlabel);
            this.RemoteServergroupBox.Location = new System.Drawing.Point(12, 32);
            this.RemoteServergroupBox.Name = "RemoteServergroupBox";
            this.RemoteServergroupBox.Size = new System.Drawing.Size(337, 73);
            this.RemoteServergroupBox.TabIndex = 1;
            this.RemoteServergroupBox.TabStop = false;
            this.RemoteServergroupBox.Text = "Server";
            // 
            // connectionModeComboBox
            // 
            this.connectionModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.connectionModeComboBox.FormattingEnabled = true;
            this.connectionModeComboBox.Items.AddRange(new object[] {
            "Active",
            "Passive"});
            this.connectionModeComboBox.Location = new System.Drawing.Point(146, 40);
            this.connectionModeComboBox.Name = "connectionModeComboBox";
            this.connectionModeComboBox.Size = new System.Drawing.Size(178, 21);
            this.connectionModeComboBox.TabIndex = 3;
            // 
            // dataTransferModeLabel
            // 
            this.dataTransferModeLabel.Location = new System.Drawing.Point(6, 34);
            this.dataTransferModeLabel.Name = "dataTransferModeLabel";
            this.dataTransferModeLabel.Size = new System.Drawing.Size(134, 30);
            this.dataTransferModeLabel.TabIndex = 3;
            this.dataTransferModeLabel.Text = "Data transfer mode:";
            this.dataTransferModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ftpServerTextBox
            // 
            this.ftpServerTextBox.Location = new System.Drawing.Point(146, 15);
            this.ftpServerTextBox.Name = "ftpServerTextBox";
            this.ftpServerTextBox.Size = new System.Drawing.Size(178, 20);
            this.ftpServerTextBox.TabIndex = 2;
            this.ftpServerTextBox.TextChanged += new System.EventHandler(this.requiredFieldsTextChanged);
            // 
            // hostlabel
            // 
            this.hostlabel.Location = new System.Drawing.Point(6, 18);
            this.hostlabel.Name = "hostlabel";
            this.hostlabel.Size = new System.Drawing.Size(134, 23);
            this.hostlabel.TabIndex = 0;
            this.hostlabel.Text = "Host name or IP: ftp://";
            this.hostlabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // acceptButton
            // 
            this.acceptButton.Enabled = false;
            this.acceptButton.Location = new System.Drawing.Point(193, 297);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 12;
            this.acceptButton.Text = "OK";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.okButtonClick);
            // 
            // CANCELbutton
            // 
            this.CANCELbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CANCELbutton.Location = new System.Drawing.Point(274, 297);
            this.CANCELbutton.Name = "CANCELbutton";
            this.CANCELbutton.Size = new System.Drawing.Size(75, 23);
            this.CANCELbutton.TabIndex = 13;
            this.CANCELbutton.Text = "Cancel";
            this.CANCELbutton.UseVisualStyleBackColor = true;
            // 
            // testButton
            // 
            this.testButton.AutoSize = true;
            this.testButton.Enabled = false;
            this.testButton.Location = new System.Drawing.Point(50, 297);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 23);
            this.testButton.TabIndex = 11;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButtonClick);
            // 
            // helpButton
            // 
            this.helpButton.Image = global::BUtil.Configurator.Icons.Help48x48;
            this.helpButton.Location = new System.Drawing.Point(12, 288);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(32, 32);
            this.helpButton.TabIndex = 10;
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.helpButtonClick);
            // 
            // FtpStorageForm
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::BUtil.Configurator.Icons.Ftp48x48;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.CANCELbutton;
            this.ClientSize = new System.Drawing.Size(362, 331);
            this.Controls.Add(this.destinationFolderGroupBox);
            this.Controls.Add(this.captionTextBox);
            this.Controls.Add(this.AuthorizationInformationGroupBox);
            this.Controls.Add(this.StorageNamelabel);
            this.Controls.Add(this.RemoteServergroupBox);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.CANCELbutton);
            this.Controls.Add(this.acceptButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::BUtil.Configurator.Icons.BUtilIcon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FtpStorageForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FTP Storage Configuration";
            this.destinationFolderGroupBox.ResumeLayout(false);
            this.destinationFolderGroupBox.PerformLayout();
            this.AuthorizationInformationGroupBox.ResumeLayout(false);
            this.AuthorizationInformationGroupBox.PerformLayout();
            this.RemoteServergroupBox.ResumeLayout(false);
            this.RemoteServergroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.GroupBox destinationFolderGroupBox;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.Label dataTransferModeLabel;
		private System.Windows.Forms.TextBox passwordTextBox;
		private System.Windows.Forms.TextBox destinationFolderTextBox;
		private System.Windows.Forms.Label userLabel;
		private System.Windows.Forms.TextBox userTextBox;
		private System.Windows.Forms.ComboBox connectionModeComboBox;
		private System.Windows.Forms.TextBox ftpServerTextBox;
		private System.Windows.Forms.Button testButton;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.TextBox captionTextBox;
		private System.Windows.Forms.Button helpButton;
		private System.Windows.Forms.Label StorageNamelabel;
		private System.Windows.Forms.Button CANCELbutton;
		private System.Windows.Forms.Label hostlabel;
		private System.Windows.Forms.GroupBox RemoteServergroupBox;
		private System.Windows.Forms.GroupBox AuthorizationInformationGroupBox;
	}
}
