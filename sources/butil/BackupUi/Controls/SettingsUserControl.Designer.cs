
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.afterEndOfBackupGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // afterEndOfBackupGroupBox
            // 
            this.afterEndOfBackupGroupBox.Controls.Add(this.jobAfterOkBackupComboBox);
            this.afterEndOfBackupGroupBox.Controls.Add(this.hearBeepsCheckBox);
            this.afterEndOfBackupGroupBox.Controls.Add(this.helpActionAfterBackupButton);
            this.afterEndOfBackupGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.afterEndOfBackupGroupBox.Location = new System.Drawing.Point(0, 34);
            this.afterEndOfBackupGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.afterEndOfBackupGroupBox.MinimumSize = new System.Drawing.Size(300, 100);
            this.afterEndOfBackupGroupBox.Name = "afterEndOfBackupGroupBox";
            this.afterEndOfBackupGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.afterEndOfBackupGroupBox.Size = new System.Drawing.Size(737, 149);
            this.afterEndOfBackupGroupBox.TabIndex = 13;
            this.afterEndOfBackupGroupBox.TabStop = false;
            this.afterEndOfBackupGroupBox.Text = "After completion of backup";
            // 
            // jobAfterOkBackupComboBox
            // 
            this.jobAfterOkBackupComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jobAfterOkBackupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.jobAfterOkBackupComboBox.FormattingEnabled = true;
            this.jobAfterOkBackupComboBox.Location = new System.Drawing.Point(6, 22);
            this.jobAfterOkBackupComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.jobAfterOkBackupComboBox.Name = "jobAfterOkBackupComboBox";
            this.jobAfterOkBackupComboBox.Size = new System.Drawing.Size(673, 23);
            this.jobAfterOkBackupComboBox.TabIndex = 3;
            // 
            // hearBeepsCheckBox
            // 
            this.hearBeepsCheckBox.AutoSize = true;
            this.hearBeepsCheckBox.Location = new System.Drawing.Point(6, 53);
            this.hearBeepsCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.hearBeepsCheckBox.Name = "hearBeepsCheckBox";
            this.hearBeepsCheckBox.Size = new System.Drawing.Size(123, 19);
            this.hearBeepsCheckBox.TabIndex = 5;
            this.hearBeepsCheckBox.Text = "Beep several times";
            this.hearBeepsCheckBox.UseVisualStyleBackColor = true;
            // 
            // helpActionAfterBackupButton
            // 
            this.helpActionAfterBackupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpActionAfterBackupButton.Image = ((System.Drawing.Image)(resources.GetObject("helpActionAfterBackupButton.Image")));
            this.helpActionAfterBackupButton.Location = new System.Drawing.Point(687, 12);
            this.helpActionAfterBackupButton.Margin = new System.Windows.Forms.Padding(4);
            this.helpActionAfterBackupButton.Name = "helpActionAfterBackupButton";
            this.helpActionAfterBackupButton.Size = new System.Drawing.Size(42, 41);
            this.helpActionAfterBackupButton.TabIndex = 4;
            this.helpActionAfterBackupButton.UseVisualStyleBackColor = true;
            this.helpActionAfterBackupButton.Click += new System.EventHandler(this.HelpActionAfterBackupButtonClick);
            // 
            // titleLabel
            // 
            this.titleLabel.BackColor = System.Drawing.Color.DodgerBlue;
            this.titleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabel.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(737, 34);
            this.titleLabel.TabIndex = 15;
            this.titleLabel.Text = "Title";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SettingsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.afterEndOfBackupGroupBox);
            this.Controls.Add(this.titleLabel);
            this.Margin = new System.Windows.Forms.Padding(6, 9, 6, 9);
            this.MinimumSize = new System.Drawing.Size(737, 183);
            this.Name = "SettingsUserControl";
            this.Size = new System.Drawing.Size(737, 183);
            this.afterEndOfBackupGroupBox.ResumeLayout(false);
            this.afterEndOfBackupGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Button helpActionAfterBackupButton;
		private System.Windows.Forms.CheckBox hearBeepsCheckBox;
		private System.Windows.Forms.ComboBox jobAfterOkBackupComboBox;
		private System.Windows.Forms.GroupBox afterEndOfBackupGroupBox;
        private System.Windows.Forms.Label titleLabel;
    }
}
