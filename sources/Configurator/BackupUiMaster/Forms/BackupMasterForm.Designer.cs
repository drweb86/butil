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
            this.startButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tasksListView = new System.Windows.Forms.ListView();
            this.taskNameColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.processingStateInformationColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.compressionItemsListViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.settingsUserControl = new BUtil.Configurator.BackupUiMaster.Controls.SettingsUserControl();
            this.cancelButton = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.abortBackupBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this._backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.helpButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.startButton.Image = ((System.Drawing.Image)(resources.GetObject("startButton.Image")));
            this.startButton.Location = new System.Drawing.Point(492, 481);
            this.startButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(88, 31);
            this.startButton.TabIndex = 11;
            this.toolTip.SetToolTip(this.startButton, "Start");
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButtonClick);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(681, 481);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(90, 31);
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
            this.tasksListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.taskNameColumnHeader,
            this.processingStateInformationColumnHeader});
            this.tasksListView.FullRowSelect = true;
            this.tasksListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tasksListView.LargeImageList = this.compressionItemsListViewImageList;
            this.tasksListView.Location = new System.Drawing.Point(4, 14);
            this.tasksListView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tasksListView.Name = "tasksListView";
            this.tasksListView.Size = new System.Drawing.Size(767, 247);
            this.tasksListView.SmallImageList = this.compressionItemsListViewImageList;
            this.tasksListView.TabIndex = 0;
            this.tasksListView.UseCompatibleStateImageBehavior = false;
            this.tasksListView.View = System.Windows.Forms.View.Details;
            this.tasksListView.Resize += new System.EventHandler(this.OnTasksListViewResize);
            // 
            // taskNameColumnHeader
            // 
            this.taskNameColumnHeader.Text = "Location";
            this.taskNameColumnHeader.Width = 288;
            // 
            // processingStateInformationColumnHeader
            // 
            this.processingStateInformationColumnHeader.Text = "Processing";
            this.processingStateInformationColumnHeader.Width = 154;
            // 
            // compressionItemsListViewImageList
            // 
            this.compressionItemsListViewImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
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
            this.settingsUserControl.Location = new System.Drawing.Point(4, 269);
            this.settingsUserControl.Margin = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.settingsUserControl.MinimumSize = new System.Drawing.Size(764, 194);
            this.settingsUserControl.Name = "settingsUserControl";
            this.settingsUserControl.Size = new System.Drawing.Size(768, 205);
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
            this.cancelButton.Location = new System.Drawing.Point(587, 481);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 31);
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
            this._backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.OnDoWork);
            this._backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.OnRunWorkerCompleted);
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpButton.Location = new System.Drawing.Point(8, 478);
            this.helpButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(38, 38);
            this.helpButton.TabIndex = 20;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.HelpButtonClick);
            // 
            // BackupMasterForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(785, 522);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.settingsUserControl);
            this.Controls.Add(this.tasksListView);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.startButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(792, 408);
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
		private System.ComponentModel.BackgroundWorker _backgroundWorker;
		private System.ComponentModel.BackgroundWorker abortBackupBackgroundWorker;
		private System.Windows.Forms.ColumnHeader taskNameColumnHeader;
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
