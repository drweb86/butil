
namespace BUtil.Configurator.Controls
{
    partial class OtherTaskOptionsUserControl
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
            this.beforeBackupEventsControl = new BUtil.Configurator.Controls.ProgramsToExecuteUserControl();
            this._afterBackupTasksChainToExecuteUserControl = new BUtil.Configurator.Controls.ProgramsToExecuteUserControl();
            this.SuspendLayout();
            // 
            // beforeBackupEventsControl
            // 
            this.beforeBackupEventsControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.beforeBackupEventsControl.Location = new System.Drawing.Point(3, 6);
            this.beforeBackupEventsControl.Name = "beforeBackupEventsControl";
            this.beforeBackupEventsControl.Size = new System.Drawing.Size(485, 339);
            this.beforeBackupEventsControl.TabIndex = 1;
            // 
            // _afterBackupTasksChainToExecuteUserControl
            // 
            this._afterBackupTasksChainToExecuteUserControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._afterBackupTasksChainToExecuteUserControl.Location = new System.Drawing.Point(3, 351);
            this._afterBackupTasksChainToExecuteUserControl.Name = "_afterBackupTasksChainToExecuteUserControl";
            this._afterBackupTasksChainToExecuteUserControl.Size = new System.Drawing.Size(485, 340);
            this._afterBackupTasksChainToExecuteUserControl.TabIndex = 2;
            // 
            // TaskOtherOptionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._afterBackupTasksChainToExecuteUserControl);
            this.Controls.Add(this.beforeBackupEventsControl);
            this.MinimumSize = new System.Drawing.Size(491, 708);
            this.Name = "TaskOtherOptionsUserControl";
            this.Size = new System.Drawing.Size(491, 708);
            this.ResumeLayout(false);

		}
		private BUtil.Configurator.Controls.ProgramsToExecuteUserControl _afterBackupTasksChainToExecuteUserControl;
        private BUtil.Configurator.Controls.ProgramsToExecuteUserControl beforeBackupEventsControl;
	}
}
