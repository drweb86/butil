
using BUtil.Configurator.LogsManagement;

namespace BUtil.Configurator.Configurator.Controls
{
	partial class LogsUserControl
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
            this.logsCaptionTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.logsLocationLinkLabel = new System.Windows.Forms.LinkLabel();
            this._settingsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.logsListUserControl1 = new BUtil.Configurator.LogsManagement.LogsListUserControl();
            this._settingsTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // logsCaptionTableLayoutPanel
            // 
            this.logsCaptionTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logsCaptionTableLayoutPanel.AutoSize = true;
            this.logsCaptionTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.logsCaptionTableLayoutPanel.ColumnCount = 3;
            this.logsCaptionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.logsCaptionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.logsCaptionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.logsCaptionTableLayoutPanel.Location = new System.Drawing.Point(18, 192);
            this.logsCaptionTableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.logsCaptionTableLayoutPanel.Name = "logsCaptionTableLayoutPanel";
            this.logsCaptionTableLayoutPanel.RowCount = 1;
            this.logsCaptionTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.logsCaptionTableLayoutPanel.Size = new System.Drawing.Size(0, 0);
            this.logsCaptionTableLayoutPanel.TabIndex = 3;
            // 
            // logsLocationLinkLabel
            // 
            this.logsLocationLinkLabel.AutoSize = true;
            this.logsLocationLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsLocationLinkLabel.Location = new System.Drawing.Point(4, 4);
            this.logsLocationLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.logsLocationLinkLabel.Name = "logsLocationLinkLabel";
            this.logsLocationLinkLabel.Size = new System.Drawing.Size(89, 15);
            this.logsLocationLinkLabel.TabIndex = 17;
            this.logsLocationLinkLabel.TabStop = true;
            this.logsLocationLinkLabel.Text = "<Log location>";
            this.logsLocationLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.logsLocationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenJournalsFolderLinkLabelLinkClicked);
            // 
            // _settingsTableLayoutPanel
            // 
            this._settingsTableLayoutPanel.AutoSize = true;
            this._settingsTableLayoutPanel.ColumnCount = 2;
            this._settingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._settingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._settingsTableLayoutPanel.Controls.Add(this.logsLocationLinkLabel, 0, 1);
            this._settingsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._settingsTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._settingsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 11, 3, 2);
            this._settingsTableLayoutPanel.Name = "_settingsTableLayoutPanel";
            this._settingsTableLayoutPanel.RowCount = 3;
            this._settingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this._settingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._settingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this._settingsTableLayoutPanel.Size = new System.Drawing.Size(592, 23);
            this._settingsTableLayoutPanel.TabIndex = 4;
            // 
            // logsListUserControl1
            // 
            this.logsListUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsListUserControl1.Location = new System.Drawing.Point(0, 23);
            this.logsListUserControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.logsListUserControl1.MinimumSize = new System.Drawing.Size(383, 326);
            this.logsListUserControl1.Name = "logsListUserControl1";
            this.logsListUserControl1.Size = new System.Drawing.Size(592, 674);
            this.logsListUserControl1.TabIndex = 5;
            // 
            // LogsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.logsListUserControl1);
            this.Controls.Add(this._settingsTableLayoutPanel);
            this.Controls.Add(this.logsCaptionTableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MinimumSize = new System.Drawing.Size(460, 215);
            this.Name = "LogsUserControl";
            this.Size = new System.Drawing.Size(592, 697);
            this._settingsTableLayoutPanel.ResumeLayout(false);
            this._settingsTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.LinkLabel logsLocationLinkLabel;
		private System.Windows.Forms.TableLayoutPanel logsCaptionTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel _settingsTableLayoutPanel;
        private LogsListUserControl logsListUserControl1;
    }
}
