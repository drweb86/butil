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
            toolTip = new System.Windows.Forms.ToolTip(components);
            helpToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            helpStatusStrip = new System.Windows.Forms.StatusStrip();
            _backupTasksUserControl = new TasksUserControl();
            helpStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // toolTip
            // 
            toolTip.IsBalloon = true;
            // 
            // helpToolStripStatusLabel
            // 
            helpToolStripStatusLabel.Name = "helpToolStripStatusLabel";
            helpToolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // helpStatusStrip
            // 
            helpStatusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            helpStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { helpToolStripStatusLabel });
            helpStatusStrip.Location = new System.Drawing.Point(0, 351);
            helpStatusStrip.Name = "helpStatusStrip";
            helpStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            helpStatusStrip.Size = new System.Drawing.Size(820, 22);
            helpStatusStrip.TabIndex = 6;
            helpStatusStrip.Text = "statusStrip1";
            // 
            // _backupTasksUserControl
            // 
            _backupTasksUserControl.BackColor = System.Drawing.SystemColors.Window;
            _backupTasksUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            _backupTasksUserControl.HelpLabel = null;
            _backupTasksUserControl.Location = new System.Drawing.Point(0, 0);
            _backupTasksUserControl.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            _backupTasksUserControl.Name = "_backupTasksUserControl";
            _backupTasksUserControl.Size = new System.Drawing.Size(820, 351);
            _backupTasksUserControl.TabIndex = 7;
            // 
            // TasksForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new System.Drawing.Size(820, 373);
            Controls.Add(_backupTasksUserControl);
            Controls.Add(helpStatusStrip);
            Icon = Icons.BUtilIcon;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(425, 412);
            Name = "TasksForm";
            Text = "Configurator";
            helpStatusStrip.ResumeLayout(false);
            helpStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ToolStripStatusLabel helpToolStripStatusLabel;
        private System.Windows.Forms.StatusStrip helpStatusStrip;
        private System.Windows.Forms.ToolTip toolTip;
        private TasksUserControl _backupTasksUserControl;
    }
}
