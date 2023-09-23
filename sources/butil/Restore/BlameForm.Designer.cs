namespace BUtil.Configurator.Restore
{
    partial class BlameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlameForm));
            _versionsListView = new System.Windows.Forms.ListView();
            _contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            _openSelectedVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _imageList = new System.Windows.Forms.ImageList(components);
            _closeButton = new System.Windows.Forms.Button();
            _mainToolStrip = new System.Windows.Forms.ToolStrip();
            _versionsListToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            _openSelectedVersionToolStripButton = new System.Windows.Forms.ToolStripButton();
            _contextMenuStrip.SuspendLayout();
            _mainToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _versionsListView
            // 
            _versionsListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _versionsListView.ContextMenuStrip = _contextMenuStrip;
            _versionsListView.Location = new System.Drawing.Point(17, 47);
            _versionsListView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            _versionsListView.Name = "_versionsListView";
            _versionsListView.Size = new System.Drawing.Size(590, 681);
            _versionsListView.SmallImageList = _imageList;
            _versionsListView.TabIndex = 0;
            _versionsListView.UseCompatibleStateImageBehavior = false;
            _versionsListView.View = System.Windows.Forms.View.List;
            _versionsListView.DoubleClick += OnOpenSelectedVersion;
            // 
            // _contextMenuStrip
            // 
            _contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            _contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _openSelectedVersionToolStripMenuItem });
            _contextMenuStrip.Name = "_contextMenuStrip";
            _contextMenuStrip.Size = new System.Drawing.Size(272, 36);
            // 
            // _openSelectedVersionToolStripMenuItem
            // 
            _openSelectedVersionToolStripMenuItem.Name = "_openSelectedVersionToolStripMenuItem";
            _openSelectedVersionToolStripMenuItem.Size = new System.Drawing.Size(271, 32);
            _openSelectedVersionToolStripMenuItem.Text = "Open selected version...";
            _openSelectedVersionToolStripMenuItem.Click += OnOpenSelectedVersion;
            // 
            // _imageList
            // 
            _imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            _imageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("_imageList.ImageStream");
            _imageList.TransparentColor = System.Drawing.Color.Transparent;
            _imageList.Images.SetKeyName(0, "VC-Created.png");
            _imageList.Images.SetKeyName(1, "VC-Deleted.png");
            _imageList.Images.SetKeyName(2, "VC-Updated.png");
            // 
            // _closeButton
            // 
            _closeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            _closeButton.Location = new System.Drawing.Point(501, 740);
            _closeButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            _closeButton.Name = "_closeButton";
            _closeButton.Size = new System.Drawing.Size(107, 38);
            _closeButton.TabIndex = 1;
            _closeButton.Text = "Close";
            _closeButton.UseVisualStyleBackColor = true;
            _closeButton.Click += OnClose;
            // 
            // _mainToolStrip
            // 
            _mainToolStrip.CanOverflow = false;
            _mainToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            _mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _versionsListToolStripLabel, _openSelectedVersionToolStripButton });
            _mainToolStrip.Location = new System.Drawing.Point(0, 0);
            _mainToolStrip.Name = "_mainToolStrip";
            _mainToolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            _mainToolStrip.Size = new System.Drawing.Size(626, 34);
            _mainToolStrip.TabIndex = 2;
            _mainToolStrip.Text = "toolStrip1";
            // 
            // _versionsListToolStripLabel
            // 
            _versionsListToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            _versionsListToolStripLabel.Name = "_versionsListToolStripLabel";
            _versionsListToolStripLabel.Size = new System.Drawing.Size(100, 29);
            _versionsListToolStripLabel.Text = "VERSIONS";
            // 
            // _openSelectedVersionToolStripButton
            // 
            _openSelectedVersionToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            _openSelectedVersionToolStripButton.Image = (System.Drawing.Image)resources.GetObject("_openSelectedVersionToolStripButton.Image");
            _openSelectedVersionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _openSelectedVersionToolStripButton.Name = "_openSelectedVersionToolStripButton";
            _openSelectedVersionToolStripButton.Size = new System.Drawing.Size(203, 29);
            _openSelectedVersionToolStripButton.Text = "Open selected version...";
            _openSelectedVersionToolStripButton.Click += OnOpenSelectedVersion;
            // 
            // BlameForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(626, 798);
            Controls.Add(_mainToolStrip);
            Controls.Add(_closeButton);
            Controls.Add(_versionsListView);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "BlameForm";
            ShowIcon = false;
            Text = "BlameForm";
            _contextMenuStrip.ResumeLayout(false);
            _mainToolStrip.ResumeLayout(false);
            _mainToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView _versionsListView;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.ToolStrip _mainToolStrip;
        private System.Windows.Forms.ToolStripLabel _versionsListToolStripLabel;
        private System.Windows.Forms.ToolStripButton _openSelectedVersionToolStripButton;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _openSelectedVersionToolStripMenuItem;
    }
}