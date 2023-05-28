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
            _filesTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _dataLabel = new System.Windows.Forms.Label();
            _filesTreeView = new System.Windows.Forms.TreeView();
            _treeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            recoverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _changesTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _changesLabel = new System.Windows.Forms.Label();
            _changesListView = new System.Windows.Forms.ListView();
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
            _filesTableLayoutPanel.SuspendLayout();
            _treeContextMenuStrip.SuspendLayout();
            _changesTableLayoutPanel.SuspendLayout();
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
            _treeToChangesSplitContainer.Panel1.Controls.Add(_filesTableLayoutPanel);
            // 
            // _treeToChangesSplitContainer.Panel2
            // 
            _treeToChangesSplitContainer.Panel2.Controls.Add(_changesTableLayoutPanel);
            _treeToChangesSplitContainer.Size = new System.Drawing.Size(786, 507);
            _treeToChangesSplitContainer.SplitterDistance = 251;
            _treeToChangesSplitContainer.TabIndex = 0;
            // 
            // _filesTableLayoutPanel
            // 
            _filesTableLayoutPanel.ColumnCount = 1;
            _filesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _filesTableLayoutPanel.Controls.Add(_dataLabel, 0, 0);
            _filesTableLayoutPanel.Controls.Add(_filesTreeView, 0, 1);
            _filesTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _filesTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _filesTableLayoutPanel.Name = "_filesTableLayoutPanel";
            _filesTableLayoutPanel.RowCount = 2;
            _filesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _filesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _filesTableLayoutPanel.Size = new System.Drawing.Size(786, 251);
            _filesTableLayoutPanel.TabIndex = 0;
            // 
            // _dataLabel
            // 
            _dataLabel.AutoSize = true;
            _dataLabel.Location = new System.Drawing.Point(3, 0);
            _dataLabel.Name = "_dataLabel";
            _dataLabel.Size = new System.Drawing.Size(220, 15);
            _dataLabel.TabIndex = 1;
            _dataLabel.Text = "State of source items at selected version:";
            // 
            // _filesTreeView
            // 
            _filesTreeView.ContextMenuStrip = _treeContextMenuStrip;
            _filesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            _filesTreeView.ImageIndex = 0;
            _filesTreeView.ImageList = imagesList;
            _filesTreeView.Location = new System.Drawing.Point(3, 18);
            _filesTreeView.Name = "_filesTreeView";
            _filesTreeView.SelectedImageIndex = 0;
            _filesTreeView.Size = new System.Drawing.Size(780, 230);
            _filesTreeView.TabIndex = 2;
            _filesTreeView.MouseDown += OnMouseDown;
            // 
            // _treeContextMenuStrip
            // 
            _treeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { recoverToolStripMenuItem });
            _treeContextMenuStrip.Name = "_treeContextMenuStrip";
            _treeContextMenuStrip.Size = new System.Drawing.Size(126, 26);
            // 
            // recoverToolStripMenuItem
            // 
            recoverToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("recoverToolStripMenuItem.Image");
            recoverToolStripMenuItem.Name = "recoverToolStripMenuItem";
            recoverToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            recoverToolStripMenuItem.Text = "Recover...";
            recoverToolStripMenuItem.Click += OnRecover;
            // 
            // _changesTableLayoutPanel
            // 
            _changesTableLayoutPanel.ColumnCount = 1;
            _changesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _changesTableLayoutPanel.Controls.Add(_changesLabel, 0, 0);
            _changesTableLayoutPanel.Controls.Add(_changesListView, 0, 1);
            _changesTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _changesTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _changesTableLayoutPanel.Name = "_changesTableLayoutPanel";
            _changesTableLayoutPanel.RowCount = 2;
            _changesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _changesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _changesTableLayoutPanel.Size = new System.Drawing.Size(786, 252);
            _changesTableLayoutPanel.TabIndex = 1;
            // 
            // _changesLabel
            // 
            _changesLabel.AutoSize = true;
            _changesLabel.Location = new System.Drawing.Point(3, 0);
            _changesLabel.Name = "_changesLabel";
            _changesLabel.Size = new System.Drawing.Size(56, 15);
            _changesLabel.TabIndex = 0;
            _changesLabel.Text = "Changes:";
            // 
            // _changesListView
            // 
            _changesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            _changesListView.Location = new System.Drawing.Point(3, 18);
            _changesListView.Name = "_changesListView";
            _changesListView.Size = new System.Drawing.Size(780, 231);
            _changesListView.SmallImageList = imagesList;
            _changesListView.TabIndex = 1;
            _changesListView.UseCompatibleStateImageBehavior = false;
            _changesListView.View = System.Windows.Forms.View.List;
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
            _treeToChangesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_treeToChangesSplitContainer).EndInit();
            _treeToChangesSplitContainer.ResumeLayout(false);
            _filesTableLayoutPanel.ResumeLayout(false);
            _filesTableLayoutPanel.PerformLayout();
            _treeContextMenuStrip.ResumeLayout(false);
            _changesTableLayoutPanel.ResumeLayout(false);
            _changesTableLayoutPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ImageList imagesList;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _toolStripStatusLabel;
        private System.Windows.Forms.SplitContainer _versionToContentSplitContainer;
        private System.Windows.Forms.SplitContainer _treeToChangesSplitContainer;
        private System.Windows.Forms.TableLayoutPanel _filesTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel _changesTableLayoutPanel;
        private System.Windows.Forms.Label _dataLabel;
        private System.Windows.Forms.Label _changesLabel;
        private System.Windows.Forms.TreeView _filesTreeView;
        private System.Windows.Forms.ContextMenuStrip _treeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem recoverToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog _fbdialog;
        private System.Windows.Forms.ListView _changesListView;
        private System.Windows.Forms.ListView _versionsListView;
        private System.Windows.Forms.ToolStrip _versionsToolStrip;
        private System.Windows.Forms.ToolStripLabel _selectVersionToolStripLabel;
        private System.Windows.Forms.ToolStripLabel _storageToolStripLabel;
    }
}
