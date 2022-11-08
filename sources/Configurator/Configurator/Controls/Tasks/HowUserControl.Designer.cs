
namespace BUtil.Configurator.Controls
{
	partial class HowUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HowUserControl));
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._chooseBackupModel = new System.Windows.Forms.Label();
            this._incrementalBackupRadioButton = new System.Windows.Forms.RadioButton();
            this._disableCompressionEncryptionCheckBox = new System.Windows.Forms.CheckBox();
            this._disableCompressionAndEncryptionUsagesLabel = new System.Windows.Forms.Label();
            this._backupModelLabel = new System.Windows.Forms.Label();
            this._tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.ColumnCount = 1;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.Controls.Add(this._chooseBackupModel, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._incrementalBackupRadioButton, 0, 1);
            this._tableLayoutPanel.Controls.Add(this._disableCompressionEncryptionCheckBox, 0, 3);
            this._tableLayoutPanel.Controls.Add(this._disableCompressionAndEncryptionUsagesLabel, 0, 4);
            this._tableLayoutPanel.Controls.Add(this._backupModelLabel, 0, 2);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.Padding = new System.Windows.Forms.Padding(8, 8, 8, 0);
            this._tableLayoutPanel.RowCount = 5;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.Size = new System.Drawing.Size(685, 342);
            this._tableLayoutPanel.TabIndex = 0;
            // 
            // _chooseBackupModel
            // 
            this._chooseBackupModel.AutoSize = true;
            this._chooseBackupModel.Location = new System.Drawing.Point(11, 8);
            this._chooseBackupModel.Name = "_chooseBackupModel";
            this._chooseBackupModel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this._chooseBackupModel.Size = new System.Drawing.Size(126, 23);
            this._chooseBackupModel.TabIndex = 1;
            this._chooseBackupModel.Text = "Choose backup model";
            // 
            // _incrementalBackupRadioButton
            // 
            this._incrementalBackupRadioButton.AutoSize = true;
            this._incrementalBackupRadioButton.Checked = true;
            this._incrementalBackupRadioButton.Location = new System.Drawing.Point(11, 34);
            this._incrementalBackupRadioButton.Name = "_incrementalBackupRadioButton";
            this._incrementalBackupRadioButton.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this._incrementalBackupRadioButton.Size = new System.Drawing.Size(130, 27);
            this._incrementalBackupRadioButton.TabIndex = 2;
            this._incrementalBackupRadioButton.TabStop = true;
            this._incrementalBackupRadioButton.Text = "Incremental backup";
            this._incrementalBackupRadioButton.UseVisualStyleBackColor = true;
            // 
            // _disableCompressionEncryptionCheckBox
            // 
            this._disableCompressionEncryptionCheckBox.AutoSize = true;
            this._disableCompressionEncryptionCheckBox.Checked = true;
            this._disableCompressionEncryptionCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._disableCompressionEncryptionCheckBox.Enabled = false;
            this._disableCompressionEncryptionCheckBox.Location = new System.Drawing.Point(11, 157);
            this._disableCompressionEncryptionCheckBox.Name = "_disableCompressionEncryptionCheckBox";
            this._disableCompressionEncryptionCheckBox.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this._disableCompressionEncryptionCheckBox.Size = new System.Drawing.Size(218, 27);
            this._disableCompressionEncryptionCheckBox.TabIndex = 4;
            this._disableCompressionEncryptionCheckBox.Text = "Disable compression and encryption";
            this._disableCompressionEncryptionCheckBox.UseVisualStyleBackColor = true;
            // 
            // _disableCompressionAndEncryptionUsagesLabel
            // 
            this._disableCompressionAndEncryptionUsagesLabel.AutoSize = true;
            this._disableCompressionAndEncryptionUsagesLabel.Location = new System.Drawing.Point(11, 187);
            this._disableCompressionAndEncryptionUsagesLabel.Name = "_disableCompressionAndEncryptionUsagesLabel";
            this._disableCompressionAndEncryptionUsagesLabel.Size = new System.Drawing.Size(658, 120);
            this._disableCompressionAndEncryptionUsagesLabel.TabIndex = 5;
            this._disableCompressionAndEncryptionUsagesLabel.Text = resources.GetString("_disableCompressionAndEncryptionUsagesLabel.Text");
            // 
            // _backupModelLabel
            // 
            this._backupModelLabel.AutoSize = true;
            this._backupModelLabel.Location = new System.Drawing.Point(11, 64);
            this._backupModelLabel.Name = "_backupModelLabel";
            this._backupModelLabel.Size = new System.Drawing.Size(661, 90);
            this._backupModelLabel.TabIndex = 3;
            this._backupModelLabel.Text = resources.GetString("_backupModelLabel.Text");
            // 
            // HowUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MinimumSize = new System.Drawing.Size(363, 193);
            this.Name = "HowUserControl";
            this.Size = new System.Drawing.Size(685, 342);
            this._tableLayoutPanel.ResumeLayout(false);
            this._tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.Label _chooseBackupModel;
        private System.Windows.Forms.CheckBox _disableCompressionEncryptionCheckBox;
        private System.Windows.Forms.RadioButton _incrementalBackupRadioButton;
        private System.Windows.Forms.Label _backupModelLabel;
        private System.Windows.Forms.Label _disableCompressionAndEncryptionUsagesLabel;
    }
}
