
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
            this.putOffBackupTillLabel = new System.Windows.Forms.Label();
            this.cpuLoadingNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._parallelNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._parallelLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cpuLoadingNumericUpDown)).BeginInit();
            this._tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._parallelNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // putOffBackupTillLabel
            // 
            this.putOffBackupTillLabel.AutoSize = true;
            this.putOffBackupTillLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.putOffBackupTillLabel.Enabled = false;
            this.putOffBackupTillLabel.Location = new System.Drawing.Point(4, 58);
            this.putOffBackupTillLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.putOffBackupTillLabel.Name = "putOffBackupTillLabel";
            this.putOffBackupTillLabel.Size = new System.Drawing.Size(338, 29);
            this.putOffBackupTillLabel.TabIndex = 23;
            this.putOffBackupTillLabel.Text = "Put off making backup till processor\'s  loading will be less then";
            this.putOffBackupTillLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cpuLoadingNumericUpDown
            // 
            this.cpuLoadingNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cpuLoadingNumericUpDown.Enabled = false;
            this.cpuLoadingNumericUpDown.Location = new System.Drawing.Point(350, 61);
            this.cpuLoadingNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
            this.cpuLoadingNumericUpDown.Size = new System.Drawing.Size(226, 23);
            this.cpuLoadingNumericUpDown.TabIndex = 4;
            this.cpuLoadingNumericUpDown.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableLayoutPanel.ColumnCount = 2;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.Controls.Add(this._parallelNumericUpDown, 1, 0);
            this._tableLayoutPanel.Controls.Add(this.cpuLoadingNumericUpDown, 1, 2);
            this._tableLayoutPanel.Controls.Add(this.putOffBackupTillLabel, 0, 2);
            this._tableLayoutPanel.Controls.Add(this._parallelLabel, 0, 0);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 4;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayoutPanel.Size = new System.Drawing.Size(580, 189);
            this._tableLayoutPanel.TabIndex = 0;
            // 
            // _parallelNumericUpDown
            // 
            this._parallelNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this._parallelNumericUpDown.Location = new System.Drawing.Point(350, 3);
            this._parallelNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._parallelNumericUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this._parallelNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._parallelNumericUpDown.Name = "_parallelNumericUpDown";
            this._parallelNumericUpDown.Size = new System.Drawing.Size(226, 23);
            this._parallelNumericUpDown.TabIndex = 2;
            this._parallelNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _parallelLabel
            // 
            this._parallelLabel.AutoSize = true;
            this._parallelLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this._parallelLabel.Location = new System.Drawing.Point(90, 0);
            this._parallelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._parallelLabel.Name = "_parallelLabel";
            this._parallelLabel.Size = new System.Drawing.Size(252, 29);
            this._parallelLabel.TabIndex = 0;
            this._parallelLabel.Text = "Amount of storages to process synchronously:";
            this._parallelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OtherOptionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MinimumSize = new System.Drawing.Size(573, 182);
            this.Name = "OtherOptionsUserControl";
            this.Size = new System.Drawing.Size(580, 189);
            ((System.ComponentModel.ISupportInitialize)(this.cpuLoadingNumericUpDown)).EndInit();
            this._tableLayoutPanel.ResumeLayout(false);
            this._tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._parallelNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }
		private System.Windows.Forms.NumericUpDown _parallelNumericUpDown;
		private System.Windows.Forms.Label _parallelLabel;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private System.Windows.Forms.NumericUpDown cpuLoadingNumericUpDown;
		private System.Windows.Forms.Label putOffBackupTillLabel;
	}
}
