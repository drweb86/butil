namespace BUtil.RestorationMaster
{
	partial class RestoreForm
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
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Folders", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Files", System.Windows.Forms.HorizontalAlignment.Left);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RestoreForm));
			this.itemsListView = new System.Windows.Forms.ListView();
			this.itemsColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.restoreContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.imagesList = new System.Windows.Forms.ImageList(this.components);
			this.finishButton = new System.Windows.Forms.Button();
			this.restoreButton = new System.Windows.Forms.Button();
			this.restoreContextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// itemsListView
			// 
			this.itemsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.itemsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.itemsColumnHeader});
			this.itemsListView.ContextMenuStrip = this.restoreContextMenuStrip;
			this.itemsListView.FullRowSelect = true;
			listViewGroup1.Header = "Folders";
			listViewGroup1.Name = "FolderslistViewGroup";
			listViewGroup2.Header = "Files";
			listViewGroup2.Name = "FileslistViewGroup";
			this.itemsListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
									listViewGroup1,
									listViewGroup2});
			this.itemsListView.LargeImageList = this.imagesList;
			this.itemsListView.Location = new System.Drawing.Point(3, 4);
			this.itemsListView.Name = "itemsListView";
			this.itemsListView.Size = new System.Drawing.Size(503, 282);
			this.itemsListView.SmallImageList = this.imagesList;
			this.itemsListView.TabIndex = 5;
			this.itemsListView.UseCompatibleStateImageBehavior = false;
			this.itemsListView.View = System.Windows.Forms.View.Details;
			this.itemsListView.Resize += new System.EventHandler(this.itemsListViewResize);
			this.itemsListView.SelectedIndexChanged += new System.EventHandler(this.itemsListViewSelectedIndexChanged);
			this.itemsListView.DoubleClick += new System.EventHandler(this.itemsListViewDoubleClick);
			// 
			// itemsColumnHeader
			// 
			this.itemsColumnHeader.Text = "Items in image";
			this.itemsColumnHeader.Width = 470;
			// 
			// restoreContextMenuStrip
			// 
			this.restoreContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.restoreToolStripMenuItem});
			this.restoreContextMenuStrip.Name = "restorecontextMenuStrip";
			this.restoreContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.restoreContextMenuStrip.Size = new System.Drawing.Size(140, 26);
			this.restoreContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.restoreContextMenuStripOpening);
			// 
			// restoreToolStripMenuItem
			// 
			this.restoreToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("restoreToolStripMenuItem.Image")));
			this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
			this.restoreToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
			this.restoreToolStripMenuItem.Text = "Restore....";
			this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItemClick);
			// 
			// imagesList
			// 
			this.imagesList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagesList.ImageStream")));
			this.imagesList.TransparentColor = System.Drawing.Color.White;
			this.imagesList.Images.SetKeyName(0, "16x16 Binary.png");
			this.imagesList.Images.SetKeyName(1, "16x16 Folder.png");
			// 
			// finishButton
			// 
			this.finishButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.finishButton.AutoSize = true;
			this.finishButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.finishButton.Location = new System.Drawing.Point(431, 292);
			this.finishButton.Name = "finishButton";
			this.finishButton.Size = new System.Drawing.Size(75, 23);
			this.finishButton.TabIndex = 6;
			this.finishButton.Text = "Finish";
			this.finishButton.UseVisualStyleBackColor = true;
			// 
			// restoreButton
			// 
			this.restoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.restoreButton.AutoSize = true;
			this.restoreButton.Enabled = false;
			this.restoreButton.Location = new System.Drawing.Point(350, 292);
			this.restoreButton.Name = "restoreButton";
			this.restoreButton.Size = new System.Drawing.Size(75, 23);
			this.restoreButton.TabIndex = 7;
			this.restoreButton.Text = "Recover!";
			this.restoreButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.restoreButton.UseVisualStyleBackColor = true;
			this.restoreButton.Click += new System.EventHandler(this.restoreButtonClick);
			// 
			// RestoreForm
			// 
			this.AcceptButton = this.restoreButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.finishButton;
			this.ClientSize = new System.Drawing.Size(518, 327);
			this.Controls.Add(this.itemsListView);
			this.Controls.Add(this.finishButton);
			this.Controls.Add(this.restoreButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(377, 266);
			this.Name = "RestoreForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "{Image location} - Restoration";
			this.restoreContextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ListView itemsListView;
		private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip restoreContextMenuStrip;
		private System.Windows.Forms.Button restoreButton;
		private System.Windows.Forms.ImageList imagesList;
        private System.Windows.Forms.Button finishButton;
		private System.Windows.Forms.ColumnHeader itemsColumnHeader;
	}
}
