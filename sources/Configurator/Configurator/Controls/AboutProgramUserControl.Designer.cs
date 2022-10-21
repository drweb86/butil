
namespace BUtil.Configurator.Controls
{
	partial class AboutProgramUserControl
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
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
			this.visitWebSiteLabel = new System.Windows.Forms.Label();
			this.reportABugLabel = new System.Windows.Forms.Label();
			this.suggestAFeatureLabel = new System.Windows.Forms.Label();
			this.supportLabel = new System.Windows.Forms.Label();
			this.aboutTextBox = new System.Windows.Forms.TextBox();
			this.documentationLabel = new System.Windows.Forms.Label();
			this.checkForUpdatesLabel = new System.Windows.Forms.Label();
			this.sevenZipPictureBox = new System.Windows.Forms.PictureBox();
			this.virtuawinPictureBox = new System.Windows.Forms.PictureBox();
			this.logoPictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.sevenZipPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.virtuawinPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// visitWebSiteLabel
			// 
			this.visitWebSiteLabel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.visitWebSiteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.visitWebSiteLabel.ForeColor = System.Drawing.Color.Blue;
			this.visitWebSiteLabel.Location = new System.Drawing.Point(12, 9);
			this.visitWebSiteLabel.Name = "visitWebSiteLabel";
			this.visitWebSiteLabel.Size = new System.Drawing.Size(183, 14);
			this.visitWebSiteLabel.TabIndex = 0;
			this.visitWebSiteLabel.Text = "Visit project homepage";
			this.visitWebSiteLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.visitWebSiteLabel.Click += new System.EventHandler(this.VisitWebSiteLabelClick);
			// 
			// reportABugLabel
			// 
			this.reportABugLabel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.reportABugLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.reportABugLabel.ForeColor = System.Drawing.Color.Blue;
			this.reportABugLabel.Location = new System.Drawing.Point(12, 54);
			this.reportABugLabel.Name = "reportABugLabel";
			this.reportABugLabel.Size = new System.Drawing.Size(183, 14);
			this.reportABugLabel.TabIndex = 2;
			this.reportABugLabel.Text = "Report a bug";
			this.reportABugLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.reportABugLabel.Click += new System.EventHandler(this.ReportABugLabelClick);
			// 
			// suggestAFeatureLabel
			// 
			this.suggestAFeatureLabel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.suggestAFeatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.suggestAFeatureLabel.ForeColor = System.Drawing.Color.Blue;
			this.suggestAFeatureLabel.Location = new System.Drawing.Point(12, 31);
			this.suggestAFeatureLabel.Name = "suggestAFeatureLabel";
			this.suggestAFeatureLabel.Size = new System.Drawing.Size(183, 14);
			this.suggestAFeatureLabel.TabIndex = 1;
			this.suggestAFeatureLabel.Text = "Suggest a feature";
			this.suggestAFeatureLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.suggestAFeatureLabel.Click += new System.EventHandler(this.SuggestAFeatureLabelClick);
			// 
			// supportLabel
			// 
			this.supportLabel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.supportLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.supportLabel.ForeColor = System.Drawing.Color.Blue;
			this.supportLabel.Location = new System.Drawing.Point(12, 76);
			this.supportLabel.Name = "supportLabel";
			this.supportLabel.Size = new System.Drawing.Size(183, 14);
			this.supportLabel.TabIndex = 3;
			this.supportLabel.Text = "Support";
			this.supportLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.supportLabel.Click += new System.EventHandler(this.SupportLabelClick);
			// 
			// aboutTextBox
			// 
			this.aboutTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.aboutTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.aboutTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.aboutTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.aboutTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.aboutTextBox.Location = new System.Drawing.Point(201, 12);
			this.aboutTextBox.Multiline = true;
			this.aboutTextBox.Name = "aboutTextBox";
			this.aboutTextBox.ReadOnly = true;
			this.aboutTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.aboutTextBox.Size = new System.Drawing.Size(293, 262);
			this.aboutTextBox.TabIndex = 16;
			this.aboutTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// documentationLabel
			// 
			this.documentationLabel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.documentationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.documentationLabel.ForeColor = System.Drawing.Color.Blue;
			this.documentationLabel.Location = new System.Drawing.Point(12, 97);
			this.documentationLabel.Name = "documentationLabel";
			this.documentationLabel.Size = new System.Drawing.Size(183, 14);
			this.documentationLabel.TabIndex = 4;
			this.documentationLabel.Text = "Documentation";
			this.documentationLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.documentationLabel.Click += new System.EventHandler(this.DocumentationLabelClick);
			// 
			// checkForUpdatesLabel
			// 
			this.checkForUpdatesLabel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.checkForUpdatesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.checkForUpdatesLabel.ForeColor = System.Drawing.Color.Blue;
			this.checkForUpdatesLabel.Location = new System.Drawing.Point(12, 119);
			this.checkForUpdatesLabel.Name = "checkForUpdatesLabel";
			this.checkForUpdatesLabel.Size = new System.Drawing.Size(183, 14);
			this.checkForUpdatesLabel.TabIndex = 17;
			this.checkForUpdatesLabel.Text = "Check for updates...";
			this.checkForUpdatesLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.checkForUpdatesLabel.Click += new System.EventHandler(this.CheckForUpdatesLabelClick);
			// 
			// sevenZipPictureBox
			// 
			this.sevenZipPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.sevenZipPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.sevenZipPictureBox.Image = Icons.sevenZipPictureBox_Image;
			this.sevenZipPictureBox.Location = new System.Drawing.Point(12, 246);
			this.sevenZipPictureBox.Name = "sevenZipPictureBox";
			this.sevenZipPictureBox.Size = new System.Drawing.Size(35, 28);
			this.sevenZipPictureBox.TabIndex = 18;
			this.sevenZipPictureBox.TabStop = false;
			this.sevenZipPictureBox.Click += new System.EventHandler(this.SevenZipPictureBoxClick);
			// 
			// virtuawinPictureBox
			// 
			this.virtuawinPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.virtuawinPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.virtuawinPictureBox.Image = Icons.virtuawinPictureBox_Image;
			this.virtuawinPictureBox.Location = new System.Drawing.Point(165, 246);
			this.virtuawinPictureBox.Name = "virtuawinPictureBox";
			this.virtuawinPictureBox.Size = new System.Drawing.Size(35, 28);
			this.virtuawinPictureBox.TabIndex = 23;
			this.virtuawinPictureBox.TabStop = false;
			this.virtuawinPictureBox.Click += new System.EventHandler(this.VirtuawinPictureBoxClick);
			// 
			// logoPictureBox
			// 
			this.logoPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.logoPictureBox.Image = Icons.logoPictureBox_Image;
			this.logoPictureBox.Location = new System.Drawing.Point(55, 140);
			this.logoPictureBox.Name = "logoPictureBox";
			this.logoPictureBox.Size = new System.Drawing.Size(100, 100);
			this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.logoPictureBox.TabIndex = 24;
			this.logoPictureBox.TabStop = false;
			this.logoPictureBox.Click += new System.EventHandler(this.LogoPictureBoxClick);
			// 
			// AboutProgramUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.logoPictureBox);
			this.Controls.Add(this.virtuawinPictureBox);
			this.Controls.Add(this.sevenZipPictureBox);
			this.Controls.Add(this.checkForUpdatesLabel);
			this.Controls.Add(this.documentationLabel);
			this.Controls.Add(this.visitWebSiteLabel);
			this.Controls.Add(this.reportABugLabel);
			this.Controls.Add(this.suggestAFeatureLabel);
			this.Controls.Add(this.supportLabel);
			this.Controls.Add(this.aboutTextBox);
			this.MinimumSize = new System.Drawing.Size(506, 286);
			this.Name = "AboutProgramUserControl";
			this.Size = new System.Drawing.Size(506, 286);
			((System.ComponentModel.ISupportInitialize)(this.sevenZipPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.virtuawinPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.PictureBox logoPictureBox;
		private System.Windows.Forms.PictureBox virtuawinPictureBox;
		private System.Windows.Forms.PictureBox sevenZipPictureBox;
		private System.Windows.Forms.Label checkForUpdatesLabel;
		private System.Windows.Forms.Label documentationLabel;
		private System.Windows.Forms.TextBox aboutTextBox;
		private System.Windows.Forms.Label supportLabel;
		private System.Windows.Forms.Label suggestAFeatureLabel;
		private System.Windows.Forms.Label reportABugLabel;
		private System.Windows.Forms.Label visitWebSiteLabel;
	}
}
