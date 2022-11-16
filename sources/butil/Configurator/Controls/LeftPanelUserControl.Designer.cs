
namespace BUtil.Configurator.Configurator.Controls
{
	partial class LeftPanelUserControl
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
            this._otherOptionsButton = new System.Windows.Forms.Button();
            this._loggingButton = new System.Windows.Forms.Button();
            this._tasksButton = new System.Windows.Forms.Button();
            this._tableLayoutPanel = new BUtil.Configurator.Configurator.Controls.Common.TransparentTableLayoutPanel();
            this._tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _otherOptionsButton
            // 
            this._otherOptionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._otherOptionsButton.BackColor = System.Drawing.SystemColors.Control;
            this._otherOptionsButton.FlatAppearance.BorderSize = 0;
            this._otherOptionsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._otherOptionsButton.Image = global::BUtil.Configurator.Icons.OtherSettings48x48;
            this._otherOptionsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._otherOptionsButton.Location = new System.Drawing.Point(4, 103);
            this._otherOptionsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._otherOptionsButton.Name = "_otherOptionsButton";
            this._otherOptionsButton.Size = new System.Drawing.Size(261, 88);
            this._otherOptionsButton.TabIndex = 1;
            this._otherOptionsButton.Text = "Other options";
            this._otherOptionsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._otherOptionsButton.UseVisualStyleBackColor = false;
            this._otherOptionsButton.Click += new System.EventHandler(this.OtherOptionsButtonClick);
            // 
            // _loggingButton
            // 
            this._loggingButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._loggingButton.BackColor = System.Drawing.SystemColors.Control;
            this._loggingButton.FlatAppearance.BorderSize = 0;
            this._loggingButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._loggingButton.Image = global::BUtil.Configurator.Icons.app_48;
            this._loggingButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._loggingButton.Location = new System.Drawing.Point(4, 201);
            this._loggingButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._loggingButton.Name = "_loggingButton";
            this._loggingButton.Size = new System.Drawing.Size(261, 88);
            this._loggingButton.TabIndex = 2;
            this._loggingButton.Text = "Logging";
            this._loggingButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._loggingButton.UseVisualStyleBackColor = false;
            this._loggingButton.Click += new System.EventHandler(this.LoggingButtonClick);
            // 
            // _tasksButton
            // 
            this._tasksButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tasksButton.BackColor = System.Drawing.SystemColors.Control;
            this._tasksButton.FlatAppearance.BorderSize = 0;
            this._tasksButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._tasksButton.Image = global::BUtil.Configurator.Icons.BackupTask48x48;
            this._tasksButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._tasksButton.Location = new System.Drawing.Point(4, 5);
            this._tasksButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._tasksButton.Name = "_tasksButton";
            this._tasksButton.Size = new System.Drawing.Size(261, 88);
            this._tasksButton.TabIndex = 0;
            this._tasksButton.Text = "Tasks";
            this._tasksButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._tasksButton.UseVisualStyleBackColor = false;
            this._tasksButton.Click += new System.EventHandler(this.TasksShowRequest);
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.ColumnCount = 1;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.Controls.Add(this._tasksButton, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._loggingButton, 0, 2);
            this._tableLayoutPanel.Controls.Add(this._otherOptionsButton, 0, 1);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 4;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.Size = new System.Drawing.Size(269, 450);
            this._tableLayoutPanel.TabIndex = 3;
            // 
            // LeftPanelUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::BUtil.Configurator.Icons.BackgroundImage;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this._tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.MinimumSize = new System.Drawing.Size(184, 403);
            this.Name = "LeftPanelUserControl";
            this.Size = new System.Drawing.Size(269, 450);
            this.Load += new System.EventHandler(this.LeftPanelUserControlLoad);
            this._tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		private System.Windows.Forms.Button _loggingButton;
        private System.Windows.Forms.Button _otherOptionsButton;
        private System.Windows.Forms.Button _tasksButton;
        private Common.TransparentTableLayoutPanel _tableLayoutPanel;
    }
}
