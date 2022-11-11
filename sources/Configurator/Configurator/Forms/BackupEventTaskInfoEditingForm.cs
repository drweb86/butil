using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BUtil.Configurator.Configurator;
using BUtil.Configurator.Localization;
using BUtil.Core.Options;

namespace BUtil.Configurator.Forms
{
    internal sealed partial class ExecuteProgramTaskInfoForm : Form
	{
		private readonly IEnumerable<string> _forbiddenNames;

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
		
		public ExecuteProgramTaskInfoForm(ExecuteProgramTaskInfo task = null, IEnumerable<string> forbiddenNames = null)
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
			this._forbiddenNames = forbiddenNames;
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
			if (string.IsNullOrWhiteSpace(_nameTextBox.Text))
			{
				Messages.ShowErrorBox(Resources.NameIsEmpty);
				return;
			}

			if (_forbiddenNames.Any(x => x == _nameTextBox.Text))
			{
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.ThisNameIsAlreadyTakenTryAnotherOne);
                return;
            }

			if (!File.Exists(programTextBox.Text))
			{
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.ProgramDoesNotExist);
                return;
            }

            if (!Directory.Exists(_workingDirectoryTextBox.Text))
            {
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.WorkingDirectoryDoesNotExist);
                return;
            }

            this.DialogResult = DialogResult.OK;
		}

		private void OnNameChanged(object sender, EventArgs e)
		{
            var trimmedText = TaskNameStringHelper.TrimIllegalChars(_nameTextBox.Text);
            if (trimmedText != _nameTextBox.Text)
            {
                _nameTextBox.Text = trimmedText;
            }
        }
	}
}
