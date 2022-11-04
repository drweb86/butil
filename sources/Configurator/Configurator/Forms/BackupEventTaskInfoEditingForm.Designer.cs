
namespace BUtil.Configurator.Forms
{
	partial class BackupEventTaskInfoEditingForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
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
            this.taskToRunGroupBox = new System.Windows.Forms.GroupBox();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.argumentsTextBox = new System.Windows.Forms.TextBox();
            this.commandLineArgumentsLabel = new System.Windows.Forms.Label();
            this.browseForProgramButton = new System.Windows.Forms.Button();
            this.programTextBox = new System.Windows.Forms.TextBox();
            this.programLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.taskToRunGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // taskToRunGroupBox
            // 
            this.taskToRunGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskToRunGroupBox.Controls.Add(this._nameTextBox);
            this.taskToRunGroupBox.Controls.Add(this.label1);
            this.taskToRunGroupBox.Controls.Add(this.argumentsTextBox);
            this.taskToRunGroupBox.Controls.Add(this.commandLineArgumentsLabel);
            this.taskToRunGroupBox.Controls.Add(this.browseForProgramButton);
            this.taskToRunGroupBox.Controls.Add(this.programTextBox);
            this.taskToRunGroupBox.Controls.Add(this.programLabel);
            this.taskToRunGroupBox.Location = new System.Drawing.Point(14, 14);
            this.taskToRunGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.taskToRunGroupBox.Name = "taskToRunGroupBox";
            this.taskToRunGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.taskToRunGroupBox.Size = new System.Drawing.Size(500, 166);
            this.taskToRunGroupBox.TabIndex = 0;
            this.taskToRunGroupBox.TabStop = false;
            this.taskToRunGroupBox.Text = "Task to run";
            // 
            // _nameTextBox
            // 
            this._nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._nameTextBox.Location = new System.Drawing.Point(8, 35);
            this._nameTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._nameTextBox.Name = "_nameTextBox";
            this._nameTextBox.Size = new System.Drawing.Size(484, 23);
            this._nameTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Name:";
            // 
            // argumentsTextBox
            // 
            this.argumentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.argumentsTextBox.Location = new System.Drawing.Point(8, 125);
            this.argumentsTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.argumentsTextBox.Name = "argumentsTextBox";
            this.argumentsTextBox.Size = new System.Drawing.Size(485, 23);
            this.argumentsTextBox.TabIndex = 4;
            // 
            // commandLineArgumentsLabel
            // 
            this.commandLineArgumentsLabel.AutoSize = true;
            this.commandLineArgumentsLabel.Location = new System.Drawing.Point(8, 106);
            this.commandLineArgumentsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.commandLineArgumentsLabel.Name = "commandLineArgumentsLabel";
            this.commandLineArgumentsLabel.Size = new System.Drawing.Size(149, 15);
            this.commandLineArgumentsLabel.TabIndex = 3;
            this.commandLineArgumentsLabel.Text = "Command line arguments:";
            // 
            // browseForProgramButton
            // 
            this.browseForProgramButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseForProgramButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseForProgramButton.Location = new System.Drawing.Point(444, 78);
            this.browseForProgramButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.browseForProgramButton.Name = "browseForProgramButton";
            this.browseForProgramButton.Size = new System.Drawing.Size(50, 27);
            this.browseForProgramButton.TabIndex = 3;
            this.browseForProgramButton.Text = "...";
            this.browseForProgramButton.UseVisualStyleBackColor = true;
            this.browseForProgramButton.Click += new System.EventHandler(this.browseForProgramButtonClick);
            // 
            // programTextBox
            // 
            this.programTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.programTextBox.Location = new System.Drawing.Point(8, 80);
            this.programTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.programTextBox.Name = "programTextBox";
            this.programTextBox.Size = new System.Drawing.Size(428, 23);
            this.programTextBox.TabIndex = 2;
            this.programTextBox.TextChanged += new System.EventHandler(this.programTextBoxTextChanged);
            // 
            // programLabel
            // 
            this.programLabel.AutoSize = true;
            this.programLabel.Location = new System.Drawing.Point(8, 61);
            this.programLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.programLabel.Name = "programLabel";
            this.programLabel.Size = new System.Drawing.Size(56, 15);
            this.programLabel.TabIndex = 0;
            this.programLabel.Text = "Program:";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(332, 190);
            this.okButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(88, 27);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(427, 190);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 27);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // BackupEventTaskInfoEditingForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(528, 230);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.taskToRunGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::BUtil.Configurator.Icons.BUtilIcon;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "BackupEventTaskInfoEditingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<New|Edit> - Configurator";
            this.taskToRunGroupBox.ResumeLayout(false);
            this.taskToRunGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.OpenFileDialog ofd;
		private System.Windows.Forms.TextBox argumentsTextBox;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Label programLabel;
		private System.Windows.Forms.TextBox programTextBox;
		private System.Windows.Forms.Button browseForProgramButton;
		private System.Windows.Forms.Label commandLineArgumentsLabel;
		private System.Windows.Forms.GroupBox taskToRunGroupBox;
		private System.Windows.Forms.TextBox _nameTextBox;
		private System.Windows.Forms.Label label1;
	}
}
