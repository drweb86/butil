using BUtil.Configurator.Common;

namespace BUtil.Configurator.BackupUiMaster.Forms
{
    partial class TaskProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskProgressForm));
            startButton = new System.Windows.Forms.Button();
            closeButton = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            tasksListView = new ListViewDoubleBuffered();
            taskNameColumnHeader = new System.Windows.Forms.ColumnHeader();
            processingStateInformationColumnHeader = new System.Windows.Forms.ColumnHeader();
            compressionItemsListViewImageList = new System.Windows.Forms.ImageList(components);
            cancelButton = new System.Windows.Forms.Button();
            _backgroundWorker = new System.ComponentModel.BackgroundWorker();
            _listViewUpdateTimer = new System.Windows.Forms.Timer(components);
            _powerTaskComboBox = new System.Windows.Forms.ComboBox();
            _powerTaskLinkLabel = new System.Windows.Forms.LinkLabel();
            backupProgressUserControl = new BUtil.BackupUiMaster.Controls.TaskProgressUserControl();
            SuspendLayout();
            // 
            // startButton
            // 
            startButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            startButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            startButton.Image = (System.Drawing.Image)resources.GetObject("startButton.Image");
            startButton.Location = new System.Drawing.Point(492, 469);
            startButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            startButton.Name = "startButton";
            startButton.Size = new System.Drawing.Size(88, 43);
            startButton.TabIndex = 11;
            toolTip.SetToolTip(startButton, "Start");
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += StartButtonClick;
            // 
            // closeButton
            // 
            closeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            closeButton.Location = new System.Drawing.Point(681, 469);
            closeButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            closeButton.Name = "closeButton";
            closeButton.Size = new System.Drawing.Size(90, 43);
            closeButton.TabIndex = 12;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += CloseButtonClick;
            // 
            // tasksListView
            // 
            tasksListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tasksListView.BackColor = System.Drawing.SystemColors.Window;
            tasksListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { taskNameColumnHeader, processingStateInformationColumnHeader });
            tasksListView.FullRowSelect = true;
            tasksListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            tasksListView.LargeImageList = compressionItemsListViewImageList;
            tasksListView.Location = new System.Drawing.Point(4, 14);
            tasksListView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tasksListView.Name = "tasksListView";
            tasksListView.Size = new System.Drawing.Size(767, 339);
            tasksListView.SmallImageList = compressionItemsListViewImageList;
            tasksListView.TabIndex = 0;
            tasksListView.UseCompatibleStateImageBehavior = false;
            tasksListView.View = System.Windows.Forms.View.Details;
            tasksListView.VirtualListSize = 10000;
            tasksListView.VirtualMode = true;
            tasksListView.RetrieveVirtualItem += OnRetrieveVirtualItem;
            tasksListView.Resize += OnTasksListViewResize;
            // 
            // taskNameColumnHeader
            // 
            taskNameColumnHeader.Text = "Location";
            taskNameColumnHeader.Width = 288;
            // 
            // processingStateInformationColumnHeader
            // 
            processingStateInformationColumnHeader.Text = " ";
            processingStateInformationColumnHeader.Width = 50;
            // 
            // compressionItemsListViewImageList
            // 
            compressionItemsListViewImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            compressionItemsListViewImageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("compressionItemsListViewImageList.ImageStream");
            compressionItemsListViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            compressionItemsListViewImageList.Images.SetKeyName(0, "folder_48.png");
            compressionItemsListViewImageList.Images.SetKeyName(1, "New.ico");
            compressionItemsListViewImageList.Images.SetKeyName(2, "Ftp16x16.png");
            compressionItemsListViewImageList.Images.SetKeyName(3, "Hdd16x16.png");
            compressionItemsListViewImageList.Images.SetKeyName(4, "Share16x16.png");
            compressionItemsListViewImageList.Images.SetKeyName(5, "otherTask.png");
            compressionItemsListViewImageList.Images.SetKeyName(6, "runProgram.png");
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Enabled = false;
            cancelButton.Image = (System.Drawing.Image)resources.GetObject("cancelButton.Image");
            cancelButton.Location = new System.Drawing.Point(587, 469);
            cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(88, 43);
            cancelButton.TabIndex = 19;
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButtonClick;
            // 
            // _backgroundWorker
            // 
            _backgroundWorker.DoWork += OnDoWork;
            _backgroundWorker.RunWorkerCompleted += OnRunWorkerCompleted;
            // 
            // _listViewUpdateTimer
            // 
            _listViewUpdateTimer.Enabled = true;
            _listViewUpdateTimer.Interval = 5000;
            _listViewUpdateTimer.Tick += OnListViewFlushUpdates;
            // 
            // _powerTaskComboBox
            // 
            _powerTaskComboBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            _powerTaskComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _powerTaskComboBox.FormattingEnabled = true;
            _powerTaskComboBox.Location = new System.Drawing.Point(4, 489);
            _powerTaskComboBox.Margin = new System.Windows.Forms.Padding(4);
            _powerTaskComboBox.Name = "_powerTaskComboBox";
            _powerTaskComboBox.Size = new System.Drawing.Size(480, 23);
            _powerTaskComboBox.TabIndex = 3;
            // 
            // _powerTaskLinkLabel
            // 
            _powerTaskLinkLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            _powerTaskLinkLabel.AutoSize = true;
            _powerTaskLinkLabel.Location = new System.Drawing.Point(4, 470);
            _powerTaskLinkLabel.Name = "_powerTaskLinkLabel";
            _powerTaskLinkLabel.Size = new System.Drawing.Size(153, 15);
            _powerTaskLinkLabel.TabIndex = 22;
            _powerTaskLinkLabel.TabStop = true;
            _powerTaskLinkLabel.Text = "After completion of backup";
            _powerTaskLinkLabel.LinkClicked += OnShowHelp;
            // 
            // backupProgressUserControl
            // 
            backupProgressUserControl.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            backupProgressUserControl.BackColor = System.Drawing.SystemColors.Window;
            backupProgressUserControl.HelpLabel = null;
            backupProgressUserControl.Location = new System.Drawing.Point(4, 359);
            backupProgressUserControl.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            backupProgressUserControl.MinimumSize = new System.Drawing.Size(586, 104);
            backupProgressUserControl.Name = "backupProgressUserControl";
            backupProgressUserControl.Size = new System.Drawing.Size(766, 104);
            backupProgressUserControl.TabIndex = 23;
            backupProgressUserControl.Title = "";
            // 
            // BackupMasterForm
            // 
            AcceptButton = startButton;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(785, 522);
            Controls.Add(backupProgressUserControl);
            Controls.Add(_powerTaskLinkLabel);
            Controls.Add(_powerTaskComboBox);
            Controls.Add(cancelButton);
            Controls.Add(tasksListView);
            Controls.Add(closeButton);
            Controls.Add(startButton);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(792, 408);
            Name = "BackupMasterForm";
            Text = "Wellcome to Backup Wizard!";
            FormClosing += ClosingForm;
            Load += LoadForm;
            ResumeLayout(false);
            PerformLayout();
        }

        private System.ComponentModel.BackgroundWorker _backgroundWorker;
        private System.Windows.Forms.ColumnHeader taskNameColumnHeader;
        private System.Windows.Forms.ColumnHeader processingStateInformationColumnHeader;
        private ListViewDoubleBuffered tasksListView;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ImageList compressionItemsListViewImageList;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Timer _listViewUpdateTimer;
        private System.Windows.Forms.ComboBox _powerTaskComboBox;
        private System.Windows.Forms.LinkLabel _powerTaskLinkLabel;
        private BUtil.BackupUiMaster.Controls.TaskProgressUserControl backupProgressUserControl;
    }
}
