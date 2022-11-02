
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
            this.howButton = new System.Windows.Forms.Button();
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // itemsForBackupButton
            // 
            this.itemsForBackupButton.BackColor = System.Drawing.SystemColors.Control;
            this.itemsForBackupButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemsForBackupButton.FlatAppearance.BorderSize = 0;
            this.itemsForBackupButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.itemsForBackupButton.Image = global::BUtil.Configurator.Icons.SourceItems48x48;
            this.itemsForBackupButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.itemsForBackupButton.Location = new System.Drawing.Point(4, 3);
            this.itemsForBackupButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.itemsForBackupButton.Name = "itemsForBackupButton";
            this.itemsForBackupButton.Size = new System.Drawing.Size(167, 51);
            this.itemsForBackupButton.TabIndex = 0;
            this.itemsForBackupButton.Text = "What ?";
            this.itemsForBackupButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.itemsForBackupButton.UseVisualStyleBackColor = false;
            this.itemsForBackupButton.Click += new System.EventHandler(this.itemsForBackupButtonClick);
            // 
            // storagesButton
            // 
            this.storagesButton.BackColor = System.Drawing.SystemColors.Control;
            this.storagesButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.storagesButton.FlatAppearance.BorderSize = 0;
            this.storagesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.storagesButton.Image = global::BUtil.Configurator.Icons.Storages48x48;
            this.storagesButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.storagesButton.Location = new System.Drawing.Point(4, 117);
            this.storagesButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.storagesButton.Name = "storagesButton";
            this.storagesButton.Size = new System.Drawing.Size(167, 51);
            this.storagesButton.TabIndex = 1;
            this.storagesButton.Text = "Where ?";
            this.storagesButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.storagesButton.UseVisualStyleBackColor = false;
            this.storagesButton.Click += new System.EventHandler(this.storagesButtonClick);
            // 
            // schedulerButton
            // 
            this.schedulerButton.BackColor = System.Drawing.SystemColors.Control;
            this.schedulerButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schedulerButton.FlatAppearance.BorderSize = 0;
            this.schedulerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.schedulerButton.Image = global::BUtil.Configurator.Icons.Schedule48x48;
            this.schedulerButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.schedulerButton.Location = new System.Drawing.Point(4, 60);
            this.schedulerButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.schedulerButton.Name = "schedulerButton";
            this.schedulerButton.Size = new System.Drawing.Size(167, 51);
            this.schedulerButton.TabIndex = 2;
            this.schedulerButton.Text = "When ?";
            this.schedulerButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.schedulerButton.UseVisualStyleBackColor = false;
            this.schedulerButton.Click += new System.EventHandler(this.schedulerButtonClick);
            // 
            // encryptionButton
            // 
            this.encryptionButton.BackColor = System.Drawing.SystemColors.Control;
            this.encryptionButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.encryptionButton.FlatAppearance.BorderSize = 0;
            this.encryptionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encryptionButton.Image = global::BUtil.Configurator.Icons.Password48x48;
            this.encryptionButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.encryptionButton.Location = new System.Drawing.Point(4, 231);
            this.encryptionButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.encryptionButton.Name = "encryptionButton";
            this.encryptionButton.Size = new System.Drawing.Size(167, 51);
            this.encryptionButton.TabIndex = 3;
            this.encryptionButton.Text = "Encryption";
            this.encryptionButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.encryptionButton.UseVisualStyleBackColor = false;
            this.encryptionButton.Click += new System.EventHandler(this.encryptionButtonClick);
            // 
            // otherOptionsButton
            // 
            this.otherOptionsButton.BackColor = System.Drawing.SystemColors.Control;
            this.otherOptionsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.otherOptionsButton.FlatAppearance.BorderSize = 0;
            this.otherOptionsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.otherOptionsButton.Image = global::BUtil.Configurator.Icons.OtherSettings48x48;
            this.otherOptionsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.otherOptionsButton.Location = new System.Drawing.Point(4, 288);
            this.otherOptionsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.otherOptionsButton.Name = "otherOptionsButton";
            this.otherOptionsButton.Size = new System.Drawing.Size(167, 51);
            this.otherOptionsButton.TabIndex = 4;
            this.otherOptionsButton.Text = "Other options";
            this.otherOptionsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.otherOptionsButton.UseVisualStyleBackColor = false;
            this.otherOptionsButton.Click += new System.EventHandler(this.otherOptionsButtonClick);
            // 
            // howButton
            // 
            this.howButton.BackColor = System.Drawing.SystemColors.Control;
            this.howButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.howButton.FlatAppearance.BorderSize = 0;
            this.howButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.howButton.Image = global::BUtil.Configurator.Icons.SourceItems48x48;
            this.howButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.howButton.Location = new System.Drawing.Point(4, 174);
            this.howButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.howButton.Name = "howButton";
            this.howButton.Size = new System.Drawing.Size(167, 51);
            this.howButton.TabIndex = 5;
            this.howButton.Text = "How ?";
            this.howButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.howButton.UseVisualStyleBackColor = false;
            this.howButton.Click += new System.EventHandler(this.HowButtonClick);
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this._tableLayoutPanel.ColumnCount = 1;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.Controls.Add(this.otherOptionsButton, 0, 5);
            this._tableLayoutPanel.Controls.Add(this.encryptionButton, 0, 4);
            this._tableLayoutPanel.Controls.Add(this.storagesButton, 0, 2);
            this._tableLayoutPanel.Controls.Add(this.itemsForBackupButton, 0, 0);
            this._tableLayoutPanel.Controls.Add(this.schedulerButton, 0, 1);
            this._tableLayoutPanel.Controls.Add(this.howButton, 0, 3);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 7;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this._tableLayoutPanel.Size = new System.Drawing.Size(175, 357);
            this._tableLayoutPanel.TabIndex = 6;
            // 
            // EditTasksLeftPanelUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImage = global::BUtil.Configurator.Icons.BackgroundImage;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this._tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MinimumSize = new System.Drawing.Size(161, 357);
            this.Name = "EditTasksLeftPanelUserControl";
            this.Size = new System.Drawing.Size(175, 357);
            this.Load += new System.EventHandler(this.leftPanelUserControlLoad);
            this._tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		private System.Windows.Forms.Button otherOptionsButton;
		private System.Windows.Forms.Button encryptionButton;
		private System.Windows.Forms.Button schedulerButton;
		private System.Windows.Forms.Button storagesButton;
        private System.Windows.Forms.Button itemsForBackupButton;
        private System.Windows.Forms.Button howButton;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
    }
}
