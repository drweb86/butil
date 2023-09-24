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
            _nameLabel.Location = new System.Drawing.Point(34, 32);
            _nameLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            _nameLabel.Name = "_nameLabel";
            _nameLabel.Size = new System.Drawing.Size(57, 47);
            _nameLabel.TabIndex = 0;
            _nameLabel.Text = "Title:";
            _nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _titleTextBox
            // 
            _titleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _titleTextBox.Location = new System.Drawing.Point(105, 38);
            _titleTextBox.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            _titleTextBox.Name = "_titleTextBox";
            _titleTextBox.Size = new System.Drawing.Size(899, 35);
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
            _tableLayoutPanel.Controls.Add(_backupModelLabel, 0, 1);
            _tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _tableLayoutPanel.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            _tableLayoutPanel.Name = "_tableLayoutPanel";
            _tableLayoutPanel.Padding = new System.Windows.Forms.Padding(27, 32, 27, 0);
            _tableLayoutPanel.RowCount = 2;
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.Size = new System.Drawing.Size(1025, 766);
            _tableLayoutPanel.TabIndex = 2;
            // 
            // _backupModelLabel
            // 
            _backupModelLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _backupModelLabel.AutoSize = true;
            _tableLayoutPanel.SetColumnSpan(_backupModelLabel, 2);
            _backupModelLabel.Location = new System.Drawing.Point(32, 79);
            _backupModelLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            _backupModelLabel.MaximumSize = new System.Drawing.Size(900, 0);
            _backupModelLabel.Name = "_backupModelLabel";
            _backupModelLabel.Size = new System.Drawing.Size(530, 687);
            _backupModelLabel.TabIndex = 4;
            _backupModelLabel.Text = "Description";
            // 
            // TaskNameUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_tableLayoutPanel);
            Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
            Name = "TaskNameUserControl";
            Size = new System.Drawing.Size(1025, 766);
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
