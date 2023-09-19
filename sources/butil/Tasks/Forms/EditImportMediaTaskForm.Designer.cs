namespace BUtil.Configurator.Configurator.Forms
{
    partial class EditImportMediaTaskForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditImportMediaTaskForm));
            nestingControlsPanel = new System.Windows.Forms.Panel();
            okButton = new System.Windows.Forms.Button();
            cancelButton = new System.Windows.Forms.Button();
            helpStatusStrip = new System.Windows.Forms.StatusStrip();
            _toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            optionsHeader = new BUtil.Configurator.Controls.OptionsHeader();
            choosePanelUserControl = new BUtil.Configurator.Controls.TaskNavigationControl();
            helpStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // nestingControlsPanel
            // 
            nestingControlsPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            nestingControlsPanel.Location = new System.Drawing.Point(166, 37);
            nestingControlsPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nestingControlsPanel.Name = "nestingControlsPanel";
            nestingControlsPanel.Size = new System.Drawing.Size(765, 357);
            nestingControlsPanel.TabIndex = 6;
            // 
            // okButton
            // 
            okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            okButton.AutoSize = true;
            okButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            okButton.Location = new System.Drawing.Point(734, 400);
            okButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            okButton.MinimumSize = new System.Drawing.Size(88, 27);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(88, 27);
            okButton.TabIndex = 8;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += SaveRequest;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancelButton.AutoSize = true;
            cancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(831, 400);
            cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancelButton.MinimumSize = new System.Drawing.Size(88, 27);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(88, 27);
            cancelButton.TabIndex = 9;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // helpStatusStrip
            // 
            helpStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            helpStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _toolStripStatusLabel });
            helpStatusStrip.Location = new System.Drawing.Point(0, 434);
            helpStatusStrip.Name = "helpStatusStrip";
            helpStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            helpStatusStrip.Size = new System.Drawing.Size(933, 22);
            helpStatusStrip.TabIndex = 10;
            helpStatusStrip.Text = "statusStrip1";
            // 
            // _toolStripStatusLabel
            // 
            _toolStripStatusLabel.Name = "_toolStripStatusLabel";
            _toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // optionsHeader
            // 
            optionsHeader.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            optionsHeader.BackColor = System.Drawing.Color.DodgerBlue;
            optionsHeader.ForeColor = System.Drawing.Color.White;
            optionsHeader.Location = new System.Drawing.Point(166, 2);
            optionsHeader.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            optionsHeader.MinimumSize = new System.Drawing.Size(190, 33);
            optionsHeader.Name = "optionsHeader";
            optionsHeader.Size = new System.Drawing.Size(765, 33);
            optionsHeader.TabIndex = 7;
            optionsHeader.Title = "Title";
            // 
            // choosePanelUserControl
            // 
            choosePanelUserControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            choosePanelUserControl.BackColor = System.Drawing.SystemColors.Control;
            choosePanelUserControl.BackgroundImage = (System.Drawing.Image)resources.GetObject("choosePanelUserControl.BackgroundImage");
            choosePanelUserControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            choosePanelUserControl.DrawAtractiveBorders = false;
            choosePanelUserControl.EncryptionVisi1ble = true;
            choosePanelUserControl.HelpLabel = null;
            choosePanelUserControl.Location = new System.Drawing.Point(2, 1);
            choosePanelUserControl.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            choosePanelUserControl.MinimumSize = new System.Drawing.Size(161, 357);
            choosePanelUserControl.Name = "choosePanelUserControl";
            choosePanelUserControl.Size = new System.Drawing.Size(161, 426);
            choosePanelUserControl.TabIndex = 0;
            choosePanelUserControl.WhenVisible = true;
            choosePanelUserControl.ViewChanged += ViewChangeNotification;
            choosePanelUserControl.CanChangeView += OnCanChangeView;
            // 
            // EditImportMediaTaskForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(933, 456);
            Controls.Add(helpStatusStrip);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
            Controls.Add(nestingControlsPanel);
            Controls.Add(optionsHeader);
            Controls.Add(choosePanelUserControl);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(804, 492);
            Name = "EditImportMediaTaskForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "BackupTaskEditForm";
            helpStatusStrip.ResumeLayout(false);
            helpStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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