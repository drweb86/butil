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
            _mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _afterCompletionTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _bottomTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _mainTableLayoutPanel.SuspendLayout();
            _afterCompletionTableLayoutPanel.SuspendLayout();
            _bottomTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // startButton
            // 
            startButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            startButton.Dock = System.Windows.Forms.DockStyle.Fill;
            startButton.Image = (System.Drawing.Image)resources.GetObject("startButton.Image");
            startButton.Location = new System.Drawing.Point(701, 5);
            startButton.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            startButton.MinimumSize = new System.Drawing.Size(128, 64);
            startButton.Name = "startButton";
            startButton.Size = new System.Drawing.Size(128, 72);
            startButton.TabIndex = 11;
            toolTip.SetToolTip(startButton, "Start");
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += StartButtonClick;
            // 
            // closeButton
            // 
            closeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            closeButton.Location = new System.Drawing.Point(981, 5);
            closeButton.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            closeButton.MinimumSize = new System.Drawing.Size(128, 64);
            closeButton.Name = "closeButton";
            closeButton.Size = new System.Drawing.Size(128, 72);
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
            tasksListView.Location = new System.Drawing.Point(6, 5);
            tasksListView.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            tasksListView.Name = "tasksListView";
            tasksListView.Size = new System.Drawing.Size(1109, 589);
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
            cancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            cancelButton.Enabled = false;
            cancelButton.Image = (System.Drawing.Image)resources.GetObject("cancelButton.Image");
            cancelButton.Location = new System.Drawing.Point(841, 5);
            cancelButton.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            cancelButton.MinimumSize = new System.Drawing.Size(128, 64);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(128, 72);
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
            _powerTaskComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _powerTaskComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _powerTaskComboBox.FormattingEnabled = true;
            _powerTaskComboBox.Location = new System.Drawing.Point(6, 32);
            _powerTaskComboBox.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            _powerTaskComboBox.Name = "_powerTaskComboBox";
            _powerTaskComboBox.Size = new System.Drawing.Size(284, 33);
            _powerTaskComboBox.TabIndex = 3;
            // 
            // _powerTaskLinkLabel
            // 
            _powerTaskLinkLabel.AutoSize = true;
            _powerTaskLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _powerTaskLinkLabel.Location = new System.Drawing.Point(4, 0);
            _powerTaskLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _powerTaskLinkLabel.Name = "_powerTaskLinkLabel";
            _powerTaskLinkLabel.Size = new System.Drawing.Size(288, 25);
            _powerTaskLinkLabel.TabIndex = 22;
            _powerTaskLinkLabel.TabStop = true;
            _powerTaskLinkLabel.Text = "After completion of backup";
            _powerTaskLinkLabel.LinkClicked += OnShowHelp;
            // 
            // backupProgressUserControl
            // 
            backupProgressUserControl.BackColor = System.Drawing.SystemColors.Window;
            backupProgressUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            backupProgressUserControl.HelpLabel = null;
            backupProgressUserControl.Location = new System.Drawing.Point(9, 604);
            backupProgressUserControl.Margin = new System.Windows.Forms.Padding(9, 5, 9, 5);
            backupProgressUserControl.MinimumSize = new System.Drawing.Size(837, 173);
            backupProgressUserControl.Name = "backupProgressUserControl";
            backupProgressUserControl.Size = new System.Drawing.Size(1103, 173);
            backupProgressUserControl.TabIndex = 23;
            backupProgressUserControl.Title = "";
            backupProgressUserControl.TitleBackground = System.Drawing.Color.DodgerBlue;
            // 
            // _mainTableLayoutPanel
            // 
            _mainTableLayoutPanel.ColumnCount = 1;
            _mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _mainTableLayoutPanel.Controls.Add(_bottomTableLayoutPanel, 0, 2);
            _mainTableLayoutPanel.Controls.Add(tasksListView, 0, 0);
            _mainTableLayoutPanel.Controls.Add(backupProgressUserControl, 0, 1);
            _mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _mainTableLayoutPanel.Name = "_mainTableLayoutPanel";
            _mainTableLayoutPanel.RowCount = 3;
            _mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _mainTableLayoutPanel.Size = new System.Drawing.Size(1121, 870);
            _mainTableLayoutPanel.TabIndex = 24;
            // 
            // _afterCompletionTableLayoutPanel
            // 
            _afterCompletionTableLayoutPanel.AutoSize = true;
            _afterCompletionTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _afterCompletionTableLayoutPanel.ColumnCount = 1;
            _afterCompletionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _afterCompletionTableLayoutPanel.Controls.Add(_powerTaskLinkLabel, 0, 0);
            _afterCompletionTableLayoutPanel.Controls.Add(_powerTaskComboBox, 0, 1);
            _afterCompletionTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            _afterCompletionTableLayoutPanel.Name = "_afterCompletionTableLayoutPanel";
            _afterCompletionTableLayoutPanel.RowCount = 2;
            _afterCompletionTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _afterCompletionTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _afterCompletionTableLayoutPanel.Size = new System.Drawing.Size(296, 72);
            _afterCompletionTableLayoutPanel.TabIndex = 25;
            // 
            // _bottomTableLayoutPanel
            // 
            _bottomTableLayoutPanel.AutoSize = true;
            _bottomTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _bottomTableLayoutPanel.ColumnCount = 5;
            _bottomTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _bottomTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _bottomTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _bottomTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _bottomTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _bottomTableLayoutPanel.Controls.Add(_afterCompletionTableLayoutPanel, 0, 0);
            _bottomTableLayoutPanel.Controls.Add(closeButton, 4, 0);
            _bottomTableLayoutPanel.Controls.Add(startButton, 2, 0);
            _bottomTableLayoutPanel.Controls.Add(cancelButton, 3, 0);
            _bottomTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _bottomTableLayoutPanel.Location = new System.Drawing.Point(3, 785);
            _bottomTableLayoutPanel.Name = "_bottomTableLayoutPanel";
            _bottomTableLayoutPanel.RowCount = 1;
            _bottomTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _bottomTableLayoutPanel.Size = new System.Drawing.Size(1115, 82);
            _bottomTableLayoutPanel.TabIndex = 26;
            // 
            // TaskProgressForm
            // 
            AcceptButton = startButton;
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(1121, 870);
            Controls.Add(_mainTableLayoutPanel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            MinimumSize = new System.Drawing.Size(1122, 643);
            Name = "TaskProgressForm";
            Text = "Wellcome to Backup Wizard!";
            FormClosing += ClosingForm;
            Load += LoadForm;
            _mainTableLayoutPanel.ResumeLayout(false);
            _mainTableLayoutPanel.PerformLayout();
            _afterCompletionTableLayoutPanel.ResumeLayout(false);
            _afterCompletionTableLayoutPanel.PerformLayout();
            _bottomTableLayoutPanel.ResumeLayout(false);
            _bottomTableLayoutPanel.PerformLayout();
            ResumeLayout(false);
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
        private System.Windows.Forms.TableLayoutPanel _mainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel _afterCompletionTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel _bottomTableLayoutPanel;
    }
}
