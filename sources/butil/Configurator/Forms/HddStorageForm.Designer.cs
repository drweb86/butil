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
            this.searchButton = new System.Windows.Forms.Button();
            this.destinationFolderTextBox = new System.Windows.Forms.TextBox();
            this.whereToStoreBackupLabel = new System.Windows.Forms.Label();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this.captionLabel = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchButton
            // 
            this.searchButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Location = new System.Drawing.Point(660, 32);
            this.searchButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(48, 23);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "...";
            this.searchButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButtonClick);
            // 
            // destinationFolderTextBox
            // 
            this.destinationFolderTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.destinationFolderTextBox.Location = new System.Drawing.Point(202, 32);
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
            this.whereToStoreBackupLabel.Location = new System.Drawing.Point(4, 29);
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
            this._nameTextBox.Location = new System.Drawing.Point(202, 3);
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
            this.captionLabel.Location = new System.Drawing.Point(4, 0);
            this.captionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.captionLabel.Name = "captionLabel";
            this.captionLabel.Size = new System.Drawing.Size(190, 29);
            this.captionLabel.TabIndex = 2;
            this.captionLabel.Text = "Name:";
            this.captionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Enabled = false;
            this.acceptButton.Location = new System.Drawing.Point(540, 130);
            this.acceptButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(88, 27);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "OK";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(635, 130);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 27);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Controls.Add(this.searchButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.captionLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.destinationFolderTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._nameTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.whereToStoreBackupLabel, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(712, 112);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // HddStorageForm
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(736, 169);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::BUtil.Configurator.Icons.BUtilIcon;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HddStorageForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HDD Storage Configuration";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

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
    }
}
