using System;
using System.Windows.Forms;
using BUtil.Configurator.AddBackupTaskWizard.Controller;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    public partial class AddBackupTaskWizardForm : Form
    {
        #region Fields

        readonly AddBackupTaskWizardController _controller;

        #endregion

        #region Constructors

        internal AddBackupTaskWizardForm(AddBackupTaskWizardController controller)
        {
            _controller = controller;

            InitializeComponent();

            _controller.AddBackupTaskWizardView.Configure();
            _controller.AddBackupTaskWizardView.ApplyLocalization();
            ApplyLocals();

            RefreshActivePage();
        }

        #endregion

        #region Private Methods

        private void ApplyLocals()
        {
            Text = Resources.NewBackupTaskWizard;
            _nextButton.Text = Resources.Next;
            _previousButton.Text = Resources.Back;
            _cancelButton.Text = Resources.Cancel;
        }

        private void NextPageRequest(object sender, EventArgs e)
        {
            if (_controller.AddBackupTaskWizardView.CanGoNextPage)
            {
                _controller.AddBackupTaskWizardView.GoNext();
                RefreshActivePage();
            }
            else
            {
                _controller.AddBackupTaskWizardView.GetOptions();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void PreviousPageRequest(object sender, EventArgs e)
        {
            if (_controller.AddBackupTaskWizardView.CanGoPreviousPage)
            {
                _controller.AddBackupTaskWizardView.GoPrevious();
            }

            RefreshActivePage();
        }

        private void RefreshActivePage()
        {
            _nextButton.Text = _controller.AddBackupTaskWizardView.CanGoNextPage ? 
                Resources.Next : Resources.Finish;
            _previousButton.Enabled = _controller.AddBackupTaskWizardView.CanGoPreviousPage;

            _titleLabel.Text = _controller.AddBackupTaskWizardView.CurrentPage.Title;
            _descriptionLabel.Text = _controller.AddBackupTaskWizardView.CurrentPage.Description;
            _panel.BackgroundImageLayout = ImageLayout.None;
            _panel.BackgroundImage = _controller.AddBackupTaskWizardView.CurrentPage.Image;
            _containerPanel.Controls.Clear();
            
            var control = _controller.AddBackupTaskWizardView.CurrentPage.ControlToShow;

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

        #endregion
    }
}
