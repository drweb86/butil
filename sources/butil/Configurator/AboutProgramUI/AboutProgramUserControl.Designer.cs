
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
            this.aboutTextBox = new System.Windows.Forms.TextBox();
            this.documentationLabel = new System.Windows.Forms.Label();
            this.sevenZipPictureBox = new System.Windows.Forms.PictureBox();
            this.virtuawinPictureBox = new System.Windows.Forms.PictureBox();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this._leftToRightTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._leftTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.sevenZipPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.virtuawinPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this._leftToRightTableLayoutPanel.SuspendLayout();
            this._leftTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // visitWebSiteLabel
            // 
            this.visitWebSiteLabel.AutoSize = true;
            this.visitWebSiteLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.visitWebSiteLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visitWebSiteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.visitWebSiteLabel.ForeColor = System.Drawing.Color.Blue;
            this.visitWebSiteLabel.Location = new System.Drawing.Point(4, 0);
            this.visitWebSiteLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.visitWebSiteLabel.Name = "visitWebSiteLabel";
            this.visitWebSiteLabel.Size = new System.Drawing.Size(224, 13);
            this.visitWebSiteLabel.TabIndex = 0;
            this.visitWebSiteLabel.Text = "Visit project homepage";
            this.visitWebSiteLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.visitWebSiteLabel.Click += new System.EventHandler(this.VisitWebSiteLabelClick);
            // 
            // aboutTextBox
            // 
            this.aboutTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.aboutTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.aboutTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.aboutTextBox.Location = new System.Drawing.Point(242, 3);
            this.aboutTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.aboutTextBox.Multiline = true;
            this.aboutTextBox.Name = "aboutTextBox";
            this.aboutTextBox.ReadOnly = true;
            this.aboutTextBox.Size = new System.Drawing.Size(451, 488);
            this.aboutTextBox.TabIndex = 16;
            this.aboutTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // documentationLabel
            // 
            this.documentationLabel.AutoSize = true;
            this.documentationLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.documentationLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.documentationLabel.ForeColor = System.Drawing.Color.Blue;
            this.documentationLabel.Location = new System.Drawing.Point(4, 13);
            this.documentationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.documentationLabel.Name = "documentationLabel";
            this.documentationLabel.Size = new System.Drawing.Size(224, 13);
            this.documentationLabel.TabIndex = 4;
            this.documentationLabel.Text = "Documentation";
            this.documentationLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.documentationLabel.Click += new System.EventHandler(this.DocumentationLabelClick);
            // 
            // sevenZipPictureBox
            // 
            this.sevenZipPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sevenZipPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.sevenZipPictureBox.Image = global::BUtil.Configurator.Icons.sevenZipPictureBox_Image;
            this.sevenZipPictureBox.Location = new System.Drawing.Point(187, 150);
            this.sevenZipPictureBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sevenZipPictureBox.Name = "sevenZipPictureBox";
            this.sevenZipPictureBox.Size = new System.Drawing.Size(41, 32);
            this.sevenZipPictureBox.TabIndex = 18;
            this.sevenZipPictureBox.TabStop = false;
            this.sevenZipPictureBox.Click += new System.EventHandler(this.SevenZipPictureBoxClick);
            // 
            // virtuawinPictureBox
            // 
            this.virtuawinPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.virtuawinPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.virtuawinPictureBox.Image = global::BUtil.Configurator.Icons.virtuawinPictureBox_Image;
            this.virtuawinPictureBox.Location = new System.Drawing.Point(187, 188);
            this.virtuawinPictureBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.virtuawinPictureBox.Name = "virtuawinPictureBox";
            this.virtuawinPictureBox.Size = new System.Drawing.Size(41, 297);
            this.virtuawinPictureBox.TabIndex = 23;
            this.virtuawinPictureBox.TabStop = false;
            this.virtuawinPictureBox.Click += new System.EventHandler(this.VirtuawinPictureBoxClick);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.logoPictureBox.Image = global::BUtil.Configurator.Icons.logoPictureBox_Image;
            this.logoPictureBox.Location = new System.Drawing.Point(111, 29);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(117, 115);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPictureBox.TabIndex = 24;
            this.logoPictureBox.TabStop = false;
            this.logoPictureBox.Click += new System.EventHandler(this.LogoPictureBoxClick);
            // 
            // _leftToRightTableLayoutPanel
            // 
            this._leftToRightTableLayoutPanel.ColumnCount = 2;
            this._leftToRightTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.28981F));
            this._leftToRightTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.71019F));
            this._leftToRightTableLayoutPanel.Controls.Add(this.aboutTextBox, 1, 0);
            this._leftToRightTableLayoutPanel.Controls.Add(this._leftTableLayoutPanel, 0, 0);
            this._leftToRightTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._leftToRightTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._leftToRightTableLayoutPanel.Name = "_leftToRightTableLayoutPanel";
            this._leftToRightTableLayoutPanel.RowCount = 1;
            this._leftToRightTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._leftToRightTableLayoutPanel.Size = new System.Drawing.Size(697, 494);
            this._leftToRightTableLayoutPanel.TabIndex = 25;
            // 
            // _leftTableLayoutPanel
            // 
            this._leftTableLayoutPanel.ColumnCount = 1;
            this._leftTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._leftTableLayoutPanel.Controls.Add(this.visitWebSiteLabel, 0, 0);
            this._leftTableLayoutPanel.Controls.Add(this.documentationLabel, 0, 1);
            this._leftTableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 2);
            this._leftTableLayoutPanel.Controls.Add(this.sevenZipPictureBox, 0, 3);
            this._leftTableLayoutPanel.Controls.Add(this.virtuawinPictureBox, 0, 4);
            this._leftTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._leftTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this._leftTableLayoutPanel.Name = "_leftTableLayoutPanel";
            this._leftTableLayoutPanel.RowCount = 5;
            this._leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._leftTableLayoutPanel.Size = new System.Drawing.Size(232, 488);
            this._leftTableLayoutPanel.TabIndex = 17;
            // 
            // AboutProgramUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._leftToRightTableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MinimumSize = new System.Drawing.Size(590, 330);
            this.Name = "AboutProgramUserControl";
            this.Size = new System.Drawing.Size(697, 494);
            ((System.ComponentModel.ISupportInitialize)(this.sevenZipPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.virtuawinPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this._leftToRightTableLayoutPanel.ResumeLayout(false);
            this._leftToRightTableLayoutPanel.PerformLayout();
            this._leftTableLayoutPanel.ResumeLayout(false);
            this._leftTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.PictureBox logoPictureBox;
		private System.Windows.Forms.PictureBox virtuawinPictureBox;
		private System.Windows.Forms.PictureBox sevenZipPictureBox;
		private System.Windows.Forms.Label documentationLabel;
		private System.Windows.Forms.TextBox aboutTextBox;
		private System.Windows.Forms.Label visitWebSiteLabel;
		private System.Windows.Forms.TableLayoutPanel _leftToRightTableLayoutPanel;
		private System.Windows.Forms.TableLayoutPanel _leftTableLayoutPanel;
	}
}
