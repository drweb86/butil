using BUtil.Configurator.Configurator;
using BUtil.Configurator.Localization;
using BUtil.Core.Options;


namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    public partial class TaskNameUserControl : Core.PL.BackUserControl
    {
        #region Fields

        BackupTask _task;

        #endregion

        public TaskNameUserControl()
        {
            InitializeComponent();
        }

        public override void ApplyLocalization()
        {
            _titleLabel.Text = Resources.Title;
        }

        public override void SetOptionsToUi(object settings)
        {
            _task = (BackupTask) settings;

            _titleTextBox.Text = _task.Name;
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
