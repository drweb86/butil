
namespace BUtil.Configurator.LogsManagement
{
	partial class LogsListUserControl
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogsListUserControl));
            this.journalsImageList = new System.Windows.Forms.ImageList(this.components);
            this.journalsListView = new System.Windows.Forms.ListView();
            this._logsColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.backupJournalsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewSelectedLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.removeSelectedLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshLogsButton = new System.Windows.Forms.Button();
            this.removeSuccesfullLogsButton = new System.Windows.Forms.Button();
            this.deleteSelectedLogsButton = new System.Windows.Forms.Button();
            this.openSelectedLogsButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backupJournalsContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // journalsImageList
            // 
            this.journalsImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.journalsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("journalsImageList.ImageStream")));
            this.journalsImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.journalsImageList.Images.SetKeyName(0, "OkLogState.PNG");
            this.journalsImageList.Images.SetKeyName(1, "BadLogState.PNG");
            this.journalsImageList.Images.SetKeyName(2, "UnknownLogState.PNG");
            // 
            // journalsListView
            // 
            this.journalsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.journalsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._logsColumnHeader});
            this.journalsListView.ContextMenuStrip = this.backupJournalsContextMenuStrip;
            this.journalsListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.journalsListView.FullRowSelect = true;
            this.journalsListView.LabelWrap = false;
            this.journalsListView.LargeImageList = this.journalsImageList;
            this.journalsListView.Location = new System.Drawing.Point(4, 6);
            this.journalsListView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.journalsListView.Name = "journalsListView";
            this.journalsListView.Size = new System.Drawing.Size(337, 432);
            this.journalsListView.SmallImageList = this.journalsImageList;
            this.journalsListView.TabIndex = 0;
            this.journalsListView.UseCompatibleStateImageBehavior = false;
            this.journalsListView.View = System.Windows.Forms.View.Details;
            this.journalsListView.SelectedIndexChanged += new System.EventHandler(this.updateLocalOperationsButtonsState);
            this.journalsListView.DoubleClick += new System.EventHandler(this.openSelectedLogsInBrowser);
            this.journalsListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.journalsListViewKeyDown);
            this.journalsListView.Resize += new System.EventHandler(this.listViewResize);
            // 
            // _logsColumnHeader
            // 
            this._logsColumnHeader.Text = "Logs";
            this._logsColumnHeader.Width = 300;
            // 
            // backupJournalsContextMenuStrip
            // 
            this.backupJournalsContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.backupJournalsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewSelectedLogsToolStripMenuItem,
            this.toolStripSeparator1,
            this.removeSelectedLogsToolStripMenuItem});
            this.backupJournalsContextMenuStrip.Name = "backupJournalsContextMenuStrip";
            this.backupJournalsContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.backupJournalsContextMenuStrip.Size = new System.Drawing.Size(137, 62);
            // 
            // viewSelectedLogsToolStripMenuItem
            // 
            this.viewSelectedLogsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("viewSelectedLogsToolStripMenuItem.Image")));
            this.viewSelectedLogsToolStripMenuItem.Name = "viewSelectedLogsToolStripMenuItem";
            this.viewSelectedLogsToolStripMenuItem.Size = new System.Drawing.Size(136, 26);
            this.viewSelectedLogsToolStripMenuItem.Text = "View...";
            this.viewSelectedLogsToolStripMenuItem.Click += new System.EventHandler(this.openSelectedLogsInBrowser);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            // 
            // removeSelectedLogsToolStripMenuItem
            // 
            this.removeSelectedLogsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeSelectedLogsToolStripMenuItem.Image")));
            this.removeSelectedLogsToolStripMenuItem.Name = "removeSelectedLogsToolStripMenuItem";
            this.removeSelectedLogsToolStripMenuItem.Size = new System.Drawing.Size(136, 26);
            this.removeSelectedLogsToolStripMenuItem.Text = "Remove";
            this.removeSelectedLogsToolStripMenuItem.Click += new System.EventHandler(this.removeSelected);
            // 
            // refreshLogsButton
            // 
            this.refreshLogsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshLogsButton.Image = global::BUtil.Configurator.Icons.Refresh48x48;
            this.refreshLogsButton.Location = new System.Drawing.Point(349, 333);
            this.refreshLogsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.refreshLogsButton.Name = "refreshLogsButton";
            this.refreshLogsButton.Size = new System.Drawing.Size(85, 98);
            this.refreshLogsButton.TabIndex = 3;
            this.refreshLogsButton.UseVisualStyleBackColor = true;
            this.refreshLogsButton.Click += new System.EventHandler(this.refresh);
            // 
            // removeSuccesfullLogsButton
            // 
            this.removeSuccesfullLogsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeSuccesfullLogsButton.Image = ((System.Drawing.Image)(resources.GetObject("removeSuccesfullLogsButton.Image")));
            this.removeSuccesfullLogsButton.Location = new System.Drawing.Point(349, 225);
            this.removeSuccesfullLogsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.removeSuccesfullLogsButton.Name = "removeSuccesfullLogsButton";
            this.removeSuccesfullLogsButton.Size = new System.Drawing.Size(85, 98);
            this.removeSuccesfullLogsButton.TabIndex = 2;
            this.removeSuccesfullLogsButton.UseVisualStyleBackColor = true;
            this.removeSuccesfullLogsButton.Click += new System.EventHandler(this.removeSuccessfullLogs);
            // 
            // deleteSelectedLogsButton
            // 
            this.deleteSelectedLogsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteSelectedLogsButton.Image = global::BUtil.Configurator.Icons.cross_48;
            this.deleteSelectedLogsButton.Location = new System.Drawing.Point(349, 117);
            this.deleteSelectedLogsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.deleteSelectedLogsButton.Name = "deleteSelectedLogsButton";
            this.deleteSelectedLogsButton.Size = new System.Drawing.Size(85, 98);
            this.deleteSelectedLogsButton.TabIndex = 1;
            this.deleteSelectedLogsButton.UseVisualStyleBackColor = true;
            this.deleteSelectedLogsButton.Click += new System.EventHandler(this.removeSelected);
            // 
            // openSelectedLogsButton
            // 
            this.openSelectedLogsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openSelectedLogsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.openSelectedLogsButton.ForeColor = System.Drawing.Color.Blue;
            this.openSelectedLogsButton.Image = global::BUtil.Configurator.Icons.OpenSelectedLogs;
            this.openSelectedLogsButton.Location = new System.Drawing.Point(349, 10);
            this.openSelectedLogsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.openSelectedLogsButton.Name = "openSelectedLogsButton";
            this.openSelectedLogsButton.Size = new System.Drawing.Size(85, 98);
            this.openSelectedLogsButton.TabIndex = 0;
            this.openSelectedLogsButton.UseVisualStyleBackColor = true;
            this.openSelectedLogsButton.Click += new System.EventHandler(this.openSelectedLogsInBrowser);
            // 
            // LogsViewerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.refreshLogsButton);
            this.Controls.Add(this.removeSuccesfullLogsButton);
            this.Controls.Add(this.journalsListView);
            this.Controls.Add(this.deleteSelectedLogsButton);
            this.Controls.Add(this.openSelectedLogsButton);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(438, 435);
            this.Name = "LogsViewerUserControl";
            this.Size = new System.Drawing.Size(438, 447);
            this.backupJournalsContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		private System.Windows.Forms.ToolStripMenuItem removeSelectedLogsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem viewSelectedLogsToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip backupJournalsContextMenuStrip;
		private System.Windows.Forms.ColumnHeader _logsColumnHeader;
		private System.Windows.Forms.ListView journalsListView;
        private System.Windows.Forms.ImageList journalsImageList;
        private System.Windows.Forms.Button refreshLogsButton;
        private System.Windows.Forms.Button removeSuccesfullLogsButton;
        private System.Windows.Forms.Button deleteSelectedLogsButton;
        private System.Windows.Forms.Button openSelectedLogsButton;
        private System.Windows.Forms.ToolTip toolTip;
	}
}
