
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
            this.logsLocationLabel = new System.Windows.Forms.Label();
            this.logsLocationLinkLabel = new System.Windows.Forms.LinkLabel();
            this._resetLogsLocationLinkLabel = new System.Windows.Forms.LinkLabel();
            this._changeLogsLocationLinkLabel = new System.Windows.Forms.LinkLabel();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
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
            this.logsCaptionTableLayoutPanel.Location = new System.Drawing.Point(21, 256);
            this.logsCaptionTableLayoutPanel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.logsCaptionTableLayoutPanel.Name = "logsCaptionTableLayoutPanel";
            this.logsCaptionTableLayoutPanel.RowCount = 1;
            this.logsCaptionTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.logsCaptionTableLayoutPanel.Size = new System.Drawing.Size(0, 0);
            this.logsCaptionTableLayoutPanel.TabIndex = 3;
            // 
            // logsLocationLabel
            // 
            this.logsLocationLabel.AutoSize = true;
            this.logsLocationLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsLocationLabel.Location = new System.Drawing.Point(5, 5);
            this.logsLocationLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.logsLocationLabel.Name = "logsLocationLabel";
            this.logsLocationLabel.Size = new System.Drawing.Size(101, 20);
            this.logsLocationLabel.TabIndex = 16;
            this.logsLocationLabel.Text = "Logs location:";
            this.logsLocationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // logsLocationLinkLabel
            // 
            this.logsLocationLinkLabel.AutoSize = true;
            this.logsLocationLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsLocationLinkLabel.Location = new System.Drawing.Point(116, 5);
            this.logsLocationLinkLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.logsLocationLinkLabel.Name = "logsLocationLinkLabel";
            this.logsLocationLinkLabel.Size = new System.Drawing.Size(112, 20);
            this.logsLocationLinkLabel.TabIndex = 17;
            this.logsLocationLinkLabel.TabStop = true;
            this.logsLocationLinkLabel.Text = "<Log location>";
            this.logsLocationLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.logsLocationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenJournalsFolderLinkLabelLinkClicked);
            // 
            // _resetLogsLocationLinkLabel
            // 
            this._resetLogsLocationLinkLabel.AutoSize = true;
            this._resetLogsLocationLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._resetLogsLocationLinkLabel.Location = new System.Drawing.Point(238, 5);
            this._resetLogsLocationLinkLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this._resetLogsLocationLinkLabel.Name = "_resetLogsLocationLinkLabel";
            this._resetLogsLocationLinkLabel.Size = new System.Drawing.Size(45, 20);
            this._resetLogsLocationLinkLabel.TabIndex = 18;
            this._resetLogsLocationLinkLabel.TabStop = true;
            this._resetLogsLocationLinkLabel.Text = "Reset";
            this._resetLogsLocationLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._resetLogsLocationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RestoreDefaultLogsLocationLinkLabelLinkClicked);
            // 
            // _changeLogsLocationLinkLabel
            // 
            this._changeLogsLocationLinkLabel.AutoSize = true;
            this._changeLogsLocationLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._changeLogsLocationLinkLabel.Location = new System.Drawing.Point(293, 5);
            this._changeLogsLocationLinkLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this._changeLogsLocationLinkLabel.Name = "_changeLogsLocationLinkLabel";
            this._changeLogsLocationLinkLabel.Size = new System.Drawing.Size(68, 20);
            this._changeLogsLocationLinkLabel.TabIndex = 19;
            this._changeLogsLocationLinkLabel.TabStop = true;
            this._changeLogsLocationLinkLabel.Text = "Change...";
            this._changeLogsLocationLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._changeLogsLocationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ChooseOtherLogsLocationLinkLabelLinkClicked);
            // 
            // _settingsTableLayoutPanel
            // 
            this._settingsTableLayoutPanel.AutoSize = true;
            this._settingsTableLayoutPanel.ColumnCount = 5;
            this._settingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._settingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._settingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._settingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._settingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._settingsTableLayoutPanel.Controls.Add(this._changeLogsLocationLinkLabel, 3, 1);
            this._settingsTableLayoutPanel.Controls.Add(this._resetLogsLocationLinkLabel, 2, 1);
            this._settingsTableLayoutPanel.Controls.Add(this.logsLocationLinkLabel, 1, 1);
            this._settingsTableLayoutPanel.Controls.Add(this.logsLocationLabel, 0, 1);
            this._settingsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._settingsTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._settingsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this._settingsTableLayoutPanel.Name = "_settingsTableLayoutPanel";
            this._settingsTableLayoutPanel.RowCount = 3;
            this._settingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this._settingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._settingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this._settingsTableLayoutPanel.Size = new System.Drawing.Size(677, 30);
            this._settingsTableLayoutPanel.TabIndex = 4;
            // 
            // logsListUserControl1
            // 
            this.logsListUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsListUserControl1.Location = new System.Drawing.Point(0, 30);
            this.logsListUserControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.logsListUserControl1.MinimumSize = new System.Drawing.Size(438, 435);
            this.logsListUserControl1.Name = "logsListUserControl1";
            this.logsListUserControl1.Size = new System.Drawing.Size(677, 899);
            this.logsListUserControl1.TabIndex = 5;
            // 
            // LogsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.logsListUserControl1);
            this.Controls.Add(this._settingsTableLayoutPanel);
            this.Controls.Add(this.logsCaptionTableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.MinimumSize = new System.Drawing.Size(526, 287);
            this.Name = "LogsUserControl";
            this.Size = new System.Drawing.Size(677, 929);
            this._settingsTableLayoutPanel.ResumeLayout(false);
            this._settingsTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.FolderBrowserDialog fbd;
		private System.Windows.Forms.LinkLabel _resetLogsLocationLinkLabel;
		private System.Windows.Forms.LinkLabel _changeLogsLocationLinkLabel;
		private System.Windows.Forms.LinkLabel logsLocationLinkLabel;
		private System.Windows.Forms.Label logsLocationLabel;
		private System.Windows.Forms.TableLayoutPanel logsCaptionTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel _settingsTableLayoutPanel;
        private LogsListUserControl logsListUserControl1;
    }
}
