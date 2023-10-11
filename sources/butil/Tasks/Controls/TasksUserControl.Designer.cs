namespace BUtil.Configurator.Configurator.Controls
{
    partial class TasksUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TasksUserControl));
            _tasksListView = new System.Windows.Forms.ListView();
            _nameColumn = new System.Windows.Forms.ColumnHeader();
            _lastExecutionStateColumn = new System.Windows.Forms.ColumnHeader();
            _contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            _executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _createIncrementalBackupTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _createImportMultimediaTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            _removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _recoverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _imageList = new System.Windows.Forms.ImageList(components);
            _addButton = new System.Windows.Forms.Button();
            _createTaskContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            _createIncrementalBackupTaskToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            _createImportMultimediaTaskToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            _mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _rightTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _contextMenuStrip.SuspendLayout();
            _createTaskContextMenuStrip.SuspendLayout();
            _mainTableLayoutPanel.SuspendLayout();
            _rightTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _tasksListView
            // 
            _tasksListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _tasksListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { _nameColumn, _lastExecutionStateColumn });
            _tasksListView.ContextMenuStrip = _contextMenuStrip;
            _tasksListView.FullRowSelect = true;
            _tasksListView.LargeImageList = _imageList;
            _tasksListView.Location = new System.Drawing.Point(4, 3);
            _tasksListView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _tasksListView.Name = "_tasksListView";
            _tasksListView.Size = new System.Drawing.Size(536, 396);
            _tasksListView.SmallImageList = _imageList;
            _tasksListView.TabIndex = 0;
            _tasksListView.UseCompatibleStateImageBehavior = false;
            _tasksListView.View = System.Windows.Forms.View.Details;
            _tasksListView.SelectedIndexChanged += RefreshTaskControls;
            _tasksListView.DoubleClick += ExecuteRequest;
            // 
            // _nameColumn
            // 
            _nameColumn.Text = "Name";
            _nameColumn.Width = 404;
            // 
            // _lastExecutionStateColumn
            // 
            _lastExecutionStateColumn.Text = "State";
            _lastExecutionStateColumn.Width = 120;
            // 
            // _contextMenuStrip
            // 
            _contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            _contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _executeToolStripMenuItem, _addToolStripMenuItem, _editToolStripMenuItem, toolStripSeparator1, _removeToolStripMenuItem, _recoverToolStripMenuItem });
            _contextMenuStrip.Name = "_contextMenuStrip";
            _contextMenuStrip.Size = new System.Drawing.Size(134, 160);
            // 
            // _executeToolStripMenuItem
            // 
            _executeToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("_executeToolStripMenuItem.Image");
            _executeToolStripMenuItem.Name = "_executeToolStripMenuItem";
            _executeToolStripMenuItem.Size = new System.Drawing.Size(133, 30);
            _executeToolStripMenuItem.Text = "Execute...";
            _executeToolStripMenuItem.Click += ExecuteRequest;
            // 
            // _addToolStripMenuItem
            // 
            _addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _createIncrementalBackupTaskToolStripMenuItem, _createImportMultimediaTaskToolStripMenuItem });
            _addToolStripMenuItem.Image = Icons.Add;
            _addToolStripMenuItem.Name = "_addToolStripMenuItem";
            _addToolStripMenuItem.Size = new System.Drawing.Size(133, 30);
            _addToolStripMenuItem.Text = "Add...";
            // 
            // _createIncrementalBackupTaskToolStripMenuItem
            // 
            _createIncrementalBackupTaskToolStripMenuItem.Name = "_createIncrementalBackupTaskToolStripMenuItem";
            _createIncrementalBackupTaskToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            _createIncrementalBackupTaskToolStripMenuItem.Text = "Create incremental backup task";
            _createIncrementalBackupTaskToolStripMenuItem.Click += OnCreateIncrementalBackupTask;
            // 
            // _createImportMultimediaTaskToolStripMenuItem
            // 
            _createImportMultimediaTaskToolStripMenuItem.Name = "_createImportMultimediaTaskToolStripMenuItem";
            _createImportMultimediaTaskToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            _createImportMultimediaTaskToolStripMenuItem.Text = "Create import multimedia task";
            _createImportMultimediaTaskToolStripMenuItem.Click += OnCreateImportMultimediaTask;
            // 
            // _editToolStripMenuItem
            // 
            _editToolStripMenuItem.Image = Icons.OtherOptions48x48;
            _editToolStripMenuItem.Name = "_editToolStripMenuItem";
            _editToolStripMenuItem.Size = new System.Drawing.Size(133, 30);
            _editToolStripMenuItem.Text = "Edit...";
            _editToolStripMenuItem.Click += OnEditTask;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(130, 6);
            // 
            // _removeToolStripMenuItem
            // 
            _removeToolStripMenuItem.Image = Icons.removeFromListToolStripMenuItem_Image;
            _removeToolStripMenuItem.Name = "_removeToolStripMenuItem";
            _removeToolStripMenuItem.Size = new System.Drawing.Size(133, 30);
            _removeToolStripMenuItem.Text = "Remove";
            // 
            // _recoverToolStripMenuItem
            // 
            _recoverToolStripMenuItem.Image = Icons.Refresh48x48;
            _recoverToolStripMenuItem.Name = "_recoverToolStripMenuItem";
            _recoverToolStripMenuItem.Size = new System.Drawing.Size(133, 30);
            _recoverToolStripMenuItem.Text = "Recover...";
            // 
            // _imageList
            // 
            _imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            _imageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("_imageList.ImageStream");
            _imageList.TransparentColor = System.Drawing.Color.Transparent;
            _imageList.Images.SetKeyName(0, "BackupTask16x16.png");
            // 
            // _addButton
            // 
            _addButton.ContextMenuStrip = _createTaskContextMenuStrip;
            _addButton.Dock = System.Windows.Forms.DockStyle.Fill;
            _addButton.Image = Icons.add_48;
            _addButton.Location = new System.Drawing.Point(4, 3);
            _addButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _addButton.MaximumSize = new System.Drawing.Size(67, 58);
            _addButton.MinimumSize = new System.Drawing.Size(67, 58);
            _addButton.Name = "_addButton";
            _addButton.Size = new System.Drawing.Size(67, 58);
            _addButton.TabIndex = 2;
            _addButton.UseVisualStyleBackColor = true;
            _addButton.Click += OnAddButtonOpenMenu;
            // 
            // _createTaskContextMenuStrip
            // 
            _createTaskContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            _createTaskContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _createIncrementalBackupTaskToolStripMenuItem2, _createImportMultimediaTaskToolStripMenuItem2 });
            _createTaskContextMenuStrip.Name = "_createTaskContextMenuStrip";
            _createTaskContextMenuStrip.Size = new System.Drawing.Size(241, 48);
            // 
            // _createIncrementalBackupTaskToolStripMenuItem2
            // 
            _createIncrementalBackupTaskToolStripMenuItem2.Name = "_createIncrementalBackupTaskToolStripMenuItem2";
            _createIncrementalBackupTaskToolStripMenuItem2.Size = new System.Drawing.Size(240, 22);
            _createIncrementalBackupTaskToolStripMenuItem2.Text = "Create incremental backup task";
            _createIncrementalBackupTaskToolStripMenuItem2.Click += OnCreateIncrementalBackupTask;
            // 
            // _createImportMultimediaTaskToolStripMenuItem2
            // 
            _createImportMultimediaTaskToolStripMenuItem2.Name = "_createImportMultimediaTaskToolStripMenuItem2";
            _createImportMultimediaTaskToolStripMenuItem2.Size = new System.Drawing.Size(240, 22);
            _createImportMultimediaTaskToolStripMenuItem2.Text = "Create import multimedia task";
            _createImportMultimediaTaskToolStripMenuItem2.Click += OnCreateImportMultimediaTask;
            // 
            // _mainTableLayoutPanel
            // 
            _mainTableLayoutPanel.ColumnCount = 2;
            _mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _mainTableLayoutPanel.Controls.Add(_rightTableLayoutPanel, 1, 0);
            _mainTableLayoutPanel.Controls.Add(_tasksListView, 0, 0);
            _mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _mainTableLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            _mainTableLayoutPanel.Name = "_mainTableLayoutPanel";
            _mainTableLayoutPanel.RowCount = 1;
            _mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _mainTableLayoutPanel.Size = new System.Drawing.Size(623, 402);
            _mainTableLayoutPanel.TabIndex = 6;
            // 
            // _rightTableLayoutPanel
            // 
            _rightTableLayoutPanel.AutoSize = true;
            _rightTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _rightTableLayoutPanel.ColumnCount = 1;
            _rightTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _rightTableLayoutPanel.Controls.Add(_addButton, 0, 1);
            _rightTableLayoutPanel.Location = new System.Drawing.Point(546, 2);
            _rightTableLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            _rightTableLayoutPanel.Name = "_rightTableLayoutPanel";
            _rightTableLayoutPanel.RowCount = 6;
            _rightTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _rightTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _rightTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _rightTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _rightTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _rightTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _rightTableLayoutPanel.Size = new System.Drawing.Size(75, 64);
            _rightTableLayoutPanel.TabIndex = 7;
            // 
            // TasksUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_mainTableLayoutPanel);
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            Name = "TasksUserControl";
            Size = new System.Drawing.Size(623, 402);
            _contextMenuStrip.ResumeLayout(false);
            _createTaskContextMenuStrip.ResumeLayout(false);
            _mainTableLayoutPanel.ResumeLayout(false);
            _mainTableLayoutPanel.PerformLayout();
            _rightTableLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView _tasksListView;
        private System.Windows.Forms.Button _addButton;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ColumnHeader _nameColumn;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.ToolStripMenuItem _executeToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader _lastExecutionStateColumn;
        private System.Windows.Forms.ToolStripMenuItem _recoverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _createIncrementalBackupTaskToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _createImportMultimediaTaskToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip _createTaskContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _createIncrementalBackupTaskToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem _createImportMultimediaTaskToolStripMenuItem2;
        private System.Windows.Forms.TableLayoutPanel _mainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel _rightTableLayoutPanel;
    }
}
