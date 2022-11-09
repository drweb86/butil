using System;
using System.IO;
using System.Windows.Forms;
using BUtil.Configurator.Localization;
using BUtil.Core.Options;

namespace BUtil.Configurator.Forms
{
	internal sealed partial class ExecuteProgramTaskInfoForm : Form
	{
		public ExecuteProgramTaskInfo EventTask
		{
			get { 
				return new ExecuteProgramTaskInfo(
					_nameTextBox.Text,
					programTextBox.Text.Trim(),
					_workingDirectoryTextBox.Text,
					argumentsTextBox.Text.Trim()); 
			}
		}
		
		public ExecuteProgramTaskInfoForm(ExecuteProgramTaskInfo task = null)
		{
			InitializeComponent();
			
			this.Text = task == null ? Resources.NewEventTaskConfigurator : Resources.EditingEventTaskConfigurator;
			if (task != null)
			{
                _nameTextBox.Text = task.Name;
                _workingDirectoryTextBox.Text = task.WorkingDirectory;
                programTextBox.Text = task.Program;
                argumentsTextBox.Text = task.Arguments;
            }
            ApplyLocals();

            programTextBoxTextChanged(null, null);
		}
		
		private void ApplyLocals()
		{
			this._nameLabel.Text = Resources.Name;
            taskToRunGroupBox.Text = Resources.TaskToRun;
            programLabel.Text = Resources.Program;
            commandLineArgumentsLabel.Text = Resources.CommandLineArguments;
            cancelButton.Text = Resources.Cancel;
			_workingDirectoryLabel.Text = BUtil.Configurator.Localization.Resources.WorkingDirectory;
        }

        private void programTextBoxTextChanged(object sender, EventArgs e)
		{
			okButton.Enabled = !string.IsNullOrEmpty(programTextBox.Text.Trim());
		}

        private void browseForProgramButtonClick(object sender, EventArgs e)
		{
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				programTextBox.Text = ofd.FileName;
				_workingDirectoryTextBox.Text = Path.GetDirectoryName(ofd.FileName);
			}
		}

        private void okButtonClick(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}		
	}
}
