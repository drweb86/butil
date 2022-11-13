
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
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.addFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._ignoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._itemsListView = new System.Windows.Forms.ListView();
            this.itemsToCompressColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.addFoldersButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.addFilesButton = new System.Windows.Forms.Button();
            this._ignoreButton = new System.Windows.Forms.Button();
            this._openInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filesFoldersContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // itemsToCompressImageList
            // 
            this.itemsToCompressImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.itemsToCompressImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("itemsToCompressImageList.ImageStream")));
            this.itemsToCompressImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.itemsToCompressImageList.Images.SetKeyName(0, "folder_48.png");
            this.itemsToCompressImageList.Images.SetKeyName(1, "AddFiles48x48.ico");
            this.itemsToCompressImageList.Images.SetKeyName(2, "Exclude16x16.png");
            // 
            // filesFoldersContextMenuStrip
            // 
            this.filesFoldersContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.toolStripSeparator6,
            this.addFilesToolStripMenuItem,
            this.addFoldersToolStripMenuItem,
            this._ignoreToolStripMenuItem,
            this._openInExplorerToolStripMenuItem});
            this.filesFoldersContextMenuStrip.Name = "filesFoldersContextMenuStrip";
            this.filesFoldersContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.filesFoldersContextMenuStrip.Size = new System.Drawing.Size(181, 142);
            this.filesFoldersContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.OnMenuOpening);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Image = global::BUtil.Configurator.Icons.removeFromListToolStripMenuItem_Image;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeToolStripMenuItem.Text = "Remove from list";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.OnRemoveToolStripMenuItemClick);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(177, 6);
            // 
            // addFilesToolStripMenuItem
            // 
            this.addFilesToolStripMenuItem.Image = global::BUtil.Configurator.Icons.Add_Files;
            this.addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
            this.addFilesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addFilesToolStripMenuItem.Text = "Add files...";
            this.addFilesToolStripMenuItem.Click += new System.EventHandler(this.OnAddFilesToolStripMenuItemClick);
            // 
            // addFoldersToolStripMenuItem
            // 
            this.addFoldersToolStripMenuItem.Image = global::BUtil.Configurator.Icons.AddFolder;
            this.addFoldersToolStripMenuItem.Name = "addFoldersToolStripMenuItem";
            this.addFoldersToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addFoldersToolStripMenuItem.Text = "Add folders...";
            this.addFoldersToolStripMenuItem.Click += new System.EventHandler(this.OnAddFoldersToolStripMenuItemClick);
            // 
            // _ignoreToolStripMenuItem
            // 
            this._ignoreToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_ignoreToolStripMenuItem.Image")));
            this._ignoreToolStripMenuItem.Name = "_ignoreToolStripMenuItem";
            this._ignoreToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this._ignoreToolStripMenuItem.Text = "Ignore...";
            this._ignoreToolStripMenuItem.Click += new System.EventHandler(this.OnExcludeAdd);
            // 
            // _itemsListView
            // 
            this._itemsListView.AllowDrop = true;
            this._itemsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._itemsListView.BackColor = System.Drawing.SystemColors.Window;
            this._itemsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.itemsToCompressColumnHeader});
            this._itemsListView.ContextMenuStrip = this.filesFoldersContextMenuStrip;
            this._itemsListView.LargeImageList = this.itemsToCompressImageList;
            this._itemsListView.Location = new System.Drawing.Point(4, 3);
            this._itemsListView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._itemsListView.Name = "_itemsListView";
            this._itemsListView.Size = new System.Drawing.Size(344, 303);
            this._itemsListView.SmallImageList = this.itemsToCompressImageList;
            this._itemsListView.StateImageList = this.itemsToCompressImageList;
            this._itemsListView.TabIndex = 0;
            this._itemsListView.UseCompatibleStateImageBehavior = false;
            this._itemsListView.View = System.Windows.Forms.View.Details;
            this._itemsListView.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            this._itemsListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this._itemsListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this._itemsListView.DoubleClick += new System.EventHandler(this.OnItemDoubleClick);
            this._itemsListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this._itemsListView.Resize += new System.EventHandler(this.OnResize);
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
            this.addFoldersButton.Location = new System.Drawing.Point(355, 77);
            this.addFoldersButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.addFoldersButton.Name = "addFoldersButton";
            this.addFoldersButton.Size = new System.Drawing.Size(71, 67);
            this.addFoldersButton.TabIndex = 2;
            this.addFoldersButton.UseVisualStyleBackColor = true;
            this.addFoldersButton.Click += new System.EventHandler(this.OnAddFoldersButtonClick);
            // 
            // removeButton
            // 
            this.removeButton.AccessibleDescription = "";
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.removeButton.Enabled = false;
            this.removeButton.Image = global::BUtil.Configurator.Icons.cross_48;
            this.removeButton.Location = new System.Drawing.Point(355, 151);
            this.removeButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(71, 67);
            this.removeButton.TabIndex = 3;
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.OnRemoveItemsButtonClick);
            // 
            // addFilesButton
            // 
            this.addFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addFilesButton.Image = global::BUtil.Configurator.Icons.Add_Files1;
            this.addFilesButton.Location = new System.Drawing.Point(355, 3);
            this.addFilesButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.addFilesButton.Name = "addFilesButton";
            this.addFilesButton.Size = new System.Drawing.Size(71, 67);
            this.addFilesButton.TabIndex = 1;
            this.addFilesButton.UseVisualStyleBackColor = true;
            this.addFilesButton.Click += new System.EventHandler(this.OnAddFilesButtonClick);
            // 
            // _ignoreButton
            // 
            this._ignoreButton.AccessibleDescription = "";
            this._ignoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._ignoreButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this._ignoreButton.Image = ((System.Drawing.Image)(resources.GetObject("_ignoreButton.Image")));
            this._ignoreButton.Location = new System.Drawing.Point(355, 224);
            this._ignoreButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._ignoreButton.Name = "_ignoreButton";
            this._ignoreButton.Size = new System.Drawing.Size(71, 67);
            this._ignoreButton.TabIndex = 4;
            this._ignoreButton.UseVisualStyleBackColor = true;
            this._ignoreButton.Click += new System.EventHandler(this.OnExcludeAdd);
            // 
            // _openInExplorerToolStripMenuItem
            // 
            this._openInExplorerToolStripMenuItem.Image = global::BUtil.Configurator.Icons.folder_48;
            this._openInExplorerToolStripMenuItem.Name = "_openInExplorerToolStripMenuItem";
            this._openInExplorerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this._openInExplorerToolStripMenuItem.Text = "Open in Explorer...";
            this._openInExplorerToolStripMenuItem.Click += new System.EventHandler(this.OnItemDoubleClick);
            // 
            // WhatUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._ignoreButton);
            this.Controls.Add(this._itemsListView);
            this.Controls.Add(this.addFoldersButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addFilesButton);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MinimumSize = new System.Drawing.Size(429, 311);
            this.Name = "WhatUserControl";
            this.Size = new System.Drawing.Size(429, 311);
            this.filesFoldersContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Button addFilesButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button addFoldersButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ColumnHeader itemsToCompressColumnHeader;
		private System.Windows.Forms.ListView _itemsListView;
		private System.Windows.Forms.ToolStripMenuItem addFoldersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addFilesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip filesFoldersContextMenuStrip;
		private System.Windows.Forms.ImageList itemsToCompressImageList;
		private System.Windows.Forms.FolderBrowserDialog ofd;
        private System.Windows.Forms.Button _ignoreButton;
        private System.Windows.Forms.ToolStripMenuItem _ignoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _openInExplorerToolStripMenuItem;
    }
}
