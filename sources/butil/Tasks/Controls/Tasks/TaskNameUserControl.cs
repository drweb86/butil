using BUtil.Configurator.Configurator;
using BUtil.Core.Localization;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    public partial class TaskNameUserControl : Core.PL.BackUserControl
    {
        private TaskV2 _task;

        public TaskNameUserControl()
        {
            InitializeComponent();
            _backupModelLabel.Text = Resources.IncrementalBackupDescription;
        }

        public override void ApplyLocalization()
        {
            _nameLabel.Text = Resources.Name;
        }

        public override void SetOptionsToUi(object settings)
        {
            _task = (TaskV2)settings;

            _titleTextBox.Text = _task.Name;
        }

        public override bool ValidateUi()
        {
            var name = _titleTextBox.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.ShowErrorBox(BUtil.Core.Localization.Resources.NameIsEmpty);
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
