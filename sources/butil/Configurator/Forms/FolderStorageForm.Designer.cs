namespace BUtil.Configurator
{
	partial class FolderStorageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FolderStorageForm));
            this.searchButton = new System.Windows.Forms.Button();
            this.destinationFolderTextBox = new System.Windows.Forms.TextBox();
            this.whereToStoreBackupLabel = new System.Windows.Forms.Label();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this.captionLabel = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._limitUploadLabel = new System.Windows.Forms.Label();
            this._uploadLimitGbNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._limitUploadDescriptionLabel = new System.Windows.Forms.Label();
            this._enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._uploadLimitGbNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // searchButton
            // 
            this.searchButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Location = new System.Drawing.Point(680, 52);
            this.searchButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(88, 23);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "...";
            this.searchButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButtonClick);
            // 
            // destinationFolderTextBox
            // 
            this.destinationFolderTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.destinationFolderTextBox.Location = new System.Drawing.Point(222, 52);
            this.destinationFolderTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.destinationFolderTextBox.Name = "destinationFolderTextBox";
            this.destinationFolderTextBox.Size = new System.Drawing.Size(450, 23);
            this.destinationFolderTextBox.TabIndex = 2;
            this.destinationFolderTextBox.TabStop = false;
            this.destinationFolderTextBox.TextChanged += new System.EventHandler(this.requiredFieldsTextChanged);
            // 
            // whereToStoreBackupLabel
            // 
            this.whereToStoreBackupLabel.AutoSize = true;
            this.whereToStoreBackupLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whereToStoreBackupLabel.Location = new System.Drawing.Point(24, 49);
            this.whereToStoreBackupLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.whereToStoreBackupLabel.Name = "whereToStoreBackupLabel";
            this.whereToStoreBackupLabel.Size = new System.Drawing.Size(190, 29);
            this.whereToStoreBackupLabel.TabIndex = 4;
            this.whereToStoreBackupLabel.Text = "Folder where to store your backup:";
            this.whereToStoreBackupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _nameTextBox
            // 
            this._nameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._nameTextBox.Location = new System.Drawing.Point(222, 23);
            this._nameTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._nameTextBox.Name = "_nameTextBox";
            this._nameTextBox.Size = new System.Drawing.Size(450, 23);
            this._nameTextBox.TabIndex = 1;
            this._nameTextBox.TextChanged += new System.EventHandler(this.OnNameChange);
            // 
            // captionLabel
            // 
            this.captionLabel.BackColor = System.Drawing.Color.Transparent;
            this.captionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captionLabel.Location = new System.Drawing.Point(24, 20);
            this.captionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.captionLabel.Name = "captionLabel";
            this.captionLabel.Size = new System.Drawing.Size(190, 29);
            this.captionLabel.TabIndex = 2;
            this.captionLabel.Text = "Name:";
            this.captionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // acceptButton
            // 
            this.acceptButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.acceptButton.Enabled = false;
            this.acceptButton.Location = new System.Drawing.Point(584, 190);
            this.acceptButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.acceptButton.MaximumSize = new System.Drawing.Size(88, 27);
            this.acceptButton.MinimumSize = new System.Drawing.Size(88, 27);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(88, 27);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "OK";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Location = new System.Drawing.Point(680, 190);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cancelButton.MaximumSize = new System.Drawing.Size(88, 27);
            this.cancelButton.MinimumSize = new System.Drawing.Size(88, 27);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 27);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.searchButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.acceptButton, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.cancelButton, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.captionLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.destinationFolderTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._nameTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.whereToStoreBackupLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._limitUploadLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._uploadLimitGbNumericUpDown, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._limitUploadDescriptionLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._enabledCheckBox, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(20);
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(781, 319);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // _limitUploadLabel
            // 
            this._limitUploadLabel.AutoSize = true;
            this._limitUploadLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._limitUploadLabel.Location = new System.Drawing.Point(24, 78);
            this._limitUploadLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._limitUploadLabel.Name = "_limitUploadLabel";
            this._limitUploadLabel.Size = new System.Drawing.Size(190, 29);
            this._limitUploadLabel.TabIndex = 5;
            this._limitUploadLabel.Text = "Upload limit, GB:";
            this._limitUploadLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _uploadLimitGbNumericUpDown
            // 
            this._uploadLimitGbNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this._uploadLimitGbNumericUpDown.Location = new System.Drawing.Point(221, 81);
            this._uploadLimitGbNumericUpDown.Name = "_uploadLimitGbNumericUpDown";
            this._uploadLimitGbNumericUpDown.Size = new System.Drawing.Size(452, 23);
            this._uploadLimitGbNumericUpDown.TabIndex = 7;
            // 
            // _limitUploadDescriptionLabel
            // 
            this._limitUploadDescriptionLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this._limitUploadDescriptionLabel, 3);
            this._limitUploadDescriptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._limitUploadDescriptionLabel.Location = new System.Drawing.Point(24, 107);
            this._limitUploadDescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._limitUploadDescriptionLabel.MaximumSize = new System.Drawing.Size(700, 0);
            this._limitUploadDescriptionLabel.Name = "_limitUploadDescriptionLabel";
            this._limitUploadDescriptionLabel.Size = new System.Drawing.Size(700, 60);
            this._limitUploadDescriptionLabel.TabIndex = 6;
            this._limitUploadDescriptionLabel.Text = resources.GetString("_limitUploadDescriptionLabel.Text");
            // 
            // _enabledCheckBox
            // 
            this._enabledCheckBox.AutoSize = true;
            this._enabledCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._enabledCheckBox.Location = new System.Drawing.Point(23, 190);
            this._enabledCheckBox.Name = "_enabledCheckBox";
            this._enabledCheckBox.Size = new System.Drawing.Size(192, 19);
            this._enabledCheckBox.TabIndex = 8;
            this._enabledCheckBox.Text = "Enabled";
            this._enabledCheckBox.Checked = true;
            this._enabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // HddStorageForm
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(781, 319);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = global::BUtil.Configurator.Icons.BUtilIcon;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FolderStorageForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Folder Storage Configuration";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._uploadLimitGbNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.TextBox destinationFolderTextBox;
		private System.Windows.Forms.Label whereToStoreBackupLabel;
		private System.Windows.Forms.TextBox _nameTextBox;
		private System.Windows.Forms.Label captionLabel;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label _limitUploadLabel;
        private System.Windows.Forms.Label _limitUploadDescriptionLabel;
        private System.Windows.Forms.NumericUpDown _uploadLimitGbNumericUpDown;
        private System.Windows.Forms.CheckBox _enabledCheckBox;
    }
}
