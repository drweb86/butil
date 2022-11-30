namespace BUtil.Configurator
{
	partial class SambaForm
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
            this._shareNameTextBox = new System.Windows.Forms.TextBox();
            this._shareNameLabel = new System.Windows.Forms.Label();
            this._hostTextBox = new System.Windows.Forms.TextBox();
            this._hostLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this._userTextBox = new System.Windows.Forms.TextBox();
            this._passwordTextBox = new System.Windows.Forms.TextBox();
            this._userLabel = new System.Windows.Forms.Label();
            this._passwordLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _shareNameTextBox
            // 
            this._shareNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._shareNameTextBox.Location = new System.Drawing.Point(222, 52);
            this._shareNameTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._shareNameTextBox.Name = "_shareNameTextBox";
            this._shareNameTextBox.PlaceholderText = "share";
            this._shareNameTextBox.Size = new System.Drawing.Size(450, 23);
            this._shareNameTextBox.TabIndex = 2;
            this._shareNameTextBox.TabStop = false;
            // 
            // _shareNameLabel
            // 
            this._shareNameLabel.AutoSize = true;
            this._shareNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._shareNameLabel.Location = new System.Drawing.Point(24, 49);
            this._shareNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._shareNameLabel.Name = "_shareNameLabel";
            this._shareNameLabel.Size = new System.Drawing.Size(190, 29);
            this._shareNameLabel.TabIndex = 4;
            this._shareNameLabel.Text = "Share Name:";
            this._shareNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _hostTextBox
            // 
            this._hostTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._hostTextBox.Location = new System.Drawing.Point(222, 23);
            this._hostTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._hostTextBox.Name = "_hostTextBox";
            this._hostTextBox.PlaceholderText = "100.100.100.100 / server.name";
            this._hostTextBox.Size = new System.Drawing.Size(450, 23);
            this._hostTextBox.TabIndex = 1;
            // 
            // _hostLabel
            // 
            this._hostLabel.BackColor = System.Drawing.Color.Transparent;
            this._hostLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._hostLabel.Location = new System.Drawing.Point(24, 20);
            this._hostLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._hostLabel.Name = "_hostLabel";
            this._hostLabel.Size = new System.Drawing.Size(190, 29);
            this._hostLabel.TabIndex = 2;
            this._hostLabel.Text = "Host:";
            this._hostLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this._hostLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._shareNameTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._hostTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._shareNameLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.acceptButton, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.cancelButton, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this._userTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._passwordTextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this._userLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._passwordLabel, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(20);
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(789, 207);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // acceptButton
            // 
            this.acceptButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.acceptButton.Location = new System.Drawing.Point(584, 139);
            this.acceptButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.acceptButton.MaximumSize = new System.Drawing.Size(88, 27);
            this.acceptButton.MinimumSize = new System.Drawing.Size(88, 27);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(88, 27);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "OK";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.OnAccept);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Location = new System.Drawing.Point(680, 139);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cancelButton.MaximumSize = new System.Drawing.Size(88, 27);
            this.cancelButton.MinimumSize = new System.Drawing.Size(88, 27);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 27);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // _userTextBox
            // 
            this._userTextBox.Location = new System.Drawing.Point(221, 81);
            this._userTextBox.Name = "_userTextBox";
            this._userTextBox.Size = new System.Drawing.Size(452, 23);
            this._userTextBox.TabIndex = 11;
            // 
            // _passwordTextBox
            // 
            this._passwordTextBox.Location = new System.Drawing.Point(221, 110);
            this._passwordTextBox.Name = "_passwordTextBox";
            this._passwordTextBox.Size = new System.Drawing.Size(452, 23);
            this._passwordTextBox.TabIndex = 12;
            // 
            // _userLabel
            // 
            this._userLabel.AutoSize = true;
            this._userLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._userLabel.Location = new System.Drawing.Point(24, 78);
            this._userLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._userLabel.Name = "_userLabel";
            this._userLabel.Size = new System.Drawing.Size(190, 29);
            this._userLabel.TabIndex = 9;
            this._userLabel.Text = "User:";
            this._userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _passwordLabel
            // 
            this._passwordLabel.AutoSize = true;
            this._passwordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._passwordLabel.Location = new System.Drawing.Point(24, 107);
            this._passwordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new System.Drawing.Size(190, 29);
            this._passwordLabel.TabIndex = 10;
            this._passwordLabel.Text = "Password:";
            this._passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SambaForm
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(789, 207);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = global::BUtil.Configurator.Icons.BUtilIcon;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SambaForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Samba";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.TextBox _shareNameTextBox;
		private System.Windows.Forms.Label _shareNameLabel;
		private System.Windows.Forms.TextBox _hostTextBox;
		private System.Windows.Forms.Label _hostLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label _passwordLabel;
        private System.Windows.Forms.Label _userLabel;
        private System.Windows.Forms.TextBox _passwordTextBox;
        private System.Windows.Forms.TextBox _userTextBox;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
    }
}
