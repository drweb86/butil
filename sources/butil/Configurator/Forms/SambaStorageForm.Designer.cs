namespace BUtil.Configurator
{
	partial class SambaStorageForm
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
            this._shareTextBox = new System.Windows.Forms.TextBox();
            this._shareLabel = new System.Windows.Forms.Label();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this.captionLabel = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._uploadLimitGbNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._enabledCheckBox = new System.Windows.Forms.CheckBox();
            this._passwordLabel = new System.Windows.Forms.Label();
            this._userLabel = new System.Windows.Forms.Label();
            this._passwordTextBox = new System.Windows.Forms.TextBox();
            this._userTextBox = new System.Windows.Forms.TextBox();
            this._limitUploadLabel = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._uploadLimitGbNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // _shareTextBox
            // 
            this._shareTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._shareTextBox.Location = new System.Drawing.Point(222, 52);
            this._shareTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._shareTextBox.Name = "_shareTextBox";
            this._shareTextBox.PlaceholderText = "\\\\192.168.11.1\\share\\folder";
            this._shareTextBox.Size = new System.Drawing.Size(450, 23);
            this._shareTextBox.TabIndex = 2;
            this._shareTextBox.TabStop = false;
            this._shareTextBox.TextChanged += new System.EventHandler(this.RequiredFieldsTextChanged);
            // 
            // _shareLabel
            // 
            this._shareLabel.AutoSize = true;
            this._shareLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._shareLabel.Location = new System.Drawing.Point(24, 49);
            this._shareLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._shareLabel.Name = "_shareLabel";
            this._shareLabel.Size = new System.Drawing.Size(190, 29);
            this._shareLabel.TabIndex = 4;
            this._shareLabel.Text = "Share:";
            this._shareLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.acceptButton.Location = new System.Drawing.Point(584, 168);
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
            this.cancelButton.Location = new System.Drawing.Point(680, 168);
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
            this.tableLayoutPanel1.Controls.Add(this.captionLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._shareTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._nameTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._shareLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._uploadLimitGbNumericUpDown, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._enabledCheckBox, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.acceptButton, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.cancelButton, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this._passwordLabel, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this._userLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._passwordTextBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this._userTextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this._limitUploadLabel, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(20);
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(781, 223);
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
            this._enabledCheckBox.Location = new System.Drawing.Point(23, 168);
            this._enabledCheckBox.Name = "_enabledCheckBox";
            this._enabledCheckBox.Size = new System.Drawing.Size(192, 19);
            this._enabledCheckBox.TabIndex = 8;
            this._enabledCheckBox.Text = "Enabled";
            this._enabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // _passwordLabel
            // 
            this._passwordLabel.AutoSize = true;
            this._passwordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._passwordLabel.Location = new System.Drawing.Point(24, 136);
            this._passwordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new System.Drawing.Size(190, 29);
            this._passwordLabel.TabIndex = 10;
            this._passwordLabel.Text = "Password:";
            this._passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _userLabel
            // 
            this._userLabel.AutoSize = true;
            this._userLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._userLabel.Location = new System.Drawing.Point(24, 107);
            this._userLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._userLabel.Name = "_userLabel";
            this._userLabel.Size = new System.Drawing.Size(190, 29);
            this._userLabel.TabIndex = 9;
            this._userLabel.Text = "User:";
            this._userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _passwordTextBox
            // 
            this._passwordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._passwordTextBox.Location = new System.Drawing.Point(221, 139);
            this._passwordTextBox.Name = "_passwordTextBox";
            this._passwordTextBox.Size = new System.Drawing.Size(452, 23);
            this._passwordTextBox.TabIndex = 12;
            // 
            // _userTextBox
            // 
            this._userTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._userTextBox.Location = new System.Drawing.Point(221, 110);
            this._userTextBox.Name = "_userTextBox";
            this._userTextBox.Size = new System.Drawing.Size(452, 23);
            this._userTextBox.TabIndex = 11;
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
            // SambaStorageForm
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(781, 223);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = global::BUtil.Configurator.Icons.BUtilIcon;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SambaStorageForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Samba";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._uploadLimitGbNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.TextBox _shareTextBox;
		private System.Windows.Forms.Label _shareLabel;
		private System.Windows.Forms.TextBox _nameTextBox;
		private System.Windows.Forms.Label captionLabel;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown _uploadLimitGbNumericUpDown;
        private System.Windows.Forms.CheckBox _enabledCheckBox;
        private System.Windows.Forms.Label _passwordLabel;
        private System.Windows.Forms.Label _userLabel;
        private System.Windows.Forms.TextBox _passwordTextBox;
        private System.Windows.Forms.TextBox _userTextBox;
        private System.Windows.Forms.LinkLabel _limitUploadLabel;
    }
}
