namespace BUtil.RestorationMaster
{
	partial class RestoreMasterMainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RestoreMasterMainForm));
			this.openImageButton = new System.Windows.Forms.Button();
			this.ofd = new System.Windows.Forms.OpenFileDialog();
			this.closeButton = new System.Windows.Forms.Button();
			this.passwordGroupBox = new System.Windows.Forms.GroupBox();
			this.passwordHintLabel = new System.Windows.Forms.Label();
			this.passwordMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
			this.passwordLabel = new System.Windows.Forms.Label();
			this.continueButton = new System.Windows.Forms.Button();
			this.imageLocationLabel = new System.Windows.Forms.Label();
			this.imageLocationTextBox = new System.Windows.Forms.TextBox();
			this.helpButton = new System.Windows.Forms.Button();
			this.passwordGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// openImageButton
			// 
			this.openImageButton.AutoSize = true;
			this.openImageButton.BackColor = System.Drawing.SystemColors.Control;
			this.openImageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.openImageButton.Location = new System.Drawing.Point(314, 26);
			this.openImageButton.Name = "openImageButton";
			this.openImageButton.Size = new System.Drawing.Size(121, 23);
			this.openImageButton.TabIndex = 2;
			this.openImageButton.Text = "Open an image...";
			this.openImageButton.UseVisualStyleBackColor = true;
			this.openImageButton.Click += new System.EventHandler(this.openImageButtonClick);
			// 
			// ofd
			// 
			this.ofd.DefaultExt = "butil";
			this.ofd.Filter = "BUTIL images(*.butil)|*.butil";
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.AutoSize = true;
			this.closeButton.Location = new System.Drawing.Point(360, 159);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(74, 23);
			this.closeButton.TabIndex = 5;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.closeButtonClick);
			// 
			// passwordGroupBox
			// 
			this.passwordGroupBox.Controls.Add(this.passwordHintLabel);
			this.passwordGroupBox.Controls.Add(this.passwordMaskedTextBox);
			this.passwordGroupBox.Controls.Add(this.passwordLabel);
			this.passwordGroupBox.Enabled = false;
			this.passwordGroupBox.Location = new System.Drawing.Point(12, 55);
			this.passwordGroupBox.Name = "passwordGroupBox";
			this.passwordGroupBox.Size = new System.Drawing.Size(422, 98);
			this.passwordGroupBox.TabIndex = 7;
			this.passwordGroupBox.TabStop = false;
			this.passwordGroupBox.Text = "Password";
			// 
			// passwordHintLabel
			// 
			this.passwordHintLabel.AutoSize = true;
			this.passwordHintLabel.Location = new System.Drawing.Point(5, 65);
			this.passwordHintLabel.Name = "passwordHintLabel";
			this.passwordHintLabel.Size = new System.Drawing.Size(311, 13);
			this.passwordHintLabel.TabIndex = 2;
			this.passwordHintLabel.Text = "Note: For restoration of data as archives password is not needed";
			// 
			// passwordMaskedTextBox
			// 
			this.passwordMaskedTextBox.Location = new System.Drawing.Point(16, 42);
			this.passwordMaskedTextBox.Name = "passwordMaskedTextBox";
			this.passwordMaskedTextBox.PasswordChar = '*';
			this.passwordMaskedTextBox.Size = new System.Drawing.Size(375, 20);
			this.passwordMaskedTextBox.TabIndex = 1;
			// 
			// passwordLabel
			// 
			this.passwordLabel.AutoSize = true;
			this.passwordLabel.Location = new System.Drawing.Point(16, 21);
			this.passwordLabel.Name = "passwordLabel";
			this.passwordLabel.Size = new System.Drawing.Size(316, 13);
			this.passwordLabel.TabIndex = 0;
			this.passwordLabel.Text = "If your backup is password protected, please type password here:";
			// 
			// continueButton
			// 
			this.continueButton.AutoSize = true;
			this.continueButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.continueButton.Enabled = false;
			this.continueButton.Location = new System.Drawing.Point(286, 159);
			this.continueButton.Name = "continueButton";
			this.continueButton.Size = new System.Drawing.Size(68, 23);
			this.continueButton.TabIndex = 8;
			this.continueButton.Text = "Continue >";
			this.continueButton.UseVisualStyleBackColor = true;
			this.continueButton.Click += new System.EventHandler(this.nextButtonClick);
			// 
			// imageLocationLabel
			// 
			this.imageLocationLabel.AutoSize = true;
			this.imageLocationLabel.Location = new System.Drawing.Point(10, 13);
			this.imageLocationLabel.Name = "imageLocationLabel";
			this.imageLocationLabel.Size = new System.Drawing.Size(79, 13);
			this.imageLocationLabel.TabIndex = 9;
			this.imageLocationLabel.Text = "Image location:";
			// 
			// imageLocationTextBox
			// 
			this.imageLocationTextBox.Location = new System.Drawing.Point(13, 29);
			this.imageLocationTextBox.Name = "imageLocationTextBox";
			this.imageLocationTextBox.ReadOnly = true;
			this.imageLocationTextBox.Size = new System.Drawing.Size(287, 20);
			this.imageLocationTextBox.TabIndex = 10;
			this.imageLocationTextBox.TabStop = false;
			// 
			// helpButton
			// 
			this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.helpButton.Location = new System.Drawing.Point(10, 154);
			this.helpButton.Name = "helpButton";
			this.helpButton.Size = new System.Drawing.Size(33, 33);
			this.helpButton.TabIndex = 11;
			this.helpButton.Text = "?";
			this.helpButton.UseVisualStyleBackColor = true;
			this.helpButton.Click += new System.EventHandler(this.helpButtonClick);
			// 
			// RestoreMasterMainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(447, 195);
			this.Controls.Add(this.helpButton);
			this.Controls.Add(this.imageLocationTextBox);
			this.Controls.Add(this.imageLocationLabel);
			this.Controls.Add(this.continueButton);
			this.Controls.Add(this.passwordGroupBox);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.openImageButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RestoreMasterMainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Restoration master";
			this.passwordGroupBox.ResumeLayout(false);
			this.passwordGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button helpButton;
		private System.Windows.Forms.Button openImageButton;
		private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.GroupBox passwordGroupBox;
		private System.Windows.Forms.MaskedTextBox passwordMaskedTextBox;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.Button continueButton;
		private System.Windows.Forms.Label imageLocationLabel;
		private System.Windows.Forms.TextBox imageLocationTextBox;
		private System.Windows.Forms.Label passwordHintLabel;
	}
}
