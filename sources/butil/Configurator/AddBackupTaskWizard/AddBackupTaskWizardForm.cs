using System;
using System.Windows.Forms;
using BUtil.Configurator.Localization;
using BUtil.Core.Options;

namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    partial class CreateBackupTaskWizardForm : Form
    {
        private readonly AddBackupTaskWizardView _addBackupTaskWizardView;

        public CreateBackupTaskWizardForm()
        {
            _addBackupTaskWizardView = new AddBackupTaskWizardView();

            InitializeComponent();

            _addBackupTaskWizardView.Configure();
            _addBackupTaskWizardView.ApplyLocalization();
            ApplyLocals();

            RefreshActivePage();
        }

        public BackupTask BackupTask { get; private set; }
        public ScheduleInfo ScheduleInfo { get; private set; }

        private void ApplyLocals()
        {
            Text = Resources.NewBackupTaskWizard;
            _nextButton.Text = Resources.Next;
            _previousButton.Text = Resources.Back;
            _cancelButton.Text = Resources.Cancel;
        }

        private void NextPageRequest(object sender, EventArgs e)
        {
            if (_addBackupTaskWizardView.CanGoNextPage)
            {
                _addBackupTaskWizardView.GoNext();
                RefreshActivePage();
            }
            else
            {
                _addBackupTaskWizardView.GetOptions();
                DialogResult = DialogResult.OK;
                BackupTask = _addBackupTaskWizardView.Task;
                ScheduleInfo = _addBackupTaskWizardView.ScheduleInfo;
                Close();
            }
        }

        private void PreviousPageRequest(object sender, EventArgs e)
        {
            if (_addBackupTaskWizardView.CanGoPreviousPage)
            {
                _addBackupTaskWizardView.GoPrevious();
            }

            RefreshActivePage();
        }

        private void RefreshActivePage()
        {
            _nextButton.Text = _addBackupTaskWizardView.CanGoNextPage ? 
                Resources.Next : Resources.Finish;
            _previousButton.Enabled = _addBackupTaskWizardView.CanGoPreviousPage;

            _titleLabel.Text = _addBackupTaskWizardView.CurrentPage.Title;
            _descriptionLabel.Text = _addBackupTaskWizardView.CurrentPage.Description;
            _pictureBox.Image = _addBackupTaskWizardView.CurrentPage.Image;
            _containerPanel.Controls.Clear();
            
            var control = _addBackupTaskWizardView.CurrentPage.ControlToShow;

            if (control != null)
            {
                _containerPanel.Controls.Add(control);
                control.Dock = DockStyle.Fill;
            }
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
