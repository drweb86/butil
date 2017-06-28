
namespace BUtil.Configurator.Controls
{
	partial class SourceItemsUserControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourceItemsUserControl));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Store compression", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Fastest compression", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Fast compression", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Normal compression", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Maximum compression", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Ultra compression", System.Windows.Forms.HorizontalAlignment.Left);
            this.ofd = new System.Windows.Forms.FolderBrowserDialog();
            this.itemsToCompressImageList = new System.Windows.Forms.ImageList(this.components);
            this.filesFoldersContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setCompressionDegreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.storeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fastestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maximumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFromListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.addFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressionItemsListView = new System.Windows.Forms.ListView();
            this.itemsToCompressColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.addFoldersButton = new System.Windows.Forms.Button();
            this.removeCompressionItemButton = new System.Windows.Forms.Button();
            this.addFilesButton = new System.Windows.Forms.Button();
            this.filesFoldersContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // itemsToCompressImageList
            // 
            this.itemsToCompressImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("itemsToCompressImageList.ImageStream")));
            this.itemsToCompressImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.itemsToCompressImageList.Images.SetKeyName(0, "folder_48.png");
            this.itemsToCompressImageList.Images.SetKeyName(1, "AddFiles48x48.ico");
            // 
            // filesFoldersContextMenuStrip
            // 
            this.filesFoldersContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setCompressionDegreeToolStripMenuItem,
            this.removeFromListToolStripMenuItem,
            this.toolStripSeparator6,
            this.addFilesToolStripMenuItem,
            this.addFoldersToolStripMenuItem});
            this.filesFoldersContextMenuStrip.Name = "filesFoldersContextMenuStrip";
            this.filesFoldersContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.filesFoldersContextMenuStrip.Size = new System.Drawing.Size(201, 98);
            this.filesFoldersContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.filesFoldersContextMenuStripOpening);
            // 
            // setCompressionDegreeToolStripMenuItem
            // 
            this.setCompressionDegreeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.storeToolStripMenuItem,
            this.fastestToolStripMenuItem,
            this.fastToolStripMenuItem,
            this.normalToolStripMenuItem,
            this.maximumToolStripMenuItem,
            this.ultraToolStripMenuItem});
            this.setCompressionDegreeToolStripMenuItem.Name = "setCompressionDegreeToolStripMenuItem";
            this.setCompressionDegreeToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.setCompressionDegreeToolStripMenuItem.Text = "Set compression degree";
            // 
            // storeToolStripMenuItem
            // 
            this.storeToolStripMenuItem.Name = "storeToolStripMenuItem";
            this.storeToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.storeToolStripMenuItem.Text = "Store";
            this.storeToolStripMenuItem.Click += new System.EventHandler(this.storeToolStripMenuItemClick);
            // 
            // fastestToolStripMenuItem
            // 
            this.fastestToolStripMenuItem.Name = "fastestToolStripMenuItem";
            this.fastestToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.fastestToolStripMenuItem.Text = "Fastest";
            this.fastestToolStripMenuItem.Click += new System.EventHandler(this.fastestToolStripMenuItemClick);
            // 
            // fastToolStripMenuItem
            // 
            this.fastToolStripMenuItem.Name = "fastToolStripMenuItem";
            this.fastToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.fastToolStripMenuItem.Text = "Fast";
            this.fastToolStripMenuItem.Click += new System.EventHandler(this.fastToolStripMenuItemClick);
            // 
            // normalToolStripMenuItem
            // 
            this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
            this.normalToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.normalToolStripMenuItem.Text = "Normal";
            this.normalToolStripMenuItem.Click += new System.EventHandler(this.normalToolStripMenuItemClick);
            // 
            // maximumToolStripMenuItem
            // 
            this.maximumToolStripMenuItem.Name = "maximumToolStripMenuItem";
            this.maximumToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.maximumToolStripMenuItem.Text = "Maximum";
            this.maximumToolStripMenuItem.Click += new System.EventHandler(this.maximumToolStripMenuItemClick);
            // 
            // ultraToolStripMenuItem
            // 
            this.ultraToolStripMenuItem.Name = "ultraToolStripMenuItem";
            this.ultraToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.ultraToolStripMenuItem.Text = "Ultra";
            this.ultraToolStripMenuItem.Click += new System.EventHandler(this.ultraToolStripMenuItemClick);
            // 
            // removeFromListToolStripMenuItem
            // 
            this.removeFromListToolStripMenuItem.Image = global::BUtil.Configurator.Icons.removeFromListToolStripMenuItem_Image;
            this.removeFromListToolStripMenuItem.Name = "removeFromListToolStripMenuItem";
            this.removeFromListToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.removeFromListToolStripMenuItem.Text = "Remove from list";
            this.removeFromListToolStripMenuItem.Click += new System.EventHandler(this.removeFromListToolStripMenuItemClick);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(197, 6);
            // 
            // addFilesToolStripMenuItem
            // 
            this.addFilesToolStripMenuItem.Image = global::BUtil.Configurator.Icons.Add_Files;
            this.addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
            this.addFilesToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.addFilesToolStripMenuItem.Text = "Add files...";
            this.addFilesToolStripMenuItem.Click += new System.EventHandler(this.addFilesToolStripMenuItemClick);
            // 
            // addFoldersToolStripMenuItem
            // 
            this.addFoldersToolStripMenuItem.Image = global::BUtil.Configurator.Icons.AddFolder;
            this.addFoldersToolStripMenuItem.Name = "addFoldersToolStripMenuItem";
            this.addFoldersToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.addFoldersToolStripMenuItem.Text = "Add folders...";
            this.addFoldersToolStripMenuItem.Click += new System.EventHandler(this.addFoldersToolStripMenuItemClick);
            // 
            // compressionItemsListView
            // 
            this.compressionItemsListView.AllowDrop = true;
            this.compressionItemsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.compressionItemsListView.BackColor = System.Drawing.SystemColors.Window;
            this.compressionItemsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.itemsToCompressColumnHeader});
            this.compressionItemsListView.ContextMenuStrip = this.filesFoldersContextMenuStrip;
            listViewGroup1.Header = "Store compression";
            listViewGroup1.Name = "Cl0listViewGroup";
            listViewGroup2.Header = "Fastest compression";
            listViewGroup2.Name = "Cl1listViewGroup";
            listViewGroup3.Header = "Fast compression";
            listViewGroup3.Name = "Cl2listViewGroup";
            listViewGroup4.Header = "Normal compression";
            listViewGroup4.Name = "Cl3listViewGroup";
            listViewGroup5.Header = "Maximum compression";
            listViewGroup5.Name = "Cl4listViewGroup";
            listViewGroup6.Header = "Ultra compression";
            listViewGroup6.Name = "Cl5listViewGroup";
            this.compressionItemsListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6});
            this.compressionItemsListView.LargeImageList = this.itemsToCompressImageList;
            this.compressionItemsListView.Location = new System.Drawing.Point(3, 3);
            this.compressionItemsListView.Name = "compressionItemsListView";
            this.compressionItemsListView.Size = new System.Drawing.Size(295, 201);
            this.compressionItemsListView.SmallImageList = this.itemsToCompressImageList;
            this.compressionItemsListView.StateImageList = this.itemsToCompressImageList;
            this.compressionItemsListView.TabIndex = 0;
            this.compressionItemsListView.UseCompatibleStateImageBehavior = false;
            this.compressionItemsListView.View = System.Windows.Forms.View.Details;
            this.compressionItemsListView.SelectedIndexChanged += new System.EventHandler(this.compressionItemsListViewSelectedIndexChanged);
            this.compressionItemsListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.compressionItemsListViewDragDrop);
            this.compressionItemsListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.compressionItemsListViewDragEnter);
            this.compressionItemsListView.DoubleClick += new System.EventHandler(this.compressionItemsListViewDoubleClick);
            this.compressionItemsListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.compressionItemsListViewKeyDown);
            this.compressionItemsListView.Resize += new System.EventHandler(this.сompressionItemsListViewResize);
            // 
            // itemsToCompressColumnHeader
            // 
            this.itemsToCompressColumnHeader.Text = "Items to backup";
            this.itemsToCompressColumnHeader.Width = 421;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Multiselect = true;
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            // 
            // addFoldersButton
            // 
            this.addFoldersButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addFoldersButton.Image = global::BUtil.Configurator.Icons.AddFolder;
            this.addFoldersButton.Location = new System.Drawing.Point(304, 67);
            this.addFoldersButton.Name = "addFoldersButton";
            this.addFoldersButton.Size = new System.Drawing.Size(61, 58);
            this.addFoldersButton.TabIndex = 2;
            this.addFoldersButton.UseVisualStyleBackColor = true;
            this.addFoldersButton.Click += new System.EventHandler(this.addFoldersButtonClick);
            // 
            // removeCompressionItemButton
            // 
            this.removeCompressionItemButton.AccessibleDescription = "";
            this.removeCompressionItemButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeCompressionItemButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.removeCompressionItemButton.Enabled = false;
            this.removeCompressionItemButton.Image = global::BUtil.Configurator.Icons.cross_48;
            this.removeCompressionItemButton.Location = new System.Drawing.Point(304, 131);
            this.removeCompressionItemButton.Name = "removeCompressionItemButton";
            this.removeCompressionItemButton.Size = new System.Drawing.Size(61, 58);
            this.removeCompressionItemButton.TabIndex = 3;
            this.removeCompressionItemButton.UseVisualStyleBackColor = true;
            this.removeCompressionItemButton.Click += new System.EventHandler(this.removeCompressionItemButtonClick);
            // 
            // addFilesButton
            // 
            this.addFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addFilesButton.Image = global::BUtil.Configurator.Icons.Add_Files1;
            this.addFilesButton.Location = new System.Drawing.Point(304, 3);
            this.addFilesButton.Name = "addFilesButton";
            this.addFilesButton.Size = new System.Drawing.Size(61, 58);
            this.addFilesButton.TabIndex = 1;
            this.addFilesButton.UseVisualStyleBackColor = true;
            this.addFilesButton.Click += new System.EventHandler(this.addFilesButtonClick);
            // 
            // SourceItemsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.compressionItemsListView);
            this.Controls.Add(this.addFoldersButton);
            this.Controls.Add(this.removeCompressionItemButton);
            this.Controls.Add(this.addFilesButton);
            this.MinimumSize = new System.Drawing.Size(368, 207);
            this.Name = "SourceItemsUserControl";
            this.Size = new System.Drawing.Size(368, 207);
            this.filesFoldersContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Button addFilesButton;
		private System.Windows.Forms.Button removeCompressionItemButton;
		private System.Windows.Forms.Button addFoldersButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ColumnHeader itemsToCompressColumnHeader;
		private System.Windows.Forms.ListView compressionItemsListView;
		private System.Windows.Forms.ToolStripMenuItem addFoldersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addFilesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem removeFromListToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ultraToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem maximumToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem normalToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fastToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fastestToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem storeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setCompressionDegreeToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip filesFoldersContextMenuStrip;
		private System.Windows.Forms.ImageList itemsToCompressImageList;
		private System.Windows.Forms.FolderBrowserDialog ofd;
	}
}
