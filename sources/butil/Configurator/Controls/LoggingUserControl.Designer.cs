
using BUtil.Configurator.LogsManagement;

namespace BUtil.Configurator.Configurator.Controls
{
	partial class LoggingUserControl
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
            this.logsTypeTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.logsLocationLabel = new System.Windows.Forms.Label();
            this.logsLocationLinkLabel = new System.Windows.Forms.LinkLabel();
            this.restoreDefaultLogsLocationLinkLabel = new System.Windows.Forms.LinkLabel();
            this.chooseOtherLogsLocationLinkLabel = new System.Windows.Forms.LinkLabel();
            this.logsViewerUserControl1 = new BUtil.Configurator.LogsManagement.LogsViewerUserControl();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.logsTypeTableLayoutPanel.SuspendLayout();
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
            // logsTypeTableLayoutPanel
            // 
            this.logsTypeTableLayoutPanel.AutoSize = true;
            this.logsTypeTableLayoutPanel.ColumnCount = 2;
            this.logsTypeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.logsTypeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.logsTypeTableLayoutPanel.Controls.Add(this.logsLocationLabel, 0, 0);
            this.logsTypeTableLayoutPanel.Controls.Add(this.logsLocationLinkLabel, 1, 0);
            this.logsTypeTableLayoutPanel.Controls.Add(this.restoreDefaultLogsLocationLinkLabel, 1, 1);
            this.logsTypeTableLayoutPanel.Controls.Add(this.chooseOtherLogsLocationLinkLabel, 1, 2);
            this.logsTypeTableLayoutPanel.Controls.Add(this.logsViewerUserControl1, 0, 4);
            this.logsTypeTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsTypeTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.logsTypeTableLayoutPanel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.logsTypeTableLayoutPanel.Name = "logsTypeTableLayoutPanel";
            this.logsTypeTableLayoutPanel.Padding = new System.Windows.Forms.Padding(18, 21, 18, 21);
            this.logsTypeTableLayoutPanel.RowCount = 5;
            this.logsTypeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.logsTypeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.logsTypeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.logsTypeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.logsTypeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.logsTypeTableLayoutPanel.Size = new System.Drawing.Size(677, 929);
            this.logsTypeTableLayoutPanel.TabIndex = 0;
            // 
            // logsLocationLabel
            // 
            this.logsLocationLabel.AutoSize = true;
            this.logsLocationLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsLocationLabel.Location = new System.Drawing.Point(23, 21);
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
            this.logsLocationLinkLabel.Location = new System.Drawing.Point(134, 21);
            this.logsLocationLinkLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.logsLocationLinkLabel.Name = "logsLocationLinkLabel";
            this.logsLocationLinkLabel.Size = new System.Drawing.Size(520, 20);
            this.logsLocationLinkLabel.TabIndex = 17;
            this.logsLocationLinkLabel.TabStop = true;
            this.logsLocationLinkLabel.Text = "<Log location>";
            this.logsLocationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenJournalsFolderLinkLabelLinkClicked);
            // 
            // restoreDefaultLogsLocationLinkLabel
            // 
            this.restoreDefaultLogsLocationLinkLabel.AutoSize = true;
            this.restoreDefaultLogsLocationLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.restoreDefaultLogsLocationLinkLabel.Location = new System.Drawing.Point(134, 41);
            this.restoreDefaultLogsLocationLinkLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.restoreDefaultLogsLocationLinkLabel.Name = "restoreDefaultLogsLocationLinkLabel";
            this.restoreDefaultLogsLocationLinkLabel.Size = new System.Drawing.Size(520, 20);
            this.restoreDefaultLogsLocationLinkLabel.TabIndex = 18;
            this.restoreDefaultLogsLocationLinkLabel.TabStop = true;
            this.restoreDefaultLogsLocationLinkLabel.Text = "● Restore default logs location";
            this.restoreDefaultLogsLocationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RestoreDefaultLogsLocationLinkLabelLinkClicked);
            // 
            // chooseOtherLogsLocationLinkLabel
            // 
            this.chooseOtherLogsLocationLinkLabel.AutoSize = true;
            this.chooseOtherLogsLocationLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chooseOtherLogsLocationLinkLabel.Location = new System.Drawing.Point(134, 61);
            this.chooseOtherLogsLocationLinkLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.chooseOtherLogsLocationLinkLabel.Name = "chooseOtherLogsLocationLinkLabel";
            this.chooseOtherLogsLocationLinkLabel.Size = new System.Drawing.Size(520, 20);
            this.chooseOtherLogsLocationLinkLabel.TabIndex = 19;
            this.chooseOtherLogsLocationLinkLabel.TabStop = true;
            this.chooseOtherLogsLocationLinkLabel.Text = "● Change logs location";
            this.chooseOtherLogsLocationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ChooseOtherLogsLocationLinkLabelLinkClicked);
            // 
            // logsViewerUserControl1
            // 
            this.logsTypeTableLayoutPanel.SetColumnSpan(this.logsViewerUserControl1, 2);
            this.logsViewerUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsViewerUserControl1.Location = new System.Drawing.Point(22, 86);
            this.logsViewerUserControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.logsViewerUserControl1.MinimumSize = new System.Drawing.Size(603, 831);
            this.logsViewerUserControl1.Name = "logsViewerUserControl1";
            this.logsViewerUserControl1.Size = new System.Drawing.Size(633, 831);
            this.logsViewerUserControl1.TabIndex = 20;
            // 
            // LoggingUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.logsTypeTableLayoutPanel);
            this.Controls.Add(this.logsCaptionTableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.MinimumSize = new System.Drawing.Size(526, 287);
            this.Name = "LoggingUserControl";
            this.Size = new System.Drawing.Size(677, 929);
            this.logsTypeTableLayoutPanel.ResumeLayout(false);
            this.logsTypeTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.FolderBrowserDialog fbd;
		private System.Windows.Forms.LinkLabel restoreDefaultLogsLocationLinkLabel;
		private System.Windows.Forms.LinkLabel chooseOtherLogsLocationLinkLabel;
		private System.Windows.Forms.LinkLabel logsLocationLinkLabel;
		private System.Windows.Forms.Label logsLocationLabel;
		private System.Windows.Forms.TableLayoutPanel logsTypeTableLayoutPanel;
		private System.Windows.Forms.TableLayoutPanel logsCaptionTableLayoutPanel;
        private LogsViewerUserControl logsViewerUserControl1;
    }
}
