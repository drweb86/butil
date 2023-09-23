using System.Collections.Generic;
using System.Windows.Forms;
using BUtil.Core.Localization;
using BUtil.Core.Options;

namespace BUtil.Configurator.BackupUiMaster.Forms
{
    public partial class SelectTaskForm : Form
    {
        public SelectTaskForm(IEnumerable<string> taskNames)
        {
            InitializeComponent();

            _tasksComboBox.DataSource = taskNames;

            ApplyLocalization();
        }

        public string TaskToRun
        {
            get { return (string)_tasksComboBox.SelectedItem; }
        }

        void OnOkButtonClick(object sender, System.EventArgs e)
        {
            if (_tasksComboBox.SelectedIndex != -1)
            {
                DialogResult = DialogResult.OK;
            }
        }

        void ApplyLocalization()
        {
            Text = Resources.ApplicationName_LaunchTask;
            _cancelButton.Text = Resources.Button_Cancel;
            _chooseTaskLabel.Text = Resources.Task_Field_Choose;
        }
    }
}
