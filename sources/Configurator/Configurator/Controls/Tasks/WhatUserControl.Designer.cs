
namespace BUtil.Configurator.Controls
{
	partial class WhatUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhatUserControl));
            this.ofd = new System.Windows.Forms.FolderBrowserDialog();
            this.itemsToCompressImageList = new System.Windows.Forms.ImageList(this.components);
            this.filesFoldersContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
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
            this.removeFromListToolStripMenuItem,
            this.toolStripSeparator6,
            this.addFilesToolStripMenuItem,
            this.addFoldersToolStripMenuItem});
            this.filesFoldersContextMenuStrip.Name = "filesFoldersContextMenuStrip";
            this.filesFoldersContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.filesFoldersContextMenuStrip.Size = new System.Drawing.Size(201, 98);
            this.filesFoldersContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.filesFoldersContextMenuStripOpening);
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
            this.compressionItemsListView.Resize += new System.EventHandler(this.compressionItemsListViewResize);
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
		private System.Windows.Forms.ContextMenuStrip filesFoldersContextMenuStrip;
		private System.Windows.Forms.ImageList itemsToCompressImageList;
		private System.Windows.Forms.FolderBrowserDialog ofd;
	}
}
