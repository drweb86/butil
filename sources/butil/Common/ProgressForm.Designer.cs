namespace BUtil.RestorationMaster
{
    partial class ProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            _progressBar = new System.Windows.Forms.ProgressBar();
            _backgroundWorker = new System.ComponentModel.BackgroundWorker();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // _progressBar
            // 
            _progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            _progressBar.Location = new System.Drawing.Point(20, 21);
            _progressBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            _progressBar.MaximumSize = new System.Drawing.Size(940, 80);
            _progressBar.MinimumSize = new System.Drawing.Size(940, 80);
            _progressBar.Name = "_progressBar";
            _progressBar.Size = new System.Drawing.Size(940, 80);
            _progressBar.TabIndex = 0;
            _progressBar.Value = 1;
            // 
            // _backgroundWorker
            // 
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.DoWork += OnDoWork;
            _backgroundWorker.ProgressChanged += OnProgressChanged;
            _backgroundWorker.RunWorkerCompleted += OnWorkCompleted;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(_progressBar, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.MinimumSize = new System.Drawing.Size(940, 80);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(16);
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new System.Drawing.Size(986, 134);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // ProgressForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ClientSize = new System.Drawing.Size(986, 134);
            ControlBox = false;
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            Name = "ProgressForm";
            Text = "In progress";
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ProgressBar _progressBar;
        private System.ComponentModel.BackgroundWorker _backgroundWorker;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
