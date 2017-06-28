using System;
using System.Drawing;
using System.Windows.Forms;
using BUtil.Core.Options;

using BULocalization;

namespace BUtil.Configurator.Forms
{
	/// <summary>
	/// This is a form where we can edit or create or edit backup task program that comes before or after backup
	/// </summary>
	internal sealed partial class BackupEventTaskInfoEditingForm : Form
	{
		#region Properties
		
		/// <summary>
		/// Returns the task instance
		/// </summary>
		public BackupEventTaskInfo EventTask
		{
			get { return new BackupEventTaskInfo(programTextBox.Text.Trim(), argumentsTextBox.Text.Trim()) ; }
		}
		
		#endregion
		
		#region Constructors
		
		/// <summary>
		/// The constructor of form for adding the new task
		/// </summary>
		/// <param name="taskWillGoBeforeBackup">Shows when the task will go</param>
		public BackupEventTaskInfoEditingForm(bool taskWillGoBeforeBackup)
		{
			InitializeComponent();
			
			helpForPostBackupTasksLabel.Visible = !taskWillGoBeforeBackup;
			
			this.Text = Translation.Current[594];
			helpForPostBackupTasksLabel.Text = Translation.Current[596];
			taskToRunGroupBox.Text = Translation.Current[597];
			programLabel.Text = Translation.Current[598];
			commandLineArgumentsLabel.Text = Translation.Current[599];
			cancelButton.Text = Translation.Current[600];
			
			programTextBoxTextChanged(null, null);
		}

		/// <summary>
		/// The constructor of form for editing the task
		/// </summary>
		/// <param name="taskWillGoBeforeBackup">Shows when the task will go</param>
		/// <param name="taskToEdit">The task</param>
		/// <exception cref="ArgumentNullException">taskToEdit is null</exception>
		public BackupEventTaskInfoEditingForm(bool taskWillGoBeforeBackup, BackupEventTaskInfo taskToEdit)
		{
			InitializeComponent();
			
			if (taskToEdit == null)
			{
				throw new ArgumentNullException("taskToEdit");
			}
			
			programTextBox.Text = taskToEdit.Program;
			argumentsTextBox.Text = taskToEdit.Arguments;
			
			helpForPostBackupTasksLabel.Visible = !taskWillGoBeforeBackup;
			
			this.Text = Translation.Current[595];
			helpForPostBackupTasksLabel.Text = Translation.Current[596];
			taskToRunGroupBox.Text = Translation.Current[597];
			programLabel.Text = Translation.Current[598];
			commandLineArgumentsLabel.Text = Translation.Current[599];
			cancelButton.Text = Translation.Current[600];
			
			programTextBoxTextChanged(null, null);
		}
		
		#endregion
		
		#region Private Methods
		
		void programTextBoxTextChanged(object sender, EventArgs e)
		{
			okButton.Enabled = !string.IsNullOrEmpty(programTextBox.Text.Trim());
		}
		
		void browseForProgramButtonClick(object sender, EventArgs e)
		{
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				programTextBox.Text = ofd.FileName;
			}
		}
		
		void okButtonClick(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}		
		#endregion
		

	}
}
