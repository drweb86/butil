
namespace BUtil.Configurator.Controls
{
	partial class TasksChainToExecuteUserControl
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
            this.headerGroupBox = new System.Windows.Forms.GroupBox();
            this.tasksListView = new System.Windows.Forms.ListView();
            this.programColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.argumentsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headerGroupBox.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerGroupBox
            // 
            this.headerGroupBox.Controls.Add(this.editButton);
            this.headerGroupBox.Controls.Add(this.moveDownButton);
            this.headerGroupBox.Controls.Add(this.moveUpButton);
            this.headerGroupBox.Controls.Add(this.removeButton);
            this.headerGroupBox.Controls.Add(this.addButton);
            this.headerGroupBox.Controls.Add(this.tasksListView);
            this.headerGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerGroupBox.Location = new System.Drawing.Point(0, 0);
            this.headerGroupBox.Name = "headerGroupBox";
            this.headerGroupBox.Size = new System.Drawing.Size(411, 338);
            this.headerGroupBox.TabIndex = 0;
            this.headerGroupBox.TabStop = false;
            this.headerGroupBox.Text = "<Header>";
            // 
            // tasksListView
            // 
            this.tasksListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tasksListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.programColumnHeader,
            this.argumentsColumnHeader});
            this.tasksListView.ContextMenuStrip = this.contextMenuStrip;
            this.tasksListView.FullRowSelect = true;
            this.tasksListView.Location = new System.Drawing.Point(6, 19);
            this.tasksListView.Name = "tasksListView";
            this.tasksListView.Size = new System.Drawing.Size(332, 313);
            this.tasksListView.TabIndex = 0;
            this.tasksListView.UseCompatibleStateImageBehavior = false;
            this.tasksListView.View = System.Windows.Forms.View.Details;
            this.tasksListView.SelectedIndexChanged += new System.EventHandler(this.tasksListViewSelectedIndexChanged);
            this.tasksListView.DoubleClick += new System.EventHandler(this.editSelectedItem);
            this.tasksListView.Resize += new System.EventHandler(this.tasksListViewResize);
            // 
            // programColumnHeader
            // 
            this.programColumnHeader.Text = "Program";
            // 
            // argumentsColumnHeader
            // 
            this.argumentsColumnHeader.Text = "Arguments";
            this.argumentsColumnHeader.Width = 100;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.editToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(139, 114);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editButton.Image = global::BUtil.Configurator.Icons.OtherOptions48x48;
            this.editButton.Location = new System.Drawing.Point(344, 83);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(61, 58);
            this.editButton.TabIndex = 2;
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editSelectedItem);
            // 
            // moveDownButton
            // 
            this.moveDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveDownButton.Image = global::BUtil.Configurator.Icons.ArrayDown;
            this.moveDownButton.Location = new System.Drawing.Point(344, 275);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(61, 58);
            this.moveDownButton.TabIndex = 5;
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.moveDownSelectedItem);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveUpButton.Image = global::BUtil.Configurator.Icons.ArrayUp;
            this.moveUpButton.Location = new System.Drawing.Point(344, 211);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(61, 58);
            this.moveUpButton.TabIndex = 4;
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.moveUpSelectedItem);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.Image = global::BUtil.Configurator.Icons.cross_48;
            this.removeButton.Location = new System.Drawing.Point(344, 147);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(61, 58);
            this.removeButton.TabIndex = 3;
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeSelectedItems);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::BUtil.Configurator.Icons.Add48x48;
            this.addButton.Location = new System.Drawing.Point(344, 19);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(61, 58);
            this.addButton.TabIndex = 1;
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addItem);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Image = global::BUtil.Configurator.Icons.addNewToolStripMenuItem_Image;
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.addToolStripMenuItem.Text = "Add...";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addItem);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = global::BUtil.Configurator.Icons.OtherOptions48x48;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.editToolStripMenuItem.Text = "Edit...";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editSelectedItem);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Image = global::BUtil.Configurator.Icons.removeFromListToolStripMenuItem_Image;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedItems);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Image = global::BUtil.Configurator.Icons.ArrayUp;
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpSelectedItem);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Image = global::BUtil.Configurator.Icons.ArrayDown;
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownSelectedItem);
            // 
            // TasksChainToExecuteUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.headerGroupBox);
            this.Name = "TasksChainToExecuteUserControl";
            this.Size = new System.Drawing.Size(411, 338);
            this.headerGroupBox.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.Button editButton;
		private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ColumnHeader argumentsColumnHeader;
		private System.Windows.Forms.ColumnHeader programColumnHeader;
		private System.Windows.Forms.ListView tasksListView;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button moveUpButton;
		private System.Windows.Forms.Button moveDownButton;
		private System.Windows.Forms.GroupBox headerGroupBox;
	}
}
