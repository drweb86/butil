namespace BUtil.Configurator.Configurator.Forms
{
    partial class AboutForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this._aboutProgramUserControl = new BUtil.Configurator.Controls.AboutProgramUserControl();
            this._okButton = new System.Windows.Forms.Button();
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _aboutProgramUserControl
            // 
            this._aboutProgramUserControl.BackColor = System.Drawing.SystemColors.Window;
            this._aboutProgramUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._aboutProgramUserControl.HelpLabel = null;
            this._aboutProgramUserControl.Location = new System.Drawing.Point(14, 11);
            this._aboutProgramUserControl.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this._aboutProgramUserControl.MinimumSize = new System.Drawing.Size(590, 330);
            this._aboutProgramUserControl.Name = "_aboutProgramUserControl";
            this._aboutProgramUserControl.Size = new System.Drawing.Size(590, 335);
            this._aboutProgramUserControl.TabIndex = 0;
            // 
            // _okButton
            // 
            this._okButton.AutoSize = true;
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._okButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._okButton.Location = new System.Drawing.Point(515, 352);
            this._okButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(88, 34);
            this._okButton.TabIndex = 1;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.ColumnCount = 1;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.Controls.Add(this._okButton, 0, 1);
            this._tableLayoutPanel.Controls.Add(this._aboutProgramUserControl, 0, 0);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Margin = new System.Windows.Forms.Padding(8);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.Padding = new System.Windows.Forms.Padding(8);
            this._tableLayoutPanel.RowCount = 2;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this._tableLayoutPanel.Size = new System.Drawing.Size(615, 397);
            this._tableLayoutPanel.TabIndex = 2;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._okButton;
            this.ClientSize = new System.Drawing.Size(615, 397);
            this.Controls.Add(this._tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(631, 436);
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this._tableLayoutPanel.ResumeLayout(false);
            this._tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BUtil.Configurator.Controls.AboutProgramUserControl _aboutProgramUserControl;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
    }
}