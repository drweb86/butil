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
            this._nameLabel = new System.Windows.Forms.Label();
            this._titleTextBox = new System.Windows.Forms.TextBox();
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _nameLabel
            // 
            this._nameLabel.AutoSize = true;
            this._nameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._nameLabel.Location = new System.Drawing.Point(20, 16);
            this._nameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new System.Drawing.Size(32, 29);
            this._nameLabel.TabIndex = 0;
            this._nameLabel.Text = "Title:";
            this._nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _titleTextBox
            // 
            this._titleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._titleTextBox.Location = new System.Drawing.Point(60, 19);
            this._titleTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._titleTextBox.Name = "_titleTextBox";
            this._titleTextBox.Size = new System.Drawing.Size(518, 23);
            this._titleTextBox.TabIndex = 1;
            this._titleTextBox.TextChanged += new System.EventHandler(this.OnNameChange);
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableLayoutPanel.ColumnCount = 2;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.Controls.Add(this._nameLabel, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._titleTextBox, 1, 0);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.Padding = new System.Windows.Forms.Padding(16, 16, 16, 0);
            this._tableLayoutPanel.RowCount = 2;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.Size = new System.Drawing.Size(598, 383);
            this._tableLayoutPanel.TabIndex = 2;
            // 
            // TaskNameUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "TaskNameUserControl";
            this.Size = new System.Drawing.Size(598, 383);
            this._tableLayoutPanel.ResumeLayout(false);
            this._tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _nameLabel;
        private System.Windows.Forms.TextBox _titleTextBox;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
    }
}
