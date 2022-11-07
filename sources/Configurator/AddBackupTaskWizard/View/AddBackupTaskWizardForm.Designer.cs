namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    partial class CreateBackupTaskWizardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateBackupTaskWizardForm));
            this._panel = new System.Windows.Forms.Panel();
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this._descriptionLabel = new System.Windows.Forms.Label();
            this._titleLabel = new System.Windows.Forms.Label();
            this._containerPanel = new System.Windows.Forms.Panel();
            this._nextButton = new System.Windows.Forms.Button();
            this._previousButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _panel
            // 
            this._panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panel.BackColor = System.Drawing.Color.White;
            this._panel.Controls.Add(this._pictureBox);
            this._panel.Controls.Add(this._descriptionLabel);
            this._panel.Controls.Add(this._titleLabel);
            this._panel.Location = new System.Drawing.Point(1, 2);
            this._panel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._panel.Name = "_panel";
            this._panel.Size = new System.Drawing.Size(960, 62);
            this._panel.TabIndex = 0;
            // 
            // _pictureBox
            // 
            this._pictureBox.Location = new System.Drawing.Point(15, 7);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(48, 48);
            this._pictureBox.TabIndex = 2;
            this._pictureBox.TabStop = false;
            // 
            // _descriptionLabel
            // 
            this._descriptionLabel.AutoSize = true;
            this._descriptionLabel.Location = new System.Drawing.Point(74, 35);
            this._descriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._descriptionLabel.Name = "_descriptionLabel";
            this._descriptionLabel.Size = new System.Drawing.Size(69, 15);
            this._descriptionLabel.TabIndex = 1;
            this._descriptionLabel.Text = "<Step title>";
            // 
            // _titleLabel
            // 
            this._titleLabel.AutoSize = true;
            this._titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._titleLabel.Location = new System.Drawing.Point(74, 12);
            this._titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Size = new System.Drawing.Size(72, 13);
            this._titleLabel.TabIndex = 0;
            this._titleLabel.Text = "<Step title>";
            // 
            // _containerPanel
            // 
            this._containerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._containerPanel.Location = new System.Drawing.Point(14, 72);
            this._containerPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._containerPanel.Name = "_containerPanel";
            this._containerPanel.Size = new System.Drawing.Size(936, 562);
            this._containerPanel.TabIndex = 1;
            // 
            // _nextButton
            // 
            this._nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._nextButton.Location = new System.Drawing.Point(768, 640);
            this._nextButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._nextButton.Name = "_nextButton";
            this._nextButton.Size = new System.Drawing.Size(88, 27);
            this._nextButton.TabIndex = 2;
            this._nextButton.Text = "Next >";
            this._nextButton.UseVisualStyleBackColor = true;
            this._nextButton.Click += new System.EventHandler(this.NextPageRequest);
            // 
            // _previousButton
            // 
            this._previousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._previousButton.Location = new System.Drawing.Point(673, 640);
            this._previousButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._previousButton.Name = "_previousButton";
            this._previousButton.Size = new System.Drawing.Size(88, 27);
            this._previousButton.TabIndex = 3;
            this._previousButton.Text = "< Back";
            this._previousButton.UseVisualStyleBackColor = true;
            this._previousButton.Click += new System.EventHandler(this.PreviousPageRequest);
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(862, 640);
            this._cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(88, 27);
            this._cancelButton.TabIndex = 4;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // CreateBackupTaskWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(964, 681);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._previousButton);
            this.Controls.Add(this._nextButton);
            this.Controls.Add(this._containerPanel);
            this.Controls.Add(this._panel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(980, 719);
            this.Name = "CreateBackupTaskWizardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Backup Task Wizard";
            this._panel.ResumeLayout(false);
            this._panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _panel;
        private System.Windows.Forms.Panel _containerPanel;
        private System.Windows.Forms.Button _nextButton;
        private System.Windows.Forms.Button _previousButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Label _titleLabel;
        private System.Windows.Forms.Label _descriptionLabel;
        private System.Windows.Forms.PictureBox _pictureBox;
    }
}