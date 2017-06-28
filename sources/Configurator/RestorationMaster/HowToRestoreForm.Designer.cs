namespace BUtil.RestorationMaster
{
	partial class HowToRestoreForm
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
            this.components = new System.ComponentModel.Container();
            this.restoreButton = new System.Windows.Forms.Button();
            this.HowToRestoregroupBox = new System.Windows.Forms.GroupBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.recoverItemTextBox = new System.Windows.Forms.TextBox();
            this.choose7zipArchiveDestinationLocationButton = new System.Windows.Forms.Button();
            this.sevenZipDestinationArchiveLocationTextBox = new System.Windows.Forms.TextBox();
            this.specifiedFolderTextBox = new System.Windows.Forms.TextBox();
            this.chooseFolderButton = new System.Windows.Forms.Button();
            this.toFolderRadioButton = new System.Windows.Forms.RadioButton();
            this.toOriginalLocationRadioButton = new System.Windows.Forms.RadioButton();
            this.to7zipArchiveRadioButton = new System.Windows.Forms.RadioButton();
            this.skipButton = new System.Windows.Forms.Button();
            this.save7zipArchiveDialog = new System.Windows.Forms.SaveFileDialog();
            this.fbDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.helpToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.restorationBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.restorationProgressBar = new System.Windows.Forms.ProgressBar();
            this.HowToRestoregroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // restoreButton
            // 
            this.restoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.restoreButton.AutoSize = true;
            this.restoreButton.Location = new System.Drawing.Point(311, 194);
            this.restoreButton.Name = "restoreButton";
            this.restoreButton.Size = new System.Drawing.Size(75, 24);
            this.restoreButton.TabIndex = 9;
            this.restoreButton.Text = "Restore";
            this.restoreButton.UseVisualStyleBackColor = true;
            this.restoreButton.Click += new System.EventHandler(this.restoreButtonClick);
            // 
            // HowToRestoregroupBox
            // 
            this.HowToRestoregroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.HowToRestoregroupBox.Controls.Add(this.pictureBox);
            this.HowToRestoregroupBox.Controls.Add(this.recoverItemTextBox);
            this.HowToRestoregroupBox.Controls.Add(this.choose7zipArchiveDestinationLocationButton);
            this.HowToRestoregroupBox.Controls.Add(this.sevenZipDestinationArchiveLocationTextBox);
            this.HowToRestoregroupBox.Controls.Add(this.specifiedFolderTextBox);
            this.HowToRestoregroupBox.Controls.Add(this.chooseFolderButton);
            this.HowToRestoregroupBox.Controls.Add(this.toFolderRadioButton);
            this.HowToRestoregroupBox.Controls.Add(this.toOriginalLocationRadioButton);
            this.HowToRestoregroupBox.Controls.Add(this.to7zipArchiveRadioButton);
            this.HowToRestoregroupBox.Location = new System.Drawing.Point(12, 12);
            this.HowToRestoregroupBox.Name = "HowToRestoregroupBox";
            this.HowToRestoregroupBox.Size = new System.Drawing.Size(455, 176);
            this.HowToRestoregroupBox.TabIndex = 0;
            this.HowToRestoregroupBox.TabStop = false;
            this.HowToRestoregroupBox.Text = "How to restore your information?";
            // 
            // pictureBox
            // 
            this.pictureBox.BackgroundImage = global::BUtil.Configurator.Icons.folder_48;
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox.Location = new System.Drawing.Point(16, 133);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(38, 37);
            this.pictureBox.TabIndex = 9;
            this.pictureBox.TabStop = false;
            // 
            // recoverItemTextBox
            // 
            this.recoverItemTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.recoverItemTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.recoverItemTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.recoverItemTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.recoverItemTextBox.ForeColor = System.Drawing.Color.ForestGreen;
            this.recoverItemTextBox.Location = new System.Drawing.Point(56, 143);
            this.recoverItemTextBox.Name = "recoverItemTextBox";
            this.recoverItemTextBox.Size = new System.Drawing.Size(393, 15);
            this.recoverItemTextBox.TabIndex = 8;
            this.recoverItemTextBox.TabStop = false;
            this.recoverItemTextBox.Text = "<File name>";
            // 
            // choose7zipArchiveDestinationLocationButton
            // 
            this.choose7zipArchiveDestinationLocationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.choose7zipArchiveDestinationLocationButton.Location = new System.Drawing.Point(410, 89);
            this.choose7zipArchiveDestinationLocationButton.Name = "choose7zipArchiveDestinationLocationButton";
            this.choose7zipArchiveDestinationLocationButton.Size = new System.Drawing.Size(39, 23);
            this.choose7zipArchiveDestinationLocationButton.TabIndex = 6;
            this.choose7zipArchiveDestinationLocationButton.Text = "...";
            this.choose7zipArchiveDestinationLocationButton.UseVisualStyleBackColor = true;
            this.choose7zipArchiveDestinationLocationButton.Click += new System.EventHandler(this.chooseTarget7zipArchiveButtonClick);
            // 
            // sevenZipDestinationArchiveLocationTextBox
            // 
            this.sevenZipDestinationArchiveLocationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sevenZipDestinationArchiveLocationTextBox.Location = new System.Drawing.Point(16, 91);
            this.sevenZipDestinationArchiveLocationTextBox.Name = "sevenZipDestinationArchiveLocationTextBox";
            this.sevenZipDestinationArchiveLocationTextBox.ReadOnly = true;
            this.sevenZipDestinationArchiveLocationTextBox.Size = new System.Drawing.Size(388, 20);
            this.sevenZipDestinationArchiveLocationTextBox.TabIndex = 5;
            // 
            // specifiedFolderTextBox
            // 
            this.specifiedFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.specifiedFolderTextBox.Location = new System.Drawing.Point(16, 42);
            this.specifiedFolderTextBox.Name = "specifiedFolderTextBox";
            this.specifiedFolderTextBox.ReadOnly = true;
            this.specifiedFolderTextBox.Size = new System.Drawing.Size(388, 20);
            this.specifiedFolderTextBox.TabIndex = 2;
            // 
            // chooseFolderButton
            // 
            this.chooseFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chooseFolderButton.Location = new System.Drawing.Point(410, 38);
            this.chooseFolderButton.Name = "chooseFolderButton";
            this.chooseFolderButton.Size = new System.Drawing.Size(39, 24);
            this.chooseFolderButton.TabIndex = 3;
            this.chooseFolderButton.Text = "...";
            this.chooseFolderButton.UseVisualStyleBackColor = true;
            this.chooseFolderButton.Click += new System.EventHandler(this.chooseTargetFolderButtonClick);
            // 
            // toFolderRadioButton
            // 
            this.toFolderRadioButton.AutoSize = true;
            this.toFolderRadioButton.Checked = true;
            this.toFolderRadioButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.toFolderRadioButton.Location = new System.Drawing.Point(6, 19);
            this.toFolderRadioButton.Name = "toFolderRadioButton";
            this.toFolderRadioButton.Size = new System.Drawing.Size(112, 17);
            this.toFolderRadioButton.TabIndex = 1;
            this.toFolderRadioButton.TabStop = true;
            this.toFolderRadioButton.Text = "To specified folder";
            this.toFolderRadioButton.UseVisualStyleBackColor = true;
            this.toFolderRadioButton.CheckedChanged += new System.EventHandler(this.toFolderRadioButtonCheckedChanged);
            // 
            // toOriginalLocationRadioButton
            // 
            this.toOriginalLocationRadioButton.AutoSize = true;
            this.toOriginalLocationRadioButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.toOriginalLocationRadioButton.Location = new System.Drawing.Point(6, 117);
            this.toOriginalLocationRadioButton.Name = "toOriginalLocationRadioButton";
            this.toOriginalLocationRadioButton.Size = new System.Drawing.Size(114, 17);
            this.toOriginalLocationRadioButton.TabIndex = 7;
            this.toOriginalLocationRadioButton.Text = "To original location";
            this.toOriginalLocationRadioButton.UseVisualStyleBackColor = true;
            this.toOriginalLocationRadioButton.CheckedChanged += new System.EventHandler(this.ToOriginalLocationRadioButtonCheckedChanged);
            // 
            // to7zipArchiveRadioButton
            // 
            this.to7zipArchiveRadioButton.AutoSize = true;
            this.to7zipArchiveRadioButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.to7zipArchiveRadioButton.Location = new System.Drawing.Point(6, 68);
            this.to7zipArchiveRadioButton.Name = "to7zipArchiveRadioButton";
            this.to7zipArchiveRadioButton.Size = new System.Drawing.Size(139, 17);
            this.to7zipArchiveRadioButton.TabIndex = 4;
            this.to7zipArchiveRadioButton.Text = "Restore as 7-zip archive";
            this.to7zipArchiveRadioButton.UseVisualStyleBackColor = true;
            this.to7zipArchiveRadioButton.CheckedChanged += new System.EventHandler(this.to7zipArchiveRadioButtonCheckedChanged);
            // 
            // skipButton
            // 
            this.skipButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skipButton.AutoSize = true;
            this.skipButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.skipButton.Location = new System.Drawing.Point(392, 194);
            this.skipButton.Name = "skipButton";
            this.skipButton.Size = new System.Drawing.Size(75, 24);
            this.skipButton.TabIndex = 10;
            this.skipButton.Text = "Skip";
            this.skipButton.UseVisualStyleBackColor = true;
            // 
            // save7zipArchiveDialog
            // 
            this.save7zipArchiveDialog.DefaultExt = "7z";
            this.save7zipArchiveDialog.Filter = "7-zip|*.7z";
            // 
            // fbDialog
            // 
            this.fbDialog.Description = "Choose destination location";
            // 
            // restorationBackgroundWorker
            // 
            this.restorationBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.RestorationBackgroundWorkerDoWork);
            this.restorationBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.RestorationBackgroundWorkerRunWorkerCompleted);
            // 
            // restorationProgressBar
            // 
            this.restorationProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.restorationProgressBar.Location = new System.Drawing.Point(12, 195);
            this.restorationProgressBar.Name = "restorationProgressBar";
            this.restorationProgressBar.Size = new System.Drawing.Size(293, 23);
            this.restorationProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.restorationProgressBar.TabIndex = 11;
            this.restorationProgressBar.Visible = false;
            // 
            // HowToRestoreForm
            // 
            this.AcceptButton = this.restoreButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.skipButton;
            this.ClientSize = new System.Drawing.Size(479, 230);
            this.Controls.Add(this.restorationProgressBar);
            this.Controls.Add(this.skipButton);
            this.Controls.Add(this.HowToRestoregroupBox);
            this.Controls.Add(this.restoreButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HowToRestoreForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "{Item location} - Restoration";
            this.HowToRestoregroupBox.ResumeLayout(false);
            this.HowToRestoregroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ProgressBar restorationProgressBar;
		private System.ComponentModel.BackgroundWorker restorationBackgroundWorker;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.SaveFileDialog save7zipArchiveDialog;
		private System.Windows.Forms.TextBox sevenZipDestinationArchiveLocationTextBox;
		private System.Windows.Forms.Button choose7zipArchiveDestinationLocationButton;
		private System.Windows.Forms.FolderBrowserDialog fbDialog;
		private System.Windows.Forms.RadioButton toFolderRadioButton;
        private System.Windows.Forms.RadioButton toOriginalLocationRadioButton;
		private System.Windows.Forms.Button skipButton;
        private System.Windows.Forms.RadioButton to7zipArchiveRadioButton;
		private System.Windows.Forms.GroupBox HowToRestoregroupBox;
		private System.Windows.Forms.Button restoreButton;
		private System.Windows.Forms.TextBox recoverItemTextBox;
        private System.Windows.Forms.ToolTip helpToolTip;
        private System.Windows.Forms.TextBox specifiedFolderTextBox;
        private System.Windows.Forms.Button chooseFolderButton;
        private System.Windows.Forms.FolderBrowserDialog fbd;
	}
}
