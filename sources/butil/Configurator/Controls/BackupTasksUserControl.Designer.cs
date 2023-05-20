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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupTasksUserControl));
            _tasksListView = new System.Windows.Forms.ListView();
            _nameColumn = new System.Windows.Forms.ColumnHeader();
            _contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            _executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            _removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _imageList = new System.Windows.Forms.ImageList(components);
            _executeButton = new System.Windows.Forms.Button();
            _addButton = new System.Windows.Forms.Button();
            _removeButton = new System.Windows.Forms.Button();
            _editButton = new System.Windows.Forms.Button();
            _contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _tasksListView
            // 
            _tasksListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _tasksListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { _nameColumn });
            _tasksListView.ContextMenuStrip = _contextMenuStrip;
            _tasksListView.FullRowSelect = true;
            _tasksListView.LargeImageList = _imageList;
            _tasksListView.Location = new System.Drawing.Point(4, 3);
            _tasksListView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _tasksListView.Name = "_tasksListView";
            _tasksListView.Size = new System.Drawing.Size(535, 326);
            _tasksListView.SmallImageList = _imageList;
            _tasksListView.TabIndex = 0;
            _tasksListView.UseCompatibleStateImageBehavior = false;
            _tasksListView.View = System.Windows.Forms.View.Details;
            _tasksListView.SelectedIndexChanged += RefreshTaskControls;
            _tasksListView.DoubleClick += ExecuteRequest;
            _tasksListView.Resize += OnTasksListViewResize;
            // 
            // _nameColumn
            // 
            _nameColumn.Text = "Name";
            _nameColumn.Width = 434;
            // 
            // _contextMenuStrip
            // 
            _contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _executeToolStripMenuItem, _addToolStripMenuItem, _editToolStripMenuItem, toolStripSeparator1, _removeToolStripMenuItem });
            _contextMenuStrip.Name = "_contextMenuStrip";
            _contextMenuStrip.Size = new System.Drawing.Size(125, 98);
            // 
            // _executeToolStripMenuItem
            // 
            _executeToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("_executeToolStripMenuItem.Image");
            _executeToolStripMenuItem.Name = "_executeToolStripMenuItem";
            _executeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            _executeToolStripMenuItem.Text = "Execute...";
            _executeToolStripMenuItem.Click += ExecuteRequest;
            // 
            // _addToolStripMenuItem
            // 
            _addToolStripMenuItem.Image = Icons.Add;
            _addToolStripMenuItem.Name = "_addToolStripMenuItem";
            _addToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            _addToolStripMenuItem.Text = "Add...";
            _addToolStripMenuItem.Click += AddTaskRequest;
            // 
            // _editToolStripMenuItem
            // 
            _editToolStripMenuItem.Image = Icons.OtherOptions48x48;
            _editToolStripMenuItem.Name = "_editToolStripMenuItem";
            _editToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            _editToolStripMenuItem.Text = "Edit...";
            _editToolStripMenuItem.Click += OnEditBackupTask;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // _removeToolStripMenuItem
            // 
            _removeToolStripMenuItem.Image = Icons.removeFromListToolStripMenuItem_Image;
            _removeToolStripMenuItem.Name = "_removeToolStripMenuItem";
            _removeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            _removeToolStripMenuItem.Text = "Remove";
            _removeToolStripMenuItem.Click += RemoveTaskRequest;
            // 
            // _imageList
            // 
            _imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            _imageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("_imageList.ImageStream");
            _imageList.TransparentColor = System.Drawing.Color.Transparent;
            _imageList.Images.SetKeyName(0, "BackupTask16x16.png");
            // 
            // _executeButton
            // 
            _executeButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _executeButton.Image = (System.Drawing.Image)resources.GetObject("_executeButton.Image");
            _executeButton.Location = new System.Drawing.Point(546, 6);
            _executeButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _executeButton.Name = "_executeButton";
            _executeButton.Size = new System.Drawing.Size(71, 67);
            _executeButton.TabIndex = 1;
            _executeButton.UseVisualStyleBackColor = true;
            _executeButton.Click += ExecuteRequest;
            // 
            // _addButton
            // 
            _addButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _addButton.Image = Icons.add_48;
            _addButton.Location = new System.Drawing.Point(546, 80);
            _addButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _addButton.Name = "_addButton";
            _addButton.Size = new System.Drawing.Size(71, 67);
            _addButton.TabIndex = 2;
            _addButton.UseVisualStyleBackColor = true;
            _addButton.Click += AddTaskRequest;
            // 
            // _removeButton
            // 
            _removeButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _removeButton.Image = Icons.cross_48;
            _removeButton.Location = new System.Drawing.Point(546, 227);
            _removeButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _removeButton.Name = "_removeButton";
            _removeButton.Size = new System.Drawing.Size(71, 67);
            _removeButton.TabIndex = 4;
            _removeButton.UseVisualStyleBackColor = true;
            _removeButton.Click += RemoveTaskRequest;
            // 
            // _editButton
            // 
            _editButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _editButton.Image = Icons.OtherOptions48x48;
            _editButton.Location = new System.Drawing.Point(546, 153);
            _editButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _editButton.Name = "_editButton";
            _editButton.Size = new System.Drawing.Size(71, 67);
            _editButton.TabIndex = 3;
            _editButton.UseVisualStyleBackColor = true;
            _editButton.Click += OnEditBackupTask;
            // 
            // BackupTasksUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_removeButton);
            Controls.Add(_editButton);
            Controls.Add(_executeButton);
            Controls.Add(_addButton);
            Controls.Add(_tasksListView);
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            Name = "BackupTasksUserControl";
            Size = new System.Drawing.Size(623, 333);
            _contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
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
        private System.Windows.Forms.ColumnHeader _nameColumn;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.ToolStripMenuItem _executeToolStripMenuItem;
        private System.Windows.Forms.Button _executeButton;
    }
}
