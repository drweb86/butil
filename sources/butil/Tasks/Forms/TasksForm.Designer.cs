using BUtil.Configurator.Configurator.Controls;

namespace BUtil.Configurator.Configurator.Forms
{
    partial class TasksForm : System.Windows.Forms.Form
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

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TasksForm));
            MainmenuStrip = new System.Windows.Forms.MenuStrip();
            _logsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            restorationToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolTip = new System.Windows.Forms.ToolTip(components);
            helpToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            helpStatusStrip = new System.Windows.Forms.StatusStrip();
            _backupTasksUserControl = new TasksUserControl();
            MainmenuStrip.SuspendLayout();
            helpStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // MainmenuStrip
            // 
            MainmenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            MainmenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _logsToolStripMenuItem, restorationToolToolStripMenuItem, _helpToolStripMenuItem });
            MainmenuStrip.Location = new System.Drawing.Point(0, 0);
            MainmenuStrip.Name = "MainmenuStrip";
            MainmenuStrip.Padding = new System.Windows.Forms.Padding(10, 3, 0, 3);
            MainmenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            MainmenuStrip.Size = new System.Drawing.Size(1172, 35);
            MainmenuStrip.TabIndex = 3;
            MainmenuStrip.Text = "MainmenuStrip";
            // 
            // _logsToolStripMenuItem
            // 
            _logsToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("_logsToolStripMenuItem.Image");
            _logsToolStripMenuItem.Name = "_logsToolStripMenuItem";
            _logsToolStripMenuItem.Size = new System.Drawing.Size(90, 29);
            _logsToolStripMenuItem.Text = "Logs";
            _logsToolStripMenuItem.Click += OnOpenLogsFolder;
            // 
            // restorationToolToolStripMenuItem
            // 
            restorationToolToolStripMenuItem.Image = Icons.Refresh48x48;
            restorationToolToolStripMenuItem.Name = "restorationToolToolStripMenuItem";
            restorationToolToolStripMenuItem.Size = new System.Drawing.Size(142, 29);
            restorationToolToolStripMenuItem.Text = "Restoration";
            restorationToolToolStripMenuItem.Click += OnOpenRestorationApp;
            // 
            // _helpToolStripMenuItem
            // 
            _helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
            _helpToolStripMenuItem.Size = new System.Drawing.Size(36, 29);
            _helpToolStripMenuItem.Text = "?";
            _helpToolStripMenuItem.Click += OnOpenHomePage;
            // 
            // toolTip
            // 
            toolTip.IsBalloon = true;
            // 
            // helpToolStripStatusLabel
            // 
            helpToolStripStatusLabel.Name = "helpToolStripStatusLabel";
            helpToolStripStatusLabel.Size = new System.Drawing.Size(0, 15);
            // 
            // helpStatusStrip
            // 
            helpStatusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            helpStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { helpToolStripStatusLabel });
            helpStatusStrip.Location = new System.Drawing.Point(0, 582);
            helpStatusStrip.Name = "helpStatusStrip";
            helpStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 23, 0);
            helpStatusStrip.Size = new System.Drawing.Size(1172, 22);
            helpStatusStrip.TabIndex = 6;
            helpStatusStrip.Text = "statusStrip1";
            // 
            // _backupTasksUserControl
            // 
            _backupTasksUserControl.BackColor = System.Drawing.SystemColors.Window;
            _backupTasksUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            _backupTasksUserControl.HelpLabel = null;
            _backupTasksUserControl.Location = new System.Drawing.Point(0, 35);
            _backupTasksUserControl.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            _backupTasksUserControl.Name = "_backupTasksUserControl";
            _backupTasksUserControl.Size = new System.Drawing.Size(1172, 547);
            _backupTasksUserControl.TabIndex = 7;
            // 
            // TasksForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new System.Drawing.Size(1172, 604);
            Controls.Add(_backupTasksUserControl);
            Controls.Add(helpStatusStrip);
            Controls.Add(MainmenuStrip);
            Icon = Icons.BUtilIcon;
            MainMenuStrip = MainmenuStrip;
            Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            MinimumSize = new System.Drawing.Size(600, 660);
            Name = "TasksForm";
            Text = "Configurator";
            MainmenuStrip.ResumeLayout(false);
            MainmenuStrip.PerformLayout();
            helpStatusStrip.ResumeLayout(false);
            helpStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ToolStripMenuItem _helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel helpToolStripStatusLabel;
        private System.Windows.Forms.StatusStrip helpStatusStrip;
        private System.Windows.Forms.MenuStrip MainmenuStrip;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem restorationToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _logsToolStripMenuItem;
        private TasksUserControl _backupTasksUserControl;
    }
}
