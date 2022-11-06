namespace BUtil.Configurator
{
	partial class HddStorageForm
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
            this.optionsGroupBox = new System.Windows.Forms.GroupBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.destinationFolderTextBox = new System.Windows.Forms.TextBox();
            this.whereToStoreBackupLabel = new System.Windows.Forms.Label();
            this.captionTextBox = new System.Windows.Forms.TextBox();
            this.captionLabel = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.optionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // optionsGroupBox
            // 
            this.optionsGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.optionsGroupBox.Controls.Add(this.searchButton);
            this.optionsGroupBox.Controls.Add(this.destinationFolderTextBox);
            this.optionsGroupBox.Controls.Add(this.whereToStoreBackupLabel);
            this.optionsGroupBox.Location = new System.Drawing.Point(3, 51);
            this.optionsGroupBox.Name = "optionsGroupBox";
            this.optionsGroupBox.Size = new System.Drawing.Size(454, 89);
            this.optionsGroupBox.TabIndex = 1;
            this.optionsGroupBox.TabStop = false;
            this.optionsGroupBox.Text = "Options";
            // 
            // searchButton
            // 
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Location = new System.Drawing.Point(404, 29);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(44, 24);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "...";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButtonClick);
            // 
            // destinationFolderTextBox
            // 
            this.destinationFolderTextBox.Location = new System.Drawing.Point(12, 32);
            this.destinationFolderTextBox.Name = "destinationFolderTextBox";
            this.destinationFolderTextBox.ReadOnly = true;
            this.destinationFolderTextBox.Size = new System.Drawing.Size(386, 20);
            this.destinationFolderTextBox.TabIndex = 2;
            this.destinationFolderTextBox.TabStop = false;
            this.destinationFolderTextBox.TextChanged += new System.EventHandler(this.requiredFieldsTextChanged);
            // 
            // whereToStoreBackupLabel
            // 
            this.whereToStoreBackupLabel.AutoSize = true;
            this.whereToStoreBackupLabel.Location = new System.Drawing.Point(9, 16);
            this.whereToStoreBackupLabel.Name = "whereToStoreBackupLabel";
            this.whereToStoreBackupLabel.Size = new System.Drawing.Size(171, 13);
            this.whereToStoreBackupLabel.TabIndex = 4;
            this.whereToStoreBackupLabel.Text = "Folder where to store your backup:";
            // 
            // captionTextBox
            // 
            this.captionTextBox.Location = new System.Drawing.Point(148, 13);
            this.captionTextBox.Name = "captionTextBox";
            this.captionTextBox.Size = new System.Drawing.Size(242, 20);
            this.captionTextBox.TabIndex = 0;
            this.captionTextBox.TextChanged += new System.EventHandler(this.requiredFieldsTextChanged);
            // 
            // captionLabel
            // 
            this.captionLabel.BackColor = System.Drawing.Color.Transparent;
            this.captionLabel.Location = new System.Drawing.Point(50, 16);
            this.captionLabel.Name = "captionLabel";
            this.captionLabel.Size = new System.Drawing.Size(92, 23);
            this.captionLabel.TabIndex = 2;
            this.captionLabel.Text = "Caption:";
            this.captionLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // acceptButton
            // 
            this.acceptButton.Enabled = false;
            this.acceptButton.Location = new System.Drawing.Point(301, 146);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 5;
            this.acceptButton.Text = "OK";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(382, 146);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // HddStorageForm
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::BUtil.Configurator.Icons.Hdd48x48;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(466, 180);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.optionsGroupBox);
            this.Controls.Add(this.captionTextBox);
            this.Controls.Add(this.captionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::BUtil.Configurator.Icons.BUtilIcon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HddStorageForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HDD Storage Configuration";
            this.optionsGroupBox.ResumeLayout(false);
            this.optionsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.TextBox destinationFolderTextBox;
		private System.Windows.Forms.Label whereToStoreBackupLabel;
		private System.Windows.Forms.TextBox captionTextBox;
		private System.Windows.Forms.Label captionLabel;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.GroupBox optionsGroupBox;
		private System.Windows.Forms.FolderBrowserDialog fbd;
	}
}
