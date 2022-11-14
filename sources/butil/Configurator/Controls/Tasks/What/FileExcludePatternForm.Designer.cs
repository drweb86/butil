namespace BUtil.Configurator.Configurator.Controls.Tasks.What
{
    partial class FileExcludePatternForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileExcludePatternForm));
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._patternLabel = new System.Windows.Forms.Label();
            this._patternTextBox = new System.Windows.Forms.TextBox();
            this._descriptionLabel = new System.Windows.Forms.Label();
            this._formatLinkLabel = new System.Windows.Forms.LinkLabel();
            this._okCancelTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._tableLayoutPanel.SuspendLayout();
            this._okCancelTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.AutoSize = true;
            this._tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableLayoutPanel.ColumnCount = 2;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.Controls.Add(this._patternLabel, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._patternTextBox, 1, 0);
            this._tableLayoutPanel.Controls.Add(this._descriptionLabel, 0, 1);
            this._tableLayoutPanel.Controls.Add(this._formatLinkLabel, 0, 2);
            this._tableLayoutPanel.Controls.Add(this._okCancelTableLayoutPanel, 1, 3);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.Padding = new System.Windows.Forms.Padding(8);
            this._tableLayoutPanel.RowCount = 5;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.Size = new System.Drawing.Size(800, 172);
            this._tableLayoutPanel.TabIndex = 0;
            // 
            // _patternLabel
            // 
            this._patternLabel.AutoSize = true;
            this._patternLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._patternLabel.Location = new System.Drawing.Point(11, 8);
            this._patternLabel.Name = "_patternLabel";
            this._patternLabel.Size = new System.Drawing.Size(58, 33);
            this._patternLabel.TabIndex = 0;
            this._patternLabel.Text = "Pattern:";
            this._patternLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _patternTextBox
            // 
            this._patternTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._patternTextBox.Location = new System.Drawing.Point(75, 11);
            this._patternTextBox.Name = "_patternTextBox";
            this._patternTextBox.Size = new System.Drawing.Size(714, 27);
            this._patternTextBox.TabIndex = 1;
            // 
            // _descriptionLabel
            // 
            this._descriptionLabel.AutoSize = true;
            this._tableLayoutPanel.SetColumnSpan(this._descriptionLabel, 2);
            this._descriptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._descriptionLabel.Location = new System.Drawing.Point(11, 41);
            this._descriptionLabel.Name = "_descriptionLabel";
            this._descriptionLabel.Size = new System.Drawing.Size(778, 20);
            this._descriptionLabel.TabIndex = 2;
            this._descriptionLabel.Text = "Description";
            // 
            // _formatLinkLabel
            // 
            this._formatLinkLabel.AutoSize = true;
            this._tableLayoutPanel.SetColumnSpan(this._formatLinkLabel, 2);
            this._formatLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._formatLinkLabel.Location = new System.Drawing.Point(11, 61);
            this._formatLinkLabel.Name = "_formatLinkLabel";
            this._formatLinkLabel.Size = new System.Drawing.Size(778, 20);
            this._formatLinkLabel.TabIndex = 3;
            this._formatLinkLabel.TabStop = true;
            this._formatLinkLabel.Text = "Link";
            this._formatLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // _okCancelTableLayoutPanel
            // 
            this._okCancelTableLayoutPanel.AutoSize = true;
            this._okCancelTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._okCancelTableLayoutPanel.ColumnCount = 2;
            this._okCancelTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._okCancelTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._okCancelTableLayoutPanel.Controls.Add(this._okButton, 0, 0);
            this._okCancelTableLayoutPanel.Controls.Add(this._cancelButton, 1, 0);
            this._okCancelTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this._okCancelTableLayoutPanel.Location = new System.Drawing.Point(596, 84);
            this._okCancelTableLayoutPanel.Name = "_okCancelTableLayoutPanel";
            this._okCancelTableLayoutPanel.RowCount = 1;
            this._okCancelTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._okCancelTableLayoutPanel.Size = new System.Drawing.Size(193, 36);
            this._okCancelTableLayoutPanel.TabIndex = 3;
            // 
            // _okButton
            // 
            this._okButton.AutoSize = true;
            this._okButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._okButton.Location = new System.Drawing.Point(3, 3);
            this._okButton.MinimumSize = new System.Drawing.Size(87, 29);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(94, 30);
            this._okButton.TabIndex = 1;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this.OnOkButtonClick);
            // 
            // _cancelButton
            // 
            this._cancelButton.AutoSize = true;
            this._cancelButton.Dock = System.Windows.Forms.DockStyle.Left;
            this._cancelButton.Location = new System.Drawing.Point(103, 3);
            this._cancelButton.MinimumSize = new System.Drawing.Size(87, 29);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(87, 30);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this.OnCancelButtonClick);
            // 
            // FileExcludePatternForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 172);
            this.Controls.Add(this._tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileExcludePatternForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add/Edit Ignore";
            this._tableLayoutPanel.ResumeLayout(false);
            this._tableLayoutPanel.PerformLayout();
            this._okCancelTableLayoutPanel.ResumeLayout(false);
            this._okCancelTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.Label _patternLabel;
        private System.Windows.Forms.TextBox _patternTextBox;
        private System.Windows.Forms.Label _descriptionLabel;
        private System.Windows.Forms.LinkLabel _formatLinkLabel;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.TableLayoutPanel _okCancelTableLayoutPanel;
    }
}