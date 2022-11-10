
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
            this.beforeBackupEventsControl = new BUtil.Configurator.Controls.ExecuteProgramTaskInfoListControl();
            this._afterBackupTasksChainToExecuteUserControl = new BUtil.Configurator.Controls.ExecuteProgramTaskInfoListControl();
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // beforeBackupEventsControl
            // 
            this.beforeBackupEventsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.beforeBackupEventsControl.Location = new System.Drawing.Point(0, 0);
            this.beforeBackupEventsControl.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.beforeBackupEventsControl.Name = "beforeBackupEventsControl";
            this.beforeBackupEventsControl.Size = new System.Drawing.Size(291, 390);
            this.beforeBackupEventsControl.TabIndex = 1;
            // 
            // _afterBackupTasksChainToExecuteUserControl
            // 
            this._afterBackupTasksChainToExecuteUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._afterBackupTasksChainToExecuteUserControl.Location = new System.Drawing.Point(0, 0);
            this._afterBackupTasksChainToExecuteUserControl.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this._afterBackupTasksChainToExecuteUserControl.Name = "_afterBackupTasksChainToExecuteUserControl";
            this._afterBackupTasksChainToExecuteUserControl.Size = new System.Drawing.Size(278, 390);
            this._afterBackupTasksChainToExecuteUserControl.TabIndex = 2;
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.Location = new System.Drawing.Point(0, 0);
            this._splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this.beforeBackupEventsControl);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._afterBackupTasksChainToExecuteUserControl);
            this._splitContainer.Size = new System.Drawing.Size(573, 390);
            this._splitContainer.SplitterDistance = 291;
            this._splitContainer.TabIndex = 3;
            // 
            // OtherTaskOptionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._splitContainer);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MinimumSize = new System.Drawing.Size(573, 390);
            this.Name = "OtherTaskOptionsUserControl";
            this.Size = new System.Drawing.Size(573, 390);
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		private BUtil.Configurator.Controls.ExecuteProgramTaskInfoListControl _afterBackupTasksChainToExecuteUserControl;
        private BUtil.Configurator.Controls.ExecuteProgramTaskInfoListControl beforeBackupEventsControl;
        private System.Windows.Forms.SplitContainer _splitContainer;
    }
}
