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
            this.searchButton = new System.Windows.Forms.Button();
            this.destinationFolderTextBox = new System.Windows.Forms.TextBox();
            this.whereToStoreBackupLabel = new System.Windows.Forms.Label();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this.captionLabel = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._uploadLimitGbNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._enabledCheckBox = new System.Windows.Forms.CheckBox();
            this._unmountScriptLabel = new System.Windows.Forms.Label();
            this._mountScriptLabel = new System.Windows.Forms.Label();
            this._unmountTextBox = new System.Windows.Forms.TextBox();
            this._mountTextBox = new System.Windows.Forms.TextBox();
            this._mountButton = new System.Windows.Forms.Button();
            this._unmountButton = new System.Windows.Forms.Button();
            this._sambaButton = new System.Windows.Forms.Button();
            this._scriptsLabel = new System.Windows.Forms.Label();
            this._limitUploadLabel = new System.Windows.Forms.LinkLabel();
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
            this.destinationFolderTextBox.TextChanged += new System.EventHandler(this.RequiredFieldsTextChanged);
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
            this.acceptButton.Location = new System.Drawing.Point(584, 243);
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
            this.cancelButton.Location = new System.Drawing.Point(680, 243);
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
            this.tableLayoutPanel1.Controls.Add(this.searchButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.captionLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.destinationFolderTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._nameTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.whereToStoreBackupLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._uploadLimitGbNumericUpDown, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._enabledCheckBox, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.acceptButton, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.cancelButton, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this._unmountScriptLabel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this._mountScriptLabel, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this._unmountTextBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this._mountTextBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this._mountButton, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this._unmountButton, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this._sambaButton, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this._scriptsLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._limitUploadLabel, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(20);
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(781, 314);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // _uploadLimitGbNumericUpDown
            // 
            this._uploadLimitGbNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this._uploadLimitGbNumericUpDown.Location = new System.Drawing.Point(221, 81);
            this._uploadLimitGbNumericUpDown.Name = "_uploadLimitGbNumericUpDown";
            this._uploadLimitGbNumericUpDown.Size = new System.Drawing.Size(452, 23);
            this._uploadLimitGbNumericUpDown.TabIndex = 7;
            // 
            // _enabledCheckBox
            // 
            this._enabledCheckBox.AutoSize = true;
            this._enabledCheckBox.Checked = true;
            this._enabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._enabledCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._enabledCheckBox.Location = new System.Drawing.Point(23, 243);
            this._enabledCheckBox.Name = "_enabledCheckBox";
            this._enabledCheckBox.Size = new System.Drawing.Size(192, 19);
            this._enabledCheckBox.TabIndex = 8;
            this._enabledCheckBox.Text = "Enabled";
            this._enabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // _unmountScriptLabel
            // 
            this._unmountScriptLabel.AutoSize = true;
            this._unmountScriptLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._unmountScriptLabel.Location = new System.Drawing.Point(24, 190);
            this._unmountScriptLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._unmountScriptLabel.Name = "_unmountScriptLabel";
            this._unmountScriptLabel.Size = new System.Drawing.Size(190, 50);
            this._unmountScriptLabel.TabIndex = 10;
            this._unmountScriptLabel.Text = "Unmount:";
            this._unmountScriptLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _mountScriptLabel
            // 
            this._mountScriptLabel.AutoSize = true;
            this._mountScriptLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mountScriptLabel.Location = new System.Drawing.Point(24, 140);
            this._mountScriptLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mountScriptLabel.Name = "_mountScriptLabel";
            this._mountScriptLabel.Size = new System.Drawing.Size(190, 50);
            this._mountScriptLabel.TabIndex = 9;
            this._mountScriptLabel.Text = "Mount:";
            this._mountScriptLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _unmountTextBox
            // 
            this._unmountTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._unmountTextBox.Location = new System.Drawing.Point(221, 193);
            this._unmountTextBox.Multiline = true;
            this._unmountTextBox.Name = "_unmountTextBox";
            this._unmountTextBox.Size = new System.Drawing.Size(452, 44);
            this._unmountTextBox.TabIndex = 12;
            // 
            // _mountTextBox
            // 
            this._mountTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mountTextBox.Location = new System.Drawing.Point(221, 143);
            this._mountTextBox.Multiline = true;
            this._mountTextBox.Name = "_mountTextBox";
            this._mountTextBox.Size = new System.Drawing.Size(452, 44);
            this._mountTextBox.TabIndex = 11;
            // 
            // _mountButton
            // 
            this._mountButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mountButton.Location = new System.Drawing.Point(680, 143);
            this._mountButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._mountButton.MaximumSize = new System.Drawing.Size(88, 27);
            this._mountButton.MinimumSize = new System.Drawing.Size(88, 27);
            this._mountButton.Name = "_mountButton";
            this._mountButton.Size = new System.Drawing.Size(88, 27);
            this._mountButton.TabIndex = 13;
            this._mountButton.Text = "Run";
            this._mountButton.UseVisualStyleBackColor = true;
            this._mountButton.Click += new System.EventHandler(this.OnRunMountScript);
            // 
            // _unmountButton
            // 
            this._unmountButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this._unmountButton.Location = new System.Drawing.Point(680, 193);
            this._unmountButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._unmountButton.MaximumSize = new System.Drawing.Size(88, 27);
            this._unmountButton.MinimumSize = new System.Drawing.Size(88, 27);
            this._unmountButton.Name = "_unmountButton";
            this._unmountButton.Size = new System.Drawing.Size(88, 27);
            this._unmountButton.TabIndex = 14;
            this._unmountButton.Text = "Run";
            this._unmountButton.UseVisualStyleBackColor = true;
            this._unmountButton.Click += new System.EventHandler(this.OnMountRun);
            // 
            // _sambaButton
            // 
            this._sambaButton.Location = new System.Drawing.Point(680, 110);
            this._sambaButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._sambaButton.MaximumSize = new System.Drawing.Size(88, 27);
            this._sambaButton.MinimumSize = new System.Drawing.Size(88, 27);
            this._sambaButton.Name = "_sambaButton";
            this._sambaButton.Size = new System.Drawing.Size(88, 27);
            this._sambaButton.TabIndex = 15;
            this._sambaButton.Text = "Samba...";
            this._sambaButton.UseVisualStyleBackColor = true;
            this._sambaButton.Click += new System.EventHandler(this.OnSambaButtonClick);
            // 
            // _scriptsLabel
            // 
            this._scriptsLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this._scriptsLabel, 2);
            this._scriptsLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this._scriptsLabel.Location = new System.Drawing.Point(123, 107);
            this._scriptsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._scriptsLabel.MaximumSize = new System.Drawing.Size(600, 0);
            this._scriptsLabel.Name = "_scriptsLabel";
            this._scriptsLabel.Size = new System.Drawing.Size(549, 33);
            this._scriptsLabel.TabIndex = 16;
            this._scriptsLabel.Text = "If folder becomes accessible after mounting, specify PowerShell scripts for  moun" +
    "ting and unmounting";
            this._scriptsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _limitUploadLabel
            // 
            this._limitUploadLabel.AutoSize = true;
            this._limitUploadLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._limitUploadLabel.Location = new System.Drawing.Point(23, 78);
            this._limitUploadLabel.Name = "_limitUploadLabel";
            this._limitUploadLabel.Size = new System.Drawing.Size(192, 29);
            this._limitUploadLabel.TabIndex = 17;
            this._limitUploadLabel.TabStop = true;
            this._limitUploadLabel.Text = "Upload limit, GB:";
            this._limitUploadLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._limitUploadLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnUploadLimitClick);
            // 
            // FolderStorageForm
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(781, 314);
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
        private System.Windows.Forms.NumericUpDown _uploadLimitGbNumericUpDown;
        private System.Windows.Forms.CheckBox _enabledCheckBox;
        private System.Windows.Forms.Label _unmountScriptLabel;
        private System.Windows.Forms.Label _mountScriptLabel;
        private System.Windows.Forms.TextBox _unmountTextBox;
        private System.Windows.Forms.TextBox _mountTextBox;
        private System.Windows.Forms.Button _mountButton;
        private System.Windows.Forms.Button _unmountButton;
        private System.Windows.Forms.Button _sambaButton;
        private System.Windows.Forms.Label _scriptsLabel;
        private System.Windows.Forms.LinkLabel _limitUploadLabel;
    }
}
