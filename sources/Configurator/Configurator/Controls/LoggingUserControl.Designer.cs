
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
            this._manageLogsLinkLabel = new System.Windows.Forms.LinkLabel();
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
            this.logsCaptionTableLayoutPanel.Location = new System.Drawing.Point(18, 192);
            this.logsCaptionTableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
            this.logsTypeTableLayoutPanel.Controls.Add(this._manageLogsLinkLabel, 1, 3);
            this.logsTypeTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsTypeTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.logsTypeTableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.logsTypeTableLayoutPanel.Name = "logsTypeTableLayoutPanel";
            this.logsTypeTableLayoutPanel.Padding = new System.Windows.Forms.Padding(16);
            this.logsTypeTableLayoutPanel.RowCount = 5;
            this.logsTypeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.logsTypeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.logsTypeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.logsTypeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.logsTypeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.logsTypeTableLayoutPanel.Size = new System.Drawing.Size(460, 215);
            this.logsTypeTableLayoutPanel.TabIndex = 0;
            // 
            // logsLocationLabel
            // 
            this.logsLocationLabel.AutoSize = true;
            this.logsLocationLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsLocationLabel.Location = new System.Drawing.Point(20, 16);
            this.logsLocationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.logsLocationLabel.Name = "logsLocationLabel";
            this.logsLocationLabel.Size = new System.Drawing.Size(81, 15);
            this.logsLocationLabel.TabIndex = 16;
            this.logsLocationLabel.Text = "Logs location:";
            this.logsLocationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // logsLocationLinkLabel
            // 
            this.logsLocationLinkLabel.AutoSize = true;
            this.logsLocationLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsLocationLinkLabel.Location = new System.Drawing.Point(109, 16);
            this.logsLocationLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.logsLocationLinkLabel.Name = "logsLocationLinkLabel";
            this.logsLocationLinkLabel.Size = new System.Drawing.Size(331, 15);
            this.logsLocationLinkLabel.TabIndex = 17;
            this.logsLocationLinkLabel.TabStop = true;
            this.logsLocationLinkLabel.Text = "<Log location>";
            this.logsLocationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenJournalsFolderLinkLabelLinkClicked);
            // 
            // restoreDefaultLogsLocationLinkLabel
            // 
            this.restoreDefaultLogsLocationLinkLabel.AutoSize = true;
            this.restoreDefaultLogsLocationLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.restoreDefaultLogsLocationLinkLabel.Location = new System.Drawing.Point(109, 31);
            this.restoreDefaultLogsLocationLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.restoreDefaultLogsLocationLinkLabel.Name = "restoreDefaultLogsLocationLinkLabel";
            this.restoreDefaultLogsLocationLinkLabel.Size = new System.Drawing.Size(331, 15);
            this.restoreDefaultLogsLocationLinkLabel.TabIndex = 18;
            this.restoreDefaultLogsLocationLinkLabel.TabStop = true;
            this.restoreDefaultLogsLocationLinkLabel.Text = "● Restore default logs location";
            this.restoreDefaultLogsLocationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RestoreDefaultLogsLocationLinkLabelLinkClicked);
            // 
            // chooseOtherLogsLocationLinkLabel
            // 
            this.chooseOtherLogsLocationLinkLabel.AutoSize = true;
            this.chooseOtherLogsLocationLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chooseOtherLogsLocationLinkLabel.Location = new System.Drawing.Point(109, 46);
            this.chooseOtherLogsLocationLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.chooseOtherLogsLocationLinkLabel.Name = "chooseOtherLogsLocationLinkLabel";
            this.chooseOtherLogsLocationLinkLabel.Size = new System.Drawing.Size(331, 15);
            this.chooseOtherLogsLocationLinkLabel.TabIndex = 19;
            this.chooseOtherLogsLocationLinkLabel.TabStop = true;
            this.chooseOtherLogsLocationLinkLabel.Text = "● Change logs location";
            this.chooseOtherLogsLocationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ChooseOtherLogsLocationLinkLabelLinkClicked);
            // 
            // _manageLogsLinkLabel
            // 
            this._manageLogsLinkLabel.AutoSize = true;
            this._manageLogsLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._manageLogsLinkLabel.Location = new System.Drawing.Point(109, 61);
            this._manageLogsLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._manageLogsLinkLabel.Name = "_manageLogsLinkLabel";
            this._manageLogsLinkLabel.Size = new System.Drawing.Size(331, 15);
            this._manageLogsLinkLabel.TabIndex = 20;
            this._manageLogsLinkLabel.TabStop = true;
            this._manageLogsLinkLabel.Text = "● View Logs...";
            this._manageLogsLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnManageLogsLinkLabelLinkClicked);
            // 
            // LoggingUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.logsTypeTableLayoutPanel);
            this.Controls.Add(this.logsCaptionTableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MinimumSize = new System.Drawing.Size(460, 215);
            this.Name = "LoggingUserControl";
            this.Size = new System.Drawing.Size(460, 215);
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
        private System.Windows.Forms.LinkLabel _manageLogsLinkLabel;
	}
}
