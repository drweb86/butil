namespace BUtil.RestorationMaster
{
    partial class VersionsViewerForm
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
            if (disposing)
            {
                if (components != null)
                {
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionsViewerForm));
            imagesList = new System.Windows.Forms.ImageList(components);
            _statusStrip = new System.Windows.Forms.StatusStrip();
            _toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            _versionToContentSplitContainer = new System.Windows.Forms.SplitContainer();
            _versionsListView = new System.Windows.Forms.ListView();
            _versionsToolStrip = new System.Windows.Forms.ToolStrip();
            _selectVersionToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            _storageToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            _treeToChangesSplitContainer = new System.Windows.Forms.SplitContainer();
            _selectedVersionToolStrip = new System.Windows.Forms.ToolStrip();
            _selectedVersionToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            _recoverToolStripButton = new System.Windows.Forms.ToolStripButton();
            _treeViewJournalSelectedToolStripButton = new System.Windows.Forms.ToolStripButton();
            _filesTreeView = new System.Windows.Forms.TreeView();
            _treeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            recoverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _treeJournalSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _changesToolStrip = new System.Windows.Forms.ToolStrip();
            _changesToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            _journalSelectedToolStripButton = new System.Windows.Forms.ToolStripButton();
            _changesListView = new System.Windows.Forms.ListView();
            _changesContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            _journalSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _fbdialog = new System.Windows.Forms.FolderBrowserDialog();
            _statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_versionToContentSplitContainer).BeginInit();
            _versionToContentSplitContainer.Panel1.SuspendLayout();
            _versionToContentSplitContainer.Panel2.SuspendLayout();
            _versionToContentSplitContainer.SuspendLayout();
            _versionsToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_treeToChangesSplitContainer).BeginInit();
            _treeToChangesSplitContainer.Panel1.SuspendLayout();
            _treeToChangesSplitContainer.Panel2.SuspendLayout();
            _treeToChangesSplitContainer.SuspendLayout();
            _selectedVersionToolStrip.SuspendLayout();
            _treeContextMenuStrip.SuspendLayout();
            _changesToolStrip.SuspendLayout();
            _changesContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // imagesList
            // 
            imagesList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imagesList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imagesList.ImageStream");
            imagesList.TransparentColor = System.Drawing.Color.White;
            imagesList.Images.SetKeyName(0, "16x16 Binary.png");
            imagesList.Images.SetKeyName(1, "16x16 Folder.png");
            imagesList.Images.SetKeyName(2, "SourceItems48x48.png");
            imagesList.Images.SetKeyName(3, "VC-Created.png");
            imagesList.Images.SetKeyName(4, "VC-Deleted.png");
            imagesList.Images.SetKeyName(5, "VC-Updated.png");
            imagesList.Images.SetKeyName(6, "VC-Version.png");
            // 
            // _statusStrip
            // 
            _statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _toolStripStatusLabel });
            _statusStrip.Location = new System.Drawing.Point(0, 507);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Size = new System.Drawing.Size(1079, 22);
            _statusStrip.TabIndex = 0;
            // 
            // _toolStripStatusLabel
            // 
            _toolStripStatusLabel.Name = "_toolStripStatusLabel";
            _toolStripStatusLabel.Size = new System.Drawing.Size(383, 17);
            _toolStripStatusLabel.Text = "Click on item you want to restore and open context menu by right click";
            // 
            // _versionToContentSplitContainer
            // 
            _versionToContentSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            _versionToContentSplitContainer.Location = new System.Drawing.Point(0, 0);
            _versionToContentSplitContainer.Name = "_versionToContentSplitContainer";
            // 
            // _versionToContentSplitContainer.Panel1
            // 
            _versionToContentSplitContainer.Panel1.Controls.Add(_versionsListView);
            _versionToContentSplitContainer.Panel1.Controls.Add(_versionsToolStrip);
            // 
            // _versionToContentSplitContainer.Panel2
            // 
            _versionToContentSplitContainer.Panel2.Controls.Add(_treeToChangesSplitContainer);
            _versionToContentSplitContainer.Size = new System.Drawing.Size(1079, 507);
            _versionToContentSplitContainer.SplitterDistance = 289;
            _versionToContentSplitContainer.TabIndex = 1;
            // 
            // _versionsListView
            // 
            _versionsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            _versionsListView.Location = new System.Drawing.Point(0, 25);
            _versionsListView.MultiSelect = false;
            _versionsListView.Name = "_versionsListView";
            _versionsListView.Size = new System.Drawing.Size(289, 482);
            _versionsListView.SmallImageList = imagesList;
            _versionsListView.TabIndex = 1;
            _versionsListView.UseCompatibleStateImageBehavior = false;
            _versionsListView.View = System.Windows.Forms.View.List;
            _versionsListView.SelectedIndexChanged += OnVersionChanged;
            // 
            // _versionsToolStrip
            // 
            _versionsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _selectVersionToolStripLabel, _storageToolStripLabel });
            _versionsToolStrip.Location = new System.Drawing.Point(0, 0);
            _versionsToolStrip.Name = "_versionsToolStrip";
            _versionsToolStrip.Size = new System.Drawing.Size(289, 25);
            _versionsToolStrip.TabIndex = 0;
            _versionsToolStrip.Text = "toolStrip1";
            // 
            // _selectVersionToolStripLabel
            // 
            _selectVersionToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            _selectVersionToolStripLabel.Name = "_selectVersionToolStripLabel";
            _selectVersionToolStripLabel.Size = new System.Drawing.Size(100, 22);
            _selectVersionToolStripLabel.Text = "SELECT VERSION";
            // 
            // _storageToolStripLabel
            // 
            _storageToolStripLabel.Name = "_storageToolStripLabel";
            _storageToolStripLabel.Size = new System.Drawing.Size(75, 22);
            _storageToolStripLabel.Text = "Storage: 0 TB";
            // 
            // _treeToChangesSplitContainer
            // 
            _treeToChangesSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            _treeToChangesSplitContainer.Location = new System.Drawing.Point(0, 0);
            _treeToChangesSplitContainer.Name = "_treeToChangesSplitContainer";
            _treeToChangesSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _treeToChangesSplitContainer.Panel1
            // 
            _treeToChangesSplitContainer.Panel1.Controls.Add(_selectedVersionToolStrip);
            _treeToChangesSplitContainer.Panel1.Controls.Add(_filesTreeView);
            // 
            // _treeToChangesSplitContainer.Panel2
            // 
            _treeToChangesSplitContainer.Panel2.Controls.Add(_changesToolStrip);
            _treeToChangesSplitContainer.Panel2.Controls.Add(_changesListView);
            _treeToChangesSplitContainer.Size = new System.Drawing.Size(786, 507);
            _treeToChangesSplitContainer.SplitterDistance = 251;
            _treeToChangesSplitContainer.TabIndex = 0;
            // 
            // _selectedVersionToolStrip
            // 
            _selectedVersionToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _selectedVersionToolStripLabel, _recoverToolStripButton, _treeViewJournalSelectedToolStripButton });
            _selectedVersionToolStrip.Location = new System.Drawing.Point(0, 0);
            _selectedVersionToolStrip.Name = "_selectedVersionToolStrip";
            _selectedVersionToolStrip.Size = new System.Drawing.Size(786, 25);
            _selectedVersionToolStrip.TabIndex = 1;
            _selectedVersionToolStrip.Text = "toolStrip2";
            // 
            // _selectedVersionToolStripLabel
            // 
            _selectedVersionToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            _selectedVersionToolStripLabel.Name = "_selectedVersionToolStripLabel";
            _selectedVersionToolStripLabel.Size = new System.Drawing.Size(36, 22);
            _selectedVersionToolStripLabel.Text = "FILES";
            // 
            // _recoverToolStripButton
            // 
            _recoverToolStripButton.Image = (System.Drawing.Image)resources.GetObject("_recoverToolStripButton.Image");
            _recoverToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _recoverToolStripButton.Name = "_recoverToolStripButton";
            _recoverToolStripButton.Size = new System.Drawing.Size(124, 22);
            _recoverToolStripButton.Text = "Recover selected...";
            _recoverToolStripButton.Click += OnRecover;
            // 
            // _treeViewJournalSelectedToolStripButton
            // 
            _treeViewJournalSelectedToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            _treeViewJournalSelectedToolStripButton.Image = (System.Drawing.Image)resources.GetObject("_treeViewJournalSelectedToolStripButton.Image");
            _treeViewJournalSelectedToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _treeViewJournalSelectedToolStripButton.Name = "_treeViewJournalSelectedToolStripButton";
            _treeViewJournalSelectedToolStripButton.Size = new System.Drawing.Size(104, 22);
            _treeViewJournalSelectedToolStripButton.Text = "Journal selected...";
            _treeViewJournalSelectedToolStripButton.Click += OnTreeJournalSelected;
            // 
            // _filesTreeView
            // 
            _filesTreeView.ContextMenuStrip = _treeContextMenuStrip;
            _filesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            _filesTreeView.ImageIndex = 0;
            _filesTreeView.ImageList = imagesList;
            _filesTreeView.Location = new System.Drawing.Point(0, 0);
            _filesTreeView.Name = "_filesTreeView";
            _filesTreeView.SelectedImageIndex = 0;
            _filesTreeView.Size = new System.Drawing.Size(786, 251);
            _filesTreeView.TabIndex = 2;
            _filesTreeView.MouseDown += OnMouseDown;
            // 
            // _treeContextMenuStrip
            // 
            _treeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { recoverToolStripMenuItem, _treeJournalSelectedToolStripMenuItem });
            _treeContextMenuStrip.Name = "_treeContextMenuStrip";
            _treeContextMenuStrip.Size = new System.Drawing.Size(172, 48);
            // 
            // recoverToolStripMenuItem
            // 
            recoverToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("recoverToolStripMenuItem.Image");
            recoverToolStripMenuItem.Name = "recoverToolStripMenuItem";
            recoverToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            recoverToolStripMenuItem.Text = "Recover selected...";
            recoverToolStripMenuItem.Click += OnRecover;
            // 
            // _treeJournalSelectedToolStripMenuItem
            // 
            _treeJournalSelectedToolStripMenuItem.Name = "_treeJournalSelectedToolStripMenuItem";
            _treeJournalSelectedToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            _treeJournalSelectedToolStripMenuItem.Text = "Journal selected...";
            _treeJournalSelectedToolStripMenuItem.Click += OnTreeJournalSelected;
            // 
            // _changesToolStrip
            // 
            _changesToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _changesToolStripLabel, _journalSelectedToolStripButton });
            _changesToolStrip.Location = new System.Drawing.Point(0, 0);
            _changesToolStrip.Name = "_changesToolStrip";
            _changesToolStrip.Size = new System.Drawing.Size(786, 25);
            _changesToolStrip.TabIndex = 2;
            _changesToolStrip.Text = "toolStrip1";
            // 
            // _changesToolStripLabel
            // 
            _changesToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            _changesToolStripLabel.Name = "_changesToolStripLabel";
            _changesToolStripLabel.Size = new System.Drawing.Size(62, 22);
            _changesToolStripLabel.Text = "CHANGES";
            // 
            // _journalSelectedToolStripButton
            // 
            _journalSelectedToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            _journalSelectedToolStripButton.Image = (System.Drawing.Image)resources.GetObject("_journalSelectedToolStripButton.Image");
            _journalSelectedToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _journalSelectedToolStripButton.Name = "_journalSelectedToolStripButton";
            _journalSelectedToolStripButton.Size = new System.Drawing.Size(104, 22);
            _journalSelectedToolStripButton.Text = "Journal selected...";
            _journalSelectedToolStripButton.Click += OnJournalSelectedChangesView;
            // 
            // _changesListView
            // 
            _changesListView.ContextMenuStrip = _changesContextMenuStrip;
            _changesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            _changesListView.Location = new System.Drawing.Point(0, 0);
            _changesListView.Name = "_changesListView";
            _changesListView.Size = new System.Drawing.Size(786, 252);
            _changesListView.SmallImageList = imagesList;
            _changesListView.TabIndex = 1;
            _changesListView.UseCompatibleStateImageBehavior = false;
            _changesListView.View = System.Windows.Forms.View.List;
            // 
            // _changesContextMenuStrip
            // 
            _changesContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _journalSelectedToolStripMenuItem });
            _changesContextMenuStrip.Name = "_changesContextMenuStrip";
            _changesContextMenuStrip.Size = new System.Drawing.Size(168, 26);
            // 
            // _journalSelectedToolStripMenuItem
            // 
            _journalSelectedToolStripMenuItem.Name = "_journalSelectedToolStripMenuItem";
            _journalSelectedToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            _journalSelectedToolStripMenuItem.Text = "Journal selected...";
            _journalSelectedToolStripMenuItem.Click += OnJournalSelectedChangesView;
            // 
            // VersionsViewerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1079, 529);
            Controls.Add(_versionToContentSplitContainer);
            Controls.Add(_statusStrip);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(437, 301);
            Name = "VersionsViewerForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Restoration";
            Load += OnLoad;
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            _versionToContentSplitContainer.Panel1.ResumeLayout(false);
            _versionToContentSplitContainer.Panel1.PerformLayout();
            _versionToContentSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_versionToContentSplitContainer).EndInit();
            _versionToContentSplitContainer.ResumeLayout(false);
            _versionsToolStrip.ResumeLayout(false);
            _versionsToolStrip.PerformLayout();
            _treeToChangesSplitContainer.Panel1.ResumeLayout(false);
            _treeToChangesSplitContainer.Panel1.PerformLayout();
            _treeToChangesSplitContainer.Panel2.ResumeLayout(false);
            _treeToChangesSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_treeToChangesSplitContainer).EndInit();
            _treeToChangesSplitContainer.ResumeLayout(false);
            _selectedVersionToolStrip.ResumeLayout(false);
            _selectedVersionToolStrip.PerformLayout();
            _treeContextMenuStrip.ResumeLayout(false);
            _changesToolStrip.ResumeLayout(false);
            _changesToolStrip.PerformLayout();
            _changesContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ImageList imagesList;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _toolStripStatusLabel;
        private System.Windows.Forms.SplitContainer _versionToContentSplitContainer;
        private System.Windows.Forms.SplitContainer _treeToChangesSplitContainer;
        private System.Windows.Forms.TreeView _filesTreeView;
        private System.Windows.Forms.ContextMenuStrip _treeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem recoverToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog _fbdialog;
        private System.Windows.Forms.ListView _changesListView;
        private System.Windows.Forms.ListView _versionsListView;
        private System.Windows.Forms.ToolStrip _versionsToolStrip;
        private System.Windows.Forms.ToolStripLabel _selectVersionToolStripLabel;
        private System.Windows.Forms.ToolStripLabel _storageToolStripLabel;
        private System.Windows.Forms.ContextMenuStrip _changesContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _journalSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStrip _changesToolStrip;
        private System.Windows.Forms.ToolStripLabel _changesToolStripLabel;
        private System.Windows.Forms.ToolStripButton _journalSelectedToolStripButton;
        private System.Windows.Forms.ToolStrip _selectedVersionToolStrip;
        private System.Windows.Forms.ToolStripLabel _selectedVersionToolStripLabel;
        private System.Windows.Forms.ToolStripButton _recoverToolStripButton;
        private System.Windows.Forms.ToolStripButton _treeViewJournalSelectedToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem _treeJournalSelectedToolStripMenuItem;
    }
}
