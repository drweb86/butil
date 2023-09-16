namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    partial class TaskNameUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskNameUserControl));
            _nameLabel = new System.Windows.Forms.Label();
            _titleTextBox = new System.Windows.Forms.TextBox();
            _tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _backupModelLabel = new System.Windows.Forms.Label();
            _tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _nameLabel
            // 
            _nameLabel.AutoSize = true;
            _nameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _nameLabel.Location = new System.Drawing.Point(20, 16);
            _nameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _nameLabel.Name = "_nameLabel";
            _nameLabel.Size = new System.Drawing.Size(32, 29);
            _nameLabel.TabIndex = 0;
            _nameLabel.Text = "Title:";
            _nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _titleTextBox
            // 
            _titleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _titleTextBox.Location = new System.Drawing.Point(60, 19);
            _titleTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _titleTextBox.Name = "_titleTextBox";
            _titleTextBox.Size = new System.Drawing.Size(526, 23);
            _titleTextBox.TabIndex = 1;
            _titleTextBox.TextChanged += OnNameChange;
            // 
            // _tableLayoutPanel
            // 
            _tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _tableLayoutPanel.ColumnCount = 2;
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _tableLayoutPanel.Controls.Add(_nameLabel, 0, 0);
            _tableLayoutPanel.Controls.Add(_titleTextBox, 1, 0);
            _tableLayoutPanel.Controls.Add(_backupModelLabel, 1, 1);
            _tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            _tableLayoutPanel.Name = "_tableLayoutPanel";
            _tableLayoutPanel.Padding = new System.Windows.Forms.Padding(16, 16, 16, 0);
            _tableLayoutPanel.RowCount = 2;
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.Size = new System.Drawing.Size(598, 383);
            _tableLayoutPanel.TabIndex = 2;
            // 
            // _backupModelLabel
            // 
            _backupModelLabel.AutoSize = true;
            _backupModelLabel.Location = new System.Drawing.Point(59, 45);
            _backupModelLabel.MaximumSize = new System.Drawing.Size(530, 0);
            _backupModelLabel.Name = "_backupModelLabel";
            _backupModelLabel.Size = new System.Drawing.Size(528, 105);
            _backupModelLabel.TabIndex = 4;
            _backupModelLabel.Text = resources.GetString("_backupModelLabel.Text");
            // 
            // TaskNameUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_tableLayoutPanel);
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            Name = "TaskNameUserControl";
            Size = new System.Drawing.Size(598, 383);
            _tableLayoutPanel.ResumeLayout(false);
            _tableLayoutPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _nameLabel;
        private System.Windows.Forms.TextBox _titleTextBox;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.Label _backupModelLabel;
    }
}
