using BUtil.Configurator.Common;

namespace BUtil.Configurator.Controls
{
    partial class TaskNavigationControl
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
            if (disposing)
            {
                if (components != null)
                {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskNavigationControl));
            itemsForBackupButton = new System.Windows.Forms.Button();
            storagesButton = new System.Windows.Forms.Button();
            schedulerButton = new System.Windows.Forms.Button();
            encryptionButton = new System.Windows.Forms.Button();
            _tableLayoutPanel = new TransparentTableLayoutPanel();
            _nameButton = new System.Windows.Forms.Button();
            _tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // itemsForBackupButton
            // 
            itemsForBackupButton.BackColor = System.Drawing.SystemColors.Control;
            itemsForBackupButton.Dock = System.Windows.Forms.DockStyle.Fill;
            itemsForBackupButton.FlatAppearance.BorderSize = 0;
            itemsForBackupButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            itemsForBackupButton.Image = Icons.SourceItems48x48;
            itemsForBackupButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            itemsForBackupButton.Location = new System.Drawing.Point(4, 83);
            itemsForBackupButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            itemsForBackupButton.Name = "itemsForBackupButton";
            itemsForBackupButton.Size = new System.Drawing.Size(167, 74);
            itemsForBackupButton.TabIndex = 0;
            itemsForBackupButton.Text = "What ?";
            itemsForBackupButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            itemsForBackupButton.UseVisualStyleBackColor = false;
            itemsForBackupButton.Click += ItemsForBackupButtonClick;
            // 
            // storagesButton
            // 
            storagesButton.BackColor = System.Drawing.SystemColors.Control;
            storagesButton.Dock = System.Windows.Forms.DockStyle.Fill;
            storagesButton.FlatAppearance.BorderSize = 0;
            storagesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            storagesButton.Image = Icons.Storages48x48;
            storagesButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            storagesButton.Location = new System.Drawing.Point(4, 243);
            storagesButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            storagesButton.Name = "storagesButton";
            storagesButton.Size = new System.Drawing.Size(167, 74);
            storagesButton.TabIndex = 1;
            storagesButton.Text = "Where ?";
            storagesButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            storagesButton.UseVisualStyleBackColor = false;
            storagesButton.Click += StoragesButtonClick;
            // 
            // schedulerButton
            // 
            schedulerButton.BackColor = System.Drawing.SystemColors.Control;
            schedulerButton.Dock = System.Windows.Forms.DockStyle.Fill;
            schedulerButton.FlatAppearance.BorderSize = 0;
            schedulerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            schedulerButton.Image = Icons.Schedule48x48;
            schedulerButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            schedulerButton.Location = new System.Drawing.Point(4, 163);
            schedulerButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            schedulerButton.Name = "schedulerButton";
            schedulerButton.Size = new System.Drawing.Size(167, 74);
            schedulerButton.TabIndex = 2;
            schedulerButton.Text = "When ?";
            schedulerButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            schedulerButton.UseVisualStyleBackColor = false;
            schedulerButton.Click += SchedulerButtonClick;
            // 
            // encryptionButton
            // 
            encryptionButton.BackColor = System.Drawing.SystemColors.Control;
            encryptionButton.Dock = System.Windows.Forms.DockStyle.Fill;
            encryptionButton.FlatAppearance.BorderSize = 0;
            encryptionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            encryptionButton.Image = Icons.Password48x48;
            encryptionButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            encryptionButton.Location = new System.Drawing.Point(4, 323);
            encryptionButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            encryptionButton.Name = "encryptionButton";
            encryptionButton.Size = new System.Drawing.Size(167, 74);
            encryptionButton.TabIndex = 3;
            encryptionButton.Text = "Encryption";
            encryptionButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            encryptionButton.UseVisualStyleBackColor = false;
            encryptionButton.Click += EncryptionButtonClick;
            // 
            // _tableLayoutPanel
            // 
            _tableLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            _tableLayoutPanel.ColumnCount = 1;
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tableLayoutPanel.Controls.Add(encryptionButton, 0, 4);
            _tableLayoutPanel.Controls.Add(storagesButton, 0, 3);
            _tableLayoutPanel.Controls.Add(schedulerButton, 0, 2);
            _tableLayoutPanel.Controls.Add(itemsForBackupButton, 0, 1);
            _tableLayoutPanel.Controls.Add(_nameButton, 0, 0);
            _tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _tableLayoutPanel.Name = "_tableLayoutPanel";
            _tableLayoutPanel.RowCount = 6;
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.Size = new System.Drawing.Size(175, 427);
            _tableLayoutPanel.TabIndex = 6;
            // 
            // _nameButton
            // 
            _nameButton.BackColor = System.Drawing.SystemColors.Control;
            _nameButton.Dock = System.Windows.Forms.DockStyle.Fill;
            _nameButton.FlatAppearance.BorderSize = 0;
            _nameButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _nameButton.Image = (System.Drawing.Image)resources.GetObject("_nameButton.Image");
            _nameButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            _nameButton.Location = new System.Drawing.Point(4, 3);
            _nameButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _nameButton.Name = "_nameButton";
            _nameButton.Size = new System.Drawing.Size(167, 74);
            _nameButton.TabIndex = 6;
            _nameButton.Text = "Name";
            _nameButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            _nameButton.UseVisualStyleBackColor = false;
            _nameButton.Click += OnNameButtonClick;
            // 
            // TaskNavigationControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Window;
            BackgroundImage = Icons.BackgroundImage;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            Controls.Add(_tableLayoutPanel);
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            MinimumSize = new System.Drawing.Size(161, 357);
            Name = "TaskNavigationControl";
            Size = new System.Drawing.Size(175, 427);
            Load += leftPanelUserControlLoad;
            _tableLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Button encryptionButton;
        private System.Windows.Forms.Button schedulerButton;
        private System.Windows.Forms.Button storagesButton;
        private System.Windows.Forms.Button itemsForBackupButton;
        private TransparentTableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.Button _nameButton;
    }
}
