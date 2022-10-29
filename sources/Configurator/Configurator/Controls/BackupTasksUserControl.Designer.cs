namespace BUtil.Configurator.Configurator.Controls
{
    partial class BackupTasksUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupTasksUserControl));
            this._tasksListView = new System.Windows.Forms.ListView();
            this.titleColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._imageList = new System.Windows.Forms.ImageList(this.components);
            this._executeButton = new System.Windows.Forms.Button();
            this._addButton = new System.Windows.Forms.Button();
            this._removeButton = new System.Windows.Forms.Button();
            this._editButton = new System.Windows.Forms.Button();
            this._contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tasksListView
            // 
            this._tasksListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tasksListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.titleColumnHeader});
            this._tasksListView.ContextMenuStrip = this._contextMenuStrip;
            this._tasksListView.FullRowSelect = true;
            this._tasksListView.LargeImageList = this._imageList;
            this._tasksListView.Location = new System.Drawing.Point(3, 3);
            this._tasksListView.Name = "_tasksListView";
            this._tasksListView.Size = new System.Drawing.Size(459, 283);
            this._tasksListView.SmallImageList = this._imageList;
            this._tasksListView.TabIndex = 0;
            this._tasksListView.UseCompatibleStateImageBehavior = false;
            this._tasksListView.View = System.Windows.Forms.View.Details;
            this._tasksListView.SelectedIndexChanged += new System.EventHandler(this.RefreshTaskControls);
            this._tasksListView.DoubleClick += new System.EventHandler(this.OnEditBackupTask);
            this._tasksListView.Resize += new System.EventHandler(this.OnTasksListViewResize);
            // 
            // titleColumnHeader
            // 
            this.titleColumnHeader.Text = "Title";
            this.titleColumnHeader.Width = 434;
            // 
            // _contextMenuStrip
            // 
            this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._executeToolStripMenuItem,
            this._addToolStripMenuItem,
            this._editToolStripMenuItem,
            this.toolStripSeparator1,
            this._removeToolStripMenuItem});
            this._contextMenuStrip.Name = "_contextMenuStrip";
            this._contextMenuStrip.Size = new System.Drawing.Size(124, 98);
            // 
            // _executeToolStripMenuItem
            // 
            this._executeToolStripMenuItem.Image = global::BUtil.Configurator.Icons.Start;
            this._executeToolStripMenuItem.Name = "_executeToolStripMenuItem";
            this._executeToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this._executeToolStripMenuItem.Text = "Execute...";
            this._executeToolStripMenuItem.Click += new System.EventHandler(this.ExecuteRequest);
            // 
            // _addToolStripMenuItem
            // 
            this._addToolStripMenuItem.Image = global::BUtil.Configurator.Icons.Add;
            this._addToolStripMenuItem.Name = "_addToolStripMenuItem";
            this._addToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this._addToolStripMenuItem.Text = "Add...";
            this._addToolStripMenuItem.Click += new System.EventHandler(this.AddTaskRequest);
            // 
            // _editToolStripMenuItem
            // 
            this._editToolStripMenuItem.Image = global::BUtil.Configurator.Icons.OtherOptions48x48;
            this._editToolStripMenuItem.Name = "_editToolStripMenuItem";
            this._editToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this._editToolStripMenuItem.Text = "Edit...";
            this._editToolStripMenuItem.Click += new System.EventHandler(this.OnEditBackupTask);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(120, 6);
            // 
            // _removeToolStripMenuItem
            // 
            this._removeToolStripMenuItem.Image = global::BUtil.Configurator.Icons.removeFromListToolStripMenuItem_Image;
            this._removeToolStripMenuItem.Name = "_removeToolStripMenuItem";
            this._removeToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this._removeToolStripMenuItem.Text = "Remove";
            this._removeToolStripMenuItem.Click += new System.EventHandler(this.RemoveTaskRequest);
            // 
            // _imageList
            // 
            this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
            this._imageList.TransparentColor = System.Drawing.Color.Transparent;
            this._imageList.Images.SetKeyName(0, "BackupTask16x16.png");
            // 
            // _executeButton
            // 
            this._executeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._executeButton.Image = global::BUtil.Configurator.Icons.Start48x48;
            this._executeButton.Location = new System.Drawing.Point(468, 5);
            this._executeButton.Name = "_executeButton";
            this._executeButton.Size = new System.Drawing.Size(61, 58);
            this._executeButton.TabIndex = 1;
            this._executeButton.UseVisualStyleBackColor = true;
            this._executeButton.Click += new System.EventHandler(this.ExecuteRequest);
            // 
            // _addButton
            // 
            this._addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._addButton.Image = global::BUtil.Configurator.Icons.add_48;
            this._addButton.Location = new System.Drawing.Point(468, 69);
            this._addButton.Name = "_addButton";
            this._addButton.Size = new System.Drawing.Size(61, 58);
            this._addButton.TabIndex = 2;
            this._addButton.UseVisualStyleBackColor = true;
            this._addButton.Click += new System.EventHandler(this.AddTaskRequest);
            // 
            // _removeButton
            // 
            this._removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._removeButton.Image = global::BUtil.Configurator.Icons.cross_48;
            this._removeButton.Location = new System.Drawing.Point(468, 197);
            this._removeButton.Name = "_removeButton";
            this._removeButton.Size = new System.Drawing.Size(61, 58);
            this._removeButton.TabIndex = 4;
            this._removeButton.UseVisualStyleBackColor = true;
            this._removeButton.Click += new System.EventHandler(this.RemoveTaskRequest);
            // 
            // _editButton
            // 
            this._editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._editButton.Image = global::BUtil.Configurator.Icons.OtherOptions48x48;
            this._editButton.Location = new System.Drawing.Point(468, 133);
            this._editButton.Name = "_editButton";
            this._editButton.Size = new System.Drawing.Size(61, 58);
            this._editButton.TabIndex = 3;
            this._editButton.UseVisualStyleBackColor = true;
            this._editButton.Click += new System.EventHandler(this.OnEditBackupTask);
            // 
            // BackupTasksUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._removeButton);
            this.Controls.Add(this._editButton);
            this.Controls.Add(this._executeButton);
            this.Controls.Add(this._addButton);
            this.Controls.Add(this._tasksListView);
            this.Name = "BackupTasksUserControl";
            this.Size = new System.Drawing.Size(534, 289);
            this._contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView _tasksListView;
        private System.Windows.Forms.Button _addButton;
        private System.Windows.Forms.Button _editButton;
        private System.Windows.Forms.Button _removeButton;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ColumnHeader titleColumnHeader;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.ToolStripMenuItem _executeToolStripMenuItem;
        private System.Windows.Forms.Button _executeButton;
    }
}
