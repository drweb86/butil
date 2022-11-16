
namespace BUtil.Configurator.BackupUiMaster.Controls
{
	partial class SettingsUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsUserControl));
            this.afterEndOfBackupGroupBox = new System.Windows.Forms.GroupBox();
            this.jobAfterOkBackupComboBox = new System.Windows.Forms.ComboBox();
            this.hearBeepsCheckBox = new System.Windows.Forms.CheckBox();
            this.helpActionAfterBackupButton = new System.Windows.Forms.Button();
            this._transparentTableLayoutPanel = new BUtil.Configurator.Configurator.Controls.Common.TransparentTableLayoutPanel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.encryptionUserControl1 = new BUtil.Core.PL.EncryptionUserControl();
            this.afterEndOfBackupGroupBox.SuspendLayout();
            this._transparentTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // afterEndOfBackupGroupBox
            // 
            this.afterEndOfBackupGroupBox.Controls.Add(this.jobAfterOkBackupComboBox);
            this.afterEndOfBackupGroupBox.Controls.Add(this.hearBeepsCheckBox);
            this.afterEndOfBackupGroupBox.Controls.Add(this.helpActionAfterBackupButton);
            this.afterEndOfBackupGroupBox.Location = new System.Drawing.Point(4, 55);
            this.afterEndOfBackupGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.afterEndOfBackupGroupBox.Name = "afterEndOfBackupGroupBox";
            this.afterEndOfBackupGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.afterEndOfBackupGroupBox.Size = new System.Drawing.Size(490, 100);
            this.afterEndOfBackupGroupBox.TabIndex = 13;
            this.afterEndOfBackupGroupBox.TabStop = false;
            this.afterEndOfBackupGroupBox.Text = "After completion of backup";
            // 
            // jobAfterOkBackupComboBox
            // 
            this.jobAfterOkBackupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.jobAfterOkBackupComboBox.FormattingEnabled = true;
            this.jobAfterOkBackupComboBox.Location = new System.Drawing.Point(7, 29);
            this.jobAfterOkBackupComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.jobAfterOkBackupComboBox.Name = "jobAfterOkBackupComboBox";
            this.jobAfterOkBackupComboBox.Size = new System.Drawing.Size(297, 28);
            this.jobAfterOkBackupComboBox.TabIndex = 3;
            // 
            // hearBeepsCheckBox
            // 
            this.hearBeepsCheckBox.AutoSize = true;
            this.hearBeepsCheckBox.Location = new System.Drawing.Point(7, 71);
            this.hearBeepsCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.hearBeepsCheckBox.Name = "hearBeepsCheckBox";
            this.hearBeepsCheckBox.Size = new System.Drawing.Size(155, 24);
            this.hearBeepsCheckBox.TabIndex = 5;
            this.hearBeepsCheckBox.Text = "Beep several times";
            this.hearBeepsCheckBox.UseVisualStyleBackColor = true;
            // 
            // helpActionAfterBackupButton
            // 
            this.helpActionAfterBackupButton.Image = ((System.Drawing.Image)(resources.GetObject("helpActionAfterBackupButton.Image")));
            this.helpActionAfterBackupButton.Location = new System.Drawing.Point(313, 29);
            this.helpActionAfterBackupButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.helpActionAfterBackupButton.Name = "helpActionAfterBackupButton";
            this.helpActionAfterBackupButton.Size = new System.Drawing.Size(48, 55);
            this.helpActionAfterBackupButton.TabIndex = 4;
            this.helpActionAfterBackupButton.UseVisualStyleBackColor = true;
            this.helpActionAfterBackupButton.Click += new System.EventHandler(this.HelpActionAfterBackupButtonClick);
            // 
            // _transparentTableLayoutPanel
            // 
            this._transparentTableLayoutPanel.AutoSize = true;
            this._transparentTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._transparentTableLayoutPanel.ColumnCount = 2;
            this._transparentTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._transparentTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._transparentTableLayoutPanel.Controls.Add(this.titleLabel, 0, 0);
            this._transparentTableLayoutPanel.Controls.Add(this.afterEndOfBackupGroupBox, 0, 1);
            this._transparentTableLayoutPanel.Controls.Add(this.encryptionUserControl1, 1, 1);
            this._transparentTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._transparentTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._transparentTableLayoutPanel.Name = "_transparentTableLayoutPanel";
            this._transparentTableLayoutPanel.RowCount = 2;
            this._transparentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this._transparentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._transparentTableLayoutPanel.Size = new System.Drawing.Size(997, 374);
            this._transparentTableLayoutPanel.TabIndex = 14;
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.titleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._transparentTableLayoutPanel.SetColumnSpan(this.titleLabel, 2);
            this.titleLabel.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(4, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(1243, 45);
            this.titleLabel.TabIndex = 15;
            this.titleLabel.Text = "Title";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // encryptionUserControl1
            // 
            this.encryptionUserControl1.BackColor = System.Drawing.SystemColors.Window;
            this.encryptionUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.encryptionUserControl1.HelpLabel = null;
            this.encryptionUserControl1.Location = new System.Drawing.Point(504, 54);
            this.encryptionUserControl1.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.encryptionUserControl1.Name = "encryptionUserControl1";
            this.encryptionUserControl1.Size = new System.Drawing.Size(741, 316);
            this.encryptionUserControl1.TabIndex = 16;
            // 
            // SettingsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._transparentTableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(7, 12, 7, 12);
            this.MinimumSize = new System.Drawing.Size(689, 374);
            this.Name = "SettingsUserControl";
            this.Size = new System.Drawing.Size(997, 374);
            this.afterEndOfBackupGroupBox.ResumeLayout(false);
            this.afterEndOfBackupGroupBox.PerformLayout();
            this._transparentTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Button helpActionAfterBackupButton;
		private System.Windows.Forms.CheckBox hearBeepsCheckBox;
		private System.Windows.Forms.ComboBox jobAfterOkBackupComboBox;
		private System.Windows.Forms.GroupBox afterEndOfBackupGroupBox;
        private Configurator.Controls.Common.TransparentTableLayoutPanel _transparentTableLayoutPanel;
        private System.Windows.Forms.Label titleLabel;
        private Core.PL.EncryptionUserControl encryptionUserControl1;
    }
}
