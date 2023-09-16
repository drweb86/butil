namespace BUtil.Configurator.BackupUiMaster.Forms
{
    partial class SelectTaskForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectTaskForm));
            this._chooseTaskLabel = new System.Windows.Forms.Label();
            this._tasksComboBox = new System.Windows.Forms.ComboBox();
            this._cancelButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _chooseTaskLabel
            // 
            this._chooseTaskLabel.AutoSize = true;
            this._chooseTaskLabel.Location = new System.Drawing.Point(14, 10);
            this._chooseTaskLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._chooseTaskLabel.Name = "_chooseTaskLabel";
            this._chooseTaskLabel.Size = new System.Drawing.Size(181, 15);
            this._chooseTaskLabel.TabIndex = 0;
            this._chooseTaskLabel.Text = "Choose the task you want to run:";
            // 
            // _tasksComboBox
            // 
            this._tasksComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._tasksComboBox.FormattingEnabled = true;
            this._tasksComboBox.Location = new System.Drawing.Point(14, 29);
            this._tasksComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._tasksComboBox.Name = "_tasksComboBox";
            this._tasksComboBox.Size = new System.Drawing.Size(486, 23);
            this._tasksComboBox.TabIndex = 0;
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(413, 69);
            this._cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(88, 27);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _okButton
            // 
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okButton.Location = new System.Drawing.Point(318, 69);
            this._okButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(88, 27);
            this._okButton.TabIndex = 1;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this.OnOkButtonClick);
            // 
            // SelectTaskToRunForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(514, 110);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._tasksComboBox);
            this.Controls.Add(this._chooseTaskLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectTaskToRunForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wellcome to Backup Master!!!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _chooseTaskLabel;
        private System.Windows.Forms.ComboBox _tasksComboBox;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _okButton;
    }
}