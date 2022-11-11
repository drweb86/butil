namespace BUtil.Configurator
{
	partial class NetworkStorageForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.acceptButton = new System.Windows.Forms.Button();
            this.OptionsgroupBox = new System.Windows.Forms.GroupBox();
            this.mbLabel = new System.Windows.Forms.Label();
            this.limitSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.skipIfExceedsLimitCheckBox = new System.Windows.Forms.CheckBox();
            this._EncryptUnderLsaCheckBox = new System.Windows.Forms.CheckBox();
            this.Searchbutton = new System.Windows.Forms.Button();
            this.destinationFolderTextBox = new System.Windows.Forms.TextBox();
            this.WhereToStoreBackupslabel = new System.Windows.Forms.Label();
            this.captionTextBox = new System.Windows.Forms.TextBox();
            this.namelabel = new System.Windows.Forms.Label();
            this.helpButton = new System.Windows.Forms.Button();
            this.OptionsgroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limitSizeNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancelbutton.Location = new System.Drawing.Point(475, 298);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(75, 23);
            this.Cancelbutton.TabIndex = 7;
            this.Cancelbutton.Text = "Cancel";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            this.Cancelbutton.Click += new System.EventHandler(this.cancelButtonClick);
            // 
            // acceptButton
            // 
            this.acceptButton.Enabled = false;
            this.acceptButton.Location = new System.Drawing.Point(394, 298);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 6;
            this.acceptButton.Text = "OK";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButtonClick);
            // 
            // OptionsgroupBox
            // 
            this.OptionsgroupBox.BackColor = System.Drawing.Color.Transparent;
            this.OptionsgroupBox.Controls.Add(this.mbLabel);
            this.OptionsgroupBox.Controls.Add(this.limitSizeNumericUpDown);
            this.OptionsgroupBox.Controls.Add(this.skipIfExceedsLimitCheckBox);
            this.OptionsgroupBox.Controls.Add(this._EncryptUnderLsaCheckBox);
            this.OptionsgroupBox.Controls.Add(this.Searchbutton);
            this.OptionsgroupBox.Controls.Add(this.destinationFolderTextBox);
            this.OptionsgroupBox.Controls.Add(this.WhereToStoreBackupslabel);
            this.OptionsgroupBox.Location = new System.Drawing.Point(6, 52);
            this.OptionsgroupBox.Name = "OptionsgroupBox";
            this.OptionsgroupBox.Size = new System.Drawing.Size(544, 240);
            this.OptionsgroupBox.TabIndex = 1;
            this.OptionsgroupBox.TabStop = false;
            this.OptionsgroupBox.Text = "Options";
            // 
            // mbLabel
            // 
            this.mbLabel.AutoSize = true;
            this.mbLabel.Location = new System.Drawing.Point(115, 199);
            this.mbLabel.Name = "mbLabel";
            this.mbLabel.Size = new System.Drawing.Size(22, 13);
            this.mbLabel.TabIndex = 8;
            this.mbLabel.Text = "Mb";
            // 
            // limitSizeNumericUpDown
            // 
            this.limitSizeNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.limitSizeNumericUpDown.Location = new System.Drawing.Point(20, 197);
            this.limitSizeNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.limitSizeNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.limitSizeNumericUpDown.Name = "limitSizeNumericUpDown";
            this.limitSizeNumericUpDown.Size = new System.Drawing.Size(89, 20);
            this.limitSizeNumericUpDown.TabIndex = 7;
            this.limitSizeNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // skipIfExceedsLimitCheckBox
            // 
            this.skipIfExceedsLimitCheckBox.AutoSize = true;
            this.skipIfExceedsLimitCheckBox.Location = new System.Drawing.Point(11, 174);
            this.skipIfExceedsLimitCheckBox.Name = "skipIfExceedsLimitCheckBox";
            this.skipIfExceedsLimitCheckBox.Size = new System.Drawing.Size(309, 17);
            this.skipIfExceedsLimitCheckBox.TabIndex = 6;
            this.skipIfExceedsLimitCheckBox.Text = "Skip copying and write a warning into log if backup exceeds";
            this.skipIfExceedsLimitCheckBox.UseVisualStyleBackColor = true;
            this.skipIfExceedsLimitCheckBox.CheckedChanged += new System.EventHandler(this.skipIfExceedsLimitCheckBox_CheckedChanged);
            // 
            // _EncryptUnderLsaCheckBox
            // 
            this._EncryptUnderLsaCheckBox.Location = new System.Drawing.Point(11, 103);
            this._EncryptUnderLsaCheckBox.Name = "_EncryptUnderLsaCheckBox";
            this._EncryptUnderLsaCheckBox.Size = new System.Drawing.Size(527, 65);
            this._EncryptUnderLsaCheckBox.TabIndex = 5;
            this._EncryptUnderLsaCheckBox.Text = "Additional encryption under your local system account. \r\nRequires NTFS file syste" +
                "m and win2k OS on remote machine. Backup data will be available only for your OS" +
                " account. See documentation for details";
            this._EncryptUnderLsaCheckBox.UseVisualStyleBackColor = true;
            // 
            // Searchbutton
            // 
            this.Searchbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Searchbutton.Location = new System.Drawing.Point(494, 38);
            this.Searchbutton.Name = "Searchbutton";
            this.Searchbutton.Size = new System.Drawing.Size(44, 23);
            this.Searchbutton.TabIndex = 2;
            this.Searchbutton.Text = "...";
            this.Searchbutton.UseVisualStyleBackColor = true;
            this.Searchbutton.Click += new System.EventHandler(this.searchbutton_Click);
            // 
            // destinationFolderTextBox
            // 
            this.destinationFolderTextBox.Location = new System.Drawing.Point(20, 40);
            this.destinationFolderTextBox.Name = "destinationFolderTextBox";
            this.destinationFolderTextBox.Size = new System.Drawing.Size(468, 20);
            this.destinationFolderTextBox.TabIndex = 3;
            this.destinationFolderTextBox.TextChanged += new System.EventHandler(this.requiredFieldsTextChanged);
            // 
            // WhereToStoreBackupslabel
            // 
            this.WhereToStoreBackupslabel.AutoSize = true;
            this.WhereToStoreBackupslabel.Location = new System.Drawing.Point(11, 24);
            this.WhereToStoreBackupslabel.Name = "WhereToStoreBackupslabel";
            this.WhereToStoreBackupslabel.Size = new System.Drawing.Size(171, 13);
            this.WhereToStoreBackupslabel.TabIndex = 4;
            this.WhereToStoreBackupslabel.Text = "Folder where to store your backup:";
            // 
            // captionTextBox
            // 
            this.captionTextBox.Location = new System.Drawing.Point(150, 17);
            this.captionTextBox.Name = "captionTextBox";
            this.captionTextBox.Size = new System.Drawing.Size(248, 20);
            this.captionTextBox.TabIndex = 0;
            // 
            // namelabel
            // 
            this.namelabel.BackColor = System.Drawing.Color.Transparent;
            this.namelabel.Location = new System.Drawing.Point(51, 20);
            this.namelabel.Name = "namelabel";
            this.namelabel.Size = new System.Drawing.Size(93, 23);
            this.namelabel.TabIndex = 8;
            this.namelabel.Text = "Caption:";
            this.namelabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // helpButton
            // 
            this.helpButton.Image = global::BUtil.Configurator.Icons.Help48x48;
            this.helpButton.Location = new System.Drawing.Point(6, 295);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(32, 32);
            this.helpButton.TabIndex = 13;
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.helpButtonClick);
            // 
            // NetworkStorageForm
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::BUtil.Configurator.Icons.Share48x48;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.Cancelbutton;
            this.ClientSize = new System.Drawing.Size(562, 333);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.Cancelbutton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.OptionsgroupBox);
            this.Controls.Add(this.captionTextBox);
            this.Controls.Add(this.namelabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::BUtil.Configurator.Icons.BUtilIcon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetworkStorageForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Network Storage Configuration";
            this.OptionsgroupBox.ResumeLayout(false);
            this.OptionsgroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limitSizeNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.TextBox destinationFolderTextBox;
		private System.Windows.Forms.TextBox captionTextBox;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button helpButton;

		#endregion

        private System.Windows.Forms.Button Cancelbutton;
		private System.Windows.Forms.FolderBrowserDialog fbd;
		private System.Windows.Forms.GroupBox OptionsgroupBox;
		private System.Windows.Forms.Button Searchbutton;
		private System.Windows.Forms.Label WhereToStoreBackupslabel;
		private System.Windows.Forms.Label namelabel;
        private System.Windows.Forms.CheckBox _EncryptUnderLsaCheckBox;
        private System.Windows.Forms.Label mbLabel;
        private System.Windows.Forms.NumericUpDown limitSizeNumericUpDown;
        private System.Windows.Forms.CheckBox skipIfExceedsLimitCheckBox;
	}
}