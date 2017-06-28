
namespace BUtil.Configurator.Controls
{
	partial class OtherOptionsUserControl
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
            this.priorityComboBox = new System.Windows.Forms.ComboBox();
            this.chooseBackUpPriorityLabel = new System.Windows.Forms.Label();
            this.putOffBackupTillLabel = new System.Windows.Forms.Label();
            this.cpuLoadingNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.amountOf7ZipProcessesToRunSynchronouslyNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.amountOf7ZipProcessesToRunSynchronouslyLabel = new System.Windows.Forms.Label();
            this.amountOfStoragesToProcessSynchronouslyNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.amountOfStoragesToProcessSynchronouslyLabel = new System.Windows.Forms.Label();
            this.minimizeUsageOfSystemResourcesCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.cpuLoadingNumericUpDown)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amountOf7ZipProcessesToRunSynchronouslyNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.amountOfStoragesToProcessSynchronouslyNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // priorityComboBox
            // 
            this.priorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priorityComboBox.FormattingEnabled = true;
            this.priorityComboBox.Items.AddRange(new object[] {
            "Low",
            "Below normal",
            "Normal",
            "Above normal"});
            this.priorityComboBox.Location = new System.Drawing.Point(308, 55);
            this.priorityComboBox.Name = "priorityComboBox";
            this.priorityComboBox.Size = new System.Drawing.Size(119, 21);
            this.priorityComboBox.TabIndex = 3;
            // 
            // chooseBackUpPriorityLabel
            // 
            this.chooseBackUpPriorityLabel.AutoSize = true;
            this.chooseBackUpPriorityLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chooseBackUpPriorityLabel.Location = new System.Drawing.Point(3, 52);
            this.chooseBackUpPriorityLabel.Name = "chooseBackUpPriorityLabel";
            this.chooseBackUpPriorityLabel.Size = new System.Drawing.Size(299, 27);
            this.chooseBackUpPriorityLabel.TabIndex = 22;
            this.chooseBackUpPriorityLabel.Text = "Choose backup priority:";
            this.chooseBackUpPriorityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // putOffBackupTillLabel
            // 
            this.putOffBackupTillLabel.AutoSize = true;
            this.putOffBackupTillLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.putOffBackupTillLabel.Location = new System.Drawing.Point(3, 79);
            this.putOffBackupTillLabel.Name = "putOffBackupTillLabel";
            this.putOffBackupTillLabel.Size = new System.Drawing.Size(299, 26);
            this.putOffBackupTillLabel.TabIndex = 23;
            this.putOffBackupTillLabel.Text = "Put off making backup till processor\'s  loading will be less then";
            this.putOffBackupTillLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cpuLoadingNumericUpDown
            // 
            this.cpuLoadingNumericUpDown.Location = new System.Drawing.Point(308, 82);
            this.cpuLoadingNumericUpDown.Maximum = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.cpuLoadingNumericUpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.cpuLoadingNumericUpDown.Name = "cpuLoadingNumericUpDown";
            this.cpuLoadingNumericUpDown.ReadOnly = true;
            this.cpuLoadingNumericUpDown.Size = new System.Drawing.Size(119, 20);
            this.cpuLoadingNumericUpDown.TabIndex = 4;
            this.cpuLoadingNumericUpDown.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.amountOf7ZipProcessesToRunSynchronouslyNumericUpDown, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.amountOf7ZipProcessesToRunSynchronouslyLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.amountOfStoragesToProcessSynchronouslyNumericUpDown, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.amountOfStoragesToProcessSynchronouslyLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cpuLoadingNumericUpDown, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.putOffBackupTillLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.priorityComboBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.chooseBackUpPriorityLabel, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 8);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(430, 105);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // amountOf7ZipProcessesToRunSynchronouslyNumericUpDown
            // 
            this.amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.Location = new System.Drawing.Point(308, 29);
            this.amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.Name = "amountOf7ZipProcessesToRunSynchronouslyNumericUpDown";
            this.amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.Size = new System.Drawing.Size(119, 20);
            this.amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.TabIndex = 2;
            // 
            // amountOf7ZipProcessesToRunSynchronouslyLabel
            // 
            this.amountOf7ZipProcessesToRunSynchronouslyLabel.AutoSize = true;
            this.amountOf7ZipProcessesToRunSynchronouslyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.amountOf7ZipProcessesToRunSynchronouslyLabel.Location = new System.Drawing.Point(3, 26);
            this.amountOf7ZipProcessesToRunSynchronouslyLabel.Name = "amountOf7ZipProcessesToRunSynchronouslyLabel";
            this.amountOf7ZipProcessesToRunSynchronouslyLabel.Size = new System.Drawing.Size(299, 26);
            this.amountOf7ZipProcessesToRunSynchronouslyLabel.TabIndex = 26;
            this.amountOf7ZipProcessesToRunSynchronouslyLabel.Text = "Amount of 7-zip processes to run synchronously:";
            this.amountOf7ZipProcessesToRunSynchronouslyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // amountOfStoragesToProcessSynchronouslyNumericUpDown
            // 
            this.amountOfStoragesToProcessSynchronouslyNumericUpDown.Location = new System.Drawing.Point(308, 3);
            this.amountOfStoragesToProcessSynchronouslyNumericUpDown.Name = "amountOfStoragesToProcessSynchronouslyNumericUpDown";
            this.amountOfStoragesToProcessSynchronouslyNumericUpDown.Size = new System.Drawing.Size(119, 20);
            this.amountOfStoragesToProcessSynchronouslyNumericUpDown.TabIndex = 1;
            // 
            // amountOfStoragesToProcessSynchronouslyLabel
            // 
            this.amountOfStoragesToProcessSynchronouslyLabel.AutoSize = true;
            this.amountOfStoragesToProcessSynchronouslyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.amountOfStoragesToProcessSynchronouslyLabel.Location = new System.Drawing.Point(3, 0);
            this.amountOfStoragesToProcessSynchronouslyLabel.Name = "amountOfStoragesToProcessSynchronouslyLabel";
            this.amountOfStoragesToProcessSynchronouslyLabel.Size = new System.Drawing.Size(299, 26);
            this.amountOfStoragesToProcessSynchronouslyLabel.TabIndex = 0;
            this.amountOfStoragesToProcessSynchronouslyLabel.Text = "Amount of storages to process synchronously:";
            this.amountOfStoragesToProcessSynchronouslyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // minimizeUsageOfSystemResourcesCheckBox
            // 
            this.minimizeUsageOfSystemResourcesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizeUsageOfSystemResourcesCheckBox.AutoSize = true;
            this.minimizeUsageOfSystemResourcesCheckBox.Location = new System.Drawing.Point(9, 128);
            this.minimizeUsageOfSystemResourcesCheckBox.Name = "minimizeUsageOfSystemResourcesCheckBox";
            this.minimizeUsageOfSystemResourcesCheckBox.Size = new System.Drawing.Size(209, 17);
            this.minimizeUsageOfSystemResourcesCheckBox.TabIndex = 25;
            this.minimizeUsageOfSystemResourcesCheckBox.Text = "Run backup scheduler in hidden mode";
            this.minimizeUsageOfSystemResourcesCheckBox.UseVisualStyleBackColor = true;
            // 
            // OtherOptionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.minimizeUsageOfSystemResourcesCheckBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(491, 158);
            this.Name = "OtherOptionsUserControl";
            this.Size = new System.Drawing.Size(497, 164);
            ((System.ComponentModel.ISupportInitialize)(this.cpuLoadingNumericUpDown)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amountOf7ZipProcessesToRunSynchronouslyNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.amountOfStoragesToProcessSynchronouslyNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		private System.Windows.Forms.Label amountOf7ZipProcessesToRunSynchronouslyLabel;
		private System.Windows.Forms.NumericUpDown amountOf7ZipProcessesToRunSynchronouslyNumericUpDown;
		private System.Windows.Forms.NumericUpDown amountOfStoragesToProcessSynchronouslyNumericUpDown;
		private System.Windows.Forms.Label amountOfStoragesToProcessSynchronouslyLabel;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.NumericUpDown cpuLoadingNumericUpDown;
		private System.Windows.Forms.Label putOffBackupTillLabel;
		private System.Windows.Forms.Label chooseBackUpPriorityLabel;
		private System.Windows.Forms.ComboBox priorityComboBox;
        private System.Windows.Forms.CheckBox minimizeUsageOfSystemResourcesCheckBox;
	}
}
