using System;
using System.Drawing;
using System.Windows.Forms;
using BUtil.Configurator.Localization;
using BUtil.Core.Options;



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
		public ExecuteProgramTaskInfo EventTask
		{
			get { return new ExecuteProgramTaskInfo(programTextBox.Text.Trim(), argumentsTextBox.Text.Trim()) ; }
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
			
			this.Text = Resources.NewEventTaskConfigurator;
			helpForPostBackupTasksLabel.Text = Resources.TextBackupimagefileWillBeReplacedOnAnExistingBackupImageNameForExampleOnDTempBackupButil;
			taskToRunGroupBox.Text = Resources.TaskToRun;
			programLabel.Text = Resources.Program;
			commandLineArgumentsLabel.Text = Resources.CommandLineArguments;
			cancelButton.Text = Resources.Cancel;
			
			programTextBoxTextChanged(null, null);
		}

		/// <summary>
		/// The constructor of form for editing the task
		/// </summary>
		/// <param name="taskWillGoBeforeBackup">Shows when the task will go</param>
		/// <param name="taskToEdit">The task</param>
		/// <exception cref="ArgumentNullException">taskToEdit is null</exception>
		public BackupEventTaskInfoEditingForm(bool taskWillGoBeforeBackup, ExecuteProgramTaskInfo taskToEdit)
		{
			InitializeComponent();
			
			if (taskToEdit == null)
			{
				throw new ArgumentNullException("taskToEdit");
			}
			
			programTextBox.Text = taskToEdit.Program;
			argumentsTextBox.Text = taskToEdit.Arguments;
			
			helpForPostBackupTasksLabel.Visible = !taskWillGoBeforeBackup;
			
			this.Text = Resources.EditingEventTaskConfigurator;
			helpForPostBackupTasksLabel.Text = Resources.TextBackupimagefileWillBeReplacedOnAnExistingBackupImageNameForExampleOnDTempBackupButil;
			taskToRunGroupBox.Text = Resources.TaskToRun;
			programLabel.Text = Resources.Program;
			commandLineArgumentsLabel.Text = Resources.CommandLineArguments;
			cancelButton.Text = Resources.Cancel;
			
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
