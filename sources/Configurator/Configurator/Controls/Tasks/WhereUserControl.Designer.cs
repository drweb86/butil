
namespace BUtil.Configurator.Configurator.Controls
{
	partial class WhereUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhereUserControl));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("HDD", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("FTP", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Network storages", System.Windows.Forms.HorizontalAlignment.Left);
            this.storageTypesImageList = new System.Windows.Forms.ImageList(this.components);
            this.storagesContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hardDriveStorageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ftpStorageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.networkStorageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addStorageContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hardDriveStorageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ftpStorageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.networkStorageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.storagesListView = new System.Windows.Forms.ListView();
            this.addStorageButton = new System.Windows.Forms.Button();
            this.removeStorageButton = new System.Windows.Forms.Button();
            this.modifyStorageButton = new System.Windows.Forms.Button();
            this.storagesContextMenuStrip.SuspendLayout();
            this.addStorageContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // storageTypesImageList
            // 
            this.storageTypesImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("storageTypesImageList.ImageStream")));
            this.storageTypesImageList.TransparentColor = System.Drawing.Color.White;
            this.storageTypesImageList.Images.SetKeyName(0, "Hdd48x48.png");
            this.storageTypesImageList.Images.SetKeyName(1, "Ftp16x16.png");
            this.storageTypesImageList.Images.SetKeyName(2, "Share48x48.png");
            // 
            // storagesContextMenuStrip
            // 
            this.storagesContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewToolStripMenuItem,
            this.modifyToolStripMenuItem,
            this.toolStripSeparator4,
            this.removeToolStripMenuItem});
            this.storagesContextMenuStrip.Name = "StoragescontextMenuStrip";
            this.storagesContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.storagesContextMenuStrip.Size = new System.Drawing.Size(153, 98);
            this.storagesContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.StoragesContextMenuStripOpening);
            // 
            // addNewToolStripMenuItem
            // 
            this.addNewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hardDriveStorageToolStripMenuItem,
            this.ftpStorageToolStripMenuItem,
            this.networkStorageToolStripMenuItem});
            this.addNewToolStripMenuItem.Image = global::BUtil.Configurator.Icons.addNewToolStripMenuItem_Image;
            this.addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            this.addNewToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addNewToolStripMenuItem.Text = "Add";
            // 
            // hardDriveStorageToolStripMenuItem
            // 
            this.hardDriveStorageToolStripMenuItem.Image = global::BUtil.Configurator.Icons.Hdd16x16;
            this.hardDriveStorageToolStripMenuItem.Name = "hardDriveStorageToolStripMenuItem";
            this.hardDriveStorageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.hardDriveStorageToolStripMenuItem.Text = "Hard drive storage...";
            this.hardDriveStorageToolStripMenuItem.Click += new System.EventHandler(this.HardDriveStorageToolStripMenuItem1Click);
            // 
            // ftpStorageToolStripMenuItem
            // 
            this.ftpStorageToolStripMenuItem.Image = global::BUtil.Configurator.Icons.Ftp16x16;
            this.ftpStorageToolStripMenuItem.Name = "ftpStorageToolStripMenuItem";
            this.ftpStorageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ftpStorageToolStripMenuItem.Text = "Ftp storage...";
            this.ftpStorageToolStripMenuItem.Click += new System.EventHandler(this.FtpStorageToolStripMenuItem1Click);
            // 
            // networkStorageToolStripMenuItem
            // 
            this.networkStorageToolStripMenuItem.Image = global::BUtil.Configurator.Icons.Share16x16;
            this.networkStorageToolStripMenuItem.Name = "networkStorageToolStripMenuItem";
            this.networkStorageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.networkStorageToolStripMenuItem.Text = "Network storage...";
            this.networkStorageToolStripMenuItem.Click += new System.EventHandler(this.NetworkStorageToolStripMenuItem1Click);
            // 
            // modifyToolStripMenuItem
            // 
            this.modifyToolStripMenuItem.Image = global::BUtil.Configurator.Icons.OtherOptions48x48;
            this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            this.modifyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.modifyToolStripMenuItem.Text = "Modify...";
            this.modifyToolStripMenuItem.Click += new System.EventHandler(this.ModifyStorageButtonClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Image = global::BUtil.Configurator.Icons.removeFromListToolStripMenuItem_Image;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.RemoveStorageButtonClick);
            // 
            // addStorageContextMenuStrip
            // 
            this.addStorageContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hardDriveStorageToolStripMenuItem1,
            this.ftpStorageToolStripMenuItem1,
            this.networkStorageToolStripMenuItem1});
            this.addStorageContextMenuStrip.Name = "addStorageContextMenuStrip";
            this.addStorageContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.addStorageContextMenuStrip.Size = new System.Drawing.Size(181, 70);
            // 
            // hardDriveStorageToolStripMenuItem1
            // 
            this.hardDriveStorageToolStripMenuItem1.Image = global::BUtil.Configurator.Icons.Hdd16x16;
            this.hardDriveStorageToolStripMenuItem1.Name = "hardDriveStorageToolStripMenuItem1";
            this.hardDriveStorageToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.hardDriveStorageToolStripMenuItem1.Text = "Hard drive storage...";
            this.hardDriveStorageToolStripMenuItem1.Click += new System.EventHandler(this.HardDriveStorageToolStripMenuItem1Click);
            // 
            // ftpStorageToolStripMenuItem1
            // 
            this.ftpStorageToolStripMenuItem1.Image = global::BUtil.Configurator.Icons.Ftp16x16;
            this.ftpStorageToolStripMenuItem1.Name = "ftpStorageToolStripMenuItem1";
            this.ftpStorageToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.ftpStorageToolStripMenuItem1.Text = "Ftp storage...";
            this.ftpStorageToolStripMenuItem1.Click += new System.EventHandler(this.FtpStorageToolStripMenuItem1Click);
            // 
            // networkStorageToolStripMenuItem1
            // 
            this.networkStorageToolStripMenuItem1.Image = global::BUtil.Configurator.Icons.Share16x16;
            this.networkStorageToolStripMenuItem1.Name = "networkStorageToolStripMenuItem1";
            this.networkStorageToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.networkStorageToolStripMenuItem1.Text = "Network storage...";
            this.networkStorageToolStripMenuItem1.Click += new System.EventHandler(this.NetworkStorageToolStripMenuItem1Click);
            // 
            // storagesListView
            // 
            this.storagesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.storagesListView.ContextMenuStrip = this.storagesContextMenuStrip;
            listViewGroup1.Header = "HDD";
            listViewGroup1.Name = "HDDCopylistViewGroup";
            listViewGroup2.Header = "FTP";
            listViewGroup2.Name = "FTPlistViewGroup";
            listViewGroup3.Header = "Network storages";
            listViewGroup3.Name = "networkStoragesListViewGroup";
            this.storagesListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.storagesListView.LargeImageList = this.storageTypesImageList;
            this.storagesListView.Location = new System.Drawing.Point(3, 3);
            this.storagesListView.MultiSelect = false;
            this.storagesListView.Name = "storagesListView";
            this.storagesListView.ShowItemToolTips = true;
            this.storagesListView.Size = new System.Drawing.Size(329, 209);
            this.storagesListView.SmallImageList = this.storageTypesImageList;
            this.storagesListView.TabIndex = 0;
            this.storagesListView.UseCompatibleStateImageBehavior = false;
            this.storagesListView.ItemActivate += new System.EventHandler(this.StoragesListViewItemActivate);
            this.storagesListView.SelectedIndexChanged += new System.EventHandler(this.StoragesListViewSelectedIndexChanged);
            // 
            // addStorageButton
            // 
            this.addStorageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addStorageButton.ContextMenuStrip = this.addStorageContextMenuStrip;
            this.addStorageButton.Image = global::BUtil.Configurator.Icons.Add48x48;
            this.addStorageButton.Location = new System.Drawing.Point(338, 3);
            this.addStorageButton.Name = "addStorageButton";
            this.addStorageButton.Size = new System.Drawing.Size(61, 58);
            this.addStorageButton.TabIndex = 1;
            this.addStorageButton.UseVisualStyleBackColor = true;
            this.addStorageButton.Click += new System.EventHandler(this.AddStorageButtonClick);
            // 
            // removeStorageButton
            // 
            this.removeStorageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeStorageButton.Enabled = false;
            this.removeStorageButton.Image = global::BUtil.Configurator.Icons.cross_48;
            this.removeStorageButton.Location = new System.Drawing.Point(338, 131);
            this.removeStorageButton.Name = "removeStorageButton";
            this.removeStorageButton.Size = new System.Drawing.Size(61, 58);
            this.removeStorageButton.TabIndex = 3;
            this.removeStorageButton.UseVisualStyleBackColor = true;
            this.removeStorageButton.Click += new System.EventHandler(this.RemoveStorageButtonClick);
            // 
            // modifyStorageButton
            // 
            this.modifyStorageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modifyStorageButton.Enabled = false;
            this.modifyStorageButton.Image = global::BUtil.Configurator.Icons.OtherOptions48x48;
            this.modifyStorageButton.Location = new System.Drawing.Point(338, 67);
            this.modifyStorageButton.Name = "modifyStorageButton";
            this.modifyStorageButton.Size = new System.Drawing.Size(61, 58);
            this.modifyStorageButton.TabIndex = 2;
            this.modifyStorageButton.UseVisualStyleBackColor = true;
            this.modifyStorageButton.Click += new System.EventHandler(this.ModifyStorageButtonClick);
            // 
            // StoragesUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.storagesListView);
            this.Controls.Add(this.addStorageButton);
            this.Controls.Add(this.removeStorageButton);
            this.Controls.Add(this.modifyStorageButton);
            this.MinimumSize = new System.Drawing.Size(285, 200);
            this.Name = "StoragesUserControl";
            this.Size = new System.Drawing.Size(402, 215);
            this.storagesContextMenuStrip.ResumeLayout(false);
            this.addStorageContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.ImageList storageTypesImageList;
		private System.Windows.Forms.ToolStripMenuItem networkStorageToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem ftpStorageToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem hardDriveStorageToolStripMenuItem1;
		private System.Windows.Forms.ContextMenuStrip addStorageContextMenuStrip;
		private System.Windows.Forms.ListView storagesListView;
		private System.Windows.Forms.Button addStorageButton;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem networkStorageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ftpStorageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hardDriveStorageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip storagesContextMenuStrip;
		private System.Windows.Forms.Button modifyStorageButton;
		private System.Windows.Forms.Button removeStorageButton;
	}
}
