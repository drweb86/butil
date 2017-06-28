
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
			this.helpForPostBackupTasksLabel = new System.Windows.Forms.Label();
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
			this.taskToRunGroupBox.Controls.Add(this.helpForPostBackupTasksLabel);
			this.taskToRunGroupBox.Controls.Add(this.argumentsTextBox);
			this.taskToRunGroupBox.Controls.Add(this.commandLineArgumentsLabel);
			this.taskToRunGroupBox.Controls.Add(this.browseForProgramButton);
			this.taskToRunGroupBox.Controls.Add(this.programTextBox);
			this.taskToRunGroupBox.Controls.Add(this.programLabel);
			this.taskToRunGroupBox.Location = new System.Drawing.Point(12, 12);
			this.taskToRunGroupBox.Name = "taskToRunGroupBox";
			this.taskToRunGroupBox.Size = new System.Drawing.Size(421, 137);
			this.taskToRunGroupBox.TabIndex = 0;
			this.taskToRunGroupBox.TabStop = false;
			this.taskToRunGroupBox.Text = "Task to run";
			// 
			// helpForPostBackupTasksLabel
			// 
			this.helpForPostBackupTasksLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.helpForPostBackupTasksLabel.Location = new System.Drawing.Point(6, 94);
			this.helpForPostBackupTasksLabel.Name = "helpForPostBackupTasksLabel";
			this.helpForPostBackupTasksLabel.Size = new System.Drawing.Size(409, 40);
			this.helpForPostBackupTasksLabel.TabIndex = 5;
			this.helpForPostBackupTasksLabel.Text = "<Help for post backup event>";
			// 
			// argumentsTextBox
			// 
			this.argumentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.argumentsTextBox.Location = new System.Drawing.Point(6, 71);
			this.argumentsTextBox.Name = "argumentsTextBox";
			this.argumentsTextBox.Size = new System.Drawing.Size(409, 20);
			this.argumentsTextBox.TabIndex = 3;
			// 
			// commandLineArgumentsLabel
			// 
			this.commandLineArgumentsLabel.AutoSize = true;
			this.commandLineArgumentsLabel.Location = new System.Drawing.Point(6, 55);
			this.commandLineArgumentsLabel.Name = "commandLineArgumentsLabel";
			this.commandLineArgumentsLabel.Size = new System.Drawing.Size(128, 13);
			this.commandLineArgumentsLabel.TabIndex = 3;
			this.commandLineArgumentsLabel.Text = "Command line arguments:";
			// 
			// browseForProgramButton
			// 
			this.browseForProgramButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.browseForProgramButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.browseForProgramButton.Location = new System.Drawing.Point(372, 30);
			this.browseForProgramButton.Name = "browseForProgramButton";
			this.browseForProgramButton.Size = new System.Drawing.Size(43, 23);
			this.browseForProgramButton.TabIndex = 2;
			this.browseForProgramButton.Text = "...";
			this.browseForProgramButton.UseVisualStyleBackColor = true;
			this.browseForProgramButton.Click += new System.EventHandler(this.browseForProgramButtonClick);
			// 
			// programTextBox
			// 
			this.programTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.programTextBox.Location = new System.Drawing.Point(6, 32);
			this.programTextBox.Name = "programTextBox";
			this.programTextBox.Size = new System.Drawing.Size(360, 20);
			this.programTextBox.TabIndex = 1;
			this.programTextBox.TextChanged += new System.EventHandler(this.programTextBoxTextChanged);
			// 
			// programLabel
			// 
			this.programLabel.AutoSize = true;
			this.programLabel.Location = new System.Drawing.Point(6, 16);
			this.programLabel.Name = "programLabel";
			this.programLabel.Size = new System.Drawing.Size(49, 13);
			this.programLabel.TabIndex = 0;
			this.programLabel.Text = "Program:";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.Location = new System.Drawing.Point(277, 154);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 4;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButtonClick);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(358, 154);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// BackupEventTaskInfoEditingForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(445, 189);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.taskToRunGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = BUtil.Configurator.Icons.BUtilIcon;
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
		private System.Windows.Forms.Label helpForPostBackupTasksLabel;
		private System.Windows.Forms.GroupBox taskToRunGroupBox;
	}
}
