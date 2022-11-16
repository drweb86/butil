﻿namespace BUtil.Configurator.Configurator.Forms
{
    partial class EditBackupTaskForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditBackupTaskForm));
            this.nestingControlsPanel = new System.Windows.Forms.Panel();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.helpStatusStrip = new System.Windows.Forms.StatusStrip();
            this._toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.optionsHeader = new BUtil.Configurator.Controls.OptionsHeader();
            this.choosePanelUserControl = new BUtil.Configurator.Controls.TaskNavigationControl();
            this.helpStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // nestingControlsPanel
            // 
            this.nestingControlsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nestingControlsPanel.Location = new System.Drawing.Point(190, 49);
            this.nestingControlsPanel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.nestingControlsPanel.Name = "nestingControlsPanel";
            this.nestingControlsPanel.Size = new System.Drawing.Size(710, 476);
            this.nestingControlsPanel.TabIndex = 6;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.AutoSize = true;
            this.okButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.okButton.Location = new System.Drawing.Point(674, 533);
            this.okButton.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.okButton.MinimumSize = new System.Drawing.Size(101, 36);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(101, 36);
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
            this.cancelButton.Location = new System.Drawing.Point(785, 533);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cancelButton.MinimumSize = new System.Drawing.Size(101, 36);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(101, 36);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // helpStatusStrip
            // 
            this.helpStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.helpStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripStatusLabel});
            this.helpStatusStrip.Location = new System.Drawing.Point(0, 586);
            this.helpStatusStrip.Name = "helpStatusStrip";
            this.helpStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 18, 0);
            this.helpStatusStrip.Size = new System.Drawing.Size(902, 22);
            this.helpStatusStrip.TabIndex = 10;
            this.helpStatusStrip.Text = "statusStrip1";
            // 
            // _toolStripStatusLabel
            // 
            this._toolStripStatusLabel.Name = "_toolStripStatusLabel";
            this._toolStripStatusLabel.Size = new System.Drawing.Size(0, 16);
            // 
            // optionsHeader
            // 
            this.optionsHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsHeader.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.optionsHeader.ForeColor = System.Drawing.Color.White;
            this.optionsHeader.Location = new System.Drawing.Point(190, 3);
            this.optionsHeader.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.optionsHeader.MinimumSize = new System.Drawing.Size(217, 44);
            this.optionsHeader.Name = "optionsHeader";
            this.optionsHeader.Size = new System.Drawing.Size(710, 44);
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
            this.choosePanelUserControl.Margin = new System.Windows.Forms.Padding(7, 4, 7, 4);
            this.choosePanelUserControl.MinimumSize = new System.Drawing.Size(184, 476);
            this.choosePanelUserControl.Name = "choosePanelUserControl";
            this.choosePanelUserControl.Size = new System.Drawing.Size(184, 568);
            this.choosePanelUserControl.TabIndex = 0;
            this.choosePanelUserControl.ViewChanged += new BUtil.Configurator.Controls.TaskNavigationControl.ChangeViewEventHandler(this.ViewChangeNotification);
            this.choosePanelUserControl.CanChangeView += new BUtil.Configurator.Controls.TaskNavigationControl.CanChangeViewEventHandler(this.OnCanChangeView);
            // 
            // EditBackupTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(902, 608);
            this.Controls.Add(this.helpStatusStrip);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.nestingControlsPanel);
            this.Controls.Add(this.optionsHeader);
            this.Controls.Add(this.choosePanelUserControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MinimumSize = new System.Drawing.Size(917, 643);
            this.Name = "EditBackupTaskForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BackupTaskEditForm";
            this.helpStatusStrip.ResumeLayout(false);
            this.helpStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BUtil.Configurator.Controls.TaskNavigationControl choosePanelUserControl;
        private System.Windows.Forms.Panel nestingControlsPanel;
        private BUtil.Configurator.Controls.OptionsHeader optionsHeader;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.StatusStrip helpStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _toolStripStatusLabel;
    }
}