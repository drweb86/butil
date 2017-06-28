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
            this.SuspendLayout();
            // 
            // _aboutProgramUserControl
            // 
            this._aboutProgramUserControl.BackColor = System.Drawing.SystemColors.Window;
            this._aboutProgramUserControl.HelpLabel = null;
            this._aboutProgramUserControl.Location = new System.Drawing.Point(12, 12);
            this._aboutProgramUserControl.MinimumSize = new System.Drawing.Size(506, 286);
            this._aboutProgramUserControl.Name = "_aboutProgramUserControl";
            this._aboutProgramUserControl.Size = new System.Drawing.Size(506, 286);
            this._aboutProgramUserControl.TabIndex = 0;
            // 
            // _okButton
            // 
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._okButton.Location = new System.Drawing.Point(442, 306);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 23);
            this._okButton.TabIndex = 1;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._okButton;
            this.ClientSize = new System.Drawing.Size(529, 341);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._aboutProgramUserControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.ResumeLayout(false);

        }

        #endregion

        private BUtil.Configurator.Controls.AboutProgramUserControl _aboutProgramUserControl;
        private System.Windows.Forms.Button _okButton;
    }
}