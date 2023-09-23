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
            _chooseTaskLabel = new System.Windows.Forms.Label();
            _tasksComboBox = new System.Windows.Forms.ComboBox();
            _cancelButton = new System.Windows.Forms.Button();
            _okButton = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // _chooseTaskLabel
            // 
            _chooseTaskLabel.AutoSize = true;
            _chooseTaskLabel.Location = new System.Drawing.Point(20, 17);
            _chooseTaskLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            _chooseTaskLabel.Name = "_chooseTaskLabel";
            _chooseTaskLabel.Size = new System.Drawing.Size(274, 25);
            _chooseTaskLabel.TabIndex = 0;
            _chooseTaskLabel.Text = "Choose the task you want to run:";
            // 
            // _tasksComboBox
            // 
            _tasksComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _tasksComboBox.FormattingEnabled = true;
            _tasksComboBox.Location = new System.Drawing.Point(20, 48);
            _tasksComboBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            _tasksComboBox.Name = "_tasksComboBox";
            _tasksComboBox.Size = new System.Drawing.Size(693, 33);
            _tasksComboBox.TabIndex = 0;
            // 
            // _cancelButton
            // 
            _cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            _cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            _cancelButton.Location = new System.Drawing.Point(590, 115);
            _cancelButton.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            _cancelButton.Name = "_cancelButton";
            _cancelButton.Size = new System.Drawing.Size(126, 45);
            _cancelButton.TabIndex = 2;
            _cancelButton.Text = "Cancel";
            _cancelButton.UseVisualStyleBackColor = true;
            // 
            // _okButton
            // 
            _okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            _okButton.Location = new System.Drawing.Point(454, 115);
            _okButton.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            _okButton.Name = "_okButton";
            _okButton.Size = new System.Drawing.Size(126, 45);
            _okButton.TabIndex = 1;
            _okButton.Text = "OK";
            _okButton.UseVisualStyleBackColor = true;
            _okButton.Click += OnOkButtonClick;
            // 
            // SelectTaskForm
            // 
            AcceptButton = _okButton;
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = _cancelButton;
            ClientSize = new System.Drawing.Size(734, 183);
            Controls.Add(_okButton);
            Controls.Add(_cancelButton);
            Controls.Add(_tasksComboBox);
            Controls.Add(_chooseTaskLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SelectTaskForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Wellcome to Backup Master!!!";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _chooseTaskLabel;
        private System.Windows.Forms.ComboBox _tasksComboBox;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _okButton;
    }
}