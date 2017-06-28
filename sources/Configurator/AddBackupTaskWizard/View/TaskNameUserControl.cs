using BUtil.Core.Options;
using BULocalization;

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
            _titleLabel.Text = Translation.Current[633];
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
    }
}
