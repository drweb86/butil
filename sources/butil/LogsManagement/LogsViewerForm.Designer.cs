
namespace BUtil.Configurator.LogsManagement
{
	partial class LogsViewerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogsViewerForm));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Succesfull", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Erroneous", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Unknown", System.Windows.Forms.HorizontalAlignment.Left);
            this.journalsImageList = new System.Windows.Forms.ImageList(this.components);
            this.journalsListView = new System.Windows.Forms.ListView();
            this.journalsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.backupJournalsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewSelectedLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.removeSelectedLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.helpButton = new System.Windows.Forms.Button();
            this.openRecentLogButton = new System.Windows.Forms.Button();
            this.openLogsFolderButton = new System.Windows.Forms.Button();
            this.refreshLogsButton = new System.Windows.Forms.Button();
            this.removeSuccesfullLogsButton = new System.Windows.Forms.Button();
            this.deleteSelectedLogsButton = new System.Windows.Forms.Button();
            this.openSelectedLogsButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backupJournalsContextMenuStrip.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // journalsImageList
            // 
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
            this.journalsColumnHeader});
            this.journalsListView.ContextMenuStrip = this.backupJournalsContextMenuStrip;
            this.journalsListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.journalsListView.FullRowSelect = true;
            listViewGroup1.Header = "Succesfull";
            listViewGroup1.Name = "okListViewGroup";
            listViewGroup2.Header = "Erroneous";
            listViewGroup2.Name = "erroneousListViewGroup";
            listViewGroup3.Header = "Unknown";
            listViewGroup3.Name = "unknownListViewGroup";
            this.journalsListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.journalsListView.LabelWrap = false;
            this.journalsListView.LargeImageList = this.journalsImageList;
            this.journalsListView.Location = new System.Drawing.Point(3, 4);
            this.journalsListView.Name = "journalsListView";
            this.journalsListView.Size = new System.Drawing.Size(358, 501);
            this.journalsListView.SmallImageList = this.journalsImageList;
            this.journalsListView.TabIndex = 0;
            this.journalsListView.UseCompatibleStateImageBehavior = false;
            this.journalsListView.View = System.Windows.Forms.View.Details;
            this.journalsListView.SelectedIndexChanged += new System.EventHandler(this.updateLocalOperationsButtonsState);
            this.journalsListView.DoubleClick += new System.EventHandler(this.openSelectedLogsInBrowser);
            this.journalsListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.journalsListViewKeyDown);
            this.journalsListView.Resize += new System.EventHandler(this.listViewResize);
            // 
            // journalsColumnHeader
            // 
            this.journalsColumnHeader.Text = "Backup journals";
            this.journalsColumnHeader.Width = 300;
            // 
            // backupJournalsContextMenuStrip
            // 
            this.backupJournalsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewSelectedLogsToolStripMenuItem,
            this.toolStripSeparator1,
            this.removeSelectedLogsToolStripMenuItem});
            this.backupJournalsContextMenuStrip.Name = "backupJournalsContextMenuStrip";
            this.backupJournalsContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.backupJournalsContextMenuStrip.Size = new System.Drawing.Size(118, 54);
            // 
            // viewSelectedLogsToolStripMenuItem
            // 
            this.viewSelectedLogsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("viewSelectedLogsToolStripMenuItem.Image")));
            this.viewSelectedLogsToolStripMenuItem.Name = "viewSelectedLogsToolStripMenuItem";
            this.viewSelectedLogsToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.viewSelectedLogsToolStripMenuItem.Text = "View...";
            this.viewSelectedLogsToolStripMenuItem.Click += new System.EventHandler(this.openSelectedLogsInBrowser);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(114, 6);
            // 
            // removeSelectedLogsToolStripMenuItem
            // 
            this.removeSelectedLogsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeSelectedLogsToolStripMenuItem.Image")));
            this.removeSelectedLogsToolStripMenuItem.Name = "removeSelectedLogsToolStripMenuItem";
            this.removeSelectedLogsToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeSelectedLogsToolStripMenuItem.Text = "Remove";
            this.removeSelectedLogsToolStripMenuItem.Click += new System.EventHandler(this.removeSelected);
            // 
            // rightPanel
            // 
            this.rightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rightPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rightPanel.BackgroundImage")));
            this.rightPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rightPanel.Controls.Add(this.helpButton);
            this.rightPanel.Controls.Add(this.openRecentLogButton);
            this.rightPanel.Controls.Add(this.openLogsFolderButton);
            this.rightPanel.Controls.Add(this.refreshLogsButton);
            this.rightPanel.Controls.Add(this.removeSuccesfullLogsButton);
            this.rightPanel.Controls.Add(this.deleteSelectedLogsButton);
            this.rightPanel.Controls.Add(this.openSelectedLogsButton);
            this.rightPanel.Location = new System.Drawing.Point(366, 4);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(76, 501);
            this.rightPanel.TabIndex = 4;
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.helpButton.Image = global::BUtil.Configurator.Icons.Help48x48;
            this.helpButton.Location = new System.Drawing.Point(9, 434);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(64, 64);
            this.helpButton.TabIndex = 10;
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.helpButtonClick);
            // 
            // openRecentLogButton
            // 
            this.openRecentLogButton.Image = ((System.Drawing.Image)(resources.GetObject("openRecentLogButton.Image")));
            this.openRecentLogButton.Location = new System.Drawing.Point(9, 357);
            this.openRecentLogButton.Name = "openRecentLogButton";
            this.openRecentLogButton.Size = new System.Drawing.Size(64, 64);
            this.openRecentLogButton.TabIndex = 5;
            this.openRecentLogButton.UseVisualStyleBackColor = true;
            this.openRecentLogButton.Click += new System.EventHandler(this.openTheRecentLog);
            // 
            // openLogsFolderButton
            // 
            this.openLogsFolderButton.Image = global::BUtil.Configurator.Icons.RedFolder48x48;
            this.openLogsFolderButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openLogsFolderButton.Location = new System.Drawing.Point(9, 287);
            this.openLogsFolderButton.Name = "openLogsFolderButton";
            this.openLogsFolderButton.Size = new System.Drawing.Size(64, 64);
            this.openLogsFolderButton.TabIndex = 4;
            this.openLogsFolderButton.UseVisualStyleBackColor = true;
            this.openLogsFolderButton.Click += new System.EventHandler(this.openLogsFolderInExplorer);
            // 
            // refreshLogsButton
            // 
            this.refreshLogsButton.Image = global::BUtil.Configurator.Icons.Refresh48x48;
            this.refreshLogsButton.Location = new System.Drawing.Point(9, 217);
            this.refreshLogsButton.Name = "refreshLogsButton";
            this.refreshLogsButton.Size = new System.Drawing.Size(64, 64);
            this.refreshLogsButton.TabIndex = 3;
            this.refreshLogsButton.UseVisualStyleBackColor = true;
            this.refreshLogsButton.Click += new System.EventHandler(this.refresh);
            // 
            // removeSuccesfullLogsButton
            // 
            this.removeSuccesfullLogsButton.Image = ((System.Drawing.Image)(resources.GetObject("removeSuccesfullLogsButton.Image")));
            this.removeSuccesfullLogsButton.Location = new System.Drawing.Point(9, 147);
            this.removeSuccesfullLogsButton.Name = "removeSuccesfullLogsButton";
            this.removeSuccesfullLogsButton.Size = new System.Drawing.Size(64, 64);
            this.removeSuccesfullLogsButton.TabIndex = 2;
            this.removeSuccesfullLogsButton.UseVisualStyleBackColor = true;
            this.removeSuccesfullLogsButton.Click += new System.EventHandler(this.removeSuccessfullLogs);
            // 
            // deleteSelectedLogsButton
            // 
            this.deleteSelectedLogsButton.Image = global::BUtil.Configurator.Icons.cross_48;
            this.deleteSelectedLogsButton.Location = new System.Drawing.Point(9, 77);
            this.deleteSelectedLogsButton.Name = "deleteSelectedLogsButton";
            this.deleteSelectedLogsButton.Size = new System.Drawing.Size(64, 64);
            this.deleteSelectedLogsButton.TabIndex = 1;
            this.deleteSelectedLogsButton.UseVisualStyleBackColor = true;
            this.deleteSelectedLogsButton.Click += new System.EventHandler(this.removeSelected);
            // 
            // openSelectedLogsButton
            // 
            this.openSelectedLogsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold);
            this.openSelectedLogsButton.ForeColor = System.Drawing.Color.Blue;
            this.openSelectedLogsButton.Image = global::BUtil.Configurator.Icons.OpenSelectedLogs;
            this.openSelectedLogsButton.Location = new System.Drawing.Point(9, 7);
            this.openSelectedLogsButton.Name = "openSelectedLogsButton";
            this.openSelectedLogsButton.Size = new System.Drawing.Size(64, 64);
            this.openSelectedLogsButton.TabIndex = 0;
            this.openSelectedLogsButton.UseVisualStyleBackColor = true;
            this.openSelectedLogsButton.Click += new System.EventHandler(this.openSelectedLogsInBrowser);
            // 
            // LogsViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 506);
            this.Controls.Add(this.journalsListView);
            this.Controls.Add(this.rightPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(452, 540);
            this.Name = "LogsViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Backup Journals";
            this.backupJournalsContextMenuStrip.ResumeLayout(false);
            this.rightPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		private System.Windows.Forms.ToolStripMenuItem removeSelectedLogsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem viewSelectedLogsToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip backupJournalsContextMenuStrip;
		private System.Windows.Forms.ColumnHeader journalsColumnHeader;
		private System.Windows.Forms.ListView journalsListView;
        private System.Windows.Forms.ImageList journalsImageList;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Button openLogsFolderButton;
        private System.Windows.Forms.Button refreshLogsButton;
        private System.Windows.Forms.Button removeSuccesfullLogsButton;
        private System.Windows.Forms.Button deleteSelectedLogsButton;
        private System.Windows.Forms.Button openSelectedLogsButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button openRecentLogButton;
        private System.Windows.Forms.Button helpButton;
	}
}
