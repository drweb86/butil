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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionsViewerForm));
            this.imagesList = new System.Windows.Forms.ImageList(this.components);
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._versionToContentSplitContainer = new System.Windows.Forms.SplitContainer();
            this._versionsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._versionsListBox = new System.Windows.Forms.ListBox();
            this._versionsLabel = new System.Windows.Forms.Label();
            this._treeToChangesSplitContainer = new System.Windows.Forms.SplitContainer();
            this._filesTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._dataLabel = new System.Windows.Forms.Label();
            this._filesTreeView = new System.Windows.Forms.TreeView();
            this._treeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.recoverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._changesTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._changesLabel = new System.Windows.Forms.Label();
            this._changesTextBox = new System.Windows.Forms.TextBox();
            this._fbdialog = new System.Windows.Forms.FolderBrowserDialog();
            this._statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._versionToContentSplitContainer)).BeginInit();
            this._versionToContentSplitContainer.Panel1.SuspendLayout();
            this._versionToContentSplitContainer.Panel2.SuspendLayout();
            this._versionToContentSplitContainer.SuspendLayout();
            this._versionsTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._treeToChangesSplitContainer)).BeginInit();
            this._treeToChangesSplitContainer.Panel1.SuspendLayout();
            this._treeToChangesSplitContainer.Panel2.SuspendLayout();
            this._treeToChangesSplitContainer.SuspendLayout();
            this._filesTableLayoutPanel.SuspendLayout();
            this._treeContextMenuStrip.SuspendLayout();
            this._changesTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // imagesList
            // 
            this.imagesList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imagesList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagesList.ImageStream")));
            this.imagesList.TransparentColor = System.Drawing.Color.White;
            this.imagesList.Images.SetKeyName(0, "16x16 Binary.png");
            this.imagesList.Images.SetKeyName(1, "16x16 Folder.png");
            // 
            // _statusStrip
            // 
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripStatusLabel});
            this._statusStrip.Location = new System.Drawing.Point(0, 507);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(1079, 22);
            this._statusStrip.TabIndex = 0;
            // 
            // _toolStripStatusLabel
            // 
            this._toolStripStatusLabel.Name = "_toolStripStatusLabel";
            this._toolStripStatusLabel.Size = new System.Drawing.Size(383, 17);
            this._toolStripStatusLabel.Text = "Click on item you want to restore and open context menu by right click";
            // 
            // _versionToContentSplitContainer
            // 
            this._versionToContentSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._versionToContentSplitContainer.Location = new System.Drawing.Point(0, 0);
            this._versionToContentSplitContainer.Name = "_versionToContentSplitContainer";
            // 
            // _versionToContentSplitContainer.Panel1
            // 
            this._versionToContentSplitContainer.Panel1.Controls.Add(this._versionsTableLayoutPanel);
            // 
            // _versionToContentSplitContainer.Panel2
            // 
            this._versionToContentSplitContainer.Panel2.Controls.Add(this._treeToChangesSplitContainer);
            this._versionToContentSplitContainer.Size = new System.Drawing.Size(1079, 507);
            this._versionToContentSplitContainer.SplitterDistance = 289;
            this._versionToContentSplitContainer.TabIndex = 1;
            // 
            // _versionsTableLayoutPanel
            // 
            this._versionsTableLayoutPanel.ColumnCount = 1;
            this._versionsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._versionsTableLayoutPanel.Controls.Add(this._versionsListBox, 0, 1);
            this._versionsTableLayoutPanel.Controls.Add(this._versionsLabel, 0, 0);
            this._versionsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._versionsTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._versionsTableLayoutPanel.Name = "_versionsTableLayoutPanel";
            this._versionsTableLayoutPanel.RowCount = 2;
            this._versionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._versionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._versionsTableLayoutPanel.Size = new System.Drawing.Size(289, 507);
            this._versionsTableLayoutPanel.TabIndex = 0;
            // 
            // _versionsListBox
            // 
            this._versionsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._versionsListBox.FormattingEnabled = true;
            this._versionsListBox.ItemHeight = 15;
            this._versionsListBox.Location = new System.Drawing.Point(3, 18);
            this._versionsListBox.Name = "_versionsListBox";
            this._versionsListBox.Size = new System.Drawing.Size(283, 486);
            this._versionsListBox.TabIndex = 0;
            this._versionsListBox.SelectedIndexChanged += new System.EventHandler(this.OnVersionListChange);
            // 
            // _versionsLabel
            // 
            this._versionsLabel.AutoSize = true;
            this._versionsLabel.Location = new System.Drawing.Point(3, 0);
            this._versionsLabel.Name = "_versionsLabel";
            this._versionsLabel.Size = new System.Drawing.Size(82, 15);
            this._versionsLabel.TabIndex = 1;
            this._versionsLabel.Text = "Select version:";
            // 
            // _treeToChangesSplitContainer
            // 
            this._treeToChangesSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeToChangesSplitContainer.Location = new System.Drawing.Point(0, 0);
            this._treeToChangesSplitContainer.Name = "_treeToChangesSplitContainer";
            this._treeToChangesSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _treeToChangesSplitContainer.Panel1
            // 
            this._treeToChangesSplitContainer.Panel1.Controls.Add(this._filesTableLayoutPanel);
            // 
            // _treeToChangesSplitContainer.Panel2
            // 
            this._treeToChangesSplitContainer.Panel2.Controls.Add(this._changesTableLayoutPanel);
            this._treeToChangesSplitContainer.Size = new System.Drawing.Size(786, 507);
            this._treeToChangesSplitContainer.SplitterDistance = 251;
            this._treeToChangesSplitContainer.TabIndex = 0;
            // 
            // _filesTableLayoutPanel
            // 
            this._filesTableLayoutPanel.ColumnCount = 1;
            this._filesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._filesTableLayoutPanel.Controls.Add(this._dataLabel, 0, 0);
            this._filesTableLayoutPanel.Controls.Add(this._filesTreeView, 0, 1);
            this._filesTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._filesTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._filesTableLayoutPanel.Name = "_filesTableLayoutPanel";
            this._filesTableLayoutPanel.RowCount = 2;
            this._filesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._filesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._filesTableLayoutPanel.Size = new System.Drawing.Size(786, 251);
            this._filesTableLayoutPanel.TabIndex = 0;
            // 
            // _dataLabel
            // 
            this._dataLabel.AutoSize = true;
            this._dataLabel.Location = new System.Drawing.Point(3, 0);
            this._dataLabel.Name = "_dataLabel";
            this._dataLabel.Size = new System.Drawing.Size(220, 15);
            this._dataLabel.TabIndex = 1;
            this._dataLabel.Text = "State of source items at selected version:";
            // 
            // _filesTreeView
            // 
            this._filesTreeView.ContextMenuStrip = this._treeContextMenuStrip;
            this._filesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._filesTreeView.Location = new System.Drawing.Point(3, 18);
            this._filesTreeView.Name = "_filesTreeView";
            this._filesTreeView.Size = new System.Drawing.Size(780, 230);
            this._filesTreeView.TabIndex = 2;
            // 
            // _treeContextMenuStrip
            // 
            this._treeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recoverToolStripMenuItem});
            this._treeContextMenuStrip.Name = "_treeContextMenuStrip";
            this._treeContextMenuStrip.Size = new System.Drawing.Size(126, 26);
            // 
            // recoverToolStripMenuItem
            // 
            this.recoverToolStripMenuItem.Name = "recoverToolStripMenuItem";
            this.recoverToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.recoverToolStripMenuItem.Text = "Recover...";
            this.recoverToolStripMenuItem.Click += new System.EventHandler(this.OnRecover);
            // 
            // _changesTableLayoutPanel
            // 
            this._changesTableLayoutPanel.ColumnCount = 1;
            this._changesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._changesTableLayoutPanel.Controls.Add(this._changesLabel, 0, 0);
            this._changesTableLayoutPanel.Controls.Add(this._changesTextBox, 0, 1);
            this._changesTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._changesTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._changesTableLayoutPanel.Name = "_changesTableLayoutPanel";
            this._changesTableLayoutPanel.RowCount = 2;
            this._changesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._changesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._changesTableLayoutPanel.Size = new System.Drawing.Size(786, 252);
            this._changesTableLayoutPanel.TabIndex = 1;
            // 
            // _changesLabel
            // 
            this._changesLabel.AutoSize = true;
            this._changesLabel.Location = new System.Drawing.Point(3, 0);
            this._changesLabel.Name = "_changesLabel";
            this._changesLabel.Size = new System.Drawing.Size(56, 15);
            this._changesLabel.TabIndex = 0;
            this._changesLabel.Text = "Changes:";
            // 
            // _changesTextBox
            // 
            this._changesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._changesTextBox.Location = new System.Drawing.Point(3, 18);
            this._changesTextBox.Multiline = true;
            this._changesTextBox.Name = "_changesTextBox";
            this._changesTextBox.ReadOnly = true;
            this._changesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._changesTextBox.Size = new System.Drawing.Size(780, 231);
            this._changesTextBox.TabIndex = 1;
            // 
            // VersionsViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 529);
            this.Controls.Add(this._versionToContentSplitContainer);
            this.Controls.Add(this._statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(437, 301);
            this.Name = "VersionsViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Restoration";
            this.Load += new System.EventHandler(this.OnLoad);
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this._versionToContentSplitContainer.Panel1.ResumeLayout(false);
            this._versionToContentSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._versionToContentSplitContainer)).EndInit();
            this._versionToContentSplitContainer.ResumeLayout(false);
            this._versionsTableLayoutPanel.ResumeLayout(false);
            this._versionsTableLayoutPanel.PerformLayout();
            this._treeToChangesSplitContainer.Panel1.ResumeLayout(false);
            this._treeToChangesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._treeToChangesSplitContainer)).EndInit();
            this._treeToChangesSplitContainer.ResumeLayout(false);
            this._filesTableLayoutPanel.ResumeLayout(false);
            this._filesTableLayoutPanel.PerformLayout();
            this._treeContextMenuStrip.ResumeLayout(false);
            this._changesTableLayoutPanel.ResumeLayout(false);
            this._changesTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ImageList imagesList;
		private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _toolStripStatusLabel;
        private System.Windows.Forms.SplitContainer _versionToContentSplitContainer;
        private System.Windows.Forms.TableLayoutPanel _versionsTableLayoutPanel;
        private System.Windows.Forms.ListBox _versionsListBox;
        private System.Windows.Forms.Label _versionsLabel;
        private System.Windows.Forms.SplitContainer _treeToChangesSplitContainer;
        private System.Windows.Forms.TableLayoutPanel _filesTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel _changesTableLayoutPanel;
        private System.Windows.Forms.Label _dataLabel;
        private System.Windows.Forms.Label _changesLabel;
        private System.Windows.Forms.TreeView _filesTreeView;
        private System.Windows.Forms.ContextMenuStrip _treeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem recoverToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog _fbdialog;
        private System.Windows.Forms.TextBox _changesTextBox;
    }
}
