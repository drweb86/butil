namespace BUtil.Configurator.Configurator.Forms
{
    partial class BackupTaskEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupTaskEditForm));
            this.nestingControlsPanel = new System.Windows.Forms.Panel();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.helpStatusStrip = new System.Windows.Forms.StatusStrip();
            this.helpToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.backupTaskTitleLabel_ = new System.Windows.Forms.Label();
            this._taskTitleTextBox = new System.Windows.Forms.TextBox();
            this.optionsHeader = new BUtil.Configurator.Controls.OptionsHeader();
            this.choosePanelUserControl = new BUtil.Configurator.Controls.EditTasksLeftPanelUserControl();
            this._saveAndExecuteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nestingControlsPanel
            // 
            this.nestingControlsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nestingControlsPanel.Location = new System.Drawing.Point(142, 32);
            this.nestingControlsPanel.Name = "nestingControlsPanel";
            this.nestingControlsPanel.Size = new System.Drawing.Size(532, 296);
            this.nestingControlsPanel.TabIndex = 6;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.AutoSize = true;
            this.okButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.okButton.Location = new System.Drawing.Point(396, 347);
            this.okButton.MinimumSize = new System.Drawing.Size(75, 23);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.SaveRequest);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.AutoSize = true;
            this.cancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(589, 347);
            this.cancelButton.MinimumSize = new System.Drawing.Size(75, 23);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // helpStatusStrip
            // 
            this.helpStatusStrip.Location = new System.Drawing.Point(0, 373);
            this.helpStatusStrip.Name = "helpStatusStrip";
            this.helpStatusStrip.Size = new System.Drawing.Size(676, 22);
            this.helpStatusStrip.TabIndex = 10;
            this.helpStatusStrip.Text = "statusStrip1";
            // 
            // helpToolStripStatusLabel
            // 
            this.helpToolStripStatusLabel.Name = "helpToolStripStatusLabel";
            this.helpToolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // backupTaskTitleLabel_
            // 
            this.backupTaskTitleLabel_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.backupTaskTitleLabel_.AutoSize = true;
            this.backupTaskTitleLabel_.Location = new System.Drawing.Point(146, 352);
            this.backupTaskTitleLabel_.Name = "backupTaskTitleLabel_";
            this.backupTaskTitleLabel_.Size = new System.Drawing.Size(53, 13);
            this.backupTaskTitleLabel_.TabIndex = 11;
            this.backupTaskTitleLabel_.Text = "Task title:";
            // 
            // _taskTitleTextBox
            // 
            this._taskTitleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._taskTitleTextBox.Location = new System.Drawing.Point(205, 349);
            this._taskTitleTextBox.Name = "_taskTitleTextBox";
            this._taskTitleTextBox.Size = new System.Drawing.Size(185, 20);
            this._taskTitleTextBox.TabIndex = 12;
            // 
            // optionsHeader
            // 
            this.optionsHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsHeader.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.optionsHeader.ForeColor = System.Drawing.Color.White;
            this.optionsHeader.Location = new System.Drawing.Point(142, 2);
            this.optionsHeader.MinimumSize = new System.Drawing.Size(163, 29);
            this.optionsHeader.Name = "optionsHeader";
            this.optionsHeader.Size = new System.Drawing.Size(532, 29);
            this.optionsHeader.TabIndex = 7;
            this.optionsHeader.Title = "Title";
            // 
            // choosePanelUserControl
            // 
            this.choosePanelUserControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.choosePanelUserControl.BackColor = System.Drawing.SystemColors.Control;
            this.choosePanelUserControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("choosePanelUserControl.BackgroundImage")));
            this.choosePanelUserControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.choosePanelUserControl.DrawAtractiveBorders = false;
            this.choosePanelUserControl.HelpLabel = null;
            this.choosePanelUserControl.Location = new System.Drawing.Point(2, 1);
            this.choosePanelUserControl.MinimumSize = new System.Drawing.Size(138, 309);
            this.choosePanelUserControl.Name = "choosePanelUserControl";
            this.choosePanelUserControl.Size = new System.Drawing.Size(138, 369);
            this.choosePanelUserControl.TabIndex = 0;
            this.choosePanelUserControl.ViewChanged += new BUtil.Configurator.Controls.EditTasksLeftPanelUserControl.ChangeViewEventHandler(this.ViewChangeNotification);
            // 
            // _saveAndExecuteButton
            // 
            this._saveAndExecuteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._saveAndExecuteButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._saveAndExecuteButton.Location = new System.Drawing.Point(478, 334);
            this._saveAndExecuteButton.MinimumSize = new System.Drawing.Size(75, 23);
            this._saveAndExecuteButton.Name = "_saveAndExecuteButton";
            this._saveAndExecuteButton.Size = new System.Drawing.Size(105, 36);
            this._saveAndExecuteButton.TabIndex = 13;
            this._saveAndExecuteButton.Text = "Save and Run";
            this._saveAndExecuteButton.UseVisualStyleBackColor = true;
            this._saveAndExecuteButton.Click += new System.EventHandler(this.SaveAndExecuteTaskButtonRequest);
            // 
            // BackupTaskEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(676, 395);
            this.Controls.Add(this._saveAndExecuteButton);
            this.Controls.Add(this._taskTitleTextBox);
            this.Controls.Add(this.backupTaskTitleLabel_);
            this.Controls.Add(this.helpStatusStrip);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.nestingControlsPanel);
            this.Controls.Add(this.optionsHeader);
            this.Controls.Add(this.choosePanelUserControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(692, 433);
            this.Name = "BackupTaskEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BackupTaskEditForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BUtil.Configurator.Controls.EditTasksLeftPanelUserControl choosePanelUserControl;
        private System.Windows.Forms.Panel nestingControlsPanel;
        private BUtil.Configurator.Controls.OptionsHeader optionsHeader;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.StatusStrip helpStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel helpToolStripStatusLabel;
        private System.Windows.Forms.Label backupTaskTitleLabel_;
        private System.Windows.Forms.TextBox _taskTitleTextBox;
        private System.Windows.Forms.Button _saveAndExecuteButton;
    }
}