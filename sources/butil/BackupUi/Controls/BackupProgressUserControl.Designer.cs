
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
            if (disposing)
            {
                if (components != null)
                {
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupProgressUserControl));
            elapsedLabel = new System.Windows.Forms.Label();
            passedLabel = new System.Windows.Forms.Label();
            progressBar = new System.Windows.Forms.ProgressBar();
            clockTimer = new System.Windows.Forms.Timer(components);
            backupIconPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)backupIconPictureBox).BeginInit();
            SuspendLayout();
            // 
            // elapsedLabel
            // 
            elapsedLabel.AutoSize = true;
            elapsedLabel.Location = new System.Drawing.Point(150, 78);
            elapsedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            elapsedLabel.Name = "elapsedLabel";
            elapsedLabel.Size = new System.Drawing.Size(13, 15);
            elapsedLabel.TabIndex = 25;
            elapsedLabel.Text = "0";
            // 
            // passedLabel
            // 
            passedLabel.AutoSize = true;
            passedLabel.Location = new System.Drawing.Point(69, 78);
            passedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            passedLabel.Name = "passedLabel";
            passedLabel.Size = new System.Drawing.Size(50, 15);
            passedLabel.TabIndex = 24;
            passedLabel.Text = "Elapsed:";
            // 
            // progressBar
            // 
            progressBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            progressBar.Location = new System.Drawing.Point(69, 44);
            progressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            progressBar.Maximum = 10;
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(516, 27);
            progressBar.TabIndex = 23;
            progressBar.Value = 10;
            // 
            // clockTimer
            // 
            clockTimer.Enabled = true;
            clockTimer.Interval = 1000;
            clockTimer.Tick += timerTick;
            // 
            // backupIconPictureBox
            // 
            backupIconPictureBox.Image = (System.Drawing.Image)resources.GetObject("backupIconPictureBox.Image");
            backupIconPictureBox.Location = new System.Drawing.Point(4, 42);
            backupIconPictureBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            backupIconPictureBox.Name = "backupIconPictureBox";
            backupIconPictureBox.Size = new System.Drawing.Size(57, 51);
            backupIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            backupIconPictureBox.TabIndex = 27;
            backupIconPictureBox.TabStop = false;
            // 
            // BackupProgressUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(elapsedLabel);
            Controls.Add(backupIconPictureBox);
            Controls.Add(passedLabel);
            Controls.Add(progressBar);
            Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            MinimumSize = new System.Drawing.Size(586, 104);
            Name = "BackupProgressUserControl";
            Size = new System.Drawing.Size(588, 104);
            Title = "Backup is in a progress...";
            Controls.SetChildIndex(progressBar, 0);
            Controls.SetChildIndex(passedLabel, 0);
            Controls.SetChildIndex(backupIconPictureBox, 0);
            Controls.SetChildIndex(elapsedLabel, 0);
            ((System.ComponentModel.ISupportInitialize)backupIconPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Timer clockTimer;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label passedLabel;
        private System.Windows.Forms.PictureBox backupIconPictureBox;
        private System.Windows.Forms.Label elapsedLabel;
    }
}
