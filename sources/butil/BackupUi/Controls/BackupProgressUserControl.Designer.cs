
namespace BUtil.BackupUiMaster.Controls
{
	partial class BackupProgressUserControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupProgressUserControl));
            this.elapsedLabel = new System.Windows.Forms.Label();
            this.passedLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.clockTimer = new System.Windows.Forms.Timer(this.components);
            this.backupIconPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.backupIconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // elapsedLabel
            // 
            this.elapsedLabel.AutoSize = true;
            this.elapsedLabel.Location = new System.Drawing.Point(150, 78);
            this.elapsedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.elapsedLabel.Name = "elapsedLabel";
            this.elapsedLabel.Size = new System.Drawing.Size(13, 15);
            this.elapsedLabel.TabIndex = 25;
            this.elapsedLabel.Text = "0";
            // 
            // passedLabel
            // 
            this.passedLabel.AutoSize = true;
            this.passedLabel.Location = new System.Drawing.Point(69, 78);
            this.passedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.passedLabel.Name = "passedLabel";
            this.passedLabel.Size = new System.Drawing.Size(50, 15);
            this.passedLabel.TabIndex = 24;
            this.passedLabel.Text = "Elapsed:";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(69, 44);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(516, 27);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 23;
            // 
            // clockTimer
            // 
            this.clockTimer.Enabled = true;
            this.clockTimer.Interval = 1000;
            this.clockTimer.Tick += new System.EventHandler(this.timerTick);
            // 
            // backupIconPictureBox
            // 
            this.backupIconPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("backupIconPictureBox.Image")));
            this.backupIconPictureBox.Location = new System.Drawing.Point(4, 42);
            this.backupIconPictureBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.backupIconPictureBox.Name = "backupIconPictureBox";
            this.backupIconPictureBox.Size = new System.Drawing.Size(57, 51);
            this.backupIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.backupIconPictureBox.TabIndex = 27;
            this.backupIconPictureBox.TabStop = false;
            // 
            // BackupProgressUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.elapsedLabel);
            this.Controls.Add(this.backupIconPictureBox);
            this.Controls.Add(this.passedLabel);
            this.Controls.Add(this.progressBar);
            this.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.MinimumSize = new System.Drawing.Size(586, 104);
            this.Name = "BackupProgressUserControl";
            this.Size = new System.Drawing.Size(588, 104);
            this.Title = "Backup is in a progress...";
            this.Controls.SetChildIndex(this.progressBar, 0);
            this.Controls.SetChildIndex(this.passedLabel, 0);
            this.Controls.SetChildIndex(this.backupIconPictureBox, 0);
            this.Controls.SetChildIndex(this.elapsedLabel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.backupIconPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Timer clockTimer;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label passedLabel;
		private System.Windows.Forms.PictureBox backupIconPictureBox;
		private System.Windows.Forms.Label elapsedLabel;
	}
}
