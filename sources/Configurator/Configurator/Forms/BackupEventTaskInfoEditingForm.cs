using System;
using System.Drawing;
using System.Windows.Forms;
using BUtil.Configurator.Localization;
using BUtil.Core.Options;



namespace BUtil.Configurator.Forms
{
	internal sealed partial class BackupEventTaskInfoEditingForm : Form
	{
		#region Properties
		
		public ExecuteProgramTaskInfo EventTask
		{
			get { return new ExecuteProgramTaskInfo(_nameTextBox.Text, programTextBox.Text.Trim(), argumentsTextBox.Text.Trim()) ; }
		}
		
		#endregion
		
		#region Constructors
		
		public BackupEventTaskInfoEditingForm()
		{
			InitializeComponent();
			
			this.Text = Resources.NewEventTaskConfigurator;
            ApplyLocals();

            programTextBoxTextChanged(null, null);
		}

		/// <summary>
		/// The constructor of form for editing the task
		/// </summary>
		/// <param name="taskWillGoBeforeBackup">Shows when the task will go</param>
		/// <param name="taskToEdit">The task</param>
		/// <exception cref="ArgumentNullException">taskToEdit is null</exception>
		public BackupEventTaskInfoEditingForm(ExecuteProgramTaskInfo taskToEdit)
		{
			InitializeComponent();
			
			if (taskToEdit == null)
			{
				throw new ArgumentNullException("taskToEdit");
			}

			_nameTextBox.Text = taskToEdit.Name;
			programTextBox.Text = taskToEdit.Program;
			argumentsTextBox.Text = taskToEdit.Arguments;
			
			this.Text = Resources.EditingEventTaskConfigurator;
			ApplyLocals();

            programTextBoxTextChanged(null, null);
		}
		
		#endregion
		
		#region Private Methods
		
		private void ApplyLocals()
		{
            taskToRunGroupBox.Text = Resources.TaskToRun;
            programLabel.Text = Resources.Program;
            commandLineArgumentsLabel.Text = Resources.CommandLineArguments;
            cancelButton.Text = Resources.Cancel;
        }

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
