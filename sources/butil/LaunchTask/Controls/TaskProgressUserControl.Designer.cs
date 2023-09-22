
namespace BUtil.BackupUiMaster.Controls
{
    partial class TaskProgressUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskProgressUserControl));
            _headerTitleLabel = new System.Windows.Forms.Label();
            clockTimer = new System.Windows.Forms.Timer(components);
            _mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _elapsedTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            elapsedLabel = new System.Windows.Forms.Label();
            passedLabel = new System.Windows.Forms.Label();
            progressBar = new System.Windows.Forms.ProgressBar();
            _progressControlTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            backupIconPictureBox = new System.Windows.Forms.PictureBox();
            _controlTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _mainTableLayoutPanel.SuspendLayout();
            _elapsedTableLayoutPanel.SuspendLayout();
            _progressControlTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)backupIconPictureBox).BeginInit();
            _controlTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _headerTitleLabel
            // 
            _headerTitleLabel.AutoSize = true;
            _headerTitleLabel.BackColor = System.Drawing.Color.DodgerBlue;
            _headerTitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _headerTitleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _headerTitleLabel.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            _headerTitleLabel.ForeColor = System.Drawing.Color.White;
            _headerTitleLabel.Location = new System.Drawing.Point(4, 0);
            _headerTitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _headerTitleLabel.Name = "_headerTitleLabel";
            _headerTitleLabel.Size = new System.Drawing.Size(1054, 49);
            _headerTitleLabel.TabIndex = 1;
            _headerTitleLabel.Text = "Title";
            _headerTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clockTimer
            // 
            clockTimer.Interval = 1000;
            clockTimer.Tick += timerTick;
            // 
            // _mainTableLayoutPanel
            // 
            _mainTableLayoutPanel.ColumnCount = 2;
            _mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _mainTableLayoutPanel.Controls.Add(backupIconPictureBox, 0, 0);
            _mainTableLayoutPanel.Controls.Add(_progressControlTableLayoutPanel, 1, 0);
            _mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _mainTableLayoutPanel.Location = new System.Drawing.Point(3, 52);
            _mainTableLayoutPanel.Name = "_mainTableLayoutPanel";
            _mainTableLayoutPanel.RowCount = 1;
            _mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _mainTableLayoutPanel.Size = new System.Drawing.Size(1056, 343);
            _mainTableLayoutPanel.TabIndex = 30;
            // 
            // _elapsedTableLayoutPanel
            // 
            _elapsedTableLayoutPanel.AutoSize = true;
            _elapsedTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _elapsedTableLayoutPanel.ColumnCount = 2;
            _elapsedTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.139534F));
            _elapsedTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.860466F));
            _elapsedTableLayoutPanel.Controls.Add(passedLabel, 0, 0);
            _elapsedTableLayoutPanel.Controls.Add(elapsedLabel, 1, 0);
            _elapsedTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _elapsedTableLayoutPanel.Location = new System.Drawing.Point(3, 64);
            _elapsedTableLayoutPanel.Name = "_elapsedTableLayoutPanel";
            _elapsedTableLayoutPanel.RowCount = 1;
            _elapsedTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            _elapsedTableLayoutPanel.Size = new System.Drawing.Size(951, 270);
            _elapsedTableLayoutPanel.TabIndex = 28;
            // 
            // elapsedLabel
            // 
            elapsedLabel.AutoSize = true;
            elapsedLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            elapsedLabel.Location = new System.Drawing.Point(416, 0);
            elapsedLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            elapsedLabel.Name = "elapsedLabel";
            elapsedLabel.Size = new System.Drawing.Size(529, 270);
            elapsedLabel.TabIndex = 25;
            elapsedLabel.Text = "0";
            // 
            // passedLabel
            // 
            passedLabel.AutoSize = true;
            passedLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            passedLabel.Location = new System.Drawing.Point(6, 0);
            passedLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            passedLabel.Name = "passedLabel";
            passedLabel.Size = new System.Drawing.Size(398, 270);
            passedLabel.TabIndex = 24;
            passedLabel.Text = "Elapsed:";
            // 
            // progressBar
            // 
            progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            progressBar.Location = new System.Drawing.Point(6, 5);
            progressBar.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            progressBar.Maximum = 1;
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(945, 51);
            progressBar.TabIndex = 23;
            // 
            // _progressControlTableLayoutPanel
            // 
            _progressControlTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _progressControlTableLayoutPanel.ColumnCount = 1;
            _progressControlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _progressControlTableLayoutPanel.Controls.Add(progressBar, 0, 0);
            _progressControlTableLayoutPanel.Controls.Add(_elapsedTableLayoutPanel, 0, 1);
            _progressControlTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _progressControlTableLayoutPanel.Location = new System.Drawing.Point(96, 3);
            _progressControlTableLayoutPanel.Name = "_progressControlTableLayoutPanel";
            _progressControlTableLayoutPanel.RowCount = 2;
            _progressControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _progressControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _progressControlTableLayoutPanel.Size = new System.Drawing.Size(957, 337);
            _progressControlTableLayoutPanel.TabIndex = 29;
            // 
            // backupIconPictureBox
            // 
            backupIconPictureBox.Image = (System.Drawing.Image)resources.GetObject("backupIconPictureBox.Image");
            backupIconPictureBox.Location = new System.Drawing.Point(6, 5);
            backupIconPictureBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            backupIconPictureBox.Name = "backupIconPictureBox";
            backupIconPictureBox.Size = new System.Drawing.Size(81, 85);
            backupIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            backupIconPictureBox.TabIndex = 27;
            backupIconPictureBox.TabStop = false;
            // 
            // _controlTableLayoutPanel
            // 
            _controlTableLayoutPanel.ColumnCount = 1;
            _controlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _controlTableLayoutPanel.Controls.Add(_headerTitleLabel, 0, 0);
            _controlTableLayoutPanel.Controls.Add(_mainTableLayoutPanel, 0, 1);
            _controlTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _controlTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _controlTableLayoutPanel.Name = "_controlTableLayoutPanel";
            _controlTableLayoutPanel.RowCount = 2;
            _controlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _controlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _controlTableLayoutPanel.Size = new System.Drawing.Size(1062, 398);
            _controlTableLayoutPanel.TabIndex = 31;
            // 
            // TaskProgressUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Window;
            Controls.Add(_controlTableLayoutPanel);
            Margin = new System.Windows.Forms.Padding(9, 5, 9, 5);
            MinimumSize = new System.Drawing.Size(837, 173);
            Name = "TaskProgressUserControl";
            Size = new System.Drawing.Size(1062, 398);
            _mainTableLayoutPanel.ResumeLayout(false);
            _elapsedTableLayoutPanel.ResumeLayout(false);
            _elapsedTableLayoutPanel.PerformLayout();
            _progressControlTableLayoutPanel.ResumeLayout(false);
            _progressControlTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)backupIconPictureBox).EndInit();
            _controlTableLayoutPanel.ResumeLayout(false);
            _controlTableLayoutPanel.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Timer clockTimer;
        private System.Windows.Forms.TableLayoutPanel _mainTableLayoutPanel;
        private System.Windows.Forms.Label _headerTitleLabel;
        private System.Windows.Forms.PictureBox backupIconPictureBox;
        private System.Windows.Forms.TableLayoutPanel _progressControlTableLayoutPanel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TableLayoutPanel _elapsedTableLayoutPanel;
        private System.Windows.Forms.Label passedLabel;
        private System.Windows.Forms.Label elapsedLabel;
        private System.Windows.Forms.TableLayoutPanel _controlTableLayoutPanel;
    }
}
