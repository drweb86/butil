using BUtil.Configurator.BackupUiMaster.Controls;

namespace BUtil.Configurator.BackupUiMaster.Forms
{
	partial class BackupMasterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupMasterForm));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Packing Folders", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Packing Files", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Copying To Storages", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Other Tasks", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Before backup programs chain", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("After backup programs chain", System.Windows.Forms.HorizontalAlignment.Left);
            this.startButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tasksListView = new System.Windows.Forms.ListView();
            this.taskNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.informationAboutTaskColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.processingStateInformationColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.compressionItemsListViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.settingsUserControl = new BUtil.Configurator.BackupUiMaster.Controls.SettingsUserControl();
            this.cancelButton = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.abortBackupBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.backupBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.helpButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.startButton.Image = ((System.Drawing.Image)(resources.GetObject("startButton.Image")));
            this.startButton.Location = new System.Drawing.Point(422, 417);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 27);
            this.startButton.TabIndex = 11;
            this.toolTip.SetToolTip(this.startButton, "Start");
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButtonClick);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(584, 417);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(77, 27);
            this.closeButton.TabIndex = 12;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // tasksListView
            // 
            this.tasksListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tasksListView.BackColor = System.Drawing.SystemColors.Window;
            this.tasksListView.CheckBoxes = true;
            this.tasksListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.taskNameColumnHeader,
            this.informationAboutTaskColumnHeader,
            this.processingStateInformationColumnHeader});
            this.tasksListView.FullRowSelect = true;
            listViewGroup1.Header = "Packing Folders";
            listViewGroup1.Name = "packingFoldersListViewGroup";
            listViewGroup2.Header = "Packing Files";
            listViewGroup2.Name = "packingFilesListViewGroup";
            listViewGroup3.Header = "Copying To Storages";
            listViewGroup3.Name = "copyingToStoragesListViewGroup";
            listViewGroup4.Header = "Other Tasks";
            listViewGroup4.Name = "otherTaksListViewGroup";
            listViewGroup5.Header = "Before backup programs chain";
            listViewGroup5.Name = "beforeBackupListViewGroup";
            listViewGroup6.Header = "After backup programs chain";
            listViewGroup6.Name = "afterBackupProgramsChainListViewGroup";
            this.tasksListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6});
            this.tasksListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tasksListView.LargeImageList = this.compressionItemsListViewImageList;
            this.tasksListView.Location = new System.Drawing.Point(3, 12);
            this.tasksListView.Name = "tasksListView";
            this.tasksListView.Size = new System.Drawing.Size(658, 215);
            this.tasksListView.SmallImageList = this.compressionItemsListViewImageList;
            this.tasksListView.TabIndex = 0;
            this.tasksListView.UseCompatibleStateImageBehavior = false;
            this.tasksListView.View = System.Windows.Forms.View.Details;
            this.tasksListView.Resize += new System.EventHandler(this.CompressionItemsListViewResize);
            // 
            // taskNameColumnHeader
            // 
            this.taskNameColumnHeader.Text = "Location";
            this.taskNameColumnHeader.Width = 288;
            // 
            // informationAboutTaskColumnHeader
            // 
            this.informationAboutTaskColumnHeader.Text = "Compression";
            this.informationAboutTaskColumnHeader.Width = 180;
            // 
            // processingStateInformationColumnHeader
            // 
            this.processingStateInformationColumnHeader.Text = "Processing";
            this.processingStateInformationColumnHeader.Width = 154;
            // 
            // compressionItemsListViewImageList
            // 
            this.compressionItemsListViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("compressionItemsListViewImageList.ImageStream")));
            this.compressionItemsListViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.compressionItemsListViewImageList.Images.SetKeyName(0, "folder_48.png");
            this.compressionItemsListViewImageList.Images.SetKeyName(1, "New.ico");
            this.compressionItemsListViewImageList.Images.SetKeyName(2, "Ftp16x16.png");
            this.compressionItemsListViewImageList.Images.SetKeyName(3, "Hdd16x16.png");
            this.compressionItemsListViewImageList.Images.SetKeyName(4, "Share16x16.png");
            this.compressionItemsListViewImageList.Images.SetKeyName(5, "otherTask.png");
            this.compressionItemsListViewImageList.Images.SetKeyName(6, "runProgram.png");
            // 
            // settingsUserControl
            // 
            this.settingsUserControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsUserControl.BackColor = System.Drawing.SystemColors.Window;
            this.settingsUserControl.HelpLabel = null;
            this.settingsUserControl.Location = new System.Drawing.Point(3, 233);
            this.settingsUserControl.MinimumSize = new System.Drawing.Size(655, 168);
            this.settingsUserControl.Name = "settingsUserControl";
            this.settingsUserControl.Size = new System.Drawing.Size(658, 178);
            this.settingsUserControl.TabIndex = 18;
            this.settingsUserControl.Title = "Settings";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Enabled = false;
            this.cancelButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelButton.Image")));
            this.cancelButton.Location = new System.Drawing.Point(503, 417);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 27);
            this.cancelButton.TabIndex = 19;
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Backup is in progress...";
            this.notifyIcon.Click += new System.EventHandler(this.NotifyIconClick);
            // 
            // abortBackupBackgroundWorker
            // 
            this.abortBackupBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AbortBackupBackgroundWorkerDoWork);
            this.abortBackupBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AbortBackupBackgroundWorkerRunWorkerCompleted);
            // 
            // backupBackgroundWorker
            // 
            this.backupBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackupBackgroundWorkerDoWork);
            this.backupBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackupBackgroundWorkerRunWorkerCompleted);
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpButton.Location = new System.Drawing.Point(7, 414);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(33, 33);
            this.helpButton.TabIndex = 20;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.HelpButtonClick);
            // 
            // BackupMasterForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(673, 452);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.settingsUserControl);
            this.Controls.Add(this.tasksListView);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.startButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(681, 359);
            this.Name = "BackupMasterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wellcome to Backup Wizard!";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingForm);
            this.Load += new System.EventHandler(this.LoadForm);
            this.Resize += new System.EventHandler(this.ResizeForm);
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Button helpButton;
		private System.ComponentModel.BackgroundWorker backupBackgroundWorker;
		private System.ComponentModel.BackgroundWorker abortBackupBackgroundWorker;
		private System.Windows.Forms.ColumnHeader taskNameColumnHeader;
		private System.Windows.Forms.ColumnHeader informationAboutTaskColumnHeader;
		private System.Windows.Forms.ColumnHeader processingStateInformationColumnHeader;
		private System.Windows.Forms.ListView tasksListView;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.Button cancelButton;
		private SettingsUserControl settingsUserControl;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.ImageList compressionItemsListViewImageList;
		private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ToolTip toolTip;
	}
}
