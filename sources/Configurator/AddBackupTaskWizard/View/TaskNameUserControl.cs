using BUtil.Configurator.Configurator;
using BUtil.Configurator.Localization;
using BUtil.Core.Options;
using BUtil.Core.PL;

namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    public partial class TaskNameUserControl : Core.PL.BackUserControl
    {
        private BackupTask _task;

        public TaskNameUserControl()
        {
            InitializeComponent();
        }

        public override void ApplyLocalization()
        {
            _nameLabel.Text = Resources.Name;
        }

        public override void SetOptionsToUi(object settings)
        {
            _task = (BackupTask) settings;

            _titleTextBox.Text = _task.Name;
        }

        public override bool ValidateUi()
        {
            var name = _titleTextBox.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.ShowErrorBox(BUtil.Configurator.Localization.Resources.NameIsEmpty);
                return false;
            }

            return true;
        }

        public override void GetOptionsFromUi()
        {
            _task.Name = _titleTextBox.Text; 
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
