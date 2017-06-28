
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
			this.backupPriorityComboBox = new System.Windows.Forms.ComboBox();
			this.chooseBackUpPriorityLabel = new System.Windows.Forms.Label();
			this.afterEndOfBackupGroupBox = new System.Windows.Forms.GroupBox();
			this.jobAfterOkBackupComboBox = new System.Windows.Forms.ComboBox();
			this.hearBeepsCheckBox = new System.Windows.Forms.CheckBox();
			this.helpActionAfterBackupButton = new System.Windows.Forms.Button();
			this.encryptionUserControl = new BUtil.Core.PL.EncryptionUserControl();
			this.afterEndOfBackupGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// backupPriorityComboBox
			// 
			this.backupPriorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.backupPriorityComboBox.FormattingEnabled = true;
			this.backupPriorityComboBox.Location = new System.Drawing.Point(131, 40);
			this.backupPriorityComboBox.Name = "backupPriorityComboBox";
			this.backupPriorityComboBox.Size = new System.Drawing.Size(149, 21);
			this.backupPriorityComboBox.TabIndex = 11;
			// 
			// chooseBackUpPriorityLabel
			// 
			this.chooseBackUpPriorityLabel.AutoSize = true;
			this.chooseBackUpPriorityLabel.Location = new System.Drawing.Point(3, 43);
			this.chooseBackUpPriorityLabel.Name = "chooseBackUpPriorityLabel";
			this.chooseBackUpPriorityLabel.Size = new System.Drawing.Size(81, 13);
			this.chooseBackUpPriorityLabel.TabIndex = 12;
			this.chooseBackUpPriorityLabel.Text = "Process priority:";
			// 
			// afterEndOfBackupGroupBox
			// 
			this.afterEndOfBackupGroupBox.Controls.Add(this.jobAfterOkBackupComboBox);
			this.afterEndOfBackupGroupBox.Controls.Add(this.hearBeepsCheckBox);
			this.afterEndOfBackupGroupBox.Controls.Add(this.helpActionAfterBackupButton);
			this.afterEndOfBackupGroupBox.Location = new System.Drawing.Point(3, 67);
			this.afterEndOfBackupGroupBox.Name = "afterEndOfBackupGroupBox";
			this.afterEndOfBackupGroupBox.Size = new System.Drawing.Size(277, 75);
			this.afterEndOfBackupGroupBox.TabIndex = 13;
			this.afterEndOfBackupGroupBox.TabStop = false;
			this.afterEndOfBackupGroupBox.Text = "After completion of backup";
			// 
			// jobAfterOkBackupComboBox
			// 
			this.jobAfterOkBackupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.jobAfterOkBackupComboBox.FormattingEnabled = true;
			this.jobAfterOkBackupComboBox.Location = new System.Drawing.Point(5, 19);
			this.jobAfterOkBackupComboBox.Name = "jobAfterOkBackupComboBox";
			this.jobAfterOkBackupComboBox.Size = new System.Drawing.Size(224, 21);
			this.jobAfterOkBackupComboBox.TabIndex = 3;
			// 
			// hearBeepsCheckBox
			// 
			this.hearBeepsCheckBox.AutoSize = true;
			this.hearBeepsCheckBox.Location = new System.Drawing.Point(5, 46);
			this.hearBeepsCheckBox.Name = "hearBeepsCheckBox";
			this.hearBeepsCheckBox.Size = new System.Drawing.Size(115, 17);
			this.hearBeepsCheckBox.TabIndex = 5;
			this.hearBeepsCheckBox.Text = "Beep several times";
			this.hearBeepsCheckBox.UseVisualStyleBackColor = true;
			// 
			// helpActionAfterBackupButton
			// 
			this.helpActionAfterBackupButton.Image = ((System.Drawing.Image)(resources.GetObject("helpActionAfterBackupButton.Image")));
			this.helpActionAfterBackupButton.Location = new System.Drawing.Point(235, 19);
			this.helpActionAfterBackupButton.Name = "helpActionAfterBackupButton";
			this.helpActionAfterBackupButton.Size = new System.Drawing.Size(36, 36);
			this.helpActionAfterBackupButton.TabIndex = 4;
			this.helpActionAfterBackupButton.UseVisualStyleBackColor = true;
			this.helpActionAfterBackupButton.Click += new System.EventHandler(this.HelpActionAfterBackupButtonClick);
			// 
			// encryptionUserControl
			// 
			this.encryptionUserControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.encryptionUserControl.BackColor = System.Drawing.SystemColors.Window;
			this.encryptionUserControl.DrawAtractiveBorders = true;
			this.encryptionUserControl.HelpLabel = null;
			this.encryptionUserControl.Location = new System.Drawing.Point(286, 33);
			this.encryptionUserControl.Name = "encryptionUserControl";
			this.encryptionUserControl.Size = new System.Drawing.Size(230, 198);
			this.encryptionUserControl.TabIndex = 14;
			// 
			// SettingsUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.Controls.Add(this.encryptionUserControl);
			this.Controls.Add(this.afterEndOfBackupGroupBox);
			this.Controls.Add(this.backupPriorityComboBox);
			this.Controls.Add(this.chooseBackUpPriorityLabel);
			this.MinimumSize = new System.Drawing.Size(517, 243);
			this.Name = "SettingsUserControl";
			this.Size = new System.Drawing.Size(519, 245);
			this.Title = "Settings";
			this.Controls.SetChildIndex(this.chooseBackUpPriorityLabel, 0);
			this.Controls.SetChildIndex(this.backupPriorityComboBox, 0);
			this.Controls.SetChildIndex(this.afterEndOfBackupGroupBox, 0);
			this.Controls.SetChildIndex(this.encryptionUserControl, 0);
			this.afterEndOfBackupGroupBox.ResumeLayout(false);
			this.afterEndOfBackupGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private BUtil.Core.PL.EncryptionUserControl encryptionUserControl;
		private System.Windows.Forms.Button helpActionAfterBackupButton;
		private System.Windows.Forms.CheckBox hearBeepsCheckBox;
		private System.Windows.Forms.ComboBox jobAfterOkBackupComboBox;
		private System.Windows.Forms.GroupBox afterEndOfBackupGroupBox;
		private System.Windows.Forms.Label chooseBackUpPriorityLabel;
		private System.Windows.Forms.ComboBox backupPriorityComboBox;
	}
}
