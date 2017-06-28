
namespace BUtil.Configurator.Controls
{
    partial class EditTasksLeftPanelUserControl
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
            this.itemsForBackupButton = new System.Windows.Forms.Button();
            this.storagesButton = new System.Windows.Forms.Button();
            this.schedulerButton = new System.Windows.Forms.Button();
            this.encryptionButton = new System.Windows.Forms.Button();
            this.otherOptionsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // itemsForBackupButton
            // 
            this.itemsForBackupButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.itemsForBackupButton.BackColor = System.Drawing.SystemColors.Control;
            this.itemsForBackupButton.FlatAppearance.BorderSize = 0;
            this.itemsForBackupButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.itemsForBackupButton.Image = global::BUtil.Configurator.Icons.SourceItems48x48;
            this.itemsForBackupButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.itemsForBackupButton.Location = new System.Drawing.Point(1, 4);
            this.itemsForBackupButton.Name = "itemsForBackupButton";
            this.itemsForBackupButton.Size = new System.Drawing.Size(135, 57);
            this.itemsForBackupButton.TabIndex = 0;
            this.itemsForBackupButton.Text = "What ?";
            this.itemsForBackupButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.itemsForBackupButton.UseVisualStyleBackColor = false;
            this.itemsForBackupButton.Click += new System.EventHandler(this.itemsForBackupButtonClick);
            // 
            // storagesButton
            // 
            this.storagesButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.storagesButton.BackColor = System.Drawing.SystemColors.Control;
            this.storagesButton.FlatAppearance.BorderSize = 0;
            this.storagesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.storagesButton.Image = global::BUtil.Configurator.Icons.Storages48x48;
            this.storagesButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.storagesButton.Location = new System.Drawing.Point(1, 64);
            this.storagesButton.Name = "storagesButton";
            this.storagesButton.Size = new System.Drawing.Size(135, 57);
            this.storagesButton.TabIndex = 1;
            this.storagesButton.Text = "Where ?";
            this.storagesButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.storagesButton.UseVisualStyleBackColor = false;
            this.storagesButton.Click += new System.EventHandler(this.storagesButtonClick);
            // 
            // schedulerButton
            // 
            this.schedulerButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.schedulerButton.BackColor = System.Drawing.SystemColors.Control;
            this.schedulerButton.FlatAppearance.BorderSize = 0;
            this.schedulerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.schedulerButton.Image = global::BUtil.Configurator.Icons.Schedule48x48;
            this.schedulerButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.schedulerButton.Location = new System.Drawing.Point(1, 124);
            this.schedulerButton.Name = "schedulerButton";
            this.schedulerButton.Size = new System.Drawing.Size(135, 57);
            this.schedulerButton.TabIndex = 2;
            this.schedulerButton.Text = "When ?";
            this.schedulerButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.schedulerButton.UseVisualStyleBackColor = false;
            this.schedulerButton.Click += new System.EventHandler(this.schedulerButtonClick);
            // 
            // encryptionButton
            // 
            this.encryptionButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.encryptionButton.BackColor = System.Drawing.SystemColors.Control;
            this.encryptionButton.FlatAppearance.BorderSize = 0;
            this.encryptionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encryptionButton.Image = global::BUtil.Configurator.Icons.Password48x48;
            this.encryptionButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.encryptionButton.Location = new System.Drawing.Point(1, 184);
            this.encryptionButton.Name = "encryptionButton";
            this.encryptionButton.Size = new System.Drawing.Size(135, 57);
            this.encryptionButton.TabIndex = 3;
            this.encryptionButton.Text = "Encryption";
            this.encryptionButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.encryptionButton.UseVisualStyleBackColor = false;
            this.encryptionButton.Click += new System.EventHandler(this.encryptionButtonClick);
            // 
            // otherOptionsButton
            // 
            this.otherOptionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.otherOptionsButton.BackColor = System.Drawing.SystemColors.Control;
            this.otherOptionsButton.FlatAppearance.BorderSize = 0;
            this.otherOptionsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.otherOptionsButton.Image = global::BUtil.Configurator.Icons.OtherSettings48x48;
            this.otherOptionsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.otherOptionsButton.Location = new System.Drawing.Point(1, 244);
            this.otherOptionsButton.Name = "otherOptionsButton";
            this.otherOptionsButton.Size = new System.Drawing.Size(135, 57);
            this.otherOptionsButton.TabIndex = 4;
            this.otherOptionsButton.Text = "Other options";
            this.otherOptionsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.otherOptionsButton.UseVisualStyleBackColor = false;
            this.otherOptionsButton.Click += new System.EventHandler(this.otherOptionsButtonClick);
            // 
            // EditTasksLeftPanelUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::BUtil.Configurator.Icons.BackgroundImage;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.otherOptionsButton);
            this.Controls.Add(this.encryptionButton);
            this.Controls.Add(this.schedulerButton);
            this.Controls.Add(this.storagesButton);
            this.Controls.Add(this.itemsForBackupButton);
            this.MinimumSize = new System.Drawing.Size(138, 309);
            this.Name = "EditTasksLeftPanelUserControl";
            this.Size = new System.Drawing.Size(138, 309);
            this.Load += new System.EventHandler(this.leftPanelUserControlLoad);
            this.ResumeLayout(false);

        }
		private System.Windows.Forms.Button otherOptionsButton;
		private System.Windows.Forms.Button encryptionButton;
		private System.Windows.Forms.Button schedulerButton;
		private System.Windows.Forms.Button storagesButton;
        private System.Windows.Forms.Button itemsForBackupButton;
	}
}
