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
            this._titleLabel = new System.Windows.Forms.Label();
            this._titleTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _titleLabel
            // 
            this._titleLabel.AutoSize = true;
            this._titleLabel.Location = new System.Drawing.Point(24, 16);
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Size = new System.Drawing.Size(30, 13);
            this._titleLabel.TabIndex = 0;
            this._titleLabel.Text = "Title:";
            // 
            // _titleTextBox
            // 
            this._titleTextBox.Location = new System.Drawing.Point(93, 13);
            this._titleTextBox.Name = "_titleTextBox";
            this._titleTextBox.Size = new System.Drawing.Size(228, 20);
            this._titleTextBox.TabIndex = 1;
            // 
            // TaskNameUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._titleTextBox);
            this.Controls.Add(this._titleLabel);
            this.Name = "TaskNameUserControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _titleLabel;
        private System.Windows.Forms.TextBox _titleTextBox;
    }
}
