using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BUtil.Core;
using BUtil.Ghost.Localization;

namespace BUtil.Ghost
{
    internal class WithTray :IDisposable
    {
    	#region Fields
    	
        readonly Controller _controller;
        
        readonly string _Start; //"Start"
        readonly string _Cancel; //"Cancel";
        readonly string _Exit; //"Exit";

        #endregion
        
        #region Constructors
        
        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="controller">The Scheduler Conntroller</param>
        /// <exception cref="ArgumentNullException">Controller is null</exception>
        public WithTray(Controller controller)
        {
        	if (controller == null)
        	{
        		throw new ArgumentNullException("controller");
        	}
        	
            _controller = controller;
            
            //locals
            _Start = Resources.Start;
            _Cancel = Resources.Cancel;
            _Exit = Resources.Exit;
            //--

            InitializeComponent();

			_trayIcon.Text = _controller.GetSchedulingState();
            if (Program.RunAsWinFormApp)
            {
	            _controller.OnNotificationEvent += new BackupNotification(onNotificationReceived);
            }            
        }
		
		#endregion
        
        #region Public Methods
        
        /// <summary>
        /// Hides the tray icon
        /// </summary>
        public void TurnIntoHiddenMode()
        {
        	//TODO: still not tested
        	_trayIcon.Visible = false;
        }
        
		#endregion
        
		#region Private Nethods
		
        void onNotificationReceived(string text)
        {
            //-- this is a bug fix and should be removed when microsoft will fix their class
			_trayIcon.Visible = false;
			_trayIcon.Visible = true;
			//--  this is a bug fix and should be removed when microsoft will fix their class
			_trayIcon.BalloonTipIcon = ToolTipIcon.Info;
			_trayIcon.BalloonTipText = text;
            _trayIcon.BalloonTipTitle = Resources.BackupToolButil;

			_trayIcon.ShowBalloonTip(3000);
        }
        
        void cancelBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controller.AbortBackup();
        }

        void startBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: fixme
            _controller.DoBackup(null, false);
        }

        void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _trayIcon.Visible = false;
            Application.Exit();
        }

        void _trayIcon_MouseMove(object sender, MouseEventArgs e)
        {
            _trayIcon.Text = _controller.GetSchedulingState();
        }

        void _TrayIconMenuContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            // here's a next problem - with menu - it doesnot clears itself from clipboard
            lock (_TrayIconMenuContextMenuStrip)
            {
                cancelBackupToolStripMenuItem.Enabled = _controller.IsBackupInProgress();
                startBackupToolStripMenuItem.Enabled = !_controller.IsBackupInProgress();
            }
        }
		
        #endregion
		
        #region IDisposable implementation

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
            	if (Program.RunAsWinFormApp)
            	{
	            	_controller.OnNotificationEvent -= new BackupNotification(onNotificationReceived);
            	}

                _trayIcon.Dispose();
                exitToolStripMenuItem.Dispose();
                startBackupToolStripMenuItem.Dispose();
                cancelBackupToolStripMenuItem.Dispose();
                _TrayIconMenuContextMenuStrip.Dispose();
                toolStripSeparator1.Dispose();

                components.Dispose();
                
            }
        }

        #endregion

        #region Initialization code for menu and tray icon

        System.ComponentModel.IContainer components;

        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WithTray));
            this._trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this._TrayIconMenuContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._TrayIconMenuContextMenuStrip.SuspendLayout();
            // 
            // _trayIcon
            // 
            this._trayIcon.ContextMenuStrip = this._TrayIconMenuContextMenuStrip;
            this._trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("_trayIcon.Icon")));
            this._trayIcon.Text = string.Empty;
            this._trayIcon.Visible = true;
            this._trayIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(this._trayIcon_MouseMove);
            // 
            // _TrayIconMenuContextMenuStrip
            // 
            this._TrayIconMenuContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startBackupToolStripMenuItem,
            this.cancelBackupToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            //this._TrayIconMenuContextMenuStrip.Name = "_TrayIconMenuContextMenuStrip";
            this._TrayIconMenuContextMenuStrip.Size = new System.Drawing.Size(153, 98);
            this._TrayIconMenuContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this._TrayIconMenuContextMenuStrip_Opening);

            // startBackupToolStripMenuItem
            //this.startBackupToolStripMenuItem.Name = "startBackupToolStripMenuItem";
            this.startBackupToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.startBackupToolStripMenuItem.Text = _Start;
            this.startBackupToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("Start")));
            this.startBackupToolStripMenuItem.Click += new System.EventHandler(this.startBackupToolStripMenuItem_Click);

            // cancelBackupToolStripMenuItem
            //this.cancelBackupToolStripMenuItem.Name = "cancelBackupToolStripMenuItem";
            this.cancelBackupToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.cancelBackupToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("Stop")));
            this.cancelBackupToolStripMenuItem.Text = _Cancel;
            this.cancelBackupToolStripMenuItem.Click += new System.EventHandler(this.cancelBackupToolStripMenuItem_Click);

            // toolStripSeparator1
            //this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);

            // exitToolStripMenuItem
            // this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = _Exit;
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);

            this._TrayIconMenuContextMenuStrip.ResumeLayout(false);
        }

        NotifyIcon _trayIcon;
        ContextMenuStrip _TrayIconMenuContextMenuStrip;
        ToolStripSeparator toolStripSeparator1;
        ToolStripMenuItem exitToolStripMenuItem;
        ToolStripMenuItem cancelBackupToolStripMenuItem;
        ToolStripMenuItem startBackupToolStripMenuItem;

        #endregion
    }
}
