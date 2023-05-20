
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsUserControl));
            afterEndOfBackupGroupBox = new System.Windows.Forms.GroupBox();
            jobAfterOkBackupComboBox = new System.Windows.Forms.ComboBox();
            helpActionAfterBackupButton = new System.Windows.Forms.Button();
            titleLabel = new System.Windows.Forms.Label();
            afterEndOfBackupGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // afterEndOfBackupGroupBox
            // 
            afterEndOfBackupGroupBox.Controls.Add(jobAfterOkBackupComboBox);
            afterEndOfBackupGroupBox.Controls.Add(helpActionAfterBackupButton);
            afterEndOfBackupGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            afterEndOfBackupGroupBox.Location = new System.Drawing.Point(0, 34);
            afterEndOfBackupGroupBox.Margin = new System.Windows.Forms.Padding(4);
            afterEndOfBackupGroupBox.MinimumSize = new System.Drawing.Size(300, 100);
            afterEndOfBackupGroupBox.Name = "afterEndOfBackupGroupBox";
            afterEndOfBackupGroupBox.Padding = new System.Windows.Forms.Padding(4);
            afterEndOfBackupGroupBox.Size = new System.Drawing.Size(737, 149);
            afterEndOfBackupGroupBox.TabIndex = 13;
            afterEndOfBackupGroupBox.TabStop = false;
            afterEndOfBackupGroupBox.Text = "After completion of backup";
            // 
            // jobAfterOkBackupComboBox
            // 
            jobAfterOkBackupComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            jobAfterOkBackupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            jobAfterOkBackupComboBox.FormattingEnabled = true;
            jobAfterOkBackupComboBox.Location = new System.Drawing.Point(6, 22);
            jobAfterOkBackupComboBox.Margin = new System.Windows.Forms.Padding(4);
            jobAfterOkBackupComboBox.Name = "jobAfterOkBackupComboBox";
            jobAfterOkBackupComboBox.Size = new System.Drawing.Size(673, 23);
            jobAfterOkBackupComboBox.TabIndex = 3;
            // 
            // helpActionAfterBackupButton
            // 
            helpActionAfterBackupButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            helpActionAfterBackupButton.Image = (System.Drawing.Image)resources.GetObject("helpActionAfterBackupButton.Image");
            helpActionAfterBackupButton.Location = new System.Drawing.Point(687, 12);
            helpActionAfterBackupButton.Margin = new System.Windows.Forms.Padding(4);
            helpActionAfterBackupButton.Name = "helpActionAfterBackupButton";
            helpActionAfterBackupButton.Size = new System.Drawing.Size(42, 41);
            helpActionAfterBackupButton.TabIndex = 4;
            helpActionAfterBackupButton.UseVisualStyleBackColor = true;
            helpActionAfterBackupButton.Click += HelpActionAfterBackupButtonClick;
            // 
            // titleLabel
            // 
            titleLabel.BackColor = System.Drawing.Color.DodgerBlue;
            titleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            titleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            titleLabel.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            titleLabel.ForeColor = System.Drawing.Color.White;
            titleLabel.Location = new System.Drawing.Point(0, 0);
            titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new System.Drawing.Size(737, 34);
            titleLabel.TabIndex = 15;
            titleLabel.Text = "Title";
            titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SettingsUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(afterEndOfBackupGroupBox);
            Controls.Add(titleLabel);
            Margin = new System.Windows.Forms.Padding(6, 9, 6, 9);
            MinimumSize = new System.Drawing.Size(737, 183);
            Name = "SettingsUserControl";
            Size = new System.Drawing.Size(737, 183);
            afterEndOfBackupGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Button helpActionAfterBackupButton;
        private System.Windows.Forms.ComboBox jobAfterOkBackupComboBox;
        private System.Windows.Forms.GroupBox afterEndOfBackupGroupBox;
        private System.Windows.Forms.Label titleLabel;
    }
}
