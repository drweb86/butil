
using BUtil.Configurator.Configurator;
using BUtil.Core.Localization;

namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    public partial class TaskNameUserControl : Core.PL.BackUserControl
    {
        public TaskNameUserControl(string taskInfoHelp = null)
        {
            InitializeComponent();

            _nameLabel.Text = Resources.Name_Title;
            _backupModelLabel.Text = taskInfoHelp ?? string.Empty;
        }

        public string TaskName
        {
            get => _titleTextBox.Text;
            set => _titleTextBox.Text = value;
        }

        public override bool ValidateUi()
        {
            var name = _titleTextBox.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.ShowErrorBox(Resources.Name_Field_Validation);
                return false;
            }

            return true;
        }

        private void OnNameChange(object sender, System.EventArgs e)
        {
            var trimmedText = TaskNameStringHelper.TrimIllegalChars(_titleTextBox.Text);
            if (trimmedText != _titleTextBox.Text)
            {
                _titleTextBox.Text = trimmedText;
            }
        }
    }
}
